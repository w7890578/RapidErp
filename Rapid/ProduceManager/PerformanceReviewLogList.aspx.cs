using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class PerformanceReviewLogList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0310", "Add");
            divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0310", "Delete");
            //删除
            if (ToolManager.CheckQueryString("ids"))
            {

                string ids = ToolManager.GetQueryString("ids");
                string error = string.Empty;
                List<string> sqls = new List<string>();
                string sqlOne = string.Format(@" delete PerformanceReviewLog where Guid in ({0})", ids);
                sqls.Add(sqlOne);
                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除员工绩效信息", "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除员工绩效信息", "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }
            //查询
            if (ToolManager.CheckQueryString("pageIndex"))
            {
                GetPage("AddOrEditPerformanceReviewLog.aspx", "btnSearch", "440", "600");

            }
        }


        private void GetPage(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0310", "Edit");
            int pageCount = 0;//总页数 
            int totalRecords = 0;//总行数
            string error = string.Empty;
            string text = string.Empty;
            string tdTextTemp = string.Empty;
            string pageIndex = ToolManager.GetQueryString("pageIndex");
            string pageSize = ToolManager.GetQueryString("pageSize");
            string sortName = ToolManager.GetQueryString("sortName");
            string sortDirection = ToolManager.GetQueryString("sortDirection");
            sortName = "年度";
            sortDirection = "desc";
            string querySql = ToolManager.GetQueryString("querySql");
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, "order by 年度 desc,月份 desc ,姓名 asc,序号 asc", ref totalRecords);
            int columCount = dt.Columns.Count;
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    if (i == 0 || i == 12)
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
<input type='checkbox' name='subBox' value='{11}'/></td>{1}  
<td>
<span style='display:{12};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?Year={3}&Month={4}&PerformanceReviewItem={5}&Name={6}&date={7}','{8}','{9}','{10}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>
</span>
</td></tr>",

      dr[1], tdTextTemp, url, dr[1], dr[2], Server.UrlEncode(dr[3].ToString()), Server.UrlEncode(dr[4].ToString()), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, dr["Guid"], hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
