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
    public partial class EditAccountPayDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Guid"))
                {
                    string guid = ToolManager.GetQueryString("Guid");

                    string sql = string.Format(@" select *  from V_AccountsPayDetail 
where Guid ='{0}' ", guid);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtInvoiceNumber.Text = dr["发票号码"] == null ? "" : dr["发票号码"].ToString();
                        txtInvoiceDate.Text = dr["开票日期"] == null ? "" : dr["开票日期"].ToString();
                        txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string remark = Request.Form["txtRemark"].ToString();
            string invoiceNumber = Request.Form["txtInvoiceNumber"].ToString();
            string invoiceDate = Request.Form["txtInvoiceDate"].ToString();
            string guid = ToolManager.GetQueryString("Guid");
            string fatherGuid = ToolManager.GetQueryString("fatherGuid");
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"update T_AccountsPayable_Detail 
set Remark='{0}',
invoiceNumber='{2}',
BillingDate='{3}' 
where Guid='{1}'",
               remark, guid, invoiceNumber, invoiceDate);
            SqlHelper.ExecuteSql(sql);
            List<string> sqls = new List<string>();
            string tempText = "";
            string sqlTemp = string.Format(@"
select distinct tpd.InvoiceNumber  from T_AccountsPayable_Main tam inner join 
T_AccountsPayable_Detail tpd on tam.OrdersNumber =tpd.PurchaseOrderNumber
and tam.CreateTime =tpd.CreateTime 
and tam.IsPrepaid='否'
and tam.guid='{0}'", fatherGuid);
            foreach (DataRow dr in SqlHelper.GetTable(sqlTemp).Rows) //找
            {
                if (!tempText.Contains(dr["InvoiceNumber"].ToString()))
                {
                    tempText += "," + dr["InvoiceNumber"].ToString();
                }
            }
            tempText = tempText.TrimStart(',');

            sql = string.Format(@"update T_AccountsPayable_Main 
                set InvoiceNumber='{0}'  ,
                InvoiceDate ='{2}' where guid ='{1}'", tempText, fatherGuid, invoiceDate);
            sqls.Add(sql);

            if (SqlHelper.BatchExecuteSql(sqls, ref error))
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
