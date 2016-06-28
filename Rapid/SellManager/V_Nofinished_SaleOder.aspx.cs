using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.SellManager
{
    public partial class V_Nofinished_SaleOder : System.Web.UI.Page
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
            if (txtOrdersNumber.Text != "")
            {
                condition += " and 销售订单号 like '%" + txtOrdersNumber.Text.Trim() + "%'";
            }
            if (txtCGNomber.Text != "")
            {
                condition += " and 客户采购订单号 like '%" + txtCGNomber.Text.Trim() + "%'";
            }
            if (txtCustomerName.Text != "")
            {
                condition += " and 客户 like '%" + txtCustomerName.Text.Trim() + "%'";
            }
            if (txtCustomerProductNumber.Text != "")
            {
                condition += " and 客户产成品编号 like '%" + txtCustomerProductNumber.Text.Trim() + "%'";
            }
            if (txtProductNumber.Text != "")
            {
                condition += " and 产成品编号 like '%" + txtProductNumber.Text.Trim() + "%'";
            }
            if (txtStatus.Text != "")
            {
                condition += " and 订单状态 like '%" + txtStatus.Text.Trim() + "%'";
            }
            if (txtOrdersDate.Text != "")
            {
                condition += " and 订单日期 like '%" + txtOrdersDate.Text.Trim() + "%'";
            }
            if (txtYW.Text != "")
            {
                condition += " and 业务员 like '%" + txtYW.Text.Trim() + "%'";
            }
            if (txtType.Text != "")
            {
                condition += " and 订单类型 like '%" + txtType.Text.Trim() + "%'";
            }
            if (txtProductType.Text != "")
            {
                condition += " and 生产类型 like '%" + txtProductType.Text.Trim() + "%'";
            }
            if (txtProjectName.Text != "")
            {
                condition += " and 项目 like '%" + txtProjectName.Text.Trim() + "%'";
            }
            if (txtLeadTime.Text != "")
            {
                condition += " and 订单交期 like '%" + txtLeadTime.Text.Trim() + "%'";
            }
            string sql = string.Format(" select * from V_Nofinished_SaleOder {0}", condition);
            string tempSql = string.Format(@"
union all
select '合计','','','','','','','',
SUM (未交数量),SUM (已交数量),SUM (数量),'',SUM (总价),'','','','','','','' from ({0})t  order by 销售订单号 asc", sql);
            return sql + " " + tempSql;
        }

        private void Bind()
        {
            string result = GetSql();
            this.Repeater1.DataSource = SqlHelper.GetTable(result);
            this.Repeater1.DataBind();
        }

        protected void btnSearch_Click1(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string result=GetSql();
            if (string.IsNullOrEmpty(result))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(result, "未交销售订单");
        }
    }
}
