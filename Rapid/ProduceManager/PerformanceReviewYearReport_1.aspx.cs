using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class PerformanceReviewYearReport_1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                this.rpList.DataSource = SqlHelper.GetTable(Bind());
                this.rpList.DataBind();
            }
        }
        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Bind()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(Bind(), "员工绩效年度报表");
        }
        private string Bind()
        {
            string sql = @" 
WITH A
     AS (SELECT Name,
                Sum(Isnull(TotalScore, 0)) / Count(0) AS score
         FROM   ExaminationLog
         WHERE  YEAR = {0} {1}
         GROUP  BY Name
         UNION
         SELECT Name,
                Sum(Isnull(SumScore, 0)) / Count(0) AS score
         FROM   T_ExaminationLog_KF
         WHERE  YEAR = {0} {1}
         GROUP  BY Name),
     B
     AS (SELECT Name,
                Sum(Isnull(Score, 0)) / Count(0) AS score
         FROM   (SELECT Name,
                        Sum(Isnull(Score, 0)) Score,
                        MONTH
                 FROM   PerformanceReviewLog
                 WHERE  YEAR = {0} {1}
                 GROUP  BY Name,
                           MONTH)t
         GROUP  BY t.Name)
SELECT {0}                                                       AS 年份,
       Row_number()
         OVER(
           ORDER BY (Isnull(A.score, 0)+Isnull(B.score, 0)) DESC) AS 名次,
       PM_USER.USER_NAME                                          AS 员工姓名,
       Cast(Round(Isnull(A.score, 0), 2) AS NUMERIC(12, 2))       AS 考试,
       Cast(Round(Isnull(B.score, 0), 2) AS NUMERIC(12, 2))       AS 绩效,
       Cast(Round( Isnull(A.score, 0), 2)
            + Round(Isnull(B.score, 0), 2) AS NUMERIC(12, 2))     AS 总分数
FROM   PM_USER
       LEFT JOIN A
              ON PM_USER.USER_NAME = A.Name
       LEFT JOIN B
              ON PM_USER.USER_NAME = B.Name
";
            string condtion = string.Empty;
            if (!string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                condtion += " and Name Like '%" + txtName.Text.Trim() + "%'";
            }
            sql = string.Format(sql,drpYear.SelectedValue,condtion);
            //string condtion = " where 1=1"; //查询条件
            // if (drpYear.SelectedValue!="")
            //{
            //    condtion +=" and 年份='"+drpYear.SelectedValue+"'";
            //}
            //if (txtName.Text != "")
            //{
            //    condtion += " and 员工姓名 like '%" + txtName.Text.Trim() + "%'";
            //}

            //sql = sql + condtion;
            return sql;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.rpList.DataSource = SqlHelper.GetTable(Bind());
            this.rpList.DataBind();
        }

        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rpList.DataSource = SqlHelper.GetTable(Bind());
            this.rpList.DataBind();
        }
    }
}
