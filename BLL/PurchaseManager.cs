using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;

namespace BLL
{
    public class PurchaseManager
    {
        /// <summary>
        /// 审核采购订单
        /// </summary>
        /// <param name="OrdersNumbers">订单编号集合</param>
        /// <param name="userNumber">当前登录用户k</param>
        /// <returns></returns>
        public static string AuditorPurchase(string OrdersNumbers, string userNumber)
        {
            string error = string.Empty;
            List<string> sqls = new List<string>();
            string sql = string.Format(@" update CertificateOrders set CheckTime ='{0}' ,Auditor ='{1}'
where OrdersNumber in({2})  "
, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userNumber, OrdersNumbers);
            sqls.Add(sql);
            string[] numbers = OrdersNumbers.Split(',');
            foreach (string number in numbers)
            {
                sql = string.Format(" select PaymentMode  from CertificateOrders where OrdersNumber ={0}  ", number);
                sql = GeneratingCopeSql(SqlHelper.GetScalar(sql), number); //获取产生应付的sql语句
                if (!string.IsNullOrEmpty(sql))
                {
                    sqls.Add(sql);
                }
            }
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }
        /// <summary>
        /// 判断是否存在该采购订单明细
        /// </summary>
        /// <param name="certificateOrders"></param>
        /// <param name="materialNumber"></param>
        /// <param name="leadTime"></param>
        /// <returns></returns>
        public static bool IsExitForCertificateOrdersDetail(string ordersNumber, string materialNumber, string leadTime)
        {
            string sql = string.Format(@" select COUNT (*) from CertificateOrdersDetail
where OrdersNumber ='{0}' and MaterialNumber ='{1}' and LeadTime ='{2}' ", ordersNumber, materialNumber, leadTime);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }

        /// <summary>
        /// 获取产生应付sql语句
        /// </summary>
        /// <param name="PaymentMode">付款方式</param>
        /// <param name="ordersNumber">订单编号</param>
        /// <returns></returns>
        public static string GeneratingCopeSql(string PaymentMode, string ordersNumber)
        {
            string sql = string.Empty;
            switch (PaymentMode.ToUpper())
            {
                //                //预付全款
                //                case "YFQK":
                //                    sql = string.Format(@" 
                //insert into AccountsPayable (OrdersNumber ,MaterialNumber ,CreateTime ,SumPrice,SupplierId )
                // select co.OrdersNumber,'','{0}',SUM ( coe .SumPrice) , co.SupplierId  from 
                // CertificateOrders co inner join CertificateOrdersDetail coe on co.OrdersNumber=coe.OrdersNumber
                // where co.OrdersNumber ={1} group by co.OrdersNumber,co.SupplierId", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ordersNumber);
                //                    break;
                //                //预付部分货款 从供应商表查询预付百分比
                //                case "YFBF":
                //                    sql = string.Format(@" insert into AccountsPayable (OrdersNumber ,MaterialNumber ,CreateTime ,SumPrice,SupplierId )
                // select co.OrdersNumber,'','{0}',SUM ( coe .SumPrice)*CAST (si.PercentageInAdvance as decimal(18,2)) , co.SupplierId  from  CertificateOrders co 
                // inner join CertificateOrdersDetail coe on co.OrdersNumber=coe.OrdersNumber
                // left join SupplierInfo si on si.SupplierId =co.SupplierId 
                //  where co.OrdersNumber ={1}  group by co.OrdersNumber,co.SupplierId,si.PercentageInAdvance", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ordersNumber);
                //                    break;
                case "YFQK":
                    sql = string.Format(@"
insert into T_AccountsPayable_Main (OrdersNumber ,CreateTime ,CGHTNumber,SumPrice ,SupplierId ,PaymentMode,PaymentTypes,YFOne ,YFTwo,IsPrepaid )
select a.OrdersNumber,CONVERT (varchar(100),GETDATE(),120),co.HTNumber ,a.订单总价,co.SupplierId,co.PaymentMode ,si.PayType,a.预付一,a.预付二,'是'  from (
select cod.OrdersNumber, SUM(cod.SumPrice) 订单总价,SUM(cod.SumPrice) 预付一 ,0 预付二  from 
 CertificateOrdersDetail cod
where cod.OrdersNumber in({0})
group by cod.OrdersNumber
) a inner join CertificateOrders co on a.OrdersNumber =co.OrdersNumber 
inner join SupplierInfo si on co.SupplierId =si.SupplierId ", ordersNumber);
                    break;
                case "YFBF": //预付部分货款
                    sql = string.Format(@"
insert into T_AccountsPayable_Main (OrdersNumber ,CreateTime ,CGHTNumber,SumPrice ,SupplierId ,PaymentMode,PaymentTypes,YFOne ,YFTwo,IsPrepaid )
select a.OrdersNumber,CONVERT (varchar(100),GETDATE(),120),co.HTNumber ,a.订单总价,co.SupplierId,co.PaymentMode ,si.PayType,a.预付一,a.预付二,'是'  from (
select cod.OrdersNumber, SUM(cod.SumPrice) 订单总价,SUM ( cod.PayOne) 预付一 ,sum(cod.PayTwo) 预付二  from 
 CertificateOrdersDetail cod
where cod.OrdersNumber in({0})
group by cod.OrdersNumber
) a inner join CertificateOrders co on a.OrdersNumber =co.OrdersNumber 
inner join SupplierInfo si on co.SupplierId =si.SupplierId ", ordersNumber);
                    break;

            }
            return sql;
        }

        /// <summary>
        /// 获取采购订单审核状态
        /// </summary>
        /// <param name="ordersNumber"></param>
        /// <returns></returns>
        public static bool GetCheckStatus(string ordersNumber)
        {
            string sql = string.Format(@"		select ISNULL (CheckTime ,'未审核') as 审核状态  from CertificateOrders where OrdersNumber ='{0}'", ordersNumber);
            return SqlHelper.GetScalar(sql) == "未审核" ? false : true;
        }


        /// <summary>
        /// 导入采购订单明细
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordersNumber"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddCertificateOrdersDetail(DataRow dr, string ordersNumber, ref string error)
        {
            string leadTime = string.Empty;
            try
            {
                leadTime = Convert.ToDateTime(dr["交期"]).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                error = string.Format(" 此交期{0}不是正规的日期格式，日期格式应为'yyyy-MM-dd'", dr["交期"]);
                return false;
            }

            //序号 供应商物料编号 交期 数量
            string sql = string.Format(@" select MaterialNumber from MaterialSupplierProperty where SupplierMaterialNumber='{0}'", dr["供应商物料编号"]);
            string materialNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(materialNumber))
            {
                error = string.Format(" 系统不存在该供应商物料编号：{0} ", dr["供应商物料编号"]); ;
                return false;
            }
            sql = string.Format(@"select COUNT(*) from CertificateOrdersDetail 
 where OrdersNumber ='{0}' and MaterialNumber='{1}' and LeadTime =convert(varchar,CAST( '{2}' as date),120) ",
 ordersNumber, materialNumber, leadTime);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("已存在相同记录！采购订单号：{0},供应商物料编号:{1},交期:{2}", ordersNumber, dr["供应商物料编号"], leadTime);
                return false;
            }
            sql = string.Format(@"insert into CertificateOrdersDetail (OrdersNumber,MaterialNumber ,
LeadTime ,RowNumber ,SupplierMaterialNumber ,OrderQty ,
 NonDeliveryQty ,DeliveryQty ,UnitPrice ,SumPrice ,Status )
 select '{0}','{1}',convert(varchar,CAST( '{2}' as date),120),{3},'{4}',{5},{5},0,ProcurementPrice,{5}*ProcurementPrice,'未完成' 
 from MarerialInfoTable where MaterialNumber='{1}'", ordersNumber, materialNumber, leadTime, dr["序号"], dr["供应商物料编号"]
 , dr["数量"]);
            return SqlHelper.ExecuteSql(sql, ref error);
        }



        /// <summary>
        /// 导入采购订单
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpCertificateOrdersList(FileUpload FU_Excel, HttpServerUtility server, string ordersNumber, ref string error)
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
                if (!AddCertificateOrdersDetail(dr, ordersNumber, ref tempError))
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
        /// 导入采购订单
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpCertificateOrdersList_New(FileUpload FU_Excel, string userId, HttpServerUtility server, ref string error)
        {
            string sql = string.Format(" delete T_CertificateOrders where userId='{0}'", userId);
            SqlHelper.ExecuteSql(sql);
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
                if (!AddCertificateOrdersDetail_New(dr, userId, ref tempError))
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
            int a = 1;
            string cgOrderNumber = string.Empty;
            List<string> sqls = new List<string>();
            sql = string.Format(@"select HTNumber ,OrdersDate,PaymentMode,SupplierId ,ContactId  from T_CertificateOrders group by HTNumber ,OrdersDate,PaymentMode,SupplierId ,ContactId ");
            DataTable dtNew = SqlHelper.GetTable(sql);
            foreach (DataRow drNew in dtNew.Rows)
            {
                cgOrderNumber = "CG" + DateTime.Now.AddSeconds(a).ToString("yyyyMMddHHmmss");
                a++;
                sql = string.Format(@"insert into CertificateOrders (OrdersNumber ,HTNumber ,OrdersDate,
PaymentMode ,SupplierId ,ContactId ,OrderStatus,CreateTime )
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", cgOrderNumber, drNew["HTNumber"]
, drNew["OrdersDate"], drNew["PaymentMode"], drNew["SupplierId"], drNew["ContactId"], "未完成", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqls.Add(sql);
                sql = string.Format(@" insert into CertificateOrdersDetail 
(OrdersNumber,MaterialNumber ,LeadTime,SupplierMaterialNumber ,OrderQty ,NonDeliveryQty ,DeliveryQty ,UnitPrice ,
SumPrice,UnitPrice_C,SumPrice_C
,MinOrderQty ,PayOne ,PayTwo )
select '{0}',tco.MaterialNumber ,tco.LeadTime ,tco.SupplierMaterialNumber,tco.OrderQty ,tco.OrderQty ,0,msp.Prcie
,tco.OrderQty *msp.Prcie ,msp.Prcie *1.17,tco.OrderQty *msp.Prcie *1.17,tco.MinOrderQty ,tco.PayOne ,tco.PayTwo  
from T_CertificateOrders tco inner join MaterialSupplierProperty msp on tco.SupplierMaterialNumber=msp.SupplierMaterialNumber
and tco.SupplierId =msp.SupplierId 
where tco.HTNumber ='{1}' and tco.OrdersDate ='{2}' and tco.PaymentMode='{3}' and tco.SupplierId ='{4}' and tco.ContactId ='{5}' and tco.userId='{6}'"
   , cgOrderNumber, drNew["HTNumber"]
, drNew["OrdersDate"], drNew["PaymentMode"], drNew["SupplierId"], drNew["ContactId"], userId);
                sqls.Add(sql);
            }
            sql = string.Format(@"delete CertificateOrders where OrdersNumber not in (
 select  distinct OrdersNumber  from CertificateOrdersDetail)");
            sqls.Add(sql);
            string tmeptempErrpr = string.Empty;
            bool resulTwo = SqlHelper.BatchExecuteSql(sqls, ref tmeptempErrpr);
            if (result && resulTwo)
            { return true; }
            else
            {
                error = error + "<br/>" + tmeptempErrpr;
                return false;
            }




        }


        /// <summary>
        /// 导入采购订单明细
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="ordersNumber"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddCertificateOrdersDetail_New(DataRow dr, string userId, ref string error)
        {
            string htNumber = dr["采购合同号"].ToString();
            string leadTime = string.Empty;
            string ordersDate = string.Empty;
            string paymentMode = "";
            //string ordersNumber = "CG" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string materialNumber = string.Empty;
            string contactId = string.Empty;
            if (string.IsNullOrEmpty(htNumber))
            {
                error = "采购合同号不能为空";
                return false;
            }
            try
            {
                leadTime = Convert.ToDateTime(dr["交期"]).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                error = string.Format(" 此交期{0}不是正规的日期格式，日期格式应为'yyyy-MM-dd'", dr["交期"]);
                return false;
            }

            try
            {
                ordersDate = Convert.ToDateTime(dr["采购订单日期"]).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                error = string.Format(" 采购订单日期{0}不是正规的日期格式，日期格式应为'yyyy-MM-dd'", dr["采购订单日期"]);
                return false;
            }
            //检查是否有该付款方式
            //string sql = string.Format(@" select id from PaymentMode where PaymentMode='{0}' ", paymentMode);
            //paymentMode = SqlHelper.GetScalar(sql);
            //if (string.IsNullOrEmpty(paymentMode))
            //{
            //    error = string.Format("系统不存在该付款方式:{0}", dr["付款方式"]);
            //    return false;
            //}
            string sql = string.Empty;
            sql = string.Format(@"select  count(*) from SupplierInfo where SupplierId='{0}'", dr["供应商编号"]);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("系统不存在该供应商编号:{0}", dr["供应商编号"]);
                return false;
            }

            sql = string.Format(@" select pm.Id from SupplierInfo si   inner join PaymentMode pm on si.PaymentMode=pm.PaymentMode
 where  si.SupplierId ='{0}'", dr["供应商编号"]);
            paymentMode = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(paymentMode))
            {
                error = string.Format("系统不存在该供应商编号：{0}的付款方式", dr["供应商编号"]);
                return false;
            }

            sql = string.Format("select USER_ID from PM_USER where USER_NAME='{0}' ", dr["业务员"]);
            contactId = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(contactId))
            {
                error = string.Format("系统不存在该业务员信息：{0}", dr["业务员"]);
                return false;
            }

