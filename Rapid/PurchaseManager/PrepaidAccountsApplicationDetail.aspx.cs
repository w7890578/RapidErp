using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.PurchaseManager
{
    public partial class PrepaidAccountsApplicationDetail : System.Web.UI.Page
    {
        public  string show = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                if (ToolManager.CheckQueryString("SQ"))
                {
                    show = "none";
                }
                if (ToolManager.CheckQueryString("SP"))
                {
                    show = "none";
                }
                if (!ToolManager.CheckQueryString("OrdersNumber"))
                {
                    Response.Write("未知的采购订单号！");
                    Response.End();
                    return;
                }
                Bind();
            }

        }

        private void Bind()
        {
            string ordersnumber = ToolManager.GetQueryString("OrdersNumber");
            string condition=" where 采购订单号='"+ordersnumber+"'";
            if (txtHDnumber.Text != "")
            {
                condition += " and 采购合同号 like '%" + txtHDnumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@" select * from [V_T_AccountsPayable_YF] {0} 
union all
select '','合计','','','','','','',SUM(采购数量),SUM(到货数量),'',SUM(总价),'','','','','','','','' from V_T_AccountsPayable_YF {0}", condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
