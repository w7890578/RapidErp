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
    public partial class AddOrEditProductCustomerProperty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
                {
                    ControlBindManager.BindCustomer(this.drpCustomer);
                    string ProductNumber = Server.UrlDecode(ToolManager.GetQueryString("ProductNumber"));
                    string Version = Server.UrlDecode(ToolManager.GetQueryString("Version"));
                    string CustomerId = Server.UrlDecode(ToolManager.GetQueryString("CustomerId"));
                    string error = string.Empty;
                    string sql = string.Format(" select * from ProductCustomerProperty where ProductNumber='{0}' and Version='{1}' and CustomerId='{2}'", ProductNumber, Version, CustomerId);
                    if (!ToolManager.CheckQueryString("CustomerId"))
                    {
                        this.btnSubmit.Text = "添加";
                        this.lbCustomerProductNumber.Visible = false;
                        this.txtCustomerProductNumber.Visible = true;

                    }
                    else
                    {
                        this.btnSubmit.Text = "修改";
                        this.lbCustomerProductNumber.Visible = true;
                        this.txtCustomerProductNumber.Visible = false;
                        this.drpCustomer.Visible = false;
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        drpCustomer.Enabled = false;

                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            lbProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                            lblCustomer.Text = dr["CustomerId"] == null ? "" : dr["CustomerId"].ToString();
                            lbVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                            lbCustomerProductNumber.Text = dr["CustomerProductNumber"] == null ? "" : dr["CustomerProductNumber"].ToString();
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
            string CustomerProductNumber = this.txtCustomerProductNumber.Text.Trim();
            string CustomerId = this.drpCustomer.SelectedValue;
            string Remark = txtRemark.Text;
            string sql = string.Empty;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                if (CustomerId == "")
                {
                    lbSubmit.Text = "请选择客户！！！";
                    return;

                }

                sql = string.Format(@"
select count(0) from ProductCustomerProperty  where ProductNumber='{0}'
                and Version='{1}' and CustomerProductNumber='{2}'
", ProductNumber, Version, CustomerProductNumber);
                if (SqlHelper.GetScalar(sql) != "0")
                {
                    lbSubmit.Text = "已存在该客户产成品编号！请重新填写！";
                    return;
                }

                sql = string.Format(@" select * from ProductCustomerProperty  where ProductNumber='{0}'
                and Version='{1}' and CustomerId='{2}'", ProductNumber, Version, CustomerId);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该产品的客户编号！请重新填写！";
                    return;
                }

                sql = string.Format(@" insert into ProductCustomerProperty (ProductNumber,Version,CustomerProductNumber,CustomerId,Remark )
 values('{0}','{1}','{2}','{3}','{4}')", ProductNumber, Version, CustomerProductNumber, CustomerId, Remark);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加产品客户信息" + ProductNumber, "增加成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加产品客户信息" + ProductNumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {

                sql = string.Format(@" select * from ProductCustomerProperty  where ProductNumber='{0}'
                and Version='{1}' and CustomerId='{2}'", ProductNumber, Version, CustomerId);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该产成品的客户属性已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" select COUNT(*) from Product where ProductNumber='{0}' and Version='{1}' ", ProductNumber, Version);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该产成品编号或版本不存在，请重新填写！";
                    return;
                }
                sql = string.Format(@"update ProductCustomerProperty set CustomerProductNumber ='{0}',Remark ='{1}'
 where ProductNumber='{2}' and Version='{3}' and CustomerId='{4}'", lbCustomerProductNumber.Text, Remark, ProductNumber, Version, CustomerId);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
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
    }
}
