using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class AddOrEditStockInventoryLogDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Check();
                if (ToolManager.CheckQueryString("InventoryNumber") && ToolManager.CheckQueryString("MaterialNumber") && ToolManager.CheckQueryString("Version"))
                {
                    DataTable dt = HasDeleted();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        lbInventoryNumber.Text = ToolManager.GetQueryString("InventoryNumber");
                        lbMaterialNumber.Text = ToolManager.GetQueryString("MaterialNumber");
                        lbVersion.Text = ToolManager.GetQueryString("Version");
                        txtPaperQty.Text = dr["PaperQty"] == null ? "" : dr["PaperQty"].ToString();
                        txtInventoryQty.Text = dr["InventoryQty"] == null ? "" : dr["InventoryQty"].ToString();
                        txtProfitAndLossQty.Text = dr["ProfitAndLossQty"] == null ? "" : dr["ProfitAndLossQty"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Check();
            string error = string.Empty;
            string inventorynumber = ToolManager.GetQueryString("InventoryNumber").Trim();
            string materialnumber = ToolManager.GetQueryString("MaterialNumber");
            string version = ToolManager.GetQueryString("Version");
            // string pagerqty = ToolManager.GetQueryString("PaperQty");
            string inventoryqty = Request.Form["txtInventoryQty"].ToString();
            // string inventoryqty = txtInventoryQty.Text.Trim();
            // string profitandlossqty = Request.Form["txtProfitAndLossQty"].ToString();
            string remark = Request.Form["txtRemark"].ToString();
            List<string> sqls = new List<string>();
            string sql = string.Format(@"update StockInventoryLogDetail set InventoryQty={0},ProfitAndLossQty={0}-PaperQty,Remark='{1}' 
where InventoryNumber ='{2}' and MaterialNumber ='{3}' and Version ='{4}'", inventoryqty, remark, inventorynumber, materialnumber, version);
            sqls.Add(sql);
            decimal x;

            if (!decimal.TryParse(txtPaperQty.Text, out x))
            {
                return;
            }
            
            if (!decimal.TryParse(txtInventoryQty.Text, out x))
            {
                return;
            }
           

//            sql = string.Format(@"select sil.WarehouseName ,sild.PaperQty from StockInventoryLogDetail sild inner join StockInventoryLog sil 
//on sild.InventoryNumber =sil.InventoryNumber where sil .InventoryNumber ='{0}'", inventorynumber);
//            DataTable dt = SqlHelper.GetTable(sql);
//            DataRow dr = dt.Rows[0];
//            sql = "";
//            switch (dr["WarehouseName"].ToString())
//            {
//                case "cpk":
//                    sql = string.Format(@"update ProductStock set StockQty ={0} where ProductNumber ='{1}'
//and Version ='{2}' and WarehouseName='{3}'", inventoryqty, materialnumber, version, dr["WarehouseName"]);
//                    break;
//                case "bcpk":
//                    sql = string.Format(@"update HalfProductStock set StockQty ={0} where ProductNumber ='{1}'
//and Version ='{2}' and WarehouseName='{3}'", inventoryqty, materialnumber, version, dr["WarehouseName"]);
//                    break;
//                case "ycl":
//                    sql = string.Format(@"update MaterialStock set 
//StockQty ={0} where MaterialNumber='{1}' and WarehouseName='{2}'", inventoryqty, materialnumber, dr["WarehouseName"]);
//                    break;
//                case "ypk":
//                    sql = string.Format(@"update SampleStock set 
//StockQty ={0} where MaterialNumber='{1}' and WarehouseName='{2}'", inventoryqty, materialnumber, dr["WarehouseName"]);
//                    break;
//                case "fpk":
//                    sql = string.Format(@"update ScrapStock set 
//StockQty ={0} where MaterialNumber='{1}' and WarehouseName='{2}'", inventoryqty, materialnumber, dr["WarehouseName"]);
//                    break;
//            }
//            if (!string.IsNullOrEmpty(sql))
//            {
//                sqls.Add(sql);
//            }

//            if (GetWirteSql(dr["WarehouseName"].ToString ()).Count >0)
//            {
//                sqls.AddRange(GetWirteSql(dr["WarehouseName"].ToString()));
//            }

            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);



            lbSubmit.Text = result == true ? "修改成功" : "修改失败，原因：" + error;
            if (result)
            {

                Tool.WriteLog(Tool.LogType.Operating, "编辑库存盘点明细" + inventorynumber, "编辑成功");
                Response.Write("<script>window.close();</script>");
                return;

            }
            else
            {

                Tool.WriteLog(Tool.LogType.Operating, "编辑库存盘点明细" + inventorynumber, "编辑失败！原因" + error);
                return;
            }

        }

        /// <summary>
        /// 获取写入流水账sql语句
        /// </summary>
        /// <returns></returns>
        private List<string> GetWirteSql(string warehouseName)
        {
            List<string> sqls = new List<string>();
            if (warehouseName.Equals("ycl")) //原材料库
            { }
            else if (warehouseName.Equals("cpk"))//产成品库
            { 
            
            }
            return sqls;
        }



        //InventoryNumber:InventoryNumber,MaterialNumber: MaterialNumber ,Version:Version
        private void Check()
        {
            if (!ToolManager.CheckQueryString("InventoryNumber"))
            {
                Response.Write("未知盘点编号！");
                Response.End();
                return;
            }
            lbInventoryNumber.Text = ToolManager.GetQueryString("InventoryNumber");
            lbMaterialNumber.Text = ToolManager.GetQueryString("MaterialNumber");
            lbVersion.Text = ToolManager.GetQueryString("Version");
            txtPaperQty.Text = ToolManager.GetQueryString("PaperQty");
        }
        private DataTable HasDeleted()
        {

            string error = string.Empty;
            string inventorynumber = ToolManager.GetQueryString("InventoryNumber");
            string materialnumber = ToolManager.GetQueryString("MaterialNumber");
            string version = ToolManager.GetQueryString("Version");
            string sql = string.Format(@"
       select * from StockInventoryLogDetail  where InventoryNumber='{0}' and MaterialNumber='{1}' and Version='{2}'", inventorynumber, materialnumber, Server.UrlDecode(version));
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count <= 0)
            {
                Response.Write("异常：该条记录已被删除！");
                Response.End();
                return null;
            }
            else
            {
                return dt;
            }
        }
    }
}
