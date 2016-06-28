using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
namespace Rapid.StoreroomManager
{
    public partial class Detail_ChangeNumberInStorage : System.Web.UI.Page
    {
        public string title = string.Empty;
        public DataTable dtResult = new DataTable();
        public bool CanAutior = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string warehouseNumber = Request["warehouseNumber"];
                string typeName = MaterialWarehouseLogDetailManager.Instance.GetTypeName(warehouseNumber);
                title = typeName + warehouseNumber;

                CanAutior = MaterialWarehouseLogDetailManager.Instance.CanAutior(warehouseNumber);
                dtResult = MaterialWarehouseLogDetailManager.Instance.GetTable(warehouseNumber);
            }
        }
    }
}