            sql = string.Format(@" 
select MaterialNumber from MaterialSupplierProperty 
where SupplierMaterialNumber='{0}' and SupplierId ='{1}' ", dr["供应商物料编号"], dr["供应商编号"]);
            materialNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(materialNumber))
            {
                error = string.Format("系统不存在该供应商物料编号:{0}", dr["供应商物料编号"]);
                return false;
            }
            sql = string.Format(@"select COUNT(*) from T_CertificateOrders
where HTNumber ='{0}' and OrdersDate ='{1}' and PaymentMode='{2}' and SupplierId ='{3}'
and ContactId ='{4}' and SupplierMaterialNumber='{5}' and LeadTime ='{6}' and userId='{7}' "
, htNumber, ordersDate, paymentMode, dr["供应商编号"], contactId, dr["供应商物料编号"], leadTime, userId);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("已存在相同记录！合同号：{0}，供应商物料编号：{1}，交期：{2}", htNumber, dr["供应商物料编号"], leadTime);
                return false;
            }
            sql = string.Format(@"select MinOrderQty  from MaterialSupplierProperty
where MaterialNumber ='{0}' and SupplierId='{1}'", materialNumber, dr["供应商编号"]);
            string minOrderQty = SqlHelper.GetScalar(sql);

            sql = string.Format(@" 
insert into T_CertificateOrders(HTNumber ,OrdersDate,PaymentMode,SupplierId ,ContactId ,SupplierMaterialNumber,MaterialNumber ,
LeadTime ,OrderQty ,MinOrderQty ,PayOne ,PayTwo,UserId )
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},'{9}',{10},{11},'{12}')
", htNumber, ordersDate, paymentMode, dr["供应商编号"], contactId, dr["供应商物料编号"], materialNumber, leadTime, dr["数量"].ToString().Equals("") ? "0" : dr["数量"],
 minOrderQty, dr["付款一"].ToString().Equals("") ? "0" : dr["付款一"], dr["付款二"].ToString().Equals("") ? "0" : dr["付款二"], userId);
            return SqlHelper.ExecuteSql(sql, ref error);

        }



    }
}
