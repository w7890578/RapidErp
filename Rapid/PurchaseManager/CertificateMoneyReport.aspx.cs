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
    public partial class CertificateMoneyReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpMonth.SelectedValue = DateTime.Now.Month.ToString();
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                Bind();
            }

        }

        private void Bind()
        {
            string sql = string.Format(@"exec  P_CertificateMoneyReport '{0}','{1}'", drpYear.SelectedValue, drpMonth.SelectedValue);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
