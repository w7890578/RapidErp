using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class WarehousePerformanceReviewLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                drpMonth.SelectedValue = DateTime.Now.Month.ToString();
                Bind();
            }
        }

        private void Bind()
        {
            string condition = " where 1=1 and 考核类型='库房及其它'";
            if (drpYear.SelectedValue != "")
            {
                condition += " and 年度='" + drpYear.SelectedValue + "'";
            }
            if (drpMonth.SelectedValue != "")
            {
                condition += " and 月份='" + drpMonth.SelectedValue + "'";
            }
            if (txtPerformanceReviewItem.Text != "")
            {
                condition += " and 考核项目='" + txtPerformanceReviewItem.Text + "'";
            }
            if (txtName.Text != "")
            {
                condition += " and 姓名='" + txtName.Text + "'";
            }
            string sql = string.Format(@"select * from V_PerformanceReviewLogList {0}
 union all
 select '合计','','','','100',SUM(满分),SUM(扣分),
 SUM(得分),'','','','',''
  from V_PerformanceReviewLogList {0} ",condition
);
            sql = string.Format(" select * from ({0})t order by 序号 asc ", sql);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string year = drpYear.SelectedValue;
            string month = drpMonth.SelectedValue;
            string sql = string.Format(@" delete PerformanceReviewLog where type='库房及其它' 
and Year='{0}' and month='{1}'", year, month);
            lbMsg.Text = SqlHelper.ExecuteSql(sql, ref error) ? "删除成功" : "删除失败！原因：" + error;
            Bind();
        }

    }
}
