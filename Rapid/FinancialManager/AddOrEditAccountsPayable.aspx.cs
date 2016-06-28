using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;


namespace Rapid.FinancialManager
{
    public partial class AddOrEditAccountsPayable : System.Web.UI.Page
    {
        public static string  AmountCollected=string.Empty;
        public static string  IsSettle=string.Empty ;
        public static string PaymentDueDate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("OrdersNumber") && ToolManager.CheckQueryString("MaterialNumber") && ToolManager.CheckQueryString("CreateTime"))
                {
                    string OrdersNumber = ToolManager.GetQueryString("OrdersNumber");
                    string MaterialNumber = ToolManager.GetQueryString("MaterialNumber");
                    string CreateTime = ToolManager.GetQueryString("CreateTime");
                    string sql = string.Format(@"select * from AccountsPayable where OrdersNumber='{0}' and MaterialNumber='{1}' and CreateTime='{2}'", OrdersNumber, MaterialNumber, CreateTime);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        this.lblOrdersNumber.Text = dr["OrdersNumber"] == null ? "" : dr["OrdersNumber"].ToString();
                        this.lblMaterialNumber.Text = dr["MaterialNumber"] == null ? "" : dr["MaterialNumber"].ToString();
                        this.lblCreateTime.Text = dr["CreateTime"] == null ? "" : dr["CreateTime"].ToString();
                        this.lblSupplierMaterialNumber.Text = dr["SupplierMaterialNumber"] == null ? "" : dr["SupplierMaterialNumber"].ToString();
                        this.lblSupplierId.Text = dr["SupplierId"] == null ? "" : dr["SupplierId"].ToString();
                        this.lblQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        this.lblUnitPrice.Text = dr["UnitPrice"] == null ? "" : dr["UnitPrice"].ToString();
                        this.lblSumPrice.Text = dr["SumPrice"] == null ? "" : dr["SumPrice"].ToString();
                        this.txtDeliveryDate.Text = dr["DeliveryDate"] == null ? "" : dr["DeliveryDate"].ToString();
                        this.txtInvoiceNumber.Text = dr["InvoiceNumber"] == null ? "" : dr["InvoiceNumber"].ToString();
                        this.txtInvoiceDate.Text = dr["InvoiceDate"] == null ? "" : dr["InvoiceDate"].ToString();
                        this.drpAccountPeriod.SelectedValue = dr["AccountPeriod"] == null ? "" : dr["AccountPeriod"].ToString();
                        this.txtActualPaymentsAmount.Text = dr["ActualPaymentsAmount"] == null ? "" : dr["ActualPaymentsAmount"].ToString();
                        this.txtActualPaymentsDate.Text = dr["ActualPaymentsDate"] == null ? "" : dr["ActualPaymentsDate"].ToString();
                        AmountCollected = dr["AmountCollected"] == null ? "" : dr["AmountCollected"].ToString();
                        IsSettle = dr["IsSettle"] == null ? "" : dr["IsSettle"].ToString();
                       
                    }
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
            string error=string.Empty;
            string OrdersNumber = this.lblOrdersNumber.Text;
            string MaterialNumber = this.lblMaterialNumber.Text;
            string CreateTime = this.lblCreateTime.Text;
            string SupplierMaterialNumber=this.lblSupplierMaterialNumber.Text;
            string lblSupplierId=this.lblSupplierId.Text;
            string Qty=this.lblQty.Text;
            string UnitPrice=this.lblUnitPrice.Text;
            string SumPrice=this.lblSumPrice.Text;
            string DeliveryDate=this.txtDeliveryDate.Text;
            string InvoiceNumber=this.txtInvoiceNumber.Text;
            string InvoiceDate=this.txtInvoiceDate.Text;
            DateTime ind = Convert.ToDateTime(InvoiceDate);
            string AccountPeriod = this.drpAccountPeriod.SelectedValue;
            string ActualPaymentsAmount=this.txtActualPaymentsAmount.Text;
            string ActualPaymentsDate = this.txtActualPaymentsDate.Text;
            PaymentDueDate=ind.AddDays(Convert.ToInt32(AccountPeriod)).ToString("yyyy-MM-dd");
            AmountCollected=(Convert.ToDecimal( SumPrice)-Convert.ToDecimal( ActualPaymentsAmount)).ToString();
            decimal amc=Convert.ToDecimal(AmountCollected) ;
            if (amc<= 0)
            {
                IsSettle = "1";
            }
            else
            {
                IsSettle = "0";
            }
            string sql = string.Format(@"update AccountsPayable set SumPrice='{0}',DeliveryDate='{1}',InvoiceNumber='{2}',InvoiceDate='{3}',
AccountPeriod='{4}',ActualPaymentsAmount='{5}',ActualPaymentsDate='{6}',IsSettle='{7}',PaymentDueDate='{8}',AmountCollected='{9}' where OrdersNumber='{10}' and  MaterialNumber='{11}' and CreateTime='{12}'",
SumPrice, DeliveryDate, InvoiceNumber, InvoiceDate, AccountPeriod, ActualPaymentsAmount, ActualPaymentsDate, IsSettle, PaymentDueDate, AmountCollected, OrdersNumber, MaterialNumber, CreateTime);
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑应付账款" + ToolManager.GetQueryString("id"), "编辑成功");
            }
            else
            {

                Tool.WriteLog(Tool.LogType.Operating, "编辑应付账款" + ToolManager.GetQueryString("id"), "编辑失败！原因：" + error);
            }
            return;
                                                                                                                                             
        }
    }
}
