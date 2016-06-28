using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Model;
using Rapid.ToolCode;
using DAL;


namespace Rapid.ProduceManager
{
    public partial class AddOrEditExaminationLog : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindDrp("select distinct USER_NAME,USER_ID from PM_USER", drpName, "USER_ID", "USER_NAME");
                LoadPage();
            }
        }
        private void LoadPage()
        {

            string sql = string.Empty;
            string error = string.Empty;
            if (ToolManager.CheckQueryString("Name") && ToolManager.CheckQueryString("Year") && ToolManager.CheckQueryString("Month"))
            {
                string name = Server.UrlDecode(ToolManager.GetQueryString("Name"));
                sql = string.Format(@" select  e.Year,e.Month,pu.USER_NAME as Name,e.Score,e.LeaderScore,
                e.Operation,e.WorkAttendance,e.WorkState,e.Teamwork,
                e.RejectsProduct,e.Security, e.TotalScore,e.Remark
                from ExaminationLog e left join PM_USER pu on e.Name=pu.USER_ID where year='{0}' and month='{1}'
                and pu.USER_NAME='{2}' ", ToolManager.GetQueryString("Year"), ToolManager.GetQueryString("Month"),
                                       name);

                ExaminationLog examinationlog = ExaminationLogManager.ConvertDataTableToModel(sql);
                this.lblYear.Text = examinationlog.Year;
                this.lblMonth.Text = examinationlog.Month;
                this.lblName.Text = examinationlog.Name;
                this.drpName.SelectedValue = examinationlog.Name; 
                this.txtScore.Text = examinationlog.Score.ToString();
                this.txtLeaderScore.Text = examinationlog.LeaderScore.ToString();
                this.txtTotalScore.Text = examinationlog.TotalScore.ToString();
                this.txtOperation.Text = examinationlog.Operation.ToString();
                this.lbWorkAttendance.Text = examinationlog.WorkAttendance.ToString();
                this.txtWorkState.Text = examinationlog.WorkState.ToString();
                this.txtTeamwork.Text = examinationlog.Teamwork.ToString();
                this.txtRejectsProduct.Text = examinationlog.RejectsProduct.ToString();
                this.txtSecurity.Text = examinationlog.Security.ToString();
                this.txtRemark.Text = examinationlog.Remark;
                btnSubmit.Text = "修改";
                this.txtWorkAttendance.Visible = false;
                drpYear.Visible = false;
                drpMonth.Visible = false;
                drpName.Visible = false;
                lblMonth.Visible = false;
                lblMonth.Visible = true;
                lblYear.Visible = true;
                lblName.Visible = true;

            }
            else
            {
                btnSubmit.Text = "添加";

                lblMonth.Visible = false;
                lblYear.Visible = false;
                lblName.Visible = false;

            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;

            Model.ExaminationLog examinationlog = new ExaminationLog();
            //examinationlog.Year = this.drpYear.SelectedValue;
            //examinationlog.Month = this.drpMonth.SelectedValue;
            //examinationlog.Name = this.drpName.SelectedValue;
            //examinationlog.WorkAttendance = Convert.ToDecimal(this.txtWorkAttendance.Text);
            examinationlog.Score = Convert.ToDecimal(this.txtScore.Text);
            examinationlog.LeaderScore = Convert.ToDecimal(this.txtLeaderScore.Text);
            examinationlog.TotalScore = Convert.ToDecimal(this.txtTotalScore.Text);
            examinationlog.TotalScore = Convert.ToDecimal(Request.Form["txtTotalScore"].ToString());
            examinationlog.Operation = Convert.ToDecimal(this.txtOperation.Text);
           
            examinationlog.WorkState = Convert.ToDecimal(this.txtWorkState.Text);
            examinationlog.Teamwork = Convert.ToDecimal(this.txtTeamwork.Text);
            examinationlog.RejectsProduct = Convert.ToDecimal(this.txtRejectsProduct.Text);
            examinationlog.Security = Convert.ToDecimal(this.txtSecurity.Text);
            examinationlog.Remark = this.txtRemark.Text;
            bool result = false;
            if (btnSubmit.Text.Equals("添加"))
            {
                if (string.IsNullOrEmpty(this.drpYear.Text)
              || string.IsNullOrEmpty(this.drpMonth.Text) || string.IsNullOrEmpty(this.drpName.Text))
                {
                    lbSubmit.Text = "请将带*号的内容填写完整！";
                    return;
                }
                string sql = string.Format(@" select * from ExaminationLog where YEAR='{0}' and MONTH='{1}' and Name='{2}'",
                     examinationlog.Year, examinationlog.Month, examinationlog.Name);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    lbSubmit.Text = "当前年份、月份，已有该用户信息，请重新填写！";
                    return;
                }
                result = ExaminationLogManager.AddExaminationLog(examinationlog, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加员工考试成绩信息" + examinationlog.Id, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加员工考试成绩信息" + examinationlog.Id, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                examinationlog.Year = this.lblYear.Text.Trim();
                examinationlog.Month = this.lblMonth.Text.Trim();
                examinationlog.Name = this.lblName.Text.Trim();
                examinationlog.WorkAttendance = Convert.ToDecimal(this.lbWorkAttendance.Text.Trim());
                result = ExaminationLogManager.EditExaminationLog(examinationlog, ref error);
                lbSubmit.Text = result == true ? "修改成功!" : "修改失败：原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑员工考试成绩信息维护" + examinationlog.Id, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑员工考试成绩信息维护" + examinationlog.Id, "编辑失败！原因：" + error);
                    return;
                }
            }
        }


    }
}
