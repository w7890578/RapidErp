using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class T_AccountsPayable_MainList_New : System.Web.UI.Page
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
            string suppName = txtSuppName.Text.Trim();
            if (!string.IsNullOrEmpty(suppName))
            {
                condion += string.Format(" and 供应商名称 like '%{0}%'", suppName);
            }
            string sql = string.Format(@"  select * from V_T_AccountsPayable_Main {0}", condion);
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
