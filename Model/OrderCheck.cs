using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OrderCheck
    {

        /// <summary>
        /// Id
        /// </summary>	 
        public Guid Id { get; set; }
        /// <summary>
        /// 出入库单号
        /// </summary>	 
        public string WarehouseNumber { get; set; }
        /// <summary>
        /// 原材料编号
        /// </summary>	 
        public string MaterialNumber { get; set; }
        /// <summary>
        /// 抽检数量
        /// </summary>	 
        public double TakeQty { get; set; }
        /// <summary>
        /// 抽检时间
        /// </summary>	 
        public DateTime TakeDateTime { get; set; }
        public string TakeDateTimeStr
        {
            get
            {
                if (TakeDateTime != null && TakeDateTime != DateTime.Parse("1900-01-01"))
                {
                    return TakeDateTime.ToString("yyyy-MM-dd");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 状态
        /// </summary>	 
        public OrderCheckStatus Status { get; set; }
        /// <summary>
        /// 抽检人
        /// </summary>	 
        public string TakeUserId { get; set; }
        /// <summary>
        /// 抽检人姓名
        /// </summary>
        public string TakeUserName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>	 
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>	 
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>	 
        public CheckOrderType OrderType { get; set; }

    }

    public enum OrderCheckStatus
    {
        不合格 = 1,
        合格 = 2
    }

    public enum CheckOrderType
    {
        采购入库 = 1
    }
}
