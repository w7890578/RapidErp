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
    public partial class EditAccountsReceive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Guid"))
                {
                    string guid = ToolManager.GetQueryString("Guid");

                    string sql = string.Format(@" select *  from [V_T_AccountsReceivableMain] 
where Guid ='{0}' ", guid);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        drpKP.SelectedValue = dr["是否已开发票"] == null ? "" : dr["是否已开发票"].ToString();
                        txtActualMakeCollectionsAmount.Text = dr["实际收款金额"] == null ? "" : dr["实际收款金额"].ToString();
                        //txtInvoiceNumber.Text = dr["实际收款金额"] == null ? "" : dr["实际收款金额"].ToString();
                        //txtInvoiceDate.Text = dr["实际收款金额"] == null ? "" : dr["实际收款金额"].ToString();
                        txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string kp = Request.Form["drpKP"].ToString();
            string actualmakecollectionsamunt = Request.Form["txtActualMakeCollectionsAmount"].ToString().Equals("") ? "0" : Request.Form["txtActualMakeCollectionsAmount"].ToString();
            string remark = Request.Form["txtRemark"].ToString();
            string guid = ToolManager.GetQueryString("Guid");
            string sql = string.Empty;
            string error = string.Empty;
            try
            {
                Convert.ToDouble(txtActualMakeCollectionsAmount);
               
            }
            catch (Exception ex)
            {
                lbSubmit.Text = "实际收款金额必须为整数或小数！";
                return;
            }
            sql = string.Format(@"update AccountsReceivable set ActualMakeCollectionsAmount='{0}',
IsInvoicing='{1}',Remark='{2}' where Guid='{3}'",
                actualmakecollectionsamunt,kp, remark, guid);
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
