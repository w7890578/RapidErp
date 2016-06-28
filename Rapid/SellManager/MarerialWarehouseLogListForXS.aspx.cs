using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.SellManager
{
    public partial class MarerialWarehouseLogListForXS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //确认
                if (ToolManager.CheckQueryString("IsDetermine"))
                {
                    string warehousenumber = ToolManager.GetQueryString("Warehousenumber");
                    Determine(warehousenumber);
                }
                //删除
                if (ToolManager.CheckQueryString("IsDelete"))
                {
                    string warehousenumber = ToolManager.GetQueryString("Warehousenumber");
                    Delete(warehousenumber);
                }
                Bind();
            }
        }
        private void Bind()
        {
            string sql = string.Format(@"
select   pwl.*,pu.USER_NAME    from ProductWarehouseLog pwl left join PM_USER pu on pwl.Creator
=pu.USER_ID 
 where pwl.Type ='销售出库' order by pwl.CreateTime desc");
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Delete(string warehousenumber)
        {
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string sql = string.Format(@"
 delete  ProductWarehouseLogDetail where WarehouseNumber='{0}'", warehousenumber);
            sqls.Add(sql);
            sql = string.Format(@" 
 delete ProductWarehouseLog where WarehouseNumber='{0}'", warehousenumber);
            sqls.Add(sql);
            Response.Write(SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error);
            Response.End();
            return;
        }

        /// <summary>
        /// 确认
        /// </summary>
        private void Determine(string warehousenumber)
        {
            string error = string.Empty;
            string sql = string.Format(@"  
update ProductWarehouseLog set IsConfirm ='是' where WarehouseNumber='{0}' ", warehousenumber);
            Response.Write(SqlHelper.ExecuteSql(sql, ref error) ? "1" : error);
            Response.End();
            return;
        }
    }
}
