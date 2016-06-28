using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class ManufactureProdudctStockMoney : System.Web.UI.Page
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
            string conection = "where 1=1";
            if (txtProductnumber.Text != "")
            {
                conection = "and 产成品编号 like '%" + txtProductnumber.Text + "%'";
            }
            string sql = string.Format(@"
select * from V_WorkInProgress_Price {0}", conection);
            return sql;
        }

        private void Bind()
        {
            this.rpList.DataSource = SqlHelper.GetTable(GetSql());
            this.rpList.DataBind();

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
            ToolCode.Tool.ExpExcel(GetSql(), "在制品库存金额报表");
        }
    }
}
