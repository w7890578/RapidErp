using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using Rapid.ToolCode;
using BLL;

namespace Rapid.FinancialManager
{
    public partial class InvoiceAccountReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //标识删除
                if (ToolManager.CheckQueryString("IsDelete"))
                {

                    string sql = string.Empty;
                    string error = string.Empty;
                    string guid = ToolManager.GetQueryString("Guid");
                    sql = string.Format("delete InvoiceAccountInfo where Guid ='{0}'",guid);
                    bool result = SqlHelper.ExecuteSql(sql, ref error);
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除发票信息", "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除发票信息", "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                this.labPage.Text = "1";
                this.contrlRepeater();

            }
        }

        private DataTable GetTable()
        {
            string condition = " where 1=1";
            if (txtCustomerName.Text != "")
            {
                condition += " and 客户名称 like '%" + txtCustomerName.Text.Trim() + "%'";
            }
            if (txtInvoiceNumber.Text != "")
            {
                condition += " and 发票号码 like '%" + txtInvoiceNumber.Text.Trim() + "%'";
            }
            if (drpInvoiceType.SelectedValue != "")
            {
                condition+=" and 发票类型='"+drpInvoiceType.SelectedValue+"'";
            }
            if (drpIsPay.SelectedValue != "")
            {
                condition+=" and 是否已收款='"+drpIsPay.SelectedValue+"'";
            }
            string sql = string.Format(@"select * from V_InvoiceAccountReport {0}  order by 序号 asc", condition);
            DataTable dt = SqlHelper.GetTable(sql);
            return dt;
        }

        //Repeater分页控制显示方法

        public void contrlRepeater()
        {
            DataTable dt = GetTable();
            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dt.DefaultView;
            pds.AllowPaging = true;
            pds.PageSize = 100;
            pds.CurrentPageIndex = Convert.ToInt32(this.labPage.Text) - 1;
            Repeater1.DataSource = pds;
            LabCountPage.Text = pds.PageCount.ToString();
            labPage.Text = (pds.CurrentPageIndex + 1).ToString();
            this.lbtnpritPage.Enabled = true;
            this.lbtnFirstPage.Enabled = true;
            this.lbtnNextPage.Enabled = true;
            this.lbtnDownPage.Enabled = true;
            if (pds.CurrentPageIndex < 1)
            {
                this.lbtnpritPage.Enabled = false;
                this.lbtnFirstPage.Enabled = false;
            }
            if (pds.CurrentPageIndex == pds.PageCount - 1)
            {
                this.lbtnNextPage.Enabled = false;
                this.lbtnDownPage.Enabled = false;
            }
            Repeater1.DataBind();
        }
        protected void lbtnpritPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = Convert.ToString(Convert.ToInt32(labPage.Text) - 1);
            this.contrlRepeater();
        }
        protected void lbtnFirstPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = "1";
            this.contrlRepeater();
        }

        protected void lbtnDownPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = this.LabCountPage.Text;
            this.contrlRepeater();
        }

        protected void lbtnNextPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = Convert.ToString(Convert.ToInt32(labPage.Text) + 1);
            this.contrlRepeater();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.labPage.Text = "1";
            this.contrlRepeater();
        }
    }
}
