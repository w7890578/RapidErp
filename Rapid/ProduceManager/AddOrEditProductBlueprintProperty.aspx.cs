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
    public partial class AddOrEditProductBlueprintProperty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                trFileName.Visible = false;
                trCCFileName.Visible = false;
                //ControlBindManager.BindDrp("select USER_ID,USER_NAME from PM_USER", this.drpImportPerson, "USER_ID", "USER_NAME");
                //ControlBindManager.BindDrp("select USER_ID,USER_NAME from PM_USER", this.drpProvidePersion, "USER_ID", "USER_NAME");
                //ControlBindManager.BindDrp("select USER_ID,USER_NAME from PM_USER", this.drpReceivePersion, "USER_ID", "USER_NAME");
                if (ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
                {

                    string ProductNumber = ToolManager.GetQueryString("ProductNumber");
                    string Version = ToolManager.GetQueryString("Version");
                    string FileName = Server.UrlDecode(ToolManager.GetQueryString("FileName"));

                    string error = string.Empty;
                    string sql = string.Format(" select * from ProductBlueprintProperty where ProductNumber='{0}' and Version='{1}' and FileName='{2}'", ProductNumber, Version, FileName);
                    if (!ToolManager.CheckQueryString("FileName"))
                    {
                        this.btnSubmit.Text = "添加";
                        this.trModifyTime.Visible = false;
                        this.trImportTime.Visible = false;
                        this.txtFileName.Visible = true;
                        this.txtCCFileName.Visible = true;
                        this.lblCCFileName.Visible = false;
                        this.lbFileName.Visible = false;
                        this.txtImportTime.Text = DateTime.Now.ToString();
                        trImportUser.Visible = false;
                        trClaimModificationPerson.Visible = false;
                    }
                    else
                    {
                        trClaimModificationPerson.Visible = true;
                        trImportUser.Visible = true;
                        this.btnSubmit.Text = "修改";
                        this.trModifyTime.Visible = false;
                        this.trImportTime.Visible = false;
                        this.txtFileName.Visible = false;
                        this.txtCCFileName.Visible = false;
                        this.lblCCFileName.Visible = true;
                        this.lbFileName.Visible = true;
                        this.trFileUrl.Visible = false;
                        this.trCCFileUrl.Visible = true;

                        // drpImportPerson.Visible = false;
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            this.lbFileName.Text = dr["FileName"] == null ? "" : dr["FileName"].ToString();
                            this.lblCCFileName.Text = dr["CCFileName"] == null ? "" : dr["CCFileName"].ToString();
                            this.drpType.SelectedValue = dr["Type"] == null ? "" : dr["Type"].ToString();
                            this.txtModifyTime.Text = dr["ModifyTime"] == null ? "" : dr["ModifyTime"].ToString();
                            this.txtImportTime.Text = dr["ImportTime"] == null ? "" : dr["ImportTime"].ToString();
                            this.txtProvidePersion.Text = dr["ProvidePersion"] == null ? "" : dr["ProvidePersion"].ToString();
                            this.txtReceivePersion.Text = dr["ReceivePersion"] == null ? "" : dr["ReceivePersion"].ToString();
                            this.txtClaimModificationPerson.Text = dr["ClaimModificationPerson"] == null ? "" : dr["ClaimModificationPerson"].ToString();
                            sql = string.Format(@" select USER_NAME from PM_USER where USER_ID='{0}'",
                                dr["ImportPerson"] == null ? "" : dr["ImportPerson"].ToString());
                            string importpersion = SqlHelper.GetScalar(sql);
                            this.lblImportPerson.Text = importpersion;
                            this.txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                            this.btnSubmit.Text = "修改";
                            //this.drpProvidePersion.SelectedValue = dr["ProvidePersion"] == null ? "" : dr["ProvidePersion"].ToString();
                            //this.drpReceivePersion.SelectedValue = dr["ReceivePersion"] == null ? "" : dr["ReceivePersion"].ToString();
                            this.txtProvideDate.Text = dr["ProvideDate"] == null ? "" : dr["ProvideDate"].ToString();
                            //trCCFileUrl.Attributes["value"] = Server.MapPath("/UpLoadFileData/" + dr["CCFileUrl"]);
                        }
                    }
                    this.lbProductNumber.Text = ProductNumber;
                    this.lbVersion.Text = Version;
                }
                else
                {
                    Response.Write("请选择产成品编号、版本！");
                    Response.End();
                    return;
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(lbProductNumber.Text) || string.IsNullOrEmpty(lbVersion.Text))
            {
                Response.Write("请选择产成品、版本！");
                Response.End();
            }
            string ProductNumber = this.lbProductNumber.Text.Trim();
            string Version = this.lbVersion.Text.Trim();

            string Type = this.drpType.SelectedValue;
            string ModifyTime = this.txtModifyTime.Text.Trim();
            string ImportTime = this.txtImportTime.Text.Trim();
            string ImportPerson = Tool.GetUser().UserNumber;
            //this.drpImportPerson.SelectedValue.Trim();
            string Remark = txtRemark.Text.Trim();
            string ProvideDate = this.txtProvideDate.Text.Trim();
            string ProvidePersion = txtProvidePersion.Text.Trim();
            string ReceivePersion = txtReceivePersion.Text.Trim();

            string sql = string.Empty;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                string guid = Guid.NewGuid().ToString();
                string strFileName = this.fuFileUrl.FileName;

                if (string.IsNullOrEmpty(strFileName))
                {
                    lbSubmit.Text = "请上传图纸！";
                    return;
                }
                string FileUrl = Server.MapPath("/UpLoadFileData/" + strFileName);
                this.fuFileUrl.SaveAs(FileUrl);
                string urlData = "../UpLoadFileData/" + strFileName;
                if (IsExitDrawing(urlData))
                {
                    lbSubmit.Text = "已上传过该张图纸，请重新上传！";
                    return;
                }
                //根据上传文件获取文件名称
                int last = strFileName.LastIndexOf('.');
                string FileName = strFileName.Substring(0, last);
                this.txtFileName.Text = FileName;

                sql = string.Format(@" select * from ProductBlueprintProperty  where ProductNumber='{0}' and Version='{1}' and FileName='{2}'", ProductNumber, Version, FileName);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该产成品的图纸属性！请重新填写文件名称！";
                    return;
                }
                string CZfilename = this.fuCCFileUrl.FileName;
                string czurldata = string.Empty;
                string czfilename = string.Empty;
                if (!string.IsNullOrEmpty(CZfilename))
                {
                    string czfileurl = Server.MapPath("/UpLoadFileData/" + CZfilename);
                    this.fuCCFileUrl.SaveAs(czfileurl);
                    czurldata = "../UpLoadFileData/" + CZfilename;
                    //根据上传文件获取文件名称
                    int czlast = strFileName.LastIndexOf('.');
                    czfilename = CZfilename.Substring(0, czlast);
                    this.txtCCFileName.Text = czfilename;
                }



                //if (IsExitCCFileUrl(czurldata))
                //{
                //    lbSubmit.Text = "已上传过该操作指导书，请重新上传！";
                //    return;
                //}



                //sql = string.Format(@" select * from ProductBlueprintProperty  where ProductNumber='{0}' and Version='{1}' and CCFileName='{2}'", ProductNumber, Version, czfilename);
                //if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                //{
                //    lbSubmit.Text = "已存在过该操作指导书！请重新填写文件名称！";
                //    return;
                //}
                sql = string.Format(@" insert into ProductBlueprintProperty (ProductNumber,Version,FileName,Type,ModifyTime,ImportTime,
                ImportPerson,FileUrl,Remark,ProvideDate,ProvidePersion,ReceivePersion,CCFileName,CCFileUrl)
                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                ProductNumber, Version, FileName, Type, ModifyTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ImportPerson, urlData, Remark, ProvideDate, ProvidePersion, ReceivePersion, czfilename, czurldata);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加产品图纸信息" + ProductNumber, "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加产品图纸信息" + ProductNumber, "增加失败！原因：" + error);
                    return;
                }

            }
            else
            {

                string CZfilename = this.fuCCFileUrl.FileName;
                string czurldata = string.Empty;
                string czfilename = string.Empty;
                if (!string.IsNullOrEmpty(CZfilename))
                {
                    string czfileurl = Server.MapPath("/UpLoadFileData/" + CZfilename);
                    this.fuCCFileUrl.SaveAs(czfileurl);
                    czurldata = "../UpLoadFileData/" + CZfilename;
                    //根据上传文件获取文件名称
                    int czlast = CZfilename.LastIndexOf('.');
                    czfilename = CZfilename.Substring(0, czlast);
                    this.txtCCFileName.Text = czfilename;
                }
                string strFileName = this.fuFileUrl.FileName; 

                string claimModificationPerson = this.txtClaimModificationPerson.Text.Trim();
                string modifytime = DateTime.Now.ToString();
                sql = string.Format(@" select * from ProductBlueprintProperty  
where ProductNumber='{0}' and Version='{1}' and FileName='{2}'", ProductNumber, Version, this.lbFileName.Text.Trim());
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该产品的图纸属性已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" select COUNT(*) from Product where ProductNumber='{0}' and Version='{1}' ", ProductNumber, Version);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该产成品编号或版本不存在，请重新填写！";
                    return;
                }
                List<string> sqls = new List<string>();
                if (!string.IsNullOrEmpty(czurldata))
                {
                    sql = string.Format(@" update ProductBlueprintProperty set  ImportTime ='{1}',
                ImportPerson='{2}',Remark='{3}',Type='{4}',ProvideDate='{5}',ProvidePersion='{6}',
ReceivePersion='{7}', 
CCFileName='{10}',CCFileUrl='{11}'
where ProductNumber='{8}' and Version='{9}' and FileName='{0}'",
                   this.lbFileName.Text.Trim(), ImportTime, ImportPerson, Remark, Type, ProvideDate, ProvidePersion, ReceivePersion, ProductNumber, Version
                   , czfilename, czurldata);
                    sqls.Add(sql);
                }
                else
                {
                    sql = string.Format(@" update ProductBlueprintProperty set  ImportTime ='{1}',
                ImportPerson='{2}',Remark='{3}',Type='{4}',ProvideDate='{5}',ProvidePersion='{6}',
ReceivePersion='{7}' 
where ProductNumber='{8}' and Version='{9}' and FileName='{0}'",
               this.lbFileName.Text.Trim(), ImportTime, ImportPerson, Remark, Type, ProvideDate, ProvidePersion, ReceivePersion, ProductNumber, Version
                        );
                    sqls.Add(sql);
                }


                if (!string.IsNullOrEmpty(claimModificationPerson)) //写了要求修改人的
                {
                    sql = string.Format(@" insert into ProductBlueprintProperty (ProductNumber,Version,FileName,ModifyTime,ClaimModificationPerson,Type)
                    values('{0}','{1}','{2}','{3}','{4}','{5}')", ProductNumber, Version, this.lbFileName.Text.Trim(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), claimModificationPerson, Type);
                    sqls.Add(sql);
                }

                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑产品图纸信息" + ProductNumber, "编辑成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑产品图纸信息" + ProductNumber, "编辑失败！原因：" + error);
                    return;
                }
            }
        }

        /// <summary>
        /// 检测是否存在该图纸
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool IsExitDrawing(string url)
        {
            string sql = string.Format("select count(*) from ProductBlueprintProperty where FileUrl='{0}'", url);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }
        /// <summary>
        /// 检测是否存在该指导书
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool IsExitCCFileUrl(string ccfileurl)
        {
            string sql = string.Format("select count(*) from ProductBlueprintProperty where CCFileUrl='{0}'", ccfileurl);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }
    }
}
