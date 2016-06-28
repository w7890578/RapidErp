using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class PrepaidAdvanceCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("sp"))//审批通过
                {
                    string error = string.Empty;
                    string guids = ToolManager.GetQueryString("sp");
                    string sql = string.Format(@" 
update AccountsReceivable set IsApproval='是' where guid in ({0}) ", guids);
                    string result = SqlHelper.ExecuteSql(sql, ref error) ? "1" : error;
                    Response.Write(result);
                    Response.End();
                    return;
                }
                if (ToolManager.CheckQueryString("wsp"))//审核未通过
                {
                    string error = string.Empty;
                    string guids = ToolManager.GetQueryString("wsp");
                    string sql = string.Format(@" 
update AccountsReceivable set IsApplicationed='否' where guid in ({0}) ", guids);
                    string result = SqlHelper.ExecuteSql(sql, ref error) ? "1" : error;
                    Response.Write(result);
                    Response.End();
                    return;
                }
                Bind();
            }

        }

        private void Bind()
        {
            string condinton = " where 是否为预收='是' and 是否已审批='否' and 是否已被申请='是'";
            if (txtOdersNumber.Text != "")
            {
                condinton += " and 销售订单号 like '%" + txtOdersNumber.Text.Trim() + "%'";
            }
            if (txtCustomerOdersNumber.Text != "")
            {
                condinton += " and 客户采购订单号 like '%" + txtCustomerOdersNumber.Text.Trim() + "%'";
            }
            if (drpType.SelectedValue != "")
            {
                condinton += " and 收款类型='" + drpType.SelectedValue + "'";
            }
            if (drpisSettle.SelectedValue != "")
            {
                condinton += " and 是否结清='" + drpisSettle.SelectedValue + "'";
            }
            if (txtCustomerName.Text != "")
            {
                condinton += " and 客户名称 like '%" + txtCustomerName.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from [V_T_AccountsReceivableMain] {0}
union all
   select '合计','','',SUM(订单总价),'','','','',SUM(交货总价),'','','','','','','','','','','','','','' from [V_T_AccountsReceivableMain] {0}", condinton);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

    }
}
