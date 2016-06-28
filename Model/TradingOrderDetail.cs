using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    /// <summary>
    /// 贸易销售订单实体
    /// </summary>
    public class TradingOrderDetail
    {
        public string OrdersNumber { get; set; }
        public string ProductNumber { get; set; }
        public string RowNumber { get; set; }
        public string Delivery { get; set; }
        public string SN { get; set; }
        public string ProductModel { get; set; }
        public string CustomerMaterialNumber { get; set; }
        public string MaterialName { get; set; }
        public string Quantity { get; set; }
        public string NonDeliveryQty { get; set; }
        public string DeliveryQty { get; set; }
        public string UnitPrice { get; set; }
        public string TotalPrice { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string CreateTime { get; set; }
        public string CustomerId { get; set; }
        public string DW { get; set; }//单位
    }

    /// <summary>
    /// 加工销售订单实体
    /// </summary>
    public class MachineOderDetail
    {
        public string OrdersNumber { get; set; }
        public string RowNumber { get; set; }
        public string SN { get; set; }
        public string LeadTime { get; set; }
        public string CustomerProductNumber { get; set; }
        public string Qty { get; set; }
        public string CustomerId { get; set; }
        public string Remark { get; set; }
        public string Version { get; set; }
    }

    /// <summary>
    /// 总订单实体
    /// </summary>
    public class SaleOder
    {
        public string OrdersNumber { get; set; }
        public string OrdersDate { get; set; }
        public string ProductType { get; set; }
        public string OdersType { get; set; }
        public string MakeCollectionsMode { get; set; }
        public string CustomerId { get; set; }
        /// <summary>
        /// 客户采购订单号
        /// </summary>
        public string CustomerOrderNumber { get; set; }
        /// <summary>
        /// 业务员编号
        /// </summary>
        public string ContactId { get; set; }
        /// <summary>
        /// 客户订单号
        /// </summary>
        public string KhddH { get; set; }
        public string CreateTime { get; set; }
        public string Remark { get; set; }
        public string CustomerName { get; set; }
    }
}
