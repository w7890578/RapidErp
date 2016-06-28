using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditMaterialBlueprintProperty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindDrp("select USER_ID,USER_NAME from PM_USER", this.drpImportPerson, "USER_ID", "USER_NAME");
                if (ToolManager.CheckQueryString("MaterialNumber"))
                {

                    string MaterialNumber = ToolManager.GetQueryString("MaterialNumber");
                    string error = string.Empty;
                    string FileName = Server.UrlDecode(ToolManager.GetQueryString("FileName"));
                    string sql = string.Format(@" select * from MaterialBlueprintProperty where MaterialNumber='{0}' and FileName='{1}' ", MaterialNumber, FileName);
                    if (!ToolManager.CheckQueryString("FileName"))
                    {
                        this.btnSubmit.Text = "添加";
                        this.txtFileName.Visible = true;
                        this.lbFileName.Visible = false;
                        this.lbMaterialNumber.Text = MaterialNumber;
                        this.trModifyTime.Visible = false;
                        this.trImportTime.Visible = false;
                        this.txtImportTime.Text = DateTime.Now.ToString();
                    }
                    else
                    {
                        this.btnSubmit.Text = "修改";
                        trFileUrl.Visible = false;
                        this.txtFileName.Visible = false;
                        this.lbFileName.Visible = true;

                        this.trModifyTime.Visible = false;
                        this.trImportTime.Visible = false;
                        this.drpImportPerson.Visible = false;
                        this.txtModifyTime.Text = DateTime.Now.ToString();
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            this.lbFileName.Text = dr["FileName"] == null ? "" : dr["FileName"].ToString();
                            this.txtModifyTime.Text = dr["ModifyTime"] == null ? "" : dr["ModifyTime"].ToString();
                            this.txtImportTime.Text = dr["ImportTime"] == null ? "" : dr["ImportTime"].ToString();
                            sql = string.Format(@" select USER_NAME from PM_USER where USER_ID='{0}'",
                               dr["ImportPerson"] == null ? "" : dr["ImportPerson"].ToString());
                            string importpersion = SqlHelper.GetScalar(sql);
                            this.lblImportPerson.Text = importpersion;
                            this.txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        }
                    }
                    this.lbMaterialNumber.Text = MaterialNumber;
                }
                else
                {
                    Response.Write("请选择原材料！");
                    Response.End();
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(lbMaterialNumber.Text))
            {
                Response.Write("请选择原材料！");
                Response.End();
            }
            string MaterialNumber = this.lbMaterialNumber.Text.Trim();
            string FileName = this.txtFileName.Text.Trim();
            string ImportTime = this.txtImportTime.Text.Trim();
            string ModifyTime = this.txtModifyTime.Text.Trim();
            string ImportPerson = this.drpImportPerson.SelectedValue.Trim();
            string Remark = txtRemark.Text.Trim();
            string importpersion = this.lblImportPerson.Text.Trim();
            string sql = string.Empty;
            if (ImportPerson == "")
            {
                lbSubmit.Text = "请选择导入人员！！！";
                return;

            }
            if (this.btnSubmit.Text.Equals("添加"))
            {
                string guid = Guid.NewGuid().ToString();
                string strFileName = this.fuFileUrl.FileName;
                string FileUrl = Server.MapPath("/UpLoadFileData/" + guid + strFileName.Substring(strFileName.LastIndexOf('.'), strFileName.Length - strFileName.LastIndexOf('.')));
                this.fuFileUrl.SaveAs(FileUrl);
                string urlData = "../UpLoadFileData/" + guid + strFileName.Substring(strFileName.LastIndexOf('.'), strFileName.Length - strFileName.LastIndexOf('.'));
                string addImportTime = DateTime.Now.ToString();
                sql = string.Format(@" select * from MaterialBlueprintProperty where MaterialNumber='{0}' and FileName='{1}' ", MaterialNumber, FileName);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该原材料的图纸属性！请重新填写文件名称！";

                    return;
                }
                sql = string.Format(@" insert into MaterialBlueprintProperty (MaterialNumber,FileName,ImportTime,ImportPerson,FileUrl,Remark )
 values('{0}','{1}','{2}','{3}','{4}','{5}')", MaterialNumber, FileName, addImportTime, ImportPerson, urlData, Remark);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料图纸信息" + MaterialNumber, "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料图纸信息" + MaterialNumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                string editModifyTime = DateTime.Now.ToString();
                sql = string.Format(@" select * from MaterialBlueprintProperty where MaterialNumber='{0}' and FileName='{1}' ", MaterialNumber, this.lbFileName.Text.Trim());
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该原材料的图纸属性已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" select COUNT(*) from MarerialInfoTable where MaterialNumber='{0}' ", MaterialNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该原材料编号不存在，请重新填写！";
                    return;
                }
                sql = string.Format(@"update MaterialBlueprintProperty set ModifyTime ='{1}',
                ImportPerson='{2}',Remark='{3}', ImportTime='{4}' where MaterialNumber='{5}' and FileName='{0}' ",
                this.lbFileName.Text.Trim(), editModifyTime, importpersion, Remark, ImportTime, MaterialNumber);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料图纸信息" + MaterialNumber, "编辑成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料图纸信息" + MaterialNumber, "编辑失败！原因：" + error);
                    return;
                }

            }
        }

    }
}
