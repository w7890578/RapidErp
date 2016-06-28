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
    public partial class ProductProcessList : System.Web.UI.Page
    {
        public static string show;
        protected void Page_Load(object sender, EventArgs e)
        {
            //查询
            if (ToolManager.CheckQueryString("pageIndex"))
            {
                GetPageOperation("btnSearch");
            }
        }
        public static void GetPageOperation(string btnId)
        {
            string tempSql = WorkOrderManager.GetOrderNofinesfinishedDetail();
            string sql = string.Format(@"   
select t.销售订单号,t.CustomerOrderNumber as 客户采购订单号,t.OdersType as 订单类型,
pcp.CustomerProductNumber as 客户产成品编号,t.产品编号,t.版本,t.订单数量,
t.已交货数量,
case when  t.未交货数量 <0 then 0 else t.未交货数量 end as 未交货数量 
,t.库存数量,t.在制品数量,

case when  t.未交货数量 <0 then 0-t.库存数量-isnull(noAddQty.qty,0)-producting.productQty 
else t.未交货数量-t.库存数量-isnull(noAddQty.qty,0)-producting.productQty end  as 需要生产数量, 
   t.交期,t.行号,t.Description as 产品描述,c.CustomerName as 客户名称 from (
 select t.*,so.CustomerId,so.OdersType ,p.Description,so . CustomerOrderNumber from ({0})t  
 left join SaleOder so on t.销售订单号=so.OdersNumber 
 left join Product p on t.产品编号=p.ProductNumber and t.版本=p.Version 
 ) t left join Customer c on t.CustomerId=c.CustomerId
left join ProductCustomerProperty pcp on t.CustomerId=pcp.CustomerId 
and t.产品编号=pcp.ProductNumber and 
t.版本=pcp.Version
left join ( select ProductNumber,Version,SUM(qty)  as qty from ProductWarehouseLogDetail where WarehouseNumber in (
 select WarehouseNumber from ProductWarehouseLog where ChangeDirection='入库' and ISNULL(CheckTime,'')=''
 ) group by ProductNumber,Version) 
as noAddQty on t.产品编号=noAddQty.ProductNumber and t.版本=noAddQty.Version
left join ( select  ProductNumber,Version   , 
case when 
sum(Qty)-SUM(StorageQty)<0 then 0 else sum(Qty)-SUM(StorageQty) end as productQty
 from  ProductPlanDetail
 group by ProductNumber,Version  ) as producting   on t.产品编号=producting.ProductNumber and t.版本=producting.Version
", tempSql);
            if (string.IsNullOrEmpty(tempSql))
            {
                string responseValue1 = "" + "^" + "" + "^" + "" + "^" + "0";
                HttpContext.Current.Response.Write(responseValue1);
                HttpContext.Current.Response.End();
                return;
            }
            int pageCount = 0;//总页数 
            int totalRecords = 0;//总行数
            string error = string.Empty;
            string text = string.Empty;
            string tdTextTemp = string.Empty;
            string pageIndex = ToolManager.GetQueryString("pageIndex");
            string pageSize = ToolManager.GetQueryString("pageSize");
            string sortName = ToolManager.GetQueryString("sortName");
            string sortDirection = ToolManager.GetQueryString("sortDirection");
            string querySql = ToolManager.GetQueryString("querySql");
            //querySql += querySql + "and t.需要生产数量>0";
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, sql + " " + querySql, string.Format(" order by {0} {1},产品编号 asc,版本 asc,交期 asc", sortName, sortDirection), ref totalRecords);

            int columCount = dt.Columns.Count;
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    if (i == 0)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);

                    }
                    else
                    {

                        if (dt.Columns[i].ColumnName.Equals("需要生产数量"))
                        {
                            int temp = 0;
                            if (dr[i] != null
                            && dr[i] != DBNull.Value
                            && Convert.ToInt32(dr[i]) > 0)
                            {
                                temp = Convert.ToInt32(dr[i]);
                            }
                            tdTextTemp += "<td>" + temp + "</td>";
                        }
                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                text += string.Format(@"<tr>{1}</tr> ", dr[1], tdTextTemp);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
