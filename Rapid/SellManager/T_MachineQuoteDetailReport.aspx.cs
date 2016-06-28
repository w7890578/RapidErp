using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.SellManager
{
    public partial class T_MachineQuoteDetailReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                Bind();
            }
        }

        private void Bind()
        {
            rpList.DataSource = SqlHelper.GetTable(GetSql());
            rpList.DataBind();
        }

        private string GetSql()
        {
            
            string condition = " where 1=1 ";
            string sql = string.Format(" select * from V_MachineQuoteDetail_Report_ForNewPrice ");
            string customerProductNubmer = txtCustomerProductNumber.Text.Trim();
            string QuoteUser = txtQuoteUser.Text.Trim();
            string customerName = txtCustomerName.Text.Trim();
          
            if (!string.IsNullOrEmpty(customerProductNubmer))
            {
                condition += string.Format(" and 客户产成品编号 like '%{0}%' ", customerProductNubmer);
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                condition += string.Format(" and 客户名称 like '%{0}%' ", customerName);
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                condition += string.Format(" and 客户名称 like '%{0}%' ", customerName);
            }
            if (!string.IsNullOrEmpty(QuoteUser))
            {
                condition += string.Format(" and 报价人 like '%{0}%' ", QuoteUser);
            }
            sql = string.Format(" {0} {1} order by 报价单号 desc ,客户产成品编号 desc,版本 desc,阶层  asc ", sql, condition);
            return sql;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(), "加工报价单报表");
        }
    }
}
