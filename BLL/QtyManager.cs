using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BLL
{
    public class QtyManager
    {
        #region Instance
        private static QtyManager _instance = null;
        public static QtyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new QtyManager();
                }
                return _instance;
            }
        }
        private QtyManager()
        { }
        #endregion

        /// <summary>
        /// 通过原材料编号获取相应的实际产品需求数（MRP应用）
        /// </summary>
        /// <param name="materialNumber"></param>
        /// <returns></returns>
        public DataTable GetProductActualQty(string materialNumber)
        {
            string sql = string.Format(" exec P_GetProductActualQty  '{0}' ", materialNumber);
            return SqlHelper.GetTable(sql);
        }

        /// <summary>
        /// 通过原材料编号获取相应的实际产品需求数（MRP应用）
        /// </summary>
        /// <param name="materialNumber"></param>
        /// <returns></returns>
        public DataTable GetMaterialActualQty(string materialNumber)
        {
            string sql = "exec P_GetMaterialActualQty  '','','',0";
            DataTable dtResult = SqlHelper.GetTable(sql);

            DataTable dt = GetProductActualQty(materialNumber);
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    sql = string.Format(@" 
                    exec P_GetMaterialActualQty '{0}' ,'{1}','{2}',{3}"
                  , materialNumber
                  , row["产成品编号"] == null ? "" : row["产成品编号"].ToString()
                  , row["版本"] == null ? "" : row["版本"].ToString()
                  , row["订单未交减库存减在制减送货单未确认数量"]);
                    DataTable temp = SqlHelper.GetTable(sql);
                    if (temp != null && temp.Rows.Count > 0)
                    {
                        DataRow drResult = dtResult.NewRow();
                        drResult["产成品编号"] = temp.Rows[0]["产成品编号"];
                        drResult["版本"] = temp.Rows[0]["版本"];
                        drResult["BOM换算后"] = temp.Rows[0]["BOM换算后"];
                        dtResult.Rows.Add(drResult);
                    }
                }
            }
            return dtResult;
        }

        public double GetWeiQueRenSongHuoDan(string materialNumber)
        {
            string sql = string.Format(@"
select t.qty from (
select ProductNumber AS MaterialNumber, 0-sum(isnull( DeliveryQty,0)) as qty from  [DeliveryNoteDetailed] 
where DeliveryNumber in (select distinct DeliveryNumber from [DeliveryBill] where IsConfirm !='已确认')
and ISNULL(Version,'')=''
group by ProductNumber)t 
where t.MaterialNumber='{0}'
", materialNumber);
            string ob = SqlHelper.GetScalar(sql);
            return string.IsNullOrEmpty(ob) ? 0 : Convert.ToDouble(ob);
        }

        public double GetMaoYiWeiJiao(string materialNumber)
        {
            string sql = string.Format(@"
SELECT       
           SUM(NonDeliveryQty) qty      
   FROM TradingOrderDetail      
   WHERE NonDeliveryQty>0      
     AND OdersNumber IN      
       (SELECT DISTINCT OdersNumber      
        FROM SaleOder      
        WHERE CheckTime IS NOT NULL)    
        and ProductNumber='{0}'  
   GROUP BY ProductNumber
", materialNumber);
            string ob = SqlHelper.GetScalar(sql);
            return string.IsNullOrEmpty(ob) ? 0 : Convert.ToDouble(ob);
        }

        public DataTable GetInBusinessQty(string productNumber, string version)
        {
            string sql = string.Format("exec GetInBusinessQty '{0}','{1}'", productNumber, version);
            return SqlHelper.GetTable(sql);
        }
    }
}
