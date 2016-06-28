
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditPerformanceReviewLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ControlBindManager.BindDrp("select distinct USER_NAME,USER_ID from PM_USER", drpName, "USER_ID", "USER_NAME");
                //ControlBindManager.BindDrp("select distinct PerformanceReviewItem from PerformanceReviewStandard", drpProject, "PerformanceReviewItem", "PerformanceReviewItem");
                LoadPage();
                //drpMonth.SelectedValue = DateTime.Now.Month.ToString();
                //drpYear.SelectedValue = DateTime.Now.Year.ToString();
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
                ToolManager.GetQueryString("Year"), ToolManager.GetQueryString("Month"), Server.UrlDecode(ToolManager.GetQueryString("PerformanceReviewItem")), ToolManager.GetQueryString("Name"));
                PerformanceReviewLog performancereviewlog = PerformanceReviewLogManager.ConvertDataTableToModel(sql);
                lbYear.Text = performancereviewlog.Year;
                lbMonth.Text = performancereviewlog.Month;
                lbProject.Text = performancereviewlog.PerformanceReviewItem;
                this.txtRowNumber.Text = performancereviewlog.RowNumber.ToString();
                this.txtFullScore.Text = performancereviewlog.FullScore.ToString();
                this.txtScore.Text = performancereviewlog.Score.ToString();
                this.txtRemark.Text = performancereviewlog.Remark;
                this.txtDeduction.Text = performancereviewlog.Deduction.ToString();
                this.txtDescription.Text = performancereviewlog.Description;
                lbName.Text = performancereviewlog.Name;
                this.drpStatMode.SelectedValue = performancereviewlog.StatMode.ToString();
                btnSubmit.Text = "修改";

                txtRowNumber.ReadOnly = true;
                txtDescription.ReadOnly = true;
                drpStatMode.Enabled = false;
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            Model.PerformanceReviewLog performancereviewlog = new PerformanceReviewLog();
            performancereviewlog.Year = lbYear.Text;
            performancereviewlog.Month = lbMonth.Text;
            performancereviewlog.PerformanceReviewItem = lbProject.Text;
            performancereviewlog.RowNumber = Convert.ToInt32(this.txtRowNumber.Text);
            performancereviewlog.FullScore = Convert.ToInt32(this.txtFullScore.Text);
            performancereviewlog.Deduction = Convert.ToInt32(this.txtDeduction.Text);
            performancereviewlog.Score = Convert.ToInt32(this.txtScore.Text);
            performancereviewlog.Description = txtDescription.Text;
            performancereviewlog.Remark = this.txtRemark.Text.Trim();
            performancereviewlog.Name = lbName.Text;
            performancereviewlog.StatMode = Convert.ToInt32(performancereviewlog.StatMode);

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



