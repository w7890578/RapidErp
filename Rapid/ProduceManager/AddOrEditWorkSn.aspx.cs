using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditWorkSn : System.Web.UI.Page
    {
        public static string worksnname = string.Empty;
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
            if (ToolManager.CheckQueryString("WorkSnNumber"))
            {
                this.txtWorkSnNumber.ReadOnly = true;
                sql = string.Format(@" select * from WorkSn where WorkSnNumber='{0}' ", ToolManager.GetQueryString("WorkSnNumber"));
                WorkSn worksn = WorkSnManager.ConvertDataTableToModel(sql);
                this.txtWorkSnNumber.Text = worksn.WorkSnNumber;
                this.txtWorkSnName.Text = worksn.WorkSnName;
                this.txtSn.Text = worksn.Sn;
                worksnname = this.txtWorkSnName.Text;
                btnSubmit.Text = "修改";
            }
        }

        private bool IsExitSn(string sn)
        {
            string sql = string.Format(" select COUNT(*) from WorkSn where Sn={0} ", sn);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            WorkSn worksn = new WorkSn();
            worksn.WorkSnNumber = this.txtWorkSnNumber.Text.Trim();
            worksn.WorkSnName = this.txtWorkSnName.Text.Trim();
            worksn.Sn = this.txtSn.Text.Trim();
            bool result = false;
            if (string.IsNullOrEmpty(worksn.WorkSnNumber) || string.IsNullOrEmpty(worksn.WorkSnName)
                || string.IsNullOrEmpty(worksn.Sn))
            {
                lbSubmit.Text = "请将带*号的内容填写完整！";
                return;
            }
            else
            {
                if (btnSubmit.Text.Equals("添加"))
                {

                    if (WorkSnManager.IsExitWorkSnName(worksn.WorkSnName))
                    {
                        lbSubmit.Text = "已有该工序，请重新填写！";
                        return;
                    }
                    if (IsExitSn(worksn.Sn))
                    {
                        lbSubmit.Text = "已有该序号，请重新填写！";
                        return;
                    }
                    result = WorkSnManager.AddWorkSn(worksn, ref error);
                    lbSubmit.Text = result == true ? "添加成功！" : "添加失败，原因：" + error;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加基础工序信息" + worksn.WorkSnNumber, "增加成功");
                        ToolCode.Tool.ResetControl(this.Controls);
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加基础工序信息" + worksn.WorkSnNumber, "增加失败！原因：" + error);
                        return;
                    }
                }
                else
                {
                    string sql = string.Format("select * from WorkSn where WorkSnNumber='{0}' and WorkSnName='{1}' and WorkSnName!='{2}'", worksn.WorkSnNumber, worksn.WorkSnName, worksnname);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        lbSubmit.Text = "已有该记录！请重新填写!!!";
                        //ToolCode.Tool.ResetControl(this.Controls);
                        return;
                    }
                    result = WorkSnManager.EditWorkSn(worksn, ref error);
                    lbSubmit.Text = result == true ? "修改成功！" : "修改失败：原因：" + error;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "编辑基础工序信息" + worksn.WorkSnNumber, "编辑成功");
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "编辑基础工序信息" + worksn.WorkSnNumber, "编辑失败！原因：" + error);
                        return;
                    }
                }
            }
        }
    }
}

