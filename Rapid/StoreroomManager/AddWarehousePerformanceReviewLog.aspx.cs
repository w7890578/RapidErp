using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class AddWarehousePerformanceReviewLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpMonth.SelectedValue = DateTime.Now.Month.ToString();
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string year = drpYear.SelectedValue;
            string month = drpMonth.SelectedValue;
            if (StoreroomToolManager.AddWarehousePerformanceReviewLog(year, month, ref error))
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                lbMsg.Text = "上报失败！原因：" + error;
            }

        }
    }
}
