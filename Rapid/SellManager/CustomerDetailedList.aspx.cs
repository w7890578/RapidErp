using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using System.Data;


namespace Rapid.ProduceManager
{
    public partial class CustomerDetailedList : System.Web.UI.Page
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
                sql = string.Format(@" select * from Customer where CustomerId='{0}' ", ToolManager.GetQueryString("Id"));        
                Customer customer = CustomerInfoManager.ConvertDataTableToModel(sql);
                this.lbCustomerId.InnerText = customer.CustomerId;
                this.lbCustomerName.InnerText = customer.CustomerName;
                this.lbRegisteredAddress.InnerText = customer.RegisteredAddress;
                this.lbLegalPerson.InnerText = customer.LegalPerson;
                this.lbContacts.InnerText = customer.Contacts;
                this.lbRegisteredPhone.InnerText = customer.RegisteredPhone;
                this.lbContactTelephone.InnerText = customer.ContactTelephone;
                this.lbFax.InnerText = customer.Fax;
                this.lbMobilePhone.InnerText = customer.MobilePhone;
                this.lbEmail.InnerText = customer.Email;
                this.lbQQ.InnerText = customer.QQ;
                this.lbZipCode.InnerText = customer.ZipCode;
                this.lbSparePhone.InnerText = customer.SparePhone;
                this.lbAccountBank.InnerText = customer.AccountBank;
                this.lbSortCode.InnerText = customer.SortCode;
                this.lbBankAccount.InnerText = customer.BankAccount;
                this.lbTaxNo.InnerText = customer.TaxNo;
                this.lbDeliveryAddress.InnerText = customer.DeliveryAddress;
                this.lbWebsiteAddress.InnerText = customer.WebsiteAddress;
                this.lbRemark.InnerText = customer.Remark;
            }
        }
    }
}
