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
    public partial class MarerialScrapLogList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0407", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0407", "Delete");
                divExp.Visible = ToolCode.Tool.GetUserMenuFunc("L0407", "Exp");
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string ids = ToolManager.GetQueryString("ids");
                    string temp = MarerialScrapLogManager.DeleteMarerialScrapLog(ToolManager.GetQueryString("ids"));
                    bool restult = temp == "1" ? true : false;
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除原材料报废信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除原材料报废信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageOperate("btnSearch", "AddOrEditMarerialScrapLog.aspx", "MarerialScrapLogDetailedList.aspx");
                }
            }
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            string sql = saveInfo.Value;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "原材料报废上报表");

        }

        //隐藏一列编号
        public static void GetPageOperate(string btnId, string editUrl, string detailedUrl)
        {
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
            string querySql = ToolManager.GetQueryString("querySql");
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);
            int columCount = dt.Columns.Count;
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    if (i == 0||i == 1)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                 text += string.Format(@"<tr>
<td><input type='checkbox' name='subBox' value='{0}'/></td>
{1}    
                        <td> 
<span style='display:{5};'>
<a href='###'   onclick=""OpenDialog('{2}?Id={0}&date={4}','{6}')"">  
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span> <label class='edit'>编辑</label>  <span >]</span>
</a>&nbsp;
<a href='{3}?Id={0}' class='edit'> <img src='../Img/037.gif' width='9' height='9' /><span ></span></a></td></tr>", dr[1], tdTextTemp, editUrl, detailedUrl,DateTime .Now,hasEdit,btnId );
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
