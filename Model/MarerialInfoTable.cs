using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    /// <summary>
    /// 原材料实体旧
    /// </summary>
    public class MarerialInfoTable
    {
        public string MaterialNumber { get; set; }
        public string MaterialName { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int StockSafeQty { get; set; }
        public Decimal ProcurementPrice { get; set; }
        public string MaterialPosition { get; set; }
        public string MinPacking { get; set; }
        public string MinOrderQty { get; set; }
        public string ScrapPosition { get; set; }
        public string Remark { get; set; }
        public string Cargo { get; set; }
        public string SixStockSafeQty { get; set; }
        public string CargoType { get; set; }
        public string NumberProperties { get; set; }
        public string Unit { get; set; }
    }
    /// <summary>
    /// 原材料实体新
    /// </summary>
    public class MarerialInfoTableNew
    {
        public string MaterialNumber { get; set; }
        public string MaterialName { get; set; }
        public string Description { get; set; }
        public string Kind { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int StockSafeQty { get; set; }
        public double ProcurementPrice { get; set; }
        public string MaterialPosition { get; set; }
        public int MinPacking { get; set; }
        public int MinOrderQty { get; set; }
        public string ScrapPosition { get; set; }
        public string Remark { get; set; }
        public string Cargo { get; set; }
        public int SixStockSafeQty { get; set; }
        public string SupplierId { get; set; }
        public string SupplierMaterialNumber { get; set; }
        public string SupplierMinOrderQty { get; set; }
        public string CustomerId { get; set; }
        public string CustomerMaterialNumber { get; set; }
    }
}
