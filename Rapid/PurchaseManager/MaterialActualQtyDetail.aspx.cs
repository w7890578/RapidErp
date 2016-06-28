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
    public partial class MaterialActualQtyDetail : System.Web.UI.Page
    {
        public DataTable ProductActualQtyTable = new DataTable();
        public DataTable MaterialActualQtyTable = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string materialNumber = Request["MaterialNumber"] ?? "";
                ProductActualQtyTable = QtyManager.Instance.GetProductActualQty(materialNumber);
                MaterialActualQtyTable = QtyManager.Instance.GetMaterialActualQty(materialNumber);
                
            }
        }
    }
}