using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class EditPurchaseStorageOrdersDetail : System.Web.UI.Page
    {
        public static string documentName = string.Empty;
        public string type = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }

        private void LoadPage()
        {
            if (ToolManager.CheckQueryString("Guid"))
            {
                string guid = ToolManager.GetQueryString("Guid");
                //string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                //string documentNumber = Server.UrlDecode(ToolManager.GetQueryString("DocumentNumber"));
                //string materialNumber = ToolManager.GetQueryString("MaterialNumber");
                //lbWarehouseNumber.Text = warehouseNumber;
                string sql = string.Format(@"
select * from MaterialWarehouseLogDetail where Guid ='{0}'", guid);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    lbWarehouseNumber.Text = dr["WarehouseNumber"].ToString();
                    lbDocumentNumber.Text = dr["DocumentNumber"].ToString();
                    txtQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    txtRoadTransport.Text = dr["RoadTransport"] == null ? "" : dr["RoadTransport"].ToString();
                    txtCompleteQty.Text = dr["CompleteQty"] == null ? "" : dr["CompleteQty"].ToString();
                }
                type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
                switch (type)
                {
                    case "采购入库":
                        documentName = "采购订单";
                        break;
                    case "采购退料出库":
                        documentName = "采购订单";
                        break;
                    case "盘盈入库":
                        documentName = "盘点编号";
                        break;
                    case "盘亏出库":
                        documentName = "盘点编号";
                        break;
                    case "销售出库（贸易）":
                        documentName = "销售订单";
                        break;
                    case "包装出库":
                        documentName = "销售订单";
                        break;
                    case "维修出库":
                        documentName = "维修订单";
                        break;
                    case "样品出库":
                        documentName = "样品订单";
                        break;
                    case "生产退料入库":
                        documentName = "开工单号";
                        break;
                    case "生产出库":
                        documentName = "开工单号";
                        break;
                    case "样品入库":
                        trDocumentNumber.Visible = false;
                        break;
                }
                if (type.Equals("采购入库"))
                {
                    trRoadTransport.Visible = true;
                }
                else
                {
                    trRoadTransport.Visible = false;
                }
                if (type.Equals("销售出库（贸易）"))
                {
                    trCompleteQty.Visible = true;
                }
                else
                {
                    trCompleteQty.Visible = false;
                }
                if (type.Equals("辅料出库"))
                {
                    trDocumentNumber.Visible = false;
                }
                else
                {
                    trDocumentNumber.Visible = true;
                }
            }
            else
            {
                Response.Write("未知信息");
                Response.End();
                return;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            string error = string.Empty;
            //string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            //string documentNumber = Server.UrlDecode(ToolManager.GetQueryString("DocumentNumber"));
            //string materialNumber = ToolManager.GetQueryString("MaterialNumber");
            sql = string.Format(@"select DocumentNumber,MaterialNumber,WarehouseNumber,RowNumber ,Qty,LeadTime  
from MaterialWarehouseLogDetail where Guid ='{0}'", ToolManager.GetQueryString("Guid"));
            DataTable dt = SqlHelper.GetTable(sql);
            string warehouseNumber = dt.Rows[0]["WarehouseNumber"].ToString();
            string documentNumber = dt.Rows[0]["DocumentNumber"].ToString();
            string materialNumber = dt.Rows[0]["MaterialNumber"].ToString();
            string rowNumber = dt.Rows[0]["RowNumber"].ToString();
            string leadTime = dt.Rows[0]["LeadTime"].ToString();
            string qty = txtQty.Text;
            string remark = txtRemark.Text;
            string oldQty = dt.Rows[0]["Qty"].ToString();
            sql = string.Empty;
            type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            //检测数量是否超出
            switch (type)
            {
                case "采购入库":
                    documentName = "采购订单";
                    sql = string.Format(@"select COUNT(*) from CertificateOrdersDetail 
            where OrdersNumber ='{0}' and MaterialNumber ='{1}'
            and LeadTime =( 
            select LeadTime  from MaterialWarehouseLogDetail where WarehouseNumber='{2}' and DocumentNumber ='{0}' and MaterialNumber='{1}')
            and {3}-NonDeliveryQty>0", documentNumber, materialNumber, warehouseNumber, qty);
                    break;
                case "采购退料出库":
                    documentName = "采购订单";
                    sql = string.Format(@"select COUNT(*) from CertificateOrdersDetail 
            where OrdersNumber ='{0}' and MaterialNumber ='{1}'
            and LeadTime = '{4}'  and {3}-NonDeliveryQty>0", documentNumber, materialNumber, warehouseNumber, qty, leadTime);
                    break;

                case "销售出库（贸易）":
                    documentName = "销售订单";
                    sql = string.Format(@"
            select COUNT(*) from TradingOrderDetail where OdersNumber ='{0}' and ProductNumber ='{1}' and 
RowNumber='{4}'  and Delivery ='{5}'  and {3}-NonDeliveryQty>0",
                             documentNumber, materialNumber, warehouseNumber, qty, rowNumber, leadTime);
                    break;
                case "包装出库":
                    documentName = "销售订单";
                    sql = string.Format(@"
            select COUNT(*) from TradingOrderDetail where OdersNumber ='{0}' and ProductNumber ='{1}' and RowNumber='{4}'
and Delivery ='{5}' and {3}-NonDeliveryQty>0", documentNumber, materialNumber, warehouseNumber, qty, rowNumber, leadTime);
                    break;
                case "维修出库":
                    documentName = "维修订单";
                    sql = string.Format(@"
            select COUNT(*) from TradingOrderDetail where OdersNumber ='{0}' and ProductNumber ='{1}' and RowNumber='{4}' 
and Delivery ='{5}' and {3}-NonDeliveryQty>0", documentNumber, materialNumber, warehouseNumber, qty, rowNumber, leadTime);
                    break;
                case "样品出库":
                    documentName = "样品订单";
                    sql = string.Format(@"
            select COUNT(*) from TradingOrderDetail where OdersNumber ='{0}' and ProductNumber ='{1}' and RowNumber='{4}' 
and Delivery ='{5}' and {3}-NonDeliveryQty>0", documentNumber, materialNumber, warehouseNumber, qty, rowNumber, leadTime);
                    break;
                case "生产退料入库":
                    documentName = "开工单号";
                    break;
                case "生产出库":
                    documentName = "开工单号";
                    break;
            }
            if (!string.IsNullOrEmpty(sql))
            {
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    lbSubmit.Text = "数量大于订单未交数量,请重新填写数量！";
                    return;
                }
            }


            type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            if (type.Equals("采购入库"))
            {
                sql = string.Format(@" update MaterialWarehouseLogDetail set Qty = {0} ,remark='{1}',RoadTransport='{3}'
   where Guid ='{2}'", qty, remark, ToolManager.GetQueryString("Guid"), txtRoadTransport.Text.Trim());
            }
            else if (type.Equals("销售出库（贸易）"))
            {
                string completeQty = txtCompleteQty.Text.Trim().Equals("") ? "0" : txtCompleteQty.Text.Trim();
                if (Convert.ToDouble(qty) < Convert.ToDouble(completeQty))
                {
                    lbSubmit.Text = "完成数量不能大于订单未交数量";
                    return;
                }

                sql = string.Format(@" update MaterialWarehouseLogDetail set Qty = {0} ,remark='{1}',CompleteQty='{3}'
   where Guid ='{2}'", qty, remark, ToolManager.GetQueryString("Guid"), txtCompleteQty.Text.Trim());
            }
            else
            {
                sql = string.Format(@" update MaterialWarehouseLogDetail set Qty = {0} ,remark='{1}'
   where Guid ='{2}'", qty, remark, ToolManager.GetQueryString("Guid"));
            }


            bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "修改成功！" : "修改失败，原因：" + error;
            string logStr = string.Format("日志参数 记录唯一标识:{0},原材料编号:{1},改动数量：{2}，改动后数量:{3}"
                , ToolManager.GetQueryString("Guid"), materialNumber, oldQty, qty);

            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑" + type + "明细", "编辑成功 " + logStr);
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑" + type + "明细", "编辑失败！原因" + error + "  " + logStr);
                return;
            }
        }
    }
}