using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;

namespace BLL
{
    public class QutoInfoManager
    {
        public static string Delete(string ids)
        {
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string sql = string.Format(@" delete TradingQuoteDetail  where QuoteNumber in({0})", ids);
            sqls.Add(sql);
            sql = string.Format(@"  delete MachineQuoteDetail where QuoteNumber in({0})", ids);
            sqls.Add(sql);
            sql = string.Format(@"  delete QuoteInfo where QuoteNumber in({0})", ids);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error) == true ? "1" : error;
        }

        #region 绑定产品
        /// <summary>
        /// 绑定产品
        /// </summary>
        /// <param name="drp"></param>
        public static void BindProduct(DropDownList drp)
        {
            string error = string.Empty;
            string sql = string.Format(" select (ProductName+' 版本:'+[Version]) as text ,(ProductNumber+'|'+[Version])as value from Product");
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                drp.DataSource = dt;
                drp.DataValueField = "value";
                drp.DataTextField = "text";
                drp.DataBind();
            }
            drp.Items.Insert(0, new ListItem("- - - 请 选 择 - - -", ""));

        }
        /// <summary>
        /// 通过产品绑定客户产品编号
        /// </summary>
        /// <param name="drpProduct"></param>
        /// <param name="drpCustomerProduct"></param>
        public static void BindCustomerProductNumber(DropDownList drpProduct, DropDownList drpCustomerProduct)
        {
            string error = string.Empty;
            string product = drpProduct.SelectedValue;
            drpCustomerProduct.Items.Clear();
            if (!string.IsNullOrEmpty(product))
            {
                string[] temp = product.Split('|');
                string sql = string.Format(@"
select CustomerProductNumber  from ProductCustomerProperty where ProductNumber ='{0}' and Version ='{1}'", temp[0], temp[1]);
                DataTable dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    drpCustomerProduct.DataSource = dt;
                    drpCustomerProduct.DataValueField = "CustomerProductNumber";
                    drpCustomerProduct.DataTextField = "CustomerProductNumber";
                    drpCustomerProduct.DataBind();
                }
            }
            drpCustomerProduct.Items.Insert(0, new ListItem("- - - 请 选 择 - - -", ""));
        }

        #endregion

        #region 绑定原材料
        /// <summary>
        /// 绑定原材料
        /// </summary>
        /// <param name="drp"></param>
        public static void BindMarerialInfo(DropDownList drp)
        {
            string error = string.Empty;
            string sql = string.Format(" select MaterialNumber,MaterialName from MarerialInfoTable");
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                drp.DataSource = dt;
                drp.DataValueField = "MaterialNumber";
                drp.DataTextField = "MaterialName";
                drp.DataBind();
            } drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));

        }
        /// <summary>
        /// 通过原材料绑定客户物料编号
        /// </summary>
        /// <param name="drpProduct"></param>
        /// <param name="drpCustomerProduct"></param>
        public static void BindCustomerMarerialNumber(DropDownList drpMarerial, DropDownList drpCustomerMarerial)
        {
            string error = string.Empty;
            string marerialNumber = drpMarerial.SelectedValue;
            drpCustomerMarerial.Items.Clear();
            if (!string.IsNullOrEmpty(marerialNumber))
            {
                string sql = string.Format(@" select CustomerMaterialNumber  from MaterialCustomerProperty where MaterialNumber='{0}'", marerialNumber);
                DataTable dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    drpCustomerMarerial.DataSource = dt;
                    drpCustomerMarerial.DataValueField = "CustomerMaterialNumber";
                    drpCustomerMarerial.DataTextField = "CustomerMaterialNumber";
                    drpCustomerMarerial.DataBind();
                }
            }
            drpCustomerMarerial.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));
        }
        #endregion


        /// <summary>
        /// 保存报价单
        /// </summary>
        /// <param name="quteNumber"></param>
        /// <param name="quoteType"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool SaveQuteInfo(string quteNumber, string quoteType, ref string error)
        {
            string quteNumberNew = string.Empty;
            if (quteNumber.Contains("_"))
            {
                string[] temp = quteNumber.Split('_');
                quteNumberNew = temp[0] + "_" + (Convert.ToInt32(temp[1]) + 1).ToString();
            }
            else
            {
                quteNumberNew = quteNumber + "_1";
            }
            string sql = string.Format(" select COUNT (*) from QuoteInfo where QuoteNumber='{0}' ", quteNumberNew);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = "该版本报价单已被保存过，请找到最新版的报价单进行修改保存操作！";
                return false;
            }
            List<string> sqls = new List<string>();
            sql = string.Format(@"insert into QuoteInfo
select '{0}',QuoteTime ,QuoteType ,CustomerId,ContactId ,'{1}',
''  from QuoteInfo where QuoteNumber='{2}'", quteNumberNew, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), quteNumber);
            sqls.Add(sql);
            if (quoteType.Equals("加工报价单"))
            {
                sql = string.Format(@"  insert into MachineQuoteDetail (QuoteNumber,PackageNumber ,ProductNumber ,Version ,CustomerProductNumber 
 ,MaterialNumber,CustomerMaterialNumber,FixedLeadTime ,Hierarchy ,BOMAmount 
 ,MaterialPrcie ,TimeCharge ,Profit ,ManagementPrcie,LossPrcie ,UnitPrice ,Remark 
 ,Description ,SS,BACNumber ,ProductType ,IsOne)
 select '{0}',PackageNumber ,ProductNumber ,Version ,CustomerProductNumber 
 ,MaterialNumber,CustomerMaterialNumber,FixedLeadTime ,Hierarchy ,BOMAmount 
 ,MaterialPrcie ,TimeCharge ,Profit ,ManagementPrcie,LossPrcie ,UnitPrice ,Remark 
 ,Description ,SS,BACNumber ,ProductType ,IsOne from MachineQuoteDetail where QuoteNumber='{1}'", quteNumberNew, quteNumber);
                sqls.Add(sql);
            }
            else
            {

            }
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        public static bool AddQuoteInfoMachine(DataRow dr, string quoteNumber, ref string error)
        {
            string version = dr["版本"].ToString().ToUpper();
            if (string.IsNullOrEmpty(version))
            {
                version = "WU";
            }
            if (version.Equals("OO"))
            {
                version = "00";
            }

            string sql = string.Format(@" select ProductNumber   from ProductCustomerProperty where CustomerProductNumber ='{0}' and version='{1}'"
                    , dr["图纸号"], version);
            string productNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(productNumber))
            {
                error = string.Format("系统不存在该图纸号，请先录入该图纸号:{0},版本{1}", dr["图纸号"], version);
                return false;
            }
            string materiNumber = string.Empty;
            if (dr["是否是原材料"].ToString().Equals("是"))
            {
                sql = string.Format(@"select MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", dr["客户物料号"]);
                materiNumber = SqlHelper.GetScalar(sql);
                if (string.IsNullOrEmpty(materiNumber))
                {
                    error = string.Format("系统不存该客户物料号：{0}", dr["客户物料号"]);
                    return false;
                }
            }


            List<string> sqls = new List<string>();
            //S#	阶层	图纸号	BAC物料号	客户物料号	版本	
            //物料描述	BOM用量	库存数量（理论值）	原材料单价（未税）	
            //原材料采购单价	工时费	利润（未税）	管销研费用（未税）	
            //损耗（未税）	单价（未税）	固定提前期	备注	是否第一级	是否是原材料

            //            sql = string.Format(@"
            //select  COUNT(*) from MachineQuoteDetail where QuoteNumber='{0}' and CustomerProductNumber='{1}'
            //", quoteNumber, dr["图纸号"]);
            //            if (!SqlHelper.GetScalar(sql).Equals("0"))
            //            {
            //                sql = string.Format(@"  
            //delete MachineQuoteDetail where QuoteNumber='{0}' and CustomerProductNumber='{1}' ", quoteNumber, dr["图纸号"]);
            //                sqls.Add(sql);
            //            }


            sql = string.Format(@"insert into MachineQuoteDetail (QuoteNumber ,
SS,Hierarchy,CustomerProductNumber,BACNumber,CustomerMaterialNumber,
Version,Description ,BOMAmount 
,MaterialPrcie,TimeCharge,Profit,ManagementPrcie
,LossPrcie,UnitPrice,FixedLeadTime,Remark ,ProductType,IsOne,IsMaril,ProductNumber,MaterialNumber)
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},{10},{11},{12},{13},{14},'{15}','{16}','{17}','{18}','{19}','{20}','{21}')"
, quoteNumber, dr["S#"], dr["阶层"], dr["图纸号"], dr["BAC物料号"], dr["客户物料号"], version,
dr["物料描述"], dr["BOM用量"],
SrConvert(dr["原材料单价（未税）"]), SrConvert(dr["工时费"]), SrConvert(dr["利润（未税）"]),
SrConvert(dr["管销研费用（未税）"]),
SrConvert(dr["损耗（未税）"]), dr["单价（未税）"], dr["固定提前期"], dr["备注"], "", dr["是否第一级"], dr["是否是原材料"], productNumber, materiNumber);
            sqls.Add(sql);
            //            if (dr["是否是原材料"].ToString().Equals("否"))
            //            {
            //                string productNumber = dt.Rows[0]["ProductNumber"].ToString();
            //                string version = dt.Rows[0]["Version"].ToString();
            //                sql = string.Format(@"
            // update Product set SalesQuotation={2} where ProductNumber ='{0}' and Version ='{1}'", productNumber, version, dr["单价（未税）"]);
            //                sqls.Add(sql);
            //            }
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        private static string SrConvert(object ob)
        {
            if (string.IsNullOrEmpty(ob.ToString()))
            {
                return "0";
            }
            return ob.ToString();
        }

        /// <summary>
        /// 批量导入加工报价单
        /// </summary>
        /// <param name="tods"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BacthAddQuoteInfoMachine(string quteNumber, FileUpload FU_Excel, HttpServerUtility server, ref string error)
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
                if (!AddQuoteInfoMachine(dr, quteNumber, ref tempError))
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




        public static bool AddQuoteInfoMachineForListory(DataRow dr, ref string error, string userId)
        {

            string version = dr["版本"].ToString().ToUpper();
            if (string.IsNullOrEmpty(version))
            {
                version = "WU";
            }
            if (version.Equals("OO"))
            {
                version = "00";
            }
            if (dr["单价（未税）"].ToString().Equals(""))
            {
                error = "单价（未税）不能为空";
                return false;
            }
            string sql = string.Format(@" select ProductNumber   from ProductCustomerProperty where CustomerProductNumber ='{0}' and version='{1}'"
                    , dr["图纸号"], version);
            string productNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(productNumber))
            {
                error = string.Format("系统不存在该图纸号，请先录入该图纸号:{0},版本{1}", dr["图纸号"], version);
                return false;
            }
            sql = string.Format(@" 
select customerName from customer where customerId='{0}'", dr["客户编号"]);

            string customerName = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(customerName))
            {
                error = string.Format("系统不存在该客户编号:{0}", dr["客户编号"]);
                return false;
            }
            string bjTime = "";
            try
            {
                bjTime = dr["报价时间"] == null ?
                   DateTime.Now.AddSeconds(2).ToString("yyyy-MM-dd") : Convert.ToDateTime(dr["报价时间"]).ToString("yyyy-MM-dd");
            }
            catch (Exception ec)
            {
                error = string.Format("报价时间:{0}不是正规的日期格式yyyy-MM-dd", dr["报价时间"]);
                return false;
            }
            //sql = string.Format(@"select COUNT(0) from PM_USER where USER_NAME='{0}'", dr["客户联系人"]);
            //if (SqlHelper.GetScalar(sql).Equals("0"))
            //{
            //    error = string.Format("系统不存该客户联系人：{0}", dr["客户联系人"]);
            //    return false;
            //}

            string materiNumber = string.Empty;
            if (dr["是否是原材料"].ToString().Equals("是"))
            {
                sql = string.Format(@"select MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", dr["客户物料号"]);
                materiNumber = SqlHelper.GetScalar(sql);
                if (string.IsNullOrEmpty(materiNumber))
                {
                    error = string.Format("系统不存该客户物料号：{0}", dr["客户物料号"]);
                    return false;
                }
            }
            else
            {
                sql = string.Format(@"
select count(*) from V_MachineQuoteDetail_Report 
where 客户产成品编号='{0}' 
and 版本='{1}' 
and isone='是' 
and 客户名称='{2}'
and 报价时间='{3}'
group by 客户产成品编号,版本,客户名称,报价时间"
, dr["图纸号"], version, customerName, bjTime);
                string tempcount = SqlHelper.GetScalar(sql);
                //出现报价时间相同的报价单【此处进行异常数据处理】
                if (!string.IsNullOrEmpty(tempcount) && Convert.ToInt32(tempcount) > 1)
                {
                    bjTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                }
            }



            List<string> sqls = new List<string>();
            //S#	阶层	图纸号	BAC物料号	客户物料号	版本	
            //物料描述	BOM用量	库存数量（理论值）	原材料单价（未税）	
            //原材料采购单价	工时费	利润（未税）	管销研费用（未税）	
            //损耗（未税）	单价（未税）	固定提前期	备注	是否第一级	是否是原材料

            //            sql = string.Format(@"
            //select  COUNT(*) from MachineQuoteDetail where QuoteNumber='{0}' and CustomerProductNumber='{1}'
            //", quoteNumber, dr["图纸号"]);
            //            if (!SqlHelper.GetScalar(sql).Equals("0"))
            //            {
            //                sql = string.Format(@"  
            //delete MachineQuoteDetail where QuoteNumber='{0}' and CustomerProductNumber='{1}' ", quoteNumber, dr["图纸号"]);
            //                sqls.Add(sql);
            //            }

            //	客户编号	客户联系人	报价时间


            sql = string.Format(@"insert into [T_MachineQuoteDetail_Temp] ( 
SS,Hierarchy,CustomerProductNumber,BACNumber,CustomerMaterialNumber,
Version,Description ,BOMAmount 
,MaterialPrcie,TimeCharge,Profit,ManagementPrcie
,LossPrcie,UnitPrice,FixedLeadTime,Remark ,ProductType,IsOne,IsMaril,ProductNumber,MaterialNumber,UserId,CustomerId,ContactId,QuteTime)
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',{8},{9},{10},{11},{12},{13},'{14}','{15}',
'{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}')"
, dr["S#"], dr["阶层"], dr["图纸号"], dr["BAC物料号"], dr["客户物料号"], version,
dr["物料描述"], dr["BOM用量"].ToString().Equals("") ? "1" : dr["BOM用量"],
SrConvert(dr["原材料单价（未税）"]), SrConvert(dr["工时费（未税）"]), SrConvert(dr["利润（未税）"]),
SrConvert(dr["管销研费用（未税）"]),
SrConvert(dr["损耗（未税）"]), dr["单价（未税）"], dr["固定提前期"], dr["备注"], "", dr["是否第一级"], dr["是否是原材料"],
productNumber, materiNumber, userId, dr["客户编号"], dr["客户联系人"], bjTime);
            sqls.Add(sql);
            //            if (dr["是否是原材料"].ToString().Equals("否"))
            //            {
            //                string productNumber = dt.Rows[0]["ProductNumber"].ToString();
            //                string version = dt.Rows[0]["Version"].ToString();
            //                sql = string.Format(@"
            // update Product set SalesQuotation={2} where ProductNumber ='{0}' and Version ='{1}'", productNumber, version, dr["单价（未税）"]);
            //                sqls.Add(sql);
            //            }
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 批量导入历史加工报价单
        /// </summary>
        /// <param name="tods"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BacthAddQuoteInfoMachineForListory(FileUpload FU_Excel, HttpServerUtility server, string userId, ref string error)
        {
            string sqlTemp = string.Format(" delete T_MachineQuoteDetail_Temp where userId='{0}' ", userId);
            SqlHelper.ExecuteSql(sqlTemp, ref error);

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
                if (!AddQuoteInfoMachineForListory(dr, ref tempError, userId))
                {
                    i++;
                    error += string.Format("添加失败：原因--{0}<br/>", tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                //error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (dt.Rows.Count - i).ToString(), i.ToString(), error);
                error = string.Format("添加失败！原因：" + error);
                return false;
            }

            //=================================以下将临时表中的数据导入报价单======================================

            List<string> sqls = new List<string>();
            string sql = string.Format(@" select userId,CustomerId,ContactId,QuteTime from T_MachineQuoteDetail_Temp 
where userId='{0}'
group by userId,CustomerId,ContactId,QuteTime ", userId);
            DataTable dtNew = SqlHelper.GetTable(sql);
            string bjNumber = "";
            int h = 1;
            foreach (DataRow drNew in dtNew.Rows)
            {
                bjNumber = "JGBJ" + DateTime.Now.AddSeconds(h).ToString("yyyyMMddHHmmss");
                //开一个报价单
                sql = string.Format(@"insert into QuoteInfo(QuoteNumber,QuoteTime,QuoteType,CustomerId ,ContactId,CreateDateTime ,QuoteUser )
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", bjNumber, drNew["QuteTime"], "加工报价单",
 drNew["CustomerId"], drNew["ContactId"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userId);
                sqls.Add(sql);
                sql = string.Format(@" insert into MachineQuoteDetail (QuoteNumber ,
SS,Hierarchy,CustomerProductNumber,BACNumber,CustomerMaterialNumber,
Version,Description ,BOMAmount 
,MaterialPrcie,TimeCharge,Profit,ManagementPrcie
,LossPrcie,UnitPrice,FixedLeadTime,Remark ,ProductType,IsOne,IsMaril,ProductNumber,MaterialNumber)
select '{4}',SS,Hierarchy,CustomerProductNumber,BACNumber,CustomerMaterialNumber,
Version,Description ,BOMAmount 
,MaterialPrcie,TimeCharge,Profit,ManagementPrcie
,LossPrcie,UnitPrice,FixedLeadTime,Remark ,ProductType,IsOne,IsMaril,ProductNumber,MaterialNumber 
from T_MachineQuoteDetail_Temp where  userId='{0}' and  CustomerId='{1}' and ContactId='{2}' 
and  QuteTime='{3}' ", userId, drNew["CustomerId"], drNew["ContactId"], drNew["QuteTime"], bjNumber);
                sqls.Add(sql);
                h++;
            }
            string tempErrorNew = string.Empty;
            if (SqlHelper.BatchExecuteSql(sqls, ref tempErrorNew))
            {
                return true;
            }
            else
            {
                error = tempErrorNew;
                return false;
            }

        }

        public static bool AddQuoteInfoTradingForListory(DataRow dr, ref string error, string userId)
        {
            string sql = string.Empty;
            string materialNumber = string.Empty;
            if (!dr["客户编号"].ToString().Equals(""))
            {
                sql = string.Format(@"select COUNT(*) from Customer where CustomerId ='{0}'", dr["客户编号"]);
                if (SqlHelper.GetScalar(sql).Equals("0"))
                {
                    error = string.Format("系统不存在该客户编号:{0}", dr["客户编号"]);
                    return false;
                }
            }

            if (dr.Table.Columns.Contains("供应商编号") && !dr["供应商编号"].ToString().Equals(""))
            {
                sql = string.Format(" select COUNT(*) from SupplierInfo where SupplierId ='{0}' ", dr["供应商编号"]);
                if (SqlHelper.GetScalar(sql).Equals("0"))
                {
                    error = string.Format("系统不存在该供应商编号:{0}", dr["供应商编号"]);
                    return false;
                }
            }
            if (!dr["供应商物料编号"].ToString().Equals(""))
            {
                sql = string.Format(@" select COUNT (*) from
MaterialSupplierProperty where SupplierMaterialNumber='{0}' and SupplierId ='{1}'", dr["供应商物料编号"], dr["供应商编号"]);
                if (SqlHelper.GetScalar(sql).Equals("0"))
                {
                    error = string.Format("系统不存在该供应商{0}的供应商物料编号{1}", dr["供应商编号"], dr["供应商物料编号"]);
                    return false;
                }
            }
            if (!dr["客户物料编号"].ToString().Equals(""))
            {
                sql = string.Format(@"  
 select MaterialNumber from MaterialCustomerProperty  where CustomerMaterialNumber='{0}' and CustomerId ='{1}' ", dr["客户物料编号"], dr["客户编号"]);
                materialNumber = SqlHelper.GetScalar(sql);

                if (string.IsNullOrEmpty(materialNumber))
                {
                    error = string.Format("系统不存在该客户{0}的客户物料编号{1}", dr["客户编号"], dr["客户物料编号"]);
                    return false;
                }

            }
            string quteDate = string.Empty;
            try
            {
                quteDate = Convert.ToDateTime(dr["报价日期"]).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                error = string.Format("报价日期：{0}不是标准的日期格式'yyyy-MM-dd'", dr["报价日期"]);
                return false;
            }



            sql = string.Format(@"  
  insert into T_TradingQuoteDetail_Temp (MaterialNumber,CustomerMaterialNumber,FixedLeadTime
 ,UnitPrice,MinPackage,MinMOQ,MaterialDescription,QuoteTime,CustomerId,ContactId,UserId,DW,Remark,SupplierMaterialNumber)
 values('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}') ", materialNumber, dr["客户物料编号"],
dr["交货周期"], dr["单价（未税）"].ToString().Replace('*', ' '), dr["最小包装"], dr["最小起订量"], dr["描述"], quteDate, dr["客户编号"],
dr["客户联系人"], userId, dr["单位"], dr["备注"], dr["供应商物料编号"]);
            return SqlHelper.ExecuteSql(sql, ref error);

        }

        /// <summary>
        /// 批量导入历史贸易报价单
        /// </summary>
        /// <param name="tods"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BacthAddQuoteInfoTradingForListory(FileUpload FU_Excel, HttpServerUtility server, string userId, ref string error)
        {
            string sqlTemp = string.Format(" delete T_TradingQuoteDetail_Temp where userId='{0}' ", userId);
            SqlHelper.ExecuteSql(sqlTemp, ref error);

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
                if (!AddQuoteInfoTradingForListory(dr, ref tempError, userId))
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

            //=================================以下将临时表中的数据导入报价单======================================

            List<string> sqls = new List<string>();
            string sql = string.Format(@" select userId,CustomerId,ContactId,QuoteTime from T_TradingQuoteDetail_Temp 
where userId='{0}'
group by userId,CustomerId,ContactId,QuoteTime ", userId);
            DataTable dtNew = SqlHelper.GetTable(sql);
            string bjNumber = "";
            int h = 1;
            foreach (DataRow drNew in dtNew.Rows)
            {
                bjNumber = "MYBJ" + DateTime.Now.AddSeconds(h).ToString("yyyyMMddHHmmss");
                //开一个报价单
                sql = string.Format(@"insert into QuoteInfo(QuoteNumber,QuoteTime,QuoteType,CustomerId ,ContactId,CreateDateTime ,QuoteUser )
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", bjNumber, drNew["QuoteTime"], "贸易报价单",
 drNew["CustomerId"], drNew["ContactId"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userId);
                sqls.Add(sql);
                sql = string.Format(@" insert into  TradingQuoteDetail (QuoteNumber,ProductNumber,CustomerMaterialNumber,FixedLeadTime
 ,UnitPrice,MinPackage,MinMOQ,MaterialDescription,DW,Remark,SupplierMaterialNumber)
 select '{4}',MaterialNumber,CustomerMaterialNumber,FixedLeadTime
 ,UnitPrice,MinPackage,MinMOQ,MaterialDescription, DW,Remark ,SupplierMaterialNumber from T_TradingQuoteDetail_Temp 
 where  userId='{0}' and  CustomerId='{1}' and ContactId='{2}' 
and  QuoteTime='{3}' ", userId, drNew["CustomerId"], drNew["ContactId"], drNew["QuoteTime"], bjNumber);
                sqls.Add(sql);
                h++;
            }
            string tempErrorNew = string.Empty;
            SqlHelper.BatchExecuteSql(sqls, ref tempErrorNew);
            return result;
        }



        public static bool AddQuoteInfoMY(DataRow dr, string quoteNumber, ref string error)
        {
            string sql = string.Format("select COUNT (*) from MaterialCustomerProperty where CustomerMaterialNumber='{0}' ", dr["客户物料编号"]);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("系统不存在该客户物料编号：{0}", dr["客户物料编号"]);
                return false;
            }
            sql = string.Format(@"   insert into TradingQuoteDetail(QuoteNumber ,
ProductNumber ,CustomerMaterialNumber ,FixedLeadTime ,UnitPrice ,MinPackage
  ,MinMOQ ,MaterialDescription ,Brand,DW ) 
 select '{0}',MaterialNumber,'{1}','{2}',{3},'{4}' ,'{5}',Description ,
Brand ,'{6}' from MarerialInfoTable where MaterialNumber=(
select top 1 MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{1}') ",
 quoteNumber, dr["客户物料编号"], dr["固定提前期"], dr["单价"].ToString().Equals("") ? "0" : dr["单价"].ToString(),
 dr["最小包装"], dr["最小起订量"], dr["单位"]);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 批量导入贸易报价单
        /// </summary>
        /// <param name="tods"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BacthAddQuoteInfoMY(string quteNumber, FileUpload FU_Excel, HttpServerUtility server, ref string error)
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
                if (!AddQuoteInfoMY(dr, quteNumber, ref tempError))
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

    }
}
