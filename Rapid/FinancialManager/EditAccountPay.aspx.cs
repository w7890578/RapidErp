using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.FinancialManager
{
    public partial class EditAccountPay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Guid"))
                {
                    string guid = ToolManager.GetQueryString("Guid");

                    string sql = string.Format(@" select *  from V_AccountsPay 
where Guid ='{0}' ", guid);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtActualPaymentsAmount.Text = dr["实际付款金额"] == null ? "" : dr["实际付款金额"].ToString();
                        txtActualPaymentsDate.Text = dr["实际付款日期"] == null ? "" : dr["实际付款日期"].ToString();
                        txtInvoiceNumber.Text = dr["发票号码"] == null ? "" : dr["发票号码"].ToString();
                        txtInvoiceDate.Text = dr["开票日期"] == null ? "" : dr["开票日期"].ToString();
                        drpJQ.SelectedValue = dr["是否结清"] == null ? "" : dr["是否结清"].ToString();
                        txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string actualpaymentsamout = Request.Form["txtActualPaymentsAmount"].ToString();
            string actualpaymentsdate = Request.Form["txtActualPaymentsDate"].ToString();
            string invoiceNumber = Request.Form["txtInvoiceNumber"].ToString();
            string invoiceDate = Request.Form["txtInvoiceDate"].ToString();
            string jq = Request.Form["drpJQ"].ToString();
            string remark = Request.Form["txtRemark"].ToString();
            string guid = ToolManager.GetQueryString("Guid");
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"update T_AccountsPayable_Main 
set ActualPaymentsAmount='{0}',
ActualPaymentsDate='{1}',
IsSettle='{2}',Remark='{3}',
InvoiceNumber='{5}',
InvoiceDate='{6}'
where Guid='{4}'",
                actualpaymentsamout, actualpaymentsdate, jq, remark, guid, invoiceNumber, invoiceDate);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                lbSubmit.Text = "修改成功！";
                return;
            }
            else
            {
                lbSubmit.Text = "修改失败！原因：" + error;
                return;
            }

        }
    }
}
