using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using System.Data;

namespace Rapid.FinancialManager
{
    public partial class EditAccountsReceiveDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ToolManager.CheckQueryString("Guid"))
            {
                string guid = ToolManager.GetQueryString("Guid");

                string sql = string.Format(@" select *  from V_AccountsReceivableDetail 
where Guid ='{0}' ", guid);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];

                    txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string remark = Request.Form["txtRemark"].ToString();
            string guid = ToolManager.GetQueryString("Guid");
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"update T_AccountsReceivable_Detail set Remark='{0}' where Guid='{1}'", remark, guid);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                lbSubmit.Text = "修改成功！";
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
