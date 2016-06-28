using BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.StoreroomManager
{
    public partial class Detail_ProductNumberInOrOutStorage : System.Web.UI.Page
    {
        public string title = string.Empty;
        public DataTable dtResult = new DataTable();
        public bool CanAutior = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string warehouseNumber = Request["warehouseNumber"];
                string typeName = ProductWarehouseLogDetailManager.Instance.GetTypeName(warehouseNumber);
                title = typeName + warehouseNumber;

                CanAutior = ProductWarehouseLogDetailManager.Instance.CanAutior(warehouseNumber);
                dtResult = ProductWarehouseLogDetailManager.Instance.GetTable(warehouseNumber);
            }
        }
    }
}