using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class AddOrEditCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }
        private void LoadPage()
        {
            string sql = string.Empty;
            string error = string.Empty;
            ControlBindManager.BindDrp(" select id,MakeCollectionsMode from MakeCollectionsMode ", this.drpMakeCollectionsModeId, "id", "MakeCollectionsMode");
            if (ToolManager.CheckQueryString("Id"))
            {
                sql = string.Format(@" select * from Customer where CustomerId='{0}' ", ToolManager.GetQueryString("Id"));
                //sql = string.Format(@" select * from Customer where CustomerId='{0}' ", "5");
                this.trCustomerNuber.Visible = false;
                Customer customer = CustomerInfoManager.ConvertDataTableToModel(sql);
                this.txtCustomerNumber.Text = customer.CustomerId;
                this.txtCustomerName.Text = customer.CustomerName;
                this.txtRegisteredAddress.Text = customer.RegisteredAddress;
                this.txtLegalPerson.Text = customer.LegalPerson;
                this.txtContacts.Text = customer.Contacts;
                this.txtRegisteredPhone.Text = customer.RegisteredPhone;
                this.txtContactTelephone.Text = customer.ContactTelephone;
                this.txtFax.Text = customer.Fax;
                this.txtMobilePhone.Text = customer.MobilePhone;
                this.txtZipCode.Text = customer.ZipCode;
                this.txtSparePhone.Text = customer.SparePhone;
                this.txtEmail.Text = customer.Email;
                this.txtQQ.Text = customer.QQ;
                this.txtAccountBank.Text = customer.AccountBank;
                this.txtSortCode.Text = customer.SortCode;
                this.txtBankAccount.Text = customer.BankAccount;
                this.txtTaxNo.Text = customer.TaxNo;
                this.txtWebsiteAddress.Text = customer.WebsiteAddress;
                this.txtDeliveryAddress.Text = customer.DeliveryAddress;
                //this.txtPaymentdays.Text = customer.Paymentdays.ToString();
                //this.drpPercentageInAdvance.SelectedValue = customer.PercentageInAdvance.ToString();
                this.txtRemark.Text = customer.Remark;
                this.drpMakeCollectionsModeId.SelectedValue = customer.MakeCollectionsModeId;
                this.txtFactoryAddress.Text = customer.FactoryAddress;
                this.drpReceiveType.SelectedValue = customer.ReceiveType;
                btnSubmit.Text = "修改";
            }
            else {

                btnSubmit.Text = "添加";
            
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        { 
            string error = string.Empty;
            Model.Customer customer = new Model.Customer();
            customer.CustomerId = this.txtCustomerNumber.Text.Trim();
            customer.CustomerName = this.txtCustomerName.Text.Trim();
            customer.RegisteredAddress = this.txtRegisteredAddress.Text.Trim();
            customer.LegalPerson = this.txtLegalPerson.Text.Trim();
            customer.Contacts = this.txtContacts.Text.Trim();
            customer.RegisteredPhone = this.txtRegisteredPhone.Text.Trim();
            customer.ContactTelephone = this.txtContactTelephone.Text.Trim();
            customer.Fax = this.txtFax.Text.Trim();
            customer.MobilePhone = this.txtMobilePhone.Text.Trim();
            customer.ZipCode = this.txtZipCode.Text.Trim();
            customer.SparePhone = this.txtSparePhone.Text.Trim();
            customer.Email = this.txtEmail.Text.Trim();
            customer.QQ = this.txtQQ.Text.Trim();
            customer.AccountBank = this.txtAccountBank.Text.Trim();
            customer.SortCode = this.txtSortCode.Text.Trim();
            customer.BankAccount = this.txtBankAccount.Text.Trim();
            customer.TaxNo = this.txtTaxNo.Text.Trim();
            customer.WebsiteAddress = this.txtWebsiteAddress.Text.Trim();
            customer.DeliveryAddress = this.txtDeliveryAddress.Text.Trim();
            customer.Paymentdays =0;
            customer.PercentageInAdvance = 0;
            customer.Remark = this.txtRemark.Text.Trim();
            customer.FactoryAddress = this.txtFactoryAddress.Text.Trim();
            customer.MakeCollectionsModeId = this.drpMakeCollectionsModeId.SelectedValue;
            customer.ReceiveType = this.drpReceiveType.SelectedValue;
            if (string.IsNullOrEmpty(customer.CustomerId) || string.IsNullOrEmpty(customer.CustomerName))
            {
                lbSubmit.Text = "请将带*号的内容填写完整！";
                return;
            }
            bool result = false;
            if (btnSubmit.Text.Equals("添加"))
            {
                result = CustomerInfoManager.AddCustomer(customer, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加客户信息" + customer.CustomerId, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加客户信息" + customer.CustomerId, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                result = CustomerInfoManager.EditCustomer(customer, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "修改客户信息" + customer.CustomerId, "修改成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "修改客户信息" + customer.CustomerId, "修改失败！原因" + error);
                    return;
                }
            }
        }
    }
}
