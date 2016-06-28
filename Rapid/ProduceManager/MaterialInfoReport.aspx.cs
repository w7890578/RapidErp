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
    public partial class MaterialInfoReport : System.Web.UI.Page
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
                condition += " and 原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            if (txtCustomerMateialNumber.Text != "")
            {
                condition += " and  客户物料编号 like '%" + txtCustomerMateialNumber.Text.Trim() + "%'";
            }
            if (txtCustomerName.Text != "")
            {
                condition += " and 客户名称 like '%" + txtCustomerName.Text.Trim() + "%'";
            }
            if (txtSupplierMaterialNumber.Text != "")
            {
                condition += " and  供应商物料编号 like '%" + txtSupplierMaterialNumber.Text.Trim() + "%'";
            }
            if (txtSupplierName.Text != "")
            {
                condition += " and 供应商名称 like '%" + txtSupplierName.Text.Trim() + "%'";
            }
            if (txtDescription.Text != "")
            {
                condition += " and  描述 like '%" + txtDescription.Text.Trim() + "%'";
            }
            if (txtKind.Text != "")
            {
                condition += " and 种类 like '%" + txtKind.Text.Trim() + "%'";
            }
            if (txtType.Text != "")
            {
                condition += " and  类别 like '%" + txtType.Text.Trim() + "%'";
            }
            if (txtHWtype.Text!= "")
            {
                condition += " and  货物类型 like '%" + txtHWtype.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from V_MaterialInfoReport {0}", condition);
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
            ToolCode.Tool.ExpExcel(GetSql(), "原材料信息报表");
        }
    }
}
