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
    public partial class SupplyMaterialInfoReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Bind();
            }
        }

        private string GetSql()
        {
            string conditon = " where 1=1";
            if (txtMaterialNumber.Text != "")
            {
                conditon += " and 原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            if (txtSupplyName.Text != "")
            {
                conditon += " and 供应商名称 like '%" + txtSupplyName.Text.Trim() + "%'";
            }
            if (txtDescription.Text != "")
            {
                conditon += " and 描述 like '%" + txtDescription.Text.Trim() + "%'";
            }
            if (txtSupplierMaterialNumber.Text != "")
            {
                conditon += " and 供应商物料编号 like '%" + txtSupplierMaterialNumber.Text.Trim() + "%'";
            }
            if (txtXH.Text != "")
            {
                conditon += " and 型号 like '%" + txtXH.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from V_SupplyMaterialInfoReport {0}", conditon);
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
            if(string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(),"供应商物料信息");

        }
    }
}
