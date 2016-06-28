using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class AddOrEditScrappedLibrary : System.Web.UI.Page
    {
        public static string warehouseNumber = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Check();
                BindDrpProduct();
                if (ToolManager.CheckQueryString("DocumentNumber") && ToolManager.CheckQueryString("WarehouseNumber")
                    && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
                {
                    string documentNumber = "无";
                    string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                    string productNumber = ToolManager.GetQueryString("ProductNumber");
                    string version = ToolManager.GetQueryString("Version");
                    string sql = string.Format(@"
 select ReturnReason ,Qty ,Remark  from ProductWarehouseLogDetail
 where WarehouseNumber ='{0}' and DocumentNumber ='{1}' and ProductNumber ='{2}' and Version ='{3}'", warehouseNumber, documentNumber
 , productNumber, version);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtProductNumber.Text = productNumber;
                        txtVersion.Text = version;
                        lbProduct.Text = productNumber + "|" + version;
                        txtQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        lbOldQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        txtReason.Text = dr["ReturnReason"] == null ? "" : dr["ReturnReason"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();

                        lbInventoryQty.Text = StoreroomToolManager.GetinventoryQty("", productNumber, version, warehouseNumber, "ProductStock", "ProductWarehouseLog", ToolEnum.ProductType.Product);

                        btnSubmit.Text = "修改";
                        drpProduct.Visible = false;
                        lbProduct.Visible = true;
                    }
                }
                else
                {
                    btnSubmit.Text = "添加";
                    drpProduct.Visible = true;
                    lbProduct.Visible = false;
                }
            }
        }
        private void Check()
        {
            if (!ToolManager.CheckQueryString("WarehouseNumber"))
            {
                Response.Write("未知出入库单！");
                Response.End();
                return;
            }
            warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            lbWarehouseNumber.Text = warehouseNumber;
        }
        private void BindDrpProduct()
        {
            string sql = string.Format(@"select distinct (mo.ProductNumber+'^'+mo.Version) as Value,
(mo.ProductName+' 【'+mo.Version+'】') as Text 
 from Product mo order by Text asc");
            ControlBindManager.BindDrp(sql, this.drpProduct, "Value", "Text");
        }

        protected void drpProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string products = drpProduct.SelectedValue;
            string[] temp = products.Split('^');
            if (string.IsNullOrEmpty(products))
            {
                lbInventoryQty.Text = "";
                return;
            }
            lbInventoryQty.Text = StoreroomToolManager.GetinventoryQty("", temp[0], temp[1], warehouseNumber, "ProductStock", "ProductWarehouseLog", ToolEnum.ProductType.Product);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string qty = txtQty.Text;
            string reason = txtReason.Text;
            string remark = txtRemark.Text;
            string inventoryQty = lbInventoryQty.Text;
            List<string> sqls = new List<string>();
            string sql = string.Empty;
            string error = string.Empty;
            bool result = false;
            if (btnSubmit.Text.Equals("添加"))
            {
                string temp = drpProduct.SelectedValue;
                string[] products = temp.Split('^');
                string productNumber = txtProductNumber.Text.Trim();
                string version = txtVersion.Text.Trim();
                if (!ProductManager.IsExit(productNumber, version))
                {
                    lbSubmit.Text = "系统不存在该产成品编号和版本，请重新输入！";
                    return;
                }
                //if (Convert.ToInt32(inventoryQty) < Convert.ToInt32(qty))
                //{
                //    lbSubmit.Text = "库存数量小于出库数量！无法出库";
                //    return;
                //}
                if (StoreroomToolManager.IsExitForProductWarehouseLogDetail(warehouseNumber, "无", productNumber, version))
                {
                    lbSubmit.Text = "已存在相同记录！";
                    return;
                }
                sql = string.Format(@"  insert into ProductWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,ProductNumber
 ,Version ,ReturnReason ,Qty ,InventoryQty ,Remark ,RowNumber) values('{0}','无','{1}','{2}','{3}',{4},0,'{5}','无') "
 , warehouseNumber, productNumber, version, reason, qty, remark);
                sqls.Add(sql);
                result = SqlHelper.BatchExecuteSql(sqls, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败，原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加产品报废出库信息" + warehouseNumber, "增加成功");
                    Response.Write("<script>window.close();</script>");
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加产品报废出库信息" + warehouseNumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                string documentNumber = "无";
                string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                string productNumber = ToolManager.GetQueryString("ProductNumber");
                string version = ToolManager.GetQueryString("Version");
                //string oldQty = lbOldQty.Text;
                //int poor = Convert.ToInt32(qty) - Convert.ToInt32(oldQty); //算差值
                //if (poor > 0) //还要出库
                //{
                //    if (Convert.ToInt32(inventoryQty) < poor) //库存数量小于要修改的差
                //    {
                //        lbSubmit.Text = "库存数量低！无法进行此操作";
                //        return;
                //    }

                //}
                //poor = 0 - poor;//取反 
                sql = string.Format(@" update ProductWarehouseLogDetail set ReturnReason ='{0}',Qty ='{1}',Remark ='{2}'
 where WarehouseNumber ='{3}' and DocumentNumber ='{4}' and ProductNumber ='{5}' and Version ='{6}' "
 , reason, qty, remark, warehouseNumber, documentNumber, productNumber, version);
                sqls.Add(sql);
                result = SqlHelper.BatchExecuteSql(sqls, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败，原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑产品报废出库明细" + warehouseNumber, "编辑成功");
                    Response.Write("<script>window.close();</script>");
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑产品报废出库明细" + warehouseNumber, "编辑失败！原因" + error);
                    return;
                }
            }
        }
    }
}
