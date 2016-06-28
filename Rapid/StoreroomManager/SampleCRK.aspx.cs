using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class SampleCRK : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("ids"))
                {
                    string error = string.Empty;
                    List<string> sqls = new List<string>();
                    string keys = ToolManager.GetQueryString("ids");
                    string sql = string.Format(@" 
delete MaterialWarehouseLogDetail where WarehouseNumber in ({0}) ", keys);
                    sqls.Add(sql);
                    sql = string.Format(@" 
delete MarerialWarehouseLog where WarehouseNumber in ({0}) ", keys);
                    sqls.Add(sql);
                    Response.Write(SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error);
                    Response.End();
                    return;
                }
                Bind();
            }
        }

        private void Bind()
        {
            string type = drpType.SelectedValue;
            string sql = string.Format(@"select * from MarerialWarehouseLog 
where (Type ='样品入库' or Type ='样品出库')");
            if (!string.IsNullOrEmpty(type))
            {
                sql += string.Format("  and ChangeDirection='{0}'", type);
            }
            sql += "  order by CreateTime desc ";
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
            return;

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
