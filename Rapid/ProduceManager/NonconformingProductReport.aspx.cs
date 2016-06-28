using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class NonconformingProductReport : System.Web.UI.Page
    {
        string sql;
        protected void Page_Load(object sender, EventArgs e)
        {
            bind(DateTime.Now.ToString("yyyy-MM"), DateTime.Now.ToString("yyyy-MM"));
        }

        private void bind(string reporttime,string reporttimes)
        {
            sql = string.Format(@"select t.班组,t.数量,'0.00%' as 不合格率,'0.00%' as 合格率 from 
                (select * from (
                select pu.Team as 班组,SUM(rp.Qty) as 数量 from RejectsProduct rp 
                left join PM_USER pu on rp.Name=pu.USER_ID
                where rp.ReportTime like '{0}%' group by pu.Team )t
                union
                select '总计',SUM(t.数量) from (
                select pu.Team as 班组,SUM(rp.Qty) as 数量 from RejectsProduct rp 
                left join PM_USER pu on rp.Name=pu.USER_ID
                where rp.ReportTime like '{1}%' group by pu.Team )t)t", reporttime, reporttimes);
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();
        }
        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            bind(drpYear.SelectedValue, drpYear.SelectedValue);
        }

        protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            string yearmonth=drpYear.SelectedValue+"-"+drpMonth.SelectedValue;
            string yearmonths=drpYear.SelectedValue+"-"+drpMonth.SelectedValue;
            bind(yearmonth, yearmonths);
            drpMonth.SelectedValue = "";
        }
    }
}
