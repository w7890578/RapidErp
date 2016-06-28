using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class ToolAddProductWarehouseLogDetail : System.Web.UI.Page
    {
        public static string titleName = string.Empty;
        public static string documentName = string.Empty; //订单名称
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                trSaleOrder.Visible = false;
                LoadPage();
            }
        }
        private void LoadPage()
        {
            if (!ToolManager.CheckQueryString("WarehouseNumber"))
            {
                Response.Write("未知出入库单");
                Response.End();
                return;
            }
            lbWarehouseNumber.Text = ToolManager.GetQueryString("WarehouseNumber");
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            titleName = type;

            //退货入库：销售订单编号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、退货原因、备注。
            //维修入库：维修订单号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            //维修出库：维修订单号、产成品编号、客户产品编号、版本、名称、描述、数量、仓位、客户名称、备注。
            //样品出库：销售订单编号、产成品编号、客户产品编号、版本、名称、描述、数量、仓位、客户名称、备注。
            //销售出库：销售订单编号、产成品编号、客户产品编号、客户名称、版本、名称、描述、数量、仓位、备注。
            //包装入库：销售订单编号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            //盘盈入库：盘点编号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            //盘亏出库：盘点编号、产成品编号、版本、名称、描述、数量、仓位、备注。
            //样品入库：开工单号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            //生产入库：开工单号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            //加急订单、正常订单、维修订单、临时订单、样品订单
            switch (type)
            {
                case "退货入库":
                    documentName = "销售订单";
                    ControlBindManager.BindListBox("select vj.OdersNumber  from V_JGSaleOder vj where vj.OdersType in('正常订单','加急订单') order by vj.CreateTime desc", drpOrdersNumber, "OdersNumber", "OdersNumber");
                    break;
                case "维修入库":
                    documentName = "维修订单";
                    ControlBindManager.BindListBox("select vj.OdersNumber  from V_JGSaleOder vj where vj.OdersType in('维修订单') order by vj.CreateTime desc", drpOrdersNumber, "OdersNumber", "OdersNumber");
                    break;
                case "维修出库":
                    documentName = "维修订单";
                    ControlBindManager.BindListBox("select vj.OdersNumber  from V_JGSaleOder vj where vj.OdersType in('维修订单') order by vj.CreateTime desc", drpOrdersNumber, "OdersNumber", "OdersNumber");
                    break;
                case "样品出库":
                    documentName = "样品订单";
                    ControlBindManager.BindListBox("select vj.OdersNumber  from V_JGSaleOder vj where vj.OdersType in('样品订单') order by vj.CreateTime desc", drpOrdersNumber, "OdersNumber", "OdersNumber");
                    break;
                case "销售出库":
                    documentName = "销售订单";
                    trSaleOrder.Visible = true;
                    ControlBindManager.BindListBox(@"select vj.OdersNumber, vj.OdersNumber+' ('+vj.customerorderNumber+')' as customerorderNumber  from V_JGSaleOder vj where 
vj.OdersType in('正常订单','加急订单','维修订单') and vj.OrderStatus!='已完成' order by vj.CreateTime desc", drpOrdersNumber, "OdersNumber", "customerorderNumber");
                    break;
                case "包装销售出库":
                    documentName = "销售订单";
                    trSaleOrder.Visible = true;
                    ControlBindManager.BindListBox("select vj.OdersNumber, vj.OdersNumber+' ('+vj.customerorderNumber+')' as customerorderNumber  from V_JGSaleOder vj where vj.OdersType in('包装生产订单') order by vj.CreateTime desc", drpOrdersNumber, "OdersNumber", "customerorderNumber");
                    break;
                case "包装入库":
                    documentName = "销售订单";
                    ControlBindManager.BindListBox("select vj.OdersNumber  from V_JGSaleOder vj where vj.OdersType in('正常订单','加急订单') order by vj.CreateTime desc", drpOrdersNumber, "OdersNumber", "OdersNumber");
                    break;
                case "盘盈入库":
                    documentName = "盘点编号";
                    ControlBindManager.BindListBox(@"select InventoryNumber from StockInventoryLog where  isnull(AuditeTime,'')!='' ", drpOrdersNumber, "InventoryNumber", "InventoryNumber");
                    break;
                case "盘亏出库":
                    documentName = "样品订单";
                    ControlBindManager.BindListBox(@"select InventoryNumber from StockInventoryLog where  isnull(AuditeTime,'')!='' ", drpOrdersNumber, "InventoryNumber", "InventoryNumber");
                    break;
                case "样品入库":
                    documentName = "开工单号";
                    ControlBindManager.BindListBox(@"  select distinct ppd.PlanNumber  from ProductPlanDetail ppd inner join SaleOder so on ppd.OrdersNumber =so.OdersNumber 
  where so.OdersType in ('样品订单')", drpOrdersNumber, "PlanNumber", "PlanNumber");
                    break;
                case "生产入库":
                    documentName = "开工单号";
                    ControlBindManager.BindListBox(@"  select distinct ppd.PlanNumber  from ProductPlanDetail ppd inner join SaleOder so on ppd.OrdersNumber =so.OdersNumber 
  where so.OdersType in ('加急订单','正常订单','临时订单')", drpOrdersNumber, "PlanNumber", "PlanNumber");
                    break;
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            List<string> sqls = new List<string>();
            string error = string.Empty;
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            string ordersNumber = ToolManager.GetValueForListBox(drpOrdersNumber);
            if (string.IsNullOrEmpty(ordersNumber))
            {
                lbSubmit.Text = "请选择订单！";
                return;
            }
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            string sql = string.Empty;
            if (type.Equals("销售出库") || type.Equals("包装销售出库"))
            {
                //如果是这两个不判断重复
            }
            else
            {
                sql = string.Format(@"
select count(*) from ProductWarehouseLogDetail where WarehouseNumber='{0}' and DocumentNumber in ({1})
", warehouseNumber, ordersNumber);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    lbSubmit.Text = "该订单已进行过出入库操作";
                    return;
                }
            }
            //退货入库：销售订单编号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、退货原因、备注。
            //维修入库：维修订单号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            //维修出库：维修订单号、产成品编号、客户产品编号、版本、名称、描述、数量、仓位、客户名称、备注。
            //样品出库：销售订单编号、产成品编号、客户产品编号、版本、名称、描述、数量、仓位、客户名称、备注。
            //销售出库：销售订单编号、产成品编号、客户产品编号、客户名称、版本、名称、描述、数量、仓位、备注。
            //包装入库：销售订单编号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            //盘盈入库：盘点编号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            //盘亏出库：盘点编号、产成品编号、版本、名称、描述、数量、仓位、备注。
            //样品入库：开工单号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            //生产入库：开工单号、产成品编号、版本、客户产品编号、名称、描述、数量、仓位、备注。
            switch (type)
            {
                case "退货入库":
                    sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,RowNumber)
select '{0}',OdersNumber ,ProductNumber ,Version  ,CustomerProductNumber,Qty ,0,'0'   from MachineOderDetail
where OdersNumber  in ({1})
", warehouseNumber, ordersNumber);
                    sqls.Add(sql);
                    break;
                case "维修入库":
                    sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,RowNumber)
select '{0}',OdersNumber ,ProductNumber ,Version  ,CustomerProductNumber,NonDeliveryQty ,0 ,'0'  from MachineOderDetail
where OdersNumber  in ({1}) and NonDeliveryQty>0
", warehouseNumber, ordersNumber); sqls.Add(sql);
                    break;
                case "维修出库": //审核产生送货单
                    sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,LeadTime,RowNumber)
select '{0}',OdersNumber ,ProductNumber ,Version  ,CustomerProductNumber,NonDeliveryQty ,0,LeadTime ,RowNumber  from MachineOderDetail
where OdersNumber in ({1})  and NonDeliveryQty>0
", warehouseNumber, ordersNumber); sqls.Add(sql);
                    break;
                case "样品出库"://审核产生送货单
                    sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,LeadTime,RowNumber)
select '{0}',OdersNumber ,ProductNumber ,Version  ,CustomerProductNumber,NonDeliveryQty ,0,LeadTime,RowNumber   from MachineOderDetail
where OdersNumber in ({1}) and NonDeliveryQty>0
", warehouseNumber, ordersNumber); sqls.Add(sql);
                    break;
                case "销售出库"://审核产生送货单
                    sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,LeadTime,RowNumber,Remark)
select '{0}',OdersNumber ,ProductNumber ,Version  ,CustomerProductNumber,NonDeliveryQty ,0 ,LeadTime,RowNumber ,Remark  from MachineOderDetail modt
where modt.OdersNumber in ({1}) and NonDeliveryQty>0
", warehouseNumber, ordersNumber);
                    string date = txtDate.Text;
                    string productNumber = txtProductNumber.Text;
                    string customerProductNumber = txtCustomerProductNumber.Text;
                    if (!string.IsNullOrEmpty(date))
                    {
                        sql += string.Format(" and modt.LeadTime <='{0}' ", date);
                    }
                    if (!string.IsNullOrEmpty(productNumber))
                    {
                        sql += string.Format("  and modt.ProductNumber ='{0}' ", productNumber);
                    }
                    if (!string.IsNullOrEmpty(customerProductNumber))
                    {
                        sql += string.Format("  and modt.customerProductNumber='{0}' ", customerProductNumber);
                    }
                    sqls.Add(sql);
                    break;
                case "包装销售出库"://审核产生送货单
                    sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,LeadTime,RowNumber)
