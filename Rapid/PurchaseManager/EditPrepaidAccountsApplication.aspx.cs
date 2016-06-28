using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.PurchaseManager
{
    public partial class EditPrepaidAccountsApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Guid"))
                {
                    string guid = ToolManager.GetQueryString("Guid");

                    string sql = string.Format(@" select *  from V_T_AccountsPayable_Main_ForYF 
where Guid ='{0}' ", guid);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtYFOneDate.Text = dr["预付一实际付款日期"] == null ? "" : dr["预付一实际付款日期"].ToString();
                        txtYFTwoDate.Text = dr["预付二实际付款日期"] == null ? "" : dr["预付二实际付款日期"].ToString();
                        drpJQ.SelectedValue = dr["是否结清"] == null ? "" : dr["是否结清"].ToString();
                        txtInvoiceNumber.Text = dr["发票号码"] == null ? "" : dr["发票号码"].ToString();
                        txtInvoiceDate.Text = dr["开票日期"] == null ? "" : dr["开票日期"].ToString();
                        txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                        txtYFOne.Text = dr["预付一"] == null ? "" : dr["预付一"].ToString();
                        txtYFTwo.Text = dr["预付二"] == null ? "" : dr["预付二"].ToString();

                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string yfonedate = Request.Form["txtYFOneDate"].ToString();
            string yftwodate = Request.Form["txtYFTwoDate"].ToString();
            string invoiceNumber = Request.Form["txtInvoiceNumber"].ToString();
            string invoiceDate = Request.Form["txtInvoiceDate"].ToString();
            string yfone = Request.Form["txtYFOne"].ToString();
            string yftwo = Request.Form["txtYFTwo"].ToString();
            string jq = Request.Form["drpJQ"].ToString();
            string remark = Request.Form["txtRemark"].ToString();
            string guid = ToolManager.GetQueryString("Guid");
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"update T_AccountsPayable_Main set 
YFOneDate='{0}',YFTwoDate='{1}',IsSettle='{2}',Remark='{3}' 
,invoiceNumber='{5}',invoiceDate='{6}',YFOne={7},YFTwo={8}
where Guid='{4}'",
                yfonedate, yftwodate, jq, remark, guid, invoiceNumber, invoiceDate, yfone, yftwo);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                lbSubmit.Text = "修改成功！";
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
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
