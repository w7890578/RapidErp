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
    public partial class PrepaidAccountStatisticsReport : System.Web.UI.Page
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
            string condition = " WHERE 是否为预付='是'";
            if (txtHDnumber.Text != "")
            {
                condition += " and 采购合同号 like '%" + txtHDnumber.Text.Trim()+ "%'";
            }
            if (txtMaterialNumber.Text != "")
            {
                condition += " and 原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            if (txtOdersNumber.Text != "")
            {
                condition += " and 采购订单号 like '%" + txtOdersNumber.Text.Trim() + "%'";
            }
            if (txtSupplierMaterialNumber.Text != "")
            {
                condition += " and 供应商物料编号 like '%" + txtSupplierMaterialNumber.Text.Trim() + "%'";
            }
            if (txtSupplierName.Text != "")
            {
                condition += " and 供应商名称 like '%" + txtSupplierName.Text.Trim() + "%'";
            }
            if (drpJQ.SelectedValue != "")
            {
                condition+=" and 是否结清='"+drpJQ.SelectedValue+"'";
            }
            string sql = string.Format(@" select * from V_PrepaidAccountStatisticsReport {0}
union all
select '合计','','','','','','','','','','','','',SUM(采购数量),SUM(到货数量),0,SUM(总价),'','','','','' from V_PrepaidAccountStatisticsReport {0}", condition);
            return sql;
        }

        private void Bind()
        {
            string sql = GetSql();
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string sql = GetSql();
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "预付账款统计报表");
        }
    }
}