select '{0}',OdersNumber ,ProductNumber ,Version  ,CustomerProductNumber,NonDeliveryQty ,0 ,LeadTime,RowNumber   from MachineOderDetail modt
where modt.OdersNumber in ({1}) and NonDeliveryQty>0
", warehouseNumber, ordersNumber);
                    date = txtDate.Text;
                    productNumber = txtProductNumber.Text;
                    customerProductNumber = txtCustomerProductNumber.Text;
                    if (!string.IsNullOrEmpty(date))
                    {
                        sql += string.Format(" and modt.LeadTime <='{0}' ", date);
                    }
                    if (!string.IsNullOrEmpty(productNumber))
                    {
                        sql += string.Format("  and modt.ProductNumber ='{0}' ", productNumber);
                    }
                    if (!string.IsNullOrEmpty(customerProductNumber))
                    {
                        sql += string.Format("  and modt.customerProductNumber='{0}' ", customerProductNumber);
                    }
                    sqls.Add(sql);
                    break;
                case "包装入库":
                    sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,RowNumber)
select '{0}',OdersNumber ,ProductNumber ,Version  ,CustomerProductNumber,Qty ,0,'0'   from MachineOderDetail
where OdersNumber in ({1})
", warehouseNumber, ordersNumber); sqls.Add(sql);
                    break;
                case "盘盈入库":
                    sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,RowNumber)
