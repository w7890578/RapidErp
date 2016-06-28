using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using BLL;
using System.Text;

namespace Rapid.StoreroomManager
{
    public partial class WarehousePerformanceReviewMonthReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                // string sql = string.Format(@" select ROW_NUMBER ()over(  order by 总分 desc)as 名次 ,* from V_WarehousePerformanceReviewMonthReport_List where 年份='{0}'", DateTime.Now.ToString("yyyy"));
                BindRepter();
            }
        }
        private string GetSql()
        {
            //string sql = "select ROW_NUMBER ()over(  order by 总分 desc)as 名次 ,* from  V_WarehousePerformanceReviewMonthReport_List";
            //string condtion = " where 1=1  "; //查询条件
            //if (drpYear.SelectedValue != "")
            //{
            //    condtion += string.Format(" and 年份='{0}'", drpYear.SelectedValue);
            //}
            //if (txtName.Text != "")
            //{
            //    condtion += " and 姓名 like '%" + txtName.Text.Trim() + "%'";
            //}
            //sql = sql + condtion;
            string sql = string.Format(@"select * from  (
select 年份,姓名,SUM([1月]) as January,SUM([2月]) as February ,SUM([3月]) as March,
SUM([4月]) as April,SUM([5月]) as May ,SUM([6月]) as June ,SUM([7月]) as July,SUM([8月]) as August,
SUM([9月]) as September,SUM([10月]) as October,SUM([11月]) as November,
SUM([12月]) as December,SUM(总分) as 总分,SUM(平均分) as 平均分
from (
select * from(
select pf.Year as 年份,pf.Name as 姓名,pf.PerformanceReviewItem as 考核项目,Type as 考核类型,
 ISNULL ( temp1.[1月] ,'') as '1月',
 ISNULL ( temp2.[2月] ,'') as '2月',
 ISNULL ( temp3.[3月] ,'') as '3月',
 ISNULL ( temp4.[4月],'') as '4月',
 ISNULL ( temp5.[5月] ,'') as '5月',
 ISNULL (temp6.[6月] ,'') as '6月',
 ISNULL (temp7.[7月],'') as '7月',
 ISNULL ( temp8.[8月] ,'') as '8月',
 ISNULL (temp9.[9月] ,'') as '9月',
 ISNULL ( temp10.[10月] ,'') as '10月',
 ISNULL (temp11.[11月] ,'') as '11月',
 ISNULL (temp12.[12月] ,'') as '12月', 
 ISNULL ( temp1.[1月] ,0)+ISNULL ( temp2.[2月] ,0)+ISNULL ( temp3.[3月] ,0)+
 ISNULL ( temp4.[4月] ,0)+ISNULL ( temp5.[5月] ,0)+ISNULL ( temp6.[6月] ,0)+
 ISNULL ( temp7.[7月] ,0)+ISNULL ( temp8.[8月] ,0)+ISNULL ( temp9.[9月] ,0)+
 ISNULL ( temp10.[10月] ,0)+ISNULL ( temp11.[11月] ,0)+ISNULL ( temp12.[12月] ,0) as 总分,
   CAST (round( 
 (ISNULL ( temp1.[1月] ,0)+ISNULL ( temp2.[2月] ,0)+ISNULL ( temp3.[3月] ,0)+
 ISNULL ( temp4.[4月] ,0)+ISNULL ( temp5.[5月] ,0)+ISNULL ( temp6.[6月] ,0)+
 ISNULL ( temp7.[7月] ,0)+ISNULL ( temp8.[8月] ,0)+ISNULL ( temp9.[9月] ,0)+
 ISNULL ( temp10.[10月] ,0)+ISNULL ( temp11.[11月] ,0)+ISNULL ( temp12.[12月] ,0))/12,2) as numeric(20,2)) as 平均分
 
 from (
(select distinct Name,PerformanceReviewItem,YEAR ,Type from PerformanceReviewLog )pf
left join (select  YEAR,name,PerformanceReviewItem ,score as '1月' from PerformanceReviewLog where [MONTH] ='1' and [year]='{0}') temp1 
on pf.Name =temp1 .Name and pf.PerformanceReviewItem =temp1 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '2月' from PerformanceReviewLog where [MONTH] ='2' and [year]='{0}') temp2 
on pf.Name =temp2 .Name and pf.PerformanceReviewItem =temp2 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '3月' from PerformanceReviewLog where [MONTH] ='3' and [year]='{0}') temp3 
on pf.Name =temp3 .Name and pf.PerformanceReviewItem =temp3 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '4月' from PerformanceReviewLog where [MONTH] ='4' and [year]='{0}') temp4 
on pf.Name =temp4 .Name and pf.PerformanceReviewItem =temp2 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '5月' from PerformanceReviewLog where [MONTH] ='5' and [year]='{0}') temp5
on pf.Name =temp5 .Name and pf.PerformanceReviewItem =temp5 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '6月' from PerformanceReviewLog where [MONTH] ='6' and [year]='{0}') temp6 
on pf.Name =temp6 .Name and pf.PerformanceReviewItem =temp6 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '7月' from PerformanceReviewLog where [MONTH] ='7' and [year]='{0}') temp7 
on pf.Name =temp7 .Name and pf.PerformanceReviewItem =temp7 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '8月' from PerformanceReviewLog where [MONTH] ='8' and [year]='{0}') temp8
on pf.Name =temp8.Name and pf.PerformanceReviewItem =temp8.PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '9月' from PerformanceReviewLog where [MONTH] ='9' and [year]='{0}') temp9
on pf.Name =temp9 .Name and pf.PerformanceReviewItem =temp9 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '10月' from PerformanceReviewLog where [MONTH] ='10' and [year]='{0}') temp10
on pf.Name =temp10 .Name and pf.PerformanceReviewItem =temp10 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '11月' from PerformanceReviewLog where [MONTH] ='11' and [year]='{0}') temp11 
on pf.Name =temp11.Name and pf.PerformanceReviewItem =temp11 .PerformanceReviewItem 
left join (select  YEAR,name,PerformanceReviewItem ,score as '12月' from PerformanceReviewLog where [MONTH] ='12' and [year]='{0}') temp12
on pf.Name =temp12 .Name and pf.PerformanceReviewItem =temp12 .PerformanceReviewItem 
) ) te where 考核类型!='生产')H  group by 年份,姓名)T", drpYear.SelectedValue);
            string condtion = " where 1=1  "; //查询条件
            if (drpYear.SelectedValue != "")
            {
                condtion += string.Format(" and 年份='{0}'", drpYear.SelectedValue);
            }
            if (txtName.Text != "")
            {
                condtion += " and 姓名 like '%" + txtName.Text.Trim() + "%'";
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
                    if (!dr[a].ToString().Equals("0"))
                    {
                        i++;
                    }
                }
                tempSql += string.Format(@" select '{0}' as 年份,'{1}' as 姓名, CAST( {2} as decimal(18,2))/{3} as 平均分新
union all   ", dr["年份"], dr["姓名"], dr["总分"], i);
            }

            tempSql = tempSql.TrimEnd(new char[] { 'u', 'n', 'i', 'o', 'n', ' ', 'a', 'l', 'l' });

            sql = string.Format(@" select t.*,cast( a.平均分新 as decimal(18,2)) as 平均分新,
 ROW_NUMBER ()over(  order by a.平均分新 desc)as 名次 
from ({0}) t inner join ({1}) a on t.年份=a.年份 and t.姓名=a.姓名 ", sql, tempSql);
            ProductManager.WriteAverageForYear(drpYear.SelectedValue, "绩效", "否", sql, "姓名");
            return sql;

        }
        private void BindRepter()
        {
            DataView dv = Compute();
            this.rpList.DataSource = dv;
            this.rpList.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindRepter();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
              DataView dv = Compute();
              ExpExcel(dv, "库房员工绩效月度报表");
            //  ToolCode.Tool.ExpExcel(GetSql(), "库房员工绩效月度报表");
        }

        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRepter();
        }

        protected DataView Compute()
        {
            string type = "库房及其它";
            string year = drpYear.SelectedValue;
            string name = txtName.Text.Trim();
            string sql = string.Format("select distinct Name from PerformanceReviewLog where [year]='{0}' and type='{1}'", year, type);
            DataTable dt = new DataTable();
            dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count <= 0)
            {
                return dt.DefaultView;
            }
            StringBuilder sqltemp = new StringBuilder();
            foreach (DataRow dr in dt.Rows)
            {
                sqltemp.AppendFormat("union select '{0}' as UserName  ", dr["Name"]);
                for (int i = 1; i <= 12; i++)
                {
                    sqltemp.AppendFormat(@", isnull((
select SUM(isnull(score,0)) from PerformanceReviewLog where [YEAR]='{0}' and [MONTH]='{1}' and type='{2}' and name='{3}'
group by [YEAR],[MONTH]),0) as '{1}'", year, i, type, dr["Name"]);
                }
            }
            DataTable dtResult = SqlHelper.GetTable(sqltemp.ToString().Remove(0, 5));


            if (!string.IsNullOrEmpty(name))
            {
                DataRow[] drs = dtResult.Select("UserName='" + name + "'");
                dtResult = dtResult.Clone();
                foreach (DataRow dr in drs)
                {
                    dtResult.ImportRow(dr);
                }
            }


            dtResult.Columns.Add(new DataColumn("SumScore", typeof(int)));
            dtResult.Columns.Add(new DataColumn("Average", typeof(int)));
            dtResult.Columns.Add(new DataColumn("Year", typeof(string)));

            //计算总分
            foreach (DataRow dr in dtResult.Rows)
            {
                int sumScore = 0;
                int monthCount = 0;
                for (int i = 1; i <= 12; i++)
                {
                    int score = (dr[i] == null ? 0 : Convert.ToInt32(dr[i]));
                    if (score > 0)
                    {
                        monthCount++;
                        sumScore += score;
                    }
                    //sumScore += (dr[i] == null ? 0 : Convert.ToInt32(dr[i]));
                }
                dr["SumScore"] = sumScore;
                dr["Average"] = monthCount == 0 ? 0 : Convert.ToDouble(sumScore) / Convert.ToDouble(monthCount);
                dr["Year"] = year;

            }
            DataView dv = dtResult.DefaultView;
            dv.Sort = "SumScore Desc";
            return dv;
        }
        #region 导出Excel通用方法
        /// <summary>
        /// 导出Execl通用方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="fileName"></param>
        public void ExpExcel(DataView dv, string fileName)
        {
            string error = string.Empty;

            GridView gvOrders = new GridView();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8).ToString());
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");  //设置输出流为简体中文 
            System.Web.UI.Page page = new System.Web.UI.Page();
            page.EnableViewState = false;
            HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=GB2312\">");
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            gvOrders.AllowPaging = false;
            gvOrders.AllowSorting = false;
            gvOrders.DataSource = dv;
            gvOrders.DataBind();
            gvOrders.RenderControl(hw);
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
        #endregion
    }
}
