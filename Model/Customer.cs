using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    [Serializable]
    public class Customer
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string RegisteredAddress { get; set; }
        public string LegalPerson { get; set; }
        public string Contacts { get; set; }
        public string RegisteredPhone { get; set; }
        public string ContactTelephone { get; set; }
        public string Fax { get; set; }
        public string MobilePhone { get; set; }
        public string ZipCode { get; set; }
        public string SparePhone { get; set; }
        public string Email { get; set; }
        public string QQ { get; set; }
        public string AccountBank { get; set; }
        public string SortCode { get; set; }
        public string BankAccount { get; set; }
        public string TaxNo { get; set; }
        public string WebsiteAddress { get; set; }
        public string DeliveryAddress { get; set; }

        public string Remark { get; set; }
        public int Paymentdays { get; set; }
        public Double PercentageInAdvance { get; set; }
        public string FactoryAddress { get; set; }
        public string MakeCollectionsModeId { get; set; }
        public string ReceiveType { get; set; }
    }
}
