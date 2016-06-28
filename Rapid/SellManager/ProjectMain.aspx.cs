using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.SellManager
{
    public partial class ProjectMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        private void Bind()
        {
            string condition = " where 1=1";
            if (txtProjectName.Text != "")
            {
                condition += " and ProjectName like '%" + txtProjectName.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select distinct ProjectName as 项目名称 from  T_ProjectInfo {0}",condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
