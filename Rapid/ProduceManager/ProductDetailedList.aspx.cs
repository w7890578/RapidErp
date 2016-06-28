using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using System.Data;

namespace Rapid.ProduceManager
{
    public partial class ProductDetailedList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                onload();
            }
        }
        private void onload()
        {
            string sql = string.Empty;
            string error = string.Empty;
            if (ToolManager.CheckQueryString("Id"))
            {
                sql = string.Format(@" select p.ProductNumber as ProductNumber,p.Version as Version,
                p.ProductName as ProductName,p.Description as Description,mk.Kind as Kind,P.Type AS Type,
                p.RatedManhour as RatedManhour,p.QuoteManhour as QuoteManhour,p.CostPrice as CostPrice,
                p.SalesQuotation as SalesQuotation,p.HalfProductPosition as HalfProductPosition,
                p.FinishedGoodsPosition as FinishedGoodsPosition,p.Remark as Remark
                from Product p left join MarerialKind mk on p.Kind=mk.Id where ProductNumber='{0}' ",
                ToolManager.GetQueryString("Id"));
                Product product = ProductManager.ConvertDataTableToModel(sql);
                this.lbProductNumber.InnerText = product.ProductNumber;
                this.lbVersion.InnerText = product.Version;
                this.lbProductName.InnerText = product.ProductName;
                this.lbDescription.InnerText = product.Description;
                this.lbKind.InnerText = product.Kind;
                this.lbType.InnerText = product.Type;
                this.lbRatedManhour.InnerText = product.RatedManhour.ToString();
                this.lbQuoteManhour.InnerText = product.QuoteManhour.ToString();
                this.lbCostPrice.InnerText = product.CostPrice.ToString();
                this.lbSalesQuotation.InnerText = product.SalesQuotation.ToString();
                this.lbHalfProductPosition.InnerText = product.HalfProductPosition;
                this.lbFinishedGoodsPosition.InnerText = product.FinishedGoodsPosition;
                this.lbRemark.InnerText = product.Remark;
            }
        }
    }
}
