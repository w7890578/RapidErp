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
    public partial class ProjectInfoList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string keys = ToolManager.GetQueryString("ids");
                    Delete(keys);
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("EditProjectInfoList.aspx", "btnSearch", "290", "700");
                }
            }
        }


        private void GetPage(string editUrl, string btnId, string height, string width)
        {

            int pageCount = 0;//总页数 
            int totalRecords = 0;//总行数
            string primaryKeyStr = string.Empty; //主键字符串
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
            if (dt.Columns.Contains("rownum"))
            {
                dt.Columns.Remove("rownum");
            }
            foreach (DataRow dr in dt.Rows)
            {

                tdTextTemp = "";
                for (int i = 0; i < columCount-1; i++)
                {
                    tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                }
                primaryKeyStr = GetprimaryKeyStr(dr);
                text += string.Format(@"<tr>
<td>
<input type='checkbox' name='subBox' value='{0}'/></td>{1}  
<td>
<a href='###'   onclick=""OpenDialog('{2}?Id={0}&date={3}','{4}','{5}','{6}')""> 
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>
</td></tr>", primaryKeyStr, tdTextTemp, editUrl, DateTime.Now.ToString(), btnId, height, width);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 获取主键字段
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private string GetprimaryKeyStr(DataRow dr)
        {
            string primaryKeyStr = string.Empty; //主键字符串
            primaryKeyStr = dr["项目名称"] == null ? "" : dr["项目名称"].ToString() + "|";
            primaryKeyStr += dr["产成品编号"] == null ? "" : dr["产成品编号"].ToString() + "|";
            primaryKeyStr += dr["版本"] == null ? "" : dr["版本"].ToString() + "|";
            primaryKeyStr += dr["客户产成品编号"] == null ? "" : dr["客户产成品编号"].ToString();
            return primaryKeyStr;
        }


        private void Delete(string keys)
        {
            string[] temp = keys.Split(',');
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string sql = string.Empty;
            foreach (string str in temp)
            {
                string[] keyTemp = str.Split('|');
                sql = string.Format(@"delete T_ProjectInfo where ProjectName ='{0}' and ProductNumber ='{1}' 
and Version ='{2}' and CustomerProductNumber='{3}'", keyTemp[0], keyTemp[1], keyTemp[2], keyTemp[3]);
                sqls.Add(sql);
            }
            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            if (result)
            {
                Response.Write("1");
                Response.End();
                return;
            }
            else
            {
                Response.Write(error);
                Response.End();
                return;
            }
        }

    }
}
