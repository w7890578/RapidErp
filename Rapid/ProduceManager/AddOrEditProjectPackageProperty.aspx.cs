using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditProjectPackageProperty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("ProjectNumber"))
                {
                    string projectnumber = ToolManager.GetQueryString("ProjectNumber");
                    string sql = string.Format(@"delete ProjectPackageProperty where ProjectNumber='{0}' ", projectnumber);
                    string error = string.Empty;
                    bool restult = SqlHelper.ExecuteSql(sql, ref error);
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除项目信息" + projectnumber, "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除项目信息" + projectnumber, "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string projectnumber = txtProjectNumber.Text.Trim();
            string packagenumber = txtPackageNumber.Text.Trim();
            string sql = string.Empty;
            string error = string.Empty;

            if (IsExitNumber(projectnumber))
            {
                lbSubmit.Text = "已存在该项目编号，请重新填写！";
                return;
            }
            if (IsExitName(packagenumber))
            {
                lbSubmit.Text = "已存在该包编号，请重新填写！";
                return;
            }

            sql = string.Format("insert into ProjectPackageProperty(ProjectNumber,packagenumber) values ('{0}','{1}'", projectnumber, packagenumber);
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加项目信息" + projectnumber, "增加成功！");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加项目信息" + projectnumber, "增加失败！原因" + error);
                return;
            }


        }
        /// <summary>
        /// 检查是否存在该项目编号
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool IsExitNumber(string number)
        {
            string sql = string.Format(" select COUNT(*) from ProjectPackageProperty where ProjectNumber='{0}' ", number);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }
        /// <summary>
        /// 检查是否存在该包编号
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private bool IsExitName(string number)
        {
            string sql = string.Format(" select COUNT(*) from ProjectPackageProperty where ProjectName='{0}' ", number);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }
    }
}
