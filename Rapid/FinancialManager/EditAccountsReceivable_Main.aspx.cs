using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class EditAccountsReceivable_Main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblOrdersNumber.Text = ToolManager.GetQueryString("ordersNumber");
                lblCreateTime.Text = ToolManager.GetQueryString("createTime");
                string sql = string.Format(@" 
select * from AccountsReceivable where OrdersNumber ='{0}' and CreateTime ='{1}' ", lblOrdersNumber.Text, lblCreateTime.Text);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtInvoiceNumber.Text = dr["invoiceNumber"] == null ? "" : dr["invoiceNumber"].ToString(); //发票号码
                    txtInvoiceDate.Text = dr["invoiceDate"] == null ? "" : dr["invoiceDate"].ToString(); //开票日期
                    drpAccountPeriod.SelectedValue = dr["accountPeriod"] == null ? "" : dr["accountPeriod"].ToString();//账期
                    txtActualPaymentsAmount.Text = dr["ActualMakeCollectionsAmount"] == null ? "" : dr["ActualMakeCollectionsAmount"].ToString();//实际收款金额
                    txtActualPaymentsDate.Text = dr["ActuaMakeCollectionsDate"] == null ? "" : dr["ActuaMakeCollectionsDate"].ToString();//实际收款日期
                    txtYSPrice.Text = dr["YSPrice"] == null ? "" : dr["YSPrice"].ToString();
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string ordersNumber = lblOrdersNumber.Text;
            string createTime = lblCreateTime.Text;
            string invoiceNumber = txtInvoiceNumber.Text; //发票号码
            string invoiceDate = txtInvoiceDate.Text; //开票日期
            string accountPeriod = drpAccountPeriod.SelectedValue;//账期
            string actualPaymentsAmount = txtActualPaymentsAmount.Text;//实际收款金额
            string actualPaymentsDate = txtActualPaymentsDate.Text;//实际收款日期
            string ysPrice = txtYSPrice.Text;//预收金额
            DateTime kxDq = Convert.ToDateTime(invoiceDate).AddDays(Convert.ToInt32(accountPeriod));
            string error = string.Empty;
            string sql = string.Empty;
            sql = string.Format(@" update AccountsReceivable set invoiceNumber='{0}',invoiceDate='{1}',accountPeriod='{2}',
ActualMakeCollectionsAmount=isnull(ActualMakeCollectionsAmount,0)+{3},ActuaMakeCollectionsDate='{4}',YSPrice='{5}',PaymentDueDate='{8}'
 where OrdersNumber ='{6}' and CreateTime ='{7}' ", invoiceNumber, invoiceDate, accountPeriod, actualPaymentsAmount, actualPaymentsDate
 , ysPrice, ordersNumber, createTime, kxDq.ToString("yyyy-MM-dd"));
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            if (result)
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                lbSubmit.Text = "修改失败！原因：" + error;
            }
        }
    }
}
