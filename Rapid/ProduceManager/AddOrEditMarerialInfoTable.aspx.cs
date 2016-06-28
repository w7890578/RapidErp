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
using System.Text.RegularExpressions;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditMarerialInfoTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindDrp("select WarehouseNumber,WarehouseName  from WarehouseInfo where Type='废品库'", this.drpScrapPosition, "WarehouseNumber", "WarehouseName");
                ControlBindManager.BindDrp("select WarehouseNumber,WarehouseName  from WarehouseInfo where Type='原材料库'", this.drpMaterialPosition, "WarehouseNumber", "WarehouseName");
                drpMaterialPosition.SelectedValue = "ycl";
                drpScrapPosition.SelectedValue = "fpk";
                LoadPage();
            }
        }
        private void LoadPage()
        {
            string sql = string.Empty;
            string error = string.Empty;
            //MarerialInfoTableManager.BindKind(drpKind);
            //ControlBindManager.BindDrp(" select type from MareriaType", drpType, "type", "type");
            //MarerialInfoTableManager.BindWarehouseName(drpMaterialPosition);
            if (ToolManager.CheckQueryString("Id"))
            {
                sql = string.Format(@" select * from MarerialInfoTable where MaterialNumber='{0}' ", Server.UrlDecode(ToolManager.GetQueryString("Id")));
                //sql = string.Format(@" select * from MarerialInfoTable where MaterialNumber='{0}' ", "test2");
                this.trMaterialNumber.Visible = false;
                MarerialInfoTable marerialinfotable = MarerialInfoTableManager.ConvertDataTableToModel(sql);
                this.txtMaterialNumber.Text = marerialinfotable.MaterialNumber;
                this.txtMaterialName.Text = marerialinfotable.MaterialName;
                this.txtDescription.Text = marerialinfotable.Description;
                this.txtUnit.Text = marerialinfotable.Unit;
                this.txtNumberProperties.Text = marerialinfotable.NumberProperties;
                this.txtKind.Text = marerialinfotable.Kind;
                this.txtType.Text = marerialinfotable.Type;
                //MarerialInfoTableManager.BindType(drpKind, drpType);
                //this.drpType.SelectedValue = marerialinfotable.Type;

                this.txtBrand.Text = marerialinfotable.Brand;
                //this.txtStockSafeQty.Text = marerialinfotable.StockSafeQty.ToString();
                this.txtProcurementPrice.Text = marerialinfotable.ProcurementPrice.ToString();

                this.drpMaterialPosition.SelectedValue = marerialinfotable.MaterialPosition;

                this.txtMinPacking.Text = marerialinfotable.MinPacking;
                this.txtMinOrderQty.Text = marerialinfotable.MinOrderQty;
                this.drpScrapPosition.SelectedValue = marerialinfotable.ScrapPosition;
                this.txtRemark.Text = marerialinfotable.Remark;
                this.txtCargo.Text = marerialinfotable.Cargo;
                this.txtCargoType.Text = marerialinfotable.CargoType;
                btnSubmit.Text = "修改";
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Regex r = new Regex(@"[\u4e00-\u9fa5]+");
            Match mc = r.Match(txtMaterialNumber.Text.Trim());
            if (mc.Length != 0)
            {
                lbSubmit.Text = "注意：原材料编号禁止输入中文";
                lbSubmit.Focus();
                return;
            }
            string error = string.Empty;
            Model.MarerialInfoTable marerialinfotable = new MarerialInfoTable();
            marerialinfotable.MaterialNumber = this.txtMaterialNumber.Text.Trim();
            marerialinfotable.MaterialName = this.txtMaterialName.Text.Trim();
            marerialinfotable.Description = this.txtDescription.Text.Trim();
            marerialinfotable.Kind = this.txtKind.Text;
            marerialinfotable.Type = this.txtType.Text;
            marerialinfotable.Brand = this.txtBrand.Text.Trim();
            marerialinfotable.Cargo = this.txtCargo.Text.Trim();
            marerialinfotable.CargoType = this.txtCargoType.Text.Trim();
            marerialinfotable.NumberProperties = this.txtNumberProperties.Text.Trim();
            marerialinfotable.Unit = this.txtUnit.Text.Trim();
            bool result = false;
            //if (this.txtStockSafeQty.Text.Trim().Length > 0 && this.txtProcurementPrice.Text.Trim().Length > 0)
            //{
            //    marerialinfotable.StockSafeQty = Convert.ToInt32(this.txtStockSafeQty.Text.Trim());
            //    marerialinfotable.ProcurementPrice = Convert.ToDecimal(this.txtProcurementPrice.Text.Trim());
            //}
            //else
            //{
            //    lbSubmit.Text = "原材料信息不完整！";
            //    return;
            //}
            marerialinfotable.StockSafeQty = 0;
            marerialinfotable.ProcurementPrice = Convert.ToDecimal(this.txtProcurementPrice.Text.Trim());
            marerialinfotable.MaterialPosition = this.drpMaterialPosition.SelectedValue;
            marerialinfotable.MinPacking = this.txtMinPacking.Text.Trim();
            marerialinfotable.MinOrderQty = this.txtMinOrderQty.Text.Trim();
            marerialinfotable.ScrapPosition = this.drpScrapPosition.SelectedValue.Trim();
            marerialinfotable.Remark = this.txtRemark.Text.Trim();
            if (btnSubmit.Text.Equals("添加"))
            {

                result = MarerialInfoTableManager.AddMarerialInfoTable(marerialinfotable, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料信息" + marerialinfotable.MaterialNumber, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料信息" + marerialinfotable.MaterialNumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                result = MarerialInfoTableManager.EditMarerialInfoTable(marerialinfotable, ref error);
                lbSubmit.Text = result == true ? "修改成功！" : "修改失败：原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料信息" + marerialinfotable.MaterialNumber, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料信息" + marerialinfotable.MaterialNumber, "编辑失败！原因" + error);
                    return;
                }
            }
        }

    }
}
