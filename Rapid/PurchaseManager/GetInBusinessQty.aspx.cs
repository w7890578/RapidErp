using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.PurchaseManager
{
    public partial class GetInBusinessQty : System.Web.UI.Page
    {
        public DataTable InBusinessQtyTable = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                InBusinessQtyTable = QtyManager.Instance.GetInBusinessQty(Request["ProductNumber"], Request["Version"]);
            }
        }
    }
}