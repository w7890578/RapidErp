using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class AccountsReceiveDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!ToolManager.CheckQueryString("OrdersNumber"))
            {
                Response.Write("未知的销售订单号！");
                Response.End();
                return;
            }
            Bind();
        }

        private void Bind()
        {
            string ordersnumber = ToolManager.GetQueryString("OrdersNumber");
            string createtime = ToolManager.GetQueryString("CreateTime");
            string condition = " where 销售订单号='" + ordersnumber + "' and 创建时间='"+createtime+"'";
            if (txtCustomerMaterialNumber.Text != "")
            {
                condition += " and 客户物料编号 like '%" + txtCustomerMaterialNumber.Text.Trim() + "%'";
            }
            if (txtCustomerOdersNumber.Text != "")
            {
                condition += " and 客户采购订单号 like '%" + txtCustomerOdersNumber.Text.Trim() + "%'";
            }
            if (txtRapidNumber.Text!= "")
            {
                condition += " and 瑞普迪编号 like '%" + txtRapidNumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@" select * from [V_AccountsReceivableDetail] {0} union all 
select '','合计','','','','','','',SUM(交货数量),'',SUM(总价),'','','','' from V_AccountsReceivableDetail {0}", condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
