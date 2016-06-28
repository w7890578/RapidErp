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
    public partial class CertificateYearReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                Bind();
            }

        }

        private void Bind()
        {
            string condition = " where 年度='" + drpYear.SelectedValue + "'";
            if (txtSupplierName.Text != "")
            {
                condition += " and 供应商名称 like'%" + txtSupplierName.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from [V_CertificateMonthAccountReport] {0} union all
 select '月总数','',cast (SUM( CAST (round( 
 (ISNULL ( [1月] ,0)),2) as numeric(20,2))) as varchar(10)),
 cast (SUM( CAST (round( 
 (ISNULL ( [2月] ,0)),2) as numeric(20,2))) as varchar(10)),
  cast (SUM( CAST (round( 
 (ISNULL ( [3月] ,0)),2) as numeric(20,2))) as varchar(10)),
  cast (SUM( CAST (round( 
 (ISNULL ( [4月] ,0)),2) as numeric(20,2))) as varchar(10)),
  cast (SUM( CAST (round( 
 (ISNULL ( [5月] ,0)),2) as numeric(20,2))) as varchar(10)),
  cast (SUM( CAST (round( 
 (ISNULL ( [6月] ,0)),2) as numeric(20,2))) as varchar(10)),
 cast ( SUM( CAST (round( 
 (ISNULL ( [7月] ,0)),2) as numeric(20,2))) as varchar(10)),
  cast (SUM( CAST (round( 
 (ISNULL ( [8月] ,0)),2) as numeric(20,2))) as varchar(10)),
 cast ( SUM( CAST (round( 
 (ISNULL ( [9月] ,0)),2) as numeric(20,2))) as varchar(10)),
  cast (SUM( CAST (round( 
 (ISNULL ( [10月] ,0)),2) as numeric(20,2))) as varchar(10)),
 cast ( SUM( CAST (round( 
 (ISNULL ( [11月] ,0)),2) as numeric(20,2))) as varchar(10)),
  cast (SUM( CAST (round( 
 (ISNULL ( [12月] ,0)),2) as numeric(20,2))) as varchar(10))
 
  from  [V_CertificateMonthAccountReport] {0}", condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
