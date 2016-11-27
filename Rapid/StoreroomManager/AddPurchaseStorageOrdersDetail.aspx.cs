using BLL;
using DAL;
using Rapid.ToolCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.StoreroomManager
{
    public partial class AddPurchaseStorageOrdersDetail : System.Web.UI.Page
    {
        public static string documentName = string.Empty;
        public static string titleName = string.Empty;

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //Search();
            string error = string.Empty;
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            string ordersNumber = ToolManager.GetValueForListBox(drpOrdersNumber);
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));

            string date = txtDate.Text;
            string productNumber = txtProductNumber.Text.Trim();
            string customerMaterialNumber = txtCustomerMaterialNumber.Text.Trim();
            string customername = txtCustomerName.Text.Trim();
            string cargotype = txtCargoType.Text.Trim();
            //            string sql = string.Format(@"
            //select count(*) from MaterialWarehouseLogDetail where WarehouseNumber='{0}' and DocumentNumber ='{1}'
            //", warehouseNumber, ordersNumber);
            //            if (!SqlHelper.GetScalar(sql).Equals("0"))
            //            {
            //                lbSubmit.Text = "该订单已进行过出入库操作";
            //                return;
            //            }
            string sql = string.Empty;
            switch (type)
            {
                case "采购入库":
                    sql = string.Format(@"
    insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber ,SupplierMaterialNumber
,Qty ,UnitPrice,LeadTime,RowNumber,Remark)
select '{0}',OrdersNumber ,MaterialNumber ,SupplierMaterialNumber,NonDeliveryQty ,UnitPrice,LeadTime,'0',Remark  from CertificateOrdersDetail
where OrdersNumber  in ({1}) and NonDeliveryQty>0
", warehouseNumber, ordersNumber); //审核产生应付
                    break;

                case "采购退料出库":
                    sql = string.Format(@"
    insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber ,SupplierMaterialNumber
,Qty ,UnitPrice,LeadTime,RowNumber)
select '{0}',OrdersNumber ,MaterialNumber ,SupplierMaterialNumber,DeliveryQty ,UnitPrice ,LeadTime,RowNumber from CertificateOrdersDetail
where OrdersNumber  in ({1})
", warehouseNumber, ordersNumber);
                    break;
                //                case "盘盈入库":
                //                    sql = string.Format(@"
                //   insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber , Qty,LeadTime,RowNumber  )
                //select '{0}',InventoryNumber,MaterialNumber , ProfitAndLossQty,'0','0' from StockInventoryLogDetail where InventoryNumber in ({1})
                //", warehouseNumber, ordersNumber);
                //                    break;
                //                case "盘亏出库":
                //                    sql = string.Format(@"
                //   insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber , Qty,LeadTime,RowNumber  )
                //select '{0}',InventoryNumber,MaterialNumber , ProfitAndLossQty,'0','0' from StockInventoryLogDetail where InventoryNumber in ({1})
                //", warehouseNumber, ordersNumber); break;
                case "销售出库（贸易）":
                    sql = string.Format(@" insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber,CustomerMaterialNumber
,Qty ,LeadTime,RowNumber,Remark)
select '{0}',tod.OdersNumber,tod.ProductNumber,tod.CustomerMaterialNumber,tod.NonDeliveryQty ,tod.Delivery,tod.RowNumber ,tod.Remark from TradingOrderDetail tod
inner join [V_MYSaleOder] MY     on MY.OdersNumber =tod.OdersNumber
inner join MarerialInfoTable mi on tod.ProductNumber=mi.MaterialNumber
where tod.OdersNumber  in ({1})
", warehouseNumber, ordersNumber);
                    if (!string.IsNullOrEmpty(date))
                    {
                        sql += string.Format(" and tod.Delivery <='{0}' ", date);
                    }
                    if (!string.IsNullOrEmpty(productNumber))
                    {
                        sql += string.Format("  and tod.ProductNumber ='{0}' ", productNumber);
                    }
                    if (!string.IsNullOrEmpty(customerMaterialNumber))
                    {
                        sql += string.Format("  and tod.CustomerMaterialNumber='{0}' ", customerMaterialNumber);
                    }
                    if (!string.IsNullOrEmpty(customername))
                    {
                        sql += string.Format(" and my.CustomerId='{0}'", customername);
                    }
                    if (!string.IsNullOrEmpty(cargotype))
                    {
                        sql += string.Format(" and mi.CargoType='{0}'", cargotype);
                    }
                    break; //审核产生送货单

                case "维修出库": sql = string.Format(@" insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber
,Qty,LeadTime,RowNumber,CustomerMaterialNumber )
select '{0}',OdersNumber,ProductNumber,NonDeliveryQty,Delivery,RowNumber,CustomerMaterialNumber from TradingOrderDetail
where OdersNumber  in ({1})
", warehouseNumber, ordersNumber); break;//审核产生送货单
                case "样品出库":
                    sql = string.Format(@" insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber
,Qty ,LeadTime,RowNumber,CustomerMaterialNumber)
select '{0}',OdersNumber,ProductNumber,NonDeliveryQty,Delivery,RowNumber,CustomerMaterialNumber from TradingOrderDetail
where OdersNumber  in ({1})
", warehouseNumber, ordersNumber); break;//审核产生送货单
                case "包装出库":
                    sql = string.Format(@" insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber,CustomerMaterialNumber
,Qty,LeadTime,RowNumber )
select '{0}',OdersNumber,ProductNumber,CustomerMaterialNumber,NonDeliveryQty,Delivery,RowNumber from TradingOrderDetail
where OdersNumber  in ({1})
", warehouseNumber, ordersNumber); break;

                case "生产退料入库":
                    documentName = "开工单号";
                    break;

                case "生产出库":
                    documentName = "开工单号";
                    break;
            }
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "添加成功！" : "添加失败！原因是:" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加" + titleName + "信息" + warehouseNumber, "增加成功");
                ToolCode.Tool.ResetControl(this.Controls);
                Response.Write(@" <script >
                                    window.close();
                                </script>");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加" + titleName + "信息" + warehouseNumber, "增加失败！原因" + error);
                return;
            }
        }

        //订单名称
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }

        private List<string> GetExistDocumentNumbers(string warehouseNumber)
        {
            List<string> result = new List<string>();
            string sql = string.Format(@"select distinct(DocumentNumber) DocumentNumber from MaterialWarehouseLogDetail where WarehouseNumber='{0}'", warehouseNumber);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var documentNumber = dr["DocumentNumber"] == null ? "" : dr["DocumentNumber"].ToString();
                    result.Add(documentNumber);
                }
            }
            return result;
        }

        private void LoadPage()
        {
            if (!ToolManager.CheckQueryString("WarehouseNumber"))
            {
                Response.Write("未知出入库单");
                Response.End();
                return;
            }
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            lbWarehouseNumber.Text = warehouseNumber;
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            titleName = type;
            var existNumbers = GetExistDocumentNumbers(warehouseNumber);
            switch (type)
            {
                case "采购入库":
                    documentName = "采购订单";
                    ControlBindManager.BindListBox("select OrdersNumber,OrdersNumber+' ('+HTNumber+')' as text  from CertificateOrders where OrderStatus ='未完成' and ISNULL (CheckTime ,'') !='' order by CreateTime asc", drpOrdersNumber, "OrdersNumber", "text", existNumbers);
                    trSaleOrder.Visible = false;
                    break;

                case "采购退料出库":
                    documentName = "采购订单";
                    ControlBindManager.BindListBox("select OrdersNumber  from CertificateOrders where OrderStatus ='未完成' and ISNULL (CheckTime ,'') !='' order by CreateTime desc", drpOrdersNumber, "OrdersNumber", "OrdersNumber", existNumbers);
                    break;

                case "盘盈入库":
                    documentName = "盘点编号";
                    ControlBindManager.BindListBox(@"select InventoryNumber
from StockInventoryLog where ISNULL ( AuditeTime ,'')!=''", drpOrdersNumber, "InventoryNumber", "InventoryNumber", existNumbers);
                    break;

                case "盘亏出库":
                    documentName = "盘点编号";
                    ControlBindManager.BindListBox(@"select InventoryNumber
from StockInventoryLog where ISNULL ( AuditeTime ,'')!=''", drpOrdersNumber, "InventoryNumber", "InventoryNumber", existNumbers);
                    break;

                case "销售出库（贸易）":
                    documentName = "销售订单";
                    ControlBindManager.BindListBox(@"select OdersNumber,OdersNumber+' ('+customerordernumber+')' customerordernumber from SaleOder where ProductType ='贸易'
and OrderStatus ='未完成'
and ISNULL (CheckTime ,'') !=''
and OdersType  in ('加急订单','正常订单')", drpOrdersNumber, "OdersNumber", "customerordernumber", existNumbers);
                    break;

                case "包装出库":
                    documentName = "销售订单";
                    ControlBindManager.BindListBox(@"select OdersNumber from SaleOder where ProductType ='贸易'
and OrderStatus ='未完成'
and ISNULL (CheckTime ,'') !=''
and OdersType  in ('加急订单','正常订单')", drpOrdersNumber, "OdersNumber", "OdersNumber", existNumbers);
                    break;

                case "维修出库": documentName = "维修订单";
                    ControlBindManager.BindListBox(@"select OdersNumber from SaleOder where ProductType ='贸易'
and OrderStatus ='未完成'
and ISNULL (CheckTime ,'') !=''
and OdersType  in ('维修订单')", drpOrdersNumber, "OdersNumber", "OdersNumber", existNumbers);
                    break;

                case "样品出库":
                    documentName = "样品订单";
                    ControlBindManager.BindListBox(@"select OdersNumber from SaleOder where ProductType ='贸易'
and OrderStatus ='未完成'
and ISNULL (CheckTime ,'') !=''
and OdersType  in ('样品订单')", drpOrdersNumber, "OdersNumber", "OdersNumber", existNumbers);
                    break;

                case "生产退料入库":
                    documentName = "开工单号";
                    break;

                case "生产出库":
                    documentName = "开工单号";
                    break;
            }
        }

        private void Search()
        {
            string date = txtDate.Text;
            string productNumber = txtProductNumber.Text.Trim();
            string customerMaterialNumber = txtCustomerMaterialNumber.Text.Trim();
            string customername = txtCustomerName.Text.Trim();
            string cargotype = txtCargoType.Text.Trim();
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            string sql = string.Empty;

            sql = string.Format(@"
select  distinct MY.OdersNumber,MY.OdersNumber+'___'+'('+MY.customerordernumber+')' as OdersNumber_Name,
MY.CreateTime from (select * from SaleOder where ProductType  ='贸易' and ISNULL (CheckTime ,'')!=''
and OrderStatus ='未完成') MY
left join  TradingOrderDetail tod on MY.OdersNumber =tod.OdersNumber
inner join MarerialInfoTable mi on tod.ProductNumber=mi.MaterialNumber  where 1=1");
            if (!string.IsNullOrEmpty(date))
            {
                sql += string.Format(" and tod.Delivery <='{0}' ", date);
            }
            if (!string.IsNullOrEmpty(productNumber))
            {
                sql += string.Format("  and tod.ProductNumber ='{0}' ", productNumber);
            }
            if (!string.IsNullOrEmpty(customerMaterialNumber))
            {
                //sql += string.Format("  and tod.CustomerMaterialNumber='{0}' ", customerMaterialNumber);
                sql += string.Format(@"   and my.OdersNumber in (
select distinct OdersNumber  from TradingOrderDetail where CustomerMaterialNumber='{0}' and Status='未完成')"
                    , customerMaterialNumber);
            }
            if (!string.IsNullOrEmpty(customername))
            {
                sql += string.Format(" and my.CustomerId='{0}'", customername);
            }
            if (!string.IsNullOrEmpty(cargotype))
            {
                sql += string.Format(" and mi.CargoType='{0}'", cargotype);
            }
            sql += " order by my.CreateTime desc ";
            ControlBindManager.BindListBox(sql, drpOrdersNumber, "OdersNumber", "OdersNumber_Name");
            foreach (ListItem item in drpOrdersNumber.Items)
            {
                item.Selected = true;
            }
        }
    }
}