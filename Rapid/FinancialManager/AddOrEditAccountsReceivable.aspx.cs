using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using BLL;
using Rapid.ToolCode;

namespace Rapid.FinancialManager
{
    public partial class AddOrEditAccountsReceivable : System.Web.UI.Page
    {
        public static string AmountCollected = string.Empty;
         public static string IsSettle =string.Empty;
         public static string PaymentDueDate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("OrdersNumber") && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("CreateTime")&&ToolManager.CheckQueryString("Version"))
                {
                    string OrdersNumber = ToolManager.GetQueryString("OrdersNumber");
                    string ProductNumber = ToolManager.GetQueryString("ProductNumber");
                    string CreateTime = ToolManager.GetQueryString("CreateTime");
                    string Version = ToolManager.GetQueryString("Version");
                    string sql = string.Format(@"select * from AccountsReceivable where OrdersNumber='{0}' and ProductNumber='{1}' and CreateTime='{2}' and Version='{3}'", OrdersNumber, ProductNumber, CreateTime,Version);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        this.lblOrdersNumber.Text = dr["OrdersNumber"] == null ? "" : dr["OrdersNumber"].ToString();
                        this.lblProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                        this.lblVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                        this.lblCreateTime.Text = dr["CreateTime"] == null ? "" : dr["CreateTime"].ToString();
                        this.lblCustomerProductNumber.Text = dr["CustomerProductNumber"] == null ? "" : dr["CustomerProductNumber"].ToString();
                        this.lblCustomerId.Text = dr["CustomerId"] == null ? "" : dr["CustomerId"].ToString();
                        this.lblQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        this.lblUnitPrice.Text = dr["UnitPrice"] == null ? "" : dr["UnitPrice"].ToString();
                        this.lblSumPrice.Text = dr["SumPrice"] == null ? "" : dr["SumPrice"].ToString();
                        this.txtDeliveryDate.Text = dr["DeliveryDate"] == null ? "" : dr["DeliveryDate"].ToString();
                        this.txtInvoiceNumber.Text = dr["InvoiceNumber"] == null ? "" : dr["InvoiceNumber"].ToString();
                        this.txtInvoiceDate.Text = dr["InvoiceDate"] == null ? "" : dr["InvoiceDate"].ToString();
                        this.drpAccountPeriod.SelectedValue = dr["AccountPeriod"] == null ? "" : dr["AccountPeriod"].ToString();
                        this.txtActualMakeCollectionsAmount.Text = dr["ActualMakeCollectionsAmount"] == null ? "" : dr["ActualMakeCollectionsAmount"].ToString();
                        this.txtActuaMakeCollectionsDate.Text = dr["ActuaMakeCollectionsDate"] == null ? "" : dr["ActuaMakeCollectionsDate"].ToString();
                        AmountCollected = dr["AmountCollected"] == null ? "" : dr["AmountCollected"].ToString();
                        IsSettle = dr["IsSettle"] == null ? "" : dr["IsSettle"].ToString();

                    }
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string OrdersNumber = this.lblOrdersNumber.Text;
            string ProductNumber = this.lblProductNumber.Text;
            string Version = this.lblVersion.Text;
            string CreateTime = this.lblCreateTime.Text;
            string CustomerProductNumber = this.lblCustomerProductNumber.Text;
            string CustomerId = this.lblCustomerId.Text;
            string Qty = this.lblQty.Text;
            string UnitPrice = this.lblUnitPrice.Text;
            string SumPrice = this.lblSumPrice.Text;
            string DeliveryDate = this.txtDeliveryDate.Text;
            string InvoiceNumber = this.txtInvoiceNumber.Text;
            string InvoiceDate = this.txtInvoiceDate.Text;
            DateTime ind = Convert.ToDateTime(InvoiceDate);
            string AccountPeriod = this.drpAccountPeriod.SelectedValue;
            string ActualMakeCollectionsAmount = this.txtActualMakeCollectionsAmount.Text;
            string ActuaMakeCollectionsDate = this.txtActuaMakeCollectionsDate.Text;
            PaymentDueDate = ind.AddDays(Convert.ToInt32(AccountPeriod)).ToString("yyyy-MM-dd");
            AmountCollected = (Convert.ToDecimal(SumPrice) - Convert.ToDecimal(ActualMakeCollectionsAmount)).ToString();
            decimal amc = Convert.ToDecimal(AmountCollected);
            if (amc <= 0)
            {
                IsSettle = "1";
            }
            else
            {
                IsSettle = "0";
            }
            string sql = string.Format(@"update AccountsReceivable set DeliveryDate='{0}',InvoiceNumber='{1}',InvoiceDate='{2}',
AccountPeriod='{3}',ActualMakeCollectionsAmount='{4}',ActuaMakeCollectionsDate='{5}' ,IsSettle='{10}',PaymentDueDate='{11}',AmountCollected='{12}' where OrdersNumber='{6}' and  ProductNumber='{7}' and CreateTime='{8}' and Version='{9}'",
 DeliveryDate, InvoiceNumber, InvoiceDate, AccountPeriod, ActualMakeCollectionsAmount, ActuaMakeCollectionsDate, OrdersNumber, ProductNumber, CreateTime, Version, IsSettle, PaymentDueDate, AmountCollected);
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑应收账款" + ToolManager.GetQueryString("id"), "编辑成功");
            }
            else
            {

                Tool.WriteLog(Tool.LogType.Operating, "编辑应收账款" + ToolManager.GetQueryString("id"), "编辑失败！原因：" + error);
            }
            return;

        }
    }
}
