using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditPackageAndProductRelation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("PackageNumber"))
                {
                    Response.Write("包编码不存在！");
                    Response.End();
                    return;
                }
                string PackageNumber = ToolManager.GetQueryString("PackageNumber");
                hdPackageNumber.Value = PackageNumber;

                if (ToolManager.CheckQueryString("PackageNumber") && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
                {
                    string error = string.Empty;
                    string sql = string.Format("select * from PackageAndProductRelation where PackageNumber ='{0}' and ProductNumber='{1}' and Version='{2}'",
                    PackageNumber, ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"));
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    DataRow dr = dt.Rows[0];
                    lbPackageNumber.Text = dr["PackageNumber"] == null ? "" : dr["PackageNumber"].ToString();
                    txtProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                    txtVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                    btnSubmit.Text = "修改";
                }
                else
                {
                    btnSubmit.Text = "添加";

                }

                if (ToolManager.CheckQueryString("m"))
                {
                    string result = string.Empty;
                    string sql = string.Format(@" select top 20  ProductNumber,[Version] ,ProductName from Product ");
                    if (ToolManager.CheckQueryString("contion"))
                    {
                        sql += string.Format(@"  where
ProductNumber like '%{0}%' or ProductNumber like'%{0}' or ProductNumber like '{0}%' or
ProductName like '%{0}%' or ProductName like'%{0}' or ProductName like '{0}%' 
order by ProductNumber asc", ToolManager.GetQueryString("contion"));
                    }
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            result += string.Format(" <tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", dr["ProductName"], dr["ProductNumber"], dr["Version"]);
                        }
                    }
                    Response.Write(result);
                    Response.End();
                    return;
                }
                this.lbPackageNumber.Text = PackageNumber;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ToolManager.CheckQueryString("PackageNumber"))
            {
                Response.Write("包编码不存在");
                Response.End();
            }
            string packagenumber = ToolManager.GetQueryString("PackageNumber");
            string error = string.Empty;
            string sql = string.Empty;
            string productNumber = txtProductNumber.Text.Trim();
            string version = Request.Form["txtVersion"].ToString();

            if (btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format("select COUNT(*) from PackageAndProductRelation where PackageNumber='{0}' and productNumber='{1}' and version='{2}'", packagenumber, productNumber, version);
                if (!SqlHelper.GetTable(sql).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "已存在该包与产品对应关系，请重新填写！";
                    return;
                }  
                sql = string.Format(@" insert into PackageAndProductRelation (PackageNumber,productNumber,version) 
                values('{0}','{1}','{2}')", packagenumber, productNumber, version);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    ToolCode.Tool.ResetControl(this.Controls);
                    Tool.WriteLog(Tool.LogType.Operating, "增加包与产品对应关系" + packagenumber, "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加包与产品对应关系" + packagenumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                sql = string.Format("select COUNT(*) from PackageAndProductRelation where PackageNumber='{0}' and productNumber='{1}' and version='{2}'", packagenumber, productNumber, version);
                if (!SqlHelper.GetTable(sql).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "已存在该包与产品对应关系，请重新填写！";
                    return;
                }
                sql = string.Format(@" select COUNT(*) from Product where ProductNumber='{0}' and Version='{1}' ", productNumber, version);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该产成品编号或版本不存在，请重新填写！";
                    return;
                }
                sql = string.Format(@"
 update PackageAndProductRelation set PackageNumber='{0}',ProductNumber='{1}',version='{2}'
  where PackageNumber='{0}' and productNumber='{3}' and version='{4}'",
   packagenumber, productNumber, version, ToolManager.GetQueryString("productNumber"), ToolManager.GetQueryString("version"));
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑包与产品对应关系" + ToolManager.GetQueryString("productNumber"), "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑包与产品对应关系" + ToolManager.GetQueryString("productNumber"), "编辑失败！原因：" + error);
                    return;
                }
            }
        }
    }
}
