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
    public partial class WarehouseInOutTypeList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0403", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0403", "Delete");
                string error = string.Empty;
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    error = string.Empty;
                    List<string> sqls = new List<string>();
                    string sqlOne = string.Format(@" delete WarehouseInOutType where guid in ({0})", ToolManager.GetQueryString("ids"));
                    sqls.Add(sqlOne);
                    bool restult = SqlHelper.BatchExecuteSql(sqls, ref error);
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除出入库类型", "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除出入库类型", "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("AddOrEditWarehouseInOutType.aspx", "btnSearch", "180", "350");
                }
            }
        }
        private void GetPage(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0403", "Edit");
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
                    if (i == 0 || i == 4)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                text += string.Format(@"<tr><td>
<input type='checkbox' name='subBox' value='{10}'/></td>{1}  
<td>
<span style='display:{11};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?WarehouseInOutType={3}&ChangeDirection={4}&InOutType={5}&date={6}','{7}','{8}','{9}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>
</span>
</td></tr>", dr[1], tdTextTemp,
 url, Server.UrlEncode(dr[1].ToString()), Server.UrlEncode(dr[2].ToString()), Server.UrlEncode(dr[3].ToString()), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId,
  height, width, dr[4],hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
