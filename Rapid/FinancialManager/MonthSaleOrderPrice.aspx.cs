using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class MonthSaleOrderPrice : System.Web.UI.Page
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
            string condition = "WHERE 1=1";
            string userId = ToolCode.Tool.GetUser().UserNumber;
            drpYear.SelectedValue = DateTime.Now.Year.ToString();
            drpMonth.SelectedValue = DateTime.Now.Month.ToString();
            if (txtCustomerName.Text!= "")
            {
                condition += " and CustomerId= '" + txtCustomerName.Text.Trim() + "'";
            }
            string p_sql = string.Format(@"exec P_MonthSaleOderPrice '" + userId + "','" + drpYear.SelectedValue + "','" + drpMonth.SelectedValue + "','" + txtCustomerName.Text + "'");
            string sql = string.Format(@"select * from T_MonthSaleOderPrice_temp {0}",condition);
            return sql;
           
        }

        private void Bind()
        {
            this.rptList.DataSource = SqlHelper.GetTable(GetSql());
            this.rptList.DataBind();
        }
    

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(), "月度销售额统计表");
        }
    }
}