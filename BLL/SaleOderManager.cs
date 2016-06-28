using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using Model;
using System.Web;

namespace BLL
{
    public class SaleOderManager
    {

        /// <summary>
        /// 临时订单转正式订单
        /// </summary>
        /// <param name="oldNumber"></param>
        /// <param name="newNumber"></param>
        /// <returns></returns>
        public static string ConvertNumber(string oldNumber, string newNumber)
        {
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string sql = string.Format(" update SaleOder set OdersNumber ='{0}',Auditor ='' , CheckTime ='',OdersType='正常订单' where OdersNumber ='{1}' ", newNumber, oldNumber);
            sqls.Add(sql);
            sql = string.Format("update ProductPlanDetail set OrdersNumber ='{0}' where OrdersNumber ='{1}'", newNumber, oldNumber);
            sqls.Add(sql);
            sql = string.Format("update ProductPlanSubDetail set   OrdersNumber ='{0}' where OrdersNumber ='{1}'", newNumber, oldNumber);
            sqls.Add(sql);
            sql = string.Format("update ProductWarehouseLogDetail set   OrdersNumber ='{0}' where OrdersNumber ='{1}'", newNumber, oldNumber);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }

        /// <summary>
        /// 审核销售订单  
        /// </summary>
        /// <param name="autor">审核人ID</param>
        /// <param name="orderNumbers">销售订单集合</param>
        /// <returns></returns>
        public static string CheckSaleOrder(string autor, string orderNumbers)
        {

            string createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string error = string.Empty;
            List<string> sqls = new List<string>();
            string sql = string.Format(@" update SaleOder set CheckTime ='{0}' ,Auditor ='{1}'
where OdersNumber in({2})  "
, createtime, autor, orderNumbers);
            sqls.Add(sql);
            //产生预收第一层
            sql = string.Format(@"insert into AccountsReceivable (OrdersNumber,CustomerOrdersNumber,SumPrice,CustomerId,PaymentTypes,SKFS,YSOne,YSTwo,CreateTime,IsAdvance) (select t.* from (
select  so.OdersNumber as 销售订单号,
so.CustomerOrderNumber as 客户采购订单号,
t.订单总价,
c.CustomerId as 客户编号,
c.ReceiveType as 收款类型 ,
c.MakeCollectionsModeId 收款方式,
t.预收一,
t.预收二,
'{1}'  as 创建时间,
'{2}' as 是否为预收
 from 
SaleOder so 
inner join (select SUM(TotalPrice) as 订单总价,
isnull(SUM(cast(ReceiveOne as decimal(18,2))),0) as 预收一,
isnull(sum(cast(ReceiveTwo as decimal(18,2))),0) as 预收二,OdersNumber from TradingOrderDetail where OdersNumber in ({0})  group by OdersNumber 
) t on t.OdersNumber =so.OdersNumber 
inner join Customer c on so.CustomerId=c.CustomerId where ISNULL(so.CheckTime,'')!='' and (c.MakeCollectionsModeId='YSBF' or c.MakeCollectionsModeId='YSQK')

union all
select  so.OdersNumber as 销售订单号,
so.CustomerOrderNumber as 客户采购订单号 ,
t.订单总价,
c.CustomerId as 客户编号,
c.ReceiveType as 收款类型 ,
c.MakeCollectionsModeId 收款方式,
t.预收一,
t.预收二,
'{1}' as 创建时间,
'{2}' as 是否为预收
from SaleOder so
inner join (select SUM(SumPrice)as 订单总价,
isnull(SUM(cast(ReceiveOne as decimal(18,2))),0) as 预收一,
isnull(sum(cast(ReceiveTwo as decimal(18,2))),0) as 预收二,
OdersNumber from MachineOderDetail  where OdersNumber in ({0})  group by OdersNumber )t
on so.OdersNumber=t.OdersNumber
inner join Customer c on so.CustomerId=c.CustomerId where isnull(so.CheckTime,'')!=''  and (c.MakeCollectionsModeId='YSBF' or c.MakeCollectionsModeId='YSQK'))t)", orderNumbers, createtime, '是');
            sqls.Add(sql);


