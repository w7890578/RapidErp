using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;
using System.Web.Util;

namespace Rapid.ProduceManager
{
    public partial class ExaminationLogList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //删除
            if (ToolManager.CheckQueryString("ids"))
            {
                string temp = ExaminationLogManager.DeleteData(ToolManager.GetQueryString("ids"));
                bool result = temp == "1" ? true : false;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除员工考试成绩" + ToolManager.CheckQueryString("ids"), "删除成功");
                    Response.Write(temp);
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除员工考试成绩" + ToolManager.CheckQueryString("ids"), "删除失败！原因" + temp);
                    Response.Write(temp);
                    Response.End();
                    return;
                }
            }
            //查询
            if (ToolManager.CheckQueryString("pageIndex"))
            {
                GetPageRepeater("EditExaminationLogList.aspx", "btnSearch", "320", "550");
            }
        }
        /// <summary>隐藏一列编号
        /// 获取数据通用方法
        /// </summary>
        /// <param name="url">请求的页面路径</param>
        /// <param name="btnName">查询按钮Id</param>
        /// <param name="height">编辑弹出窗口高度</param>
        /// <param name="width">编辑弹出窗口宽度</param>
        public static void GetPageRepeater(string url, string btnId, string height, string width)
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
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, "order by 年度 desc ,月份 desc ,班组 asc ,姓名 asc", ref totalRecords);
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
                text += string.Format(@"<tr> {0}  
<td> <a href='###' onclick=""Edit('{1}','{2}','{3}')"">编辑</a></td></tr>", tdTextTemp, dr["年度"], dr["月份"], dr["姓名"]);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
