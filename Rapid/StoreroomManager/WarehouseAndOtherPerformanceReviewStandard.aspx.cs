using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Rapid.ToolCode;
using DAL;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class WarehouseAndOtherPerformanceReviewStandard : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0309", "Add");
            divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0309", "Delete");
            //删除
            if (ToolManager.CheckQueryString("ids"))
            {

                string ids = ToolManager.GetQueryString("ids");
                string error = string.Empty;
                List<string> sqls = new List<string>();
                string sqlOne = string.Format(@" delete PerformanceReviewStandard where Guid in ({0})", ids);
                sqls.Add(sqlOne);
                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除考核标准", "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除考核标准", "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }
            //查询
            if (ToolManager.CheckQueryString("pageIndex"))
            {
                GetPage("AddOrEditWarehouseAndOtherPerformance.aspx", "btnSearch", "320", "600");

            }
        }
        private void GetPage(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0309", "Edit");
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
            int columCount = dt.Columns.Count;
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    if (i == 0 || i == 8)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else if (dt.Columns[i].ColumnName.Equals("考核类型"))
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                text += string.Format(@"<tr><td>
<input type='checkbox' name='subBox' value='{9}'/></td>{1}  
<td>
<span style='display:{10};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?StandardName={3}&PerformanceReviewItem={4}&date={5}','{6}','{7}','{8}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span></td></tr>",

      dr[1], tdTextTemp, url, Server.UrlEncode(dr[1].ToString()), Server.UrlEncode(dr[2].ToString()), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, dr[8], hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
