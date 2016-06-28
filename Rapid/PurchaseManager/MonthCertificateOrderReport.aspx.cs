using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.PurchaseManager
{
    public partial class MonthCertificateOrderReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                Bind();
            }
        }

        private string GetSql()
        {
            string condition = " where 1=1";
            if (drpYear.SelectedValue != "")
            {
                condition+=" and 年度='"+drpYear.SelectedValue+"'";
            }
            if (txtMaterialNumber.Text != "")
            {
                condition += " and 原材料编号 like'%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            if (txtSupplierMaterialNumber.Text != "")
            {
                condition += " and 供应商物料编号 like'%" + txtSupplierMaterialNumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from [V_MonthCertificateOrderReport] {0}", condition);
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
            ToolCode.Tool.ExpExcel(sql, "采购已入库统计报表");
        }
    }
}
