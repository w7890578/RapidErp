using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using System.Data;

namespace Rapid.PurchaseManager
{
    public partial class SupplierDetailedInfoList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                onload();
            }
        }

        private void onload()
        {
            string sql = string.Empty;
            string error = string.Empty;
            if (ToolManager.CheckQueryString("Id"))
            {
                sql = string.Format(@" select * from SupplierInfo where SupplierId='{0}' ", ToolManager.GetQueryString("Id"));
                SupplierInfo supplierinfo = SupplierInfoManager.ConvertDataTableToModel(sql);
                this.lbSupplierId.InnerText = supplierinfo.SupplierId;
                this.lbSupplierName.InnerText = supplierinfo.SupplierName;
                this.lbRegisteredAddress.InnerText = supplierinfo.RegisteredAddress;
                this.lbLegalPerson.InnerText = supplierinfo.LegalPerson;
                this.lbContacts.InnerText = supplierinfo.Contacts;
                this.lbRegisteredPhone.InnerText = supplierinfo.RegisteredPhone;
                this.lbContactTelephone.InnerText = supplierinfo.ContactTelephone;
                this.lbFax.InnerText = supplierinfo.Fax;
                this.lbMobilePhone.InnerText = supplierinfo.MobilePhone;
                this.lbEmail.InnerText = supplierinfo.Email;
                this.lbQQ.InnerText = supplierinfo.QQ;
                this.lbZipCode.InnerText = supplierinfo.ZipCode;
                this.lbSparePhone.InnerText = supplierinfo.SparePhone;
                this.lbAccountBank.InnerText = supplierinfo.AccountBank;
                this.lbBankRowNumber.InnerText = supplierinfo.BankRowNumber;
                this.lbBankAccount.InnerText = supplierinfo.BankAccount;
                this.lbTaxNo.InnerText = supplierinfo.TaxNo;
                this.lbDeliveryAddress.InnerText = supplierinfo.DeliveryAddress;
                this.lbWebsiteAddress.InnerText = supplierinfo.WebsiteAddress;
                this.lbRemark.InnerText = supplierinfo.Remark;
                lbPaymentdays.InnerText = supplierinfo.paymentdays + "天";
                lbPercentageInAdvance.InnerText = (Convert.ToDouble(supplierinfo.percentageInAdvance) * 100).ToString() + "%";
            }
        }
    }
}
