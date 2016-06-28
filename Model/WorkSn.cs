using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class WorkSn
    {
        public string WorkSnNumber { get; set; }
        public string WorkSnName { get; set; }
        public string Sn { get; set; }
    }
    /// <summary>
    /// 开工单分表详细实体
    /// </summary>
    public class ProductPlanSubDetail
    {
        /// <summary>
        /// 开工单号
        /// </summary>
        public string PlanNumber { get; set; }
        public string Team { get; set; }
        public string OrdersNumber { get; set; }
        public string ProductNumber { get; set; }
        public string Version { get; set; }
        public string RowNumber { get; set; }
        public string CustomerProductNumber { get; set; }
        public string LeadTime { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Qty { get; set; }
        /// <summary>
        /// 缺料原材料编号
        /// </summary>
        public string QLMareialNumbers { get; set; }
        /// <summary>
        /// 是否为半成品
        /// </summary>
        public bool IsHalfProduct { get; set; }
    }
}
