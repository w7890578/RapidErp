using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Model;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class MarerialScrapLog : System.Web.UI.Page
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
            if (ToolManager.CheckQueryString("Id"))
            {
                sql = string.Format(@" select * from MarerialScrapLog where Id='{0}' ", ToolManager.GetQueryString("Id"));
                //sql = string.Format(@" select * from MarerialScrapLog where Id='{0}' ", "1");
                txtCreateTime.ReadOnly = true;
                txtMaterialNumber.ReadOnly = true;
                txtProductNumber.ReadOnly = true;
                Model.MarerialScrapLog marerialscraplog = MarerialScrapLogManager.ConvertDataTableToModel(sql);
                this.txtId.Text = marerialscraplog.Id;
                this.trId.Visible = false;
                this.txtCreateTime.Text = marerialscraplog.CreateTime;
                this.txtProductNumber.Text = marerialscraplog.ProductNumber;
                this.txtMaterialNumber.Text = marerialscraplog.MaterialNumber;
                this.txtScrapDate.Text = marerialscraplog.ScrapDate;
                this.txtTeam.Text = marerialscraplog.Team;
                this.txtCount.Text = marerialscraplog.Count;
                this.txtResponsiblePerson.Text = marerialscraplog.ResponsiblePerson;
                this.txtScrapReason.Text = marerialscraplog.ScrapReason;
                this.txtRemark.Text = marerialscraplog.Remark;
                btnSubmit.Text = "修改";
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {          
            string error = string.Empty;
            Model.MarerialScrapLog marerialscraplog = new Model.MarerialScrapLog();
            marerialscraplog.Id = this.txtId.Text.Trim();
            marerialscraplog.CreateTime = this.txtCreateTime.Text.Trim();
            marerialscraplog.ProductNumber = this.txtProductNumber.Text.Trim();
            marerialscraplog.MaterialNumber = this.txtMaterialNumber.Text.Trim();
            marerialscraplog.ScrapDate = this.txtScrapDate.Text.Trim();
            marerialscraplog.Team = this.txtTeam.Text.Trim();
            marerialscraplog.Count = this.txtCount.Text.Trim();
            marerialscraplog.ResponsiblePerson = this.txtResponsiblePerson.Text.Trim();
            marerialscraplog.ScrapReason = this.txtScrapReason.Text.Trim();
            marerialscraplog.Remark = this.txtRemark.Text.Trim();
            bool result = false;
            if (string.IsNullOrEmpty(marerialscraplog.CreateTime) || string.IsNullOrEmpty(marerialscraplog.MaterialNumber)
                    || string.IsNullOrEmpty(marerialscraplog.ProductNumber) || string.IsNullOrEmpty(marerialscraplog.ScrapDate)
                    || string.IsNullOrEmpty(marerialscraplog.Team) || string.IsNullOrEmpty(marerialscraplog.Count)
                    || string.IsNullOrEmpty(marerialscraplog.ResponsiblePerson) || string.IsNullOrEmpty(marerialscraplog.ScrapReason))
            {
                lbSubmit.Text = "请将带*号的内容填写完整！";
                return;
            }
            if (btnSubmit.Text.Equals("添加"))
            {
                result=MarerialScrapLogManager.AddMarerialScrapLog(marerialscraplog, ref error);
                lbSubmit.Text=result==true?"添加成功！": "添加失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料报废信息" + marerialscraplog.Id, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料报废信息" + marerialscraplog.Id, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                result = MarerialScrapLogManager.EditMarerialScrapLog(marerialscraplog, ref error);
                lbSubmit.Text = result == true ? "修改成功！" : "修改失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料报废信息"+ marerialscraplog.Id, "编辑成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料报废信息" + marerialscraplog.Id, "编辑失败！原因" + error);
                    return;
                }
            }
        }
    }
}
