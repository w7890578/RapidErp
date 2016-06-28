using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class PackagingProcessInfoList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("ProductType") && ToolManager.CheckQueryString("WorkSnNumber"))
                {
                    string productType = Server.UrlDecode(ToolManager.GetQueryString("ProductType"));
                    string workSnNumber = ToolManager.GetQueryString("WorkSnNumber");
                    Delete(productType, workSnNumber);
                }
                Bind();
            }
        }
        private void Bind()
        {
            string sql = " select * from  T_PackagingProcessInfo order by ProductType asc ,Sn asc ";
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
        private void Delete(string productType, string workSnNumber)
        {
            string error = string.Empty;
            string sql = string.Format(@"  delete T_PackagingProcessInfo where productType='{0}' 
and worksnNumber='{1}'", productType, workSnNumber);
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            if (result)
            {
                Response.Write("1");
                Response.End();
                return;
            }
            else
            {
                Response.Write(error);
                Response.End();
                return;
            }
        }
    }
}
