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
    public partial class MaterialCustomerProperty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("MaterialNumber"))
                {
                    ControlBindManager.BindCustomer(this.drpCustomer);
                    string MaterialNumber = ToolManager.GetQueryString("MaterialNumber");
                    string customerid = Server.UrlDecode(ToolManager.GetQueryString("CustomerId"));
                    string error = string.Empty;
                    string sql = string.Format(@" select * from MaterialCustomerProperty where MaterialNumber='{0}' and CustomerId='{1}'", MaterialNumber, customerid);
                    if (!ToolManager.CheckQueryString("CustomerId"))
                    {
                        this.btnSubmit.Text = "添加";
                        this.lbMaterialNumber.Text = MaterialNumber;
                        this.lbCustomerMaterialNumber.Visible = false;
                        this.txtCustomerMaterialNumber.Visible = true;
                        
                    }
                    else
                    {
                        this.btnSubmit.Text = "修改";
                        this.lbCustomerMaterialNumber.Visible = true;
                        this.txtCustomerMaterialNumber.Visible = false;
                        drpCustomer.Enabled = false;
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            lbCustomerMaterialNumber.Text = dr["CustomerMaterialNumber"] == null ? "" : dr["CustomerMaterialNumber"].ToString();
                            drpCustomer.SelectedValue = dr["CustomerId"] == null ? "" : dr["CustomerId"].ToString();
                            txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                            this.btnSubmit.Text = "修改";
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
            string CustomerMaterialNumber = txtCustomerMaterialNumber.Text.Trim();
            string CustomerId = this.drpCustomer.SelectedValue;
            string Remark = txtRemark.Text.Trim();
            string sql = string.Empty;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from MaterialCustomerProperty where MaterialNumber='{0}' and CustomerId='{1}'", MaterialNumber, CustomerId);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该原材料的客户属性！";
                    return;
                }
                sql = string.Format(@" insert into MaterialCustomerProperty (MaterialNumber,CustomerMaterialNumber,CustomerId,Remark )
 values('{0}','{1}','{2}','{3}')", MaterialNumber, CustomerMaterialNumber, CustomerId, Remark);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料客户信息" + MaterialNumber, "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料客户信息" + MaterialNumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                CustomerMaterialNumber = lbCustomerMaterialNumber.Text;
                sql = string.Format(@" select * from Customer where 
                   CustomerId='{0}' ", CustomerId);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该原材料的客户属性已被删除";
                    return;
                }
                sql = string.Format(@" select COUNT(*) from MarerialInfoTable where MaterialNumber='{0}' ", MaterialNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该原材料编号不存在，请重新填写！";
                    return;
                }
                sql = string.Format(@"update MaterialCustomerProperty set CustomerMaterialNumber='{0}',Remark ='{1}'
 where MaterialNumber='{2}' and CustomerId='{3}'", CustomerMaterialNumber, Remark, MaterialNumber, CustomerId);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料客户信息" + MaterialNumber, "编辑成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料客户信息" + MaterialNumber, "编辑失败！原因：" + error);
                    return;
                }
            }
        }
    }
}
