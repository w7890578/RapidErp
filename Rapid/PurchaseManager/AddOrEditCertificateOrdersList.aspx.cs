using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.PurchaseManager
{
    public partial class AddOrEditCertificateOrdersList : System.Web.UI.Page
    {
        public string SupplierIds = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string getsupSql = "select SupplierId,SupplierName from SupplierInfo order by SupplierName asc";
                DataTable dtSupplier = SqlHelper.GetTable(getsupSql);
                foreach (DataRow dr in dtSupplier.Rows)
                {
                    SupplierIds += dr["SupplierName"] + ",";
                }
                SupplierIds = SupplierIds.TrimEnd(',');

                ControlBindManager.BindDrp("select Id,PaymentMode from PaymentMode", drpPaymentMode, "Id", "PaymentMode");

                //ControlBindManager.BindDrp("select SupplierId,SupplierName from SupplierInfo order by SupplierName asc", drpSupplierId, "SupplierId", "SupplierName");
                ControlBindManager.BindDrp("select [USER_ID],[USER_NAME] from PM_USER", drpContactId, "USER_ID", "USER_NAME");
                if (ToolManager.CheckQueryString("OrdersNumber"))
                {
                    string ordersNumber = ToolManager.GetQueryString("OrdersNumber");
                    if (!CheckHas(ordersNumber))
                    {
                        Response.Write("该采购订单不存在！");
                        Response.End();
                        return;
                    }
                    string sql = string.Format(" select * from CertificateOrders where OrdersNumber='{0}' ", ordersNumber);
                    DataTable dt = SqlHelper.GetTable(sql);
                    DataRow dr = dt.Rows[0];
                    txtOrdersDate.Text = dr["OrdersDate"] == null ? "" : dr["OrdersDate"].ToString();
                    drpPaymentMode.SelectedValue = dr["PaymentMode"] == null ? "" : dr["PaymentMode"].ToString();
                    //drpSupplierId.SelectedValue = dr["SupplierId"] == null ? "" : dr["SupplierId"].ToString();

                    DataRow[] drs = dtSupplier.Select("SupplierId='" + dr["SupplierId"] + "'");
                    if (drs.Length > 0)
                    {
                        this.SupplierName.Value = drs[0]["SupplierName"].ToString();
                    }

                    drpContactId.SelectedValue = dr["ContactId"] == null ? "" : dr["ContactId"].ToString();
                    drpOrderType.SelectedValue = dr["OrderType"].ToString();
                    txtHTNumber.Text = dr["HTNumber"] == null ? "" : dr["HTNumber"].ToString();

                    txtOrderNumber.Text = ordersNumber;
                    txtOrderNumber.ReadOnly = true;


                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    btnSubmit.Text = "修改";

                }
                else
                {
                    btnSubmit.Text = "添加";
                    txtOrderNumber.ReadOnly = false;
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string ordersDate = txtOrdersDate.Text;
            string paymentMode = drpPaymentMode.SelectedValue;
            string supplierName = this.SupplierName.Value;
            //string supplierId = drpSupplierId.SelectedValue;
            string supplierId = string.Empty;
            string contactId = drpContactId.SelectedValue;
            string orderNumber = string.Empty;
            string remark = txtRemark.Text;
            string sql = string.Empty;
            string error = string.Empty;
            string orderType = drpOrderType.SelectedValue;
            string htNumber = txtHTNumber.Text.Trim();


            string checkSql = string.Format(@"
select SupplierId from SupplierInfo where SupplierName='{0}' ", supplierName);
            supplierId = SqlHelper.GetScalar(checkSql);
            if (string.IsNullOrEmpty(supplierId))
            {
                ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = "不存在该供应商名称，请重新输入";
                return;
            }

            if (btnSubmit.Text.Equals("添加"))
            {
                orderNumber = txtOrderNumber.Text.Trim();

                if (string.IsNullOrEmpty(orderNumber))
                {
                    orderNumber = "CG" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                if (CheckHas(orderNumber))
                {
                    ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = "已存在该采购订单号！请重新输入";
                    return;
                }

                sql = string.Format(@" 
insert into CertificateOrders (OrdersNumber,OrdersDate,PaymentMode,SupplierId
,ContactId,OrderStatus,CreateTime ,Remark,OrderType ,HTNumber)
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ", orderNumber, ordersDate
, paymentMode, supplierId, contactId, "未完成", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remark, orderType, htNumber);


                bool result = SqlHelper.ExecuteSql(sql, ref error);
                ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加采购单信息" + orderNumber, "增加成功");
                    ToolManager.CloseCurrentPage();
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加采购单信息" + orderNumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                string ordersNumber = ToolManager.GetQueryString("OrdersNumber");
                if (!CheckHas(ordersNumber))
                {
                    Response.Write("该采购订单不存在！");
                    Response.End();
                    return;
                }
                sql = string.Format(@"update CertificateOrders set OrdersDate='{0}'
,PaymentMode='{1}',SupplierId='{2}',ContactId='{3}',Remark ='{4}',OrderType='{6}',HTNumber='{7}' where OrdersNumber='{5}'
", ordersDate, paymentMode, supplierId, contactId, remark, ordersNumber, orderType, htNumber);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑采购单信息" + ordersNumber, "编辑成功");
                    Response.Write(ToolManager.GetClosePageJS());
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑采购单信息" + ordersNumber, "编辑失败！原因" + error);
                    return;
                }

            }

        }


        /// <summary>
        /// 检查订单是否存在
        /// </summary>
        /// <param name="ordersNumber"></param>
        private bool CheckHas(string ordersNumber)
        {
            string sql = string.Format(" select count(*) from CertificateOrders where OrdersNumber='{0}' ", ordersNumber);
            return SqlHelper.GetTable(sql).Rows[0][0].ToString().Equals("0") ? false : true;
        }
    }
}
