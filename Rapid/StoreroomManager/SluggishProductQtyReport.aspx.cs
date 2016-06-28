using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class SluggishProductQtyReport : System.Web.UI.Page
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
            string condition = " where 1=1";
           
            if (txtProductNumber.Text != "")
            {
                condition += " and  产成品编号 like '%" + txtProductNumber.Text.Trim() + "%'";
            }
            if (txtCustomerProductNumber.Text != "")
            {
                condition += " and 客户产成品编号 like '%" + txtCustomerProductNumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from V_SluggishProductQty_New {0}", condition);
            return sql;
        }

        private void Bind()
        {
            rpList.DataSource = SqlHelper.GetTable(GetSql());
            rpList.DataBind();
        }

        protected void btnExcel_Click1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(), "呆滞产成品报表");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
