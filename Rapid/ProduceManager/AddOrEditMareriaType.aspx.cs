using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditMareriaType : System.Web.UI.Page
    {
        public static string types = string.Empty;
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

            if (ToolManager.CheckQueryString("Type"))
            {

                sql = string.Format(@" select Type FROM MareriaType where  Type='{0}' ", Server.UrlDecode(ToolManager.GetQueryString("Type")));
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    this.txtType.Text = dr["Type"] == null ? "" : dr["Type"].ToString();
                }

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
            string sql = string.Empty;

            Model.MareriaType mareriatype = new MareriaType();
            mareriatype.Type = this.txtType.Text.Trim();

            bool result = false;
            if (string.IsNullOrEmpty(mareriatype.Type))
            {
                lbSubmit.Text = "请将带*号的内容填写完整！";
                return;
            }
            else
            {
                if (btnSubmit.Text.Equals("添加"))
                {
                    sql = string.Format(@" select * from MareriaType where type='{0}'", mareriatype.Type);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        lbSubmit.Text = "已存在该原材料类别！请重新填写！";
                        return;
                    }

                    result = MareriaTypeManager.AddMareriaType(mareriatype, ref error);
                    lbSubmit.Text = result == true ? "添加成功！" : "添加失败，原因：" + error;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加原材料类型" + mareriatype.Type, "增加成功");
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加原材料类型" + mareriatype.Type, "增加失败！原因" + error);
                        return;
                    }
                }
                else
                {
                    string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
                    result = MareriaTypeManager.EditMareriaType(mareriatype, type, ref error);
                    lbSubmit.Text = result == true ? "修改成功!" : "修改失败：原因：" + error;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "编辑原材料类型" + types, "编辑成功");
                        return;

                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "编辑原材料类型" + types, "编辑失败！原因" + error);
                        return;
                    }

                }
            }
        }
    }
}