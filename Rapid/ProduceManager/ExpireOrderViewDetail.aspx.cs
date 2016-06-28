using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using BLL;

namespace Rapid.ProduceManager
{
    public partial class ExpireOrderViewDetail : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //txtLeadTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
                Bind();
            }
        }

        private string GetConditon()
        {
            string condition = " where 1=1";
            if (txtOdersNumber.Text != "")
            {
                condition += " and 销售订单号 like '%" + txtOdersNumber.Text.Trim() + "%'";
            }
            if (txtProductNumber.Text != "")
            {
                condition += " and 产成品编号 like '%" + txtProductNumber.Text.Trim() + "%'";
            }
            if (txtLeadTime.Text != "")
            {
                condition += " and 交期 <= '" + txtLeadTime.Text.Trim() + "'";
            }
            if (txtCustomerOrderNumber.Text != "")
            {
                condition += " and 客户采购订单号 like '%" + txtCustomerOrderNumber.Text.Trim() + "%'";
            }
            return condition;
        }

        private void Bind()
        {

            string sql = string.Format(@" select * from (select * from V_ExpireOrderViewDetail {0}  
union 
select '合计','','','','','','','2999-01-01',SUM(数量),SUM(未交数量) from V_ExpireOrderViewDetail {0})t order by 交期 asc 
", GetConditon());
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
            // return;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
//            string sql = string.Format(@"select t.*,'' as 未交原因,'' as 欠料预计交期,'' as 预计交货日期 from ( 
//select * from V_ExpireOrderViewDetail {0})t  order by t.交期 asc", GetConditon());

            string sql = string.Format(@" select  t.*,'' as 未交原因,'' as 欠料预计交期,'' as 预计交货日期 from (
select * from V_ExpireOrderViewDetail {0}  
union 
select '合计','','','','','','','',SUM(数量),SUM(未交数量) from V_ExpireOrderViewDetail {0})t order by 交期 asc 
", GetConditon());
            ToolCode.Tool.ExpExcel(sql, "过期未交订单");
        }
    }

}
