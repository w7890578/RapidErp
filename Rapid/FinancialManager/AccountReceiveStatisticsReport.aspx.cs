using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.FinancialManager
{
    public partial class AccountReceiveStatisticsReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        private string GetSql()
        {
            string condition = " where 是否为预收='否'";
            if (txtCustomerName.Text != "")
            {
                condition += " and 客户名称 '%" + txtCustomerName.Text.Trim() + "%'";
            }
            if (txtCustomerOdersNumber.Text != "")
            {
                condition += " and 客户采购订单号 '%" + txtCustomerOdersNumber.Text.Trim() + "%'";
            }
            if (txtOdersNumber.Text != "")
            {
                condition += " and 销售订单号 '%" + txtOdersNumber.Text.Trim() + "%'";
            }
            if (txtDeliveryDate.Text != "")
            {
                condition += " and 送货日期='" + txtDeliveryDate.Text + "'";
            }
            if (txtDeliveryNumber.Text != "")
            {
                condition += " and 送货单号 '%" + txtDeliveryNumber.Text.Trim() + "%'";
            }
            if (txtCustomerMaterialNumber.Text != "")
            {
                condition += " and 客户物料编号='" + txtCustomerMaterialNumber.Text + "'";
            }
            if (txtRapidNumber.Text != "")
            {
                condition += " and 瑞普迪编号 '%" + txtRapidNumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from V_AccountReceiveStatisticsReport {0}
union all
select '合计','','','','','','','','','','','','',SUM(交货数量),'',SUM(总价),'','','','',''
from V_AccountReceiveStatisticsReport {0}
",condition);
            return sql;
        }

        private void Bind()
        {
            string sql = GetSql();
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string sql = GetSql();
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "应收账款统计报表");
        }
    }
}
