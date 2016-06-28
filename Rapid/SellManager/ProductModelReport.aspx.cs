using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class ProductModelReport : System.Web.UI.Page
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
            string condition = "WHERE 1=1";
            if (txtNumber.Text != "")
            {
                condition += " and 客户物料号 like '%" + txtNumber.Text.Trim() + "%'";
            }
            if (ddlYear .SelectedValue != "")
            {
                condition += " and 年度 = '" +ddlYear .SelectedValue + "'";
            }
            if (txtProductNumber.Text != "")
            {
                condition += " and 产成品编号 like '%" + txtProductNumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from V_ProductModelReport {0}", condition);
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
            ToolCode.Tool.ExpExcel(GetSql(), "产品型号统计报表");
        }
    }
}