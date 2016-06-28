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
    public partial class DeliveryNoteDetailedUnpaid : System.Web.UI.Page
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
 d.DeliveryNumber as '送货单号',
 d.OrdersNumber as '销售订单号',
 d.ProductNumber as '原材料编号',
 d.CustomerProductNumber as '客户物料编号',
 d.RowNumber as '行号',
 d.LeadTime as '交期',
 d.MaterialDescription as '原材料描述',
 d.DeliveryQty as '发货数量',
 d.ConformenceQty as '实收数量'
  from DeliveryNoteDetailed d 
  inner join DeliveryBill b on 
 d.DeliveryNumber=b.DeliveryNumber
  where 
  d.ProductNumber='{0}' 
  and ISNULL(d.Version,'')=''
  and b.IsConfirm!='已确认'
", Request["Id"]);
            dtResult = SqlHelper.GetTable(sql);
        }
    }
}