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
    public partial class ScarpWarehouseLogList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0402", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0402", "Delete");
                divAudit.Visible = ToolCode.Tool.GetUserMenuFunc("L0402", "Audit");
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string temp = ScarpWarehouseLogManager.DeleteData(ToolManager.GetQueryString("ids"));
                    string ids = ToolManager.GetQueryString("ids");
                    bool restult = temp == "1" ? true : false;
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除废品出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除废品出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }

                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("AddOrEditScarpWarehouseLog.aspx", "btnSearch", "350", "600");
                }
                //审核
                if (ToolManager.CheckQueryString("check"))
                {
                    string temp = ScarpWarehouseLogManager.check(ToolManager.GetQueryString("check"));
                    string check = ToolManager.GetQueryString("check");
                    bool restult = temp == "1" ? true : false;
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "审核废品出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(check), "审核成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "审核废品出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(check), "审核失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
            }
        }
        private void GetPage(string url, string btnId, string height, string width)
        {
            //hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0402", "Edit");
            int pageCount = 0;//总页数 
            int totalRecords = 0;//总行数
            string error = string.Empty;
            string text = string.Empty;
            string tdTextTemp = string.Empty;
            string tempTemp = string.Empty;
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
                tempTemp = dr["审核时间"] == null ? "" : dr["审核时间"].ToString();
                if (string.IsNullOrEmpty(tempTemp))
                {
                    tempTemp = string.Format("<input type='checkbox' name='subBox' value='{0}'/>", dr[1]);
                    hasEdit = "inline";
                }
                else
                {
                    tempTemp = "";
                    hasEdit = "none";
                }
                text += string.Format(@"<tr><td>{8}
</td>{1}  
<td>
<span style='display:{7};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?WarehouseNumber={0}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span>


<a href='ScarpWarehouseLogDetail.aspx?WarehouseNumber={0}' > 
<img src='../Img/detail.png' width='9' height='9' />
 [ <span class='edit'>详细</span> ]</a>
</td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, hasEdit, tempTemp);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

    }
}
