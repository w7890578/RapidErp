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
    public partial class ProjectProductProperty : System.Web.UI.Page
    {
        public static string projectnumber = string.Empty; 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ToolManager.CheckQueryString("ProjectNumber"))
            {
                string sql = string.Format(@" SELECT PPP.ProjectNumber as 项目编号,ppp.productnumber as 产品编号
 FROM ProjectProductProperty PPP INNER JOIN ProjectInfo p
on PPP.ProjectNumber=p.ProjectNumber
where p.ProjectNumber='{0}'", ToolManager.GetQueryString("ProjectNumber"));
                DataTable dt = SqlHelper.GetTable(sql);
                projectnumber = dt.Rows[0]["项目编号"].ToString();
                this.rpList.DataSource = SqlHelper.GetTable(sql);
                this.rpList.DataBind();

            }
            else
            {
                Response.Write("未知项目编号！");
                Response.End();
                return;
            }

        }
    }
}
