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
    public partial class UnfinishedProductsAttheEndCountReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                Bind();
            }
        }

        private string GetSql()
        {
            string condition = " where 1=1";
            if (txtDate.Text != "")
            {
                condition += " and CAST( pp.CreateTime as date) <=CAST('" + txtDate.Text + "' as date)";
            }

            string sql = string.Format(@"  select ProductNumber as 产成品编号,CustomerProductNumber as 客户产成品编号,Version as 版本,productType as 类型,SUM( Qty -StorageQty) as 在制品数量 from 
 ProductPlanDetail ppdl 
 inner join  ProductPlan pp on ppdl.PlanNumber =pp.PlanNumber 
{0}
 group by ProductNumber ,CustomerProductNumber,Version ,productType",condition);
            return sql;
        }

        private void Bind()
        {
            string sql=GetSql ();
            rpList.DataSource = SqlHelper.GetTable(sql );
            rpList.DataBind();
            string sqla = string.Format(@" select SUM (t.在制品数量) as 电缆在制总数  from ({0}) t where t.类型='电缆'", sql);
            lblDL.Text= SqlHelper.GetScalar(sqla);
           string sqlb = string.Format(@" select SUM (t.在制品数量) as 电缆在制总数  from ({0}) t where t.类型='包装'", sql);
            string num=SqlHelper.GetScalar(sqlb) ;
            lblBZ.Text = num== "" ? "0" : num;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(), "月末在制未完成产品统计表");
        }
    }
}
