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
    public partial class ImpCertificateOrdersList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindDrp("select Id,PaymentMode from PaymentMode", drpPaymentMode, "Id", "PaymentMode");
                ControlBindManager.BindDrp("select SupplierId,SupplierName from SupplierInfo", drpSupplierId, "SupplierId", "SupplierName");
                ControlBindManager.BindDrp("select [USER_ID],[USER_NAME] from PM_USER", drpContactId, "USER_ID", "USER_NAME");
               
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string ordersDate = txtOrdersDate.Text;
            string paymentMode = drpPaymentMode.SelectedValue;
            string supplierId = drpSupplierId.SelectedValue;
            string contactId = drpContactId.SelectedValue;
            string orderNumber = string.Empty;
            string remark = txtRemark.Text;
            string sql = string.Empty;
            string error = string.Empty;

           
                orderNumber = txtOrderNumber.Text.Trim();

                if (string.IsNullOrEmpty(orderNumber))
                {
                    orderNumber = "CG" + DateTime.Now.ToString("yyyyMMddHHmmss");
                }
                if (CheckHas(orderNumber))
                {
                    lbMsg.Text = "已存在该采购订单号！请重新输入";
                    return;
                }

                sql = string.Format(@" 
insert into CertificateOrders (OrdersNumber,OrdersDate,PaymentMode,SupplierId
,ContactId,OrderStatus,CreateTime ,Remark )
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}') ", orderNumber, ordersDate
, paymentMode, supplierId, contactId, "未完成", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remark);


               SqlHelper.ExecuteSql(sql, ref error);

               bool result = BLL.PurchaseManager.ImpCertificateOrdersList(FU_Excel, Server, orderNumber, ref error); 
               if (result == true)
               {
                   lbMsg.Text = "导入成功";
                   Tool.WriteLog(Tool.LogType.Operating, "导入采购订单", "导入成功！");
                   return;
               }
               else
               {
                   lbMsg.Text = error;
                   Tool.WriteLog(Tool.LogType.Operating, "导入采购订单", "导入失败！原因" + lbSubmit.Text);
                   return;
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
