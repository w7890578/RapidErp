using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class AddSampleCRK : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string type = drpType.SelectedValue;
            if (string.IsNullOrEmpty(type))
            {
                lbMsg.Text = "请选择变动方向";
                return;
            }
            string lx = type.Equals("入库") ? "样品入库" : "样品出库";
            string error = string.Empty;
            string warehouseNumber = "YPCRK" + DateTime.Now.ToString("yyyy-MM-ddHHmmss");
            string userName = ToolCode.Tool.GetUser().UserName;
            string sql = string.Format(@"insert into MarerialWarehouseLog(WarehouseNumber,WarehouseName,ChangeDirection ,Type ,Creator,CreateTime  )
values('{0}','{1}','{2}','{3}','{4}','{5}')", warehouseNumber, "ypk", type, lx, userName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SqlHelper.ExecuteSql(sql, ref error);
            Response.Write(ToolManager.GetClosePageJS());
            Response.End();
            return;
        }
    }
}
