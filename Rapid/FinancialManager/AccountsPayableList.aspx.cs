using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.FinancialManager
{
    public partial class AccountsPayableList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    divExp.Visible = ToolCode.Tool.GetUserMenuFunc("L0501", "Exp");
                    GetPage("AddOrEditAccountsPayable.aspx", "Transfer", "btnSearch", "620", "500");
                }
            }
        }


        private static void GetPage(string editUrl, string detailFunctionForJS, string btnId, string height, string width)
        {
            string hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0501", "Edit");
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
                        if (dt.Columns[i].ColumnName.Equals("Guid"))
                        {
                            tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                        }
                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                text += string.Format(@"<tr><td style='display:none;'>
<input type='checkbox' name='subBox' value='{0}'/></td>{1}  
<td>
<span style='display:{11};'>
<span style='display:{7};'    onclick=""OpenDialog('{2}?Id={0}&date={3}&OrdersNumber={4}&MaterialNumber={5}&CreateTime={6}','{7}','{8}','{9}')""> 
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label   >编辑</label> <span >]</span></span></span>


</td></tr>", dr[1], tdTextTemp, editUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), dr["销售订单号"], dr["原材料编号"], dr["创建时间"], btnId, height, width, dr[6],hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string sql = saveInfo.Value;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "应付账款表");
        }
    }
}
