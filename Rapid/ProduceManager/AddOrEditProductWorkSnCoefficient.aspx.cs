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
    public partial class AddOrEditProductWorkSnCoefficient : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version") && ToolManager.CheckQueryString("WorkSnNumber"))
                {
                    string ProductNumber = ToolManager.GetQueryString("ProductNumber");
                    string Version = ToolManager.GetQueryString("Version");
                    string WorkSnNumber = Server.UrlDecode(ToolManager.GetQueryString("WorkSnNumber"));

                    string RowNumber = ToolManager.GetQueryString("RowNumber");
                    string error = string.Empty;
                    string sql = string.Format(@" select * from ProductWorkSnCoefficient where ProductNumber='{0}' and Version='{1}' and WorkSnNumber='{2}' and RowNumber='{3}'", ProductNumber, Version, WorkSnNumber, RowNumber);
                    if (!ToolManager.CheckQueryString("RowNumber"))
                    {
                        this.btnSubmit.Text = "添加";
                        this.txtRowNumber.Visible = true;
                        this.lbRowNumber.Visible = false;
                    }
                    else
                    {
                        this.btnSubmit.Text = "修改";
                        this.txtRowNumber.Visible = false;
                        this.lbRowNumber.Visible = true;
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            lbProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                            lbVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                            lbWorkSnNumber.Text = dr["WorkSnNumber"] == null ? "" : dr["WorkSnNumber"].ToString();
                            lbRowNumber.Text = dr["RowNumber"] == null ? "" : dr["RowNumber"].ToString();
                            txtMinValue.Text = dr["MinValue"] == null ? "" : dr["MinValue"].ToString();
                            txtMaxValue.Text = dr["MaxValue"] == null ? "" : dr["MaxValue"].ToString();
                            txtCoefficient.Text = dr["Coefficient"] == null ? "" : dr["Coefficient"].ToString();
                            txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                            this.btnSubmit.Text = "修改";
                        }
                    }

                    this.lbProductNumber.Text = ProductNumber;
                    this.lbVersion.Text = Version;
                    this.lbWorkSnNumber.Text = WorkSnNumber;
                }
                else
                {
                    Response.Write("请选择产品工序！");
                    Response.End();
                    return;
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(lbProductNumber.Text) || string.IsNullOrEmpty(lbVersion.Text) || string.IsNullOrEmpty(lbWorkSnNumber.Text))
            {
                Response.Write("请选择产品工序！");
                Response.End();
                return;
            }
            string ProductNumber = this.lbProductNumber.Text.Trim();
            string Version = this.lbVersion.Text.Trim();
            string WorkSnNumber = this.lbWorkSnNumber.Text.Trim();
            string RowNumber = this.txtRowNumber.Text.Trim();
            string MinValue = this.txtMinValue.Text.Trim();
            string MaxValue = this.txtMaxValue.Text.Trim();
            string Coefficient = txtCoefficient.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string sql = string.Empty;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from ProductWorkSnCoefficient where ProductNumber='{0}' and Version='{1}' and WorkSnNumber='{2}' and RowNumber='{3}'", ProductNumber, Version, WorkSnNumber, RowNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "该序号在产品工序系数表中已存在！请重新填写！";
                    return;
                }
                sql = string.Format(@" insert into ProductWorkSnCoefficient (ProductNumber,Version,WorkSnNumber,RowNumber,MinValue,MaxValue,Coefficient,Remark )
 values('{0}','{1}','{2}',{3},{4},{5},'{6}','{7}')", ProductNumber, Version, WorkSnNumber, RowNumber, MinValue, MaxValue, Coefficient, Remark);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加产品工序系数信息" + WorkSnNumber, "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加产品工序系数信息" + WorkSnNumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                sql = string.Format(@" select * from ProductWorkSnCoefficient where ProductNumber='{0}' and Version='{1}' and WorkSnNumber='{2}' and RowNumber='{3}'",
                    ProductNumber, Version, WorkSnNumber, this.lbRowNumber.Text.Trim());
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该产品工序信息列表的工序信息已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" select COUNT(*) from ProductWorkSnProperty where ProductNumber='{0}' and Version='{1}' and WorkSnNumber='{2}'", ProductNumber, Version, WorkSnNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该产成品编号、版本或工序编码不存在，请重新填写！";
                    return;
                }
                sql = string.Format(@"update ProductWorkSnCoefficient set MinValue ={0},MaxValue ={1},Coefficient='{2}',
                 Remark='{3}' where ProductNumber='{4}' and Version='{5}' and WorkSnNumber='{6}' and RowNumber={7} ",
                MinValue, MaxValue, Coefficient, Remark, ProductNumber, Version, WorkSnNumber, ToolManager.GetQueryString("RowNumber"));
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑产品工序系数信息" + WorkSnNumber, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑产品工序系数信息" + WorkSnNumber, "编辑失败！原因：" + error);
                    return;
                }
            }
        }
    }
}