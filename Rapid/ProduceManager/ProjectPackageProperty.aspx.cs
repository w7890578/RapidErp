using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using BLL;

namespace Rapid.ProduceManager
{
    public partial class ProjectPackageProperty : System.Web.UI.Page
    {
        public static string projectnumber = string.Empty; 
        protected void Page_Load(object sender, EventArgs e)
        {

            if (ToolManager.CheckQueryString("ProjectNumber"))
            {
                string sql = string.Format(@"select ppp.ProjectNumber as 项目编号,ppp.PackageNumber as 包编号 from ProjectPackageProperty ppp 
inner join ProjectInfo p on ppp.ProjectNumber=p.ProjectNumber
where p.ProjectNumber='{0}'",ToolManager.GetQueryString("ProjectNumber"));
                DataTable dt = SqlHelper.GetTable(sql);
                projectnumber = dt.Rows[0]["项目编号"].ToString();
                this.rpList.DataSource = SqlHelper.GetTable(sql);
                this.rpList.DataBind();

            }
            else {
                Response.Write("未知项目编号！");
                Response.End();
                return;
            }
          
        }
    }
}
