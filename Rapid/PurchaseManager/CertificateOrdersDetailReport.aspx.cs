using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.PurchaseManager
{
    public partial class CertificateOrdersDetailReport : System.Web.UI.Page
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
            string conditon = " where 1=1";
            if (txtCutomerMaterialNumber.Text != "")
            {
                conditon += " and 客户物料编号 like '%" + txtCutomerMaterialNumber.Text + "%'";
            }
            if (txtSupplyName.Text != "")
            {
                conditon += " and 供应商名称 like '%" + txtSupplyName.Text + "%'";
            }
            string sql = string.Format(@"select * from V_CertificateOrdersDetailReport {0}", conditon);
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

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(), "采购明细报表");

        }
    }
}
