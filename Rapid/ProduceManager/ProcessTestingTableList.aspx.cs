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
    public partial class ProcessTestingTableList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0306", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0306", "Delete");
                divPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0306", "Delete");
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string temp = ProcessTestingTableManager.DeleteMarerialScrapLog(ToolManager.GetQueryString("ids"));
                    string ids = ToolManager.GetQueryString("ids");
                    bool restult = temp == "1" ? true : false;
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除过程检验信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除过程检验信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageOperate("btnSearch", "AddOrEditProcessTestingTable.aspx", "320", "600");
                }
            }
        }
        //隐藏一列编号
        public static void GetPageOperate(string btnId, string editUrl,string height,string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0306", "Edit");
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
                    if (i == 0 || dt.Columns[i].ColumnName.Equals("编号") || dt.Columns[i].ColumnName.Equals("上传路径"))
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else if (dt.Columns[i].ColumnName.Equals("文件名"))
                    {
                        tdTextTemp += string.Format("<td><a  target ='_blank' style='color:blue;' href='{1}'>{0}</a></td>", dr[i], dr["上传路径"]);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                text += string.Format(@"<tr>
                        <td><input type='checkbox' name='subBox' value='{0}'/></td>{1}    
                        <td><span > [</span>   <label class='edit' id='{0}'>编辑</label> <span >]</span></td>                       
</tr>", dr[1], tdTextTemp, editUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), btnId, height, width,hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();

//             <td>
//<span style='display:{7};'>
//<a href='###'  value='{0}' onclick=""OpenDialog('{2}?Id={0}&date={3}','{4}','{5}','{6}')""> 
//                       <img src='../Img/037.gif' width='9' height='9' />
//                      <span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span>
//                        </td>
        }
    }
}
