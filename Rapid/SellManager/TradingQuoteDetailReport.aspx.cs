using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.SellManager
{
    public partial class TradingQuoteDetailReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        private string Getsql()
        {
            string sql = string.Empty;
            string condition = " where 1=1";
            if (txtCustomerMaterialNumber.Text != "")
            {
                condition += " and 客户物料编号 like '%" + txtCustomerMaterialNumber.Text.Trim() + "%'";
            }
            if (txtCustomerName.Text != "")
            {
                condition += " and 客户名称 like '%" + txtCustomerName.Text.Trim() + "%'";
            }
            if (txtSupplierMaterialNumber.Text != "")
            {
                condition += "  and 供应商物料编号 like '%" + txtSupplierMaterialNumber.Text.Trim() + "%'";
            }
            if (txtQuoteUser.Text != "")
            {
                condition += "  and 报价人 like '%" + txtQuoteUser.Text.Trim() + "%'";
            }
            sql = string.Format(@"select * from V_TradingQuoteDetailReport_New {0}", condition);
            return sql;
        }
        private void Bind()
        {
            
            rpList.DataSource = SqlHelper.GetTable(Getsql());
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Getsql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(Getsql(), "贸易报价单报表");

        }
    }
}
