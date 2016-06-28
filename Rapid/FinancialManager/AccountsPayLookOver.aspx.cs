using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class AccountsPayLookOver : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Bind();
            }

        }

        private void Bind()
        {
            rpList.DataSource = SqlHelper.GetTable(GetSql());
            rpList.DataBind();
        }
        private string GetSql()
        {
            string condinton = " where 是否为预付='否' and 是否已审批='是' and 是否已被申请='是'";
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
            if (drpIsJQ.SelectedValue != "")
            {
                condinton += " and 是否结清='" + drpIsJQ.SelectedValue + "'";
            }
            if (!string.IsNullOrEmpty(txtfpnumber.Text.Trim()))
            {
                condinton += " and 发票号码 like '%" + txtfpnumber.Text.Trim() + "%' ";
            }
            string sql = string.Format(@"select * from V_AccountsPay {0} union all
select '合计','', sum(到货数量),SUM(订单总价未税),SUM(到货总价未税),'','','','','','','','','','','','','','','','' from [V_AccountsPay] {0}
order by 创建时间 desc
", condinton);

            return sql;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnExpExcel_Click(object sender, EventArgs e)
        {
            ToolCode.Tool.ExpExcel(GetSql(), "应付账款");
        }

    }
}
