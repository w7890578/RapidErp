using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class MarerialKindList : System.Web.UI.Page
    {
        public string sql = string.Empty;
        public string error = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DeleteKind();
            }

        }
        private void DeleteKind()
        {
           
            sql = string.Format(@"select Kind as 原材料种类 from MarerialKind");
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DeleteKind();
        }


    }
}
