using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class NonDeliveryCompare
    {
        public string ImportTime { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerProductNumber { get; set; }
        public string CustomerId { get; set; }
        public int RowNumber { get; set; }
        public string SupplierId { get; set; }
        public string UserId { get; set; }
        public string CertificateDate { get; set; }
        public string DeliveryDate { get; set; }
        public string ShortText { get; set; }
        public int OrderQty { get; set; }
        public int DeliveryQty { get; set; }
        public int StillDeliveryQty { get; set; }
        public string CollectNumber { get; set; }
    }
}
