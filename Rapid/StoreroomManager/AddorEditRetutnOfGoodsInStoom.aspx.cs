using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Model;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class AddorEditRetutnOfGoodsInStoom : System.Web.UI.Page
    {
        public static string titleName = string.Empty;
        public static string orderName = string.Empty;
        public static string showReason = "inline";
        public static string type = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                trPlanNumberOrderNumber.Visible = false;
                Check();
                string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                lbWarehouseNumber.Text = warehouseNumber;
                if (ToolManager.CheckQueryString("DocumentNumber") && ToolManager.CheckQueryString("WarehouseNumber")
                     && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version")
                     && ToolManager.CheckQueryString("PlanNumberOrderNumber") && ToolManager.CheckQueryString("RowNumber")
                    )
                {
                    string documentNumber = ToolManager.GetQueryString("DocumentNumber");
                    string productNumber = ToolManager.GetQueryString("ProductNumber");
                    string version = ToolManager.GetQueryString("Version");
                    string planNumberOrderNumber = ToolManager.GetQueryString("PlanNumberOrderNumber");
                    string rowNumber = ToolManager.GetQueryString("RowNumber");
                    string sql = string.Format(@" select * from ProductWarehouseLogDetail where WarehouseNumber='{0}'
 and DocumentNumber='{1}' and ProductNumber ='{2}' and Version ='{3}'  and RowNumber='{4}' and OrdersNumber='{5}'", warehouseNumber, documentNumber, productNumber, version, rowNumber, planNumberOrderNumber);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        lbOdersNumber.Text = dr["DocumentNumber"] == null ? "" : dr["DocumentNumber"].ToString();
                        lbProduct.Text = dr["ProductNumber"].ToString() + "|" + dr["Version"].ToString();
                        txtCustomerProductNumber.Text = dr["CustomerProductNumber"] == null ? "" : dr["CustomerProductNumber"].ToString();
                        txtQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        lbOldQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        txtReason.Text = dr["ReturnReason"] == null ? "" : dr["ReturnReason"].ToString() == "" ? "无" : dr["ReturnReason"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        txtInventoryQty.Text = StoreroomToolManager.GetinventoryQty("", dr["ProductNumber"].ToString(), dr["Version"].ToString(), warehouseNumber
, "ProductStock", "ProductWarehouseLog", ToolEnum.ProductType.Product);
                        lbOrdersNumber.Text = dr["OrdersNumber"] == null ? "" : dr["OrdersNumber"].ToString();
                        lbRowNumber.Text = dr["RowNumber"] == null ? "" : dr["RowNumber"].ToString();
                        txtOdersNumber.Visible = false;
                        lbOdersNumber.Visible = true;
                        drpProduct.Visible = false;
                        btnSubmit.Text = "修改";

                        if (!planNumberOrderNumber.Equals("0"))
                        {
                            trPlanNumberOrderNumber.Visible = false;
                            trRowNumber.Visible = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string warehouseNumber = this.lbWarehouseNumber.Text;
            string documentNumber = txtOdersNumber.Text;
            string product = string.Empty;
            string productNumber = string.Empty;
            string version = string.Empty;
            string customerProductNumber = string.Empty;
            string qty = this.txtQty.Text;
            string remark = txtRemark.Text;
            string inventoryQty = txtInventoryQty.Text;
            string reason = txtReason.Text;
            string sql = string.Empty;
            string error = string.Empty;
            string planNumberOrderNumber = lbOrdersNumber.Text;
            string rowNumber = lbRowNumber.Text;

            documentNumber = lbOdersNumber.Text;
            product = lbProduct.Text;
            productNumber = product.Split('|')[0].ToString();
            version = product.Split('|')[1].ToString();
            List<string> sqls = new List<string>();
            if (type.Equals("出库"))
            {
                if (Convert.ToInt32(qty) > Convert.ToInt32(inventoryQty))
                {
                    ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = "数量大于库存数量";
                    return;
                }
            }
            if (!CheckQty(warehouseNumber, documentNumber, productNumber, version, rowNumber, qty))
            {
                ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = "数量大于订单未交数量";
                return;
            }

            sql = string.Format(@" update ProductWarehouseLogDetail 
set ReturnReason ='{0}' , qty={1} ,Remark ='{2}'
 where WarehouseNumber='{3}' and DocumentNumber='{4}' and ProductNumber='{5}' and [Version] ='{6}' and RowNumber='{7}' and OrdersNumber='{8}'",
reason, qty, remark, warehouseNumber, documentNumber, productNumber, version, rowNumber, planNumberOrderNumber);
            sqls.Add(sql);
            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = result == true ? "修改成功" : "修改失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑" + titleName + "明细" + warehouseNumber, "编辑成功");
                Response.Write("<script>window.close();</script>");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑" + titleName + "明细" + warehouseNumber, "编辑失败！原因" + error);
                txtCustomerProductNumber.Text = customerProductNumber;
                txtInventoryQty.Text = inventoryQty;
                return;
            } 
        }

        /// <summary>
        /// 检查销售出库和包装销售出库的数量
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <param name="documentNumber"></param>
        /// <param name="productNumber"></param>
        /// <param name="version"></param>
        /// <param name="rowNumber"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        private bool CheckQty(string warehouseNumber, string documentNumber, string productNumber, string version, string rowNumber, string qty)
        {
            string sql = string.Format(@"  select COUNT(*) from  ProductWarehouseLogDetail pwld 
 inner join ProductWarehouseLog pwl on pwld.WarehouseNumber =pwl.WarehouseNumber 
 left join MachineOderDetail modt on 
 pwld.DocumentNumber =modt.OdersNumber and pwld.ProductNumber =modt.ProductNumber and pwld.Version =modt.Version 
 and pwld.RowNumber =modt.RowNumber 
 where pwld.WarehouseNumber ='{0}' and pwl.Type in ('销售出库','包装销售出库') and pwld.DocumentNumber ='{1}'
 and pwld.ProductNumber ='{2}' and pwld.Version ='{3}' and pwld.RowNumber ='{4}' and ({5} - isnull( modt.NonDeliveryQty,0) >0 )"
 , warehouseNumber, documentNumber, productNumber, version, rowNumber, qty);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //        
        private void Check()
        {
            if (!ToolManager.CheckQueryString("WarehouseNumber"))
            {
                Response.Write("未知入库单！");
                Response.End();
                return;
            }
            titleName = Server.UrlDecode(ToolManager.GetQueryString("ChangeDirection"));
            switch (titleName)
            {
                case "退货入库": orderName = "销售订单编号";
                    showReason = "inline";
                    type = "入库";
                    break;
                case "包装入库":
                    orderName = "销售订单编号";
                    showReason = "none";
                    type = "入库";
                    break;
                case "维修入库": orderName = "维修订单号";
                    showReason = "none";
                    type = "入库";
                    break;
                case "包装销售出库": orderName = "销售订单编号";
                    showReason = "none";
                    type = "出库";
                    break;
                case "销售出库": orderName = "销售订单编号";
                    showReason = "none";
                    type = "出库";
                    break;
                case "样品出库": orderName = "销售订单编号";
                    showReason = "none";
                    type = "出库";
                    break;
                case "维修出库": orderName = "维修订单号";
                    showReason = "none";
                    type = "出库";
                    break;
                case "样品入库": orderName = "开工单号";
                    showReason = "none";
                    type = "入库";
                    break;
                case "生产入库": orderName = "开工单号";
                    showReason = "none";
                    type = "入库";
                    break;
            }
        }
    }
}
