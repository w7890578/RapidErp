using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class PrepaidAdvanceStatisticsReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        private string GetSql()
        {
            string condinton = " where 是否为预收='是'";
            if (txtOdersNumber.Text != "")
            {
                condinton += " and 销售订单号 like '%" + txtOdersNumber.Text.Trim() + "%'";
            }
            if (txtCustomerOdersNumber.Text != "")
            {
                condinton += " and 客户采购订单号 like '%" + txtCustomerOdersNumber.Text.Trim() + "%'";
            }
            if (drpType.SelectedValue != "")
            {
                condinton += " and 收款类型='" + drpType.SelectedValue + "'";
            }
            if (drpisSettle.SelectedValue != "")
            {
                condinton += " and 是否结清='" + drpisSettle.SelectedValue + "'";
            }
            if (txtCustomerName.Text != "")
            {
                condinton += " and 客户名称 like '%" + txtCustomerName.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from [V_PrepaidAdvanceReport] {0}
union all
 select '合计','','','','','','','','','','','',SUM(交货数量),'',SUM(总价),'','' from [V_PrepaidAdvanceReport] {0}
", condinton);
            return sql;
        }
        private void Bind()
        {

            string sql = GetSql();
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string sql = GetSql();
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "预收账款统计报表");
        }
    }
}
