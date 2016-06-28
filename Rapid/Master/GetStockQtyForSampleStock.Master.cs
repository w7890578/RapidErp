using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.Master
{
    public partial class GetStockQtyForSampleStock : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                txtWarehouseNumber.Text = warehouseNumber;
            }
        }
    }
}
