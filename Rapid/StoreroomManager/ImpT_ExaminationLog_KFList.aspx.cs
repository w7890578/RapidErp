using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class ImpT_ExaminationLog_KFList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                drpMonth.SelectedValue = DateTime.Now.Month.ToString();
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string year = drpYear.SelectedValue;
            string month = drpMonth.SelectedValue;
            string error = string.Empty;
            if (StoreroomToolManager.ImpT_ExaminationLog_KFList(year, month, FU_Excel, Server, ref  error))
            {
                lbMsg.Text = "导入成功";
            }
            else
            {
                lbMsg.Text = "导入失败！<br/>" + error;
            }
        }
    }
}
