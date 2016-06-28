using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.StoreroomManager
{
    public partial class OrderCheckPage : System.Web.UI.Page
    {
        public IList<OrderCheck> orderCheckList = new List<OrderCheck>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }
        protected void LoadPage()
        {
            string WarehouseNumber = ToolManager.GetRequestString("WarehouseNumber");
            orderCheckList = OrderCheckBLL.Instance.GetList(new OrderCheck() { WarehouseNumber = WarehouseNumber });
        }

    }
}