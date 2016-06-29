using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;
using Model;
using System.Web.UI.WebControls;
using System.Web;

namespace BLL
{
    public class ProductManager
    {
        private static string sql = string.Empty;
        private static string error = string.Empty;

        public static string GetProductNumbers()
        {
            string ProductNumbers = string.Empty;
            string sql = @"  
select distinct ProductNumber from product ";
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ProductNumbers += dr[0] + ",";
                }
            }
            return ProductNumbers.TrimEnd(',');
        }

        public static string GetVersions()
        {
            string temp = string.Empty;
            string sql = @"  
select distinct Version from product ";
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    temp += dr[0] + ",";
                }
            }
            return temp.TrimEnd(',');
        }

        public static string GetCustomerProductNumbers()
        {
            string temp = string.Empty;
            string sql = @"  
select distinct CustomerProductNumber from ProductCustomerProperty ";
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    temp += dr[0] + ",";
                }
            }
            return temp.TrimEnd(',');
        }

        /// <summary>
        /// 检查某产品是否有BOM
        /// </summary>
        /// <param name="productNumber"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool HasBOM(string productNumber, string version)
        {
            string sql = string.Format(@"  
select SUM(number) from(
select COUNT(0) as number from BOMInfo where ProductNumber = '{0}' and Version = '{1}'
union all
select COUNT(0) from PackageAndProductRelation where PackageNumber = '{0}'
)t
                ", productNumber, version);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }

        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Product ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            Product product = new Product();
            product.ProductNumber = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
            product.Version = dr["Version"] == null ? "" : dr["Version"].ToString();
            product.ProductName = dr["ProductName"] == null ? "" : dr["ProductName"].ToString();
            product.Description = dr["Description"] == null ? "" : dr["Description"].ToString();
            product.Kind = dr["Kind"] == null ? "" : dr["Kind"].ToString();
            product.Type = dr["Type"] == null ? "" : dr["Type"].ToString();
            product.RatedManhour = dr["RatedManhour"] == null ? 0 : Convert.ToDouble(dr["RatedManhour"].ToString());
            product.QuoteManhour = dr["QuoteManhour"] == null ? 0 : Convert.ToDouble(dr["QuoteManhour"].ToString());
            product.CostPrice = Convert.ToDecimal(dr["CostPrice"] == null ? "" : dr["CostPrice"].ToString());
            product.SalesQuotation = Convert.ToDecimal(dr["SalesQuotation"] == null ? "" : dr["SalesQuotation"].ToString());
            product.HalfProductPosition = dr["HalfProductPosition"] == null ? "" : dr["HalfProductPosition"].ToString();
            product.FinishedGoodsPosition = dr["FinishedGoodsPosition"] == null ? "" : dr["FinishedGoodsPosition"].ToString();
            product.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
            product.IsOldVersion = dr["IsOldVersion"] == null ? "" : dr["IsOldVersion"].ToString();
            product.Cargo = dr["Cargo"] == null ? "" : dr["Cargo"].ToString();
            product.Unit = dr["Unit"] == null ? "" : dr["Unit"].ToString();
            product.NumberProperties = dr["NumberProperties"] == null ? "" : dr["NumberProperties"].ToString();
            return product;

        }
        /// <summary>
        /// 检测是否有该编号
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public static bool IsExit(string ProductNumber, string Version)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from Product where ProductNumber='{0}' and Version='{1}'", ProductNumber, Version);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 添加产成品基本信息
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddProduct(Model.Product product, ref string error)
        {
            List<string> sqls = new List<string>();
            if (IsExit(product.ProductNumber, product.Version))
            {
                error = "已存在该编号、版本！请重新填写！";
                return false;
            }

            if (string.IsNullOrEmpty(product.ProductNumber) || string.IsNullOrEmpty(product.Version))
            {
                error = "产成品信息不完整！";
                return false;
            }
            string sql = string.Format(@" insert into Product (ProductNumber,Version,ProductName,Description,RatedManhour,QuoteManhour,
             CostPrice,SalesQuotation,HalfProductPosition,FinishedGoodsPosition,Remark,IsOldVersion,type,Cargo,Unit,NumberProperties) values 
              ('{0}','{1}','{2}','{3}',{4},{5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}')",
                product.ProductNumber, product.Version, product.ProductName, product.Description, product.RatedManhour,
                product.QuoteManhour, product.CostPrice, product.SalesQuotation, product.HalfProductPosition,
                product.FinishedGoodsPosition, product.Remark, product.IsOldVersion, product.Type, product.Cargo, product.Unit, product.NumberProperties);
            sqls.Add(sql);
            sql = string.Format(@"
insert into ProductStock (ProductNumber ,Version ,WarehouseName ,StockQty ,UpdateTime )
values('{0}','{1}','cpk',0,'{2}')", product.ProductNumber, product.Version, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sqls.Add(sql);
            if (product.Type == "包")
            {
                sql = string.Format(@"insert into PackageInfo(PackageNumber,PackageName)values('{0}','{1}')", product.ProductNumber, product.ProductName);
                sqls.Add(sql);
            }
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }
        /// <summary>
        /// 删除产成品信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string Version, string ProductNumber)
        {
            List<string> list = new List<string>();
            //产品客户属性
            string GuidOne = string.Format(@" select guid from ProductCustomerProperty where ProductNumber in ({0}) 
            and Version in ({1})", ProductNumber, Version);
            sql = string.Format(@" delete ProductCustomerProperty where guid in ({0})", GuidOne);
            list.Add(sql);
            //产品图纸属性
            string GuidTwo = string.Format(@" select guid from ProductBlueprintProperty where ProductNumber in ({0})
            and Version in ({1})", ProductNumber, Version);
            sql = string.Format(@" delete ProductBlueprintProperty where guid in ({0})", GuidTwo);
            list.Add(sql);
            //BOM信息表--裁线信息维护表
            string GuidThree = string.Format(@" select guid from BOMInfo where ProductNumber in ({0}) 
            and Version in ({1})", ProductNumber, Version);
            sql = string.Format(@" delete BOMInfo where guid in ({0})", GuidThree);
            list.Add(sql);
            string ProductCuttingLineInfoGuid = " select pc.Guid as guid from dbo.BOMInfo bom inner join ProductCuttingLineInfo pc on bom.ProductNumber=pc.ProductNumber and bom.Version=pc.Version and bom.MaterialNumber=pc.MaterialNumber";
            sql = string.Format(@" delete ProductCuttingLineInfo where guid in ({0})", ProductCuttingLineInfoGuid);
            list.Add(sql);


            //产品工序信息---产品工序系数


            string ProductWorkSnPropertyGuid = "select pc.guid as Guid from ProductWorkSnProperty pp inner join ProductWorkSnCoefficient pc on pp.ProductNumber=pc.ProductNumber and pp.Version=pc.Version and pp.WorkSnNumber=pc.WorkSnNumber";
            sql = string.Format(@" delete ProductWorkSnCoefficient where guid in ({0})", ProductWorkSnPropertyGuid);
            list.Add(sql);
            string GuidFour = string.Format(@" select guid from ProductWorkSnProperty where ProductNumber in ({0}) 
            and Version in ({1})", ProductNumber, Version);
            sql = string.Format(@" delete ProductWorkSnProperty where guid in ({0})", GuidFour);
            list.Add(sql);

            //产品基本信息表

            sql = string.Format(@" delete from Product where ProductNumber in ({0}) 
            and Version in ({1})", ProductNumber, Version);
            list.Add(sql);
            sql = string.Format(@" delete PackageAndProductRelation where ProductNumber in ({0}) and Version in ({1})",
                 ProductNumber, Version);
            list.Add(sql);

            sql = string.Format(@" delete ProductStock  where ProductNumber in ({0}) and Version in ({1}) 
and WarehouseName='cpk'  ", ProductNumber, Version);
            list.Add(sql);
            return SqlHelper.BatchExecuteSql(list, ref error) == true ? "1" : error;

        }

        /// <summary>
        /// 编辑产成品信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditProduct(Product product, ref string error)
        {
            sql = string.Format(@" update Product set Version='{0}',ProductName='{1}',Description='{2}',
           QuoteManhour={3},CostPrice='{4}',SalesQuotation='{5}',HalfProductPosition='{6}',FinishedGoodsPosition='{7}',
            Remark='{8}',IsOldVersion='{9}',type='{10}' ,Cargo='{13}',Unit='{14}',NumberProperties='{15}' where ProductNumber='{11}' and version='{12}'", product.Version, product.ProductName,
            product.Description,
            product.QuoteManhour, product.CostPrice, product.SalesQuotation, product.HalfProductPosition, product.FinishedGoodsPosition,
            product.Remark, product.IsOldVersion, product.Type, product.ProductNumber, product.Version, product.Cargo, product.Unit, product.NumberProperties);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="users"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BatchAddData(List<Product> products, ref string error)
        {
            int i = 0;
            string tempError = string.Empty;
            if (products.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (Product product in products)
            {
                tempError = "";
                if (!AddProduct(product, ref tempError))
                {
                    i++;
                    error += string.Format("产成品{0}&nbsp;&nbsp;添加失败：原因--{1}<br/>", product.ProductName, tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>添加产成品失败:<br/>{2}", (products.Count - i).ToString(), i.ToString(), error);
            }
            return result;
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

        /// <summary>
        /// 不合格品上报单条添加
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddRejectsProduct(DataRow dr, ref string error)
        {
            string version = dr["版本"].ToString().Equals("") ? "WU" : dr["版本"].ToString().ToUpper();
            string sql = string.Format(@"select ProductNumber ,Version 
from ProductCustomerProperty where CustomerProductNumber ='{0}' and Version='{1}'", dr["客户产成品编号"], version);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count == 0)
            {
                error = string.Format("系统不存在该客户产成品编号:{0},版本:{1}", dr["客户产成品编号"], version);
                return false;
            }
            sql = string.Format(@"select COUNT (*) from PM_USER where USER_NAME ='{0}'", dr["姓名"]);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("系统不存在该职工：{0}", dr["姓名"]);
                return false;
            }
            sql = string.Format(@"select COUNT (*) from PM_USER where Team ='{0}'", dr["班组"].ToString().ToUpper());
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("系统不存在该班组：{0}", dr["班组"].ToString().ToUpper());
                return false;
            }
            string productNumber = dt.Rows[0]["ProductNumber"].ToString();
            //version = dt.Rows[0]["Version"].ToString();
            string fxDate = dr["返修日期"].ToString();
            string xhDate = dr["修回检验日期"].ToString();
            try
            {
                fxDate = Convert.ToDateTime(fxDate).ToString("yyyy-MM-dd");
                xhDate = Convert.ToDateTime(xhDate).ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                error = string.Format("日期不是正确的yyyy-MM-dd格式,返修日期:{0},修回检验日期:{1}", dr["返修日期"].ToString(), dr["修回检验日期"].ToString()); ;
                return false;
            }
            sql = string.Format(@"insert into RejectsProduct (ReportTime ,ProductNumber ,Version ,CustomerProductNumber ,
Qty ,RepairReason
,RepairDate,RepairInspectionDate,Name,Team ,Remark ,WhetherBatch)
values('{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}')"
                , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), productNumber, version, dr["客户产成品编号"], dr["数量"].ToString().Equals("") ? "0" : dr["数量"], dr["返修原因"]
, fxDate, xhDate, dr["姓名"], dr["班组"].ToString().ToUpper(), dr["备注"], dr["是否成批"]);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 导入不合格品
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpRejectsProduct(FileUpload FU_Excel, HttpServerUtility server, ref string error)
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
                if (!AddRejectsProduct(dr, ref tempError))
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


        public static bool AddExaminationLog(string year, string month, DataRow dr, ref string error)
        {
            string sql = string.Format(" select count(*) from pm_user where user_name='{0}' ", dr["姓名"]);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("系统不存在该姓名：{0}", dr["姓名"]);
                return false;
            }
            sql = string.Format(@"insert into ExaminationLog(Year ,Month ,Name ,Score,Operation,TotalScore )
values('{0}','{1}','{2}',{3},{4},{3}+{4})", year, month, dr["姓名"], dr["笔试得分"], dr["实操得分"]);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 导入员工考试成绩
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpExaminationLog(string year, string month, FileUpload FU_Excel, HttpServerUtility server, ref string error)
        {
            sql = string.Format("select COUNT(*) from ExaminationLog  where YEAR ='{0}' and MONTH ='{1}' ", year, month);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("已上报过{0}年{1}月的考试成绩", year, month);
                return false;
            }
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
                if (!AddExaminationLog(year, month, dr, ref tempError))
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
        /// 算两时间之间的实际总工时
        /// </summary>
        /// <param name="startTime">实际开始时间</param>
        /// <param name="endTime">实际结束时间</param>
        /// <returns>返回差</returns>
        public static double GetTime(string startTime, string endTime)
        {
            double result = 0;

            DateTime start = Convert.ToDateTime(startTime);
            DateTime end = Convert.ToDateTime(endTime);
            if (start > end) //开始大于结束
            {
                return 0;
            }
            int Deday = (Convert.ToDateTime(end.ToString("yyyy-MM-dd")) - Convert.ToDateTime(start.ToString("yyyy-MM-dd"))).Days;
            if (Deday > 0) //跨天
            {
                if (Deday == 1)
                {
                    double one = 0.00;
                    if (start > Convert.ToDateTime(end.ToString("yyyy-MM-dd 17:30:00")))
                    {
                        one = 0;
                    }
                    else
                    {
                        one = GetTimeForNK(startTime, end.AddDays(-1).ToString("yyyy-MM-dd 17:30:00"));
                    }
                    double two = GetTimeForNK(end.ToString("yyyy-MM-dd 08:30:00"), endTime);
                    result = one + two;
                }
                else
                {
                    double one = GetTimeForNK(startTime, start.ToString("yyyy-MM-dd 17:30:00"));
                    double two = GetTimeForNK(end.ToString("yyyy-MM-dd 08:30:00"), endTime);
                    result = one + two + (Deday - 1) * 8;
                }
            }
            else //不跨天
            {
                result = GetTimeForNK(startTime, endTime);
            }
            return result;
        }

        /// <summary>
        /// 计算不跨天的情况
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static double GetTimeForNK(string startTime, string endTime)
        {
            double result = 0;
            DateTime sbTime = Convert.ToDateTime("08:30:00"); //上班时间
            DateTime zxTime = Convert.ToDateTime("12:00:00"); //中午休息时间开始
            DateTime xxTime = Convert.ToDateTime("13:00:00"); //中午休息时间结束
            DateTime xbTime = Convert.ToDateTime("17:30:00"); //下班时间
            DateTime start = Convert.ToDateTime(startTime);
            DateTime end = Convert.ToDateTime(endTime);
            DateTime tempStartTime = Convert.ToDateTime(start.ToString("HH:mm:ss")); //开始时间
            DateTime tempEndTime = Convert.ToDateTime(end.ToString("HH:mm:ss")); //结束时间
            if (tempStartTime < sbTime)
            {
                tempStartTime = sbTime;
            }
            if (tempEndTime > xbTime)
            {
                tempEndTime = xbTime;
            }
            if (tempStartTime >= zxTime && tempStartTime <= xxTime && tempEndTime >= zxTime && tempEndTime <= xxTime)
            {
                result = 0;
            }
            else if (tempStartTime >= sbTime && tempEndTime <= zxTime) //上班到中午休息
            {
                result = (tempEndTime - tempStartTime).TotalHours;
            }
            else if (tempStartTime >= xxTime && tempEndTime <= xbTime) //下午13点到下班之内
            {
                result = (tempEndTime - tempStartTime).TotalHours;
            }
            else if (tempStartTime >= sbTime && tempEndTime >= xxTime && tempEndTime <= xxTime)//结束时间在12点到13点
            {
                result = (zxTime - tempStartTime).TotalHours;
            }
            else if (tempStartTime >= zxTime && tempStartTime <= xxTime && tempEndTime <= xbTime) //开始时间在午休之间
            {
                result = (tempEndTime - xxTime).TotalHours;
            }
            else if (tempStartTime >= sbTime && tempStartTime <= zxTime && tempEndTime >= xxTime && tempEndTime <= xbTime)
            {
                result = (tempEndTime - tempStartTime).TotalHours - 1;
            }
            return result;
        }


        /// <summary>
        /// 添加产成品基本信息
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddProduct(DataRow dr, ref string error)
        {

            string version = dr["版本"].ToString().Trim().Equals("") ? "WU" : dr["版本"].ToString().ToUpper();
            if (version.Equals("OO"))
            {
                version = "00";
            }
            List<string> sqls = new List<string>();
            //客户产成品编号（图纸号）	客户编号	产成品编号(自己的号)	版本	型号	成品类别	描述
            string sql = string.Format(@"
 select count(*) from ProductCustomerProperty where CustomerProductNumber ='{0}' and CustomerId='{1}' and Version='{2}' ", dr["客户产成品编号（图纸号）"], dr["客户编号"], version);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("系统已存在该图纸号：{0},版本：{1}", dr["客户产成品编号（图纸号）"], version);
                return false;
            }
            sql = string.Format(@"
select count(0) from ProductCustomerProperty    where ProductNumber ='{0}' and Version ='{1}' and  CustomerProductNumber='{2}'
", dr["产成品编号(自己的号)"], version, dr["客户产成品编号（图纸号）"]);
            if (SqlHelper.GetScalar(sql) != "0")
            {
                error = string.Format("系统已存在该图纸号：{0} ", dr["客户产成品编号（图纸号）"]);
                return false;
            }

            sql = string.Format("  select COUNT(*) from Product where ProductNumber ='{0}' and Version ='{1}' ", dr["产成品编号(自己的号)"], version);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {

                sql = string.Format(@" insert into Product (ProductNumber,Version,ProductName ,Type,Description )
 values('{0}','{1}','{2}','{3}','{4}')", dr["产成品编号(自己的号)"], version, dr["型号"], dr["成品类别"], dr["描述"]);
                sqls.Add(sql);
            }
            sql = string.Format(@"
 insert into ProductCustomerProperty(ProductNumber ,Version ,CustomerProductNumber,CustomerId )
 values('{0}','{1}','{2}','{3}')", dr["产成品编号(自己的号)"], version, dr["客户产成品编号（图纸号）"], dr["客户编号"]);
            sqls.Add(sql);

            sql = string.Format(" select COUNT(*) from ProductStock where ProductNumber ='{0}' and Version ='{1}' "
                , dr["产成品编号(自己的号)"], version);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                sql = string.Format(@" insert into ProductStock (ProductNumber ,Version ,WarehouseName ,StockQty ,UpdateTime )
 values('{0}','{1}','cpk',0,'{2}')", dr["产成品编号(自己的号)"], version, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqls.Add(sql);
            }

            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        /// <summary>
        /// 导入产成品基本信息
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpProduct(FileUpload FU_Excel, HttpServerUtility server, ref string error)
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
                if (!AddProduct(dr, ref tempError))
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


        public static bool AddTwoBom(DataRow dr, ref string error)
        {
            string version = dr["版本"].ToString().Trim().Equals("") ? "WU" : dr["版本"].ToString().ToUpper();

            if (version.Equals("OO"))
            {
                version = "00";
            }
            //客户产成品编号（图纸号）	客户物料号	单机用量	单位
            string sql = string.Format(@" 
 select ProductNumber  from ProductCustomerProperty where CustomerProductNumber ='{0}' and Version='{1}' ", dr["客户产成品编号（图纸号）"], version);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count == 0)
            {
                error = string.Format("系统不存在该图纸号：{0},版本：{1}", dr["客户产成品编号（图纸号）"], version);
                return false;
            }
            string productNumber = dt.Rows[0]["ProductNumber"].ToString();

            sql = string.Format(@"  select MaterialNumber from MaterialCustomerProperty 
where CustomerMaterialNumber ='{0}' ", dr["客户物料号"]);
            string materialNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(materialNumber))
            {
                error = string.Format("系统不存在该客户物料号：{0}。图纸号：{1}", dr["客户物料号"], dr["客户产成品编号（图纸号）"]);
                return false;
            }
            // sql = string.Format(@"select Kind  from MarerialInfoTable where MaterialNumber='{0}'", materialNumber);
            string js = "1";
            if (dr["单位"].Equals("mm"))
            {
                js = "CAST( 1000 as decimal(18,5))";
            }
            List<string> sqls = new List<string>();
            sql = string.Format(@"insert BOMInfo(ProductNumber ,Version ,MaterialNumber ,SingleDose ,CustomerMaterialNumber,
Unit,CustomnerProductNumber )
 values('{0}','{1}','{2}',cast( {3}/{7} as decimal(18,5)),'{4}','{5}','{6}') ",
productNumber, version, materialNumber,
dr["单机用量"].ToString().Equals("") ? "1" : dr["单机用量"],
dr["客户物料号"], dr["单位"], dr["客户产成品编号（图纸号）"], js);
            sqls.Add(sql);

            ////改变成本价
            //sql = new BLL.ToolChangeProduct().changeProductCostPrice(productNumber, version);
            //sqls.Add(sql);
            //            sql = string.Format(@" 
            //select kind from  MarerialInfoTable where  materialNumber='{0}' ", materialNumber);
            //            if (SqlHelper.GetScalar(sql).Equals("线材")) //增加裁线信息 
            //            {
            //                sql = string.Format(@"
            //insert into ProductCuttingLineInfo (ProductNumber ,Version ,MaterialNumber ,Length ,Qty )
            //values('{0}','{1}','{2}','{3}',1)", productNumber, version, materialNumber, dr["单机用量"].ToString().Equals("") ? "1" : dr["单机用量"]);
            //                sqls.Add(sql);
            //            }
            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            string temp = string.Empty;
            if (!result)
            {
                foreach (string str in sqls)
                {
                    temp += string.Format("<br/>{0}", str);
                }
                error = string.Format("具体执行命令:{2},图纸号：{0},版本：{1}", dr["客户产成品编号（图纸号）"], version, temp);
            }
            return result;
        }


        /// <summary>
        /// 导入2级BOM
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpProductTwoBOM(FileUpload FU_Excel, HttpServerUtility server, ref string error)
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
                if (!AddTwoBom(dr, ref tempError))
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


        public static bool AddThreeBom(DataRow dr, ref string error)
        {
            string version = dr["版本"].ToString().Trim().Equals("") ? "WU" : dr["版本"].ToString().ToUpper();
            if (version.Equals("OO"))
            {
                version = "00";
            }
            string sql = string.Format(@"
 select ProductNumber,Version  from ProductCustomerProperty where CustomerProductNumber ='{0}'", dr["包号(客户的号)"]);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count == 0)
            {
                error = string.Format("系统不存在该包号：{0}", dr["包号(客户的号)"]);
                return false;
            }
            string BH = dt.Rows[0]["ProductNumber"].ToString();
            sql = string.Format(@"
 select ProductNumber  from ProductCustomerProperty where CustomerProductNumber ='{0}' and Version='{1}'", dr["客户产成品编号(图纸号)"], version);
            dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count == 0)
            {
                error = string.Format("系统不存在该图纸号：{0},版本：{1}。包号：{2}", dr["客户产成品编号(图纸号)"], version, dr["包号(客户的号)"]);
                return false;
            }
            string productNumber = dt.Rows[0]["ProductNumber"].ToString();

            //            sql = string.Format(@" select COUNT(*) from PackageAndProductRelation where PackageNumber='{0}' and ProductNumber ='{1}'
            // and Version ='{2}' and SingleDose ={3}", BH, productNumber, version, dr["单机"]);
            //            if (!SqlHelper.GetScalar(sql).Equals("0"))
            //            {
            //                error = string.Format("已存在相同记录,包号：{0}", dr["包号(客户的号)"]); ;
            //                return false;
            //            }
            sql = string.Format(@" insert into PackageAndProductRelation (PackageNumber ,ProductNumber ,Version ,SingleDose )
 values('{0}','{1}','{2}',{3})  ", BH, productNumber, version,
                                 dr["单机"].ToString().Equals("") ? "1" : dr["单机"]);
            bool result = SqlHelper.ExecuteSql(sql, ref error);

            string temp = string.Empty;
            if (!result)
            {

                temp += string.Format("<br/>{0}", sql);
                error = string.Format("具体执行语句：{3},图纸号：{0},版本：{1}。包号：{2}", dr["客户产成品编号(图纸号)"], version, dr["包号(客户的号)"], temp);
                // error += string.Format("具体执行语句：{0},图纸号：{1},版本：{2}", temp, dr["图纸号"], version);
            }
            return result;


        }

        /// <summary>
        /// 导入3级BOM
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpProductThreeBOM(FileUpload FU_Excel, HttpServerUtility server, ref string error)
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
                if (!AddThreeBom(dr, ref tempError))
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
        // select ProductNumber ,Version  from ProductCustomerProperty where CustomerProductNumber=''


        public static bool AddWorkSn(DataRow dr, ref string error)
        {
            string columName = string.Empty;
            string version = dr["版本"].ToString().Trim().Equals("") ? "WU" : dr["版本"].ToString().ToUpper();
            string snNumber = string.Empty;
            string sn = string.Empty;
            if (version.Equals("OO"))
            {
                version = "00";
            }
            string sql = string.Format(@" 
         select ProductNumber  from ProductCustomerProperty where CustomerProductNumber ='{0}' and Version='{1}' ", dr["图纸号"], version);
            string productNumber = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(productNumber))
            {
                error = string.Format("系统不存在该图纸号：{0},版本：{1}", dr["图纸号"], version);
                return false;
            }
            List<string> sqls = new List<string>();
            sql = string.Format("select WorkSnNumber,WorkSnName,Sn  from WorkSn");
            foreach (DataRow drTemp in SqlHelper.GetTable(sql).Rows)
            {
                columName = drTemp["WorkSnName"].ToString();
                if (dr.Table.Columns.Contains(columName) && !dr[columName].ToString().Trim().Equals(""))//如果有工时
                {
                    //                    sql = string.Format(@"select COUNT(*) from ProductWorkSnProperty 
                    //where ProductNumber='{0}' and Version ='{1}' and WorkSnNumber='{2}'", productNumber, version, drTemp["WorkSnNumber"].ToString());
                    //                    if (SqlHelper.GetScalar(sql).Equals("0"))
                    //                    {
                    //                        sqls.Add(GetInsertSqlForWorkSn(productNumber, version, drTemp["WorkSnNumber"].ToString(), drTemp["Sn"].ToString(), dr[columName].ToString()));
                    //                    }
                    //将原有数据库删除
                    sql = string.Format(@"delete ProductWorkSnProperty 
where ProductNumber='{0}' and Version ='{1}' and WorkSnNumber='{2}'", productNumber, version, drTemp["WorkSnNumber"].ToString());
                    sqls.Add(sql);
                    sqls.Add(GetInsertSqlForWorkSn(productNumber, version, drTemp["WorkSnNumber"].ToString(), drTemp["Sn"].ToString(), dr[columName].ToString()));

                }
            }
            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            string temp = string.Empty;
            if (!result)
            {
                foreach (string str in sqls)
                {
                    temp += string.Format("<br/>{0}", str);
                }
                error += string.Format("具体执行语句：{0},图纸号：{1},版本：{2}", temp, dr["图纸号"], version);
            }
            return result;

        }

        private static string GetInsertSqlForWorkSn(string prdouctNumber, string version, string snNumber, string sn, string hour)
        {
            sql = string.Format(@" insert into ProductWorkSnProperty(ProductNumber ,Version ,WorkSnNumber,RowNumber ,RatedManhour)
values('{0}','{1}','{2}',{3},{4}) ", prdouctNumber, version, snNumber, sn, hour);
            return sql;
        }


        /// <summary>
        /// 导入工序工时
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpProductWorkSN(FileUpload FU_Excel, HttpServerUtility server, ref string error)
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
                if (!AddWorkSn(dr, ref tempError))
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
            sql = string.Format(@"update  Product set RatedManhour =ISNULL (a.总工时,0) from Product p left join (
select ProductNumber ,Version,SUM(RatedManhour) as 总工时 from ProductWorkSnProperty group by ProductNumber ,Version 
) a on p.ProductNumber=a.ProductNumber and p.Version =a.Version ");
            SqlHelper.ExecuteSql(sql);
            return result;

        }

        /// <summary>
        /// 根据工序名称获取工序编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSnNumberForName(string name)
        {
            string sql = string.Format(@"select WorkSnNumber from WorkSn where WorkSnName='{0}'", name);
            return SqlHelper.GetScalar(sql);
        }
        /// <summary>
        /// 根据工序名称获取工序序号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetSnForName(string name)
        {
            string sql = string.Format(@"select sn from WorkSn where WorkSnName='{0}'", name);
            return SqlHelper.GetScalar(sql);
        }

        public static void WriteAverageForYear(string year, string type, string isPorduct, string sql, string userNameColmName)
        {
            string error = string.Empty;
            List<string> sqls = new List<string>();
            string updateSql = string.Format(@" delete T_TempScore where YEAR ='{0}' 
and Type='{1}' and IsProduct='{2}' ", year, type, isPorduct);
            sqls.Add(updateSql);
            updateSql = string.Format(@" insert into T_TempScore(Year,UserName,Average,Type,IsProduct)
            select t.年份,t.{1},t.平均分新,'{2}','{3}' from ({0}) t", sql, userNameColmName, type, isPorduct);
            sqls.Add(updateSql);
            SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        //        /// <summary>
        //        /// 记录统计结果
        //        /// </summary>
        //        /// <param name="year"></param>
        //        /// <param name="type"></param>
        //        /// <param name="isPorduct"></param>
        //        /// <param name="sql"></param>
        //        /// <param name="userNameColmName"></param>
        //        public static void WriteAverageForYear(string year, string type, string isPorduct, string sql, string userNameColmName)
        //        {
        //            string error = string.Empty;
        //            List<string> sqls = new List<string>();
        //            string updateSql = string.Format(@" delete T_TempScore where YEAR ='{0}' 
        //and Type='{1}' and IsProduct='{2}' ", year, type, isPorduct);
        //            sqls.Add(updateSql);
        //            updateSql = string.Format(@" insert into T_TempScore(Year,UserName,Average,Type,IsProduct)
        //            select t.年份,t.{1},t.平均分新,'{2}','{3}' from ({0}) t", sql, userNameColmName, type, isPorduct);
        //            sqls.Add(updateSql);
        //            SqlHelper.BatchExecuteSql(sqls, ref error);
        //        }
    }
}

