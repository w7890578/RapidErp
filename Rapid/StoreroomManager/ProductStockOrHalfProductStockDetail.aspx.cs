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
    public partial class ProductStockOrHalfProductStockDetail : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        public static string showQLNumber = "none";
        public static string wNumber = ToolManager.GetQueryString("number");
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageOperate();
                }
                string warehousenumber = ToolManager.GetQueryString("number");
                wNumber = warehousenumber;
                saveWareNumber.Value = wNumber;
                if (warehousenumber == "cpk")
                {
                    showQLNumber = "none";
                }
                else
                {
                    showQLNumber = "inline";
                }
            }
        }


        //隐藏一列编号
        public void GetPageOperate()
        {
            //saveWareNumber.Value = ToolManager.GetQueryString("WarehouseNumber");
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0407", "Edit");
            int pageCount = 0;//总页数 
            int totalRecords = 0;//总行数
            string error = string.Empty;
            string text = string.Empty;
            string tdTextTemp = string.Empty;
            string pageIndex = ToolManager.GetQueryString("pageIndex");
            string pageSize = ToolManager.GetQueryString("pageSize");
            string sortName = ToolManager.GetQueryString("sortName");
            string sortDirection = ToolManager.GetQueryString("sortDirection");
            string condition = ToolManager.GetQueryString("querySql");
            string warehousenumber = ToolManager.GetQueryString("WarehouseNumber");

            string querySql = string.Empty;
            string tabname = string.Empty;
            //querySql += "and WarehouseNam='" + warehousenumber + "'";
            if (warehousenumber == "cpk")
            {
                querySql = string.Format(@"select distinct ps.ProductNumber as 产成品编号,ps.Version as 版本
,'' as 缺料原材料编号,p.Description as 描述,
p.Cargo as 货位,ps.StockQty as 数量,tpi.ProjectName as 项目,
p.Type as 类型,p.Unit as 单位,p.NumberProperties as 编号属性,p.Remark as 备注
from ProductStock ps 
left join Product p on ps.ProductNumber =p.ProductNumber and ps.Version =p.Version 
left join T_ProjectInfo tpi on ps.ProductNumber =tpi.ProductNumber and ps.Version =tpi.Version 
  and ps.WarehouseName='cpk'
  ");
                tabname = "ProductStock";

            }
            else if (warehousenumber == "bcpk")
            {
                querySql = string.Format(@"    select  distinct hp .ProductNumber as 产成品编号, hp .Version as 版本,
hp.MaterialNumber as 缺料原材料编号,p.Description as 描述,
p.Cargo as 货位, hp .StockQty as 数量,tpi.ProjectName as 项目,
p.Type as 类型,p.Unit as 单位,p.NumberProperties as 编号属性,p.Remark as 备注
from HalfProductStock hp 
left join Product p on  hp .ProductNumber =p.ProductNumber and  hp .Version =p.Version 
left join T_ProjectInfo tpi on  hp .ProductNumber =tpi.ProductNumber and  hp .Version =tpi.Version 
 and hp.WarehouseName='bcpk'
 ");
                tabname = "HalfProductStock";

            }
            querySql = string.Format(" select * from ({0})t {1}", querySql, condition);
            querySql += string.Format(@" union all 
 select '合计',''
,'','',
'',sum(a.数量) ,'',
'','','','' from ({0}) a
", querySql);
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);

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
                    if (i == 0)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else if (dt.Columns[i].ColumnName.Equals("缺料原材料编号"))
                    {
                        tdTextTemp += string.Format("<td style='display:{1};'>{0}</td>", dr[i], showQLNumber);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                text += string.Format(@"<tr> {0}  
<td>
 <a href='###' onclick=""Edit('{1}','{2}','{3}','{4}')"">编辑</a>
</td></tr>", tdTextTemp, warehousenumber, dr["产成品编号"], dr["版本"], tabname);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        protected void btnExp_Click(object sender, EventArgs e)
        {
            string condition = saveInfo.Value;
            string warehousenumber = ToolManager.GetQueryString("number");
            string querySql = string.Empty;
            string tabname = string.Empty;
            //querySql += "and WarehouseNam='" + warehousenumber + "'";
            if (warehousenumber == "cpk")
            {
                querySql = string.Format(@"select distinct ps.ProductNumber as 产成品编号,ps.Version as 版本
,'' as 缺料原材料编号,p.Description as 描述,
p.Cargo as 货位,ps.StockQty as 数量,tpi.ProjectName as 项目,
p.Type as 类型,p.Unit as 单位,p.NumberProperties as 编号属性,p.Remark as 备注
from ProductStock ps 
left join Product p on ps.ProductNumber =p.ProductNumber and ps.Version =p.Version 
left join T_ProjectInfo tpi on ps.ProductNumber =tpi.ProductNumber and ps.Version =tpi.Version 
  and ps.WarehouseName='cpk'
  ");
                tabname = "ProductStock";

            }
            else if (warehousenumber == "bcpk")
            {
                querySql = string.Format(@"    select  distinct hp .ProductNumber as 产成品编号, hp .Version as 版本,
hp.MaterialNumber as 缺料原材料编号,p.Description as 描述,
p.Cargo as 货位, hp .StockQty as 数量,tpi.ProjectName as 项目,
p.Type as 类型,p.Unit as 单位,p.NumberProperties as 编号属性,p.Remark as 备注
from HalfProductStock hp 
left join Product p on  hp .ProductNumber =p.ProductNumber and  hp .Version =p.Version 
left join T_ProjectInfo tpi on  hp .ProductNumber =tpi.ProductNumber and  hp .Version =tpi.Version 
 and hp.WarehouseName='bcpk'
 ");
                tabname = "HalfProductStock";

            }
            querySql = string.Format(" select * from ({0})t {1}", querySql, condition);
            querySql += string.Format(@" union all 
 select '合计',''
,'','',
'',sum(a.数量) ,'',
'','','','' from ({0}) a
", querySql);
            ToolCode.Tool.ExpExcel(querySql, warehousenumber.Equals("cpk") ? "产成品库存" : "半成品库存");
        }


    }
}
