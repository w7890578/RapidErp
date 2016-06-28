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
    public partial class EditPrepaidAccountsApplicationDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ToolManager.CheckQueryString("Guid"))
            {
                string guid = ToolManager.GetQueryString("Guid");
                string tableName = ToolManager.CheckQueryString("IsSK") ? @"(select va.*,ta.InvoiceNumber as 发票号码,ta.InvoiceDate as 开票日期 from V_AccountsReceivableDetail va inner join T_AccountsReceivable_Detail 
ta on va.guid=ta.guid)t" : "V_T_AccountsPayable_YF";
                string sql = string.Format(@" select *  from {1} 
where Guid ='{0}' ", guid, tableName);
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string remark = Request.Form["txtRemark"].ToString();
            string guid = ToolManager.GetQueryString("Guid");
            string fatherGuid = ToolManager.GetQueryString("fatherGuid");
            string invoiceNumber = Request.Form["txtInvoiceNumber"].ToString();
            string invoiceDate = Request.Form["txtInvoiceDate"].ToString();
            string sql = string.Empty;
            string error = string.Empty;
            List<string> sqls = new List<string>();

            // T_AccountsReceivable_Detail

            if (ToolManager.CheckQueryString("IsSK"))
            {
                sql = string.Format(@"update T_AccountsReceivable_Detail 
set Remark='{0}', 
InvoiceNumber='{2}',
InvoiceDate='{3}'
where Guid='{1}'", remark, guid, invoiceNumber, invoiceDate);
                SqlHelper.ExecuteSql(sql);
                sql = string.Format(@"select IsAdvance  from AccountsReceivable where guid='{0}'", fatherGuid);
                if (SqlHelper.GetScalar(sql).Equals("否"))
                {
                    string tempText = "";
                    string sqlTemp = string.Format(@"
select distinct tad.InvoiceNumber from AccountsReceivable ar inner join T_AccountsReceivable_Detail
tad on ar.CreateTime =tad.CreateTime and ar.OrdersNumber =tad.OrdersNumber 
where ar.guid ='{0}'", fatherGuid);
                    foreach (DataRow dr in SqlHelper.GetTable(sqlTemp).Rows) //找
                    {
                        if (!tempText.Contains(dr["InvoiceNumber"].ToString()))
                        {
                            tempText += "," + dr["InvoiceNumber"].ToString();
                        }
                    }
                    tempText = tempText.TrimStart(',');

                    sql = string.Format(@"update AccountsReceivable 
                set InvoiceNumber='{0}'  ,
                InvoiceDate ='{2}' where guid ='{1}'", tempText, fatherGuid, invoiceDate);
                    sqls.Add(sql);
                }
                else
                {
                    string tempText = "";
                    string sqlTemp = string.Format(@"
select distinct tad.InvoiceNumber from AccountsReceivable ar inner join T_AccountsReceivable_Detail
tad on  ar.OrdersNumber =tad.OrdersNumber 
where ar.guid ='{0}'", fatherGuid);
                    foreach (DataRow dr in SqlHelper.GetTable(sqlTemp).Rows) //找
                    {
                        if (!tempText.Contains(dr["InvoiceNumber"].ToString()))
                        {
                            tempText += "," + dr["InvoiceNumber"].ToString();
                        }
                    }
                    tempText = tempText.TrimStart(',');

                    sql = string.Format(@"update AccountsReceivable 
                set InvoiceNumber='{0}'  ,
                InvoiceDate ='{2}' where guid ='{1}'", tempText, fatherGuid, invoiceDate);
                    sqls.Add(sql);
                }
            }
            else
            {
                sql = string.Format(@"update T_AccountsPayable_Detail 
set Remark='{0}', 
InvoiceNumber='{2}',
BillingDate='{3}'
where Guid='{1}'", remark, guid, invoiceNumber, invoiceDate);
                //SqlHelper.ExecuteSql(sql);
                sqls.Add(sql);
                sql = string.Format(@"
update T_AccountsPayable_Main set InvoiceDate='{0}' where guid='{1}'                    
                    ", invoiceDate, Request["fatherguid"]);
                sqls.Add(sql);
                //                sql = string.Format(@" 
                //update T_AccountsPayable_Main set InvoiceNumber=InvoiceNumber+',{0}'
                //where guid='{1}' and InvoiceNumber not like '%{0}%'", invoiceNumber, Request["fatherguid"]);
                //                sqls.Add(sql); 

            }
            if (SqlHelper.BatchExecuteSql(sqls, ref error))
            {
                try
                {
                    sql = string.Format(@"
select distinct 发票号码 from V_T_AccountsPayable_YF where 采购订单号 = (
select OrdersNumber from T_AccountsPayable_Main where guid = '{0}')", Request["fatherguid"]);
                    DataTable dttemps = SqlHelper.GetTable(sql);
                    if (dttemps != null && dttemps.Rows.Count > 0)
                    {
                        string tempstrs = string.Empty;
                        foreach (DataRow item in dttemps.Rows)
                        {
                            tempstrs += "," + item["发票号码"];
                        }
                        tempstrs = tempstrs.TrimStart(',');
                        sql = string.Format(@"
update T_AccountsPayable_Main set InvoiceNumber='{0}' where guid='{1}'",tempstrs,Request["fatherguid"]);
                        SqlHelper.ExecuteSql(sql);
                    }

                }
                catch { }
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
