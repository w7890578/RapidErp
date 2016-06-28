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
    public partial class HalfProductWarehouseLogDetailList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0408", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0408", "Delete");
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string ids = ToolManager.GetQueryString("ids");
                    string temp = HalfProductWarehouseLogManager.DeleteDetail(ToolManager.GetQueryString("ids"));
                    bool result = temp == "1" ? true : false;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除半成品入库信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除半成品入库信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }

                }
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("AddOrEditHalfInProductWarehouseLogDetail.aspx", "btnSearch", "350", "500");
                }
              
            }
        }
        private void GetPage(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0408", "Edit");
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
                    if (i == 0||i==10)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                text += string.Format(@"<tr><td>
<input type='checkbox' name='subBox' value='{8}'/></td>{1}  
<td>
<span style='display:{7};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?Guid={8}&WarehouseNumber={9}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span>
</td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, hasEdit,dr["Guid"],dr["入库编号"]);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

    }
}

