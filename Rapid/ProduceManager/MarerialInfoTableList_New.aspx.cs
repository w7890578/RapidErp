using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class MarerialInfoTableList_New : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (ToolManager.CheckQueryString("ChoosedVlaue"))
                {
                    string selectedValuePid = ToolManager.GetQueryString("ChoosedVlaue");
                    string sql = string.Format(@" select Type from MareriaType where Pid='{0}'", selectedValuePid);
                    Response.Write(ControlBindManager.GetOption(sql, "Type", "Type"));
                    Response.End();
                    return;
                }
                //divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0301", "Add");
                //divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0301", "Delete");
                //divImp.Visible = ToolCode.Tool.GetUserMenuFunc("L0301", "Imp");
                //divExp.Visible = ToolCode.Tool.GetUserMenuFunc("L0301", "Exp");
                //divPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0301", "Print");

            }


            //删除
            if (ToolManager.CheckQueryString("ids"))
            {

                string temp = MarerialInfoTableManager.DeleteData(Server.UrlDecode(Server.UrlDecode(ToolManager.GetQueryString("ids"))));

                string ids = ToolManager.GetQueryString("ids");
                bool result = temp == "1" ? true : false;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除原材料信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                    Response.Write(temp);
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除原材料信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + temp);
                    Response.Write(temp);
                    Response.End();
                    return;
                }
            }
            //查询
            if (ToolManager.CheckQueryString("pageIndex"))
            {
                GetPage("AddOrEditMarerialInfoTable.aspx", "MarerialInfoTableDetailedList.aspx", "btnSearch", "550", "600");
            }

        }
        public void GetPage(string editUrl, string detailFunctionForJS, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0112", "Edit");
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
                    //第一列为序号
                    if (i == 0)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        //Guid列
                        if (dt.Columns[i].ColumnName.Equals("Guid") || dt.Columns[i].ColumnName.Equals("废品仓位") ||
                            dt.Columns[i].ColumnName.Equals("原材料仓位") || dt.Columns[i].ColumnName.Equals("6个月库存安全值")
                             || dt.Columns[i].ColumnName.Equals("采购价格")
                            )
                        {

                        }
                        else if (dt.Columns[i].ColumnName.Equals("描述"))
                        {

                            tdTextTemp += string.Format("<td style ='width :150px;'>{0}</td>", dr[i]);
                        }

                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                string id = dr[1].ToString();
                text += string.Format(@"<tr><td>
<input type='checkbox' name='subBox' value='{0}'/></td>{1}  
<td>
<span style='display:{8};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?Id={0}&date={3}','{4}','{5}','{6}')""> 
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>
</span>
</td></tr>", Server.UrlEncode(id), tdTextTemp, editUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), btnId, height, width, detailFunctionForJS, hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
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
            ToolCode.Tool.ExpExcel(sql, "原材料信息");

        }
    }
}
