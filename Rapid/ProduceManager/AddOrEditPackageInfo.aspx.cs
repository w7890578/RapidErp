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
    public partial class AddOrEditPackageInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string PackageNumber = ToolManager.GetQueryString("PackageNumber");
                string error = string.Empty;
                string sql = string.Format(" select * from PackageInfo where PackageNumber='{0}' ", PackageNumber);
                if (!ToolManager.CheckQueryString("PackageNumber"))
                {
                    this.btnSubmit.Text = "添加";
                }
                else
                {
                    this.btnSubmit.Text = "修改";
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtPackageNumber.Text = dr["PackageNumber"] == null ? "" : dr["PackageNumber"].ToString();
                        txtPackageName.Text = dr["PackageName"] == null ? "" : dr["PackageName"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        this.btnSubmit.Text = "修改";
                    }
                }

            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string sql = string.Empty;
            string PackageNumber = this.txtPackageNumber.Text.Trim();
            string PackageName = this.txtPackageName.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from PackageInfo  where PackageNumber='{0}'", PackageNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该包编码！请重新填写！";
                    return;
                }
                sql = string.Format(@" insert into PackageInfo (PackageNumber,PackageName,Remark )
         values('{0}','{1}','{2}')", PackageNumber, PackageName, Remark);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    ToolCode.Tool.ResetControl(this.Controls);
                    Tool.WriteLog(Tool.LogType.Operating, "增加包信息" + PackageNumber, "增加成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加包信息" + PackageNumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                sql = string.Format(@" select * from PackageInfo where PackageNumber='{0}'", ToolManager.GetQueryString("PackageNumber"));
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该包信息已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" update PackageInfo set  PackageNumber ='{0}',PackageName='{1}',Remark='{2}'
         where PackageNumber='{3}' ", PackageNumber, PackageName, Remark, ToolManager.GetQueryString("PackageNumber"));
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    ToolCode.Tool.ResetControl(this.Controls);
                    Tool.WriteLog(Tool.LogType.Operating, "编辑包信息" + ToolManager.GetQueryString("PackageNumber"), "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑包信息" + ToolManager.GetQueryString("PackageNumber"), "编辑失败！原因：" + error);
                    return;
                }
            }
        }

    }
}
