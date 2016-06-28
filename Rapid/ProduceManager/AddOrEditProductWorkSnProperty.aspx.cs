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
    public partial class AddOrEditProductWorkSnProperty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindDrp(" select WorkSnNumber,WorkSnName from WorkSn ", drpWorkSnNumber, "WorkSnNumber", "WorkSnName");
                if (ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
                {
                    string ProductNumber = ToolManager.GetQueryString("ProductNumber");
                    string Version = ToolManager.GetQueryString("Version");
                    string tempSql = string.Format(@"select CustomerProductNumber from ProductCustomerProperty where ProductNumber ='{0}' and Version ='{1}'", ProductNumber, Version);
                    string temp = string.Empty;
                    foreach (DataRow drT in SqlHelper.GetTable(tempSql).Rows)
                    {
                        temp += drT["CustomerProductNumber"] == null ? "" : drT["CustomerProductNumber"].ToString() + ",";

                    }
                    lbCustomerProductNumber.Text = temp.TrimEnd(',');
                    string WorkSnNumber = Server.UrlDecode(ToolManager.GetQueryString("WorkSnNumber"));
                    string error = string.Empty;
                    string sql = string.Format(@" select * from ProductWorkSnProperty where ProductNumber='{0}' and Version='{1}' and WorkSnNumber='{2}'", ProductNumber, Version, WorkSnNumber);
                    if (!ToolManager.CheckQueryString("WorkSnNumber"))
                    {
                        this.btnSubmit.Text = "添加";
                        this.lbWorkSnNumber.Visible = false;

                    }
                    else
                    {
                        this.btnSubmit.Text = "修改";
                        this.drpWorkSnNumber.Visible = false;
                        this.lbWorkSnNumber.Visible = true;
                        sql = string.Format(@" select * from ProductWorkSnProperty where 
                        ProductNumber='{0}' and Version='{1}' and WorkSnNumber='{2}'", ProductNumber, Version, WorkSnNumber);
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            lbProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                            lbVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();

                            lbWorkSnNumber.Text = WorkSnNumber;
                            txtRowNumber.Text = dr["RowNumber"] == null ? "" : dr["RowNumber"].ToString();
                            txtRatedManhour.Text = dr["RatedManhour"] == null ? "" : dr["RatedManhour"].ToString();
                            txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                            this.btnSubmit.Text = "修改";
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
                Response.Write("请选择产成品编号、版本！！");
                Response.End();
            }
            string ProductNumber = this.lbProductNumber.Text.Trim();
            string Version = this.lbVersion.Text.Trim();
            string WorkSnNumber = this.drpWorkSnNumber.SelectedValue;
            string RowNumber = this.txtRowNumber.Text.Trim();
            string RatedManhour = this.txtRatedManhour.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string sql = string.Empty;
            List<string> sqls = new List<string>();
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from ProductWorkSnProperty where ProductNumber='{0}' and Version='{1}' and WorkSnNumber='{2}'", ProductNumber, Version, WorkSnNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该产品工序的工序编码！请重新填写！";
                    return;
                }
                sql = string.Format(@" insert into ProductWorkSnProperty (ProductNumber,Version,WorkSnNumber,RowNumber,RatedManhour,Remark )
 values('{0}','{1}','{2}','{3}','{4}','{5}')", ProductNumber, Version, WorkSnNumber, RowNumber, RatedManhour, Remark);
                sqls.Add(sql);
                sql = string.Format(@"update product set ratedManhour=(
select sum(ratedManhour) from ProductWorkSnProperty where  productNumber='{0}' and version ='{1}')
where productNumber='{0}' and version='{1}'", ProductNumber, Version);
                sqls.Add(sql);
                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加产品工序信息" + ProductNumber, "增加成功");
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加产品工序信息" + ProductNumber, "增加失败！原因：" + error);
                }
                new BLL.ToolChangeProduct().changeproduct(ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"));
                return;
            }
            else
            {
                sql = string.Format(@" select * from ProductWorkSnProperty where ProductNumber='{0}' and Version='{1}' and WorkSnNumber='{2}'",
                    ProductNumber, Version, lbWorkSnNumber.Text.Trim());
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该产成品的工序信息已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" select COUNT(*) from Product where ProductNumber='{0}' and Version='{1}' ", ProductNumber, Version);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该产成品编号或版本不存在，请重新填写！";
                    return;
                }
                //sql = string.Format(@"  select WorkSnNumber from WorkSn where WorkSnName in ('{0}')", Server.UrlDecode(ToolManager.GetQueryString("WorkSnNumber")));
                sql = string.Format(@" update ProductWorkSnProperty set RowNumber ='{0}',RatedManhour='{1}',Remark ='{2}'
                where ProductNumber='{3}' and Version='{4}' and WorkSnNumber= '{5}'", RowNumber, RatedManhour,
                Remark, ProductNumber, Version, lbWorkSnNumber.Text.Trim());
                sqls.Add(sql);
                sql = string.Format(@"update product set ratedManhour=(
select sum(ratedManhour) from ProductWorkSnProperty where  productNumber='{0}' and version ='{1}')
where productNumber='{0}' and version='{1}'", ProductNumber, Version);
                sqls.Add(sql);
                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑产品工序信息" + lbProductNumber.Text, "编辑成功");

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑产品工序信息" + lbProductNumber.Text, "编辑失败！原因：" + error);
                }
                new BLL.ToolChangeProduct().changeproduct(ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"));
                return;
            }
        }
    }
}
