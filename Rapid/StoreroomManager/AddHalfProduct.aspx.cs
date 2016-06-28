using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class AddHalfProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Data"))
                {
                    string keys = ToolManager.GetQueryString("Data");
                    CK(keys);
                }
                string sql = " select * from V_AddHalfProductWarehouseLogDetail order by LeadTime asc";
                rpList.DataSource = SqlHelper.GetTable(sql);
                rpList.DataBind();
            }

        }
        private void CK(string keys)
        {
            string error = string.Empty;
            List<string> sqls = new List<string>();
            string warehouseNumber = "BCPCK" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string sql = string.Format(@"insert into HalfProductWarehouseLog(WarehouseNumber,WarehouseName,ChangeDirection,Creator
   ,CreateTime)
   values('{0}','{1}','{2}','{3}','{4}')", warehouseNumber, "bcpk", "出库", userId, timeNow);
            sqls.Add(sql);
            sql = string.Format(@"insert into HalfProductWarehouseLogDetail(WarehouseNumber ,DocumentNumber,ProductNumber,Version ,MaterialNumber,SailOrderNumber
   ,LeadTime ,Qty  )
   select '{0}',DocumentNumber ,ProductNumber,Version ,MaterialNumber,SailOrderNumber
   ,LeadTime ,Qty  from HalfProductWarehouseLogDetail where Guid in({1})", warehouseNumber, keys);
            sqls.Add(sql);
            Response.Write(SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error);
            Response.End();
            return;
        }
    }
}
