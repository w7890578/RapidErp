using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using BLL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.PurchaseManager
{
    public partial class AddOrEditSupplierInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindDrp("select distinct PaymentMode from PaymentMode", this.drpPaymentMode, "PaymentMode", "PaymentMode");
                LoadPage();
            }
        }

        private void LoadPage()
        {
            string sql = string.Empty;
            string error = string.Empty;

            if (ToolManager.CheckQueryString("Id"))
            {
                sql = string.Format(@" select * from SupplierInfo where SupplierId='{0}' ", ToolManager.GetQueryString("Id"));
                //sql = string.Format(@" select * from SupplierInfo where SupplierId='{0}' ", "1");
                this.trSupplierId.Visible = false;
                SupplierInfo supplierinfo = SupplierInfoManager.ConvertDataTableToModel(sql);
                this.txtSupplierId.Text = supplierinfo.SupplierId;
                this.txtSupplierName.Text = supplierinfo.SupplierName;
                this.txtRegisteredAddress.Text = supplierinfo.RegisteredAddress;
                this.txtLegalPerson.Text = supplierinfo.LegalPerson;
                this.txtContacts.Text = supplierinfo.Contacts;
                this.txtRegisteredPhone.Text = supplierinfo.RegisteredPhone;
                this.txtContactTelephone.Text = supplierinfo.ContactTelephone;
                this.txtFax.Text = supplierinfo.Fax;
                this.txtMobilePhone.Text = supplierinfo.MobilePhone;
                this.txtZipCode.Text = supplierinfo.ZipCode;
                this.txtSparePhone.Text = supplierinfo.SparePhone;
                this.txtEmail.Text = supplierinfo.Email;
                this.txtQQ.Text = supplierinfo.QQ;
                this.txtAccountBank.Text = supplierinfo.AccountBank;
                this.txtBankRowNumber.Text = supplierinfo.BankRowNumber;
                this.txtBankAccount.Text = supplierinfo.BankAccount;
                this.txtTaxNo.Text = supplierinfo.TaxNo;
                this.txtWebsiteAddress.Text = supplierinfo.WebsiteAddress;
                this.txtDeliveryAddress.Text = supplierinfo.DeliveryAddress;
                this.txtRemark.Text = supplierinfo.Remark;
                this.drpPaymentMode.SelectedValue = supplierinfo.PaymentMode;
                //this.txtPaymentdays.Text = supplierinfo.paymentdays.ToString();
                //this.drpPercentageInAdvance.SelectedValue = supplierinfo.percentageInAdvance.ToString();
                this.txtFactoryAddress.Text = supplierinfo.FactoryAddress;
                this.drpPayType.SelectedValue = supplierinfo.PayType;
                btnSubmit.Text = "修改";
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            Model.SupplierInfo supplierinfo = new SupplierInfo();
            supplierinfo.SupplierId = this.txtSupplierId.Text.Trim();
            supplierinfo.SupplierName = this.txtSupplierName.Text.Trim();
            supplierinfo.RegisteredAddress = this.txtRegisteredAddress.Text.Trim();
            supplierinfo.LegalPerson = this.txtLegalPerson.Text.Trim();
            supplierinfo.Contacts = this.txtContacts.Text.Trim();
            supplierinfo.RegisteredPhone = this.txtRegisteredPhone.Text.Trim();
            supplierinfo.ContactTelephone = this.txtContactTelephone.Text.Trim();
            supplierinfo.Fax = this.txtFax.Text.Trim();
            supplierinfo.MobilePhone = this.txtMobilePhone.Text.Trim();
            supplierinfo.ZipCode = this.txtZipCode.Text.Trim();
            supplierinfo.SparePhone = this.txtSparePhone.Text.Trim();
            supplierinfo.Email = this.txtEmail.Text.Trim();
            supplierinfo.QQ = this.txtQQ.Text.Trim();
            supplierinfo.AccountBank = this.txtAccountBank.Text.Trim();
            supplierinfo.BankRowNumber = this.txtBankRowNumber.Text.Trim();
            supplierinfo.BankAccount = this.txtBankAccount.Text.Trim();
            supplierinfo.TaxNo = this.txtTaxNo.Text.Trim();
            supplierinfo.WebsiteAddress = this.txtWebsiteAddress.Text.Trim();
            supplierinfo.DeliveryAddress = this.txtDeliveryAddress.Text.Trim();
            supplierinfo.Remark = this.txtRemark.Text.Trim();
            supplierinfo.paymentdays = 0;
            supplierinfo.percentageInAdvance = 0;
            supplierinfo.FactoryAddress = this.txtFactoryAddress.Text.Trim();
            supplierinfo.PayType = this.drpPayType.SelectedValue;
            supplierinfo.PaymentMode = this.drpPaymentMode.SelectedValue;
            if (string.IsNullOrEmpty(supplierinfo.SupplierId) || string.IsNullOrEmpty(supplierinfo.SupplierName)|| string.IsNullOrEmpty(supplierinfo.PaymentMode))
            {
                lbSubmit.Text = "请将带*号的内容填写完整！";
                return;
            }
            bool result;
            if (btnSubmit.Text.Equals("添加"))
            {
                result = SupplierInfoManager.AddSupplierInfo(supplierinfo, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加供应商信息" + supplierinfo.SupplierId, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加供应商信息" + supplierinfo.SupplierId, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                result = SupplierInfoManager.EditSupplierInfo(supplierinfo, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑供应商信息" + supplierinfo.SupplierId, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑供应商信息" + supplierinfo.SupplierId, "编辑失败！原因" + error);
                    return;
                }

            }
        }


    }
}
