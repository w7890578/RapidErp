using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.SellManager
{
    public partial class EditPrepaidAdvanceApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ToolManager.CheckQueryString("Guid"))
            {
                string guid = ToolManager.GetQueryString("Guid");


                string sql = string.Format(@" select vt.*,a.InvoiceNumber as 发票号码,a.InvoiceDate as 开票日期  from V_T_AccountsReceivableMain vt
inner join AccountsReceivable a  on vt.guid=a.guid
                where vt.Guid ='{0}' ", guid);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    drpJQ.SelectedValue = dr["是否结清"] == null ? "" : dr["是否结清"].ToString();
                    txtInvoiceNumber.Text = dr["发票号码"] == null ? "" : dr["发票号码"].ToString();
                    txtInvoiceDate.Text = dr["开票日期"] == null ? "" : dr["开票日期"].ToString();
                    txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string guid = ToolManager.GetQueryString("Guid");
            string sql = string.Format(@"update AccountsReceivable set  IsSettle='{0}',Remark='{1}' 
,InvoiceNumber='{3}',InvoiceDate='{4}'
where guid='{2}'", Request.Form["drpJQ"], Request.Form["txtRemark"], guid, Request.Form["txtInvoiceNumber"], Request.Form["txtInvoiceDate"]);
            string error = string.Empty;
            lbSubmit.Text = SqlHelper.ExecuteSql(sql, ref error) ? "修改成功" : "修改失败！原因：" + error;
            return;
        }
    }
}
