using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Model;
using DAL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindDrp("select WarehouseName  from WarehouseInfo where Type='半成品库'", this.drpHalfProductPosition, "WarehouseName", "WarehouseName");
                ControlBindManager.BindDrp("select WarehouseName  from WarehouseInfo where Type='产成品库'", this.drpFinishedGoodsPosition, "WarehouseName", "WarehouseName");
                drpFinishedGoodsPosition.SelectedValue = "产成品库";
                drpHalfProductPosition.SelectedValue = "半成品库";
                this.trRatedManhour.Visible = false;
                this.trCostPrice.Visible = false;
                LoadPage();

            }
        }
        private void LoadPage()
        {
            string sql = string.Empty;
            string error = string.Empty;

            if (ToolManager.CheckQueryString("Id") && ToolManager.CheckQueryString("version"))
            {
                sql = string.Format(@" select * from Product where ProductNumber='{0}' and version='{1}'",
                    ToolManager.GetQueryString("Id"), ToolManager.GetQueryString("version"));
                Product product = ProductManager.ConvertDataTableToModel(sql);
                this.lblProductNumber.Text = product.ProductNumber;
                this.lblVersion.Text = product.Version;

                this.txtProductName.Text = product.ProductName;
                this.txtDescription.Text = product.Description;
                this.txtRatedManhour.Text = product.RatedManhour.ToString();
                this.txtQuoteManhour.Text = product.QuoteManhour.ToString();

                this.lbCostPrice.Text = product.CostPrice.ToString();

                this.txtSalesQuotation.Text = product.SalesQuotation.ToString();
                this.drpHalfProductPosition.SelectedValue = product.HalfProductPosition;
                this.drpFinishedGoodsPosition.SelectedValue = product.FinishedGoodsPosition;
                this.txtRemark.Text = product.Remark;
                this.drpIsOldVersion.SelectedValue = product.IsOldVersion;
                this.txtType.Text = product.Type;
                this.txtCargo.Text = product.Cargo;
                this.txtUnit.Text = product.Unit;
                this.txtNumberProperties.Text = product.NumberProperties;
                btnSubmit.Text = "修改";
                txtProductNumber.Visible = false;
                txtVersion.Visible = false;
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string sql = string.Empty;
            Model.Product product = new Product();
            product.ProductNumber = this.txtProductNumber.Text.Trim();
            product.Version = this.txtVersion.Text.Trim().ToUpper ();
            product.ProductName = this.txtProductName.Text.Trim();
            product.Description = this.txtDescription.Text.Trim();
            product.IsOldVersion = this.drpIsOldVersion.SelectedValue;
            product.Type = this.txtType.Text;
            product.Cargo = this.txtCargo.Text.Trim();
            product.Unit = this.txtUnit.Text.Trim();
            product.NumberProperties = this.txtNumberProperties.Text.Trim();
            bool result = false;
            if (this.txtQuoteManhour.Text.Trim().Length > 0 && this.txtSalesQuotation.Text.Trim().Length > 0)
            {

                product.QuoteManhour = Convert.ToInt32(this.txtQuoteManhour.Text.Trim());
//                sql = string.Format(@"  select isnull(sum(RatedManhour),0) as allprice from ProductWorkSnProperty where 
//                ProductNumber='{0}' and Version='{1}'", product.ProductNumber, product.Version);
//                string RatedManhour = SqlHelper.GetScalar(sql);
                product.RatedManhour =0;

                sql = string.Format(@" select isnull(sum(b.SingleDose*mt.ProcurementPrice),0) as 成本价 from Product p left join BOMInfo b on p.ProductNumber=b.ProductNumber and p.Version=b.Version
                left join MarerialInfoTable mt on b.MaterialNumber=mt.MaterialNumber
                where b.ProductNumber='{0}' and b.Version='{1}'", product.ProductNumber, product.Version);
                string CostPrice = SqlHelper.GetScalar(sql);
                this.lbCostPrice.Text = CostPrice;
                product.CostPrice = Convert.ToDecimal(this.lbCostPrice.Text.Trim());
                product.SalesQuotation = Convert.ToDecimal(this.txtSalesQuotation.Text.Trim());
            }
            else
            {
                lbSubmit.Text = "请将带*号的内容填写完整！";
                return;
            }
            product.HalfProductPosition = this.drpHalfProductPosition.SelectedValue.Trim();
            product.FinishedGoodsPosition = this.drpFinishedGoodsPosition.SelectedValue.Trim();
            product.Remark = this.txtRemark.Text.Trim();
            if (btnSubmit.Text.Equals("添加"))
            {
                result = ProductManager.AddProduct(product, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加产成品信息" + product.ProductNumber, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加产成品信息" + product.ProductNumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                product.ProductNumber = this.lblProductNumber.Text.Trim();
                product.Version = this.lblVersion.Text.Trim();
//                //额定工时
//                sql = string.Format(@"  select isnull(sum(RatedManhour),0) as allprice from ProductWorkSnProperty where 
//                ProductNumber='{0}' and Version='{1}'", product.ProductNumber, product.Version);
//                string RatedManhour = SqlHelper.GetScalar(sql);
                //product.RatedManhour = Convert.ToInt32(RatedManhour);
                product.RatedManhour = 0;
                //成本价
                sql = string.Format(@" select isnull(sum(b.SingleDose*mt.ProcurementPrice),0) as 成本价 from Product p left join BOMInfo b on p.ProductNumber=b.ProductNumber and p.Version=b.Version
                left join MarerialInfoTable mt on b.MaterialNumber=mt.MaterialNumber
                where b.ProductNumber='{0}' and b.Version='{1}'", product.ProductNumber, product.Version);
                string CostPrice = SqlHelper.GetScalar(sql);
                this.lbCostPrice.Text = CostPrice;
                product.CostPrice = Convert.ToDecimal(this.lbCostPrice.Text.Trim());

                result = ProductManager.EditProduct(product, ref error);
                lbSubmit.Text = result == true ? "修改成功！" : "修改失败：原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑产成品信息" + product.ProductNumber, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑产成品信息" + product.ProductNumber, "编辑失败！原因" + error);
                    return;
                }
            }
        }

    }
}
