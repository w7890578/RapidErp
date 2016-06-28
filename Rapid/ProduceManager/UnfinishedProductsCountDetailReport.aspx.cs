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
	public partial class UnfinishedProductsCountDetailReport: System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version") && ToolManager.CheckQueryString("Type"))
                {
                    Response.Write("未知的产成品及版本！");
                    Response.End();
                    return;
                }
                Bind();
            }
		}

        private void Bind()
        {
            string productnumber = ToolManager.GetQueryString("ProductNumber");
            string version = ToolManager.GetQueryString("Version");
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type")); 
            string sql = string.Format(@"select * from V_UnfinishedProductsCountDetailReport 
where 产成品编号='{0}' and 版本='{1}'and 产品类型='{2}'", productnumber, version, type);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }
	}
}