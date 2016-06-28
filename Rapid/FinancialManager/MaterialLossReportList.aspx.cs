using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

namespace Rapid.FinancialManager
{
    public partial class MaterialLossReportList : System.Web.UI.Page
    { 
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
            string sql = string.Format(@"exec P_MaterialLossReport'" + temp + "','" + drpMonth.SelectedValue + "'");
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();
        }
      
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetYearMonth();
        }

        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            MarerialScrapLogListManager.GetMonthForYear(drpYear.SelectedValue, drpMonth);
        }
    }
}
