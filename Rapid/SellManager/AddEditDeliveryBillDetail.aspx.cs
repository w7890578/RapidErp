using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Text;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class AddEditDeliveryBillDetail : System.Web.UI.Page
    {
        public static string sn = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("DeliveryNumber"))
                {
                    Response.Write("该送货单不存在！");
                    Response.End();
                    return;
                }
                string deliveryNumber = ToolManager.GetQueryString("DeliveryNumber");
                hdDeliveryNumber.Value = deliveryNumber;
                if (ToolManager.CheckQueryString("OrdersNumber"))
                {
                    string ordersnumber = ToolManager.GetQueryString("OrdersNumber").Trim();
                    string prodeuctnumber = ToolManager.GetQueryString("ProductNumber").Trim();
                    string customerpn =Server.UrlDecode( ToolManager.GetQueryString("CustomerProductNumber").Trim());
                    string version = ToolManager.GetQueryString("Version").Trim();
                    string rownumber = ToolManager.GetQueryString("RowNumber").Trim();
                    string sql = string.Format(@" select * from DeliveryNoteDetailed 
                    where DeliveryNumber='{0}' and OrdersNumber='{1}' and ProductNumber='{2}'
and CustomerProductNumber='{3}' and Version='{4}' and RowNumber='{5}'", deliveryNumber, ordersnumber, prodeuctnumber,
                                                                      customerpn, version, rownumber);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtSN.Text = dr["SN"].ToString();
                        sn = txtSN.Text;
                        lbOrdersNumber.Text = dr["OrdersNumber"] == null ? "" : dr["OrdersNumber"].ToString();
                        lblProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                        lblVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                        lblMaterialDescription.Text = dr["MaterialDescription"] == null ? "" : dr["MaterialDescription"].ToString();
                        lblRowNumber.Text = dr["RowNumber"] == null ? "" : dr["RowNumber"].ToString();
                        lblDeliveryQty.Text = dr["DeliveryQty"] == null ? "" : dr["DeliveryQty"].ToString();
                        txtArriveQty.Text = dr["ArriveQty"] == null ? "" : dr["ArriveQty"].ToString();
                        txtConformenceQty.Text = dr["ConformenceQty"] == null ? "" : dr["ConformenceQty"].ToString();
                        txtNGReason.Text = dr["NGReason"] == null ? "" : dr["NGReason"].ToString();
                        txtPassQty.Text = dr["PassQty"] == null ? "" : dr["PassQty"].ToString();
                        txtNgQty.Text = dr["NgQty"] == null ? "" : dr["NgQty"].ToString();
                        txtInspectorNGReason.Text = dr["InspectorNGReason"] == null ? "" : dr["InspectorNGReason"].ToString();
                        txtRoughCastingCode.Text = dr["RoughCastingCode"] == null ? "" : dr["RoughCastingCode"].ToString();
                        txtImportPartsCode.Text = dr["ImportPartsCode"] == null ? "" : dr["ImportPartsCode"].ToString();
                        lbOrdersNumber.Text = dr["OrdersNumber"] == null ? "" : dr["OrdersNumber"].ToString();
                        lbCustomerProductNumber.Text = dr["CustomerProductNumber"] == null ? "" : dr["CustomerProductNumber"].ToString();
                    }
                }
            }
        }




        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ToolManager.CheckQueryString("DeliveryNumber"))
            {
                Response.Write("该送货单不存在！");
                Response.End();
                return;
            }
            string deliveryNumber = ToolManager.GetQueryString("DeliveryNumber");
            string sn = txtSN.Text;
            string ordersNumber = lbOrdersNumber.Text;
            string productNumber = lblProductNumber.Text;
            string customerProductNumber =lbCustomerProductNumber.Text;
            string version = lblVersion.Text;
            string materialDescription = lblMaterialDescription.Text;
            string rowNumber = lblRowNumber.Text;
            string deliveryQty = lblDeliveryQty.Text;
            string arriveQty = txtArriveQty.Text;
            string conformenceQty = txtConformenceQty.Text;
            string nGReason = txtNGReason.Text;
            string passQty = txtPassQty.Text;
            string passq=passQty== "" ? "0" :passQty;
            string ngQty = txtNgQty.Text;
            string inspectorNGReason = txtInspectorNGReason.Text;
            string roughCastingCode = txtRoughCastingCode.Text;
            string importPartsCode = txtImportPartsCode.Text;
            string error = string.Empty;
            string sql = string.Empty;
            int aqty = Convert.ToInt32(arriveQty);
            int cqty = Convert.ToInt32(conformenceQty);
            int pqty = Convert.ToInt32(passq);
            int nqty = Convert.ToInt32(ngQty);
            int dqty = Convert.ToInt32(deliveryQty);
            if (aqty > dqty )
            {
                lbSubmit.Text = "实到数量不能超过发货数量！";
                return;
            }
            if (cqty> dqty)
            {
                lbSubmit.Text = "实收数量不能超过发货数量！";
                return;
            }
            if (pqty > dqty)
            {
                lbSubmit.Text = "合格品数量不能超过发货数量！";
                return;
            }
            if (nqty > dqty)
            {
                lbSubmit.Text = "拒收数量不能超过发货数量！";
                return;
            }
            sql = string.Format(@" select * from DeliveryNoteDetailed  where DeliveryNumber='{0}' and OrdersNumber='{1}' and ProductNumber='{2}'
and CustomerProductNumber='{3}' and Version='{4}' and RowNumber='{5}' and SN!='{6}'", deliveryNumber, ordersNumber, productNumber,
                                                                  customerProductNumber, version, rowNumber,sn);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count > 0)
            {
                lbSubmit.Text = "该条记录已存在，请重新填写！";
                return;
            }
            sql = string.Format(@" 
update DeliveryNoteDetailed set SN ={0},MaterialDescription='{1}',DeliveryQty={2},ArriveQty={3},ConformenceQty={4},
NGReason ='{5}',PassQty ='{6}',NgQty={7},InspectorNGReason='{8}',RoughCastingCode='{9}',ImportPartsCode='{10}'
where DeliveryNumber='{11}' and OrdersNumber='{12}' and ProductNumber='{13}'
and CustomerProductNumber='{14}' and Version='{15}' and RowNumber='{16}'",
sn, materialDescription, deliveryQty, arriveQty, conformenceQty, nGReason, passQty, ngQty, inspectorNGReason, roughCastingCode
, importPartsCode, deliveryNumber, ordersNumber, productNumber, customerProductNumber, version, rowNumber);
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑送货单明细" + deliveryNumber, "编辑成功");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑送货单明细" + deliveryNumber, "编辑失败！原因" + error);
                return;
            }
        }
    }
}
