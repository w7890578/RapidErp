using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.StoreroomManager
{
    public partial class WarehouseMarerialDeatilList : System.Web.UI.Page
    {
        public static string show = "inline";
        public static string colSpan = "13";
        public static string wNumber = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string number = ToolManager.GetQueryString("number");
                wNumber = number;
                if (number.Equals("ycl"))
                {
                    show = "inline";
                    colSpan = "16";
                }
                else
                {
                    show = "none";
                    colSpan = "13";
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage();
                }
            }
        }

        private static void GetPage()
        {
            int pageCount = 0;//总页数 
            int totalRecords = 0;//总行数
            string error = string.Empty;
            string text = string.Empty;
            string tdTextTemp = string.Empty;
            string pageIndex = ToolManager.GetQueryString("pageIndex");
            string pageSize = ToolManager.GetQueryString("pageSize");
            string sortName = ToolManager.GetQueryString("sortName");
            string sortDirection = ToolManager.GetQueryString("sortDirection");
            string condition = ToolManager.GetQueryString("condition");
            string number = ToolManager.GetQueryString("number");
            string tableName = string.Empty;
            string sql = string.Empty;
            if (number.Equals("ycl"))
            {
                sql = string.Format(@" select t.MaterialNumber ,t.materialName,t.Description
,t.Brand,t.Cargo,t.StockQty,t.三个月库存安全值,
t.Kind,t.Type,t.Unit,t.NumberProperties,t.CargoType from  (
select  distinct ms.MaterialNumber ,ms.StockQty ,mit.materialName,
mit.Description,mit.StockSafeQty as 三个月库存安全值,
mit.SixStockSafeQty as 六个月库存安全值 ,
mit.Brand,mit.Cargo,msp.SupplierMaterialNumber,
mit.Kind,mit.Type,
mit.NumberProperties ,mit.Unit,
mit.CargoType  
from MaterialStock ms 
left join 
MarerialInfoTable mit on ms.MaterialNumber =mit.MaterialNumber
left join MaterialSupplierProperty msp on ms.MaterialNumber =msp.MaterialNumber 
where ms.WarehouseName ='ycl') t ");
                tableName = "MaterialStock";
                show = "inline";
                sql = string.Format("  select distinct * from ({0})a {1} ", sql, condition);
                sql += string.Format(@" union all 
select'合计' ,'',''
,'','',sum(a.StockQty),SUM (a.三个月库存安全值),'','','','','' from ({0}) a
", sql);
            }
            else if (number.Equals("fpk"))
            {
                sql = string.Format(@"select t.MaterialNumber ,t.materialName,t.Description
,t.Brand,t.Cargo,t.StockQty,t.Kind,t.Type,t.Unit,t.NumberProperties,t.CargoType from (
select distinct ms.MaterialNumber ,ms.StockQty ,mit.Description ,
mit.Brand,mit.Cargo,msp.SupplierMaterialNumber,mit.Kind,mit.materialName,
mit.Type,mit.Unit,mit.NumberProperties,mit.CargoType
 from ScrapStock ms 
left join 
MarerialInfoTable mit on ms.MaterialNumber =mit.MaterialNumber
left join MaterialSupplierProperty msp on ms.MaterialNumber =msp.MaterialNumber 
where ms.WarehouseName ='fpk') t
 ");
                tableName = "ScrapStock";
                show = "none";
                sql = string.Format("  select distinct * from ({0})a {1} ", sql, condition);
                sql += string.Format(@" union all 
select'合计' ,'',''
,'','',sum(a.StockQty),SUM (a.三个月库存安全值),'','','','','' from ({0}) a
", sql);
            }
            else if (number.Equals("ypk"))
            {
                sql = string.Format(@"select t.MaterialNumber ,t.materialName,t.Description
,t.Brand,t.Cargo,t.StockQty,t.Kind,t.Type,t.Unit,t.NumberProperties,t.CargoType from (
select distinct ms.MaterialNumber ,ms.StockQty ,mit.Description ,mit.materialName,
mit.Brand,mit.Cargo,msp.SupplierMaterialNumber,
mit.Kind,
mit.Type,mit.Unit,mit.NumberProperties,mit.CargoType
 from SampleStock ms 
left join 
MarerialInfoTable mit on ms.MaterialNumber =mit.MaterialNumber
left join MaterialSupplierProperty msp on ms.MaterialNumber =msp.MaterialNumber 
where ms.WarehouseName ='ypk') t 
 ");
                tableName = "SampleStock";
                show = "none";
                sql = string.Format("  select distinct * from ({0})a {1} ", sql, condition);
                sql += string.Format(@" union all 
select'合计' ,'',''
,'','',sum(a.StockQty),'','','','','' from ({0}) a
", sql);
            }


            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, sql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);

            string temp = string.Empty;
            int columCount = 0;
            if (dt.Rows.Count > 0)
            {
                columCount = dt.Columns.Count;
            }
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    //第一列为序号
                    if (i == 0)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else if (dt.Columns[i].ColumnName.Equals("StockQty"))
                    {
                        string tempQty = dr["StockQty"] == null ? "0" : dr["StockQty"].ToString();
                        if (tempQty.Contains(".00"))
                        {
                            tempQty = tempQty.Replace(".00", "");
                        }
                        tdTextTemp += string.Format("<td>{0}</td>", tempQty);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }

                text += string.Format(@"<tr> {0}  
<td>
 <a href='###' onclick=""Edit('{1}','{2}','{3}')"">编辑</a>
</td></tr>", tdTextTemp, dr["MaterialNumber"], number, tableName);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
            return;
        }

        protected void btnExp_Click(object sender, EventArgs e)
        {
            //            string condition = saveInfo.Value;
            //            string number = wNumber;
            //            string tableName = string.Empty;
            //            string sql = string.Empty;
            //            string excelName = "";
            //            if (number.Equals("ycl"))
            //            {
            //                sql = string.Format(@" select t.MaterialNumber ,t.materialName,t.Description
            //,t.Brand,t.Cargo,t.StockQty,t.三个月库存安全值,
            //t.Kind,t.Type,t.Unit,t.NumberProperties,t.CargoType from  (
            //select  distinct ms.MaterialNumber ,ms.StockQty ,
            //mit.Description,mit.StockSafeQty as 三个月库存安全值,
            //mit.SixStockSafeQty as 六个月库存安全值 ,
            //mit.Brand,mit.Cargo,msp.SupplierMaterialNumber,
            //mit.Kind,mit.Type,
            //mit.NumberProperties ,mit.Unit,mit.materialName,
            //mit.CargoType  
            //from MaterialStock ms 
            //left join 
            //MarerialInfoTable mit on ms.MaterialNumber =mit.MaterialNumber
            //left join MaterialSupplierProperty msp on ms.MaterialNumber =msp.MaterialNumber 
            //where ms.WarehouseName ='ycl') t ");
            //                tableName = "MaterialStock";
            //                excelName = "原材料库存";
            //                show = "inline";
            //                sql = string.Format("  select distinct * from ({0})a {1} ", sql, condition);
            //                sql += string.Format(@" union all 
            //select'合计' ,''
            //,'','',sum(a.StockQty),SUM (a.三个月库存安全值),'','','','','' from ({0}) a
            //", sql);
            //            }
            //            else if (number.Equals("fpk"))
            //            {
            //                sql = string.Format(@"select t.MaterialNumber ,t.materialName,t.Description
            //,t.Brand,t.Cargo,t.StockQty,t.Kind,t.Type,t.Unit,t.NumberProperties,t.CargoType from (
            //select distinct ms.MaterialNumber ,ms.StockQty ,mit.Description ,
            //mit.Brand,mit.Cargo,msp.SupplierMaterialNumber,mit.Kind,mit.materialName,
            //mit.Type,mit.Unit,mit.NumberProperties,mit.CargoType
            // from ScrapStock ms 
            //left join 
            //MarerialInfoTable mit on ms.MaterialNumber =mit.MaterialNumber
            //left join MaterialSupplierProperty msp on ms.MaterialNumber =msp.MaterialNumber 
            //where ms.WarehouseName ='fpk') t
            // ");
            //                tableName = "ScrapStock";
            //                excelName = "废品库存";
            //                show = "none";
            //                sql = string.Format("  select distinct * from ({0})a {1} ", sql, condition);
            //                sql += string.Format(@" union all 
            //select'合计' ,''
            //,'','',sum(a.StockQty),SUM (a.三个月库存安全值),'','','','','' from ({0}) a
            //", sql);
            //            }
            //            else if (number.Equals("ypk"))
            //            {
            //                sql = string.Format(@"select t.MaterialNumber ,t.materialName,t.Description
            //,t.Brand,t.Cargo,t.StockQty,t.Kind,t.Type,t.Unit,t.NumberProperties,t.CargoType from (
            //select distinct ms.MaterialNumber ,ms.StockQty ,mit.Description ,mit.materialName,
            //mit.Brand,mit.Cargo,msp.SupplierMaterialNumber,
            //mit.Kind,
            //mit.Type,mit.Unit,mit.NumberProperties,mit.CargoType
            // from SampleStock ms 
            //left join 
            //MarerialInfoTable mit on ms.MaterialNumber =mit.MaterialNumber
            //left join MaterialSupplierProperty msp on ms.MaterialNumber =msp.MaterialNumber 
            //where ms.WarehouseName ='ypk') t 
            // ");
            //                tableName = "SampleStock";
            //                excelName = "样品库存";
            //                show = "none";
            //                sql = string.Format("  select distinct * from ({0})a {1} ", sql, condition);
            //                sql += string.Format(@" union all 
            //select'合计' ,''
            //,'','',sum(a.StockQty),'','','','','' from ({0}) a
            //", sql);
            //            }
            string excelName = "";
            string error = string.Empty;
            string text = string.Empty;
            string tdTextTemp = string.Empty;
            string pageIndex = ToolManager.GetQueryString("pageIndex");
            string pageSize = ToolManager.GetQueryString("pageSize");
            string sortName = ToolManager.GetQueryString("sortName");
            string sortDirection = ToolManager.GetQueryString("sortDirection");
            string condition = ToolManager.GetQueryString("condition");
            string number = ToolManager.GetQueryString("number");
            string tableName = string.Empty;
            string sql = string.Empty;
            if (number.Equals("ycl"))
            {
                sql = string.Format(@" select 
t.MaterialNumber as 原材料编号,
t.materialName as 型号,
t.Description as 描述,
t.Brand as 品牌,
t.Cargo as 货位, 
t.StockQty as 库存数量,
t.三个月库存安全值,
t.Kind as 种类,
t.Type as 类别,
t.Unit as 单位,
t.NumberProperties as 编号属性,
t.CargoType  as 货物类别 
from  (
select  distinct ms.MaterialNumber ,ms.StockQty ,mit.materialName,
mit.Description,mit.StockSafeQty as 三个月库存安全值,
mit.SixStockSafeQty as 六个月库存安全值 ,
mit.Brand,mit.Cargo,msp.SupplierMaterialNumber,
mit.Kind,mit.Type,
mit.NumberProperties ,mit.Unit,
mit.CargoType  
from MaterialStock ms 
left join 
MarerialInfoTable mit on ms.MaterialNumber =mit.MaterialNumber
left join MaterialSupplierProperty msp on ms.MaterialNumber =msp.MaterialNumber 
where ms.WarehouseName ='ycl') t ");
                tableName = "MaterialStock";
                excelName = "原材料库存";
                show = "inline";
                sql = string.Format("  select distinct * from ({0})a {1} ", sql, condition);
                sql += string.Format(@" union all 
select'合计' ,'',''
,'','',sum(a.库存数量),SUM (a.三个月库存安全值),'','','','','' from ({0}) a
", sql);
            }
            else if (number.Equals("fpk"))
            {
                sql = string.Format(@"select 
t.MaterialNumber as 原材料编号,
t.materialName as 型号,
t.Description as 描述,
t.Brand as 品牌,
t.Cargo as 货位, 
t.StockQty as 库存数量,
t.三个月库存安全值,
t.Kind as 种类,
t.Type as 类别,
t.Unit as 单位,
t.NumberProperties as 编号属性,
t.CargoType  as 货物类别  
from (
select distinct ms.MaterialNumber ,ms.StockQty ,mit.Description ,
mit.Brand,mit.Cargo,msp.SupplierMaterialNumber,mit.Kind,mit.materialName,
mit.Type,mit.Unit,mit.NumberProperties,mit.CargoType
 from ScrapStock ms 
left join 
MarerialInfoTable mit on ms.MaterialNumber =mit.MaterialNumber
left join MaterialSupplierProperty msp on ms.MaterialNumber =msp.MaterialNumber 
where ms.WarehouseName ='fpk') t
 ");
                tableName = "ScrapStock";
                excelName = "废品库存";
                show = "none";
                sql = string.Format("  select distinct * from ({0})a {1} ", sql, condition);
                sql += string.Format(@" union all 
select'合计' ,'',''
,'','',sum(a.库存数量),SUM (a.三个月库存安全值),'','','','','' from ({0}) a
", sql);
            }
            else if (number.Equals("ypk"))
            {
                sql = string.Format(@"select
t.MaterialNumber as 原材料编号,
t.materialName as 型号,
t.Description as 描述,
t.Brand as 品牌,
t.Cargo as 货位, 
t.StockQty as 库存数量, 
t.Kind as 种类,
t.Type as 类别,
t.Unit as 单位,
t.NumberProperties as 编号属性,
t.CargoType  as 货物类别 
from (
select distinct ms.MaterialNumber ,ms.StockQty ,mit.Description ,mit.materialName,
mit.Brand,mit.Cargo,msp.SupplierMaterialNumber,
mit.Kind,
mit.Type,mit.Unit,mit.NumberProperties,mit.CargoType
 from SampleStock ms 
left join 
MarerialInfoTable mit on ms.MaterialNumber =mit.MaterialNumber
left join MaterialSupplierProperty msp on ms.MaterialNumber =msp.MaterialNumber 
where ms.WarehouseName ='ypk') t 
 ");
                tableName = "SampleStock";
                excelName = "样品库存";
                show = "none";
                sql = string.Format("  select distinct * from ({0})a {1} ", sql, condition);
                sql += string.Format(@" union all 
select'合计' ,'',''
,'','',sum(a.库存数量),'','','','','' from ({0}) a
", sql);
            }


            ToolCode.Tool.ExpExcel(sql, excelName);
        }


    }
}
