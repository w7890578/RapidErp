using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

namespace Rapid.FinancialManager
{
    public partial class T_AccountsPayable_Deatil : System.Web.UI.Page
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
            string orderNumber = ToolManager.GetQueryString("OrderNumber").Trim();
            string createTime = ToolManager.GetQueryString("CreateTime").Trim();
            string condion = string.Format(" where 采购订单号='{0}' and 创建时间='{1}' ", orderNumber, createTime);
            string sql = string.Format(@"  select * from V_T_AccountsPayable_Detail {0}", condion);
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
