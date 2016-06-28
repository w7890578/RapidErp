using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;


namespace Rapid.FinancialManager
{
    public partial class AccountPayStatisticsReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }

        }

        private string GetSql()
        {
            string condition = " WHERE 是否为预付='否'";
            if (txtNumber.Text != "")
            {
                condition += " and 采购订单号 like '%" + txtNumber.Text.Trim() + "%'";
            }
            if (txtContractNumber.Text != "")
            {
                condition += " and 采购合同号 like '%" + txtContractNumber.Text.Trim() + "%'";
            }
            if (txtMaterialNumber.Text != "")
            {
                condition += " and 原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            if (txtSupplierNumber.Text != "")
            {
                condition += " and 供应商物料编号 like '%" + txtSupplierNumber.Text.Trim() + "%'";
            }
            if (txtSupplierName.Text != "")
            {
                condition += " and 供应商名称 like '%" + txtSupplierName.Text.Trim() + "%'";
            }
            if (txtInvoiceNumber.Text != "")
            {
                condition += " and 发票号码 like '%" + txtInvoiceNumber.Text.Trim() + "%'";
            }
            if (ddlIsSettlement.SelectedValue != "")
            {
                condition += " and 是否结清 = '" + ddlIsSettlement.SelectedValue + "'";
            }
            string sql = string.Format(@" select * from V_AccountPayStatisticsReport {0}
union all
 select '合计','','','','','','','',sum(采购数量),sum(到货数量),'',sum(总价),
 '','','','','','','',''
  from V_AccountPayStatisticsReport {0}",condition);
             return sql;

        }

        private void Bind()
        {
            this.rptList.DataSource = SqlHelper.GetTable(GetSql());
            this.rptList.DataBind();
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
            ToolCode.Tool.ExpExcel(GetSql(), "应付账款统计表");
        }
    }
}