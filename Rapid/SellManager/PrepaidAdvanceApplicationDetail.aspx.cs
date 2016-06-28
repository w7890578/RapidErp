using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.SellManager
{
    public partial class PrepaidAdvanceApplicationDetail : System.Web.UI.Page
    {
        public static string show = "inline";
        public static string fatherGuid = ToolManager.GetQueryString("fatherGuid");
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
            string isYS = ToolManager.GetQueryString("isYS");
            string createTime = ToolManager.GetQueryString("CreateTime");
            string deliveryNumber = ToolManager.GetQueryString("deliveryNumber");
            string ordersnumber = ToolManager.GetQueryString("OrdersNumber");
            string condition = " where 销售订单号='" + ordersnumber + "'";
            if (txtCustomerMaterialNumber.Text != "")
            {
                condition += " and 客户物料编号 like '%" + txtCustomerMaterialNumber.Text.Trim() + "%'";
            }
            if (txtCustomerOdersNumber.Text != "")
            {
                condition += " and 客户采购订单号 like '%" + txtCustomerOdersNumber.Text.Trim() + "%'";
            }
            if (txtProductNumber.Text != "")
            {
                condition += " and 瑞普迪编号 like '%" + txtProductNumber.Text.Trim() + "%'";
            }
            if (isYS != "1")
            {
                condition += " and 创建时间='" + createTime + "' ";
                condition += " and  送货单号='" + deliveryNumber + "' ";
            }
            string sql = string.Format(@" select * from (select va.*,ta.InvoiceNumber as 发票号码,ta.InvoiceDate as 开票日期 from V_AccountsReceivableDetail va inner join T_AccountsReceivable_Detail 
ta on va.guid=ta.guid)t {0} union all 
select '','合计','','','','','','',SUM(交货数量),'',SUM(总价),'','','','','','' from V_AccountsReceivableDetail {0}", condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
            fatherGuid = ToolManager.GetQueryString("fatherGuid");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
