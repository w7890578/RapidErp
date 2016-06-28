using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class T_ExaminationLog_KF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string sql = string.Format(@" select ROW_NUMBER ()over(  order by 总分 desc)as 名次 ,* from [V_T_ExaminationLog_KF] where 年份='{0}'", DateTime.Now.ToString("yyyy"));
                //this.rpList.DataSource = SqlHelper.GetTable(sql);
                //this.rpList.DataBind();
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                this.rpList.DataSource = SqlHelper.GetTable(Bind());
                this.rpList.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.rpList.DataSource = SqlHelper.GetTable(Bind());
            this.rpList.DataBind();
        }
        private string Bind()
        {
            //string sql = "select ROW_NUMBER ()over(  order by 总分 desc)as 名次 ,* from [V_T_ExaminationLog_KF]";
            //string condtion = " where 1=1  "; //查询条件
            //if (txtName.Text != "")
            //{
            //    condtion += " and 员工姓名 like '%" + txtName.Text.Trim() + "%'";
            //}
            //if (drpYear.SelectedValue != "")
            //{

            //    condtion += string.Format(" and 年份='{0}' ", drpYear.SelectedValue);
            //}


            //sql = sql + condtion;
            string sql = "select * from  V_T_ExaminationLog_KF";
            string condtion = " where 1=1  "; //查询条件
            if (drpYear.SelectedValue != "")
            {
                condtion += string.Format(" and 年份='{0}'", drpYear.SelectedValue);
            }
            if (txtName.Text != "")
            {
                condtion += " and 员工姓名 like '%" + txtName.Text.Trim() + "%'";
            }
            sql = sql + condtion;
            string tempSql = string.Empty;//记录平均分

            int i = 0;//记录有分数的月数量
            DataTable dt = SqlHelper.GetTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                i = 0;
                for (int a = 2; a <= 13; a++)
                {
                    if (!dr[a].ToString().Equals(""))
                    {
                        i++;
                    }
                }
                tempSql += string.Format(@" select '{0}' as 年份,'{1}' as 员工姓名, CAST( {2} as decimal(18,2))/{3} as 平均分新
union all   ", dr["年份"], dr["员工姓名"], dr["总分"], i);
            }

            tempSql = tempSql.TrimEnd(new char[] { 'u', 'n', 'i', 'o', 'n', ' ', 'a', 'l', 'l' });

            sql = string.Format(@" select t.*,cast( a.平均分新 as decimal(18,2)) as 平均分新,
 ROW_NUMBER ()over(  order by a.平均分新 desc)as 名次 
from ({0}) t inner join ({1}) a on t.年份=a.年份 and t.员工姓名=a.员工姓名 ", sql, tempSql);

            ProductManager.WriteAverageForYear(drpYear.SelectedValue, "考试", "否", sql, "员工姓名");
            return sql;

        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Bind()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(Bind(), "库房员工考试月度报表");
        }

        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rpList.DataSource = SqlHelper.GetTable(Bind());
            this.rpList.DataBind();
        }
    }
}
