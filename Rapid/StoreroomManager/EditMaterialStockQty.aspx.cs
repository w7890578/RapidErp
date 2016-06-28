using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class EditMaterialStockQty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string marielNumber = ToolManager.GetQueryString("marielNumber");
                string warehouseId = ToolManager.GetQueryString("warehouseId");
                string tableName = ToolManager.GetQueryString("tableName");
                string sql = string.Format(@" select StockQty from {0} where MaterialNumber='{1}' 
and WarehouseName='{2}'", tableName, marielNumber, warehouseId);
                lbMaterialNumber.Text = marielNumber;
                txtQty.Text = SqlHelper.GetScalar(sql);
                sql = string.Format("  select Cargo,StockSafeQty  from MarerialInfoTable where MaterialNumber='{0}' ", marielNumber);
                DataTable dt = SqlHelper.GetTable(sql);
                int num = dt.Rows.Count;
                if (num == 1)
                {
                    txtHW.Text = dt.Rows[0]["Cargo"].ToString();
                    txtStockSafeQty.Text = dt.Rows[0]["StockSafeQty"].ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string marielNumber = ToolManager.GetQueryString("marielNumber");
            string warehouseId = ToolManager.GetQueryString("warehouseId");
            string tableName = ToolManager.GetQueryString("tableName");
            string qty = txtQty.Text.Trim();
            string hw = txtHW.Text.Trim();
            string stocksafeqty=txtStockSafeQty.Text.Trim();
            string error = string.Empty;
            List<string> sqls = new List<string>();
            string sql = string.Format(@" 
update {0} set StockQty ={1} where MaterialNumber='{2}' and WarehouseName='{3}' ", tableName, qty, marielNumber, warehouseId);
            sqls.Add(sql);

            sql = string.Format(@"
 update MarerialInfoTable set Cargo='{0}',StockSafeQty={2} where MaterialNumber='{1}'", hw, marielNumber,stocksafeqty);
            sqls.Add(sql);

            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            if (result)
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                lbSubmit.Text = "修改失败！原因：" + error;
                return;
            }
        }
    }
}
