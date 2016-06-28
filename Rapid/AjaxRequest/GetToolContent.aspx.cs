using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.AjaxRequest
{
    public partial class GetToolContent1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadPage();
        }
        private void LoadPage()
        {
            string result = string.Empty;
            switch (ToolManager.GetQueryString("contentType"))
            {
                //测试服务器连接
                case "CheckNetWork": result = ToolManager.CmdPing() == true ? "正常" : "断开";
                    break;
                //收款方式
                case "ReceivablesMode": result = ControlBindManager.GetReceivablesMode();
                    break;
              
                //产成品编号
                case "ProductNumber": result = ControlBindManager.GetProductNumber();
                    break;
                // 原材料编号
                case "MaterialNumber": result = ControlBindManager.GetMaterialNumber();
                    break;
                //开工单号
                case "ProductionOrderNumber": result = ControlBindManager.GetProductionOrderNumber();
                    break;
                //客户产成品编号
                case "CustomerProductNumber": result = ControlBindManager.GetCustomerProductNumber();
                    break;
                //产成品编号
                case "ProductId": result = ControlBindManager.GetProductId();
                    break;
                //类别（产品基本信息表）
                case "ProductType": result = ControlBindManager.GetProductType();
                    break;
                ////编号（员工考试成绩上报表）
                //case "Id": result = ControlBindManager.GetId();
                //    break;
                //年度（员工考试成绩上报表）
                case "Year": result = ControlBindManager.GetYear();
                    break;
                //月份（员工考试成绩上报表）
                case "Month": result = ControlBindManager.GetMonth();
                    break;
                //姓名（员工考试成绩上报表）
                case "Name": result = ControlBindManager.GetName();
                    break;
              
                //原材料种类(原材料信息表)
                case "MaterialKind": result = ControlBindManager.GetMaterialKind();
                    break;
               //原材料类别(原材料信息表)
                case "MaterialType": result = ControlBindManager.GetMarerialType();
                    break;
                //仓库名称（仓库信息）
                case "WarehouseName": result = WarehouseInfoManager.GetWarehouseName();
                    break;
                //位置（仓库信息）
                case "Position": result = WarehouseInfoManager.GetPosition();
                    break;
                //仓库名称（仓库信息）
                case "WarehouseType": result = WarehouseInfoManager.GetType();
                    break;
                ////原材料描述（供需平衡表）
                //case "Materialdescription": result = ControlBindManager.GetMaterialDescription();
                //    break;
               
                //送货单号（送货单主表）
                case "DeliveryNumber": result = ControlBindManager.GetDeliveryNumber();
                    break;
                //送货人
                case "DeliveryPerson": result = ControlBindManager.GetDeliveryPerson();
                    break;
                //采购订单编号 
                case "OrdersNumber": result = ControlBindManager.GetOrdersNumber();
                    break;
                //包编码（包信息列表）
                case "PackageNumber": result = ControlBindManager.GetPackageNumber();
                    break;
                //包名称（包信息列表）
                case "PackageName": result = ControlBindManager.GetPackageName();
                    break;
                //考核标准名称（考核标准维护表）
                case "StandardName": result = PerformanceReviewStandardManager.GetStandardName();
                    break;
                //考核项目（考核标准维护表）
                case "PerformanceReviewItem": result = PerformanceReviewStandardManager.GetPerformanceReviewItem();
                    break;
                //序号（考核标准维护表）
                case "RowNumber": result = PerformanceReviewStandardManager.GetRowNumber();
                    break;
                //年度(员工绩效上报表)
                case "PerformanceReviewLogYear": result = PerformanceReviewLogManager.GetYear();
                    break;
                //月份(员工绩效上报表)
                case "PerformanceReviewLogMonth": result = PerformanceReviewLogManager.GetMonth();
                    break;
                //考核项目(员工绩效上报表)
                case "PerformanceReviewLogPerformanceReviewItem": result = PerformanceReviewLogManager.GetPerformanceReviewItem();
                    break;
                //姓名(员工绩效上报表)
                case "PerformanceReviewLogName": result = PerformanceReviewLogManager.GetName();
                    break;
                //序号(员工绩效上报表)
                case "PerformanceReviewLogRowNumber": result = PerformanceReviewLogManager.GetRowNumber();
                    break;
                //出入库编号（废品出入库主表）
                case "ScarpWarehouseLogWarehouseNumber": result = ScarpWarehouseLogManager.GetWarehouseNumber();
                    break;
                //仓库名称（废品出入库主表）
                case "ScarpWarehouseLogWarehouseName": result = ScarpWarehouseLogManager.GetWarehouseName();
                    break;
                //变动方向（废品出入库主表）
                case "ScarpWarehouseLogChangeDirection": result = ScarpWarehouseLogManager.GetChangeDirection();
                    break;

                //仓库类型（出入库类型列表）
                case "WarehouseInOutTypeWarehouseType": result = ControlBindManager.Gethousetype();
                    break;
                //变动方向（出入库类型列表）
                case "WarehouseInOutTypeChangeDeraction": result = ControlBindManager.GetChangeDirection();
                    break;
                //类型（出入库类型列表）
                case "WarehouseInOutType": result = ControlBindManager.GetInOutType();
                    break;
                //出入库编号（原材料出入库列表）
                case "MarerialWarehouseLogWarehouseNumber": result = MarerialWarehouseLogListManager.GetWarehouseNumber();
                    break;
                //仓库名称（原材料出入库列表）
                case "MarerialWarehouseLogWarehouseName": result = MarerialWarehouseLogListManager.GetWarehouseName();
                    break;
                //变动方向（原材料出入库列表）
                case "MarerialWarehouseLogChangeDirection": result = MarerialWarehouseLogListManager.GetChangeDirection();
                    break;
                //出入库类型（原材料出入库列表）
                case "MarerialWarehouseLogType": result = MarerialWarehouseLogListManager.GetType();
                    break;
                //盘点编号（库存盘点）
                case "StockInventoryLogInventoryNumber": result = ControlBindManager.GetInventoryNumber();
                    break;
                //仓库名称（库存盘点）
                case "StockInventoryLogWarehouseName": result = ControlBindManager.GetWarehouseName();
                    break;
                //盘点类型（库存盘点）
                case "StockInventoryLogInventoryType": result = ControlBindManager.GetInventoryType();
                    break;
                //出入库编号（产成品出入库列表）
                case "ProductWarehouseLogWarehouseNumber": result = ControlBindManager.GetProductWarehouseLogWarehouseNumber();
                    break;
                //仓库名称（产成品出入库列表）
                case "ProductWarehouseLogWarehouseName": result = ControlBindManager.GetProductWarehouseLogWarehouseName();
                    break;
                //变动方向（产成品出入库列表）
                case "ProductWarehouseLogChangeDirection": result = ControlBindManager.GetProductWarehouseLogChangeDirection();
                    break;
                //出入库类型（产成品出入库列表）
                case "ProductWarehouseLogType": result = ControlBindManager.GetProductWarehouseLogType();
                    break;
                //出入库编号（半成品出入库列表）
                case "HalfProductWarehouseLogWarehouseNumber": result = HalfProductWarehouseLogManager.GetHalfProductWarehouseLogWarehouseNumber();
                    break;
                //仓库名称（半成品出入库列表）
                case "HalfProductWarehouseLogWarehouseName": result = HalfProductWarehouseLogManager.GetHalfProductWarehouseLogWarehouseName();
                    break;
                //变动方向（半成品出入库列表）
                case "HalfProductWarehouseLogChangeDirection": result = HalfProductWarehouseLogManager.GetHalfProductWarehouseLogChangeDirection();
                    break;
                //订单交期（半成品入库列表）
                case "HalfLeadTime": result = HalfProductWarehouseLogManager.GetLeadTime();
                    break;
                //订单交期（半成品出库列表）
                case "HalfOutLeadtime": result = HalfProductWarehouseLogManager.GetOutLeadTime();
                    break;
                //订单号（应付账款表）
                case "PayOrdersNumber": result = ControlBindManager.GetPayOrdersNumber();
                    break;
                //供应商名称（应付账款表）
                case "PaySupplierName": result = ControlBindManager.GetPaySupplierName();
                    break;
                //订单号（应收账款表）
                case "ReceivOrdersNumber": result = ControlBindManager.GetReceivOrdersNumber();
                    break;
                //原材料编号（应收账款表）
                case "ReceivProductNumber": result = ControlBindManager.GetReceivProductNumber();
                    break;
                //客户编号（销售订单主表）
                case "SaleCustomerName": result = ControlBindManager.GetSaleOderCustomer();
                    break;
                //产品编号（不合格品上报表）
                case "RejectsProductProductNumber": result = RejectsProductListManager.GetProductNumber();
                    break;
                //版本（不合格品上报表）
                case "RejectsProductVersion": result = RejectsProductListManager.GetVersion();
                    break;
                //客户产成品编号（不合格品上报表）
                case "RejectsProductCustomerProductNumber": result = RejectsProductListManager.GetCustomerProductNumber();
                    break;
                //姓名（不合格品上报表）
                case "RejectsProductName": result = RejectsProductListManager.GetName();
                    break;
                //客户名称（不合格品上报表）
                case "RejectsProductCustomerName": result = RejectsProductListManager.GetCustomerName();
                    break;
                //销售订单号（生产进度列表）
                case "ProductProcessOdersNumber": result = ProductProcessManager.GetProductProcessOdersNumber();
                    break;
                //产成品编号（生产进度列表）
                case "ProductProcessProductNumber": result = ProductProcessManager.GetProductProcessProductNumber();
                    break;
                //版本（生产进度列表）
                case "ProductProcessVersion": result = ProductProcessManager.GetProductProcessVersion();
                    break;
                //客户产成品编号（生产进度列表）
                case "ProductProcessCustomerProductNumber": result = ProductProcessManager.GetProductProcessCustomerProductNumber();
                    break;
                //客户名称（生产进度列表）
                case "ProductProcessCustomerName": result = ProductProcessManager.GetProductProcessCustomerName();
                    break;
                //产品描述（生产进度列表）
                case "ProductProcessProductDescription": result = ProductProcessManager.GetProductProcessProductDescription();
                    break;
                //开工单号(开工单总表)
                case "PlanNumber": result = ControlBindManager.GetPlanNumber();
                    break;
                //制单人（开工单总表）
                case "Creator": result = ControlBindManager.GetCreator();
                    break;
                //班组（开工单分表）
                case "Team": result = ControlBindManager.GetTeam();
                    break;

                //采购订单编号（采购订单主表）
                case "OrdersNm": result = ControlBindManager.GetOrdersNm();
                    break;
                //付款方式（采购订单主表）
                case "PaymentMode": result = ControlBindManager.GetPaymentMode();
                    break;
                //供应商（采购订单主表）
                case "SupplierNm": result = ControlBindManager.GetSupplierN();
                    break;
                // 业务员（采购订单主表）
                case "USER_NAME": result = ControlBindManager.GetuserName();
                    break;
                //客户名称(应收账款)
                case "ReceivaCustomerId": result = ControlBindManager.GetReceivaCustomerId();
                    break;
            }
            Response.ContentType = "text/plain";
            Response.Write(result);
            Response.End();
        }
    }
}
