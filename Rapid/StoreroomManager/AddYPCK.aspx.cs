using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class AddYPCK : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = string.Format("  select OdersNumber  from SaleOder where ProductType ='贸易' and OdersType ='样品订单' and OrderStatus ='未完成' and ISNULL( CheckTime ,'')!='' ");
                ControlBindManager.BindDrp(sql, drpOrderNumber, "OdersNumber", "OdersNumber");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            lbMsg.Text = StoreroomToolManager.AddYPCK(warehouseNumber, drpOrderNumber.SelectedValue, ref error) ? "添加成功" : "添加失败！原因：" + error;
        }
    }
}