select '{0}',InventoryNumber,MaterialNumber ,Version ,'0',ProfitAndLossQty,0,'0' from StockInventoryLogDetail
where InventoryNumber in ({1})", warehouseNumber, ordersNumber); sqls.Add(sql);
                    break;
                case "盘亏出库":
                    sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,RowNumber)
select '{0}',InventoryNumber,MaterialNumber ,Version ,'0',ProfitAndLossQty,0,'0' from StockInventoryLogDetail
where InventoryNumber in ({1})", warehouseNumber, ordersNumber); sqls.Add(sql);
                    break;
                case "样品入库":
                    sql = string.Format(@" insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version ,RowNumber ,OrdersNumber,CustomerProductNumber ,Qty ,LeadTime )
select '{0}',PlanNumber ,ProductNumber ,Version ,RowNumber ,OrdersNumber ,CustomerProductNumber,Qty,LeadTime  
from ProductPlanDetail where PlanNumber in({1})
  ", warehouseNumber, ordersNumber); sqls.Add(sql);
                    break;
                case "生产入库":
                    sql = string.Format(@" insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version ,RowNumber ,OrdersNumber,CustomerProductNumber ,Qty ,LeadTime )
select '{0}',PlanNumber ,ProductNumber ,Version ,RowNumber ,OrdersNumber ,CustomerProductNumber,Qty,LeadTime  
from ProductPlanDetail where PlanNumber in({1})
  ", warehouseNumber, ordersNumber);
                    sqls.Add(sql);
                    break;
            }

            bool restult = SqlHelper.BatchExecuteSql(sqls, ref error);
            if (restult)
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加" + titleName + "信息", "增加成功");
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加" + titleName + "信息", "增加失败！原因" + error);
                lbSubmit.Text = "增加失败！原因" + error;
                return;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string date = txtDate.Text;
            string productNumber = txtProductNumber.Text;
            string customerProductNumber = txtCustomerProductNumber.Text;
            string customername = txtCustomerName.Text.Trim();
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            string sql = string.Empty;
            if (type.Equals("包装销售出库"))
            {
                sql = string.Format(@"select distinct vj.OdersNumber, vj.OdersNumber +' ('+vj.CustomerOrderNumber+')' as oderName  
from (select * from SaleOder where ProductType  ='加工' and ISNULL (CheckTime ,'')!=''
and OrderStatus ='未完成') vj left join  MachineOderDetail modt on vj.OdersNumber =modt.OdersNumber 
where vj.OdersType in('包装生产订单')
 ");
            }
            else
            {
                sql = string.Format(@"select distinct vj.OdersNumber, vj.OdersNumber +' ('+vj.CustomerOrderNumber+')' as oderName  from (select * from SaleOder where ProductType  ='加工' and ISNULL (CheckTime ,'')!=''
and OrderStatus ='未完成') vj left join  MachineOderDetail modt on vj.OdersNumber =modt.OdersNumber 
where vj.OdersType in('正常订单','加急订单') ");
            }

            if (!string.IsNullOrEmpty(date))
            {
                sql += string.Format(" and modt.LeadTime <='{0}' ", date);
            }
            if (!string.IsNullOrEmpty(productNumber))
            {
                sql += string.Format("  and modt.ProductNumber ='{0}' ", productNumber);
            }
            if (!string.IsNullOrEmpty(customerProductNumber))
            {
                //sql += string.Format("  and modt.customerProductNumber='{0}' ", customerProductNumber);
                sql += string.Format(@"and vj.OdersNumber in 
(select distinct so.OdersNumber from SaleOder so inner join MachineOderDetail modt 
on so.OdersNumber=modt.OdersNumber
where so.ProductType  ='加工' and ISNULL (so.CheckTime ,'')!=''
and so.OrderStatus ='未完成'
and modt.CustomerProductNumber = '{0}'
and modt.Status ='未完成'
)", customerProductNumber);

            }
            if (!string.IsNullOrEmpty(customername))
            {
                sql += string.Format("  and vj.CustomerId='{0}' ", customername);
            }
            sql += " order by vj.OdersNumber desc ";
            ControlBindManager.BindListBox(sql, drpOrdersNumber, "OdersNumber", "oderName");
        }
    }
}