using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class EditTradingQuote : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (ToolManager.CheckQueryString("sn"))
                {
                    
                    string error = string.Empty;
                    string guid =Server.UrlDecode( ToolManager.GetQueryString("Guid"));
                    string quoteNumber = ToolManager.GetQueryString("QuoteNumber");
                    string sql = string.Format("select * from TradingQuoteDetail where QuoteNumber ='{0}' and SN='{1}' and Guid='{2}'", quoteNumber, ToolManager.GetQueryString("sn"),guid);
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    DataRow dr = dt.Rows[0];
                    txtSN.Text = dr["SN"] == null ? "" : dr["SN"].ToString();
                    lbMarerial.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                    lbBrand.Text = dr["Brand"] == null ? "" : dr["Brand"].ToString();
                    txtUnitPrice.Text = dr["UnitPrice"] == null ? "" : dr["UnitPrice"].ToString();
                    txtMinPackage.Text = dr["MinPackage"] == null ? "" : dr["MinPackage"].ToString();
                    txtMinMOQ.Text = dr["MinMOQ"] == null ? "" : dr["MinMOQ"].ToString();
                    txtFixedLeadTime.Text = dr["FixedLeadTime"] == null ? "" : dr["FixedLeadTime"].ToString();
                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    lbCustomerMarerial.Text = dr["CustomerMaterialNumber"] == null ? "" : dr["CustomerMaterialNumber"].ToString();
                    lbDescription.Text = dr["MaterialDescription"] == null ? "" : dr["MaterialDescription"].ToString();
                    btnSubmit.Text = "修改";
                }
                else
                {
                    btnSubmit.Text = "添加";
                }
            }
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ToolManager.CheckQueryString("QuoteNumber"))
            {
                Response.Write("未知订单");
                Response.End();
            }
            string quoteNumber = ToolManager.GetQueryString("QuoteNumber");
            string error = string.Empty;
            string sql = string.Empty;
            string sn = txtSN.Text.Trim();
            string productNumber = lbMarerial.Text;
            string customerProductNumber = lbCustomerMarerial.Text;
            string unitPrice = txtUnitPrice.Text.Trim();
            string minPackage = txtMinPackage.Text.Trim();
            string minMOQ = txtMinMOQ.Text.Trim();
            string fixedLeadTime = txtFixedLeadTime.Text.Trim();
            string remark = txtRemark.Text;
            string getSN = ToolManager.GetQueryString("sn");
            string guid =Server.UrlDecode( ToolManager.GetQueryString("Guid"));
            sql = string.Format(@" 
select COUNT (*) from TradingQuoteDetail where   SN={0} and QuoteNumber='{1}' and SN !={2} and Guid='{3}'", sn, quoteNumber, getSN,guid);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                lbSubmit.Text = "该序号已存在，请重新填写！";
                return;
            }
            sql = string.Format(@"update TradingQuoteDetail set 

UnitPrice ='{0}',MinPackage ='{1}',MinMOQ ='{2}',
FixedLeadTime ='{3}',Remark ='{4}',SN={7} where QuoteNumber ='{5}' and SN ='{6}' and Guid='{8}' ", unitPrice, minPackage
, minMOQ, fixedLeadTime, remark, quoteNumber, getSN, sn,guid);
            bool result=SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text=result==true?"修改成功":"修改失败！原因"+error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑贸易报价单" + quoteNumber, "编辑成功");
                Response.Write(ToolManager.GetClosePageJS());
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑贸易报价单" + quoteNumber, "编辑失败！原因"+error);
                return;
            }

        }
    }
}
