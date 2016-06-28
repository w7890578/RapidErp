using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;
using Model;
using System.Web.UI.WebControls;
using System.Web;
using System.Text.RegularExpressions;

namespace BLL
{
    public class MarerialInfoTableManager
    {
        //类内全局变量
        private static string sql = string.Empty;
        private static string error = string.Empty;

        public static string GetMarerialNames()
        {
            string MarerialNames = string.Empty;
            string sql = @"  
select distinct MaterialNumber from [dbo].[MarerialInfoTable]  ";
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MarerialNames += dr[0] + ",";
                }
            }
            return MarerialNames.TrimEnd(',');
        }

        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static MarerialInfoTable ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            MarerialInfoTable marerialinfo = new MarerialInfoTable();
            marerialinfo.MaterialNumber = dr["MaterialNumber"] == null ? "" : dr["MaterialNumber"].ToString();
            marerialinfo.MaterialName = dr["MaterialName"] == null ? "" : dr["MaterialName"].ToString();
            marerialinfo.Description = dr["Description"] == null ? "" : dr["Description"].ToString();
            marerialinfo.Kind = dr["Kind"] == null ? "" : dr["Kind"].ToString();
            marerialinfo.Type = dr["Type"] == null ? "" : dr["Type"].ToString();
            marerialinfo.Brand = dr["Brand"] == null ? "" : dr["Brand"].ToString();
            marerialinfo.StockSafeQty = Convert.ToInt32(dr["StockSafeQty"] == null ? "" : dr["StockSafeQty"].ToString());
            marerialinfo.ProcurementPrice = Convert.ToDecimal(dr["ProcurementPrice"] == null ? "" : dr["ProcurementPrice"].ToString());
            marerialinfo.MaterialPosition = dr["MaterialPosition"] == null ? "" : dr["MaterialPosition"].ToString();
            marerialinfo.MinPacking = dr["MinPacking"] == null ? "" : dr["MinPacking"].ToString();
            marerialinfo.MinOrderQty = dr["MinOrderQty"] == null ? "" : dr["MinOrderQty"].ToString();
            marerialinfo.ScrapPosition = dr["ScrapPosition"] == null ? "" : dr["ScrapPosition"].ToString();
            marerialinfo.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
            marerialinfo.Cargo = dr["Cargo"] == null ? "" : dr["Cargo"].ToString();
            marerialinfo.SixStockSafeQty = dr["SixStockSafeQty"] == null ? "" : dr["SixStockSafeQty"].ToString();
            marerialinfo.CargoType = dr["CargoType"] == null ? "" : dr["CargoType"].ToString();
            marerialinfo.NumberProperties = dr["NumberProperties"] == null ? "" : dr["NumberProperties"].ToString();
            marerialinfo.Unit = dr["Unit"] == null ? "" : dr["Unit"].ToString();
            return marerialinfo;

        }
        /// <summary>
        /// 检测是否有该编号
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public static bool IsExit(string MaterialNumber)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from MarerialInfoTable where MaterialNumber='{0}' ", MaterialNumber);
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
        /// 添加原材料信息
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddMarerialInfoTable(Model.MarerialInfoTable marerialinfotable, ref string error)
        {
            if (IsExit(marerialinfotable.MaterialNumber))
            {
                error = "已存在该编号！请重新填写编号。";
                return false;
            }

            if (string.IsNullOrEmpty(marerialinfotable.MaterialNumber)
            || string.IsNullOrEmpty(marerialinfotable.StockSafeQty.ToString()) ||
            string.IsNullOrEmpty(marerialinfotable.ProcurementPrice.ToString()) || string.IsNullOrEmpty(marerialinfotable.MinPacking)
            || string.IsNullOrEmpty(marerialinfotable.MinOrderQty))
            {
                error = "原材料信息不完整！";
                return false;
            }
            List<string> sqls = new List<string>();
            sql = string.Format(@" insert into MarerialInfoTable (MaterialNumber,MaterialName,Description,Kind,Type,Brand,StockSafeQty,
            ProcurementPrice,MaterialPosition,MinPacking,MinOrderQty,ScrapPosition,Remark,Cargo,CargoType,NumberProperties,Unit) 
           values ('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}')",
           marerialinfotable.MaterialNumber, marerialinfotable.MaterialName, marerialinfotable.Description, marerialinfotable.Kind,
           marerialinfotable.Type, marerialinfotable.Brand, marerialinfotable.StockSafeQty, marerialinfotable.ProcurementPrice,
           marerialinfotable.MaterialPosition, marerialinfotable.MinPacking, marerialinfotable.MinOrderQty,
           marerialinfotable.ScrapPosition, marerialinfotable.Remark, marerialinfotable.Cargo, marerialinfotable.CargoType, marerialinfotable.NumberProperties, marerialinfotable.Unit);
            sqls.Add(sql);
            sql = string.Format(@"insert into MaterialStock (MaterialNumber,StockQty,UpdateTime,WarehouseName)
values('{0}',{1},'{2}','ycl')", marerialinfotable.MaterialNumber, 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }
        /// <summary>
        /// 删除原材料信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            List<string> list = new List<string>();
            sql = string.Format(@" delete MaterialSupplierProperty where MaterialNumber in ({0}) ", ids);
            list.Add(sql);
            sql = string.Format(@" delete MaterialCustomerProperty where MaterialNumber in ({0}) ", ids);
            list.Add(sql);
            sql = string.Format(@" delete MaterialBlueprintProperty where MaterialNumber in ({0}) ", ids);
            list.Add(sql);
            sql = string.Format(@" delete MarerialInfoTable where MaterialNumber in ({0}) ", ids);
            list.Add(sql);
            sql = string.Format(@"  delete ProductCuttingLineInfo where MaterialNumber in ({0}) ", ids);
            list.Add(sql);
            sql = string.Format(@" delete BOMInfo where MaterialNumber in ({0})", ids);
            list.Add(sql);
            sql = string.Format(@" delete MaterialStock where MaterialNumber in ({0})", ids);
            list.Add(sql);
            sql = string.Format(@"  update Product  set CostPrice =(select sum(bom.SingleDose*mit.ProcurementPrice ) from BOMInfo bom 
inner join MarerialInfoTable mit on bom.MaterialNumber =mit.MaterialNumber
 where bom.ProductNumber =p.ProductNumber and bom.Version =p.Version 
 group by bom.ProductNumber , bom.Version  )  from Product  p 
inner join (
select ProductNumber ,Version  from BOMInfo  where  MaterialNumber in ({0})) t on 
p.ProductNumber=t.ProductNumber  and p.Version =t.Version ", ids);
            list.Add(sql);
            return SqlHelper.BatchExecuteSql(list, ref error) == true ? "1" : error;

        }

        /// <summary>
        /// 编辑原材料信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditMarerialInfoTable(MarerialInfoTable maretialinfotable, ref string error)
        {
            if (!IsExit(maretialinfotable.MaterialNumber))
            {
                error = "不存在该原材料编号！请重新填写！";
                return false;
            }
            List<string> list = new List<string>();
            sql = string.Format(@" update MarerialInfoTable set MaterialName='{0}',
Description='{1}',Kind='{2}',Type='{3}',Brand='{4}',StockSafeQty={5},ProcurementPrice='{6}'
,MaterialPosition='{7}',MinPacking='{8}',
MinOrderQty='{9}',ScrapPosition='{10}',Remark='{11}' ,CargoType='{13}',NumberProperties='{14}',Unit='{15}'
where MaterialNumber='{12}'", maretialinfotable.MaterialName,
                            maretialinfotable.Description, maretialinfotable.Kind, maretialinfotable.Type,
                            maretialinfotable.Brand, maretialinfotable.StockSafeQty, maretialinfotable.ProcurementPrice,
                            maretialinfotable.MaterialPosition, maretialinfotable.MinPacking, maretialinfotable.MinOrderQty,
                            maretialinfotable.ScrapPosition, maretialinfotable.Remark, maretialinfotable.MaterialNumber , maretialinfotable.CargoType, maretialinfotable.NumberProperties, maretialinfotable.Unit);
            list.Add(sql);
            sql = string.Format(@" update Product  set CostPrice =(select sum(bom.SingleDose*mit.ProcurementPrice ) from BOMInfo bom 
inner join MarerialInfoTable mit on bom.MaterialNumber =mit.MaterialNumber
 where bom.ProductNumber =p.ProductNumber and bom.Version =p.Version 
 group by bom.ProductNumber , bom.Version  )  from Product  p 
inner join (
select ProductNumber ,Version  from BOMInfo  where  MaterialNumber  ='{0}') t on 
p.ProductNumber=t.ProductNumber  and p.Version =t.Version ", maretialinfotable.MaterialNumber);
            list.Add(sql);
            return SqlHelper.BatchExecuteSql(list, ref error);
        }


        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="users"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BatchAddData(List<MarerialInfoTable> marerialinfotables, ref string error)
        {
            int i = 0;
            string tempError = string.Empty;
            if (marerialinfotables.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (MarerialInfoTable marerialinfotable in marerialinfotables)
            {
                tempError = "";
                if (!AddMarerialInfoTable(marerialinfotable, ref tempError))
                {
                    i++;
                    error += string.Format("原材料{0}&nbsp;&nbsp;添加失败：原因--{1}<br/>", marerialinfotable.MaterialName, tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>添加失败客户:<br/>{2}", (marerialinfotables.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }


        /// <summary>
        /// 绑定种类
        /// </summary>
        /// <param name="drp"></param>
        public static void BindKind(DropDownList drp)
        {
            string error = string.Empty;
            string sql = string.Format(" select Kind from MarerialKind");
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                drp.DataSource = dt;
                drp.DataValueField = "Kind";
                drp.DataTextField = "Kind";
                drp.DataBind();
            } drp.Items.Insert(0, new ListItem("- - - 请 选 择 - - -", ""));

        }

        /// <summary>
        /// 通过编号绑定类别
        /// </summary>
        /// <param name="drpProduct"></param>
        /// <param name="drpCustomerProduct"></param>
        public static void BindType(DropDownList drpKind, DropDownList drpType)
        {
            string error = string.Empty;
            string Pid = drpKind.SelectedValue;
            drpType.Items.Clear();
            if (!string.IsNullOrEmpty(Pid))
            {
                string sql = string.Format(@" select Type from MareriaType where Pid='{0}'", Pid);
                DataTable dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    drpType.DataSource = dt;
                    drpType.DataValueField = "Type";
                    drpType.DataTextField = "Type";
                    drpType.DataBind();
                }
            }
            drpType.Items.Insert(0, new ListItem("- - - 请 选 择 - - -", ""));
        }

        /// <summary>
        /// 绑定仓库名称
        /// </summary>
        /// <param name="drp"></param>
        public static void BindWarehouseName(DropDownList drp)
        {
            string error = string.Empty;
            string sql = string.Format(" select WarehouseNumber, WarehouseName from WarehouseInfo");
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                drp.DataSource = dt;
                drp.DataValueField = "WarehouseNumber";
                drp.DataTextField = "WarehouseName";
                drp.DataBind();
            }
            drp.Items.Insert(0, new ListItem("- - - 请 选 择 - - -", ""));


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
        /// 批量添加原材料新
        /// </summary>
        /// <param name="marerialinfotable"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddMarielToBatch(MarerialInfoTableNew marerialinfotable, ref string error)
        {
            List<string> sqls = new List<string>();
            string sql = string.Format("  select COUNT (*) from MarerialInfoTable where MaterialNumber='{0}' ", marerialinfotable.MaterialNumber);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                sql = string.Format(@" insert into MarerialInfoTable (MaterialNumber,MaterialName,Description,Kind,Type,Brand,StockSafeQty,
            ProcurementPrice,MaterialPosition,MinPacking,MinOrderQty,ScrapPosition,Remark,Cargo) 
           values ('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
          marerialinfotable.MaterialNumber, marerialinfotable.MaterialName, marerialinfotable.Description, marerialinfotable.Kind,
          marerialinfotable.Type, marerialinfotable.Brand, marerialinfotable.StockSafeQty, marerialinfotable.ProcurementPrice,
          marerialinfotable.MaterialPosition, marerialinfotable.MinPacking, marerialinfotable.MinOrderQty,
          marerialinfotable.ScrapPosition, marerialinfotable.Remark, marerialinfotable.Cargo);
                sqls.Add(sql);
                sqls.AddRange(GetAddMaterialSupplieAndCustomerSql(marerialinfotable));
            }
            else
            {
                sqls.AddRange(GetAddMaterialSupplieAndCustomerSql(marerialinfotable));
            }
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }
        /// <summary>
        /// 是否存在相同客户物料编号
        /// </summary>
        /// <param name="mareialNumber"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private static bool IsExitCustmoerMareialNumber(string mareialNumber, string customerId)
        {
            string sql = string.Format(" select COUNT(*) from MaterialCustomerProperty where MaterialNumber ='{0}' and CustomerId ='{1}' ", mareialNumber, customerId);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }
        /// <summary>
        /// 是否存在相同供应商物料编号
        /// </summary>
        /// <param name="mareialNumber"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        private static bool IsExitSupplierMaterialNumber(string mareialNumber, string supplierId)
        {
            string sql = string.Format(" select COUNT(*) from MaterialSupplierProperty where MaterialNumber ='{0}' and SupplierId ='{1}' ", mareialNumber, supplierId);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }

        private static List<string> GetAddMaterialSupplieAndCustomerSql(MarerialInfoTableNew marerialinfotable)
        {
            List<string> sqls = new List<string>();
            string sql = string.Empty;
            if (!string.IsNullOrEmpty(marerialinfotable.SupplierMaterialNumber) && !string.IsNullOrEmpty(marerialinfotable.SupplierId))
            {
                if (!IsExitSupplierMaterialNumber(marerialinfotable.MaterialNumber, marerialinfotable.SupplierMaterialNumber))
                {
                    sql = string.Format(@" insert into MaterialSupplierProperty (MaterialNumber ,SupplierId ,SupplierMaterialNumber,MinOrderQty )
values('{0}','{1}','{2}',{3}) ", marerialinfotable.MaterialNumber, marerialinfotable.SupplierId, marerialinfotable.SupplierMaterialNumber, marerialinfotable.MinOrderQty);
                    sqls.Add(sql);
                }
            }
            if (!string.IsNullOrEmpty(marerialinfotable.CustomerMaterialNumber) && !string.IsNullOrEmpty(marerialinfotable.CustomerId))
            {
                if (!IsExitCustmoerMareialNumber(marerialinfotable.MaterialNumber, marerialinfotable.CustomerMaterialNumber))
                {
                    sql = string.Format(@" insert into MaterialCustomerProperty(MaterialNumber ,CustomerMaterialNumber,CustomerId)
values('{0}','{1}','{2}') ", marerialinfotable.MaterialNumber, marerialinfotable.CustomerMaterialNumber, marerialinfotable.CustomerId);
                    sqls.Add(sql);
                }
            }
            return sqls;
        }

        /// <summary>
        /// 批量添加原材料之单条添加【仅仅适用于批量添加】
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddAddMaterialInfo(DataRow dr, ref string error)
        {

            Regex r = new Regex(@"[\u4e00-\u9fa5]+");
            Match mc = r.Match(dr["原材料编号(瑞普迪编号)"].ToString().Trim());
            if (mc.Length != 0)
            {
                error = "原材料编号禁止输入中文";
                return false;
            }

            List<string> sqls = new List<string>();
            string sql = string.Format(" select COUNT(*) from MarerialInfoTable where MaterialNumber='{0}'", dr["原材料编号(瑞普迪编号)"]);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                sql = string.Format(@" insert into MarerialInfoTable 
(MaterialNumber,MaterialName,Description ,Kind ,Type ,Brand,ProcurementPrice ,MinPacking
  ,MinOrderQty ,Cargo ,Remark,CargoType,NumberProperties ,Unit )
  values('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}','{8}','{9}','{10}','{11}'
,'{12}','{13}') ", dr["原材料编号(瑞普迪编号)"]
                                                                            , dr["型号"], dr["描述"], dr["种类"], dr["类别"], dr["品牌"], "0"
, dr["最小包装"],
dr["最小起订量"], dr["货位"], dr["备注"], dr["货物类型"], dr["编号属性"], dr["单位"]);
                sqls.Add(sql);
            }
            if (!dr["客户编号"].ToString().Trim().Equals(""))
            {
                sql = string.Format(@" 
 select COUNT(*) from Customer where CustomerId ='{0}' ", dr["客户编号"]);
                if (SqlHelper.GetScalar(sql).Equals("0"))
                {
                    error = string.Format("系统不存在该客户编号:{0},原材料编号：{1} ", dr["客户编号"], dr["原材料编号(瑞普迪编号)"]);
                    return false;
                }

                sql = string.Format(@"  select COUNT(*) from MaterialCustomerProperty where MaterialNumber='{0}' and CustomerId='{1}'", dr["原材料编号(瑞普迪编号)"], dr["客户编号"]);
                if (SqlHelper.GetScalar(sql).Equals("0"))
                {
                    sql = string.Format(@"insert into MaterialCustomerProperty (MaterialNumber ,CustomerMaterialNumber,CustomerId)
  values('{0}','{1}','{2}')", dr["原材料编号(瑞普迪编号)"], dr["客户物料编号"].ToString().Trim(), dr["客户编号"]);
                    sqls.Add(sql);
                }
            }
            if (!dr["供应商编号"].ToString().Equals(""))
            {
                sql = string.Format(@"  
 select COUNT (*) from SupplierInfo where SupplierId ='{0}' ", dr["供应商编号"]);
                if (SqlHelper.GetScalar(sql).Equals("0"))
                {
                    error = string.Format("系统不存在该供应商编号:{0},原材料编号：{1} ", dr["供应商编号"], dr["原材料编号(瑞普迪编号)"]);
                    return false;
                }


                sql = string.Format(@" select count(*) from MaterialSupplierProperty where MaterialNumber='{0}' and SupplierId ='{1}'", dr["原材料编号(瑞普迪编号)"], dr["供应商编号"]);

                if (SqlHelper.GetScalar(sql).Equals("0"))
                {
                    sql = string.Format(@"  insert into MaterialSupplierProperty(MaterialNumber ,SupplierId ,SupplierMaterialNumber,MinOrderQty ,Prcie ,DeliveryCycle)
  values('{0}','{1}','{2}','{3}',{4},'{5}')",
                        dr["原材料编号(瑞普迪编号)"], dr["供应商编号"], dr["供应商物料编号"].ToString().Trim()
                        , dr["供应商最小起订量"].ToString().Trim().Equals("") ? "0" : dr["供应商最小起订量"].ToString(),
                        dr["供应商单价"].ToString().Trim().Equals("") ? "0" : dr["供应商单价"].ToString(), dr["供应商交货周期"]);
                    sqls.Add(sql);
                }
            }
            sql = string.Format(@"
  select COUNT(*) from MaterialStock where MaterialNumber='{0}'", dr["原材料编号(瑞普迪编号)"]);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                sql = string.Format(@" insert into MaterialStock  values('{0}',0,'{1}','ycl')", dr["原材料编号(瑞普迪编号)"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqls.Add(sql);
            }
            if (SqlHelper.BatchExecuteSql(sqls, ref error))
            {
                return true;
            }
            else
            {
                error += dr["原材料编号(瑞普迪编号)"].ToString();
                return false;
            }


        }

        /// <summary>
        /// 批量添加原材料
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BatchAddMaterialInfo(FileUpload FU_Excel, HttpServerUtility server, ref string error)
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
                if (!AddAddMaterialInfo(dr, ref tempError))
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
