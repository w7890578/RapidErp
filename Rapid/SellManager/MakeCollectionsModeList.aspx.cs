using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.SellManager
{
    public partial class MakeCollectionsModeList : System.Web.UI.Page
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
            sql = string.Format(@" select id as 编号,MakeCollectionsMode as 收款方式 from MakeCollectionsMode");
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();

        }
    }

}
