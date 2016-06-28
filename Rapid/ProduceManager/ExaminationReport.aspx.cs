using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Rapid.ToolCode;
using System.Data;
using BLL;

namespace Rapid.ProduceManager
{
    public partial class ExaminationReport : System.Web.UI.Page
    {
        public DataTable DtResult = new DataTable();
        public string year = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                Bind();
                //this.rpList.DataSource = SqlHelper.GetTable(Bind());
                //this.rpList.DataBind();
            }
        }

        private void Bind()
        {
            DtResult = GetTable();
        }

        private DataTable GetTable()
        {
            year = drpYear.SelectedValue;
            string name = txtName.Text.Trim();
            string condition = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                condition += string.Format(" AND Name Like '%{0}%'", name);
            }
            string sql = string.Format(@"
WITH A AS
  (SELECT {0} AS 年份, Name AS 员工姓名 , Sum(CASE [Month] WHEN 1 THEN TotalScore ELSE 0 END) AS '1月',
                                   Sum(CASE [Month] WHEN 2 THEN TotalScore ELSE 0 END) AS '2月',
                                   Sum(CASE [Month] WHEN 3 THEN TotalScore ELSE 0 END) AS '3月',
                                   Sum(CASE [Month] WHEN 4 THEN TotalScore ELSE 0 END) AS '4月',
                                   Sum(CASE [Month] WHEN 5 THEN TotalScore ELSE 0 END) AS '5月',
                                   Sum(CASE [Month] WHEN 6 THEN TotalScore ELSE 0 END) AS '6月',
                                   Sum(CASE [Month] WHEN 7 THEN TotalScore ELSE 0 END) AS '7月',
                                   Sum(CASE [Month] WHEN 8 THEN TotalScore ELSE 0 END) AS '8月',
                                   Sum(CASE [Month] WHEN 9 THEN TotalScore ELSE 0 END) AS '9月',
                                   Sum(CASE [Month] WHEN 10 THEN TotalScore ELSE 0 END) AS '10月',
                                   Sum(CASE [Month] WHEN 11 THEN TotalScore ELSE 0 END) AS '11月',
                                   Sum(CASE [Month] WHEN 12 THEN TotalScore ELSE 0 END) AS '12月',
                                   Sum(ISNULL(TotalScore,0)) AS '总分'
   FROM ExaminationLog
   WHERE [Year] = {0} {1}
   GROUP BY Name) ,
     B AS
  (SELECT PM_USER.USER_NAME,
          ISNULL(PM_ROLE.ROLE_NAME,'') AS ROLE_NAME
   FROM PM_USER
   LEFT JOIN PM_ROLE ON PM_USER.RoleId=pm_role.ROLE_ID),
     C AS
  ( SELECT name ,
           count(0) AS 总月数
   FROM ExaminationLog
   WHERE YEAR={0} {1}
   GROUP BY name)
SELECT ROW_NUMBER() over(
                         ORDER BY round(A.总分/C.总月数,2) DESC) AS '名次',
       isnull(B.ROle_Name,'') AS '角色',
       A.*,
       cast(round(A.总分/C.总月数,2) AS numeric(18,2)) AS 平均分
FROM A
LEFT JOIN B ON A.员工姓名=B.User_Name
LEFT JOIN C ON A.员工姓名=C.Name
", year, condition);
            DataTable dt = SqlHelper.GetTable(sql);
            DataTable dtValue = dt.Clone();
            DataTable DtTemp = SqlHelper.GetTable(string.Format("select * from ExaminationLog where year={0} {1}", year, condition));
            //修改列的数据类型
            for (int i = 4; i <= 15; i++)
            {
                dtValue.Columns[i].DataType = typeof(string);
            }
            foreach (DataRow dr in dt.Rows)
            {
                dtValue.ImportRow(dr);
            }
            foreach (DataRow dr in dtValue.Rows)
            {
                for (int i = 4; i <= 15; i++)
                {
                    if (Convert.ToDouble(dr[i]) == Convert.ToDouble(0))
                    {
                        dr[i] = "";
                    }
                }
            }
            //foreach (DataRow dr in dtValue.Rows)
            //{
            //    for (int i = 4; i <= 15; i++)
            //    {
            //        if (DtTemp.Select("Month=" + (i - 3) + " and Name='" + dr["员工姓名"] + "'").Length <= 0)
            //        {
            //            dr[i] = "";
            //        }
            //        if (Convert.ToInt32((dr[i])) > 0) Count++;
            //    }
            //}
            return dtValue;

            //DtResult.Columns.Add("平均分", typeof(double));
            //int Count = 0;
            //foreach (DataRow dr in DtResult.Rows)
            //{
            //    Count = 0;
            //    for (int i = 4; i <= 15; i++) //统计有分数的总月数
            //    {
            //        if (Convert .ToInt32( (dr[i])) > 0) Count++;
            //    }
            //    if (Count > 0)
            //    {
            //        dr["平均分"] = Convert.ToDouble(dr["总分"]) / Count;
            //    }
            //}
            //DtResult.DefaultView.Sort = "平均分 desc";

            //            string sql = "select * from  V_ExaminationReport_1";
            //            string condtion = " where 1=1  "; //查询条件
            //            if (drpYear.SelectedValue != "")
            //            {
            //                condtion += string.Format(" and 年份='{0}'", drpYear.SelectedValue);
            //            }
            //            if (txtName.Text != "")
            //            {
            //                condtion += " and 员工姓名 like '%" + txtName.Text.Trim() + "%'";
            //            }
            //            sql = sql + condtion;
            //            string tempSql = string.Empty;//记录平均分

            //            int i = 0;//记录有分数的月数量
            //            DataTable dt = SqlHelper.GetTable(sql);
            //            foreach (DataRow dr in dt.Rows)
            //            {
            //                i = 0;
            //                for (int a = 2; a <= 13; a++)
            //                {
            //                    if (!dr[a].ToString().Equals(""))
            //                    {
            //                        i++;
            //                    }
            //                }
            //                tempSql += string.Format(@" select '{0}' as 年份,'{1}' as 员工姓名, CAST( {2} as decimal(18,2))/{3} as 平均分新
            //union all   ", dr["年份"], dr["员工姓名"], dr["总分"], i);
            //            }

            //            tempSql = tempSql.TrimEnd(new char[] { 'u', 'n', 'i', 'o', 'n', ' ', 'a', 'l', 'l' });

            //            sql = string.Format(@" select t.*,cast( a.平均分新 as decimal(18,2)) as 平均分新,
            // ROW_NUMBER ()over(  order by a.平均分新 desc)as 名次 
            //from ({0}) t inner join ({1}) a on t.年份=a.年份 and t.员工姓名=a.员工姓名 ", sql, tempSql);

            //            ProductManager.WriteAverageForYear(drpYear.SelectedValue, "考试", "是", sql, "员工姓名");
            //            // insert into T_TempScore(Year,UserName,Average,Type,IsProduct)
            //            return sql;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
            //this.rpList.DataSource = SqlHelper.GetTable(Bind());
            //this.rpList.DataBind();
        }

        protected void btnExcel_Click1(object sender, EventArgs e)
        {
            DtResult = GetTable();
            if (DtResult.Rows.Count <= 0)
            {
                return;
            }
            ToolCode.Tool.ExpExcel(DtResult, "员工考试月度报表(" + year + ")");

        }

        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
