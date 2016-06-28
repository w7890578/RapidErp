using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.SellManager
{
    public partial class CustomerDeliveryDetailList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0108", "Edit");
            }
            Bind();
        }

        private void Bind()
        {
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@" select cdd.Guid as Guid, cdd.ImportTime as 导入时间,cdd.CustomerId as 客户编号,cdd.Number as 物料,
              cdd.Description as 描述,cdd.AdvanceQty as 需提前交货数量,cdd.ReplyDate as 供应商回复交货日期,
              10 as 在制品数量,cdd.NonDeliveryQty as 未交订单数量,cdd.StockQty as 库存数量,
            case when  cdd.IsMeetDelivery>=0 then '满足' else '不满足' end as 是否满足交货,
           case when cdd.OrderContrast>=0 then '正常' else '不正常' end as 订单对比结果,cdd.Remark as 备注
              from CustomerDeliveryDetail cdd ");
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataSource = dt;
            this.rpList.DataBind();

        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
