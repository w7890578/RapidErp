using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class AddOrEditMakeCollectionsMode : System.Web.UI.Page
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
                sql = string.Format(@" select * from MakeCollectionsMode where Id='{0}' ", ToolManager.GetQueryString("Id"));
                this.trNumber.Visible = false;
                //sql = string.Format(@" select * from MakeCollectionsMode where Id='{0}' ", "7");
                MakeCollectionsModes makecollectionsmode = MakeCollectionsModeManager.ConvertDataTableToModel(sql);
                this.txtNumber.Text = makecollectionsmode.Id;
                this.txtMakeCollectionsMode.Text = makecollectionsmode.MakeCollectionsMode;
                btnSubmit.Text = "修改";
            }
            else
            {
                btnSubmit.Text = "添加";

            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            Model.MakeCollectionsModes makecollectionsmode = new Model.MakeCollectionsModes();
            makecollectionsmode.Id = this.txtNumber.Text.Trim();
            makecollectionsmode.MakeCollectionsMode = this.txtMakeCollectionsMode.Text.Trim();
            if (string.IsNullOrEmpty(makecollectionsmode.Id) || string.IsNullOrEmpty(makecollectionsmode.MakeCollectionsMode))
            {
                lbSubmit.Text = "请将带*号的内容填写完整！";
                return;
            }
            if (MakeCollectionsModeManager.CheckHave(makecollectionsmode.MakeCollectionsMode))
            {
                lbSubmit.Text = "已经有该收款方式，请重新填写！";
                return;
            }
            else
            {
                bool result = false;
                if (btnSubmit.Text.Equals("添加"))
                {
                    result = MakeCollectionsModeManager.AddMakeCollectionsMode(makecollectionsmode, ref error);
                    lbSubmit.Text = result == true ? "添加成功！" : "添加失败，原因：" + error;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加付款方式", "增加成功");
                        ToolCode.Tool.ResetControl(this.Controls);
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加付款方式", "增加失败！原因" + error);
                        return;
                    }
                }

                else
                {
                    result = MakeCollectionsModeManager.EditMakeCollectionsMode(makecollectionsmode, ref error);
                    lbSubmit.Text = result == true ? "修改成功!" : "修改失败：原因：" + error;
                    if (MakeCollectionsModeManager.EditMakeCollectionsMode(makecollectionsmode, ref error))
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "编辑付款方式", "编辑失败");
                        lbSubmit.Text = "修改成功!";

                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "编辑付款方式", "编辑失败！原因" + error);
                        lbSubmit.Text = "修改失败!";
                        return;
                    }

                }
            }
        }
    }
}