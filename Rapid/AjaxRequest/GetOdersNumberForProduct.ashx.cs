using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using BLL;
using System.Data;
using DAL;
using Model;

namespace Rapid.AjaxRequest
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetOdersNumberForProduct : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            if (ToolManager.GetQueryString("IsQueryOdersNumber").Equals("true")) //查询销售订单的ajax请求
            {
                string changeDirection = System.Web.HttpContext.Current.Server.UrlDecode(ToolManager.GetQueryString("ChangeDirection")).Trim ();
                string conditon = string.Empty;
                switch (changeDirection)
                {
                    case "退货入库": conditon = " and  so.OdersType ='正常订单' ";
                        break;
                    case "包装入库": conditon = " and  so.OdersType ='正常订单' ";
                        break;
                    case "维修入库": conditon = " and  so.OdersType ='维修订单' ";
                        break;
                    case "销售出库": conditon = " and  so.OdersType ='正常订单' ";
                        break;
                    case "样品出库": conditon = " and  so.OdersType ='样品订单' ";
                        break;
                    case "维修出库": conditon = " and  so.OdersType ='维修订单' ";
                        break;
                }

                string sql = string.Format(@"select distinct top 10 t.OdersNumber from ( select distinct top 10   mo.OdersNumber from MachineOderDetail mo inner join SaleOder so on so.OdersNumber =mo.OdersNumber
   where 1=1   {1}
order by mo.OdersNumber desc) t where 
t.OdersNumber like '%{0}%' or t.OdersNumber like'%{0}' or t.OdersNumber like '{0}%'  ", ToolManager.GetQueryString("OdersNumber"), conditon);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result += string.Format(" <tr><td>{0}</td></tr>", dr["OdersNumber"]);
                    }
                }

            }
            else if (ToolManager.GetQueryString("IsQueryOdersNumber").Equals("false"))
            {
                string odersNumber = ToolManager.GetQueryString("OdersNumber");         //销售订单号
                string sql = string.Format(@"select distinct (mo.ProductNumber+'^'+mo.Version) as Value,
(p.ProductName+' 【'+mo.Version+'】') as Text 
 from MachineOderDetail mo
inner join Product p on mo.ProductNumber=p.ProductNumber
where mo.OdersNumber='{0}'  ", odersNumber);
                result = "<option value =\"\">- - - - - 请 选 择 - - - - -</option>";
                DataTable dt = SqlHelper.GetTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    result += string.Format(" <option value ='{0}'>{1}</option> ", dr["Value"], dr["Text"]);
                }
            }
            else
            {
                string ordersNumber = ToolManager.GetQueryString("OdersNumber");
                string product = ToolManager.GetQueryString("Prouduct");
                if (string.IsNullOrEmpty(product))
                {
                    result = "^";

                }
                else
                {
                    string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                    string[] products = product.Split('^');
                    string sql = string.Format(@"select CustomerProductNumber from  ProductCustomerProperty
where CustomerId=(
select CustomerId from SaleOder where OdersNumber ='{0}')
and ProductNumber='{1}' and Version ='{2}'", ordersNumber, products[0], products[1]);
                    result = SqlHelper.GetScalar(sql);
                    string inventoryQty = StoreroomToolManager.GetinventoryQty("", products[0], products[1], warehouseNumber, "ProductStock", "ProductWarehouseLog", ToolEnum.ProductType.Product);
                    inventoryQty = inventoryQty == "" ? "0" : inventoryQty;
                    result = result + "^" + inventoryQty;
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
