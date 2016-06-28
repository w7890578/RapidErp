using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using System.Data;

namespace Rapid.FinancialManager
{
    public partial class MarerialLossLogDetailList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                thisMonthEnd();
            }

        }

        private string GetSql()
        {
            string year = ToolManager.GetQueryString("year");
            string month = ToolManager.GetQueryString("month");
            string temp = Convert.ToDateTime(month).ToString("yyyy-MM-01");
            string sql = string.Format(@"exec P_MaterialLossReportDetail '" + temp + "','" + month + "'");
            return sql;
        }

        public void thisMonthEnd()
        {
            string sql = GetSql();
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string year = ToolManager.GetQueryString("year");
            string month = ToolManager.GetQueryString("month");
            string sql = GetSql();
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Columns.Contains("供应商物料编号"))
            {
                dt.Columns.Remove("供应商物料编号");
            }
            if (dt.Columns.Contains("客户物料编号"))
            {
                dt.Columns.Remove("客户物料编号");
            }
            ExcelHelper.Instance.ExpExcel(dt, string.Format("原材料损耗明细{0}年{1}月", year, month));
        }
    }
}
