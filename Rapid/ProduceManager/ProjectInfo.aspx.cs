using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using System.Data;
namespace Rapid.ProduceManager
{
    public partial class ProjectInfo : System.Web.UI.Page
    {
        public static string projectnumber = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            bind();
        }
        private void bind()
        {
            string sql = string.Format(@"select ProjectNumber as 项目编号,ProjectName as 项目名称 from ProjectInfo");
            DataTable dt = SqlHelper.GetTable(sql);
            projectnumber = dt.Rows[0]["项目编号"].ToString();
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bind();
        }
    }
}
