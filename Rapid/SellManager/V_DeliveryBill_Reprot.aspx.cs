using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.SellManager
{
    public partial class V_DeliveryBill_Reprot : System.Web.UI.Page
    {
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
            //ToolCode.Tool.MergeCells(Repeater1, "tdOrderNumber");
            //ToolCode.Tool.MergeCells(Repeater1, "tdCustomerOrderNumber");
        }

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

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string sql = GetSql();
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "送货单明细报表");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.labPage.Text = "1";
            this.contrlRepeater();
        }

        protected void lbtnDownPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = this.LabCountPage.Text;
            this.contrlRepeater();
        }

        protected void lbtnFirstPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = "1";
            this.contrlRepeater();
        }

        protected void lbtnNextPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = Convert.ToString(Convert.ToInt32(labPage.Text) + 1);
            this.contrlRepeater();
        }

        //获取指字符个数的字符
        //Repeater分页控制显示方法
        protected void lbtnpritPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = Convert.ToString(Convert.ToInt32(labPage.Text) - 1);
            this.contrlRepeater();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.labPage.Text = "1";
                this.contrlRepeater();
            }
        }

        private string GetSql()
        {
            string sql = string.Format(" select * from V_DeliveryBill_Reprot where 1=1");
            if (txtOrderNumber.Text.Trim() != "")
            {
                sql += string.Format(" and 销售订单号 like '%{0}%' ", txtOrderNumber.Text.Trim());
            }
            if (txtCustomerOrderNumber.Text.Trim() != "")
            {
                sql += string.Format(" and 客户采购订单号 like '%{0}%' ", txtCustomerOrderNumber.Text.Trim());
            }
            if (txtProductNumber.Text.Trim() != "")
            {
                sql += string.Format(" and 产成品编号 like '%{0}%' ", txtProductNumber.Text.Trim());
            }
            if (txtCustomerProductNumber.Text.Trim() != "")
            {
                sql += string.Format(" and 客户产成品编号 like '%{0}%' ", txtCustomerProductNumber.Text.Trim());
            }
            if (txtProjectName.Text.Trim() != "")
            {
                sql += string.Format(" and 项目 like '%{0}%' ", txtProjectName.Text.Trim());
            }
            if (txtCustomerName.Text.Trim() != "")
            {
                sql += string.Format(" and 客户名称 like '%{0}%' ", txtCustomerName.Text.Trim());
            }
            if (txtOrderType.Text.Trim() != "")
            {
                sql += string.Format(" and 订单类型 like '%{0}%' ", txtOrderType.Text.Trim());
            }
            if (txtDeliyNumber.Text.Trim() != "")
            {
                sql += string.Format(" and 送货单号 like '%{0}%' ", txtDeliyNumber.Text.Trim());
            }
            if (txtDeliveryPerson.Text.Trim() != "")
            {
                sql += string.Format(" and 送货人 like '%{0}%' ", txtDeliveryPerson.Text.Trim());
            }
            string tempsql = string.Format(@"union all
select '合计','','','','','','','',sum(数量),'','','',''  from ({0})t", sql);
            sql = sql + " " + tempsql;
            sql = string.Format(" select * from ({0})t order by 送货单号 desc", sql);
            return sql;
        }

        private DataTable GetTable()
        {
            string sql = GetSql();
            DataTable dt = SqlHelper.GetTable(sql);
            return dt;
        }
    }
}