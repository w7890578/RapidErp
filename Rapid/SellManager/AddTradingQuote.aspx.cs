using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class AddTradingQuote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                lbQuoteNumber.Text = ToolManager.GetQueryString("QuoteNumber");
                if (ToolManager.CheckQueryString("m"))
                {
                    string result = string.Empty;
                    string sql = string.Format(@" select top 20  MaterialName ,MaterialNumber from MarerialInfoTable ");
                    if (ToolManager.CheckQueryString("contion"))
                    {
                        sql += string.Format(@"  where
MaterialName like '%{0}%' or MaterialName like'%{0}' or MaterialName like '{0}%' or
MaterialNumber like '%{0}%' or MaterialNumber like'%{0}' or MaterialNumber like '{0}%' 
order by MaterialNumber asc", ToolManager.GetQueryString("contion"));
                    }
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            result += string.Format(" <tr><td>{0}</td><td>{1}</td></tr>", dr["MaterialName"], dr["MaterialNumber"]);
                        }
                    }

                    Response.Write(result);
                    Response.End();
                    return;
                }
                if (ToolManager.CheckQueryString("mareialNumber"))
                {
                    string materialNumber = ToolManager.GetQueryString("mareialNumber");
                    if (ToolManager.CheckQueryString("OdersNumber"))
                    { 
                        string odersNumber = ToolManager.GetQueryString("OdersNumber");
                        string sql = string.Format(@" select CustomerMaterialNumber from MaterialCustomerProperty where MaterialNumber='{0}' and  CustomerId=( select CustomerId from SaleOder where OdersNumber='{1}')", materialNumber, odersNumber);
                        Response.Write(SqlHelper.GetScalar(sql));
                        Response.End();
                        return;
                    }
                    else if (ToolManager.CheckQueryString("QuoteNumber"))
                    {
                        string QuoteNumber = ToolManager.GetQueryString("QuoteNumber");
                        string sql = string.Format(@"  select CustomerMaterialNumber from MaterialCustomerProperty where MaterialNumber='{0}' and CustomerId=(select CustomerId from  QuoteInfo where  QuoteNumber='{1}')", materialNumber, QuoteNumber);
                        Response.Write(SqlHelper.GetScalar(sql));
                        Response.End();
                        return;
                    }
                 
                }
            }
        }

        private void BindCustomer(string materialNumber, string customerMaterialNumber)
        {
                 this.txtCustomerMarerial.Text = customerMaterialNumber;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ToolManager.CheckQueryString("QuoteNumber"))
            {
                Response.Write("未知报价单");
                Response.End();
                return;
            }
            string error = string.Empty;
            string sql = string.Empty;
            string quoteNumber = ToolManager.GetQueryString("QuoteNumber");
            string sn = txtSN.Text.Trim();
            string marerialNumber = txtMarerial.Text.Trim();
            string customerMarerialNumber = Request.Form["txtCustomerMarerial"].ToString();
            string fixedLeadTime = txtFixedLeadTime.Text.Trim();
            string remark = txtRemark.Text;
            bool result = false;
            sql = string.Format(@" 
 select COUNT (*) from TradingQuoteDetail where QuoteNumber='{0}' and SN ='{1}' ", quoteNumber, sn);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                lbMsg.Text = "已存在该序号";
                BindCustomer(marerialNumber, customerMarerialNumber);
                return;
            }
            sql = string.Format(@"
select count(*) from MarerialInfoTable where MaterialNumber='{0}'", marerialNumber);
            if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                lbMsg.Text = "该原材料编号不存在！";
                return;
            }
            sql = string.Format(@" insert into TradingQuoteDetail
(QuoteNumber ,SN,ProductNumber ,CustomerMaterialNumber,UnitPrice ,MinPackage,MinMOQ,FixedLeadTime
  ,Remark ,MaterialDescription,Brand)  
 select '{0}',{1},MaterialNumber,'{2}',ProcurementPrice,MinPacking,MinOrderQty,'{3}' ,'{4}',Description,Brand
 from MarerialInfoTable where MaterialNumber='{5}'", quoteNumber, sn, customerMarerialNumber,
  fixedLeadTime, remark, marerialNumber);
            result = SqlHelper.ExecuteSql(sql, ref error);
            lbMsg.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加贸易报价单明细" + quoteNumber, "增加成功");
                ToolCode.Tool.ResetControl(this.Controls);
                lbQuoteNumber.Text = ToolManager.GetQueryString("QuoteNumber");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加贸易报价单明细" + quoteNumber, "增加失败");
                BindCustomer(marerialNumber, customerMarerialNumber);
                lbQuoteNumber.Text = ToolManager.GetQueryString("QuoteNumber");
                return;
            }

        }
    }
}
