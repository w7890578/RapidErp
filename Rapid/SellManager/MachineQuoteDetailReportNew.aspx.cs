using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.SellManager
{
    public partial class MachineQuoteDetailReportNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                Bind();
            }
        }
        private void Bind()
        {
            rpList.DataSource = SqlHelper.GetTable(GetSql());
            rpList.DataBind();
        }

        private string GetSql()
        {

            string condition = " where 1=1";
            string sql = string.Format(" select * from V_MachineQuoteDetail_Report ");
            string customerProductNubmer = txtCustomerProductNumber.Text.Trim();
            string customerName = txtCustomerName.Text.Trim();
            string BACmaterial = txtBACMaterial.Text.Trim();
            if (!string.IsNullOrEmpty(customerProductNubmer))
            {
                condition += string.Format(" and 客户产成品编号 like '%{0}%' ", customerProductNubmer);
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                condition += string.Format(" and 客户名称 like '%{0}%' ", customerName);
            }
            if (!string.IsNullOrEmpty(BACmaterial))
            {
                condition += string.Format(" and BAC物料号 like '%{0}%' ", BACmaterial);
            }
            if (!string.IsNullOrEmpty(txtContact.Text.Trim()))
            {
                condition += string.Format(" and 客户联系人 like '%{0}%' ", txtContact.Text.Trim());
            }
            if (!string.IsNullOrEmpty(txtDescrition.Text.Trim()))
            {
                condition += string.Format(" and 物料描述 like '%{0}%' ", txtDescrition.Text.Trim());
            }
            sql = string.Format(" {0} {1} order by 报价单号 desc ,客户产成品编号 desc,版本 desc,阶层  asc ", sql, condition);
            return sql;
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
            ToolCode.Tool.ExpExcel(GetSql(), "加工报价单明细报表");

        }
    }
}
