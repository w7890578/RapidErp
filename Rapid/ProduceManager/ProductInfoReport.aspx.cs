using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.ProduceManager
{
    public partial class ProductInfoReport : System.Web.UI.Page
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
            string condition = "where 1=1";
            if (txtProductNumber.Text != "")
            {
                condition += " and 产成品编号 like '%" + txtProductNumber.Text.Trim() + "%'";

            }
            if (txtVerison.Text != "")
            {
                condition += " and 版本 like '%" + txtVerison.Text.Trim() + "%'";

            }
            if (txtCustomerProductNumber.Text != "")
            {
                condition += " and 客户产成品编号 like '%" + txtCustomerProductNumber.Text.Trim() + "%'";

            }
            if (txtCustomerName.Text != "")
            {
                condition += " and 客户名称 like '%" + txtCustomerName.Text.Trim() + "%'";

            }
            if (txtDescription.Text != "")
            {
                condition += " and 描述 like '%" + txtDescription.Text.Trim() + "%'";

            }
            if (txtKind.Text != "")
            {
                condition += " and 种类 like '%" + txtKind.Text.Trim() + "%'";

            }
            if (txtType.Text != "")
            {
                condition += " and 成品类别 like '%" + txtType.Text.Trim() + "%'";

            }
            string sql = string.Format(@"select * from V_ProductInfoReport {0} order by 客户产成品编号 asc,版本 asc", condition);
            return sql;
        }

        private void Bind()
        {
            
            rpList.DataSource = SqlHelper.GetTable(GetSql());
            rpList.DataBind();
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
            ToolCode.Tool.ExpExcel(GetSql(), "产成品信息报表");
        }
    }
}
