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
    public partial class NonDeliveryCompareList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spImp.Visible = ToolCode.Tool.GetUserMenuFunc("L0106", "Imp");
                Bind();
            }
        }

        private void Bind()
        {

            string sql = string.Empty;
            string error = string.Empty;

            sql = string.Format(@"  select ndc.ImportTime as 导入时间,ndc.OrderNumber as 采购凭证,ndc.CustomerProductNumber as 物料,
                 ndc.RowNumber as 项目,ndc.SupplierId as 供应商,
                 ndc.UserId as 导入人员,ndc.CertificateDate as 凭证日期,ndc.DeliveryDate as 交货日期,
                ndc.ShortText as 短文本,ndc.OrderQty as 采购订单数量,ndc.DeliveryQty as 已交货数量,ndc.StillDeliveryQty 
               as 仍要交货数量,ndc.CollectNumber as 汇总号,ndc.isOrderNumber as 该采购凭证是否异常,
               ndc.isRowNumber as 项目对比结果,ndc.isStillDeliveryQty as 仍要交货数量对比
               from NonDeliveryCompare ndc ");
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

    }
}







