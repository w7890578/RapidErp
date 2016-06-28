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
    public partial class MonthRejectsProductReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                Bind();
            }

        }

        private string GetSql()
        {
            string condition = " where 1=1";
            if(drpYear.SelectedValue!="")
            {
                condition+=" and 年度='"+drpYear.SelectedValue+"'";
            }
            if (txtTeam.Text != "")
            {
                condition += " and 班组 like '%" + txtTeam.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select t.班组,t.年度, t.[1月],t.[2月],t.[3月],t.[4月],t.[5月],t.[6月],t.[7月],t.[8月],t.[9月],t.[10月],t.[11月],t.[12月] from 
(select 班组,年度,SUM(CAST( [1月] as int )) as '1月',
  SUM(CAST( [2月] as int )) as '2月',
  SUM(CAST( [3月] as int )) as '3月',
  SUM(CAST( [4月] as int )) as '4月',
  SUM(CAST( [5月] as int )) as '5月',
  SUM(CAST( [6月] as int )) as '6月',
  SUM(CAST( [7月] as int )) as '7月',
  SUM(CAST( [8月] as int )) as '8月',
  SUM(CAST( [9月] as int )) as '9月',
  SUM(CAST( [10月] as int )) as '10月',
 SUM(CAST( [11月] as int )) as '11月',
  SUM(CAST( [11月] as int )) as '12月'
  from V_RejectsProductYearReport {0}  group by 班组,年度)t  
 union
 
 select '月总数' as 月总数, '',SUM(CAST( [1月] as int )) as '1月',
  SUM(CAST( [2月] as int )) as '2月',
  SUM(CAST( [3月] as int )) as '3月',
  SUM(CAST( [4月] as int )) as '4月',
  SUM(CAST( [5月] as int )) as '5月',
  SUM(CAST( [6月] as int )) as '6月',
  SUM(CAST( [7月] as int )) as '7月',
  SUM(CAST( [8月] as int )) as '8月',
  SUM(CAST( [9月] as int )) as '9月',
  SUM(CAST( [10月] as int )) as '10月',
 SUM(CAST( [11月] as int )) as '11月',
  SUM(CAST( [11月] as int )) as '12月' from  V_RejectsProductYearReport {0}", condition);
            return sql;
        }

        protected void btnExcel_Click1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(), "月度不合格品报表");
        }

        private void Bind()
        {
            rpList.DataSource = SqlHelper.GetTable(GetSql());
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
