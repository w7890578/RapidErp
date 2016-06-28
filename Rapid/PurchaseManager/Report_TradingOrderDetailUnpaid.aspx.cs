using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.PurchaseManager
{
    public partial class Report_TradingOrderDetailUnpaid : System.Web.UI.Page
    {
        public DataTable dtResult = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }
        }

        private void DataBind()
        {
            string sql = string.Format(@"
select 
OdersNumber as '销售订单号',
ProductNumber as '原材料编号',
RowNumber as '行号',
Delivery as '交期',
CustomerMaterialNumber as '客户物料编号',
MaterialName as '型号',
Brand as '品牌',
Quantity as '订单数量',
NonDeliveryQty as '未交数量',
DeliveryQty as '已交数量',
CreateTime as '创建时间'
 from TradingOrderDetail  where NonDeliveryQty>0 and ProductNumber='{0}'
", Request["Id"]);
            dtResult = SqlHelper.GetTable(sql);
        }
    }
}