using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class AddYPRK : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            lbMsg.Text = StoreroomToolManager.AddYPRK(warehouseNumber, txtSuppilMateriNumber.Text.Trim()
                , txtQty.Text.Trim(), txtRemark.Text, ref error) ? "添加成功" : "添加失败！原因：" + error;
        }
    }
}
