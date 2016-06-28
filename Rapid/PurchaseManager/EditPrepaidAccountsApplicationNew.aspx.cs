using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.PurchaseManager
{
    public partial class EditPrepaidAccountsApplicationNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Guid"))
                {
                    string guid = ToolManager.GetQueryString("Guid");

                    string sql = string.Format(@" select *  from V_T_AccountsPayable_Main_ForYF 
where Guid ='{0}' ", guid);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtYFOne.Text = dr["预付一"] == null ? "" : dr["预付一"].ToString();
                        txtYFTwo.Text = dr["预付二"] == null ? "" : dr["预付二"].ToString();
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string yfone = Request.Form["txtYFOne"].ToString();
            string yftwo = Request.Form["txtYFTwo"].ToString();
            string guid = ToolManager.GetQueryString("Guid");
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"update T_AccountsPayable_Main set 
 YFOne={1},YFTwo={2}
where Guid='{0}'",  guid,  yfone, yftwo);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                lbSubmit.Text = "修改成功！";
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