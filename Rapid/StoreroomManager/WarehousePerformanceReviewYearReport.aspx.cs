using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.StoreroomManager
{
    public partial class WarehousePerformanceReviewYearReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = string.Format(@"select *,ROW_NUMBER ()over(  order by 总分数 desc)as 名次 from V_WarehousePerformanceReviewYearReport 
 where 年份='{0}'", DateTime.Now.ToString("yyyy"));
                this.rpList.DataSource = SqlHelper.GetTable(sql);
                this.rpList.DataBind();
            }
        }
        private string Bind()
        {
            string sql = " select *,ROW_NUMBER ()over(  order by 总分数 desc)as 名次 from V_WarehousePerformanceReviewYearReport";
            string condtion = " where 1=1"; //查询条件
            if (drpYear.SelectedValue == "")
            {
                condtion += " and 年份='" + DateTime.Now.ToString("yyyy") + "'";
            }
            else if (drpYear.SelectedValue != "")
            {
                condtion += " and 年份='" + drpYear.SelectedValue + "'";
            }
            if (txtName.Text != "")
            {
                condtion += " and 员工姓名 like '%" + txtName.Text.Trim() + "%'";
            }

            sql = sql + condtion;
            return sql;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.rpList.DataSource = SqlHelper.GetTable(Bind());
            this.rpList.DataBind();
        }

    }
}
