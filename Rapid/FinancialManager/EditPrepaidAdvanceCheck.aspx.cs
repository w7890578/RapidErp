using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class EditPrepaidAdvanceCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Guid"))
                {
                    string guid = ToolManager.GetQueryString("Guid");

                    string sql = string.Format(@" select *  from [V_T_AccountsReceivableMain] 
where Guid ='{0}' ", guid);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        drpIsSettle.SelectedValue = dr["是否结清"] == null ? "" : dr["是否结清"].ToString();
                        txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string jq = Request.Form["drpIsSettle"].ToString();
            string remark = Request.Form["txtRemark"].ToString();
            string guid = ToolManager.GetQueryString("Guid");
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"update AccountsReceivable set IsSettle='{0}',Remark='{1}' where Guid='{2}'",
                 jq, remark, guid);
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
