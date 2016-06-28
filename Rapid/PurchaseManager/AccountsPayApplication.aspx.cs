using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.PurchaseManager
{
    public partial class AccountsPayApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("sq"))//选选择
                {
                    string error = string.Empty;
                    string guids = ToolManager.GetQueryString("sq");
                    string sql = string.Format(@" 
update T_AccountsPayable_Main set IsApplicationed='是' where guid in ({0}) ", guids);
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
            string condinton = " where 是否为预付='否' and 是否已审批='否' and 是否已被申请='否'";
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
            if (txtDeliveryDate.Text != "")
            {
                condinton += " and 到货日期='" + txtDeliveryDate.Text + "'";
            }
            if (txtPaymentDueDate.Text != "")
            {
                condinton += " and 款项到期日='" + txtPaymentDueDate.Text + "'";
            }
            string sql = string.Format(@"select * from V_AccountsPay {0}
union all
select '合计','', sum(到货数量),SUM(订单总价未税),SUM(到货总价未税),'','','','','','','','','','','','','','','','' from [V_AccountsPay] {0}", condinton);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

    }
}
