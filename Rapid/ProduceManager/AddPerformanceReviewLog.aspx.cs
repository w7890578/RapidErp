using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

namespace Rapid.ProduceManager
{
    public partial class AddPerformanceReviewLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                drpMonth.SelectedValue = DateTime.Now.Month.ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string year = drpYear.SelectedValue;
            string month = drpMonth.SelectedValue;
            string error = string.Empty;
            List<string> sqls = new List<string>();
            string sql = string.Format(@" select count(*) from PerformanceReviewLog where year ='{0}' and month='{1}' and type!='库房及其它' "
                , year, month);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = string.Format("已上报过{0}年{1}月的绩效", year, month);
                return;
            }
            sql = string.Format(@"  
insert into PerformanceReviewLog(Year ,Month,Name ,PerformanceReviewItem ,RowNumber 
 ,FullScore ,Deduction,Score ,Description ,StatMode,Type)
 select '{0}','{1}',USER_NAME ,prs.PerformanceReviewItem,prs.RowNumber ,prs.FullScore
 ,0,prs.FullScore ,prs.Description ,prs.StatMode ,p.Type from PM_USER  p 
 inner join PerformanceReviewStandard prs on p.StandardName=prs.StandardName
 where ISNULL ( p.StandardName,'') !='' and p.Type!='库房及其它'", year, month);
            sqls.Add(sql);

            sql = string.Format(@"select FullScore  from PerformanceReviewStandard  where [Type]='生产' and PerformanceReviewItem ='技术考核' ");
            string fullScore = SqlHelper.GetScalar(sql); //技术考核满分
            //更新技术考核得分
            sql = string.Format(@"
update  PerformanceReviewLog   set  Score = case when el.totalScore>=90 then {0}
when el.totalScore >=60 and el.totalScore <=74 then {0}*0.5
when  el.totalScore >=75 and el.totalScore <=89 then {0}*0.75
else 0 end
  from PerformanceReviewLog pfrl left join ExaminationLog el on 
pfrl .Year =el.Year and pfrl.Month =el.Month and pfrl.Name =el.Name 
where pfrl.YEAR ='{1}' and pfrl.MONTH ='{2}' 
and pfrl.PerformanceReviewItem='技术考核' and pfrl.Type='生产'", fullScore, year, month);
            sqls.Add(sql);

            sql = string.Format(" update PerformanceReviewLog set Deduction =FullScore-Score  ");
            sqls.Add(sql);
            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            if (result)
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                lbSubmit.Text = error;
                return;
            }
        }
    }
}
