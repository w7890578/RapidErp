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
    public partial class MarerialInfoTableDetailedList : System.Web.UI.Page
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
                sql = string.Format(@" select mt.MaterialNumber,mt.MaterialName,mt.Description,
mk.Kind,mt.Type,mt.Brand,mt.StockSafeQty,mt.ProcurementPrice,
 (select WarehouseName from WarehouseInfo where WarehouseNumber=mt.MaterialPosition) as MaterialPosition,
 mt.MinPacking,
 mt.MinOrderQty,mt.ScrapPosition,mt.Remark
 from MarerialInfoTable mt left join MarerialKind mk on mt.Kind=mk.Id  where MaterialNumber='{0}' ", ToolManager.GetQueryString("Id"));
                Model.MarerialInfoTable materialinfotable = MarerialInfoTableManager.ConvertDataTableToModel(sql);
                this.lbMaterialNumber.InnerText = materialinfotable.MaterialNumber;
                this.lbMaterialName.InnerText = materialinfotable.MaterialName;
                this.lbDescription.InnerText = materialinfotable.Description;
                this.lbKind.InnerText = materialinfotable.Kind;
                this.lbType.InnerText = materialinfotable.Type;
                this.lbBrand.InnerText = materialinfotable.Brand;
                this.lbStockSafeQty.InnerText = materialinfotable.StockSafeQty.ToString();
                this.lbProcurementPrice.InnerText = materialinfotable.ProcurementPrice.ToString();
                this.lbMaterialPosition.InnerText = materialinfotable.MaterialPosition;
                this.lbMinPacking.InnerText = materialinfotable.MinPacking;
                this.lbMinOrderQty.InnerText = materialinfotable.MinOrderQty;
                this.lbScrapPosition.InnerText = materialinfotable.ScrapPosition;
                this.lbRemark.InnerText = materialinfotable.Remark;
            }
        }
    }
}
