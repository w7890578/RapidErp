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
    public partial class AddOrEditPackageInfoCustomerProperty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("PackageNumber"))
                {
                    ControlBindManager.BindCustomer(this.drpCustomer);
                    string PackageNumber = ToolManager.GetQueryString("PackageNumber");
                    string customerid = Server.UrlDecode(ToolManager.GetQueryString("CustomerId"));
                    string error = string.Empty;
                    string sql = string.Format(@"  select p.PackageNumber,c.CustomerName,p.CustomerPackageInfoNumber,p.Remark from 
                    PackageInfoCustomerProperty p left join Customer c on p.CustomerId=c.CustomerId where PackageNumber='{0}' and 
                    p.CustomerId='{1}'", PackageNumber, customerid);
                    if (!ToolManager.CheckQueryString("CustomerId"))
                    {
                        this.btnSubmit.Text = "添加";
                        this.lbPackageNumber.Text = PackageNumber;
                        this.lbCustomer.Visible = false;
                    }
                    else
                    {
                        this.btnSubmit.Text = "修改";
                        this.drpCustomer.Visible = false;
                        this.lbPackageNumber.Text = PackageNumber;
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            lbPackageNumber.Text = dr["PackageNumber"] == null ? "" : dr["PackageNumber"].ToString();
                            lbCustomer.Text = dr["CustomerName"] == null ? "" : dr["CustomerName"].ToString();
                            txtCustomerPackageInfoNumber.Text = dr["CustomerPackageInfoNumber"] == null ? "" : dr["CustomerPackageInfoNumber"].ToString();
                            txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                            this.btnSubmit.Text = "修改";
                        }

                    }
                
                }
                else
                {
                    Response.Write("请选择包编码！");
                    Response.End();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(lbPackageNumber.Text))
            {
                Response.Write("请选择包编码！");
                Response.End();
            }
            string PackageNumber = this.lbPackageNumber.Text.Trim();
            string CustomerId = this.drpCustomer.SelectedValue;
            string CustomerPackageInfoNumber = txtCustomerPackageInfoNumber.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string sql = string.Empty;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from PackageInfoCustomerProperty where PackageNumber='{0}' and CustomerId='{1}'", PackageNumber, CustomerId);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该包的客户属性！";
                    return;
                }
                sql = string.Format(@" insert into PackageInfoCustomerProperty (PackageNumber,CustomerId,CustomerPackageInfoNumber,Remark )
 values('{0}','{1}','{2}','{3}')", PackageNumber, CustomerId, CustomerPackageInfoNumber, Remark);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加包客户信息" + PackageNumber, "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加包客户信息" + PackageNumber, "增加失败！原因：" + error);
                    return;
            }
            }
            else
            {
                sql = string.Format(@" select COUNT(*) from PackageInfo where PackageNumber='{0}' ", PackageNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该包编码不存在，请重新填写！";
                    return;
                }
                sql = string.Format(@"update PackageInfoCustomerProperty set CustomerPackageInfoNumber='{0}',Remark ='{1}'
 where PackageNumber='{2}' and CustomerId='{3}'", CustomerPackageInfoNumber, Remark, PackageNumber, Server.UrlDecode(ToolManager.GetQueryString("CustomerId")));
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑包客户信息" + PackageNumber, "编辑成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑包客户信息" + PackageNumber, "编辑失败！原因：" + error);
                    return;
                }
            }
        }
    }
}
