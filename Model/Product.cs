using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Product
    {
        public string ProductNumber { get; set; }
        public string Version { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }
        public string Type { get; set; }
        public double RatedManhour { get; set; }
        public double QuoteManhour { get; set; }
        public Decimal CostPrice { get; set; }
        public Decimal SalesQuotation { get; set; }
        public string HalfProductPosition { get; set; }
        public string FinishedGoodsPosition { get; set; }
        public string CustomerId { get; set; }
        public string CustomerProductNumber { get; set; }
        public string Remark { get; set; }
        public string IsOldVersion { get; set; }
        public string Cargo { get; set; }
        public string Unit { get; set; }
        public string NumberProperties { get; set; }
    }
}
