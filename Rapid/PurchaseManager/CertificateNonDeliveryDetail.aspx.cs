using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.PurchaseManager
{
    public partial class CertificateNonDeliveryDetail : System.Web.UI.Page
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
            string conditon = "where 1=1";
            if (txtOrdersNumber.Text != "")
            {
                conditon += " and 采购订单编号 like '%" + txtOrdersNumber.Text.Trim() + "%'";
            }
            if (txtMaterialNumber.Text != "")
            {
                conditon += " and 原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            if (txtSupplierNumber.Text != "")
            {
                conditon += " and 供应商物料编号 like '%" + txtSupplierNumber.Text.Trim() + "%'";
            }
            if (txtDescription.Text != "")
            {
                conditon += " and 描述 like '%" + txtDescription.Text.Trim() + "%'";
            }
            if (txtName.Text != "")
            {
                conditon += " and 供应商名称 like '%" + txtName.Text.Trim() + "%'";
            }
            if (txtNumber.Text != "")
            {
                conditon += " and 运输号 like '%" + txtNumber.Text.Trim() + "%'";
            }
            if (txtPay.Text != "")
            {
                conditon += " and 付款 like '%" + txtPay.Text.Trim() + "%'";
            }
            if (txtRemark.Text != "")
            {
                conditon += " and 备注 like '%" + txtRemark.Text.Trim() + "%'";
            }
            string sql = string.Format(@"SELECT * FROM V_CertificateNonDeliveryDetail {0}
union all
select '合计','','','','','','',SUM(采购数量),
SUM(未交数量),SUM(已交数量),0,SUM(总价),
'','','',''
 from [V_CertificateNonDeliveryDetail] {0}", conditon);
            return sql;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnExp_Click(object sender, EventArgs e)
        {
            ToolCode.Tool.ExpExcel(GetSql(), "采购未交全明细表");
        }
    }
}