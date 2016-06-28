using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class InvoiceAccountInfo
    {
        public string SN { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceMoney { get; set; }
        public string CustomerId { get; set; }
        public string Money { get; set; }
        public string Money_C { get; set; }
        public string IsPay { get; set; }
        public string InvoiceType { get; set; }
    }
}
