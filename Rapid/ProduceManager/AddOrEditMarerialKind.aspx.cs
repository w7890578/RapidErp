using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model;
using BLL;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditMarerialKind : System.Web.UI.Page
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
            //this.trId.Visible = false;
            string sql = string.Empty;
            string error = string.Empty;
            if (ToolManager.CheckQueryString("Kind"))
            {
                sql = string.Format(@" select * from MarerialKind where kind='{0}' ",
                    Server.UrlDecode(ToolManager.GetQueryString("Kind")));
                MarerialKind marerialkind = MarerialKindManager.ConvertDataTableToModel(sql);
                this.txtKind.Text = marerialkind.Kind;

                btnSubmit.Text = "修改";
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            Model.MarerialKind marerialkind = new MarerialKind();
            marerialkind.Kind = this.txtKind.Text.Trim();

            if (string.IsNullOrEmpty(marerialkind.Kind))
            {
                lbSubmit.Text = "请将带*号的内容填写完整！";
                return;
            }


            else
            {
                if (btnSubmit.Text.Equals("添加"))
                {
                    if (MarerialKindManager.IsExit(marerialkind.Kind))
                    {
                        lbSubmit.Text = "已存在该原材料种类！请重新填写!!!";

                    }
                    if (MarerialKindManager.AddMarerialKind(marerialkind, ref error))
                    {
                        lbSubmit.Text = "添加成功！";
                        return;
                    }
                    else
                    {
                        lbSubmit.Text = "添加失败，原因：" + error;
                        return;
                    }
                }
                else
                {
                    string kind = Server.UrlDecode(ToolManager.GetQueryString("Kind"));
                    if (MarerialKindManager.EditMarerialKind(marerialkind, kind, ref error))
                    {
                        lbSubmit.Text = "修改成功!";
                        return;

                    }
                    else
                    {
                        lbSubmit.Text = "修改失败：原因：" + error;
                        return;
                    }

                }
            }
        }
    }
}