            //            sql = string.Format(@"select count(*) from SaleOder so inner join TradingOrderDetail tod
            //on so.OdersNumber =tod.OdersNumber 
            //where tod.OdersNumber in({0})", orderNumbers);
            //            if (!SqlHelper.GetScalar(sql).Equals("0"))//有贸易销售订单 就自动产生出库单
            //            {
            //                string createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //                string warhouseNumber = "YCLCK" + DateTime.Now.ToString("yyyyMMddHHmmss");
            //                sql = string.Format(@"
            //insert into MarerialWarehouseLog( WarehouseNumber,WarehouseName ,ChangeDirection,Type ,
            //Creator,CreateTime ,Remark ,IsConfirm )
            //values('{0}','ycl','出库','销售出库（贸易）','{1}','{2}','由贸易销售订单审核产生','是' )
            //", warhouseNumber, autor, createTime);
            //                sqls.Add(sql);
            //                sql = string.Format(@" 
            //insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber ,RowNumber ,LeadTime ,CreateTime ,
            //CustomerMaterialNumber ,Qty ,UnitPrice)
            //select '{0}',so.OdersNumber ,tod.ProductNumber,tod.RowNumber ,tod.Delivery ,'{1}',tod.CustomerMaterialNumber ,
            //tod.Quantity ,tod.UnitPrice  from SaleOder so inner join TradingOrderDetail tod
            //on so.OdersNumber =tod.OdersNumber 
            //where tod.OdersNumber in({2}) ", warhouseNumber, createTime, orderNumbers);
            //                sqls.Add(sql);
            //            }

            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;

        }

