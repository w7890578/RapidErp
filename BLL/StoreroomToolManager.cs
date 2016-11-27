using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace BLL
{
    public class StoreroomToolManager
    {
        /// <summary>
        /// 采购退料出库添加
        /// </summary>
        /// <param name="warehoseNumber"></param>
        /// <param name="cgOrderNumber"></param>
        /// <param name="supplierMaterialNumber"></param>
        /// <param name="qty"></param>
        /// <param name="remark"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddCCTLCKDetail(string warehoseNumber, string cgOrderNumber, string supplierMaterialNumber, string qty, string remark, ref string error)
        {
            string sql = string.Format(@" select COUNT (*) from CertificateOrders where ISNULL ( CheckTime ,'')!='' and OrdersNumber='{0}'", cgOrderNumber);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format(" 系统不存在该采购订单号：{0}", cgOrderNumber); ;
                return false;
            }
            sql = string.Format(@" select MaterialNumber from MaterialSupplierProperty where SupplierMaterialNumber='{0}'", supplierMaterialNumber);
            string materialNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(materialNumber))
            {
                error = string.Format("系统不存在该供应商物料编号:{0}", supplierMaterialNumber);
                return false;
            }
            sql = string.Format(@" insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber
,MaterialNumber ,RowNumber ,LeadTime ,CreateTime ,
 ProductNumber ,SupplierMaterialNumber ,CustomerMaterialNumber,Qty ,InventoryQty ,Remark )
 values('{0}','{6}','{1}','','','{2}','','{3}','',{4},0,'{5}')", warehoseNumber, materialNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
 , supplierMaterialNumber, qty, remark, cgOrderNumber);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        //            if (string.IsNullOrEmpty(materialNumber))
        //            {
        //                error = string.Format("系统不存在该客户辅料号：{0}", customerMaterialNumber);
        //                return false;
        //            }
        //            sql = string.Format(@"
        //insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber ,
        //RowNumber ,LeadTime ,CreateTime ,
        // ProductNumber ,SupplierMaterialNumber ,CustomerMaterialNumber,Qty ,InventoryQty ,Remark )
        // values('{0}','{5}','{1}','','','{2}','','','{6}',{3},0,'{4}')", warehoseNumber, materialNumber,
        //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), qty, remark, planNumber, customerMaterialNumber);
        //            return SqlHelper.ExecuteSql(sql, ref error);
        //        }
        /// <summary>
        /// 辅料出库添加
        /// </summary>
        /// <param name="warehoseNumber"></param>
        /// <param name="qty"></param>
        /// <param name="remark"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        //        public static bool AddFLCKDetail(string warehoseNumber, string planNumber, string customerMaterialNumber, string materialNumber, string qty, string remark, ref string error)
        //        {
        //            string sql = string.Empty;
        //            sql = string.Format(@"
        //insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber ,
        //RowNumber ,LeadTime ,CreateTime ,
        // ProductNumber ,SupplierMaterialNumber ,CustomerMaterialNumber,Qty ,InventoryQty ,Remark )
        // values('{0}','{5}','{1}','','','{2}','','','{6}',{3},0,'{4}')", warehoseNumber, materialNumber,
        //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), qty, remark, planNumber, customerMaterialNumber);
        //            return SqlHelper.ExecuteSql(sql, ref error);
        // }
        public static bool AddFLCKDetail(string warehoseNumber, string plannumber, string customerMaterialNumber, string materialNumber, string qty, string remark, ref string error)
        {
            string sql = string.Empty;
            sql = string.Format(@"
insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber,MaterialNumber ,
RowNumber ,LeadTime ,CreateTime ,
 ProductNumber ,SupplierMaterialNumber ,CustomerMaterialNumber,Qty ,InventoryQty ,Remark )
 values('{0}','','{1}','','','{2}','','','{5}',{3},0,'{4}')", warehoseNumber, materialNumber,
DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), qty, remark, customerMaterialNumber);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 报废上报
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddMarerialScrapLogList(DataRow dr, ref string error)
        {
            string sql = string.Empty;
            DataTable dt = new DataTable();
            if (dr["客户产成品编号(图纸号)"].ToString().Trim() != "")
            {
                sql = string.Format(@"select ProductNumber,Version
from ProductCustomerProperty where CustomerProductNumber ='{0}'", dr["客户产成品编号(图纸号)"]);
                dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count == 0)
                {
                    error = string.Format("系统不存在该客户产成品编号：{0}", dr["客户产成品编号(图纸号)"]);
                    return false;
                }
            }
            sql = string.Format(@"select count(0)  from PM_USER where USER_NAME ='{0}'", dr["责任人"]);

            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("系统不存在该责任人信息：{0}", dr["责任人"]);
                return false;
            }
            sql = string.Format(@"select MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", dr["客户物料编号"]);
            string materialNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(materialNumber))
            {
                error = string.Format("系统不存在该客户物料编号：{0}", dr["客户物料编号"]);
                return false;
            }
            string bfDate = string.Empty;
            try
            {
                bfDate = Convert.ToDateTime(dr["报废日期"]).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                error = string.Format("报废日期：{0},不是正规的日期格式yyyy-MM-dd", dr["报废日期"]);
                return false;
            }
            string productNumber = dt.Rows.Count > 0 ? dt.Rows[0]["ProductNumber"].ToString() : "";
            string version = dt.Rows.Count > 0 ? dt.Rows[0]["Version"].ToString() : "";
            sql = string.Format(@"
 insert into MarerialScrapLog(CreateTime ,ProductNumber ,version,
 CustomerProductNumber,MaterialNumber,CustomerMarealNumber
 ,ScrapDate ,Team ,Count ,ResponsiblePerson,ScrapReason,Remark )
 values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}','{10}','{11}')",
DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), productNumber, version, dr["客户产成品编号(图纸号)"], materialNumber, dr["客户物料编号"]
, bfDate, dr["班组"], dr["数量"], dr["责任人"], dr["报废原因"], dr["备注"]);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 库房绩效上报
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddWarehousePerformanceReviewLog(string year, string month, ref string error)
        {
            List<string> sqls = new List<string>();
            string sql = string.Format(@"
 select COUNT(*) from PerformanceReviewLog where YEAR ='{0}' and MONTH ='{1}' and type='{2}'", year, month, "库房及其它");
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("已上报过{0}年{1}月的绩效", year, month);
                return false;
            }
            sql = string.Format(@" insert into PerformanceReviewLog (Year ,Month ,Name ,PerformanceReviewItem ,RowNumber ,FullScore,Deduction ,Score
,Description ,StatMode ,Type )
select '{0}','{1}',pu.USER_NAME,pfrs.PerformanceReviewItem ,pfrs .RowNumber ,pfrs.FullScore ,0,pfrs .FullScore ,
pfrs.Description ,pfrs.StatMode,'库房及其它'
  from PM_USER pu
inner join PerformanceReviewStandard pfrs on pu.StandardName=pfrs.StandardName
 where pu.Type ='库房及其它' and pfrs .Type ='仓库及其他' ", year, month);
            sqls.Add(sql);
            //出勤得分计算
            sql = string.Format(@"  update PerformanceReviewLog set Score = case when LTRIM(RTRIM (isnull(ea .WeekdaysLeaveForAFewdays,''))) ='' then pfr.FullScore else 0 end
  from PerformanceReviewLog pfr left join EmployeeAttendance ea on pfr.Year =ea.Year
and pfr.Month =ea.Month and pfr.Name =ea.Name
where pfr.YEAR ='{0}' and pfr. MONTH ='{1}' and pfr.type='库房及其它' and pfr.PerformanceReviewItem ='出勤'", year, month);
            sqls.Add(sql);
            //考试成绩计算
            sql = string.Format(@" update PerformanceReviewLog set Score = case when ISNULL (telk.SumScore,0)<60  then 0
 when  ISNULL (telk.SumScore,0)>=60 and ISNULL (telk.SumScore,0)<70  then 10
 when  ISNULL (telk.SumScore,0)>=70 and ISNULL (telk.SumScore,0)<90  then 20
 else 30
  end
  from PerformanceReviewLog pfr left join T_ExaminationLog_KF telk on pfr.Year =telk.Year
and pfr.Month =telk.Month and pfr.Name =telk.Name
where pfr.YEAR ='{0}' and pfr. MONTH ='{1}' and pfr.type='库房及其它' and pfr.PerformanceReviewItem ='月度考试'", year, month);
            sqls.Add(sql);
            //计算扣分
            sql = string.Format(@"   update PerformanceReviewLog set Deduction =FullScore -Score  where
   YEAR ='{0}' and  MONTH ='{1}' and type='库房及其它'", year, month);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        /// <summary>
        /// 样品出库添加
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <param name="orderNumber"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddYPCK(string warehouseNumber, string orderNumber, ref string error)
        {
            string sql = string.Format(@"
select COUNT (*) from MaterialWarehouseLogDetail where WarehouseNumber='{0}' and DocumentNumber ='{1}'", warehouseNumber, orderNumber);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = "已存在相同记录";
                return false;
            }
            sql = string.Format(@"insert into MaterialWarehouseLogDetail(WarehouseNumber ,DocumentNumber ,MaterialNumber ,RowNumber ,LeadTime ,CreateTime ,
ProductNumber ,SupplierMaterialNumber ,CustomerMaterialNumber ,Qty,InventoryQty ,Remark )
 select '{0}',OdersNumber ,ProductNumber ,RowNumber ,Delivery ,'{1}','','',CustomerMaterialNumber,NonDeliveryQty,0,''
  from TradingOrderDetail where OdersNumber ='{2}'", warehouseNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), orderNumber);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 样品入库添加
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <param name="supplierMaterialNumber"></param>
        /// <param name="qty"></param>
        /// <param name="remark"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddYPRK(string warehouseNumber, string supplierMaterialNumber, string qty, string remark, ref string error)
        {
            string sql = string.Format(@"select MaterialNumber from MaterialSupplierProperty
where SupplierMaterialNumber='{0}'", supplierMaterialNumber);
            string materialNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(materialNumber))
            {
                error = string.Format("系统不存在该供应商物料编号:{0}", supplierMaterialNumber);
                return false;
            }
            sql = string.Format(@"insert into MaterialWarehouseLogDetail(WarehouseNumber ,DocumentNumber ,MaterialNumber ,RowNumber ,LeadTime ,CreateTime ,
ProductNumber ,SupplierMaterialNumber ,CustomerMaterialNumber ,Qty,InventoryQty ,Remark )
values('{0}','','{1}','0','','{2}','','{3}','',{4},0,'{5}')", warehouseNumber, materialNumber,
DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), supplierMaterialNumber, qty, remark);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 审核产品出入库
        /// </summary>
        /// <param name="numbers">出入库编号集合</param>
        /// <param name="userNumber">审核人</param>
        /// <returns></returns>
        public static string AutiorProductWarehouseLog(string numbers, string userNumber)
        {
            string sql = string.Empty;
            //             sql = string.Format(@"select ISNULL ( IsConfirm  ,'') as IsConfirm from ProductWarehouseLog
            //            where WarehouseNumber={0} and Type='生产入库'
            //             ", numbers);
            //            DataTable dtTemp = SqlHelper.GetTable(sql);
            //            if (dtTemp.Rows.Count > 0)
            //            {
            //                if (!dtTemp.Rows[0]["IsConfirm"].ToString().Equals("已确认"))
            //                {
            //                    return "生产还未确认该条入库单，不能审核！";
            //                }
            //            }

            sql = string.Format(@"select ISNULL(Auditor,'') from ProductWarehouseLog where WarehouseNumber='{0}'", numbers);
            if (!string.IsNullOrEmpty(SqlHelper.GetScalar(sql)))
            {
                return "该单已被审核！";
            }

            string results = CheckQty(numbers);//匹配销售订单的未交数量
            if (!string.IsNullOrEmpty(results))
            {
                return results;
            }
            if (!CheckInventoryQty(numbers))
            {
                return "当前库存数量低，无法满足当前的出库操作。";
            }
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string shNumber = string.Empty;//送货单号
            string CheckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = string.Format(@" update ProductWarehouseLog set Auditor='{0}' , CheckTime='{1}' where WarehouseNumber in ({2})",
              userNumber, CheckTime, numbers);
            sqls.Add(sql);
            //            sql = string.Format(@"
            //select pwl.WarehouseNumber ,pwl.ChangeDirection,pwld.ProductNumber ,pwld.Version ,
            //case pwl.ChangeDirection when '出库' then 0-pwld.Qty else
            //pwld.Qty end Qty  from ProductWarehouseLogDetail pwld
            //inner join ProductWarehouseLog pwl on pwld.WarehouseNumber =pwl.WarehouseNumber
            //where pwl.WarehouseNumber in ({0})", numbers);
            //            DataTable dt = SqlHelper.GetTable(sql);
            //            foreach (DataRow dr in dt.Rows)
            //            {
            //                sql = GetUpdateInventoryQtySql("", dr["ProductNumber"].ToString(), dr["Version"].ToString(),
            //                      dr["WarehouseNumber"].ToString(), dr["Qty"].ToString(), "ProductStock", "ProductWarehouseLog", ToolEnum.ProductType.Product);
            //                sqls.Add(sql);
            //            }
            //统一更新库存数量
            //            sql = string.Format(@"
            //update  ProductStock set StockQty =
            //case pwl.ChangeDirection when '出库' then StockQty-pwld.Qty else StockQty +pwld.Qty end
            //,UpdateTime =CONVERT (varchar(120),GETDATE(),120) from
            //  ProductWarehouseLogDetail pwld inner join ProductStock ps on pwld.ProductNumber=ps.ProductNumber
            //and pwld.Version =ps.Version
            //inner join ProductWarehouseLog pwl on pwl.WarehouseNumber =pwld.WarehouseNumber
            //where pwl.WarehouseNumber in ({0}) ", numbers);

            //产生送货单(销售出库、样品出库、维修出库)
            string tempSql = string.Format(@"select * from  V_ProductWarehouseLogDetail_SaleOder where WarehouseNumber in ({0})", numbers); //临时变量
            sql = string.Format(" select distinct t.CustomerId  from ( {0} )t ", tempSql); //找出客户
            DataTable dtCustomer = SqlHelper.GetTable(sql);
            foreach (DataRow dr in dtCustomer.Rows) //按客户进行遍历
            {
                //销售出库
                sql = string.Format(" select count(*) from ({0})t where t.CustomerId='{1}' and (t.Type='销售出库' or t.Type='包装销售出库') ", tempSql, dr["CustomerId"]);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    shNumber = "SH" + DateTime.Now.AddSeconds(1).ToString("yyyyMMddHHmmss") + "_" + dr["CustomerId"];
                    sql = string.Format(@"insert into DeliveryBill (DeliveryNumber,IsConfirm,CreateTime ,Remark,CustomerId )
values('{0}','未确认','{1}','由出库产生','{2}')", shNumber, CheckTime, dr["CustomerId"]);
                    sqls.Add(sql);
                    sql = string.Format(@"insert into DeliveryNoteDetailed(DeliveryNumber ,OrdersNumber,ProductNumber ,Version ,CustomerProductNumber
,LeadTime ,RowNumber,SN ,MaterialDescription,DeliveryQty)
select '{2}',vps.DocumentNumber,vps.ProductNumber,vps.Version,vps.CustomerProductNumber,vps.LeadTime,isnull(vps.RowNumber,' ')
,1,vps.Description,Qty  from ({0}) vps  where vps.CustomerId='{1}' and
(vps.Type='销售出库' or vps.Type='包装销售出库')", tempSql, dr["CustomerId"], shNumber);
                    sqls.Add(sql);
                    //sqls.AddRange(CFSHD(shNumber));
                }
                //维修出库
                sql = string.Format(" select count(*) from ({0})t where t.CustomerId='{1}' and t.Type='维修出库' ", tempSql, dr["CustomerId"]);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    shNumber = "SH" + DateTime.Now.AddSeconds(2).ToString("yyyyMMddHHmmss") + "_" + dr["CustomerId"];
                    sql = string.Format(@"insert into DeliveryBill (DeliveryNumber,IsConfirm,CreateTime ,Remark,CustomerId )
values('{0}','未确认','{1}','由产成品维修出库产生','{2}')", shNumber, CheckTime, dr["CustomerId"]);
                    sqls.Add(sql);
                    sql = string.Format(@"insert into DeliveryNoteDetailed(DeliveryNumber ,OrdersNumber,ProductNumber ,Version ,CustomerProductNumber
,LeadTime ,RowNumber,SN ,MaterialDescription,DeliveryQty)
select '{2}',vps.DocumentNumber,vps.ProductNumber,vps.Version,vps.CustomerProductNumber,vps.LeadTime,isnull(vps.RowNumber,' ')
,1,vps.Description,Qty  from ({0}) vps  where vps.CustomerId='{1}' and
vps.Type='维修出库'", tempSql, dr["CustomerId"], shNumber);
                    sqls.Add(sql);
                    //sqls.AddRange(CFSHD(shNumber));
                }
                //样品出库
                sql = string.Format(" select count(*) from ({0})t where t.CustomerId='{1}' and t.Type='样品出库' ", tempSql, dr["CustomerId"]);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    shNumber = "SH" + DateTime.Now.AddSeconds(3).ToString("yyyyMMddHHmmss") + "_" + dr["CustomerId"];
                    sql = string.Format(@"insert into DeliveryBill (DeliveryNumber,IsConfirm,CreateTime ,Remark,CustomerId )
values('{0}','未确认','{1}','由产成品样品出库产生','{2}')", shNumber, CheckTime, dr["CustomerId"]);
                    sqls.Add(sql);
                    sql = string.Format(@"insert into DeliveryNoteDetailed(DeliveryNumber ,OrdersNumber,ProductNumber ,Version ,CustomerProductNumber
,LeadTime ,RowNumber,SN ,MaterialDescription,DeliveryQty)
select '{2}',vps.DocumentNumber,vps.ProductNumber,vps.Version,vps.CustomerProductNumber,vps.LeadTime,isnull(vps.RowNumber,' ')
,1,vps.Description,Qty  from ({0}) vps  where vps.CustomerId='{1}' and
vps.Type='样品出库'", tempSql, dr["CustomerId"], shNumber);
                    sqls.Add(sql);
                    //sqls.AddRange(CFSHD(shNumber));
                }
            }

            //更新开工单中已入库数量
            sql = string.Format(@"
update ProductPlanDetail   set StorageQty =StorageQty+t.Qty  from ProductPlanDetail p inner join
(
select pwld.DocumentNumber,pwld.ProductNumber,pwld.Version ,pwld.RowNumber ,pwld.OrdersNumber ,sum(pwld.Qty ) Qty
from ProductWarehouseLogDetail pwld
inner join ProductWarehouseLog pwl on pwl.WarehouseNumber=pwld.WarehouseNumber
where pwl.Type in('生产入库','样品入库','包装入库')
and pwld.WarehouseNumber in({0})
group by pwld.DocumentNumber,pwld.ProductNumber,pwld.Version ,pwld.RowNumber ,pwld.OrdersNumber
)t
on t.DocumentNumber=p.PlanNumber
and t.OrdersNumber =p.OrdersNumber
and t.ProductNumber =p.ProductNumber
and t.Version =p.Version
and t.RowNumber =p.RowNumber ", numbers);
            sqls.Add(sql);

            //            //更新销售订单已交未交数量
            //            sqls.Add(string.Format(@"update MachineOderDetail
            //set NonDeliveryQty=modt.Qty-vplso.qty ,
            //DeliveryQty=vplso.qty
            //from MachineOderDetail modt
            //inner join
            //V_ProductWarehouseLogDetail_SaleOder vplso on modt.OdersNumber=vplso.DocumentNumber
            //and modt.ProductNumber=vplso.ProductNumber
            //and modt.Version=vplso.Version
            //and modt.RowNumber=vplso.RowNumber
            //where  vplso.WarehouseNumber in ({0})", numbers));

            //更新库存数量
            sql = string.Format(@"update  ProductStock set StockQty =
case pwl.ChangeDirection when '出库' then StockQty-pwld.Qty else StockQty +pwld.Qty end
,UpdateTime =CONVERT (varchar(120),GETDATE(),120) from
  (select WarehouseNumber,ProductNumber,version,sum(Qty) as Qty from ProductWarehouseLogDetail
where WarehouseNumber in ({0})
group by ProductNumber,version,WarehouseNumber) pwld inner join ProductStock ps on pwld.ProductNumber=ps.ProductNumber
and pwld.Version =ps.Version
inner join ProductWarehouseLog pwl on pwl.WarehouseNumber =pwld.WarehouseNumber
where pwl.WarehouseNumber in ({0})", numbers);
            sqls.Add(sql);
            //更新瞬时库存数量
            sqls.Add(GetUpdateQtyProductSql(numbers.TrimStart('\'').TrimEnd('\'')));
            //写入流水账
            sqls.Add(WriteProductLSZ(numbers.TrimStart('\'').TrimEnd('\''), userNumber));
            sql = string.Format(" delete DeliveryNoteDetailed where DeliveryQty=0 ");//清除发货数量为0的送货单
            sqls.Add(sql);
            ToolManager.ZDJC();
            return SqlHelper.BatchExecuteSql(sqls, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 拆分送货单（有项目名称和没有项目名称的进行拆分）
        /// </summary>
        /// <param name="shNumber"></param>
        /// <returns></returns>
        public static List<string> CFSHD(string shNumber)
        {
            List<string> sqls = new List<string>();
            string sql = string.Format(@" insert into DeliveryBill   (DeliveryNumber,DeliveryPerson,DeliveryDate,IsConfirm,CreateTime ,Remark,CustomerId,ConfirmTime )
 select DeliveryNumber+'_1',DeliveryPerson,DeliveryDate,IsConfirm ,CreateTime ,Remark ,
CustomerId,ConfirmTime from DeliveryBill where DeliveryNumber='{0}'
 ", shNumber);
            sqls.Add(sql);
            sql = string.Format(@" insert into  DeliveryBill (DeliveryNumber,DeliveryPerson,DeliveryDate,IsConfirm,CreateTime ,Remark,CustomerId,ConfirmTime )
 select DeliveryNumber+'_2',DeliveryPerson,DeliveryDate,IsConfirm ,CreateTime ,Remark ,
CustomerId,ConfirmTime from DeliveryBill where DeliveryNumber='{0}'
 ", shNumber);
            sqls.Add(sql);

            //项目名称为空的一张送货单
            sql = string.Format(@" insert into DeliveryNoteDetailed
 select dnd.DeliveryNumber+'_1',dnd.OrdersNumber ,dnd.ProductNumber ,dnd.CustomerProductNumber,dnd.Version ,dnd.RowNumber ,dnd.LeadTime
 ,dnd.SN ,dnd.MaterialDescription ,dnd.DeliveryQty,dnd .ArriveQty,dnd.ConformenceQty ,dnd.NGReason
  ,dnd.PassQty ,dnd.NgQty,dnd.InspectorNGReason ,dnd.RoughCastingCode,dnd.ImportPartsCode,dnd.IsGeneratingCope
  ,dnd.ProjectName,dnd.Remark   from DeliveryNoteDetailed
 dnd left join T_ProjectInfo tpi on dnd.ProductNumber=tpi.ProductNumber
 and dnd.Version =tpi.Version  where  ISNULL ( tpi.ProjectName,'')='' and DeliveryNumber='{0}'", shNumber);
            sqls.Add(sql);
            //项目名称不为空的一张送货单
            sql = string.Format(@" insert into DeliveryNoteDetailed
 select dnd.DeliveryNumber+'_2',dnd.OrdersNumber ,dnd.ProductNumber ,dnd.CustomerProductNumber,dnd.Version ,dnd.RowNumber ,dnd.LeadTime
 ,dnd.SN ,dnd.MaterialDescription ,dnd.DeliveryQty,dnd .ArriveQty,dnd.ConformenceQty ,dnd.NGReason
  ,dnd.PassQty ,dnd.NgQty,dnd.InspectorNGReason ,dnd.RoughCastingCode,dnd.ImportPartsCode,dnd.IsGeneratingCope
  ,dnd.ProjectName,dnd.Remark   from DeliveryNoteDetailed
 dnd left join T_ProjectInfo tpi on dnd.ProductNumber=tpi.ProductNumber
 and dnd.Version =tpi.Version  where  ISNULL ( tpi.ProjectName,'')!='' and DeliveryNumber='{0}'", shNumber);
            sqls.Add(sql);
            sql = string.Format(" delete DeliveryNoteDetailed where DeliveryNumber='{0}'", shNumber);
            sqls.Add(sql);
            sql = string.Format("  delete DeliveryBill where DeliveryNumber='{0}' ", shNumber);
            sqls.Add(sql);
            //删除没有明细的送货单
            sql = string.Format(@" delete DeliveryBill where DeliveryNumber not in(
 select distinct DeliveryNumber from DeliveryNoteDetailed) ");
            sqls.Add(sql);
            return sqls;
        }

        /// <summary>
        /// 检测当前库存数量是否满足出库数量(产品出入库)
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <returns></returns>
        public static bool CheckInventoryQty(string warehouseNumber)
        {
            string sql = string.Format(@"  select COUNT (*) from (
 select t.ProductNumber ,t.Version,t.WarehouseName ,(ISNULL ( ps.StockQty,0)-t.Qty) as 差 from (
  select pwld .ProductNumber ,pwld.Version ,pwld .Qty ,pwl.WarehouseName from ProductWarehouseLog pwl
  inner join ProductWarehouseLogDetail pwld on pwl.WarehouseNumber=pwld.WarehouseNumber
  where pwl.ChangeDirection ='出库' and pwl.WarehouseNumber in ({0}) )t
  left join  ProductStock ps on t.ProductNumber =ps.ProductNumber and t.Version =ps.Version and t.WarehouseName=ps.WarehouseName
  )t where 差 <0", warehouseNumber);
            return SqlHelper.GetScalar(sql).Equals("0") ? true : false;
        }

        /// <summary>
        /// 库存盘点审核
        /// </summary>
        /// <param name="inventoryNumber">盘点编号</param>
        /// <param name="userId">当前登录用户</param>
        /// <returns>返回执行结果</returns>
        public static string CheckStockInventoryLog(string inventoryNumber, string userName)
        {
            List<string> sqls = new List<string>();
            string sql = string.Format("  select WarehouseName from StockInventoryLog where InventoryNumber='{0}' ", inventoryNumber);
            string warehouseName = SqlHelper.GetScalar(sql);//查出盘点哪个库
            switch (warehouseName)
            {
                case "bcpk":
                    sql = string.Format(@"update HalfProductStock set StockQty =sild.InventoryQty,UpdateTime =CONVERT(varchar(100), GETDATE(), 120)
 from HalfProductStock hps inner join
StockInventoryLogDetail sild on hps.ProductNumber =sild.MaterialNumber
and hps.Version =sild.Version and hps.MaterialNumber=sild.qlnumber
where hps.WarehouseName ='bcpk' and sild.InventoryNumber ='{0}' and sild.ProfitAndLossQty!=0.00", inventoryNumber);
                    sqls.Add(sql);
                    break;

                case "cpk":
                    //更新库存
                    sql = string.Format(@"update ProductStock set StockQty =StockQty+(sitld.ProfitAndLossQty),UpdateTime =CONVERT(varchar(100), GETDATE(), 120)
from ProductStock ps inner join StockInventoryLogDetail sitld on ps.ProductNumber =sitld .MaterialNumber
and ps.Version =sitld .Version
where ps.WarehouseName ='cpk' and sitld .InventoryNumber ='{0}' and sitld .ProfitAndLossQty!=0.00", inventoryNumber);
                    sqls.Add(sql);
                    //写入流水账
                    sql = string.Format(@"
insert into  ProductWarehouseCurrentAccount
(ProductNumber,CustomerProductNumber,Version,MoveTimer,WarehouseNumber,Income,Issue
,Balances,HandledPerson,MoveReasons,Remark,guid,OrdersNumber)
select sl.MaterialNumber,'',sl.Version ,CONVERT(varchar(100), GETDATE(), 120),InventoryNumber
,0,abs(sl.ProfitAndLossQty) ,vq.库存数量,'{0}','产品库盘点出库',remark,NEWID (),''
 from StockInventoryLogDetail
 sl inner join V_ProductStock_Sum vq on sl.MaterialNumber=vq.ProductNumber and sl.Version =vq.Version
  where InventoryNumber ='{1}' and  sl.ProfitAndLossQty<0.00", userName, inventoryNumber);
                    sqls.Add(sql);
                    sql = string.Format(@" insert into  ProductWarehouseCurrentAccount
(ProductNumber,CustomerProductNumber,Version,MoveTimer,WarehouseNumber,Income,Issue
,Balances,HandledPerson,MoveReasons,Remark,guid,OrdersNumber)
 select sl.MaterialNumber,'',sl.Version ,CONVERT(varchar(100), GETDATE(), 120),InventoryNumber
,abs(sl.ProfitAndLossQty) ,0,vq.库存数量,'{0}','产品库盘点入库',remark,NEWID (),''
 from StockInventoryLogDetail
 sl inner join V_ProductStock_Sum vq on sl.MaterialNumber=vq.ProductNumber and sl.Version =vq.Version
  where InventoryNumber ='{1}'   and  sl.ProfitAndLossQty>0.00", userName, inventoryNumber);
                    sqls.Add(sql);

                    break;

                case "fpk":
                    sql = string.Format(@" update ScrapStock set StockQty =sild.InventoryQty,UpdateTime =CONVERT(varchar(100), GETDATE(), 120) from ScrapStock ss inner join
 StockInventoryLogDetail sild on ss.MaterialNumber =sild.MaterialNumber
 where ss.WarehouseName='fpk' and sild.InventoryNumber ='{0}' and sild.ProfitAndLossQty!=0.00", inventoryNumber);
                    sqls.Add(sql);
                    break;

                case "ycl":
                    //更新库存
                    sql = string.Format(@" update MaterialStock set StockQty =StockQty+(sild.ProfitAndLossQty) ,UpdateTime =CONVERT(varchar(100), GETDATE(), 120) from
 MaterialStock ss inner join StockInventoryLogDetail sild on ss.MaterialNumber =sild .MaterialNumber
 where ss.WarehouseName='ycl' and sild .InventoryNumber ='{0}' and sild.ProfitAndLossQty !=0.00", inventoryNumber);
                    sqls.Add(sql);

                    //写入流水账
                    sql = string.Format(@"
insert into  MateialWarehouseCurrentAccount
(MaterialNumber,CustomerMaterialNumber,MoveTime,WarehouseNumber,
OrdersNumber,Income,Issue,Balances,HandledPerson,MoveReasons,Remark,guid,SupplierMaterialNumber)
select sl.MaterialNumber,'',CONVERT(varchar(100), GETDATE(), 120),sl.InventoryNumber
,'',abs(sl.ProfitAndLossQty) ,0,vq.qty,'{0}','原材料库盘点入库',remark,NEWID (),''
 from StockInventoryLogDetail sl inner join V_MaterialStock_Qty vq on sl.MaterialNumber=vq.MaterialNumber
 where InventoryNumber ='{1}'  and sl.ProfitAndLossQty>0
", userName, inventoryNumber);
                    sqls.Add(sql);
                    sql = string.Format(@"insert into  MateialWarehouseCurrentAccount
(MaterialNumber,CustomerMaterialNumber,MoveTime,WarehouseNumber,
OrdersNumber,Income,Issue,Balances,HandledPerson,MoveReasons,Remark,guid,SupplierMaterialNumber)
 select sl.MaterialNumber,'',CONVERT(varchar(100), GETDATE(), 120),InventoryNumber
,'',0,abs(sl.ProfitAndLossQty) ,vq.qty,'{0}','原材料库盘点出库',remark,NEWID (),''
 from StockInventoryLogDetail
 sl inner join V_MaterialStock_Qty vq on sl.MaterialNumber=vq.MaterialNumber
 where InventoryNumber ='{1}'   and sl.ProfitAndLossQty<0.00", userName, inventoryNumber);
                    sqls.Add(sql);

                    //                    sql = string.Format(@"
                    //insert into  MateialWarehouseCurrentAccount
                    //select MaterialNumber,'',CONVERT(varchar(100), GETDATE(), 120),InventoryNumber
                    //,'',abs(ProfitAndLossQty) ,0,InventoryQty,'{0}','原材料库盘点入库','',NEWID (),''
                    // from StockInventoryLogDetail where InventoryNumber ='{1}' and ProfitAndLossQty>0
                    //", userName, inventoryNumber);

                    break;

                case "ypk":
                    sql = string.Format(@" update SampleStock set StockQty =sild.InventoryQty ,UpdateTime =CONVERT(varchar(100), GETDATE(), 120) from
 SampleStock ss inner join StockInventoryLogDetail sild on ss.MaterialNumber =sild .MaterialNumber
 where ss.WarehouseName='ypk' and sild .InventoryNumber ='{0}' and sild.ProfitAndLossQty !=0.00", inventoryNumber);
                    sqls.Add(sql);
                    break;
            }
            sql = string.Format(@" update StockInventoryLog set Auditor ='{0}' ,AuditeTime =CONVERT (varchar(100),GETDATE (),120)
where InventoryNumber ='{1}' ", userName, inventoryNumber);
            sqls.Add(sql);
            string error = string.Empty;
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }

        /// <summary>
        /// 废品出入库审核
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <param name="changeDirection"></param>
        /// <param name="userId"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool FPCRKCheck(string warehouseNumber, string userId, string userName, ref string error)
        {
            string sql = string.Empty;
            string qty = string.Empty;
            List<string> sqls = new List<string>();
            string changeDirection = string.Empty;
            sql = string.Format("  select ChangeDirection  from ScarpWarehouseLog where WarehouseNumber='{0}' ", warehouseNumber);
            changeDirection = SqlHelper.GetScalar(sql);
            if (changeDirection.Equals("出库"))
            {
                sql = string.Format(@" select COUNT(*) from ScarpWarehouseLogDetail swld left join V_ScrapStock_Qty vsq
 on swld.MaterialNumber =vsq.MaterialNumber where swld.Qty >ISNULL( vsq.StockQty ,0)
 and swld.WarehouseNumber ='{0}'", warehouseNumber);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    error = "当前库存数量低，无法出库！";
                    return false;
                }
            }

            sql = string.Format(@" select MaterialNumber,Qty  from ScarpWarehouseLogDetail
where WarehouseNumber='{0}'", warehouseNumber);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                qty = changeDirection.Equals("入库") ? dr["Qty"].ToString() : "-" + dr["Qty"].ToString();
                sql = GetUpdateQtySqlFP(dr["MaterialNumber"].ToString(), qty);
                sqls.Add(sql);
            }

            sql = string.Format(@" update ScarpWarehouseLog set CheckTime ='{0}' ,Auditor ='{1}'
where WarehouseNumber='{2}' ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userId, warehouseNumber);
            sqls.Add(sql);
            sql = string.Format(@"update ScarpWarehouseLogDetail set InventoryQty =vsq.StockQty   from ScarpWarehouseLogDetail swld left join V_ScrapStock_Qty vsq
 on swld.MaterialNumber =vsq.MaterialNumber where WarehouseNumber='{0}'", warehouseNumber);
            sqls.Add(sql);
            //写入流水账
            //sqls.Add(GetFPLSZSql(warehouseNumber, changeDirection, userName));

            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        /// <summary>
        /// 获取出入库的审核时间
        /// </summary>
        /// <param name="warehousenumber">出入编号</param>
        /// <returns>sql</returns>
        public static string GetCheckTime(string warehousenumber)
        {
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"select CheckTime from MarerialWarehouseLog where WarehouseNumber='{0}'", warehousenumber);
            string checktime = SqlHelper.GetScalar(sql);
            return checktime;
        }

        /// <summary>
        /// 根据销售订单号查询客户ID
        /// </summary>
        /// <param name="ordersNumber"></param>
        /// <returns></returns>
        public static string GetCustormerIdByOrdersNumber(string ordersNumber)
        {
            string sql = string.Format(" select CustomerId from SaleOder where OdersNumber ='{0}'", ordersNumber);
            return SqlHelper.GetScalar(sql);
        }

        /// <summary>
        /// 写入流水账的sql
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <param name="changeDirection"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetFPLSZSql(string warehouseNumber, string changeDirection, string userName)
        {
            string sql = string.Empty;
            if (changeDirection.Equals("入库"))
            {
                sql = string.Format(@"
 insert into MateialWarehouseCurrentAccount (MaterialNumber ,CustomerMaterialNumber ,MoveTime,WarehouseNumber ,OrdersNumber
 ,income,Balances,HandledPerson ,MoveReasons,SupplierMaterialNumber  )
  select swld. MaterialNumber,CustomerMarterilNumber,CONVERT (varchar(100),GETDATE(),20),WarehouseNumber,''
 ,Qty ,swld.InventoryQty ,'{0}','{1}',''  from ScarpWarehouseLogDetail swld
 where swld.WarehouseNumber ='{2}'  ", userName, "废品入库", warehouseNumber);
            }
            else
            {
                sql = string.Format(@" insert into MateialWarehouseCurrentAccount (MaterialNumber ,CustomerMaterialNumber ,MoveTime,WarehouseNumber ,OrdersNumber
 ,Issue ,Balances,HandledPerson ,MoveReasons,SupplierMaterialNumber  )
  select swld. MaterialNumber,CustomerMarterilNumber,CONVERT (varchar(100),GETDATE(),20),WarehouseNumber,''
 ,Qty ,swld.InventoryQty ,'{0}','{1}',''  from ScarpWarehouseLogDetail swld
 where swld.WarehouseNumber ='{2}' ", userName, "废品出库", warehouseNumber);
            }
            return sql;
        }

        /// <summary>
        /// 获取库存数量(通用)
        /// </summary>
        /// <param name="materialNumber">原材料编号</param>
        /// <param name="productNumber">产品编号</param>
        /// <param name="version">版本</param>
        /// <param name="warehouseNumber">仓库ID</param>
        /// <param name="stoomTalbleName">库存表</param>
        /// <param name="inOutTalbeName">出入库主表</param>
        /// <param name="type">类型</param>
        /// <returns>库存数量</returns>
        public static string GetinventoryQty(string materialNumber, string productNumber, string version, string warehouseNumber, string stoomTalbleName, string inOutTalbeName, ToolEnum.ProductType type)
        {
            string InventoryQty = string.Empty;
            string sql = string.Empty;
            //原材料获取
            if (type == ToolEnum.ProductType.Marerial)
            {
                sql = string.Format(@" select ms.StockQty from {2} ms where ms.MaterialNumber='{0}' and
            ms.WarehouseName=(select mw.WarehouseName from {3} mw where mw.WarehouseNumber='{1}')",
                materialNumber, warehouseNumber, stoomTalbleName, inOutTalbeName);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    InventoryQty = dt.Rows[0]["StockQty"].ToString();
                }
                else
                {
                    InventoryQty = "0";
                }
            }
            else//产品获取
            {
                sql = string.Format(@"select StockQty  from  {0} where ProductNumber ='{1}' and Version ='{2}' and
WarehouseName =(select WarehouseName from  {4} where WarehouseNumber='{3}')",
stoomTalbleName, productNumber, version, warehouseNumber, inOutTalbeName);
                InventoryQty = SqlHelper.GetScalar(sql);
                InventoryQty = InventoryQty == "" ? "0" : InventoryQty;
            }
            return InventoryQty;
        }

        /// <summary>
        /// 获取库存数量(原材料出入库主表)
        /// </summary>
        /// <param name="materialNumber">原材料编号</param>
        /// <param name="productNumber">产品编号</param>
        /// <param name="version">版本</param>
        /// <param name="warehouseNumber">出入库编号</param>
        /// <param name="talbleName">库存表名</param>
        /// <param name="type">出入库产品类型</param>
        /// <returns>库存数量</returns>
        public static string GetinventoryQty(string materialNumber, string productNumber, string version, string warehouseNumber, string talbleName, ToolEnum.ProductType type)
        {
            string InventoryQty = string.Empty;
            string sql = string.Empty;
            //原材料获取
            if (type == ToolEnum.ProductType.Marerial)
            {
                sql = string.Format(@" select ms.StockQty from {2} ms where ms.MaterialNumber='{0}' and
            ms.WarehouseName=(select mw.WarehouseName from MarerialWarehouseLog mw where mw.WarehouseNumber='{1}')",
                materialNumber, warehouseNumber, talbleName);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    InventoryQty = dt.Rows[0]["StockQty"].ToString();
                }
                else
                {
                    InventoryQty = "0";
                }
            }
            else//产品获取
            {
                sql = string.Format(@"select StockQty  from  {0} where ProductNumber ='{1}' and Version ='{2}' and
WarehouseName =(select WarehouseName from  ProductWarehouseLog where WarehouseNumber='{4}')",
talbleName, productNumber, version, warehouseNumber);
                InventoryQty = SqlHelper.GetScalar(sql);
                InventoryQty = InventoryQty == "" ? "0" : InventoryQty;
            }
            return InventoryQty;
        }

        //获取入库数量
        public static string GetQty(string WarehouseNumber, string DocumentNumber, string MaterialNumber)
        {
            string qty = string.Empty;
            string sql = string.Format(@" select Qty from MaterialWarehouseLogDetail where WarehouseNumber ='{0}' and
                DocumentNumber='{1}' and MaterialNumber='{2}'", WarehouseNumber, DocumentNumber, MaterialNumber);
            DataTable dt = SqlHelper.GetTable(sql);
            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        ///  获取更新库存数量的Sql语句(通用)
        /// </summary>
        /// <param name="materialNumber">原材料编号</param>
        /// <param name="productNumber">产品编号</param>
        /// <param name="version">版本</param>
        /// <param name="warehouseNumber">出入库编号</param>
        /// <param name="Qty">数量</param>
        /// <param name="stoomTableName">库存表名</param>
        /// <param name="inOutTableName">出入库主表名</param>
        /// <param name="type">类型</param>
        /// <returns>sql</returns>
        public static string GetUpdateInventoryQtySql(string materialNumber, string productNumber, string version, string warehouseNumber, string Qty, string stoomTableName, string inOutTableName, ToolEnum.ProductType type)
        {
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"
select WarehouseName from {0} where WarehouseNumber='{1}'", inOutTableName, warehouseNumber);
            string warehouseName = SqlHelper.GetScalar(sql);
            if (type == ToolEnum.ProductType.Marerial)
            {
                sql = string.Format(@"
select COUNT(*) from {0} where MaterialNumber='{1}'and WarehouseName='{2}'", stoomTableName, materialNumber, warehouseName);
                if (SqlHelper.GetTable(sql).Rows[0][0].ToString().Equals("0"))//表示表内没有该条数据
                {
                    sql = string.Format(@" insert into {4} (MaterialNumber,WarehouseName,StockQty,UpdateTime)
values('{0}','{1}',{2},'{3}') ", materialNumber, warehouseName, Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), stoomTableName);
                }
                else
                {
                    sql = string.Format(@" update {4} set StockQty=StockQty+({0}) ,UpdateTime ='{1}' where MaterialNumber='{2}' and WarehouseName='{3}'"
 , Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialNumber, warehouseName, stoomTableName);
                }
            }
            else
            {
                //                sql = string.Format(@" select COUNT (*)
                //from {0} where ProductNumber='{1}' and [Version]='{2}'
                //and WarehouseName='{3}'", stoomTableName, productNumber, version, warehouseName);
                //                if (SqlHelper.GetScalar(sql).Equals("0"))//表示表内没有该条数据
                //                {
                //                    sql = string.Format(@" insert into {0} (ProductNumber ,[Version] ,WarehouseName,StockQty,UpdateTime )
                //values( '{1}','{2}','{3}',{4},'{5}')", stoomTableName, productNumber, version, warehouseName, Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                //                }
                //                else
                //                {
                sql = string.Format(@"update {0} set StockQty =StockQty +({1}) ,UpdateTime ='{2}'
where ProductNumber ='{3}'
and Version ='{4}' and WarehouseName='{5}'"
, stoomTableName, Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), productNumber, version, warehouseName);
                // }
            }
            return sql;
        }

        /// <summary>
        /// 获取更新库存数量的Sql语句(原材料库)
        /// </summary>
        /// <param name="materialNumber">原材料编号</param>
        /// <param name="productNumber">产品编号</param>
        /// <param name="version">版本</param>
        /// <param name="warehouseNumber">出入库编号</param>
        /// <param name="Qty">数量</param>
        /// <param name="type">产品类型</param>
        /// <returns>sql</returns>
        public static string GetUpdateInventoryQtySql(string materialNumber, string productNumber, string version, string warehouseNumber, string Qty, ToolEnum.ProductType type)
        {
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"
select WarehouseName from MarerialWarehouseLog where WarehouseNumber='{0}'", warehouseNumber);
            string warehouseName = SqlHelper.GetScalar(sql);
            if (type == ToolEnum.ProductType.Marerial)
            {
                sql = string.Format(@"
select COUNT(*) from {0} where MaterialNumber='{1}'and WarehouseName='{2}'  ", "MaterialStock", materialNumber, warehouseName);
                if (SqlHelper.GetTable(sql).Rows[0][0].ToString().Equals("0"))//表示表内没有该条数据
                {
                    sql = string.Format(@" insert into MaterialStock (MaterialNumber,WarehouseName,StockQty,UpdateTime)
values('{0}','{1}',{2},'{3}') ", materialNumber, warehouseName, Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    sql = string.Format(@" update MaterialStock set StockQty=StockQty+({0}) ,UpdateTime ='{1}' where MaterialNumber='{2}' and WarehouseName='{3}'"
 , Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialNumber, warehouseName);
                }
            }
            else
            { }
            return sql;
        }

        /// <summary>
        /// 获取更新库存数量的Sql语句(原材料库)
        /// </summary>
        /// <param name="materialNumber">原材料编号</param>
        /// <param name="productNumber">产品编号</param>
        /// <param name="version">版本</param>
        /// <param name="warehouseName">仓库ID</param>
        /// <param name="Qty">数量</param>
        /// <param name="talbeName">表名</param>
        /// <param name="type">产品类型</param>
        /// <returns>sql</returns>
        public static string GetUpdateInventoryQtySql(string materialNumber, string productNumber, string version, string warehouseName, string Qty, string talbeName, ToolEnum.ProductType type)
        {
            string sql = string.Empty;
            string error = string.Empty;
            if (type == ToolEnum.ProductType.Marerial)
            {
                sql = string.Format(@"
select COUNT(*) from {0} where MaterialNumber='{1}'and WarehouseName='{2}'  ", talbeName, materialNumber, warehouseName);
                if (SqlHelper.GetTable(sql).Rows[0][0].ToString().Equals("0"))//表示表内没有该条数据
                {
                    sql = string.Format(@" insert into MaterialStock (MaterialNumber,WarehouseName,StockQty,UpdateTime)
values('{0}','{1}',{2},'{3}') ", materialNumber, warehouseName, Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    sql = string.Format(@" update MaterialStock set StockQty=StockQty+({0}) ,UpdateTime ='{1}' where MaterialNumber='{2}' and WarehouseName='{3}'"
 , Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialNumber, warehouseName);
                }
            }
            else
            { }
            return sql;
        }

        /// <summary>
        /// 获取更新当前原材料出入库单明细的库存数量为当前瞬时库存数量
        /// </summary>
        /// <param name="warehouseNumber">原材料出入库单号</param>
        /// <returns>返回执行sql</returns>
        public static string GetUpDateInventoryQtySql(string warehouseNumber)
        {
            string sql = string.Format(@"
 select Type from MarerialWarehouseLog where WarehouseNumber='{0}'", warehouseNumber);
            if (SqlHelper.GetScalar(sql).Equals("损耗出库"))
            {
                sql = string.Format(@"update MarerialLossLog set InventoryQty =ISNULL (vmq.qty,0) from MarerialLossLog mwld left join V_MaterialStock_Qty
vmq on mwld.MaterialNumber =vmq.MaterialNumber
where mwld .WarehouseNumber ='{0}'", warehouseNumber);
            }
            else
            {
                sql = string.Format(@"update MaterialWarehouseLogDetail set InventoryQty =ISNULL (vmq.qty,0) from MaterialWarehouseLogDetail mwld left join V_MaterialStock_Qty
vmq on mwld.MaterialNumber =vmq.MaterialNumber
where mwld .WarehouseNumber ='{0}'", warehouseNumber);
            }
            return sql;
        }

        /// <summary>
        /// 获取更新库存数量的Sql语句
        /// </summary>
        /// <param name="materialNumber">原材料编号</param>
        /// <param name="productNumber">产品编号</param>
        /// <param name="version">版本</param>
        /// <param name="warehouseNumber">出入库编号</param>
        /// <param name="Qty">数量</param>
        /// <param name="type">产品类型</param>
        /// <returns>sql</returns>
        public static string GetUpdateInventoryQtySqlForProductStoom(string materialNumber, string productNumber, string version, string warehouseNumber, string Qty, ToolEnum.ProductType type)
        {
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"
select WarehouseName from MarerialWarehouseLog where WarehouseNumber='{0}'", warehouseNumber);
            string warehouseName = SqlHelper.GetScalar(sql);
            if (type == ToolEnum.ProductType.Marerial)
            {
                sql = string.Format(@"
select COUNT(*) from {0} where MaterialNumber='{1}'and WarehouseName='{2}'  ", "MaterialStock", materialNumber, warehouseName);
                if (SqlHelper.GetTable(sql).Rows[0][0].ToString().Equals("0"))//表示表内没有该条数据
                {
                    sql = string.Format(@" insert into MaterialStock (MaterialNumber,WarehouseName,StockQty,UpdateTime)
values('{0}','{1}',{2},'{3}') ", materialNumber, warehouseName, Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else
                {
                    sql = string.Format(@" update MaterialStock set StockQty=StockQty+({0}) ,UpdateTime ='{1}' where MaterialNumber='{2}' and WarehouseName='{3}'"
 , Qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialNumber, warehouseName);
                }
            }
            else
            { }
            return sql;
        }

        /// <summary>
        /// 获取更新产品出入库明细的库存数量为当前瞬时库存数量
        /// </summary>
        /// <param name="warehouseNumber">产品出入库单号</param>
        /// <returns>返回执行sql</returns>
        public static string GetUpdateQtyProductSql(string warehouseNumber)
        {
            string sql = string.Format(@"update ProductWarehouseLogDetail set InventoryQty =ISNULL (vps.库存数量,0) from ProductWarehouseLogDetail pwld
 left join V_ProductStock_Sum vps on pwld.ProductNumber =vps.ProductNumber
 and pwld.Version =vps.Version
 where pwld.WarehouseNumber ='{0}'", warehouseNumber);
            return sql;
        }

        public static string GetUpdateQtySqlFP(string materialNumber, string qty)
        {
            string sql = string.Format(@"select COUNT (*) from ScrapStock where
MaterialNumber='{0}' and WarehouseName='fpk'", materialNumber);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                sql = string.Format(@"  insert into ScrapStock (MaterialNumber ,StockQty ,UpdateTime ,WarehouseName )
 values('{0}',{1},'{2}','fpk') ", materialNumber, qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                sql = string.Format(@"
 update ScrapStock set StockQty =StockQty +({0}),
UpdateTime ='{1}' where MaterialNumber='{2}' and WarehouseName='fpk'",
 qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialNumber);
            }
            return sql;
        }

        public static string GetUpdateStockSqlYP(string materialNumber, string qty)
        {
            string sql = string.Format(@"select COUNT (*) from SampleStock where
MaterialNumber='{0}' and WarehouseName='ypk'", materialNumber);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                sql = string.Format(@"  insert into SampleStock (MaterialNumber ,StockQty ,UpdateTime ,WarehouseName )
 values('{0}',{1},'{2}','ypk') ", materialNumber, qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                sql = string.Format(@"
 update SampleStock set StockQty =StockQty +({0}),
UpdateTime ='{1}' where MaterialNumber='{2}' and WarehouseName='ypk'",
 qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialNumber);
            }
            return sql;
        }

        /// <summary>
        /// 获取仓库ID
        /// </summary>
        /// <param name="WarehouseNumber">出入库编号</param>
        /// <returns></returns>
        public static string GetWarehouseId(string WarehouseNumber)
        {
            string sql = string.Format(" select WarehouseName from MarerialWarehouseLog where WarehouseNumber='{0}'", WarehouseNumber);
            DataTable dt = SqlHelper.GetTable(sql);
            return dt.Rows[0][0].ToString();
        }

        /// <summary>
        /// 样品出入库写入流水账
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <param name="type"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GetYPLSZSql(string warehouseNumber, string type, string userName)
        {
            string sql = string.Empty;
            if (type.Equals("样品入库"))
            {
                sql = string.Format(@"
 insert into MateialWarehouseCurrentAccount (MaterialNumber ,CustomerMaterialNumber ,MoveTime,WarehouseNumber ,OrdersNumber
 ,income,Balances,HandledPerson ,MoveReasons,SupplierMaterialNumber  )
 select MaterialNumber,CustomerMaterialNumber,CONVERT (varchar(100),GETDATE(),20),WarehouseNumber,
 DocumentNumber ,Qty,InventoryQty ,'{0}','样品入库',SupplierMaterialNumber from  MaterialWarehouseLogDetail
 where WarehouseNumber ='{1}'", userName, warehouseNumber);
            }
            else
            {
                sql = string.Format(@" insert into MateialWarehouseCurrentAccount (MaterialNumber ,CustomerMaterialNumber ,MoveTime,WarehouseNumber ,OrdersNumber
 ,Issue ,Balances,HandledPerson ,MoveReasons,SupplierMaterialNumber  )
 select MaterialNumber,CustomerMaterialNumber,CONVERT (varchar(100),GETDATE(),20),WarehouseNumber,
 DocumentNumber ,Qty,InventoryQty ,'{0}','样品出库',SupplierMaterialNumber from  MaterialWarehouseLogDetail
 where WarehouseNumber ='{1}'", userName, warehouseNumber);
            }
            return sql;
        }

        /// <summary>
        /// 批量导入报废上报
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpMarerialScrapLogList(FileUpload FU_Excel, HttpServerUtility server, ref string error)
        {
            DataSet ds = ToolManager.ImpExcel(FU_Excel, server);
            if (ds == null)
            {
                error = "选择的文件为空或不是标准的Excel文件！";
                return false;
            } DataTable dt = ds.Tables[0];
            int i = 0;
            string tempError = string.Empty;
            if (dt.Rows.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (DataRow dr in dt.Rows)
            {
                tempError = "";
                if (!AddMarerialScrapLogList(dr, ref tempError))
                {
                    i++;
                    error += string.Format("添加失败：原因--{0}<br/>", tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (dt.Rows.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }

        /// <summary>
        /// 导入原材料库存数量
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpMaterialQty(FileUpload FU_Excel, HttpServerUtility server, ref string error)
        {
            DataSet ds = ToolManager.ImpExcel(FU_Excel, server);
            if (ds == null)
            {
                error = "选择的文件为空或不是标准的Excel文件！";
                return false;
            }
            DataTable dt = ds.Tables[0];
            int i = 0;
            string tempError = string.Empty;
            if (dt.Rows.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (DataRow dr in dt.Rows)
            {
                tempError = "";
                if (!UpdateMaterialQty(dr, ref tempError))
                {
                    i++;
                    error += string.Format("添加失败：原因--{0}<br/>", tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (dt.Rows.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }

        /// <summary>
        /// 导入产成品库存数量
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpProductQty(FileUpload FU_Excel, HttpServerUtility server, ref string error)
        {
            DataSet ds = ToolManager.ImpExcel(FU_Excel, server);
            if (ds == null)
            {
                error = "选择的文件为空或不是标准的Excel文件！";
                return false;
            }
            DataTable dt = ds.Tables[0];
            int i = 0;
            string tempError = string.Empty;
            if (dt.Rows.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (DataRow dr in dt.Rows)
            {
                tempError = "";
                if (!UpdateProductQty(dr, ref tempError))
                {
                    i++;
                    error += string.Format("添加失败：原因--{0}<br/>", tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (dt.Rows.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }

        /// <summary>
        /// 批量导入库房考试成绩
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpT_ExaminationLog_KFList(string year, string month, FileUpload FU_Excel, HttpServerUtility server, ref string error)
        {
            DataSet ds = ToolManager.ImpExcel(FU_Excel, server);
            if (ds == null)
            {
                error = "选择的文件为空或不是标准的Excel文件！";
                return false;
            }
            string sql = string.Format(" select COUNT (*) from T_ExaminationLog_KF where [Year]='{0}' and [month]='{1}' ", year, month);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("已存在{0}年{1}月的考试成绩", year, month);
                return false;
            }

            DataTable dt = ds.Tables[0];
            int i = 0;
            string tempError = string.Empty;
            if (dt.Rows.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (DataRow dr in dt.Rows)
            {
                tempError = "";
                if (!AddT_ExaminationLog_KFList(year, month, dr, ref tempError))
                {
                    i++;
                    error += string.Format("添加失败：原因--{0}<br/>", tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (dt.Rows.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }

        /// <summary>
        /// 检查出入库明细表是否已存在该记录
        /// </summary>
        /// <param name="warehouseNumber">出入库编号</param>
        /// <param name="documentNumber">单据编号</param>
        /// <param name="productNumber">产品编号</param>
        /// <param name="version">版本</param>
        /// <returns></returns>
        public static bool IsExitForProductWarehouseLogDetail(string warehouseNumber, string documentNumber, string productNumber, string version)
        {
            string sql = string.Format(@"  select COUNT(*) from ProductWarehouseLogDetail
where WarehouseNumber='{0}' and DocumentNumber ='{1}'
 and ProductNumber ='{2}' and Version ='{3}'", warehouseNumber, documentNumber, productNumber, version);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }

        /// <summary>
        /// 原材料出入库删除
        /// </summary>
        /// <param name="ids">出入库编号集合</param>
        /// <returns></returns>
        public static string MarerialWarehouseLogListDelete(string ids)
        {
            string sql = string.Empty;
            string error = string.Empty;
            List<string> sqls = new List<string>();

            sql = string.Format(" delete MaterialWarehouseLogDetail where WarehouseNumber in({0}) ", ids);
            sqls.Add(sql);
            sql = string.Format(@" delete MarerialWarehouseLog where WarehouseNumber in ({0})", ids);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }

        /// <summary>
        /// 样品出入库审核
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <param name="type"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool SHYPCRK(string warehouseNumber, string type, string userName, ref string error)
        {
            string sql = string.Empty;
            string qty = string.Empty;
            List<string> sqls = new List<string>();
            if (type.Equals("样品出库"))
            {
                sql = string.Format(@"  select COUNT (*) from MaterialWarehouseLogDetail mwhl inner join TradingOrderDetail tod on mwhl.DocumentNumber =
 tod.OdersNumber and mwhl.MaterialNumber =tod.ProductNumber and mwhl.RowNumber =tod.RowNumber
 where mwhl .Qty >tod.NonDeliveryQty and mwhl.WarehouseNumber='{0}' ", warehouseNumber);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    error = "异常：出库数量大于订单的未交数量！不能出库";
                    return false;
                }
                sql = string.Format(@"
 select COUNT(*) from (
 select MaterialNumber,SUM (Qty ) qty from MaterialWarehouseLogDetail where WarehouseNumber='{0}'
 group by MaterialNumber) t left join V_SampleStock_Qty vq on t.MaterialNumber =vq.MaterialNumber
 where t.qty > isnull(vq.StockQty,0)", warehouseNumber);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    error = "库存数量低，不能出库！";
                    return false;
                }
                //更新贸易销售订单样品订单的状态
                sql = string.Format(@" update TradingOrderDetail set  DeliveryQty=mwhl.Qty ,NonDeliveryQty =Quantity -mwhl.Qty
  from  MaterialWarehouseLogDetail mwhl inner join TradingOrderDetail tod on mwhl.DocumentNumber =
 tod.OdersNumber and mwhl.MaterialNumber =tod.ProductNumber and mwhl.RowNumber =tod.RowNumber
  where mwhl.WarehouseNumber ='{0}'", warehouseNumber);

                sqls.Add(sql);

                sql = string.Format("   update TradingOrderDetail set Status ='已完成' where NonDeliveryQty=0 ");
                sqls.Add(sql);
                sql = string.Format(@" update SaleOder set OrderStatus ='已完成' where OdersNumber in (
 select t.OdersNumber  from (
 select so.OdersNumber ,SUM (tod.NonDeliveryQty ) qty from TradingOrderDetail tod inner join SaleOder so
 on so.OdersNumber =tod.OdersNumber group by so.OdersNumber  ) t where t.qty =0) and
 OdersType ='样品订单' and ProductType ='贸易' ");
                sqls.Add(sql);
            }
            sql = string.Format("  select MaterialNumber,Qty  from MaterialWarehouseLogDetail where WarehouseNumber='{0}' ", warehouseNumber);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                qty = type.Equals("样品入库") ? dr["Qty"].ToString() : "-" + dr["Qty"].ToString();
                sqls.Add(GetUpdateStockSqlYP(dr["MaterialNumber"].ToString(), qty));
            }
            sql = string.Format(@"
 update MarerialWarehouseLog set CheckTime ='{0}' ,Auditor ='{1}' where WarehouseNumber='{2}' ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
 , userName, warehouseNumber);
            sqls.Add(sql);
            sql = string.Format(@" update MaterialWarehouseLogDetail set InventoryQty =vq.StockQty from MaterialWarehouseLogDetail mwld
  left join V_SampleStock_Qty vq on mwld.MaterialNumber = vq.MaterialNumber  where  WarehouseNumber ='{0}'", warehouseNumber);
            sqls.Add(sql);
            //写入原材料流水账
            //sqls.Add(GetYPLSZSql(warehouseNumber, type, userName));
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        public static bool UpdateMaterialQty(DataRow dr, ref string error)
        {
            string sql = string.Format(" select MaterialNumber from MarerialInfoTable where MaterialNumber='{0}' ", dr["原材料编号"]);
            string materialNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(materialNumber))
            {
                error = string.Format(" 系统不存在该原材料编号 ：{0} ", dr["原材料编号"]);
                return false;
            }
            //update MaterialStock set StockQty = where MaterialNumber ");
            List<string> sqls = new List<string>();
            sql = string.Format(@"
update MarerialInfoTable set Cargo ='{0}',StockSafeQty={2} where MaterialNumber='{1}'", dr["货位"], materialNumber, dr["库存安全值"].ToString().Equals("") ? "0" : dr["库存安全值"]);
            sqls.Add(sql);
            sql = string.Format(@"
update MaterialStock set StockQty ={0} where MaterialNumber='{1}'", dr["库存数量"].ToString().Equals("") ? "0" : dr["库存数量"], materialNumber);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        //        public static bool AddFLCKDetail(string warehoseNumber, string planNumber, string customerMaterialNumber, string qty, string remark, ref string error)
        //        {
        //            string sql = string.Format(@"
        // select MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", customerMaterialNumber);
        //            string materialNumber = SqlHelper.GetScalar(sql);
        //public static bool AddSSTLRKDtail(string warehoseNumber, string materialNumber, string qty, string remark, ref string error)
        //{ }
        public static bool UpdateProductQty(DataRow dr, ref string error)
        {
            string version = dr["版本"].ToString().Trim().Equals("") ? "WU" : dr["版本"].ToString().ToUpper();
            if (version.Equals("OO"))
            {
                version = "00";
            }

            string sql = string.Format(" select ProductNumber   from Product where ProductNumber='{0}' and Version='{1}' ", dr["产成品编号"], version);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count == 0)
            {
                error = string.Format(" 系统不存在该产成品编号 ：{0} ，版本 :{1}", dr["产成品编号"], version);
                return false;
            }
            string productNumber = dt.Rows[0]["ProductNumber"].ToString();

            List<string> sqls = new List<string>();
            sql = string.Format(@"

update Product set Cargo ='{0}' where ProductNumber ='{1}' and Version ='{2}'", dr["货位"], productNumber, version);
            sqls.Add(sql);
            sql = string.Format(@"

update ProductStock set StockQty ={0} where ProductNumber ='{1}' and Version ='{2}'", dr["库存数量"], productNumber, version);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        /// <summary>
        /// 原材料出入库写入流水账
        /// <param name="warehouseNumber">出入库编号</param>
        /// <returns >返回执行语句</returns>
        /// </summary>
        public static string WriteMateriLSZ(string warehouseNumber, string userId)
        {
            //获取用户姓名
            string sql = string.Format("select USER_NAME from PM_USER where USER_ID='{0}'", userId);
            string userName = SqlHelper.GetScalar(sql);
            //获取出入库类型、变动方向
            sql = string.Format(@"
select ChangeDirection,Type  from MarerialWarehouseLog where WarehouseNumber='{0}' ", warehouseNumber);
            DataTable dt = SqlHelper.GetTable(sql);
            DataRow dr = dt.Rows[0];
            string changeDirection = dr["ChangeDirection"].ToString();
            string type = dr["Type"].ToString();
            if (type.Equals("损耗出库"))
            {
                sql = string.Format(@"
insert into
    MateialWarehouseCurrentAccount
    (
        MaterialNumber ,
        CustomerMaterialNumber ,
        SupplierMaterialNumber,
        MoveTime ,
        WarehouseNumber ,
        OrdersNumber,
        Issue ,
        Balances ,
        HandledPerson ,
        MoveReasons
    )
select
    ml.MaterialNumber,
    '' ,
    '',
    CONVERT (varchar(100),GETDATE (),20),
    WarehouseNumber,
    '',
    Qty ,
    InventoryQty ,
    '{0}'
    ,'{1}'
from
    MarerialLossLog ml
where
    ml.WarehouseNumber ='{2}'
", userName, "损耗出库", warehouseNumber);
            }
            else if (changeDirection.Equals("出库"))
            {
                //sql = string.Format(@"insert into MateialWarehouseCurrentAccount(MaterialNumber ,CustomerMaterialNumber ,SupplierMaterialNumber,
                //MoveTime ,WarehouseNumber ,OrdersNumber
                //,Issue ,Balances ,HandledPerson ,MoveReasons)
                //select MaterialNumber,CustomerMaterialNumber,SupplierMaterialNumber,convert (varchar(100),getdate(),120) ,WarehouseNumber,DocumentNumber
                //,Qty ,InventoryQty ,'{0}','{1}' from MaterialWarehouseLogDetail where WarehouseNumber='{2}'", userName, type, warehouseNumber);
                //                sql = string.Format(@"
                //insert into MateialWarehouseCurrentAccount
                // (MaterialNumber ,CustomerMaterialNumber ,SupplierMaterialNumber,
                //MoveTime ,WarehouseNumber ,OrdersNumber
                //,Issue ,Balances ,HandledPerson ,MoveReasons,Remark)
                // select MaterialNumber,CustomerMaterialNumber,SupplierMaterialNumber,convert (varchar(100),getdate(),120) ,WarehouseNumber,
                // DocumentNumber,sum(qty) as Qty,InventoryQty,'{0}','{1}','' from MaterialWarehouseLogDetail
                //  where WarehouseNumber='{2}'
                // group by MaterialNumber,CustomerMaterialNumber,SupplierMaterialNumber,WarehouseNumber,DocumentNumber,InventoryQty
                //", userName, type, warehouseNumber);

                //2016年11月27日修改： 上一版的修改存在bug，当同一个出入库单的同一颗原材料来源于不同订单时会重复插入多条流水账数据。
                //此次修改：将不同订单号合并字符串 例如：XS20150612160814,XS20150612160814,XS20150612160814
                sql = string.Format(@"
 insert into
	MateialWarehouseCurrentAccount
    (
	  MaterialNumber ,
	  CustomerMaterialNumber ,
	  SupplierMaterialNumber,
	  MoveTime ,
	  WarehouseNumber ,
	  OrdersNumber,
	  Issue ,
	  Balances ,
	  HandledPerson ,
	  MoveReasons,
	  Remark
    )
select
		 MaterialNumber,
		 CustomerMaterialNumber,
		 SupplierMaterialNumber,
		 Nowdate,
		 WarehouseNumber,
		 case when isnull(DocumentNumber,'')=''
				then  ''
			else
				substring(isnull(DocumentNumber,''),1,len(isnull(DocumentNumber,''))-1) end
		 as DocumentNumber  ,
		 Qty,
		 InventoryQty,
		 HandledPerson,
		 MoveReasons,
		 case when isnull(Remark,'')=''
				then  ''
			else
				substring(isnull(Remark,''),1,len(isnull(Remark,''))-1) end
		 as Remark
 from
 (
	 select
		 MaterialNumber,
		 CustomerMaterialNumber,
		 SupplierMaterialNumber,
		 convert (varchar(100),getdate(),120) as Nowdate,
		 WarehouseNumber,
		 [DocumentNumber]=
		 (
			 select [DocumentNumber] +','
			 from MaterialWarehouseLogDetail as b
			 where
				isnull(b.MaterialNumber,'') = isnull(a.MaterialNumber ,'')
			and isnull(b.CustomerMaterialNumber,'') = isnull(a.CustomerMaterialNumber,'')
			and isnull(b.SupplierMaterialNumber,'') = isnull(a.SupplierMaterialNumber ,'')
			and isnull(b.WarehouseNumber,'') = isnull(a.WarehouseNumber ,'')
			and isnull(b.InventoryQty,0) = isnull(a.InventoryQty ,0)
			for xml path('')
		  ) ,
		 sum(qty) as Qty,
		 InventoryQty,
		 '{0}' as  HandledPerson,
		 '{1}' as  MoveReasons,
		 Remark =
		 (
			 select Remark +','
			 from MaterialWarehouseLogDetail as b
			 where
				isnull(b.MaterialNumber,'') = isnull(a.MaterialNumber ,'')
			and isnull(b.CustomerMaterialNumber,'') = isnull(a.CustomerMaterialNumber,'')
			and isnull(b.SupplierMaterialNumber,'') = isnull(a.SupplierMaterialNumber ,'')
			and isnull(b.WarehouseNumber,'') = isnull(a.WarehouseNumber ,'')
			and isnull(b.InventoryQty,0) = isnull(a.InventoryQty ,0)
			for xml path('')
		  )
	 from MaterialWarehouseLogDetail as a
		where WarehouseNumber='{2}'
	 group by
		 MaterialNumber,
		 CustomerMaterialNumber,
		 SupplierMaterialNumber,
		 WarehouseNumber,
		 InventoryQty
) as  t
            ", userName, type, warehouseNumber);
            }
            else
            {
                //                sql = string.Format(@"insert into MateialWarehouseCurrentAccount(MaterialNumber ,CustomerMaterialNumber ,SupplierMaterialNumber,
                //MoveTime ,WarehouseNumber ,OrdersNumber
                //,Income  ,Balances ,HandledPerson ,MoveReasons)
                //select MaterialNumber,CustomerMaterialNumber,SupplierMaterialNumber,convert (varchar(100),getdate(),120) ,WarehouseNumber,DocumentNumber
                //,Qty ,InventoryQty ,'{0}','{1}' from MaterialWarehouseLogDetail where WarehouseNumber='{2}'", userName, type, warehouseNumber);
                //                sql = string.Format(@"
                //insert into
                //    MateialWarehouseCurrentAccount
                //    (
                //        MaterialNumber ,
                //        CustomerMaterialNumber ,
                //        SupplierMaterialNumber,
                //        MoveTime ,
                //        WarehouseNumber ,
                //        OrdersNumber,
                //        Income  ,
                //        Balances ,
                //        HandledPerson ,
                //        MoveReasons,
                //        Remark
                //    )
                //select
                //    mwld.MaterialNumber,
                //    mwld.CustomerMaterialNumber,
                //    mwld.SupplierMaterialNumber,
                //    convert (varchar(100),getdate(),120) ,
                //    mwld.WarehouseNumber,
                //    mwld.DocumentNumber,
                //    a.Qty ,
                //    mwld.InventoryQty ,
                //    '{0}',
                //    '{1}',
                //    mwld.Remark
                //from
                //    MaterialWarehouseLogDetail
                //mwld
                //    inner join
                //    (
                //        select
                //            MaterialNumber,
                //            sum(qty) as Qty
                //        from
                //            MaterialWarehouseLogDetail
                //        where
                //            WarehouseNumber='{2}'
                //        group by
                //            MaterialNumber
                //    )
                //a on
                //    a.MaterialNumber=mwld.MaterialNumber
                //where
                //    mwld.WarehouseNumber='{2}'
                //", userName, type, warehouseNumber);
                sql = string.Format(@"
insert into
    MateialWarehouseCurrentAccount
    (
        MaterialNumber ,
        CustomerMaterialNumber ,
        SupplierMaterialNumber,
        MoveTime ,
        WarehouseNumber ,
        OrdersNumber,
        Income  ,
        Balances ,
        HandledPerson ,
        MoveReasons,
        Remark
    )
 select
		 MaterialNumber,
		 CustomerMaterialNumber,
		 SupplierMaterialNumber,
		 Nowdate,
		 WarehouseNumber,
		 case when isnull(DocumentNumber,'')=''
				then  ''
			else
				substring(isnull(DocumentNumber,''),1,len(isnull(DocumentNumber,''))-1) end
		 as DocumentNumber  ,
		 Qty,
		 InventoryQty,
		 HandledPerson,
		 MoveReasons,
		 case when isnull(Remark,'')=''
				then  ''
			else
				substring(isnull(Remark,''),1,len(isnull(Remark,''))-1) end
		 as Remark
  from
 (
		select
			 MaterialNumber,
			 CustomerMaterialNumber,
			 SupplierMaterialNumber,
			 convert (varchar(100),getdate(),120) as Nowdate,
			 WarehouseNumber,
			 DocumentNumber=
				 (
					 select [DocumentNumber] +','
					 from MaterialWarehouseLogDetail as b
					 where
						isnull(b.MaterialNumber,'') = isnull(a.MaterialNumber ,'')
					and isnull(b.CustomerMaterialNumber,'') = isnull(a.CustomerMaterialNumber,'')
					and isnull(b.SupplierMaterialNumber,'') = isnull(a.SupplierMaterialNumber ,'')
					and isnull(b.WarehouseNumber,'') = isnull(a.WarehouseNumber ,'')
					and isnull(b.InventoryQty,0) = isnull(a.InventoryQty ,0)
					for xml path('')
				  ) ,
			 sum(qty) as Qty ,
			 InventoryQty ,
			 '{0}' as HandledPerson,
			 '{1}' as MoveReasons,
			 Remark =
				 (
					 select Remark +','
					 from MaterialWarehouseLogDetail as b
					 where
						isnull(b.MaterialNumber,'') = isnull(a.MaterialNumber ,'')
					and isnull(b.CustomerMaterialNumber,'') = isnull(a.CustomerMaterialNumber,'')
					and isnull(b.SupplierMaterialNumber,'') = isnull(a.SupplierMaterialNumber ,'')
					and isnull(b.WarehouseNumber,'') = isnull(a.WarehouseNumber ,'')
					and isnull(b.InventoryQty,0) = isnull(a.InventoryQty ,0)
					for xml path('')
				  )
		from
			MaterialWarehouseLogDetail a
		where
				WarehouseNumber='{2}'
		group by
				 MaterialNumber,
				 CustomerMaterialNumber,
				 SupplierMaterialNumber,
				 WarehouseNumber,
				 InventoryQty
)t
", userName, type, warehouseNumber);
            }
            return sql;
        }

        /// <summary>
        /// 产成品出入库写入流水账
        /// <param name="warehouseNumber">出入库编号</param>
        /// <returns >返回执行语句</returns>
        /// </summary>
        public static string WriteProductLSZ(string warehouseNumber, string userId)
        {
            //获取用户姓名
            string sql = string.Format("select USER_NAME from PM_USER where USER_ID='{0}'", userId);
            string userName = SqlHelper.GetScalar(sql);
            //获取出入库类型、变动方向
            sql = string.Format(@"
 select ChangeDirection ,Type  from ProductWarehouseLog where  WarehouseNumber='{0}' ", warehouseNumber);
            DataTable dt = SqlHelper.GetTable(sql);
            DataRow dr = dt.Rows[0];
            string changeDirection = dr["ChangeDirection"].ToString();
            string type = dr["Type"].ToString();
            if (changeDirection.Equals("入库"))
            {
                //                sql = string.Format(@"insert into ProductWarehouseCurrentAccount
                //(ProductNumber ,CustomerProductNumber ,Version ,MoveTimer
                //,WarehouseNumber ,Income ,Balances ,HandledPerson ,MoveReasons ,OrdersNumber )
                //select ProductNumber ,CustomerProductNumber,Version ,'{0}',WarehouseNumber
                //,Qty ,InventoryQty ,'{1}','{2}',DocumentNumber  from ProductWarehouseLogDetail where WarehouseNumber='{3}'",
                //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userName, type, warehouseNumber);
                sql = string.Format(@"
insert into ProductWarehouseCurrentAccount
(ProductNumber ,CustomerProductNumber ,Version ,MoveTimer
,WarehouseNumber ,Income ,Balances ,HandledPerson ,MoveReasons ,OrdersNumber,remark )
select b.ProductNumber ,a.CustomerProductNumber,b.Version ,'{0}',a.WarehouseNumber
,b.Qty ,a.InventoryQty ,'{1}','{2}',a.DocumentNumber,a.remark  from ProductWarehouseLogDetail a
inner join (select WarehouseNumber,ProductNumber,Version,sum(Qty) as Qty
from ProductWarehouseLogDetail where WarehouseNumber='{3}'
 group by WarehouseNumber,ProductNumber,Version)
 b on  a.WarehouseNumber=b.WarehouseNumber and a.ProductNumber=b.ProductNumber
 and a.Version=b.Version ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userName, type, warehouseNumber);
            }
            else
            {
                //                sql = string.Format(@"insert into ProductWarehouseCurrentAccount (ProductNumber ,CustomerProductNumber ,Version ,MoveTimer
                //,WarehouseNumber ,Issue ,Balances ,HandledPerson ,MoveReasons ,OrdersNumber )
                //select ProductNumber ,CustomerProductNumber,Version ,'{0}',WarehouseNumber
                //,Qty ,InventoryQty ,'{1}','{2}',DocumentNumber  from ProductWarehouseLogDetail where WarehouseNumber='{3}'",
                //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userName, type, warehouseNumber);
                sql = string.Format(@"
insert into ProductWarehouseCurrentAccount
(ProductNumber ,CustomerProductNumber ,Version ,MoveTimer
,WarehouseNumber ,Issue ,Balances ,HandledPerson ,MoveReasons ,OrdersNumber,remark )
select b.ProductNumber ,a.CustomerProductNumber,b.Version ,'{0}',a.WarehouseNumber
,b.Qty ,a.InventoryQty ,'{1}','{2}',a.DocumentNumber,a.remark  from ProductWarehouseLogDetail a
inner join (select WarehouseNumber,ProductNumber,Version,sum(Qty) as Qty
from ProductWarehouseLogDetail where WarehouseNumber='{3}'
 group by WarehouseNumber,ProductNumber,Version)
 b on  a.WarehouseNumber=b.WarehouseNumber and a.ProductNumber=b.ProductNumber
 and a.Version=b.Version ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userName, type, warehouseNumber);
            }
            return sql;
        }

        /// <summary>
        /// 添加考试成绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="dr"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private static bool AddT_ExaminationLog_KFList(string year, string month, DataRow dr, ref string error)
        {
            string sql = string.Format(" select COUNT (*) from PM_USER where USER_NAME ='{0}' ", dr["姓名"]);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("系统不存在该姓名:{0}", dr["姓名"]);
                return false;
            }
            sql = string.Format(@"
insert into T_ExaminationLog_KF(year,month,name,team,bsscore,scscore,sumscore)
values('{0}','{1}','{2}','{3}',{4},{5},({4}+{5}))", year, month, dr["姓名"], "", dr["笔试得分"], dr["实操得分"].ToString() == "" ? "0" : dr["实操得分"]);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 检查类型为销售出库、维修出库、样品出库的产品出入库数量
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns></returns>
        private static string CheckQty(string numbers)
        {
            string result = string.Empty;
            string sql = string.Format(@" select vcq.* ,modt.NonDeliveryQty  from
 ProductWarehouseLogDetail pwld
 inner join  ProductWarehouseLog pwl on pwl.WarehouseNumber=pwld.WarehouseNumber
 left join V_Collect_ProductWarehouseLogDetail_Qty vcq
 on pwld.DocumentNumber =vcq .DocumentNumber and pwld.ProductNumber =vcq.ProductNumber
 and pwld.RowNumber =vcq.RowNumber and pwld.Version =vcq .Version
 left join MachineOderDetail modt on vcq.DocumentNumber =modt.OdersNumber and vcq.ProductNumber  =modt.ProductNumber
and vcq.RowNumber=modt.RowNumber and modt.Version =vcq .Version
 where (pwl.Type ='销售出库' or pwl.Type='包装销售出库')  and pwl.WarehouseNumber in ({0}) and vcq.数量-modt.NonDeliveryQty >0", numbers);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                result += string.Format("销售订单汇总后类型为销售出库【订单编号：{0},产品编号：{1},版本:{2},行号{3}】的出库数量大于订单未交数量！"
, dr["DocumentNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"]);
                sql = string.Format(@"
select distinct WarehouseNumber from ProductWarehouseLogDetail where  DocumentNumber='{0}'
and ProductNumber='{1}' and Version='{2}'
and RowNumber='{3}'", dr["DocumentNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"]);
                result += "涉及出库单为：";
                string temp = "";
                foreach (DataRow drWarehouseNumber in SqlHelper.GetTable(sql).Rows)
                {
                    temp += drWarehouseNumber["WarehouseNumber"] + ",";
                }
                result += temp.TrimEnd(',') + "<br/>";
            }
            sql = string.Format(@" select vcq.* ,modt.NonDeliveryQty  from
 ProductWarehouseLogDetail pwld
 inner join  ProductWarehouseLog pwl on pwl.WarehouseNumber=pwld.WarehouseNumber
 left join V_Collect_ProductWarehouseLogDetail_Qty vcq
 on pwld.DocumentNumber =vcq .DocumentNumber and pwld.ProductNumber =vcq.ProductNumber
 and pwld.RowNumber =vcq.RowNumber and pwld.Version =vcq .Version
 left join MachineOderDetail modt on vcq.DocumentNumber =modt.OdersNumber and vcq.ProductNumber  =modt.ProductNumber
and vcq.RowNumber=modt.RowNumber and modt.Version =vcq .Version
 where pwl.Type ='维修出库' and pwl.WarehouseNumber in ({0}) and vcq.数量-modt.NonDeliveryQty >0", numbers);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                result += string.Format("销售订单汇总后类型为维修出库【订单编号：{0}产品编号：{1}版本:{2}行号{3}】的出库数量大于订单未交数量，请检查！"
, dr["DocumentNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"]);
            }
            sql = string.Format(@"select vcq.* ,modt.NonDeliveryQty  from
 ProductWarehouseLogDetail pwld
 inner join  ProductWarehouseLog pwl on pwl.WarehouseNumber=pwld.WarehouseNumber
 left join V_Collect_ProductWarehouseLogDetail_Qty vcq
 on pwld.DocumentNumber =vcq .DocumentNumber and pwld.ProductNumber =vcq.ProductNumber
 and pwld.RowNumber =vcq.RowNumber and pwld.Version =vcq .Version
 left join MachineOderDetail modt on vcq.DocumentNumber =modt.OdersNumber and vcq.ProductNumber  =modt.ProductNumber
and vcq.RowNumber=modt.RowNumber and modt.Version =vcq .Version
 where pwl.Type ='样品出库' and pwl.WarehouseNumber in ({0}) and vcq.数量-modt.NonDeliveryQty >0", numbers);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                result += string.Format("销售订单汇总后类型为样品出库【订单编号：{0}产品编号：{1}版本:{2}行号{3}】的出库数量大于订单未交数量，请检查！"
, dr["DocumentNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"]);
            }
            return result;
        }
    }
}