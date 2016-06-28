using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;

namespace Rapid.Index.tab
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack )
            {
                LoadPage();     
            }
        }
        private void LoadPage()
        {
            string sql = "select * from   MakeCollectionsMode";
            DataTable dt = SqlHelper.GetTable(sql, ref sql);
            this.Repeater1.DataSource = dt;
            this.Repeater1.DataBind();
        }
    }
}
