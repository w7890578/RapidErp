using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.StoreroomManager
{
    public class Select
    {
        public string text = string.Empty;
        public string value = string.Empty;
    }

    public partial class StockInventoryLogDetail : System.Web.UI.Page
    {
        //public static string hasEdit = "inline";
        public static string warehouseName = string.Empty;
        public static string name = string.Empty;
        public static string showVersion = "none";
        public static string showMareial = "none";
        public static string number = "";
        public static string sortName = "Cargo";
        public static string sortGuiZe = "Desc";
        public static DataTable ResultTable = new DataTable();
        public static bool IsCheck = false;
        public static List<Select> kinds = new List<Select>();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0405", "Print");
                //hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0405", "Edit");
                if (!ToolManager.CheckQueryString("InventoryNumber"))
                {
                    Response.Write("未知盘点编号！");
                    Response.End();
                    return;
                }
                string sql = string.Format(@" 
 select AuditeTime  from StockInventoryLog where InventoryNumber ='{0}' ", ToolManager.GetQueryString("InventoryNumber"));
                if (string.IsNullOrEmpty(SqlHelper.GetScalar(sql)))
                {
                    btnCheck.Visible = true;
                    IsCheck = false;
                }
                else
                {
                    btnCheck.Visible = false;
                    IsCheck = true;
                }
                BindControl();
                Bind();
            }
        }

        private string GetSql()
        {
            string sql = string.Empty;
            string error = string.Empty;

            //            sql = string.Format(@" 
            //select * from (
            // select sild.*,mit.Cargo  from StockInventoryLogDetail sild inner join MarerialInfoTable mit on sild.MaterialNumber =mit.MaterialNumber
            // union all 
            // select sild.*,mit.Cargo  from StockInventoryLogDetail sild inner join Product  mit on sild.MaterialNumber =mit.ProductNumber
            // and mit.Version =sild .Version ) t where t.InventoryNumber='{0}'", ToolManager.GetQueryString("InventoryNumber"));
            //            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            //            this.rpList.DataBind(); 
            number = ToolManager.GetQueryString("InventoryNumber");
            sql = string.Format(@" select wi.WarehouseName from StockInventoryLog sil inner join WarehouseInfo
wi on sil.WarehouseName=wi.WarehouseNumber
 where sil.InventoryNumber='{0}' ", ToolManager.GetQueryString("InventoryNumber"));
            warehouseName = SqlHelper.GetScalar(sql);

            if (warehouseName.Equals("产成品库") || warehouseName.Equals("半成品库"))
            {
                sql = string.Format(@" select sild.*,mit.Cargo ,''as Kind,mit.NumberProperties ,'' as CargoType,'' as MaterialName from StockInventoryLogDetail sild 
 inner join Product  mit on sild.MaterialNumber =mit.ProductNumber  and mit.Version =sild .Version  
 where sild.InventoryNumber ='{0}'", ToolManager.GetQueryString("InventoryNumber"));
                showVersion = "inline";
                showMareial = "none";
                name = "产成品编号";
            }
            else
            {
                sql = string.Format(@" select sild.*,mit.Cargo,mit.Kind, mit.NumberProperties ,mit.CargoType ,mit.MaterialName
  from StockInventoryLogDetail sild 
 inner join MarerialInfoTable mit on sild.MaterialNumber =mit.MaterialNumber
 where sild.InventoryNumber ='{0}'", ToolManager.GetQueryString("InventoryNumber"));
                name = "原材料编号";
                showVersion = "none";
                showMareial = "inline";
            }
            string materialName = txtMatrialName.Text.Trim();
            sql = string.Format(" select t.* from ({0})t where 1=1", sql);
            string kind = Request.Form["txtKind"];
            if (!string.IsNullOrEmpty(kind) && kind != "==请选择原材料种类==")
            {
                string[] temps = kind.Split(',');
                kind = "'" + kind.Replace(",", "','") + "'";
                sql += string.Format(" and t.Kind in ({0}) ", kind);
                txtKind.Text = Request.Form["txtKind"];
            }
            else
            {
                txtKind.Text = "==请选择原材料种类==";
            }
            if (!txtNumberProperties.Text.Trim().Equals(""))
            {
                sql += string.Format(" and t.NumberProperties like '%{0}%' ", txtNumberProperties.Text.Trim());
            }
            if (!txtMareialNumber.Text.Trim().Equals(""))
            {
                sql += string.Format(" and t.MaterialNumber like '%{0}%' ", txtMareialNumber.Text.Trim());
            }
            if (!txtNumberPropertiesa.Text.Trim().Equals(""))
            {
                sql += string.Format(" and t.NumberProperties like '%{0}%' ", txtNumberPropertiesa.Text.Trim());
            }
            if (!txtProductNumber.Text.Trim().Equals(""))
            {
                sql += string.Format(" and t.MaterialNumber like '%{0}%' ", txtProductNumber.Text.Trim());
            }
            if (!string.IsNullOrEmpty(materialName))
            {
                sql += string.Format(" and t.MaterialName like '%{0}%' ", materialName);

            }
            if (drpZhangMianNumber.SelectedValue != "")
            {
                sql += " and t.PaperQty   " + drpZhangMianNumber.SelectedValue;

            }
            if (!dpIsYK.SelectedValue.Equals(""))
            {
                string isyk = dpIsYK.SelectedValue;
                if (isyk == "1")
                {
                    sql += string.Format(" and t.ProfitAndLossQty>0 ");
                }
                else if (isyk == "-1")
                {
                    sql += string.Format(" and t.ProfitAndLossQty<0 ");
                }
                else
                {
                    sql += string.Format(" and t.ProfitAndLossQty=0.00 ");
                }

            }

            sql = string.Format(@" select * from ({0}) a  order by {1} {2} ", sql, sortName, sortGuiZe);
            return sql;
        }

        private void Bind()
        {
            //            string sql = string.Empty;
            //            string error = string.Empty;

            //            //            sql = string.Format(@" 
            //            //select * from (
            //            // select sild.*,mit.Cargo  from StockInventoryLogDetail sild inner join MarerialInfoTable mit on sild.MaterialNumber =mit.MaterialNumber
            //            // union all 
            //            // select sild.*,mit.Cargo  from StockInventoryLogDetail sild inner join Product  mit on sild.MaterialNumber =mit.ProductNumber
            //            // and mit.Version =sild .Version ) t where t.InventoryNumber='{0}'", ToolManager.GetQueryString("InventoryNumber"));
            //            //            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            //            //            this.rpList.DataBind(); 
            //            number = ToolManager.GetQueryString("InventoryNumber");
            //            sql = string.Format(@" select wi.WarehouseName from StockInventoryLog sil inner join WarehouseInfo
            //wi on sil.WarehouseName=wi.WarehouseNumber
            // where sil.InventoryNumber='{0}' ", ToolManager.GetQueryString("InventoryNumber"));
            //            warehouseName = SqlHelper.GetScalar(sql);

            //            if (warehouseName.Equals("产成品库") || warehouseName.Equals("半成品库"))
            //            {
            //                sql = string.Format(@" select sild.*,mit.Cargo ,''as Kind,mit.NumberProperties ,'' as CargoType,'' as MaterialName from StockInventoryLogDetail sild 
            // inner join Product  mit on sild.MaterialNumber =mit.ProductNumber  and mit.Version =sild .Version  
            // where sild.InventoryNumber ='{0}'", ToolManager.GetQueryString("InventoryNumber"));
            //                showVersion = "inline";
            //                showMareial = "none";
            //                name = "产成品编号";
            //            }
            //            else
            //            {
            //                sql = string.Format(@" select sild.*,mit.Cargo,mit.Kind, mit.NumberProperties ,mit.CargoType ,mit.MaterialName
            //  from StockInventoryLogDetail sild 
            // inner join MarerialInfoTable mit on sild.MaterialNumber =mit.MaterialNumber
            // where sild.InventoryNumber ='{0}'", ToolManager.GetQueryString("InventoryNumber"));
            //                name = "原材料编号";
            //                showVersion = "none";
            //                showMareial = "inline";
            //            }
            //            string materialName = txtMatrialName.Text.Trim();
            //            sql = string.Format(" select t.* from ({0})t where 1=1", sql);
            //            string kind = Request.Form["txtKind"];
            //            if (!string.IsNullOrEmpty(kind) && kind != "==请选择原材料种类==")
            //            {
            //                string[] temps = kind.Split(',');
            //                kind = "'" + kind.Replace(",", "','") + "'";
            //                sql += string.Format(" and t.Kind in ({0}) ", kind);
            //                txtKind.Text = Request.Form["txtKind"];
            //            }
            //            else
            //            {
            //                txtKind.Text = "==请选择原材料种类==";
            //            }
            //            if (!txtNumberProperties.Text.Trim().Equals(""))
            //            {
            //                sql += string.Format(" and t.NumberProperties like '%{0}%' ", txtNumberProperties.Text.Trim());
            //            }
            //            if (!txtMareialNumber.Text.Trim().Equals(""))
            //            {
            //                sql += string.Format(" and t.MaterialNumber like '%{0}%' ", txtMareialNumber.Text.Trim());
            //            }
            //            if (!txtNumberPropertiesa.Text.Trim().Equals(""))
            //            {
            //                sql += string.Format(" and t.NumberProperties like '%{0}%' ", txtNumberPropertiesa.Text.Trim());
            //            }
            //            if (!txtProductNumber.Text.Trim().Equals(""))
            //            {
            //                sql += string.Format(" and t.MaterialNumber like '%{0}%' ", txtProductNumber.Text.Trim());
            //            }
            //            if (!string.IsNullOrEmpty(materialName))
            //            {
            //                sql += string.Format(" and t.MaterialName like '%{0}%' ", materialName);

            //            }
            //            if (drpZhangMianNumber.SelectedValue != "")
            //            {
            //                sql += " and t.PaperQty   " + drpZhangMianNumber.SelectedValue;

            //            }
            //            if (!dpIsYK.SelectedValue.Equals(""))
            //            {
            //                string isyk = dpIsYK.SelectedValue;
            //                if (isyk == "1")
            //                {
            //                    sql += string.Format(" and t.ProfitAndLossQty>0 ");
            //                }
            //                else if (isyk == "-1")
            //                {
            //                    sql += string.Format(" and t.ProfitAndLossQty<0 ");
            //                }
            //                else
            //                {
            //                    sql += string.Format(" and t.ProfitAndLossQty=0.00 ");
            //                }

            //            }

            //            sql = string.Format(@" select * from ({0}) a  order by {1} {2} ", sql, sortName, sortGuiZe);
            string sql = GetSql();
            ResultTable = SqlHelper.GetTable(sql);
            lbbHuoWei.Text = sortGuiZe.ToUpper() == "DESC" ? "货位▲" : "货位▼";

            //  union all
            //select '合计','合计','',SUM (PaperQty),SUM (InventoryQty),SUM (ProfitAndLossQty),'','','','','','' from ({0}) a 
            //this.rpList.DataSource = SqlHelper.GetTable(sql);
            //this.rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string userName = ToolCode.Tool.GetUser().UserName;
            string inventoryNumber = ToolManager.GetQueryString("InventoryNumber");
            string sql = string.Format(@" select   Auditor  from StockInventoryLog
where InventoryNumber ='{0}' ", inventoryNumber);
            string tempstr = SqlHelper.GetScalar(sql);
            if (!string.IsNullOrEmpty(tempstr))
            {
                lbMsg.Text = string.Format("审核失败！原因：该盘点单已由" + tempstr + "审核,请勿重复审核！");
                return;
            }
            string result = StoreroomToolManager.CheckStockInventoryLog(inventoryNumber, userName);


            if (result.Equals("1"))
            {
                Response.Redirect("StockInventoryLogList.aspx");
            }
            else
            {
                lbMsg.Text = string.Format("审核失败！原因：" + result);
            }
        }

        private void BindControl()
        {
            kinds.Clear();
            string sql = @"select distinct Kind from MarerialInfoTable 
        where Kind!=''";
            DataTable dt = SqlHelper.GetTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                kinds.Add(new Select() { text = dr["Kind"].ToString(), value = dr["Kind"].ToString() });
            }
            //drpKind.DataSource = SqlHelper.GetTable(sql);
            //drpKind.DataTextField = "Kind";
            //drpKind.DataValueField = "Kind";
            //drpKind.DataBind();
            //drpKind.Items.Insert(0, new ListItem("==请选择==", ""));
        }

        //protected void drpKind_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Bind();
        //}

        protected void drpZhangMianNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }

        protected void lbbHuoWei_Click(object sender, EventArgs e)
        {
            sortName = "Cargo";
            if (sortGuiZe.ToUpper() == "DESC")
            {
                sortGuiZe = "ASC";
            }
            else
            {
                sortGuiZe = "DESC";
            }
            Bind();
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            bool ismaterial = !(warehouseName.Equals("产成品库") || warehouseName.Equals("半成品库"));

            string sql = GetSql();
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Columns.Contains("InventoryNumber"))
            {
                dt.Columns["InventoryNumber"].ColumnName = "盘点编码";
            }
            if (dt.Columns.Contains("MaterialNumber"))
            {
                dt.Columns["MaterialNumber"].ColumnName = name;
            }
            if (dt.Columns.Contains("Version"))
            {
                if (showVersion != "inline")
                {
                    dt.Columns.Remove("Version");
                }
                else
                {
                    dt.Columns["Version"].ColumnName = "版本";
                }
            }
            if (dt.Columns.Contains("PaperQty"))
            {
                dt.Columns["PaperQty"].ColumnName = "账面数量";
            }
            if (dt.Columns.Contains("InventoryQty"))
            {
                dt.Columns["InventoryQty"].ColumnName = "实盘数量";
            }
            if (dt.Columns.Contains("ProfitAndLossQty"))
            {
                dt.Columns["ProfitAndLossQty"].ColumnName = "盈亏数量";
            }
            if (dt.Columns.Contains("Kind"))
            {
                if (showMareial != "inline")
                {
                    dt.Columns.Remove("Kind");
                }
                else
                {
                    dt.Columns["Kind"].ColumnName = "种类";
                }
            }
            if (dt.Columns.Contains("Cargo"))
            {
                dt.Columns["Cargo"].ColumnName = "货位";
            }
            if (dt.Columns.Contains("NumberProperties"))
            {
                dt.Columns["NumberProperties"].ColumnName = "编号属性";
            }
            if (dt.Columns.Contains("MaterialName"))
            {
                if (!ismaterial)
                {
                    dt.Columns.Remove("MaterialName");
                }
                else
                {
                    dt.Columns["MaterialName"].ColumnName = "型号";
                }
            }
            if (dt.Columns.Contains("CargoType"))
            {
                if (showMareial != "inline")
                {
                    dt.Columns.Remove("CargoType");
                }
                else
                {
                    dt.Columns["CargoType"].ColumnName = "货物类型";
                }
            }
            if (dt.Columns.Contains("Remark"))
            {
                dt.Columns["Remark"].ColumnName = "备注";
            }

            
            ToolCode.Tool.ExpExcel(dt, warehouseName + "盘点明细");
        }
    }
}
