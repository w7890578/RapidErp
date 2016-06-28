using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.StoreroomManager
{
    public partial class AddFLCKDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
           string planNumber =string.Empty;
            string customerMaterilNumber = txtCustomerMaterilNumber.Text.Trim();
            string material = string.Empty;
            string qty = txtQty.Text;
            string sql = string.Empty;
            if (string.IsNullOrEmpty(customerMaterilNumber) || string.IsNullOrEmpty(qty)||string.IsNullOrEmpty (customerMaterilNumber ))
            {
                lbSubmit.Text = "信息填写不完整";
                return;
            }
            if (drpType.SelectedValue == "供应商物料编号")
            {

                sql = string.Format(@"select top 1 MaterialNumber from MaterialSupplierProperty where SupplierMaterialNumber='{0}'", customerMaterilNumber);
                if (SqlHelper.GetScalar(sql).Equals(""))
                {
                    lbSubmit.Text = "未知的供应商物料编号！";
                    return;
                }
                else
                {
                    material = SqlHelper.GetScalar(sql);
                }
            }
            if (drpType.SelectedValue == "客户物料编号")
            {
                sql = string.Format(@"select top 1 MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", customerMaterilNumber);
                if (SqlHelper.GetScalar(sql).Equals(""))
                {
                    lbSubmit.Text = "未知的客户物料编号！";
                    return;
                }
                else
                {
                    material = SqlHelper.GetScalar(sql);
                }
            }
            if (drpType.SelectedValue == "瑞普迪编号")
            {
                sql = string.Format(@"select top 1 MaterialNumber from MarerialInfoTable where MaterialNumber='{0}'", customerMaterilNumber);
                if (SqlHelper.GetScalar(sql).Equals(""))
                {
                    lbSubmit.Text = "未知的瑞普迪编号！";
                    return;
                }
                else
                {
                    material = SqlHelper.GetScalar(sql);
                }
            }
            if (drpType.SelectedValue == "物料描述")
            {
                sql = string.Format(@"select top 1 MaterialNumber from MarerialInfoTable where Description='{0}'", customerMaterilNumber);
                if (SqlHelper.GetScalar(sql).Equals(""))
                {
                    lbSubmit.Text = "未知的物料描述！";
                    return;
                }
                else
                {
                    material = SqlHelper.GetScalar(sql);
                }
            }
            string remark = txtRemark.Text;
            string error = string.Empty;
            bool result = StoreroomToolManager.AddFLCKDetail(warehouseNumber, planNumber, customerMaterilNumber, material, qty, remark, ref error);

            if (result)
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                lbSubmit.Text = "添加失败！原因：" + error;
            }
        }
    }
}
