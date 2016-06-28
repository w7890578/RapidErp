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
    public partial class EditT_AccountsPayable_MainList_New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblOrdersNumber.Text = ToolManager.GetQueryString("ordersNumber");
                lblCreateTime.Text = ToolManager.GetQueryString("createTime");
                string sql = string.Format(@" 
select * from T_AccountsPayable_Main where OrdersNumber ='{0}' and CreateTime ='{1}' ", lblOrdersNumber.Text, lblCreateTime.Text);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtInvoiceNumber.Text = dr["invoiceNumber"] == null ? "" : dr["invoiceNumber"].ToString(); //发票号码
                    txtInvoiceDate.Text = dr["invoiceDate"] == null ? "" : dr["invoiceDate"].ToString(); //开票日期
                    drpAccountPeriod.SelectedValue = dr["accountPeriod"] == null ? "" : dr["accountPeriod"].ToString();//账期
                    txtActualPaymentsAmount.Text = dr["actualPaymentsAmount"] == null ? "" : dr["actualPaymentsAmount"].ToString();//实际付款金额
                    txtActualPaymentsDate.Text = dr["actualPaymentsDate"] == null ? "" : dr["actualPaymentsDate"].ToString();//实际付款日期
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
            string actualPaymentsAmount = txtActualPaymentsAmount.Text;//实际付款金额
            string actualPaymentsDate = txtActualPaymentsDate.Text;//实际付款日期

            string kxDq = Convert.ToDateTime(invoiceDate).AddDays(Convert.ToInt32(accountPeriod)).ToString("yyyy-MM-dd");

            string error = string.Empty;
            string sql = string.Empty;
            sql = string.Format(@" update T_AccountsPayable_Main set invoiceNumber='{0}',invoiceDate='{1}',accountPeriod='{2}',
actualPaymentsAmount=actualPaymentsAmount+{3},actualPaymentsDate='{4}',PaymentDueDate='{7}'
 where OrdersNumber ='{5}' and CreateTime ='{6}' ", invoiceNumber, invoiceDate, accountPeriod, actualPaymentsAmount, actualPaymentsDate
 , ordersNumber, createTime, kxDq);
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
