using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class EditWarehousePerformanceReviewLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }

        private void LoadPage()
        {
            string sql = string.Empty;
            string error = string.Empty;
            //this.trId.Visible = false;
            if (ToolManager.CheckQueryString("Year") || ToolManager.CheckQueryString("Month") || ToolManager.CheckQueryString("PerformanceReviewItem") || ToolManager.CheckQueryString("Name"))
            {
                sql = string.Format(@" select * from PerformanceReviewLog where Year='{0}' and Month='{1}' and PerformanceReviewItem='{2}' and Name='{3}'",
                ToolManager.GetQueryString("Year"), ToolManager.GetQueryString("Month"), Server.UrlDecode(ToolManager.GetQueryString("PerformanceReviewItem")),Server.UrlDecode(ToolManager.GetQueryString("Name")));
                PerformanceReviewLog performancereviewlog = PerformanceReviewLogManager.ConvertDataTableToModel(sql);
                lbYear.Text = performancereviewlog.Year;
                lbMonth.Text = performancereviewlog.Month;
                lbProject.Text = performancereviewlog.PerformanceReviewItem;
                this.lblRowNumber.Text = performancereviewlog.RowNumber.ToString();
                this.lblFullScore.Text = performancereviewlog.FullScore.ToString();
                this.txtScore.Text = performancereviewlog.Score.ToString();
                this.txtRemark.Text = performancereviewlog.Remark;
                lbName.Text = performancereviewlog.Name;
                btnSubmit.Text = "修改";
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            Model.PerformanceReviewLog performancereviewlog = new PerformanceReviewLog();
            performancereviewlog.Year = lbYear.Text;
            performancereviewlog.Month = lbMonth.Text;
            performancereviewlog.PerformanceReviewItem = lbProject.Text;
            performancereviewlog.RowNumber = Convert.ToInt32(this.lblRowNumber.Text);
            performancereviewlog.FullScore = Convert.ToInt32(this.lblFullScore.Text);
            //performancereviewlog.Deduction = Convert.ToInt32(this.txtDeduction.Text);
            performancereviewlog.Score = Convert.ToInt32(this.txtScore.Text);
            performancereviewlog.Remark = this.txtRemark.Text.Trim();
            performancereviewlog.Name = lbName.Text;
            if (performancereviewlog.Score > performancereviewlog.FullScore)
            {
                lbSubmit.Text = "得分必须小于或行于满分！";
                return;
            }
            performancereviewlog.Deduction = performancereviewlog.FullScore - performancereviewlog.Score;
            bool result = PerformanceReviewLogManager.EditPerformanceReviewLogManager(performancereviewlog, ref error, ToolManager.GetQueryString("Year"), ToolManager.GetQueryString("Month"), Server.UrlDecode(ToolManager.GetQueryString("PerformanceReviewItem")), ToolManager.GetQueryString("Name"));
            lbSubmit.Text = result == true ? "修改成功！" : "修改失败：原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑员工绩效信息" + performancereviewlog.RowNumber, "编辑成功");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑员工绩效信息" + performancereviewlog.RowNumber, "编辑失败！原因" + error);
                return;
            } 
        }
    }
}
