using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;

namespace BLL
{
    public class FinancialManager
    {
        /// <summary>
        /// 生成采购应付账款sql语句
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static List<string> GetCSYF(string warehouseNumber)
        {
            string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            List<string> sqls = new List<string>();
            //            //找到这个采购入库单有多少张采购订单
            //            string sql = string.Format(@"select distinct DocumentNumber 
            //from MaterialWarehouseLogDetail where WarehouseNumber='{0}'", warehouseNumber);
            //            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            //            {

            //                sql = string.Format(@"insert into T_AccountsPayable_Main (OrdersNumber ,CreateTime ,SumPrice ,SupplierId,PaymentMode ,DeliveryDate)
            //select OrdersNumber,'{1}',sumPice,SupplierId,PaymentMode,'{2}' from V_CertificateOrders_sumPrice where OrdersNumber='{0}'", dr["DocumentNumber"], timeNow, dateNow);
            //                sqls.Add(sql);

            //                //明细
            //                sql = string.Format(@"insert into  T_AccountsPayable_Detail(OrdersNumber,CreateTime,MaterialNumber,SupplieNumber,Price,qty,Leadtime,SumPrice)
            //select DocumentNumber ,'{0}',MaterialNumber,SupplierMaterialNumber,UnitPrice,Qty,LeadTime,UnitPrice *Qty    
            //from MaterialWarehouseLogDetail
            //where WarehouseNumber='{1}' and DocumentNumber ='{2}'", timeNow, warehouseNumber, dr["DocumentNumber"]);
            //                sqls.Add(sql);
            //            }
            return sqls;
        }

        /// <summary>
        /// 单条送货单确认产生应收
        /// </summary>
        /// <param name="deleiyNumber"></param>
        /// <returns></returns>
        public static List<string> GetSHYS(string deleiyNumber)
        {
            string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
            List<string> sqls = new List<string>();
            string type = string.Empty;
            string sql = string.Format(@"
 select distinct OrdersNumber  from DeliveryNoteDetailed where DeliveryNumber='{0}'", deleiyNumber);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                //查找该订单的生产类型
                type = string.Format(@" select ProductType  from SaleOder where OdersNumber ='{0}'", dr["OrdersNumber"]);

                sql = string.Format(@"insert into AccountsReceivable(OrdersNumber ,CreateTime ,SKFS ,CustomerId ,DeliveryDate,SHUserId )
select OdersNumber,'{0}',MakeCollectionsMode ,CustomerId,(select DeliveryDate from DeliveryBill where DeliveryNumber='{1}') ,(select DeliveryPerson from DeliveryBill where DeliveryNumber='{1}')
from SaleOder where OdersNumber ='{2}'", timeNow, deleiyNumber, dr["OrdersNumber"]);
                sqls.Add(sql);

                if (type.Equals("贸易"))
                {
                    sql = string.Format(@"
insert into T_AccountsReceivable_Detail (OrdersNumber,CreateTime,ProductNumber,Version,CustomerProductNumber,Price ,qty ,SumPrice,
LeadTime  ,RowNumber )
select dnd.OrdersNumber ,'{0}',dnd.ProductNumber ,dnd.Version ,dnd.CustomerProductNumber,tod.UnitPrice,dnd.ConformenceQty
,isnull(tod.UnitPrice ,0)*ISNULL (dnd.ConformenceQty ,0),dnd.LeadTime ,dnd.RowNumber 
 from DeliveryNoteDetailed  dnd inner join TradingOrderDetail tod on dnd.OrdersNumber =tod.OdersNumber 
 and dnd.RowNumber =tod.RowNumber 
 where dnd.DeliveryNumber='{1}' and dnd.OrdersNumber ='{2}'", timeNow, deleiyNumber, dr["OrdersNumber"]);
                }
                else
                {
                    sql = string.Format(@"
insert into T_AccountsReceivable_Detail (OrdersNumber,CreateTime,ProductNumber,Version,CustomerProductNumber,Price ,qty ,SumPrice,
LeadTime  ,RowNumber )
select dnd.OrdersNumber ,'{0}',dnd.ProductNumber ,dnd.Version ,dnd.CustomerProductNumber,tod.UnitPrice,dnd.ConformenceQty
,isnull(tod.UnitPrice ,0)*ISNULL (dnd.ConformenceQty ,0),dnd.LeadTime ,dnd.RowNumber 
 from DeliveryNoteDetailed  dnd inner join MachineOderDetail tod on dnd.OrdersNumber =tod.OdersNumber 
 and dnd.RowNumber =tod.RowNumber 
 where dnd.DeliveryNumber='{1}' and dnd.OrdersNumber ='{2}'", timeNow, deleiyNumber, dr["OrdersNumber"]);
                }
                sqls.Add(sql);
            }
            return sqls;
        }

        public static List<string> GetSHYSForAll(string numbers)
        {
            string id = string.Empty;
            List<string> sqls = new List<string>();
            {
                foreach (string str in numbers.Split(','))
                {
                    id = str.TrimStart('\'').TrimEnd('\'');
                    sqls.AddRange(GetSHYS(id));
                }
            }
            return sqls;
        }

      
    }
}
