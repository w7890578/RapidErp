using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class AccountsReceivable_Main : System.Web.UI.Page
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
            string condion = " where 1=1 ";
            string customerName = txtCustomerName.Text.Trim();
            string receivable=txtReceivable.Text.Trim();
            if (!string.IsNullOrEmpty(customerName))
            {
                condion += string.Format(" and 客户名称 like '%{0}%'", customerName);
            }
            if(!string.IsNullOrEmpty(receivable))
            {
                condion+=string.Format(" and 收款方式 like '%{0}%'",receivable);
            }
            string sql = string.Format(@"  select * from V_AccountsReceivable_Main {0}", condion);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
            return;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
