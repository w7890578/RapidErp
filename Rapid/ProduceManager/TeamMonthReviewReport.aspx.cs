using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.ProduceManager
{
    public partial class TeamMonthReviewReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                drpMonth.SelectedValue = DateTime .Now.Month.ToString () ;
                string sql = string.Empty;
                sql = string.Format(@"select distinct Team from PM_USER where isnull(Team,'')!=''");
                drpTeam.DataSource = SqlHelper.GetTable(sql);
                drpTeam.DataTextField = "Team";
                drpTeam.DataValueField = "Team";
                drpTeam.DataBind();
                Bind();
            }
           
        }
        private string GetSql()
        {
            string sql = string.Empty;
            sql = string.Format(@" select * from V_Month_Team_Review_Report 
where month(创建时间)='{0}' and Year(创建时间)='{1}' and 班组='{2}'", drpMonth.SelectedValue, drpYear.SelectedValue, drpTeam.SelectedValue);
            return sql;
        }
        private void Bind()
        {
            string sql = string.Empty;
            rpList.DataSource = SqlHelper.GetTable(GetSql());
            rpList.DataBind();
            sql = string.Format(@" select '合计','',SUM (完成数量) as 完成数量,SUM(目标完成工时) 目标完成工时, SUM (实际完成工时 ) 实际完成工时,SUM (差额) as 差额
 from V_Month_Team_Review_Report 
where month(创建时间)='{0}' and Year(创建时间)='{1}' and 班组='{2}' group by 班组", drpMonth.SelectedValue, drpYear.SelectedValue, drpTeam.SelectedValue);
            rpListTotal.DataSource = SqlHelper.GetTable(sql);
            rpListTotal.DataBind();
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
            ToolCode.Tool.ExpExcel(GetSql(),"小组月度绩效报表");
        }
    }
}
