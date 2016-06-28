using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.FinancialManager
{
    public partial class Report_ProductWarehouseCurrentAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //查询
            if (ToolManager.CheckQueryString("pageIndex"))
            {
                GetPageOperation("btnSearch");
            }
        }

        private void GetPageOperation(string btnId)
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
            string querySql = ToolManager.GetQueryString("querySql");
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);

            if (dt.Columns.Contains("guid"))
            {
                dt.Columns.Remove("guid");
            }
            int columCount = dt.Columns.Count;
            //if(dt.Columns.Contains ("GUID"))
            //{}
            // if(dt.Columns .Contains (""))
            // {}
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    if (i == 0)
                    {
                        tdTextTemp += "";
                        // tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        //if (dt.Columns[i].ColumnName.Equals("平衡结果") && dr[i].ToString().Equals("需要采购"))
                        //{
                        //    tdTextTemp += string.Format("<td > <label class='labelRed'>{0}</label></td>", dr[i]);
                        //}
                        ////MaterialActualQtyDetail
                        //else if (dt.Columns[i].ColumnName.Equals("实际需求数"))
                        //{
                        //    tdTextTemp += string.Format("<td >  <a title='点击查看明细' href='MaterialActualQtyDetail.aspx?MaterialNumber={1}'>{0}</a></td>", dr[i], dr["原材料编号"]);
                        //}
                        //else if (dt.Columns[i].ColumnName.Equals("在途数量"))
                        //{
                        //    tdTextTemp += string.Format("<td >  <a title='点击查看在途明细' href='ZTDetail.aspx?MateriNumber={1}'>{0}</a></td>", dr[i], dr["原材料编号"]);
                        //}
                        //else if (dt.Columns[i].ColumnName.Equals("订单需求数"))
                        //{
                        //    tdTextTemp += string.Format("<td >  <a target='_blank' title='点击查看订单需求明细' href='DDDetail.aspx?MateriNumber={1}'>{0}</a></td>", dr[i], dr["原材料编号"]);
                        //}
                        //else if (dt.Columns[i].ColumnName.Equals("实际库存数量"))
                        //{
                        //    tdTextTemp += string.Format("<td >  <a   title='公式：MRP中的库存数量-原材料库房出入库类型为“生产出库、包装出库、损耗出库、销售出库（贸易）、维修出库”的且未点确认的出库单中的“数量”' href='###'>{0}</a></td>", dr[i] );
                        //}
                        //else
                        //{

                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        //}
                    }
                }
                text += string.Format(@"<tr>
              
                 {1}    </tr>", dr[1], tdTextTemp);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string sql = saveInfo.Value;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            DataTable dt = SqlHelper.GetTable(sql);
            ExcelHelper.Instance.ExpExcel(dt, "产成品库房流水账金额统计表");
        }
    }
}