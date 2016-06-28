using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using BLL;
using Rapid.ToolCode;

namespace Rapid.SellManager
{


    public partial class V_Finished_SaleOder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.labPage.Text = "1";
                this.contrlRepeater();
            }

        }

        private DataTable GetTable()
        {
            string sql = GetSql();
            string tempSql = string.Format(@"
union all
select '合计','','','','','','','','','',isnull( SUM (数量),0),isnull( SUM (总价),0)   from ({0})t
", sql);
            sql = sql + " " + tempSql;
            sql = string.Format("select * from ({0}) t order by 销售订单号 desc", sql);

            DataTable dt = SqlHelper.GetTable(sql);
            return dt;
        }

        //获取指字符个数的字符

        public string cuts(string aa, int bb)
        {
            if (aa.Length <= bb)
            {
                return aa;
            }
            else
            {
                return aa.Substring(0, bb);
            }
        }
        //Repeater分页控制显示方法

        public void contrlRepeater()
        {
            int pageSize = 20;
            if (!int.TryParse(txtPageSize.Text.Trim(), out pageSize))
            {
                pageSize = 20;
            }

            DataTable dt = GetTable();
            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dt.DefaultView;
            pds.AllowPaging = true;
            pds.PageSize = pageSize;
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
            if (txtProjectName.Text != "")
            {
                condition += " and 项目 like '%" + txtProjectName.Text.Trim() + "%'";
            }
            if (txtOrdersDate.Text != "")
            {
                condition += " and 订单日期 like '%" + txtOrdersDate.Text.Trim() + "%'";
            }
            if (txtYW.Text != "")
            {
                condition += " and 业务员 like '%" + txtYW.Text.Trim() + "%'";
            }
            string sql = string.Format(" select * from V_Finished_SaleOder {0}", condition);
            return sql;
        }
        protected void btnSearch_Click1(object sender, EventArgs e)
        {
            this.labPage.Text = "1";
            this.contrlRepeater();
        }

        protected void btnExp_Click(object sender, EventArgs e)
        {
            DataTable dt = GetTable();
            Tool.ExpExcel(dt, "已交销售订单报表(" + DateTime.Now.ToString("yyyy-MM-dd") + ")");
        }
    }
}
