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
    public partial class PackageInfoList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = string.Empty;
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0304", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0304", "Delete");
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string ids = ToolManager.GetQueryString("ids");
                    string error = string.Empty;
                    List<string> sqls = new List<string>();
                    sql = string.Format(@" delete PackageAndProductRelation where PackageNumber in ({0})", ids);
                    sqls.Add(sql);
                    sql = string.Format(@" delete PackageInfoCustomerProperty where PackageNumber in ({0})", ids);
                    sqls.Add(sql);
                    sql = string.Format(@" delete PackageInfo where PackageNumber in ({0})", ids);
                    sqls.Add(sql);
                    bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除包信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除包信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("AddOrEditPackageInfo.aspx", "btnSearch", "200", "600");
                }
            }
        }

        private void GetPage(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0304", "Edit");
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
                    if (i == 0)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                text += string.Format(@"<tr><td>
<input type='checkbox' name='subBox' value='{0}'/></td>{1}  
<td>
<span style='display:{7};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?PackageNumber={0}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span>

<a href='PackageAndProductRelationList.aspx?PackageNumber={0}' > 
<img src='../Img/detail.png' width='9' height='9' />
 [ <span class='edit'>详细</span> ]</a>
</td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
