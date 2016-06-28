using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.PurchaseManager
{
    public partial class PrepaidAccountsCheck : System.Web.UI.Page
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
update T_AccountsPayable_Main set IsApproval='是' where guid in ({0}) ", guids);
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
update T_AccountsPayable_Main set IsApplicationed='否' where guid in ({0}) ", guids);
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
            string condinton = " where 是否为预付='是' and 是否已审批='否' and 是否已被申请='是'";
            if (txtOdersNumber.Text != "")
            {
                condinton += " and 采购订单号 like '%" + txtOdersNumber.Text.Trim() + "%'";
            }
            if (txtSupplierName.Text != "")
            {
                condinton += " and 供应商名称 like '%" + txtSupplierName.Text.Trim() + "%'";
            }
            if (drpPayType.SelectedValue != "")
            {
                condinton += " and 付款类型='" + drpPayType.SelectedValue + "'";
            }
            if (txtHDnumber.Text != "")
            {
                condinton += " and 采购合同号 like '%" + txtHDnumber.Text + "%'";
            }
            string sql = string.Format(@"select * from V_T_AccountsPayable_Main_ForYF {0} 
union all
select '合计','',SUM(订单总价未税),SUM(到货总价未税),'','','','','','','','','','','','','','','','' from V_T_AccountsPayable_Main_ForYF {0}", condinton);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
