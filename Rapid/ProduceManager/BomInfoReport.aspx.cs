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
    public partial class BomInfoReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.labPage.Text = "1";
                this.contrlRepeater();
            }

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

        

        private DataTable   GetTable()
        {


            string sql = string.Format(@"select *,case when 单位='m' then 'mm' else 单位 end 单位新,case when 单位 ='m' then cast( 单机用量*1000 as int ) else CAST ( 单机用量 as int ) end
 as 单机用量新 from V_BOM_Count  {0} 
 order by 客户包号 asc,客户产成品编号 asc", GetCondition());
            return  SqlHelper.GetTable(sql);
            
        }
        private string GetCondition()
        {
            string condition = " where 1=1";
            if (txtPageNumber.Text != "")
            {
                condition += " and 包号 like '%" + txtPageNumber.Text.Trim() + "%'";
            }
            if (txtProductNumber.Text != "")
            {
                condition += " and 产成品编号 like '%" + txtProductNumber.Text.Trim() + "%'";
            }
            if (txtVerison.Text != "")
            {
                condition += " and 版本 like '%" + txtVerison.Text.Trim() + "%'";
            }
            if (txtCutomerProductNumber.Text != "")
            {
                condition += " and 客户产成品编号 like '%" + txtCutomerProductNumber.Text.Trim() + "%'";
            }
            if (txtDescription.Text != "")
            {
                condition += " and 产品描述 like '%" + txtDescription.Text.Trim() + "%'";
            }
            if (txtMaterialNumber.Text != "")
            {
                condition += " and 原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            if (txtCustomerMaterialNumber.Text != "")
            {
                condition += " and 客户物料号 like '%" + txtCustomerMaterialNumber.Text.Trim() + "%'";
            }
            if (txtMaterialDescription.Text != "")
            {
                condition += " and 物料描述 like '%" + txtMaterialDescription.Text.Trim() + "%'";
            } 
            if (txtType.Text != "")
            {
                condition += " and 成品类别 like '%" + txtType.Text.Trim() + "%' ";
            }
            if (txtCustomerPackNumber.Text != "")
            {
                condition += " and 客户包号 like '%" + txtCustomerPackNumber.Text.Trim() + "%' ";
            }
            return condition;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.labPage.Text = "1";
            this.contrlRepeater();
        }

        protected void btnExp_Click(object sender, EventArgs e)
        {
            string sql = string.Format(@"select *,case when 单位='m' then 'mm' else 单位 end 单位新,case when 单位 ='m' then cast( 单机用量*1000 as int ) else CAST ( 单机用量 as int ) end
 as 单机用量新 from V_BOM_Count  {0} 
 order by 客户包号 asc,客户产成品编号 asc", GetCondition()); 
            ToolCode.Tool.ExpExcel(sql, "BOM总报表");
        }
    }
}
