using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class MateialWarehouseCurrentAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("EditMateialWarehouseCurrentAccount.aspx", "btnSearch", "180", "400");
                }
            }


        }

        private void GetPage(string url, string btnId, string height, string width)
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
            string editName = string.Empty;
            string temp = string.Empty;
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
                        if (dt.Columns[i].ColumnName.Equals("guid"))
                        {
                            tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                        }

                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                temp = string.Format(@"<input type='checkbox' name='subBox' value='{0}'/>", dr["guid"]);
                if (dr["原材料编号"].ToString().Equals("合计"))
                {
                    editName = "";
                    temp = "";
                }
                else
                {
                    editName = "编辑 ";
                }



                text += string.Format(@"<tr><td>
{8}</td>{1}  
<td>

<a href='###'  value='{0}' onclick=""OpenDialog('{2}?guid={0}&date={3}','{4}','{5}','{6}')"">
{7}</a>
&nbsp;&nbsp;<a href='###' onclick=""sysEdit('{0}')"" style='display:{9};' >编辑各项数据</a>
</td></tr>", dr["guid"], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, editName, temp,
                ToolCode.Tool.GetUser().UserNumber.Equals("sysAdmin") ? "inline" : "none");

            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string sql = txtSql.Text;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "原材料库房流水帐");

        }




    }
}
