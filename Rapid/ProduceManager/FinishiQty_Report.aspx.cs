using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class FinishiQty_Report : System.Web.UI.Page
    {
        public DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                drpMonth.SelectedValue = DateTime.Now.Month.ToString().PadLeft(2, '0');
                Report();
            }
        }
        private void Report()
        {
            string year = drpYear.SelectedValue;
            string month = drpMonth.SelectedValue;
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
SELECT 
       YEAR(CreateTime) AS  Year,
       MONTH(CreateTime) AS Month,
       Team,
       Sum(ISNULL(FinishQty,0)) FinishQty
FROM   ProductPlanSubDetail_Record
WHERE  CONVERT(VARCHAR(100), CreateTime, 23) LIKE ");
            sb.Append("'%");
            sb.Append(year);
            if (!string.IsNullOrEmpty(month))
            {
                sb.Append("-");
                sb.Append(month);
            }
            sb.Append("%'");
            sb.Append(@"
       AND Team = '检验'
GROUP  BY Team,YEAR(CreateTime),MONTH(CreateTime)");
            dt = SqlHelper.GetTable(sb.ToString());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Report();
        }

        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Report();
        }

        protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Report();
        }
    }
}