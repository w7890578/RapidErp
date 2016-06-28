using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

namespace Rapid.SellManager
{
    public partial class MachineQuoteDetailReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("ProductNumber"))
                {
                    Response.Write("未知的产成品编号！");
                    Response.End();
                    return;
                }
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
            string productnumber = ToolManager.GetQueryString("ProductNumber");
            string version = ToolManager.GetQueryString("Version");
            string condition = " where 产成品编号='"+productnumber+"' and 版本 ='"+version+"'";
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
