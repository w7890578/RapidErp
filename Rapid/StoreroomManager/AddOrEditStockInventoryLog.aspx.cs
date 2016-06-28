using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;
namespace Rapid.StoreroomManager
{
    public partial class AddOrEditStockInventoryLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
                if (ToolManager.CheckQueryString("InventoryNumber"))
                {
                    string number = ToolManager.GetQueryString("InventoryNumber");
                    string sql = string.Format(@"    select * from StockInventoryLog where 
InventoryNumber ='{0}'", number);
                    DataTable dt = SqlHelper.GetTable(sql);
                    DataRow dr = dt.Rows[0];
                    drpWarehouseName.SelectedValue = dr["WarehouseName"] == null ? "" : dr["WarehouseName"].ToString();
                    drpInventoryType.SelectedValue = dr["InventoryType"] == null ? "" : dr["InventoryType"].ToString();
                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    btnSubmit.Text = "修改";
                }
                else
                {
                    btnSubmit.Text = "添加";
                }
            }
        }
        private void Bind()
        {
            string sql = "select WarehouseNumber,WarehouseName from WarehouseInfo";
            ControlBindManager.BindDrp(sql, this.drpWarehouseName, "WarehouseNumber", "WarehouseName");
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string inventoryNumber = "KCPD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string inventoryTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string warehouseName = drpWarehouseName.SelectedValue;
            string users = ToolCode.Tool.GetUser().UserNumber;
            string inventoryType = drpInventoryType.SelectedValue;
            string remark = txtRemark.Text;
            string sql = string.Empty;
            string error = string.Empty;
            if (btnSubmit.Text.Equals("添加"))
            {
                List<string> sqls = new List<string>();
                sql = string.Format(@"insert into StockInventoryLog (InventoryNumber,InventoryTime,WarehouseName
,Operator,InventoryType,Remark,Auditor,AuditeTime )
values('{0}','{1}','{2}','{3}','{4}','{5}','','')", inventoryNumber, inventoryTime, warehouseName, users, inventoryType, remark);
                sqls.Add(sql);
                sql = GetSql(warehouseName, inventoryType, inventoryNumber);
                if (!string.IsNullOrEmpty(sql))
                {
                    sqls.Add(sql);
                }

                error = string.Empty;
                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败！原因是:" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加库存盘点信息" + inventoryNumber, "增加成功");
                    //ToolCode.Tool.ResetControl(this.Controls);
                    Response.Write(ToolManager .GetClosePageJS ());
                    Response.End();
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加库存盘点信息" + inventoryNumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                sql = string.Format(@" 
update StockInventoryLog set WarehouseName='{0}',InventoryType='{1}'
,Remark='{2}' where InventoryNumber='{3}' ", warehouseName, inventoryType, remark, ToolManager.GetQueryString("InventoryNumber"));
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败，原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑库存盘点信息" + ToolManager.GetQueryString("InventoryNumber"), "编辑成功");
                    Response.Write("<script>window.close();</script>");
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑库存盘点信息" + ToolManager.GetQueryString("InventoryNumber"), "编辑失败！原因" + error);
                    return;
                }
            }
        }
        private string GetSql(string warehouseId, string type, string inventoryNumber)
        {
            string tableName = string.Empty;
            string sql = string.Format(@"select [Type] from WarehouseInfo where WarehouseNumber='{0}' 
 ", warehouseId);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count <= 0)
            {
                return string.Empty;
            }
            //产成品库、原材料库、半成品库、废品库、样品库
            switch (dt.Rows[0][0].ToString())
            {
                case "产成品库": tableName = "ProductStock";
                    break;
                case "原材料库": tableName = "MaterialStock";
                    break;
                case "半成品库": tableName = "HalfProductStock";
                    break;
                case "废品库": tableName = "ScrapStock";
                    break;
                case "样品库": tableName = "SampleStock";
                    break;
            }
            sql = "  insert into StockInventoryLogDetail ";
            if (type.Equals("日盘点"))
            {
                //产品
                if (tableName.Equals("ProductStock"))
                {
                    sql += string.Format(@"  select '{0}',ProductNumber,[Version],StockQty,StockQty,0,'','' from {1} where WarehouseName='{2}'
and UpdateTime between '{3} 00:00:00' and   '{3} 23:59:59'", inventoryNumber, tableName, warehouseId, DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else if (tableName.Equals("HalfProductStock"))
                {
                    sql += string.Format(@"  select '{0}',ProductNumber,[Version],StockQty,StockQty,0,'',MaterialNumber from {1} where WarehouseName='{2}'
and UpdateTime between '{3} 00:00:00' and   '{3} 23:59:59'", inventoryNumber, tableName, warehouseId, DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else //原材料
                {
                    sql += string.Format(@"  select '{0}',MaterialNumber,'0',StockQty,StockQty,0,'','' from {1} where WarehouseName='{2}'
and UpdateTime between '{3} 00:00:00' and   '{3} 23:59:59'", inventoryNumber, tableName, warehouseId, DateTime.Now.ToString("yyyy-MM-dd"));
                }
            }
            else
            {
                if (tableName.Equals("ProductStock"))
                {
                    sql += string.Format("  select '{0}',ProductNumber,[Version],StockQty,StockQty,0,'','' from {1} where WarehouseName='{2}'", inventoryNumber, tableName, warehouseId);
                }
                else if (tableName.Equals("HalfProductStock"))
                {
                    sql += string.Format("  select '{0}',ProductNumber,[Version],StockQty,StockQty,0,'',MaterialNumber from {1} where WarehouseName='{2}'", inventoryNumber, tableName, warehouseId);
                }
                else
                {
                    sql += string.Format("  select '{0}',MaterialNumber,'0',StockQty,StockQty,0,'','' from {1} where WarehouseName='{2}'", inventoryNumber, tableName, warehouseId);
                }
            }
            return sql;
        }

    }
}
