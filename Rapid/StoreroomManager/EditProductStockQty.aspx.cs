using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.StoreroomManager
{
    public partial class EditProductStockQty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string productnumber = ToolManager.GetQueryString("ProductNumber");
                string warehousenumber = ToolManager.GetQueryString("WarehouseNumber");
                string version = ToolManager.GetQueryString("Version");
                string tableName = ToolManager.GetQueryString("tableName");
                string sql = string.Format(@" select StockQty from {0} where ProductNumber='{1}' 
and Version='{2}' and WarehouseName='{3}'", tableName, productnumber, version, warehousenumber);
                lbProductNumber.Text = productnumber;
                lbVersion.Text = version;
                txtQty.Text = SqlHelper.GetScalar(sql);
                sql = string.Format(@" select  Cargo  from Product where ProductNumber ='{0}' and Version ='{1}'", productnumber, version);
                txtHW.Text = SqlHelper.GetScalar(sql);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string productnumber = ToolManager.GetQueryString("ProductNumber");
            string warehousenumber = ToolManager.GetQueryString("WarehouseNumber");
            string version = ToolManager.GetQueryString("Version");
            string tableName = ToolManager.GetQueryString("tableName");
            string hw = txtHW.Text.Trim(); ;
            string qty = txtQty.Text;
            string error = string.Empty;
            List<string> sqls = new List<string>();
            string sql = string.Format(@" 
update {0} set StockQty ={1} where ProductNumber='{2}' 
and Version='{3}' and WarehouseName='{4}'", tableName, qty, productnumber, version, warehousenumber);
            sqls.Add(sql);
            sql = string.Format(@"
 update Product set Cargo='{0}' where ProductNumber ='{1}' and Version ='{2}'", hw, productnumber, version);
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
