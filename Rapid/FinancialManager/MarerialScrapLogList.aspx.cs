using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class MarerialScrapLogList : System.Web.UI.Page
    {
        //public object qty = 0;
        //public object sumPrice = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MarerialScrapLogListManager.BindDrpForYear(drpYear);
                MarerialScrapLogListManager.GetMonthForYear(drpYear.SelectedValue, drpMonth);
                GetYearMonth();
            }
        }

        private void GetYearMonth()
        {
            string temp = Convert.ToDateTime(drpMonth.SelectedValue).ToString("yyyy-MM-01");
            // string sql = string.Format(@"exec P_MarerialScrapLog '" + temp + "','" + drpMonth.SelectedValue + "'");

            string statrDateTimeStr = startDateTime.Text.Trim();
            string endDateTimeStr = endDateTime.Text.Trim();
            string sql = string.Format("exec P_MarerialScrapLog '{0}','{1}','{2}','{3}'"
                , temp, drpMonth.SelectedValue, statrDateTimeStr, endDateTimeStr);

            DataTable dt = SqlHelper.GetTable(sql);

            lbSumQty.Text = dt.Compute("sum(数量)", "TRUE") == null ? "0" : dt.Compute("sum(数量)", "TRUE").ToString();
            lbSumPrice.Text = dt.Compute("sum(金额)", "TRUE") == null ? "0" : dt.Compute("sum(金额)", "TRUE").ToString();
            this.rpList.DataSource = dt;
            this.rpList.DataBind();
        }



        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            MarerialScrapLogListManager.GetMonthForYear(drpYear.SelectedValue, drpMonth);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetYearMonth();
        }

    }
}
