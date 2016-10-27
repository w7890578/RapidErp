using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;

namespace BLL
{
    public class MarerialWarehouseLogListManager
    {
        /// <summary>
        /// 审核原材料出入库(产生应付、更新库存数量、更新采购订单已交货数量 )【批量审核】(销售出库、维修出库、样品出库产生送货单)
        /// </summary>
        /// <param name="auditor">审核人</param>
        /// <param name="warehouseNumber">出入库编号集合</param>
        /// <returns></returns>
        public static string AuditorMarerialWarehouseLog(string auditor, string warehouseNumber)
        {
            string error = string.Empty;
            string results = CheckQty(warehouseNumber);
            if (!string.IsNullOrEmpty(results))
            {
                return results;
            }
            //if (!CheckInventoryQty(warehouseNumber))
            //{
            //    return "当前库存数量低，无法满足当前出库操作。";
            //}
            List<string> sqls = new List<string>();
            string type = string.Empty;//出入库类型
            string mode = string.Empty; //付款方式
            string paymentdays = string.Empty; //账期
            string CGOrderNumbers = string.Empty; //采购订单编号集合
            string shNumber = string.Empty;
            string checkTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = string.Format(@" update MarerialWarehouseLog set Auditor='{0}' , CheckTime='{1}' where WarehouseNumber in ({2}) and (CheckTime ='' or CheckTime = null)",
              auditor, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), warehouseNumber);
            sqls.Add(sql);
            string[] numbers = warehouseNumber.Split(',');
            foreach (string number in numbers) //遍历出入库单
            {
                sql = string.Format(@"select * from V_MaterialWarehouseLogDetail_Detail where 出入库编号={0} ", number);
                DataTable dt = SqlHelper.GetTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    type = dr["出入库类型"] == null ? "" : dr["出入库类型"].ToString();
                    if (type.Equals("采购入库")) //产生应付
                    {
                        paymentdays = dr["采购账期"] == null ? "" : dr["采购账期"].ToString() + "天";
                        mode = dr["付款方式"] == null ? "" : dr["付款方式"].ToString();
                        //货到付款和预付部分货款的采购订单 在此产生应付，其它从数据库计划产生
                        //货到付款【账期为0天的产生，账期为其它天数的数据库计划产生】
                        if (mode.Equals("HDFK") && paymentdays.Equals("0天"))
                        {
                            sql = string.Format(@" insert into AccountsPayable
            (OrdersNumber ,MaterialNumber,CreateTime
            ,SupplierMaterialNumber,SupplierId,Qty ,UnitPrice,SumPrice ,AccountPeriod)
            values('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},'{8}')", dr["订单编号"], dr["原材料编号"],
              DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["供应商物料编号"], dr["供应商编号"],
         dr["数量"], dr["单价"], dr["总价"], paymentdays);
                            sqls.Add(sql);
                        }
                        else if (mode.Equals("YFBF"))//预付部分货款【产生预付剩余货款】
                        {
                            sql = string.Format(@" insert into AccountsPayable (OrdersNumber ,MaterialNumber ,CreateTime ,SumPrice,SupplierId )
 select co.OrdersNumber,'','{0}',SUM ( coe .SumPrice)*CAST ((1-si.PercentageInAdvance) as decimal(18,2)) , co.SupplierId  from  CertificateOrders co
 inner join CertificateOrdersDetail coe on co.OrdersNumber=coe.OrdersNumber
 left join SupplierInfo si on si.SupplierId =co.SupplierId
  where co.OrdersNumber ='{1}'  group by co.OrdersNumber,co.SupplierId,si.PercentageInAdvance", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["订单编号"]);

                            sqls.Add(sql);
                        }
                        sql = string.Format(@"update CertificateOrdersDetail set DeliveryQty +={0}
where OrdersNumber='{1}' and MaterialNumber='{2}' and LeadTime ='{3}' and Status ='未完成'
 ", dr["数量"], dr["订单编号"], dr["原材料编号"], dr["交期"]);
                        sqls.Add(sql);
                        CGOrderNumbers += string.Format("'{0}',", dr["订单编号"]);
                    }
                    else if (type.Equals("销售出库（贸易）"))
                    {
                        sql = string.Format(@"update TradingOrderDetail set DeliveryQty ={0}
where OdersNumber ='{1}' and ProductNumber ='{2}' and RowNumber ='{3}'
 ", dr["数量"], dr["订单编号"], dr["原材料编号"], dr["销售订单行号"]);
                        sqls.Add(sql);
                    }
                }
            }
            //更新库存数量
            sql = string.Format(@"select t.方向,t.仓库ID,t.原材料编号,SUM (t.数量) as 数量 from V_MaterialWarehouseLogDetail_Detail  t
where t.出入库编号 in ({0})
group by t.方向,t.仓库ID,t.原材料编号", numbers);
            foreach (DataRow drtemps in SqlHelper.GetTable(sql).Rows)
            {
                sqls.Add(GetUpdateInventoryQtySql(drtemps["原材料编号"].ToString(), drtemps["仓库ID"].ToString(), drtemps["数量"].ToString(), drtemps["方向"].ToString()));
            }

            //更新采购订单交货状态
            sql = string.Format("update CertificateOrdersDetail set NonDeliveryQty =OrderQty-DeliveryQty");
            sqls.Add(sql);
            sql = string.Format("update CertificateOrdersDetail  set Status ='已完成'  where NonDeliveryQty<=0");
            sqls.Add(sql);
            if (!string.IsNullOrEmpty(CGOrderNumbers))
            {
                sql = string.Format(@"update CertificateOrders set OrderStatus ='已完成'
where OrdersNumber in (
 select OrdersNumber from CertificateOrdersDetail where OrdersNumber in({0})
 group by OrdersNumber having SUM(NonDeliveryQty )=0
)  and OrderStatus ='未完成'", CGOrderNumbers.TrimEnd(',')); //更新采购订单主表的状态
                sqls.Add(sql);
            }
            //更新销售订单（贸易）的交货状态
            sql = string.Format("update TradingOrderDetail set NonDeliveryQty =Quantity-DeliveryQty");
            sqls.Add(sql);
            sql = string.Format("update TradingOrderDetail  set Status ='已完成'  where NonDeliveryQty<=0");
            sqls.Add(sql);
            if (!string.IsNullOrEmpty(CGOrderNumbers))
            {
                sql = string.Format(@"update SaleOder set OrderStatus ='已完成'
where OdersNumber in (
 select OdersNumber from TradingOrderDetail where OdersNumber in({0})
 group by OdersNumber having SUM(NonDeliveryQty )=0
)  and OrderStatus ='未完成'", CGOrderNumbers.TrimEnd(',')); //更新采购订单主表的状态
                sqls.Add(sql);
            }

            //产生送货单(销售出库（贸易）、样品出库、维修出库)
            string tempSql = string.Format(@"select * from  V_MaterialWarehouseLogDetail_SaleOder where WarehouseNumber in ({0})", numbers); //临时变量
            sql = string.Format(" select distinct t.CustomerId  from ( {0} )t ", tempSql); //找出客户
            DataTable dtCustomer = SqlHelper.GetTable(sql);
            foreach (DataRow dr in dtCustomer.Rows) //按客户进行遍历
            {
                //销售出库
                sql = string.Format(" select count(*) from ({0})t where t.CustomerId='{1}' and t.Type='销售出库（贸易）' ", tempSql, dr["CustomerId"]);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    shNumber = "SH" + DateTime.Now.AddSeconds(1).ToString("yyyyMMddHHmmss");
                    sql = string.Format(@"insert into DeliveryBill (DeliveryNumber,IsConfirm,CreateTime ,Remark,CustomerId )
values('{0}','未确认','{1}','由原材料销售出库产生','{2}')", shNumber, checkTime, dr["CustomerId"]);
                    sqls.Add(sql);
                    sql = string.Format(@"insert into DeliveryNoteDetailed(DeliveryNumber ,OrdersNumber,ProductNumber ,Version ,CustomerProductNumber
,LeadTime ,RowNumber,SN ,MaterialDescription,DeliveryQty)
select '{2}',vps.DocumentNumber,vps.MaterialNumber,'0',vps.CusTomerMaterialNumber,vps.LeadTime,vps.RowNumber
,1,vps.Description,vps.Qty  from ({0}) vps  where vps.CustomerId='{1}' and
vps.Type='销售出库（贸易）'", tempSql, dr["CustomerId"], shNumber);
                    sqls.Add(sql);
                }
                //维修出库
                sql = string.Format(" select count(*) from ({0})t where t.CustomerId='{1}' and t.Type='维修出库' ", tempSql, dr["CustomerId"]);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    shNumber = "SH" + DateTime.Now.AddSeconds(2).ToString("yyyyMMddHHmmss");
                    sql = string.Format(@"insert into DeliveryBill (DeliveryNumber,IsConfirm,CreateTime ,Remark,CustomerId )
values('{0}','未确认','{1}','由原材料维修出库产生','{2}')", shNumber, checkTime, dr["CustomerId"]);
                    sqls.Add(sql);
                    sql = string.Format(@"insert into DeliveryNoteDetailed(DeliveryNumber ,OrdersNumber,ProductNumber ,Version ,CustomerProductNumber
,LeadTime ,RowNumber,SN ,MaterialDescription,DeliveryQty)
select '{2}',vps.DocumentNumber,vps.MaterialNumber,'0',vps.CustomerMaterialNumber,vps.LeadTime,vps.RowNumber
,1,vps.Description,vps.Qty  from ({0}) vps  where vps.CustomerId='{1}' and
vps.Type='维修出库'", tempSql, dr["CustomerId"], shNumber);
                    sqls.Add(sql);
                }
                //样品出库
                sql = string.Format(" select count(*) from ({0})t where t.CustomerId='{1}' and t.Type='样品出库' ", tempSql, dr["CustomerId"]);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    shNumber = "SH" + DateTime.Now.AddSeconds(3).ToString("yyyyMMddHHmmss");
                    sql = string.Format(@"insert into DeliveryBill (DeliveryNumber,IsConfirm,CreateTime ,Remark,CustomerId )
values('{0}','未确认','{1}','由原材料样品出库产生','{2}')", shNumber, checkTime, dr["CustomerId"]);
                    sqls.Add(sql);
                    sql = string.Format(@"insert into DeliveryNoteDetailed(DeliveryNumber ,OrdersNumber,ProductNumber ,Version ,CustomerProductNumber
,LeadTime ,RowNumber,SN ,MaterialDescription,DeliveryQty)
select '{2}',vps.DocumentNumber,vps.MaterialNumber,'0',vps.CusTomerMaterialNumber,vps.LeadTime,vps.RowNumber
,1,vps.Description,vps.Qty  from ({0}) vps  where vps.CustomerId='{1}' and
vps.Type='样品出库'", tempSql, dr["CustomerId"], shNumber);
                    sqls.Add(sql);
                }
            }
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }

        /// <summary>
        /// 原材料出入库审核【单条审核】
        /// </summary>
        /// <param name="auditor"></param>
        /// <param name="warehouseNumber"></param>
        /// <returns></returns>
        public static string AuditorMarerialWarehouseLogForWarehouseNumber(string auditor, string warehouseNumber)
        {
            string checksqls = string.Format(@" select CheckTime from MarerialWarehouseLog where WarehouseNumber='{0}'", warehouseNumber);
            if (!string.IsNullOrEmpty(SqlHelper.GetScalar(checksqls)))
            {
                return "该单已审核，请勿重复审核！";
            }

            List<string> sqls = new List<string>();
            DataTable dtTemp = null;
            string result = string.Empty;
            string sql = string.Format(@"
select COUNT(*) from MaterialWarehouseLogDetail where WarehouseNumber='{0}'", warehouseNumber);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                return "没有需要审核的记录！";
            }
            sql = string.Format("select Type from MarerialWarehouseLog where WarehouseNumber='{0}'", warehouseNumber);
            string type = SqlHelper.GetScalar(sql);
            if (type.Equals("生产出库") || type.Equals("包装出库"))
            {
                return SCCKAuditor(auditor, warehouseNumber);
            }
            if (type.Equals("销售出库（贸易）"))
            {
                return CheckMYXS(auditor, warehouseNumber);
            }
            //步骤一：检测采购入库和销售出库（贸易）的数量-订单未交数量是否大于0（大于0为异常数据）
            if (type.Equals("采购入库"))
            {
                sql = string.Format(@"
select t.DocumentNumber ,t.MaterialNumber ,t.LeadTime from (
select WarehouseNumber,DocumentNumber,MaterialNumber,LeadTime,sum(isnull(Qty,0)) qty from MaterialWarehouseLogDetail
where WarehouseNumber ='{0}'
group by
WarehouseNumber,DocumentNumber,MaterialNumber,LeadTime
)t  left join CertificateOrdersDetail cod  on t.DocumentNumber=cod.OrdersNumber
and t.MaterialNumber =cod.MaterialNumber and t.LeadTime =cod.LeadTime  where t.Qty -cod.NonDeliveryQty >0", warehouseNumber);
                dtTemp = SqlHelper.GetTable(sql);
                if (dtTemp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        result += "<br/>" + string.Format(" 订单编号为：{0} 原材料编号为：{1} 交期为{2}的记录数量大于订单未交数量！&nbsp;&nbsp;"
, dr["DocumentNumber"], dr["MaterialNumber"], dr["LeadTime"]);
                    }
                    return result;
                }
                //sqls.AddRange(FinancialManager.GetCSYF(warehouseNumber));
            }
            else if (type.Equals("销售出库（贸易）"))
            {
                sql = string.Format(@"select mwld.DocumentNumber ,mwld.MaterialNumber ,mwld.LeadTime
from MaterialWarehouseLogDetail mwld inner join MarerialWarehouseLog mwl on mwl.WarehouseNumber =mwld.WarehouseNumber
left join TradingOrderDetail cod  on mwld.DocumentNumber=cod.OdersNumber
and mwld.MaterialNumber =cod.ProductNumber and mwld.LeadTime =cod.Delivery  and mwld.RowNumber =cod.RowNumber
where mwld.Qty -cod.NonDeliveryQty >0
and mwl.WarehouseNumber ='{0}'", warehouseNumber);
                dtTemp = SqlHelper.GetTable(sql);
                if (dtTemp.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        result += string.Format(" 订单编号为：{0} 原材料编号为：{1} 交期为{2}的记录的数量大于订单未交数量！&nbsp;&nbsp;"
, dr["DocumentNumber"], dr["MaterialNumber"], dr["LeadTime"]);
                    }
                    return result;
                }

                //纠错 仅限于销售出库（贸易）
                string tempsql1 = string.Format(@"
DELETE MaterialWarehouseLogDetail
WHERE  WarehouseNumber = '{0}'
       AND Qty < 0;
", warehouseNumber);
                SqlHelper.ExecuteSql(tempsql1);

                //更新订单完成数量
                sqls.Add(GetUpdateQtySql(warehouseNumber));

                //生成送货单
                List<string> sqlsTemp = GetSqlForShSHD(warehouseNumber);
                if (sqlsTemp.Count > 0)
                {
                    sqls.AddRange(sqlsTemp);
                }
            }
            //步骤二：检测库存数量是否满足出库

            if (type.Equals("销售出库（贸易）"))
            {
                string tempchecksql = string.Format(@" select COUNT (*) from (
 select t.MaterialNumber,t.WarehouseName, (ISNULL ( ms.StockQty,0)-t.Qty) as 差 from (
 select mwld.MaterialNumber ,sum( mwld.Qty ) as Qty,mwl.WarehouseName
  from MaterialWarehouseLogDetail mwld
  inner join MarerialWarehouseLog mwl on mwld.WarehouseNumber=mwl.WarehouseNumber
  where mwl.ChangeDirection ='出库'  and mwl.WarehouseNumber in ('{0}') and mwld.Qty>0
  group by mwld.MaterialNumber,mwl.WarehouseName ) t left join MaterialStock ms on t.MaterialNumber=ms.MaterialNumber  ) t where 差<0", warehouseNumber);
                if (!SqlHelper.GetScalar(tempchecksql).Equals("0"))
                {
                    return "当前库存数量低，无法满足当前出库操作。";
                }
                //return SqlHelper.GetScalar(sql).Equals("0") ? true : false;
            }
            else
            {
                //if (!CheckInventoryQty("'" + warehouseNumber + "'"))
                //{
                //    return "当前库存数量低，无法满足当前出库操作。";
                //}
            }

            //步骤三：同步更新库存数量 并产生应付 同步更新采购入库的已交货数量
            string paymentdays = string.Empty;
            string mode = string.Empty;
            string CGOrderNumbers = string.Empty;
            string error = string.Empty;

            sql = string.Format(@"select * from V_MaterialWarehouseLogDetail_Detail where 出入库编号='{0}' ", warehouseNumber);
            dtTemp = SqlHelper.GetTable(sql);
            foreach (DataRow dr in dtTemp.Rows)
            {
                if (type.Equals("采购入库")) //产生应付
                {
                    sql = string.Format(@"update CertificateOrdersDetail set DeliveryQty +={0}
where OrdersNumber='{1}' and MaterialNumber='{2}' and LeadTime ='{3}' and Status ='未完成'
 ", dr["数量"], dr["订单编号"], dr["原材料编号"], dr["交期"]);//同步更新采购入库的已交货数量
                    sqls.Add(sql);
                    CGOrderNumbers += string.Format("'{0}',", dr["订单编号"]);
                }
                //同步更新库存数量
                //sqls.Add(GetUpdateInventoryQtySql(dr["原材料编号"].ToString(), dr["仓库ID"].ToString(), dr["数量"].ToString(), dr["方向"].ToString()));
            }
            string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //类型为采购入库=======生成预付第二层信息
            sql = string.Format(@"insert into T_AccountsPayable_Detail(ReceiptNo,PurchaseOrderNumber ,PurchaseContractNumber,MaterialNumber ,SupplierMaterialNumber
,Description,PurchaseCount,NumberOfArrival,UnitPrice ,SumPrice ,TransportNo,LeadTime,CreateTime )
select vmld.WarehouseNumber ,vmld.DocumentNumber ,co.HTNumber,co.MaterialNumber,co.SupplierMaterialNumber,mit.Description ,co.OrderQty
,vmld.Qty,co.UnitPrice ,co.UnitPrice *vmld.Qty ,vmld.RoadTransport,vmld.LeadTime,'{1}'  from (  select mwl.Type,mwld.* from MarerialWarehouseLog mwl inner join MaterialWarehouseLogDetail mwld on mwl.WarehouseNumber =mwld.WarehouseNumber
) vmld inner join ( select co.HTNumber ,co.PaymentMode ,cod.* from CertificateOrders co inner join CertificateOrdersDetail cod on co.OrdersNumber=cod.OrdersNumber
) co on vmld.DocumentNumber =
co.OrdersNumber and vmld.MaterialNumber =co.MaterialNumber and vmld.LeadTime =co.LeadTime
inner join MarerialInfoTable mit on vmld .MaterialNumber =mit.MaterialNumber
 where vmld.WarehouseNumber ='{0}' and vmld.Type ='采购入库' ", warehouseNumber, timeNow);
            sqls.Add(sql);
            //类型为采购入库的生成应付第一层
            sql = string.Format(@"insert into T_AccountsPayable_Main(OrdersNumber,CreateTime ,CGHTNumber ,ArrivalNumber,SumPrice ,ArrivalPrice
,SupplierId,PaymentTypes,PaymentMode,DeliveryDate)
select a.OrdersNumber ,'{0}',co.HTNumber ,a.到货数量,a.订单总价,a.到货总价,si.SupplierId ,si.PayType,si.PaymentMode,CONVERT(varchar(100), GETDATE(), 23) from (
select co.OrdersNumber,SUM(vmld.Qty ) as 到货数量,SUM (vmld .Qty *co.UnitPrice ) as 到货总价,SUM( co.SumPrice) as 订单总价   from (  select mwl.Type,mwld.* from MarerialWarehouseLog mwl inner join MaterialWarehouseLogDetail mwld on mwl.WarehouseNumber =mwld.WarehouseNumber
) vmld inner join ( select co.HTNumber ,co.PaymentMode,co.SupplierId  ,cod.* from CertificateOrders co inner join CertificateOrdersDetail cod on co.OrdersNumber=cod.OrdersNumber
) co on vmld.DocumentNumber =
co.OrdersNumber and vmld.MaterialNumber =co.MaterialNumber and vmld.LeadTime =co.LeadTime
inner join MarerialInfoTable mit on vmld .MaterialNumber =mit.MaterialNumber
inner join SupplierInfo si on co.SupplierId=si.SupplierId
 where vmld.WarehouseNumber ='{1}' and vmld.Type ='采购入库' and co.PaymentMode!='YFBF' and co.PaymentMode!='YFQK'
 group by co.OrdersNumber) a  inner join CertificateOrders co on a.OrdersNumber =co.OrdersNumber
 inner join SupplierInfo si on co.SupplierId =si.SupplierId ", timeNow, warehouseNumber);
            sqls.Add(sql);

            //统一更新库存数量
            //            sql = string.Format(@"update MaterialStock set StockQty =
            //case when mwl.ChangeDirection='入库' then StockQty +a.Qty else StockQty-a.Qty end
            //,UpdateTime =CONVERT (varchar(100),GETDATE(),120) from
            //( select mwld.MaterialNumber,sum(mwld.Qty) as Qty from  MaterialWarehouseLogDetail mwld
            //  inner join MarerialWarehouseLog mwl on
            // mwld.WarehouseNumber =mwl.WarehouseNumber
            // where mwl.WarehouseNumber='{0}'
            // group by mwld.MaterialNumber) a
            // inner join MaterialStock ms on ms.MaterialNumber=a.MaterialNumber
            // where  ms.WarehouseName ='ycl'
            // ", warehouseNumber);
            sql = string.Format(@"update MaterialStock set StockQty =
case when a.ChangeDirection='入库' then StockQty +a.Qty else StockQty-a.Qty end
,UpdateTime =CONVERT (varchar(100),GETDATE(),120) from
( select mwld.MaterialNumber,mwl.ChangeDirection,sum(mwld.Qty) as Qty from  MaterialWarehouseLogDetail mwld
  inner join MarerialWarehouseLog mwl on
 mwld.WarehouseNumber =mwl.WarehouseNumber
 where mwl.WarehouseNumber='{0}'
 group by mwld.MaterialNumber,mwl.ChangeDirection) a
 inner join MaterialStock ms on ms.MaterialNumber=a.MaterialNumber
 where  ms.WarehouseName ='ycl'
 ", warehouseNumber);
            sqls.Add(sql);

            //如果是采购退料出库则反写出库单号至采购订单上
            if (type.Equals("采购退料出库"))
            {
                string tempSql = string.Format(@"
update  CertificateOrders set CCTCOrdersNumber='{0}'
where OrdersNumber in (
select distinct DocumentNumber from MaterialWarehouseLogDetail
where WarehouseNumber='{0}'
)", warehouseNumber);
                sqls.Add(tempSql);
                //warehouseNumber
            }

            //步骤四：同步更新采购入库订单明细的已交货、未交货、状态以及整条订单状态
            sql = string.Format("update CertificateOrdersDetail set NonDeliveryQty =OrderQty-DeliveryQty");
            sqls.Add(sql);
            sql = string.Format("update CertificateOrdersDetail  set Status ='已完成'  where NonDeliveryQty=0");
            sqls.Add(sql);
            if (!string.IsNullOrEmpty(CGOrderNumbers))
            {
                sql = string.Format(@"update CertificateOrders set OrderStatus ='已完成'
where OrdersNumber in (
 select OrdersNumber from CertificateOrdersDetail where OrdersNumber in({0})
 group by OrdersNumber having SUM(NonDeliveryQty )=0
)  and OrderStatus ='未完成'", CGOrderNumbers.TrimEnd(',')); //更新采购订单主表的状态
                sqls.Add(sql);
            }
            sql = string.Format(@" update MarerialWarehouseLog set Auditor='{0}' , CheckTime='{1}' where WarehouseNumber  ='{2}'", auditor, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), warehouseNumber);
            sqls.Add(sql);
            //更新瞬时库存数量
            sqls.Add(StoreroomToolManager.GetUpDateInventoryQtySql(warehouseNumber));
            //写入流水账
            sqls.Add(StoreroomToolManager.WriteMateriLSZ(warehouseNumber, auditor));

            sql = string.Format(" delete DeliveryNoteDetailed where DeliveryQty=0 ");//清除发货数量为0的送货单
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }

        /// <summary>
        /// 检测当前库存数量是否满足出库数量(原材料出入库)
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <returns></returns>
        public static bool CheckInventoryQty(string warehouseNumber)
        {
            string sql = string.Format(@" select COUNT (*) from (
 select t.MaterialNumber,t.WarehouseName, (ISNULL ( ms.StockQty,0)-t.Qty) as 差 from (
 select mwld.MaterialNumber ,sum( mwld.Qty ) as Qty,mwl.WarehouseName
  from MaterialWarehouseLogDetail mwld
  inner join MarerialWarehouseLog mwl on mwld.WarehouseNumber=mwl.WarehouseNumber
  where mwl.ChangeDirection ='出库'  and mwl.WarehouseNumber in ({0})
  group by mwld.MaterialNumber,mwl.WarehouseName ) t left join MaterialStock ms on t.MaterialNumber=ms.MaterialNumber  ) t where 差<0", warehouseNumber);
            return SqlHelper.GetScalar(sql).Equals("0") ? true : false;
        }

        /// <summary>
        /// 销售出库（贸易）审核
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <returns></returns>
        public static string CheckMYXS(string auditor, string warehouseNumber)
        {
            string sql = string.Empty;
            DataTable dtTemp = new DataTable();
            string result = string.Empty;
            List<string> sqls = new List<string>();
            string error = string.Empty;

            sql = string.Format(@"select mwld.DocumentNumber ,mwld.MaterialNumber ,mwld.LeadTime
from MaterialWarehouseLogDetail mwld inner join MarerialWarehouseLog mwl on mwl.WarehouseNumber =mwld.WarehouseNumber
left join TradingOrderDetail cod  on mwld.DocumentNumber=cod.OdersNumber
and mwld.MaterialNumber =cod.ProductNumber and mwld.LeadTime =cod.Delivery  and mwld.RowNumber =cod.RowNumber
where mwld.Qty -cod.NonDeliveryQty >0
and mwl.WarehouseNumber ='{0}'", warehouseNumber);
            dtTemp = SqlHelper.GetTable(sql);
            if (dtTemp.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTemp.Rows)
                {
                    result += string.Format(" 订单编号为：{0} 原材料编号为：{1} 交期为{2}的记录的数量大于订单未交数量！&nbsp;&nbsp;"
, dr["DocumentNumber"], dr["MaterialNumber"], dr["LeadTime"]);
                }
                return result;
            }
            //纠错 仅限于销售出库（贸易）
            string tempsql1 = string.Format(@"
DELETE MaterialWarehouseLogDetail
WHERE  WarehouseNumber = '{0}'
       AND Qty < 0;
", warehouseNumber);
            SqlHelper.ExecuteSql(tempsql1);

            //更新订单完成数量
            //sqls.Add(GetUpdateQtySql(warehouseNumber));

            //生成送货单
            List<string> sqlsTemp = GetSqlForShSHD(warehouseNumber);
            if (sqlsTemp.Count > 0)
            {
                sqls.AddRange(sqlsTemp);
            }
            sql = string.Format(@"
INSERT INTO T_LessMaterialBreakdown(WarehouseNumber ,DocumentNumber ,MaterialNumber ,RowNumber ,LeadTime ,CustomerMaterialNumber ,LibraryQty,StockQty ,LessMaterialQty ,CreateTime ,IsLessMaterial)
SELECT '',
       mwld.DocumentNumber ,
       mwld. MaterialNumber,
       '',
       '' ,
       mwld.CustomerMaterialNumber,
       mwld. Qty,
       vmq.StockQty,
       CASE
           WHEN vmq.StockQty<0 THEN 0-mwld.Qty
           ELSE vmq.StockQty -mwld.Qty
       END,
       '{1}',
       '未还料'
FROM
  (SELECT DocumentNumber,
          MaterialNumber,
          CustomerMaterialNumber,
          SUM(Qty) Qty
   FROM MaterialWarehouseLogDetail
   WHERE WarehouseNumber ='{0}'
   GROUP BY DocumentNumber,
            MaterialNumber,
            CustomerMaterialNumber) mwld
INNER JOIN MaterialStock vmq ON mwld.MaterialNumber =vmq.MaterialNumber
WHERE vmq.StockQty-mwld.Qty <0
 ", warehouseNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sqls.Add(sql);
            //更新库存
            sql = string.Format(@"update MaterialStock set StockQty =
case when a.ChangeDirection='入库' then StockQty +a.Qty else StockQty-a.Qty end
,UpdateTime =CONVERT (varchar(100),GETDATE(),120) from
( select mwld.MaterialNumber,mwl.ChangeDirection,sum(mwld.Qty) as Qty from  MaterialWarehouseLogDetail mwld
  inner join MarerialWarehouseLog mwl on
 mwld.WarehouseNumber =mwl.WarehouseNumber
 where mwl.WarehouseNumber='{0}'
 group by mwld.MaterialNumber,mwl.ChangeDirection) a
 inner join MaterialStock ms on ms.MaterialNumber=a.MaterialNumber
 where  ms.WarehouseName ='ycl'
 ", warehouseNumber);
            sqls.Add(sql);

            sql = string.Format(@" update MarerialWarehouseLog set Auditor='{0}' , CheckTime='{1}' where WarehouseNumber  ='{2}'", auditor, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), warehouseNumber);
            sqls.Add(sql);
            //更新瞬时库存数量
            sqls.Add(StoreroomToolManager.GetUpDateInventoryQtySql(warehouseNumber));
            //写入流水账
            sqls.Add(StoreroomToolManager.WriteMateriLSZ(warehouseNumber, auditor));

            sql = string.Format(" delete DeliveryNoteDetailed where DeliveryQty=0 ");//清除发货数量为0的送货单
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }

        /// <summary>
        /// 获取生成送货单的语句【销售出库（贸易）】
        /// </summary>
        /// <param name="warehouserNumber"></param>
        /// <returns></returns>
        public static List<string> GetSqlForShSHD(string warehouserNumber)
        {
            string shNumber = "SH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            List<string> sqls = new List<string>();
            //找到这个出入库单
            string sql = string.Format(@"  select distinct so.CustomerId  from MaterialWarehouseLogDetail  mwl inner join SaleOder  so on so.OdersNumber =mwl.DocumentNumber
 where mwl.WarehouseNumber ='{0}'", warehouserNumber);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)//按客户遍历
            {
                //连接器一张单子 不是连接器一张单子
                sql = string.Format(@" insert into DeliveryBill (DeliveryNumber,IsConfirm ,CreateTime ,CustomerId )
 values('{0}_{2}','未确认','{1}','{2}')", shNumber + "_1", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["CustomerId"]);
                sqls.Add(sql);
                sql = string.Format(@" insert into DeliveryNoteDetailed
  select '{0}_{2}',mwl.DocumentNumber ,mwl.MaterialNumber ,mwl.CustomerMaterialNumber,'',mwl.RowNumber
  ,mwl.LeadTime ,1,mif.Description ,mwl.Qty,0,0,'',0,0,'','','','','','' from MaterialWarehouseLogDetail  mwl
 inner join SaleOder  so on so.OdersNumber =mwl.DocumentNumber
 inner join MarerialInfoTable mif on mif.MaterialNumber=mwl.MaterialNumber
 where mwl.WarehouseNumber ='{1}' and so.CustomerId ='{2}' and mif.Type ='连接器' and mwl.Qty>0", shNumber + "_1", warehouserNumber, dr["CustomerId"]);
                sqls.Add(sql);

                sql = string.Format(@" insert into DeliveryBill (DeliveryNumber,IsConfirm ,CreateTime ,CustomerId )
 values('{0}_{2}','未确认','{1}','{2}')", shNumber + "_2", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["CustomerId"]);
                sqls.Add(sql);
                sql = string.Format(@" insert into DeliveryNoteDetailed
  select '{0}_{2}',mwl.DocumentNumber ,mwl.MaterialNumber ,mwl.CustomerMaterialNumber,'',mwl.RowNumber
  ,mwl.LeadTime ,1,mif.Description ,mwl.Qty,0,0,'',0,0,'','','','','','' from MaterialWarehouseLogDetail  mwl
 inner join SaleOder  so on so.OdersNumber =mwl.DocumentNumber
 inner join MarerialInfoTable mif on mif.MaterialNumber=mwl.MaterialNumber
 where mwl.WarehouseNumber ='{1}' and so.CustomerId ='{2}' and mif.Type !='连接器' and mwl.Qty>0", shNumber + "_2", warehouserNumber, dr["CustomerId"]);
                sqls.Add(sql);
                //删除没有明细的送货单
                sql = string.Format(@" delete DeliveryBill where DeliveryNumber not in(
 select distinct DeliveryNumber from DeliveryNoteDetailed) ");
                sqls.Add(sql);
            }
            return sqls;
        }

        /// <summary>
        /// 获取更新库存数量的sql语句
        /// </summary>
        /// <param name="materialNumber"></param>
        /// <param name="warehouseId"></param>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetUpdateInventoryQtySql(string materialNumber, string warehouseId, string qty, string type)
        {
            string sql = string.Format(@"
 select count(*) from MaterialStock where MaterialNumber ='{0}' and WarehouseName='{1}'", materialNumber, warehouseId);
            //            if (SqlHelper.GetScalar(sql).Equals("0")) //没有就插入数据
            //            {
            //                string poor = type.Equals("入库") ? qty : "0";
            //                sql = string.Format(@"  insert into MaterialStock (MaterialNumber ,WarehouseName,StockQty ,UpdateTime )
            // values('{0}','{1}',{2},'{3}') ", materialNumber, warehouseId, poor, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //            }
            //            else
            //            {
            string poor = type.Equals("入库") ? qty : "-" + qty;
            sql = string.Format(@"
 update MaterialStock set StockQty =StockQty+({0}),UpdateTime ='{1}' where MaterialNumber='{2}' and WarehouseName='{3}'"
, poor, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialNumber, warehouseId);
            //}
            return sql;
        }

        /// <summary>
        /// 获取更新订单完成数量的SQL语句【仅限于销售出库（贸易）】
        /// </summary>
        /// <param name="warehouserNumber"></param>
        /// <returns></returns>
        public static string GetUpdateQtySql(string warehouserNumber)
        {
            string sql = string.Format(@"

UPDATE TradingOrderDetail
SET    DeliveryQty = Quantity,
       Status = '已完成',
       NonDeliveryQty = 0
WHERE  NonDeliveryQty < 0;

UPDATE TradingOrderDetail
SET    DeliveryQty = DeliveryQty + a.qty
FROM   TradingOrderDetail t
       INNER JOIN (SELECT DocumentNumber,
                          RowNumber,
                          MaterialNumber,
                          Sum(Qty) qty
                   FROM   MaterialWarehouseLogDetail
                   WHERE  Qty > 0
                          AND WarehouseNumber = '{0}'
                   GROUP  BY DocumentNumber,
                             RowNumber,
                             MaterialNumber) a
               ON t.OdersNumber = a.DocumentNumber
                  AND t.ProductNumber = a.MaterialNumber
                  AND t.RowNumber = a.RowNumber;

UPDATE TradingOrderDetail
SET    NonDeliveryQty = Quantity - DeliveryQty
WHERE  NonDeliveryQty > 0
       AND OdersNumber IN (SELECT DISTINCT DocumentNumber
                           FROM   MaterialWarehouseLogDetail
                           WHERE  WarehouseNumber = '{0}');

UPDATE SaleOder
SET    OrderStatus = '已完成'
WHERE  OdersNumber IN (SELECT OdersNumber
                       FROM   TradingOrderDetail
                       GROUP  BY OdersNumber
                       HAVING Sum(NonDeliveryQty) = 0)
       AND OrderStatus = '未完成';

", warehouserNumber);
            return sql;
        }

        /// <summary>
        /// 原材料生产出库审核
        /// </summary>
        /// <param name="auditor"></param>
        /// <param name="warehouseNumber"></param>
        /// <returns></returns>
        public static string SCCKAuditor(string auditor, string warehouseNumber)
        {
            string error = string.Empty;
            List<string> sqls = new List<string>();
            //            string sql = string.Format(@" update MaterialStock set StockQty =vmq.StockQty-mwld.Qty   from  MaterialWarehouseLogDetail mwld
            //inner join MaterialStock vmq on mwld.MaterialNumber =vmq.MaterialNumber
            //where mwld.WarehouseNumber='{0}' ", warehouseNumber);
            //            sqls.Add(sql);

            //写入欠料明细
            //            string sql = string.Format(@"
            //insert into T_LessMaterialBreakdown(WarehouseNumber ,DocumentNumber ,MaterialNumber ,RowNumber ,LeadTime ,CustomerMaterialNumber
            //,LibraryQty,StockQty ,LessMaterialQty ,CreateTime ,IsLessMaterial )
            //select mwld.WarehouseNumber,mwld.DocumentNumber ,mwld. MaterialNumber,mwld.RowNumber ,
            //mwld.LeadTime ,mwld.CustomerMaterialNumber,mwld. Qty,vmq.StockQty,
            //case when vmq.StockQty<0 then mwld.Qty else
            //vmq.StockQty -mwld.Qty end,'{1}','未还料'   from  MaterialWarehouseLogDetail mwld
            //inner join MaterialStock vmq on mwld.MaterialNumber =vmq.MaterialNumber
            //where mwld.WarehouseNumber='{0}'
            //and vmq.StockQty-mwld.Qty <0", warehouseNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //            sqls.Add(sql);
            string sql = string.Format(@"insert into T_LessMaterialBreakdown(WarehouseNumber ,DocumentNumber ,MaterialNumber ,RowNumber ,LeadTime ,CustomerMaterialNumber
,LibraryQty,StockQty ,LessMaterialQty ,CreateTime ,IsLessMaterial )
select '',mwld.DocumentNumber ,mwld. MaterialNumber,'',
'' ,mwld.CustomerMaterialNumber,mwld. Qty,vmq.StockQty,
case when vmq.StockQty<0 then 0-mwld.Qty else
vmq.StockQty -mwld.Qty end,'{1}','未还料'   from  (
select DocumentNumber,MaterialNumber,CustomerMaterialNumber,SUM(Qty) Qty from MaterialWarehouseLogDetail
where WarehouseNumber ='{0}'
group by DocumentNumber,MaterialNumber,CustomerMaterialNumber) mwld
inner join MaterialStock vmq on mwld.MaterialNumber =vmq.MaterialNumber
where
 vmq.StockQty-mwld.Qty <0
 ", warehouseNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sqls.Add(sql);
            //更新库存
            sql = string.Format(@"
update MaterialStock set StockQty =
case when a.ChangeDirection='入库' then StockQty +a.Qty else StockQty-a.Qty end
,UpdateTime =CONVERT (varchar(100),GETDATE(),120) from
( select mwld.MaterialNumber,mwl.ChangeDirection,sum(mwld.Qty) as Qty from  MaterialWarehouseLogDetail mwld
  inner join MarerialWarehouseLog mwl on
 mwld.WarehouseNumber =mwl.WarehouseNumber
 where mwl.WarehouseNumber='{0}' and mwld.Qty>0
 group by mwld.MaterialNumber,mwl.ChangeDirection) a
 inner join MaterialStock ms on ms.MaterialNumber=a.MaterialNumber
 where  ms.WarehouseName ='ycl'
 ", warehouseNumber);
            sqls.Add(sql);

            sql = string.Format(@" update MarerialWarehouseLog set Auditor='{0}' , CheckTime='{1}' where WarehouseNumber  ='{2}'", auditor, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), warehouseNumber);
            sqls.Add(sql);

            //更新瞬时库存数量
            sqls.Add(StoreroomToolManager.GetUpDateInventoryQtySql(warehouseNumber));
            //写入流水账
            sqls.Add(StoreroomToolManager.WriteMateriLSZ(warehouseNumber, auditor));
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }

        /// <summary>
        /// 批量审核原材料出入库时检查采购入库和销售出库（贸易）的订单的未交数量
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <returns></returns>
        private static string CheckQty(string warehouseNumber)
        {
            string result = string.Empty;
            string sql = string.Format(@"select  t.DocumentNumber,t.MaterialNumber,t.LeadTime from (
select mwhl.DocumentNumber ,mwhl .MaterialNumber,mwhl.LeadTime ,SUM ( mwhl.Qty ) as 总出入库数量 from
MarerialWarehouseLog  mwl inner join MaterialWarehouseLogDetail mwhl on mwl.WarehouseNumber =mwhl .WarehouseNumber
where mwl.Type in ('采购入库')
and mwl.WarehouseNumber in({0})
group by mwhl.DocumentNumber ,mwhl .MaterialNumber ,mwhl.LeadTime )
t left join CertificateOrdersDetail cod on t.DocumentNumber =cod.OrdersNumber and t.MaterialNumber =cod.MaterialNumber
and t.LeadTime=cod.LeadTime  where t.总出入库数量-cod .NonDeliveryQty >0", warehouseNumber);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                result += string.Format(" 类型为采购入库的订单编号为{0}原材料编号为{1}交期为{2}的入库数量异常，请检查！<br/>"
                    , dr["DocumentNumber"], dr["MaterialNumber"], dr["LeadTime"]);
            }

            sql = string.Format(@" select vcq.* ,cod.NonDeliveryQty  from MaterialWarehouseLogDetail mwld
 inner join  MarerialWarehouseLog mwl on mwl.WarehouseNumber=mwld.WarehouseNumber
 left join V_Collect_MaterialWarehouseLogDetail_Qty vcq
 on mwld.DocumentNumber =vcq .DocumentNumber and mwld.MaterialNumber =vcq.MaterialNumber
 and mwld.RowNumber =vcq.RowNumber
 left join TradingOrderDetail cod on vcq.DocumentNumber =cod.OdersNumber and vcq.MaterialNumber =cod.ProductNumber
and vcq.RowNumber=cod.RowNumber
 where mwl.Type ='销售出库（贸易）' and mwl.WarehouseNumber in ({0}) and vcq.数量-cod .NonDeliveryQty >0", warehouseNumber);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                result += string.Format(" 根据销售订单汇总后类型为销售出库（贸易）的订单编号为{0}原材料编号为{1}行号为{2}的出库数量大于未交数量异常，请检查！<br/>"
                    , dr["DocumentNumber"], dr["MaterialNumber"], dr["RowNumber"]);
            }

            sql = string.Format(@" select vcq.* ,cod.NonDeliveryQty  from MaterialWarehouseLogDetail mwld
 inner join  MarerialWarehouseLog mwl on mwl.WarehouseNumber=mwld.WarehouseNumber
 left join V_Collect_MaterialWarehouseLogDetail_Qty vcq
 on mwld.DocumentNumber =vcq .DocumentNumber and mwld.MaterialNumber =vcq.MaterialNumber
 and mwld.RowNumber =vcq.RowNumber
 left join TradingOrderDetail cod on vcq.DocumentNumber =cod.OdersNumber and vcq.MaterialNumber =cod.ProductNumber
and vcq.RowNumber=cod.RowNumber
 where mwl.Type ='维修出库' and mwl.WarehouseNumber in ({0}) and vcq.数量-cod .NonDeliveryQty >0", warehouseNumber);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                result += string.Format(" 根据销售订单汇总后类型为维修出库的订单编号为{0}原材料编号为{1}行号为{2}的出库数量大于未交数量异常，请检查！<br/>"
                    , dr["DocumentNumber"], dr["MaterialNumber"], dr["RowNumber"]);
            }

            sql = string.Format(@" select vcq.* ,cod.NonDeliveryQty  from MaterialWarehouseLogDetail mwld
 inner join  MarerialWarehouseLog mwl on mwl.WarehouseNumber=mwld.WarehouseNumber
 left join V_Collect_MaterialWarehouseLogDetail_Qty vcq
 on mwld.DocumentNumber =vcq .DocumentNumber and mwld.MaterialNumber =vcq.MaterialNumber
 and mwld.RowNumber =vcq.RowNumber
 left join TradingOrderDetail cod on vcq.DocumentNumber =cod.OdersNumber and vcq.MaterialNumber =cod.ProductNumber
and vcq.RowNumber=cod.RowNumber
 where mwl.Type ='样品出库' and mwl.WarehouseNumber in ({0}) and vcq.数量-cod .NonDeliveryQty >0", warehouseNumber);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                result += string.Format(" 根据销售订单汇总后类型为样品出库的订单编号为{0}原材料编号为{1}行号为{2}的出库数量大于未交数量异常，请检查！<br/>"
                    , dr["DocumentNumber"], dr["MaterialNumber"], dr["RowNumber"]);
            }
            return result;
        }

        #region 下拉框获取内容通用函数

        public static void BindDrp(string sql, DropDownList drp, string valueName, string textName)
        {
            drp.Items.Clear();
            string error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                drp.DataSource = dt;
                drp.DataValueField = valueName;
                drp.DataTextField = textName;
                drp.DataBind();
            }
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));
        }

        /// <summary>
        /// 下拉框通用获取内容函数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="keyCloumName">key字段名</param>
        /// <param name="valueCloumName">value字段名</param>
        /// <returns></returns>
        public static string GetOption(string sql, string keyCloumName, string valueCloumName)
        {
            string error = string.Empty;
            string result = "<option value =\"\">- - - - - 请 选 择 - - - - -</option>";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            foreach (DataRow dr in dt.Rows)
            {
                result += string.Format(" <option value ='{0}'>{1}</option> ", dr[keyCloumName], dr[valueCloumName]);
            }
            return result;
        }

        #endregion 下拉框获取内容通用函数

        #region 出入库编号（原材料出入库主表）

        public static string GetWarehouseNumber()
        {
            return GetOption(" select  distinct (WarehouseNumber) from MarerialWarehouseLog ", "WarehouseNumber", "WarehouseNumber");
        }

        #endregion 出入库编号（原材料出入库主表）

        #region 变动方向（原材料出入库主表）

        public static string GetChangeDirection()
        {
            return GetOption(" select  distinct (ChangeDirection) from MarerialWarehouseLog ", "ChangeDirection", "ChangeDirection");
        }

        #endregion 变动方向（原材料出入库主表）

        #region 仓库名称（原材料出入库主表）

        public static string GetWarehouseName()
        {
            return GetOption(" select distinct wi.WarehouseName as WarehouseName from MarerialWarehouseLog mh left join WarehouseInfo wi on mh.WarehouseName=wi.WarehouseNumber ", "WarehouseName", "WarehouseName");
        }

        #endregion 仓库名称（原材料出入库主表）

        #region 出入库类型（原材料出入库主表）

        public static string GetType()
        {
            return GetOption(" select  distinct (Type) from MarerialWarehouseLog ", "Type", "Type");
        }

        #endregion 出入库类型（原材料出入库主表）
    }
}