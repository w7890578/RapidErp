using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using System.Data;
using Model;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class AddOrEditOverageOutOfStorage : System.Web.UI.Page
    {
        public static string titleName = string.Empty;
        public static string orderName = string.Empty;

        public static string type = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Check();
                string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                lbWarehouseNumber.Text = warehouseNumber;
                if (ToolManager.CheckQueryString("DocumentNumber") && ToolManager.CheckQueryString("WarehouseNumber")
                     && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
                {
                    string documentNumber = ToolManager.GetQueryString("DocumentNumber");
                    string productNumber = ToolManager.GetQueryString("ProductNumber");
                    string version = ToolManager.GetQueryString("Version");

                    string sql = string.Format(@" select * from ProductWarehouseLogDetail where WarehouseNumber='{0}'
             and DocumentNumber='{1}' and ProductNumber ='{2}' and Version ='{3}'", warehouseNumber, documentNumber, productNumber, version);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        lbInventoryNumber.Text = dr["DocumentNumber"] == null ? "" : dr["DocumentNumber"].ToString();
                        lbProduct.Text = dr["ProductNumber"].ToString() + "|" + dr["Version"].ToString();
                        txtQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        lbOldQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        txtInventoryQty.Text = StoreroomToolManager.GetinventoryQty("", dr["ProductNumber"].ToString(), dr["Version"].ToString(), warehouseNumber
                                  , "ProductStock", "ProductWarehouseLog", ToolEnum.ProductType.Product);
                        txtInventoryNumber.Visible = false;
                        lbInventoryNumber.Visible = true;
                        drpProduct.Visible = false;
                        btnSubmit.Text = "修改";

                    }
                }
                else
                {
                    txtInventoryNumber.Visible = true;
                    lbInventoryNumber.Visible = false;
                    drpProduct.Visible = true;
                    btnSubmit.Text = "添加";
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string warehouseNumber = this.lbWarehouseNumber.Text;
            string documentNumber = txtInventoryNumber.Text;
            string product = string.Empty;
            string productNumber = string.Empty;
            string version = string.Empty;
            string customerProductNumber = string.Empty;
            string qty = this.txtQty.Text;
            string remark = txtRemark.Text;
            string inventoryQty = string.Empty; //库存数量
            string reason = string.Empty;
            string sql = string.Empty;
            string error = string.Empty;
            bool result = false;
            if (btnSubmit.Text.Equals("添加"))
            {
                product = Request.Form["ctl00$ContentPlaceHolder1$drpProduct"].ToString();
                inventoryQty = Request.Form["ctl00$ContentPlaceHolder1$txtInventoryQty"].ToString();
                productNumber = product.Split('^')[0].ToString();
                version = product.Split('^')[1].ToString();
                sql = string.Format(@"select COUNT(*) from ProductWarehouseLogDetail where WarehouseNumber='{0}' and DocumentNumber='{1}'
            and ProductNumber='{2}' and Version ='{3}'", warehouseNumber, documentNumber, productNumber, version);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = "已存在相同记录！请重新填写";
                    BindProduct(documentNumber, product);
                    txtInventoryQty.Text = inventoryQty;
                    return;
                }
                string customerId = ""; //客户编号(编辑添加不用维护，在列表页的时候用连接查询)
                string opper = type == "入库" ? "" : "-";//运算符号
                if (type.Equals("出库"))
                {
                    if (inventoryQty.Equals("0"))
                    {
                        ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = "当前库存数量为0！无法进行出库操作";
                        BindProduct(documentNumber, product);
                        txtInventoryQty.Text = inventoryQty;
                        return;
                    }
                    if (Convert.ToInt32(inventoryQty) < Convert.ToInt32(qty))
                    {
                        ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = "当前库存数量低！无法进行操作，请添加库存数量";
                        BindProduct(documentNumber, product);
                        txtInventoryQty.Text = inventoryQty;
                        return;
                    }
                }
                List<string> sqls = new List<string>();
                 
                sql = string.Format(@"insert into ProductWarehouseLogDetail (WarehouseNumber ,
            DocumentNumber,ProductNumber,Version
            ,CustomerProductNumber,ReturnReason,CustormerName,Qty ,Remark ,InventoryQty)
            values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}',0)", warehouseNumber, documentNumber, productNumber
   , version, customerProductNumber, reason, customerId, qty, remark);
                sqls.Add(sql);
                result=SqlHelper.BatchExecuteSql(sqls, ref error);
                ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text=result==true?"添加成功":"添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加" + titleName + warehouseNumber, "增加成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加" + titleName + warehouseNumber, "增加失败！原因"+error);
                    BindProduct(documentNumber, product);
                    txtInventoryQty.Text = inventoryQty;
                    return;
                }
            }
            else
            {
                documentNumber = lbInventoryNumber.Text;
                product = lbProduct.Text;
                productNumber = product.Split('|')[0].ToString();
                version = product.Split('|')[1].ToString();
                List<string> sqls = new List<string>();
                int poor = Convert.ToInt32(qty) - Convert.ToInt32(lbOldQty.Text);
                string poorStr = "";
                if (type.Equals("入库"))
                {
                    if (poor < 0)
                    {
                        if ((0 - poor) > Convert.ToInt32(txtInventoryQty.Text))
                        {
                            ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = "当前库存数量低无法进行此操作！请重新填写数量";

                           // txtInventoryQty.Text = inventoryQty;
                            return;
                        }
                        poorStr = "-";
                        poor = 0 - poor;
                    }
                   
                }
                else //出库
                {
                    if (poor > 0)
                    {
                        if (poor > Convert.ToInt32(txtInventoryQty.Text))
                        {
                            ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = "当前库存数量低无法进行此操作！请重新填写数量";

                           // txtInventoryQty.Text = inventoryQty; 
                            return;
                        }

                        poorStr = "-";
                    }
                    else
                    {
                        poorStr = "+";
                        poor = 0 - poor;
                    }
                }
               
           
                sql = string.Format(@" update ProductWarehouseLogDetail 
            set ReturnReason ='{0}' , qty={1} ,Remark ='{2}'
             where WarehouseNumber='{3}' and DocumentNumber='{4}' and ProductNumber='{5}' and [Version] ='{6}' ",
 reason, qty, remark, warehouseNumber, documentNumber, productNumber, version);
                sqls.Add(sql);

                result = SqlHelper.BatchExecuteSql(sqls, ref error);
                ToolCode.Tool.GetMasterLabel(this.Page, "lbSubmit").Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑" + titleName + "明细" + warehouseNumber, "编辑成功");  
                    Response.Write("<script>window.close();</script>");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑" + titleName+"明细"+ warehouseNumber, "编辑失败！原因" + error);
                    BindProduct(documentNumber, product);
                    txtInventoryQty.Text = inventoryQty;
                    return;
                }

            }
        }
        private void BindProduct(string documentNumber, string value)
        {
            string sql = string.Format(@"select distinct (mo.MaterialNumber+'^'+mo.Version) as Value,
(p.ProductName+' 【'+mo.Version+'】') as Text 
 from (select * from StockInventoryLogDetail where [Version] !='0') mo
inner join Product p on mo.MaterialNumber=p.ProductNumber
where mo.InventoryNumber='{0}'  ", documentNumber);
            ControlBindManager.BindDrp(sql, this.drpProduct, "Value", "Text");
            drpProduct.SelectedValue = value;
        }
        private void Check()
        {
            if (!ToolManager.CheckQueryString("WarehouseNumber"))
            {
                Response.Write("未知出入库单！");
                Response.End();
                return;
            }
            titleName = Server.UrlDecode(ToolManager.GetQueryString("ChangeDirection"));
            switch (titleName)
            {
                case "盘盈入库":
                    type = "入库";
                    break;
                case "盘亏出库":
                    type = "出库";
                    break;
            }
        }
    }
}
