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
    public partial class ExpiredOrderViewLis : System.Web.UI.Page
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
            string str = string.Format(@"select DATEADD(dd,DATEDIFF(dd,0,'" + drpMonth.SelectedValue + "'),-5)");
            string temp =Convert.ToDateTime( SqlHelper.GetScalar(str)).ToString("yyyy-MM-dd");
            string sql = string.Format(@"exec P_ExpiredOrder '"+temp+"'");
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();
        }
      

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetYearMonth();
        }
    }
}
