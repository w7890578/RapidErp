using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.FinancialManager
{
    public partial class MarerialScrapLogListDetailList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                thisMonthEnd();
            }
        }

        public void thisMonthEnd()
        {
            string year = ToolManager.GetQueryString("year");
            string month = ToolManager.GetQueryString("month");
            string temp = Convert.ToDateTime(month).ToString("yyyy-MM-01");
            string statrDateTime = ToolManager.GetQueryString("startDateTime");
            string endDateTime = ToolManager.GetQueryString("endDateTime");

            //string sql = string.Format(@"exec P_MarerialScrapLogDetail '" + temp + "','" + month + "'");
            string sql = string.Format("exec P_MarerialScrapLogDetail '{0}','{1}','{2}','{3}' ",
                temp, month, statrDateTime, endDateTime);
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string year = ToolManager.GetQueryString("year");
            string month = ToolManager.GetQueryString("month");
            string temp = Convert.ToDateTime(month).ToString("yyyy-MM-01");
            string statrDateTime = ToolManager.GetQueryString("startDateTime");
            string endDateTime = ToolManager.GetQueryString("endDateTime");

            //string sql = string.Format(@"exec P_MarerialScrapLogDetail '" + temp + "','" + month + "'");
            string sql = string.Format("exec P_MarerialScrapLogDetail '{0}','{1}','{2}','{3}' ",
                temp, month, statrDateTime, endDateTime);
            string fileName = string.Empty;

            if (!string.IsNullOrEmpty(statrDateTime)
                && !string.IsNullOrEmpty(endDateTime))
                fileName = string.Format("{0}至{1}原材料报废明细", statrDateTime, endDateTime);
            else
                fileName = string.Format("{0}至{1}原材料报废明细", temp, month);

            DataTable dt = SqlHelper.GetTable(sql);
            ExcelHelper.Instance.ExpExcel(dt, fileName);
        }
    }
}
