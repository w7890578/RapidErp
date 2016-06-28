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
    public partial class CertificateDemandReport : System.Web.UI.Page
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
            string condition = "where 1=1";
            if (txtHDnumber.Text != "")
            {
                condition += " and 合同号 like '%" + txtHDnumber.Text.Trim() + "%'";
            }
            if (txtOrderDate.Text != "")
            {
                condition += " and  采购日期='" + txtOrderDate.Text + "'";
            }
            if (txtOrdersNumber.Text != "")
            {
                condition += " and 采购订单号 like '%" + txtOrdersNumber.Text.Trim() + "%'";
            }
            if (txtSupplierMateialNumber.Text != "")
            {
                condition += " and 供应商物料编号 like '%" + txtSupplierMateialNumber.Text.Trim() + "%'";
            }
            if (txtSupplierName.Text != "")
            {
                condition += " and 供应商名称 like '%" + txtSupplierName.Text.Trim() + "%'";
            }
            if (txtRemark.Text != "")
            {
                condition += " and 备注 like '%" + txtRemark.Text.Trim() + "%'";
            }
            if (txtMaterialNumber.Text != "")
            {
                condition += " and 原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from V_CertificateDemandReport {0}
union all
select '','合计','','','','','',
sum(库存数量),sum(在途数量),
sum(安全用量),SUM(实际订单数量),
SUM(计算结果),'','',0,
SUM(总价),'' from V_CertificateDemandReport {0}", condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
