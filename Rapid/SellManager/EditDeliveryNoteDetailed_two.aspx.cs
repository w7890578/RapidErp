using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using BLL;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class EditDeliveryNoteDetailed_two : System.Web.UI.Page
    {
        public static string deliveryqty ="";
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
                    string ordersnumber = ToolManager.GetQueryString("OrdersNumber").Trim ();
                    string prodeuctnumber = ToolManager.GetQueryString("ProductNumber").Trim();
                    string customerpn =Server.UrlDecode( ToolManager.GetQueryString("CustomerProductNumber").Trim());
                    string version = ToolManager.GetQueryString("Version").Trim();
                    string rownumber = ToolManager.GetQueryString("RowNumber").Trim();
                    string sql = string.Format(@" select * from V_DeliveryNoteDetailed_two 
                    where 送货单号='{0}' and 销售订单号='{1}' and 产成品编号='{2}'
and 物料编号='{3}' and 版本='{4}' and 行号='{5}'", deliveryNumber, ordersnumber, prodeuctnumber,
                                                                      customerpn, version, rownumber);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        lblSn.Text = dr["行号"].ToString();
                        lbOrdersNumber.Text = dr["销售订单号"] == null ? "" : dr["销售订单号"].ToString();
                        lblProductNumber.Text = dr["产成品编号"] == null ? "" : dr["产成品编号"].ToString();
                        lblVersion.Text = dr["版本"] == null ? "" : dr["版本"].ToString();
                        lblMaterialDescription.Text = dr["描述"] == null ? "" : dr["描述"].ToString();
                        lblRowNumber.Text = dr["行号"] == null ? "" : dr["行号"].ToString();
                        lblKhhN.Text = dr["客户号"] == null ? "" : dr["客户号"].ToString();
                        txtConformenceQty.Text = dr["实收数量"] == null ? "" : dr["实收数量"].ToString();
                        lbCustomerOrdersNumber.Text = dr["客户订单号"] == null ? "" : dr["客户订单号"].ToString();
                        lblQty.Text = dr["订单总量"] == null ? "" : dr["订单总量"].ToString();
                        deliveryqty = dr["发货数量"] == null ? "" : dr["发货数量"].ToString();
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
            string ordersnumber = ToolManager.GetQueryString("OrdersNumber");
            string prodeuctnumber = ToolManager.GetQueryString("ProductNumber");
            string customerpn = ToolManager.GetQueryString("CustomerProductNumber");
            string version = ToolManager.GetQueryString("Version");
            string rownumber = ToolManager.GetQueryString("RowNumber");
            string conformenceQty = txtConformenceQty.Text;
            string error = string.Empty;
            string sql = string.Empty;
            int deliveryQty =Convert.ToInt32(deliveryqty);
            if (Convert.ToInt32(conformenceQty) > deliveryQty)
            {
                lbSubmit.Text = "交货数量不能大于发货数量！";
                return;
            }
            sql = string.Format(@" 
update DeliveryNoteDetailed set ConformenceQty={0}
where DeliveryNumber='{1}' and OrdersNumber='{2}' and ProductNumber='{3}'
and CustomerProductNumber='{4}' and Version='{5}' and RowNumber='{6}'",
 conformenceQty, deliveryNumber,
 ordersnumber, prodeuctnumber, customerpn, version, rownumber);
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