        /// <summary>
        /// 批量导入销售订单
        /// </summary>
        /// <param name="so"></param>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BacthAddSaleOrder(SaleOder so, FileUpload FU_Excel, HttpServerUtility server, ref string error)
        {
            bool result = false;
            DataSet ds = ToolManager.ImpExcel(FU_Excel, server);
            if (ds == null)
            {
                error = "选择的文件为空或不是标准的Excel文件！";
                return false;
            }
            DataTable dt = ds.Tables[0];
            if (so.ProductType.Equals("贸易"))
            {
                if (!dt.Columns.Contains("客户物料编号"))
                {
                    error = "导入模板与生产类型不一致！";
                    return false;
                }
            }
            else
            {
                if (!dt.Columns.Contains("客户产成品编号"))
                {
                    error = "导入模板与生产类型不一致！";
                    return false;
                }
            }

            //插入主表信息
            string sql = string.Format(@"insert into SaleOder (OdersNumber ,OrdersDate
,OdersType ,ProductType ,MakeCollectionsMode,CustomerId 
,ContactId ,CreateTime ,Remark ,CustomerOrderNumber,KhddH )
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", so.OrdersNumber, so.OrdersDate, so.OdersType
, so.ProductType, so.MakeCollectionsMode, so.CustomerId, so.ContactId, so.CreateTime, so.Remark, so.CustomerOrderNumber, so.KhddH);
            SqlHelper.ExecuteSql(sql, ref error);
            //按照生产类型分别批量添加
            if (so.ProductType.Equals("贸易"))
            {
                List<TradingOrderDetail> tods = new List<TradingOrderDetail>();
                foreach (DataRow dr in dt.Rows)
                {
                    TradingOrderDetail tod = new TradingOrderDetail();
                    tod.OrdersNumber = so.OrdersNumber;
                    tod.SN = dr["序号"].ToString();
                    tod.RowNumber = dr["行号"].ToString();
                    tod.Delivery = GetLeadTime(dr["交期"].ToString());
                    tod.CustomerMaterialNumber = dr["客户物料编号"].ToString();
                    tod.Quantity = dr["订单数量"].ToString();
                    tod.Remark = dr["备注"].ToString();
                    tod.CustomerId = so.CustomerId;

                    tods.Add(tod);
                }
                result = BacthAddTradingOrderDetail(tods, ref error);
            }
            else  // 序号	行号	交期	客户产成品编号	订单数量
            {
                List<MachineOderDetail> mods = new List<MachineOderDetail>();
                foreach (DataRow dr in dt.Rows)
                {
                    MachineOderDetail mod = new MachineOderDetail();
                    mod.OrdersNumber = so.OrdersNumber;
                    mod.SN = dr["序号"].ToString();
                    mod.RowNumber = dr["行号"].ToString();
                    mod.LeadTime = GetLeadTime(dr["交期"].ToString());
                    mod.CustomerProductNumber = dr["客户产成品编号"].ToString();
                    mod.Qty = dr["订单数量"].ToString();
                    mod.CustomerId = so.CustomerId;
                    mod.Version = dr["版本"].ToString().ToUpper();
                    if (mod.Version.Equals(""))
                    {
                        mod.Version = SellManager.GetMostNewVersion(dr["客户产成品编号"].ToString());
                    }
                    else if (mod.Version.Equals("OO"))
                    {
                        mod.Version = "00";
                    }
                    mods.Add(mod);
                }
                result = BacthAddMachineOderDetail(mods, ref error);
            }
            return result;

        }

        /// <summary>
        /// 批量导入销售订单
        /// </summary>
        /// <param name="so"></param>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BacthAddSaleOrderNew(SaleOder so, FileUpload FU_Excel, HttpServerUtility server, string userId, ref string error)
        {
            DataSet ds = ToolManager.ImpExcel(FU_Excel, server);
            if (ds == null)
            {
                error = "选择的文件为空或不是标准的Excel文件！";
                return false;
            }
            string sql = string.Format(@" delete  T_ImpSaleOder_Temp where userId='{0}'
", userId);
            SqlHelper.ExecuteSql(sql);

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
                if (!AddSaleOrder(dr, so, userId, ref tempError))
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
            List<string> sqls = new List<string>();
            string xsNumber = "";
            sql = string.Format(@"select COUNT (*) from T_ImpSaleOder_Temp where ismateriNumber ='是' and userid='{0}'", userId);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                xsNumber = "XS" + DateTime.Now.AddSeconds(1).ToString("yyyyMMddHHmmss");
                sql = string.Format(@"insert into SaleOder(OdersNumber,OrdersDate,OdersType,ProductType ,
MakeCollectionsMode,CustomerId ,ContactId ,OrderStatus ,CreateTime ,CustomerOrderNumber,KhddH )
values('{0}','{1}','{2}','贸易','{3}','{4}','{5}','未完成','{6}','{7}','{8}')", xsNumber, so.OrdersDate,
so.OdersType.Equals("包装生产订单") ? "正常订单" : so.OdersType, so.MakeCollectionsMode, so.CustomerId, so.ContactId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
so.CustomerOrderNumber, so.KhddH);
                sqls.Add(sql);
                sql = string.Format(@"insert into TradingOrderDetail (OdersNumber ,ProductNumber,RowNumber ,Delivery ,SN,CustomerMaterialNumber,MaterialName,Brand 
,Quantity ,NonDeliveryQty ,DeliveryQty ,UnitPrice ,TotalPrice ,CreateTime ,Status   )
select '{0}',tit.ProductNumber,tit.RowNumber,CONVERT(varchar(100), CAST(tit.LeadTime as date ), 23),1,tit.CustomerProductNumber,vtdn .物料描述
,vtdn.品牌,tit.Qty,tit.Qty,0,vtdn .单价,vtdn .单价*tit.Qty,'{1}','未完成' 
from T_ImpSaleOder_Temp tit inner join V_FindLastNewPriceForTradingQuoteDetail vtdn on tit.ProductNumber=vtdn.原材料编号
where tit.userId='{2}' and  vtdn.客户名称='{3}' and tit.IsMateriNumber='是'", xsNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userId, so.CustomerName);
                sqls.Add(sql);
            }
            sql = string.Format(@"select COUNT (*) from T_ImpSaleOder_Temp where ismateriNumber ='否' and userid='{0}'", userId);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                xsNumber = "XS" + DateTime.Now.AddSeconds(2).ToString("yyyyMMddHHmmss");
                sql = string.Format(@"insert into SaleOder(OdersNumber,OrdersDate,OdersType,ProductType ,
MakeCollectionsMode,CustomerId ,ContactId ,OrderStatus ,CreateTime ,CustomerOrderNumber,KhddH )
values('{0}','{1}','{2}','加工','{3}','{4}','{5}','未完成','{6}','{7}','{8}')", xsNumber, so.OrdersDate,
so.OdersType, so.MakeCollectionsMode, so.CustomerId, so.ContactId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
so.CustomerOrderNumber, so.KhddH);
                sqls.Add(sql);
                sql = string.Format(@"
insert into MachineOderDetail (OdersNumber ,ProductNumber ,Version ,LeadTime ,RowNumber ,SN ,CustomerProductNumber ,Qty ,NonDeliveryQty 
,DeliveryQty ,UnitPrice ,SumPrice ,CreateTime ,Status )
select '{0}',tit.ProductNumber ,tit.Version ,CONVERT(varchar(100), CAST(tit.LeadTime as date ), 23) ,tit.RowNumber ,1,tit.CustomerProductNumber ,tit.Qty 
,tit.Qty ,0,vmr.单价未税,tit.Qty *vmr.单价未税,'{1}','未完成' from T_ImpSaleOder_Temp tit
inner join  V_FindLastNewPriceForMachineQuoteDeatil vmr on tit.ProductNumber 
=vmr.产成品编号 and tit.Version =vmr.版本  where tit.UserId ='{2}' and vmr.客户名称='{3}' and tit.IsMateriNumber='否' ", xsNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
 , userId, so.CustomerName);
                sqls.Add(sql);
            }
            sql = string.Format(@"delete SaleOder where OdersNumber not in (
select distinct OdersNumber  from MachineOderDetail
union all 
select  distinct OdersNumber  from TradingOrderDetail)");
            sqls.Add(sql);
            string errorTwo = string.Empty;
            bool resultTwo = SqlHelper.BatchExecuteSql(sqls, ref errorTwo);

            if (result && resultTwo)
            {
                return true;
            }
            else
            {
                error += "<br/>" + errorTwo;
                return false;
            }



        }

        private static bool AddSaleOrder(DataRow dr, SaleOder so, string userId, ref string error)
        {
            //要求交期、行号、客户产成品编号、版本、数量、是否是原材料
            string leadTime = "";
            string sql = "";
            // string materialNumber = "";
            string version = dr["版本"].ToString() == "" ? "WU" : dr["版本"].ToString().ToUpper();
            string customerProductNumber = dr["客户产成品编号"].ToString();
            string productNumber = "";
            try
            {
                leadTime = Convert.ToDateTime(dr["要求交期"]).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                error = string.Format("要求交期：{0},不是正规的日期格式‘yyyy-MM-dd’", dr["要求交期"]);
                return false;
            }
            if (dr["是否是原材料"].ToString().Equals("是"))
            {
                sql = string.Format(@" select 产成品编号 from
V_TradingQuoteDetailReport_New where 客户物料编号='{0}' and 客户名称='{1}'", customerProductNumber, so.CustomerName);
                productNumber = SqlHelper.GetScalar(sql);
                if (string.IsNullOrEmpty(productNumber))
                {
                    error = string.Format(" 系统不存在该客户物料编号:{0} 的报价信息,客户{1}", customerProductNumber, so.CustomerName);
                    return false;
                }
                sql = string.Format(@"
select MaterialNumber from MaterialCustomerProperty 
where CustomerMaterialNumber='{0}' and CustomerId='{1}'", customerProductNumber, so.CustomerId);
                productNumber = SqlHelper.GetScalar(sql);
                if (string.IsNullOrEmpty(productNumber))
                {
                    error = string.Format(" 系统不存在该客户物料编号:{0} 的原材料编号,客户{1}", customerProductNumber, so.CustomerName);
                    return false;
                }

            }
            else
            {
                if (string.IsNullOrEmpty(version))
                {
                    version = SellManager.GetMostNewVersion(customerProductNumber);
                }



                sql = string.Format(@"select 产成品编号 from V_FindLastNewPriceForMachineQuoteDeatil 
where 客户产成品编号='{0}' and 版本='{1}' and 客户名称='{2}'",
customerProductNumber, version, so.CustomerName);
                productNumber = SqlHelper.GetScalar(sql);
                if (string.IsNullOrEmpty(productNumber))
                {
                    error = string.Format("系统不存在该客户产成品编号:{0}，版本:{1}的报价信息,客户{2}", customerProductNumber, version, so.CustomerName);
                    return false;
                }

                sql = string.Format(@"
select ProductNumber from  ProductCustomerProperty 
 where CustomerProductNumber='{0}' and Version='{1}' and CustomerId='{2}'", customerProductNumber, version, so.CustomerId);
                productNumber = SqlHelper.GetScalar(sql);
                if (string.IsNullOrEmpty(productNumber))
                {
                    error = string.Format("系统不存在该客户产成品编号:{0}，版本:{1}的产成品编号,客户{2}", customerProductNumber, version, so.CustomerName);
                    return false;
                }

                if (!ProductManager.HasBOM(productNumber, version))
                {
                    error = string.Format("该产成品编号和版本找不到BOM信息，请先录入BOM！客户产成品编号：{0},产成品编号：{1}，版本：{2}", customerProductNumber, productNumber, version);
                    return false;
                }
            }


            sql = string.Format(" select count(*) from  T_ImpSaleOder_Temp where userId='{0}' and RowNumber='{1}'", userId, dr["行号"]);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("已存在相同行号：{0}", dr["行号"]);
                return false;
            }
            //            if (dr["是否是原材料"].ToString().Equals("是"))
            //            {
            //                sql = string.Format(@" select count(*) from T_ImpSaleOder_Temp where ProductNumber='{0}' and LeadTime='{1}' 
            //  and  IsMateriNumber='是'", productNumber, leadTime);
            //                if (!SqlHelper.GetScalar(sql).Equals("0"))
            //                {
            //                    error = string.Format("已存在相同交期的贸易销售订单明细！重复行: 客户物料号{0},交期{1}", customerProductNumber, leadTime);
            //                    return false;
            //                }
            //            }

            sql = string.Format(@" insert into T_ImpSaleOder_Temp (ProductNumber,version,
CustomerProductNumber,RowNumber,LeadTime,Qty,IsMateriNumber,UserId)
values('{0}','{1}','{2}','{3}','{4}',{5},'{6}','{7}')", productNumber, version, customerProductNumber, dr["行号"], leadTime
 , dr["数量"], dr["是否是原材料"], userId);
            return SqlHelper.ExecuteSql(sql, ref error);

        }

        public static string GetLeadTime(string time)
        {
            //if (time.Contains("/"))
            //{
            //    string[] tempArrary = time.Split('/');
            //    time = string.Format("20{0}-{1}-{2}", tempArrary[2], tempArrary[0], tempArrary[1]);
            //}
            //return time;
            try
            {
                return Convert.ToDateTime(time).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 添加贸易销售订单明细
        /// </summary>
        /// <param name="tod">实体类</param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddTradingOrderDetail(TradingOrderDetail tod, ref string error)
        {
            try
            {
                Convert.ToDateTime(tod.Delivery);
            }
            catch (Exception)
            {
                error = string.Format("交期{0}不是标准日期格式 yyyy-MM-dd ", tod.Delivery);
                return false;
            }

            string sql = string.Format(@"select top 1 MaterialNumber  
from MaterialCustomerProperty where CustomerMaterialNumber='{0}' and CustomerId='{1}'", tod.CustomerMaterialNumber, tod.CustomerId);
            string materialNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(materialNumber))
            {
                error = string.Format("系统不存在该客户原材料编号{0}", tod.CustomerMaterialNumber);
                return false;
            }
            if (string.IsNullOrEmpty(tod.OrdersNumber) || string.IsNullOrEmpty(tod.CustomerMaterialNumber)
                || string.IsNullOrEmpty(tod.RowNumber) || string.IsNullOrEmpty(tod.Delivery)
                || string.IsNullOrEmpty(tod.SN) || string.IsNullOrEmpty(tod.Quantity))
            {
                error = string.Format("信息填写不完整！");
                return false;
            }
            sql = string.Format(@"  select COUNT(*) from TradingOrderDetail where OdersNumber ='{0}' and ProductNumber ='{1}' and RowNumber ='{2}'"
                , tod.OrdersNumber, tod.ProductNumber, tod.RowNumber);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("订单记录已存在");
                return false;
            }
            List<string> sqls = new List<string>();
            sql = string.Format(@"
  select 单价 from V_FindPriceForNewForTrading where 原材料编号='{0}' and 客户编号='{1}' ", materialNumber, tod.CustomerId);
            string unitPrice = SqlHelper.GetScalar(sql);
            unitPrice = string.IsNullOrEmpty(unitPrice) ? "0" : unitPrice;

            sql = string.Format(@" insert into TradingOrderDetail(OdersNumber ,ProductNumber,RowNumber ,Delivery ,SN,CustomerMaterialNumber,
 MaterialName ,Brand ,Quantity ,NonDeliveryQty ,DeliveryQty ,UnitPrice ,TotalPrice ,CreateTime ,Remark )
 select '{0}',MaterialNumber,'{1}','{2}',{3},'{4}',Description ,Brand ,{5},{5},0,{9},{9}*{5},'{6}','{8}'
  from MarerialInfoTable where MaterialNumber ='{7}'", tod.OrdersNumber, tod.RowNumber, tod.Delivery, tod.SN, tod.CustomerMaterialNumber,
   tod.Quantity, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materialNumber, tod.Remark, unitPrice);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        /// <summary>
        /// 批量添加贸易销售订单
        /// </summary>
        /// <param name="tods"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BacthAddTradingOrderDetail(List<TradingOrderDetail> tods, ref string error)
        {
            int i = 0;
            string tempError = string.Empty;
            if (tods.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (TradingOrderDetail tod in tods)
            {
                tempError = "";
                if (!AddTradingOrderDetail(tod, ref tempError))
                {
                    i++;
                    error += string.Format("添加失败：原因--{0}<br/>", tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (tods.Count - i).ToString(), i.ToString(), error);
            }
            string temp = string.Empty;
            string sql = string.Format(@"update TradingOrderDetail set Delivery =convert(char(10),CAST ( Delivery as date ),120)");
            SqlHelper.ExecuteSql(sql, ref temp);
            return result;
        }

        /// <summary>
        /// 添加加工销售订单明细
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddMachineOderDetail(MachineOderDetail mod, ref string error)
        {


            try
            {
                Convert.ToDateTime(mod.LeadTime);
            }
            catch (Exception)
            {
                error = string.Format("交期{0}不是标准日期格式 yyyy-MM-dd ", mod.LeadTime);
                return false;
            }
            if (string.IsNullOrEmpty(mod.OrdersNumber) || string.IsNullOrEmpty(mod.RowNumber)
                || string.IsNullOrEmpty(mod.SN) || string.IsNullOrEmpty(mod.LeadTime)
                || string.IsNullOrEmpty(mod.CustomerProductNumber) || string.IsNullOrEmpty(mod.Qty)
                || string.IsNullOrEmpty(mod.CustomerId))
            {
                error = string.Format("信息填写不完整！");
                return false;
            }
            string sql = string.Format(@" select ProductNumber   from ProductCustomerProperty 
where CustomerProductNumber ='{0}' and CustomerId='{1}' and Version='{2}' ", mod.CustomerProductNumber, mod.CustomerId, mod.Version);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count == 0)
            {
                error = string.Format("系统不存在该客户产成品编号：{0},版本：{1}", mod.CustomerProductNumber, mod.Version);
                return false;
            }

            string productNumber = dt.Rows[0]["ProductNumber"].ToString();
            string version = mod.Version;
            sql = string.Format(@"  select COUNT (*) from  MachineOderDetail where OdersNumber ='{0}' and ProductNumber ='{1}' and Version ='{2}'
  and RowNumber ='{3}'", mod.OrdersNumber, productNumber, version, mod.RowNumber);

            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("订单记录已存在");
                return false;
            }
            List<string> sqls = new List<string>();
            sql = string.Format(@"  select UnitPrice  from  V_FindPriceForNew 
where ProductNumber ='{0}' and Version ='{1}' and CustomerId='{2}' ", productNumber, version, mod.CustomerId);

            string salesQuotation = SqlHelper.GetScalar(sql);
            salesQuotation = string.IsNullOrEmpty(salesQuotation) ? "0" : salesQuotation;

            sql = string.Format(@" insert into MachineOderDetail (OdersNumber ,ProductNumber ,Version ,LeadTime ,RowNumber ,SN ,CustomerProductNumber ,Qty 
 ,NonDeliveryQty ,DeliveryQty ,UnitPrice ,SumPrice ,CreateTime,remark )
 select '{0}',ProductNumber,Version ,'{1}','{2}',{3},'{4}',{5},{5},0,{10},{10}*{5},'{6}','{9}'
  from Product where ProductNumber ='{7}' and Version ='{8}'", mod.OrdersNumber, mod.LeadTime, mod.RowNumber, mod.SN, mod.CustomerProductNumber, mod.Qty
 , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), productNumber, version, mod.Remark, salesQuotation);

            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 批量添加加工销售订单
        /// </summary>
        /// <param name="tods"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BacthAddMachineOderDetail(List<MachineOderDetail> mods, ref string error)
        {
            int i = 0;
            string tempError = string.Empty;
            if (mods.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (MachineOderDetail mod in mods)
            {
                tempError = "";
                if (!AddMachineOderDetail(mod, ref tempError))
                {
                    i++;
                    error += string.Format("添加失败：原因--{0}<br/>", tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (mods.Count - i).ToString(), i.ToString(), error);
            }

            string sql = string.Format(@"  
update MachineOderDetail set LeadTime =convert(char(10),CAST ( LeadTime as date ),120)");
            string temp = "";
            SqlHelper.ExecuteSql(sql, ref temp);
            return result;
        }

        /// <summary>
        /// 根据订单ID获取客户ID
        /// </summary>
        /// <param name="ordersNumber"></param>
        /// <returns></returns>
        public static string GetCustomerIdByOrdersNumber(string ordersNumber)
        {
            string CustomerId = string.Empty;
            string sql = string.Format(@"
select CustomerId from SaleOder where OdersNumber='{0}'", ordersNumber);
            return SqlHelper.GetScalar(sql);
        }

        public static void DatatRecovery()
        {
            string sql = @"update MachineOderDetail set NonDeliveryQty=0,DeliveryQty=Qty,Status='已完成'
where NonDeliveryQty<0";
            SqlHelper.ExecuteSql(sql);
        }
    }
}
