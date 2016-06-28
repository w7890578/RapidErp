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
    public partial class SluggishMaterialQtyReport : System.Web.UI.Page
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
           
            if (txtMaterialNumber.Text != "")
            {
                condition += " and  原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            if (txtSupplierNumber.Text != "")
            {
                condition += " and  供应商编号 like '%" + txtSupplierNumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from V_SluggishMaterialQty_New {0}", condition);
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

        protected void btnExcel_Click1(object sender, EventArgs e)
        {
           
            if (string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(), "呆滞原材料报表");
        }
    }
}
