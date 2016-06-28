using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rapid.ToolCode;
using BLL;
using DAL;
using Model;
using System.Data;

namespace Rapid.FinancialManager
{
    public partial class ImpInvoiceAccountInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
            if (ds == null)
            {
                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
                return;
            }
            List<InvoiceAccountInfo> invoiceaccountss = new List<InvoiceAccountInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                InvoiceAccountInfo invoiceaccount = new InvoiceAccountInfo(); 
                invoiceaccount.SN = dr["序号"] == null ? "" : dr["序号"].ToString();
                invoiceaccount.InvoiceCode = dr["发票代码"] == null ? "" : dr["发票代码"].ToString();
                invoiceaccount.CustomerId = dr["客户名称"] == null ? "" : dr["客户名称"].ToString();
                invoiceaccount.InvoiceNumber = dr["发票号码"] == null ? "" : dr["发票号码"].ToString();
                invoiceaccount.InvoiceDate= dr["开票日期"] == null ? "" :  dr["开票日期"].ToString();
                invoiceaccount.Money = dr["不含税金额"] == null ? "" : dr["不含税金额"].ToString();
                invoiceaccount.Money_C= dr["税额"] == null ? "" : dr["税额"].ToString();
                invoiceaccount.InvoiceMoney = dr["发票金额"] == null ? "" : dr["发票金额"].ToString();
                invoiceaccount.IsPay = dr["是否已收款"] == null ? "" : dr["是否已收款"].ToString();
                invoiceaccount.InvoiceType = dr["发票类型"] == null ? "" : dr["发票类型"].ToString();
                invoiceaccountss.Add(invoiceaccount);
            }
            bool restult =BatchAddData(invoiceaccountss, ref error);
            lbMsg.Text = restult == true ? "导入成功！" : error;
            if (restult)
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入发票信息", "导入成功！");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入发票信息", "导入失败！原因" + error);
                return;
            }

        }
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="users"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BatchAddData(List<InvoiceAccountInfo> invoiceaccounts, ref string error)
        {
            int i = 0;
            string tempError = string.Empty;
            if (invoiceaccounts.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (InvoiceAccountInfo invoiceaccount in invoiceaccounts)
            {
                tempError = "";
                if (!AddInvoice(invoiceaccount, ref tempError))
                {
                    i++;
                    error += string.Format("发票{0}&nbsp;&nbsp;添加失败：原因--{1}<br/>", invoiceaccount.InvoiceNumber, tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (invoiceaccounts.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }
        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddInvoice(InvoiceAccountInfo invoiceaccount, ref string error)
        {
            string sql = string.Empty;
            try { 
                invoiceaccount .InvoiceDate=Convert .ToDateTime (invoiceaccount .InvoiceDate).ToString ("yyyy-MM-dd");
            }
            catch (Exception ex){
                error = string.Format ("开票日期{0},不是正规的日期格式yyyy-MM-dd",invoiceaccount .InvoiceDate);
                return false;
            }

            if (string.IsNullOrEmpty(invoiceaccount.InvoiceNumber) || string.IsNullOrEmpty(invoiceaccount.InvoiceCode))
            {
                error = "发信息不完整！";
                return false;
            }
            sql = string.Format(@"select count(*) from Customer where CustomerName='{0}'", invoiceaccount.CustomerId);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("不存在客户名称{0}", invoiceaccount.CustomerId);
                return false;
            }
            sql = string.Format(@" insert into InvoiceAccountInfo (SN,InvoiceNumber,InvoiceDate,
InvoiceCode,InvoiceMoney,CustomerId,Money,Money_C,
IsPay,InvoiceType) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ",invoiceaccount.SN,
invoiceaccount.InvoiceNumber,invoiceaccount.InvoiceDate,invoiceaccount.InvoiceCode,invoiceaccount.InvoiceMoney,
invoiceaccount.CustomerId,invoiceaccount.Money,invoiceaccount.Money_C,invoiceaccount.IsPay,invoiceaccount.InvoiceType);
            return SqlHelper.ExecuteSql(sql, ref error);

        }

    }
}
