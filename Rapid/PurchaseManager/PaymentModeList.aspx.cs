using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Model;

namespace Rapid.PurchaseManager
{
    public partial class PaymentModeList : System.Web.UI.Page
    {
        public string sql = string.Empty;
        public string error = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }
        private void LoadPage()
        {
            sql = string.Format(@" select ID as 编号,PaymentMode as 付款方式 from PaymentMode");
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadPage();
        }
    }
}
