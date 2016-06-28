using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.SellManager
{
    public partial class EditMachineQuoteDetailQty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Guid"))
                {
                    string key = ToolManager.GetQueryString("Guid");
                    string sql = string.Format(@"select * from MachineQuoteDetail where Guid ='{0}'", key);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtBomQty.Text = dr["BOMAmount"].ToString();
                        txtPrice.Text = dr["MaterialPrcie"].ToString();
                        txtGSF.Text = dr["TimeCharge"].ToString();
                        txtLR.Text = dr["Profit"].ToString();
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string key = ToolManager.GetQueryString("Guid");
            string sql = string.Format(@"   update MachineQuoteDetail set BOMAmount={0},
MaterialPrcie={1},TimeCharge={2},Profit={3} where Guid ='{4}' ", txtBomQty.Text, txtPrice.Text, txtGSF.Text, txtLR.Text, key);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                lbMsg.Text = "修改失败！原因：" + error;
            }
        }
    }
}
