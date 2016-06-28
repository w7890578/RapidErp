using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Data;
using BLL;
using DAL;
using Model;

namespace Rapid.AjaxRequest
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetInventoryNumberForProduct : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            if (ToolManager.GetQueryString("IsQueryOdersNumber").Equals("true")) //查询订单的ajax请求
            {
                string sql = string.Format(@"  select distinct top 10  InventoryNumber from StockInventoryLog  
 where 
InventoryNumber like '%{0}%' or InventoryNumber like'%{0}' or InventoryNumber like '{0}%'  order by  InventoryNumber desc ", ToolManager.GetQueryString("OdersNumber"));
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result += string.Format(" <tr><td>{0}</td></tr>", dr["InventoryNumber"]);
                    }
                }

            }
            else if (ToolManager.GetQueryString("IsQueryOdersNumber").Equals("false"))
            {
                string odersNumber = ToolManager.GetQueryString("OdersNumber");         //盘点编号
                string sql = string.Format(@"select distinct (mo.MaterialNumber+'^'+mo.Version) as Value,
(p.ProductName+' 【'+mo.Version+'】') as Text 
 from (select * from StockInventoryLogDetail where [Version] !='0') mo
inner join Product p on mo.MaterialNumber=p.ProductNumber
where mo.InventoryNumber='{0}'  ", odersNumber);
                result = "<option value =\"\">- - - - - 请 选 择 - - - - -</option>";
                DataTable dt = SqlHelper.GetTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    result += string.Format(" <option value ='{0}'>{1}</option> ", dr["Value"], dr["Text"]);
                }
            }
            else
            {
                string ordersNumber = ToolManager.GetQueryString("OdersNumber"); //盘点
                string product = ToolManager.GetQueryString("Prouduct");
                if (string.IsNullOrEmpty(product))
                {
                    result = "";

                }
                else
                {
                    string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                    string[] products = product.Split('^');
                    string inventoryQty = StoreroomToolManager.GetinventoryQty("", products[0], products[1], warehouseNumber, "ProductStock", "ProductWarehouseLog", ToolEnum.ProductType.Product);
                    inventoryQty = inventoryQty == "" ? "0" : inventoryQty;
                    result = inventoryQty;
                }
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
