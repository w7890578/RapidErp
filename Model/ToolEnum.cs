using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    /// <summary>
    /// 通用枚举类
    /// </summary>
    public class ToolEnum
    {
        /// <summary>
        /// 产品类型（原材料Or产品）
        /// </summary>
        public enum ProductType
        {
            /// <summary>
            /// 原材料
            /// </summary> 
            Marerial,
            /// <summary>
            /// 产品
            /// </summary>
            Product
        }

        /// <summary>
        /// 出入库类型
        /// </summary>
        public enum OutOfStorageType
        {
            /// <summary>
            /// 入库
            /// </summary>
            In,
            /// <summary>
            /// 出库
            /// </summary>
            Out
        }
    }
}
