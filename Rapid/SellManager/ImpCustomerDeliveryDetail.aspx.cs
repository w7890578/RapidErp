using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Model;

namespace Rapid.SellManager
{
    public partial class ImpCustomerDeliveryDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            string error = string.Empty;
            DateTime newdate = DateTime.Now.AddDays(+1);
            DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
            if (ds == null)
            {
                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
                return;
            }
            List<string> list = new List<string>();
            List<string> li = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                CustomerDeliveryDetail customerdeliverydetail = new CustomerDeliveryDetail();
                customerdeliverydetail.Number = dr["物料"].ToString();
                customerdeliverydetail.Description = dr["描述"].ToString();
                customerdeliverydetail.AdvanceQty = Convert.ToInt32(dr["需提前交货数量"] == null ? 0 : dr["需提前交货数量"]);
                sql = string.Format(@"  select * from ProductCustomerProperty where CustomerProductNumber='{0}'", dr["物料"]);
                if (SqlHelper.GetTable(sql).Rows.Count > 0)
                {
                    sql = string.Format(@" insert into CustomerDeliveryDetail(ImportTime,CustomerId,StockQty,Number,Description,AdvanceQty,WipQty,
                    IsMeetDelivery,OrderContrast,ReplyDate,NonDeliveryQty,Remark)
                    select GETDATE() as 导入时间,'ddddddd' as 客户编号,
                   ISNULL(sum(ps.StockQty),0) as 产品库存数量,'{0}' as 物料, '{2}' as 描述,{1} as 需提前交货数量,10,
                   case when ISNULL(sum(ps.StockQty),0)+10-ISNULL({1},0)>=0 then ISNULL(sum(ps.StockQty),0)+10-ISNULL({1},0) 
                   else -1 end as 是否满足交货,
                   case when ISNULL(sum(t.加工未交订单数量),0)-ISNULL({1},0)>=0 then ISNULL(sum(t.加工未交订单数量),0)-ISNULL({1},0) else -1 end as 订单对比结果,
                   case when ISNULL(sum(ps.StockQty),0)+10-ISNULL({1},0)>=0 then '{3}' else ' ' end as 供应商回复交货日期 
                   ,ISNULL(sum(t.加工未交订单数量),0) as 未交订单数量,' ' as 备注
                   from 
                   (select so.OdersNumber as 销售订单,sum(mod.Qty) as 加工未交订单数量,
                   sum(tod.Quantity) as 贸易未交订单数量,mod.ProductNumber as 产品编号,mod.Version as 版本,tod.ProductNumber as 原材料编号
                   from SaleOder so inner join MachineOderDetail mod on so.OdersNumber=mod.OdersNumber 
                   inner join TradingOrderDetail tod on so.OdersNumber=tod.OdersNumber 
                   where so.CheckTime!='' and so.OrderStatus='未完成'
                   group by so.OdersNumber,mod.ProductNumber,mod.Version,tod.ProductNumber) t 
                   right join (select ProductNumber,Version  from ProductCustomerProperty where CustomerProductNumber='{0}') 
                   pcpp on t.产品编号=pcpp.ProductNumber and t.版本=pcpp.Version
                   left join ProductStock ps on t.产品编号=ps.ProductNumber and t.版本=ps.Version", dr["物料"], dr["需提前交货数量"], dr["描述"], newdate);
                    list.Add(sql);


                }
                sql = string.Format(@" select * from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", dr["物料"]);
                 if (SqlHelper.GetTable(sql).Rows.Count > 0)
                {
                    sql = string.Format(@" insert into CustomerDeliveryDetail(ImportTime,CustomerId,StockQty,Number,Description,AdvanceQty,WipQty,
                   IsMeetDelivery,OrderContrast,ReplyDate,NonDeliveryQty,Remark)
                   select GETDATE() as 导入时间,'ddddddd' as 客户编号,
                   isnull(SUM(ms.StockQty),0) as 原材料库存数量,'{0}' as 物料,'{1}' as 描述,{2} as 需提前交货数量,10 as 在制品数量,
                   case when ISNULL(sum(ms.StockQty),0)+10-ISNULL({2},0)>=0 then ISNULL(sum(ms.StockQty),0)+10-ISNULL({2},0) 
                   else -1 end as 是否满足交货,
                   case when ISNULL(sum(t.贸易未交订单数量),0)-ISNULL({2},0)>=0 then ISNULL(sum(t.贸易未交订单数量),0)-ISNULL({2},0) else -1 end as 订单对比结果,
                   case when ISNULL(sum(ms.StockQty),0)+10-ISNULL({2},0)>=0 then '{3}' else ' ' end as 供应商回复交货日期 
                   ,ISNULL(sum(t.贸易未交订单数量),0) as 未交订单数量,' ' as 备注
                   FROM 
                   (select so.OdersNumber as 销售订单,sum(mod.Qty) as 加工未交订单数量,
                   sum(tod.Quantity) as 贸易未交订单数量,mod.ProductNumber as 产品编号,mod.Version as 版本,tod.ProductNumber as 原材料编号
                   from SaleOder so inner join MachineOderDetail mod on so.OdersNumber=mod.OdersNumber 
                   inner join TradingOrderDetail tod on so.OdersNumber=tod.OdersNumber 
                   where so.CheckTime!='' and so.OrderStatus='未完成'
                   group by so.OdersNumber,mod.ProductNumber,mod.Version,tod.ProductNumber) t 
                   right join (select MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}') mcpm
                    on t.原材料编号=mcpm.MaterialNumber
                   left join MaterialStock ms on ms.MaterialNumber=t.原材料编号", dr["物料"], dr["描述"], dr["需提前交货数量"], newdate);
                    li.Add(sql);
                }


            }
            if (SqlHelper.BatchExecuteSql(list, ref error))
            {
                lbMsg.Text = "导入成功！";
            }
            else
            {
                lbMsg.Text = error;
                return;
            }

            if (SqlHelper.BatchExecuteSql(li, ref error))
            {
                lbMsg.Text = "导入成功！";

            }
            else
            {
                lbMsg.Text = error;
                return;
            }

        }


    }
}
