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
    public partial class AddOrEditWarehouseAndOtherPerformance : System.Web.UI.Page
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
            if (ToolManager.CheckQueryString("StandardName") || ToolManager.CheckQueryString("PerformanceReviewItem"))
            {
                //sql = string.Format(@" select * from MareriaType where Type='{0}' ", "类别2");
                sql = string.Format(@" select * from PerformanceReviewStandard where StandardName='{0}' and PerformanceReviewItem='{1}' ",
                Server.UrlDecode(ToolManager.GetQueryString("StandardName")), Server.UrlDecode((ToolManager.GetQueryString("PerformanceReviewItem"))));
                PerformanceReviewStandard performangcereviewstandard = PerformanceReviewStandardManager.ConvertDataTableToModel(sql);
                this.txtStandardName.Text = performangcereviewstandard.StandardName;
                this.txtProject.Text = performangcereviewstandard.PerformanceReviewItem;
                this.txtRowNumber.Text = performangcereviewstandard.RowNumber.ToString();
                this.txtDescription.Text = performangcereviewstandard.Description;
                this.txtRemark.Text = performangcereviewstandard.Remark;
                this.txtFullScore.Text = performangcereviewstandard.FullScore.ToString();
                this.drpStatMode.Text = performangcereviewstandard.StatMode.ToString();
                btnSubmit.Text = "修改";
                this.txtStandardName.Enabled = false;
                this.txtProject.Enabled = false;
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            Model.PerformanceReviewStandard performancereviewstandard = new PerformanceReviewStandard();
            performancereviewstandard.StandardName = this.txtStandardName.Text.Trim();
            performancereviewstandard.PerformanceReviewItem = this.txtProject.Text.Trim();
            performancereviewstandard.Description = this.txtDescription.Text.Trim();
            performancereviewstandard.FullScore = Convert.ToInt32(this.txtFullScore.Text);
            performancereviewstandard.RowNumber = Convert.ToInt32(this.txtRowNumber.Text);
            performancereviewstandard.StatMode = Convert.ToInt32(this.drpStatMode.SelectedIndex);
            performancereviewstandard.Remark = this.txtRemark.Text.Trim();
            performancereviewstandard.Type = "仓库及其他";
            bool result = false;
            if (string.IsNullOrEmpty(performancereviewstandard.StandardName) || string.IsNullOrEmpty(performancereviewstandard.PerformanceReviewItem)
                || string.IsNullOrEmpty(performancereviewstandard.Description) || string.IsNullOrEmpty(performancereviewstandard.FullScore.ToString())
                || string.IsNullOrEmpty(performancereviewstandard.StatMode.ToString()) || string.IsNullOrEmpty(performancereviewstandard.RowNumber.ToString()))
            {
                lbSubmit.Text = "考核标准维护信息不完整！";
                return;
            }
            if (btnSubmit.Text.Equals("添加"))
            {
                result = PerformanceReviewStandardManager.AddPerformanceReviewStandardManager(performancereviewstandard, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加考核标准信息" + performancereviewstandard.RowNumber, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加考核标准信息" + performancereviewstandard.RowNumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                result = PerformanceReviewStandardManager.EditPerformanceReviewStandardManager(performancereviewstandard, ref error, Server.UrlDecode(ToolManager.GetQueryString("StandardName")), Server.UrlDecode((ToolManager.GetQueryString("PerformanceReviewItem"))));
                lbSubmit.Text = result == true ? "修改成功！" : "修改失败：原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑考核标准信息" + performancereviewstandard.RowNumber, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑考核标准信息" + performancereviewstandard.RowNumber, "编辑成功！原因" + error);
                    return;
                }
            }

        }
    }
}
