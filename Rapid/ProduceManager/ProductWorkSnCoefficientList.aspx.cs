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
    public partial class ProductWorkSnCoefficientList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0302", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0302", "Delete");
                if (!ToolManager.CheckQueryString("ProductNumber"))
                {
                    Response.Write("<script>alert('请选择产品工序！');</script>");
                    Response.End();
                    return;
                }
                if (!ToolManager.CheckQueryString("Version"))
                {
                    Response.Write("<script>alert('请选择产品工序！');</script>");
                    Response.End();
                    return;
                }
                if (!ToolManager.CheckQueryString("WorkSnNumber"))
                {
                    Response.Write("<script>alert('请选择产品工序！');</script>");
                    Response.End();
                    return;
                }

                string error = string.Empty;
                string sql = " select * from ProductWorkSnProperty where ProductNumber='" + ToolManager.GetQueryString("ProductNumber") + "' and Version='" + ToolManager.GetQueryString("Version") + "' and WorkSnNumber='" + ToolManager.GetQueryString("WorkSnNumber") + "'";
                DataTable dt = DAL.SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count <= 0)
                {
                    Response.Write("已删除该产品工序！请重新选择产品工序！");
                    Response.End();
                    return;
                }

                //删除
                if (ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version") && ToolManager.CheckQueryString("WorkSnNumber") && ToolManager.CheckQueryString("ids"))
                {
                    string worksnnumber = ToolManager.GetQueryString("WorkSnNumber");
                    error = string.Empty;
                    List<string> sqls = new List<string>();
                    string Guid = string.Format(@" select guid from ProductWorkSnCoefficient where ProductNumber='{0}' and version='{1}' and WorkSnNumber='{2}' and RowNumber in ({3})",
                   ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"), ToolManager.GetQueryString("WorkSnNumber"), ToolManager.GetQueryString("ids"));
                    string sqlOne = string.Format(@" delete ProductWorkSnCoefficient where guid in ({0})", Guid);
                    sqls.Add(sqlOne);
                    bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除产品工序系数" + ToolManager.ReplaceSingleQuotesToBlank(worksnnumber), "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除产品工序系数" + ToolManager.ReplaceSingleQuotesToBlank(worksnnumber), "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("AddOrEditProductWorkSnCoefficient.aspx", "btnSearch", "420", "450");
                }
            }
        }
        private void GetPage(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0302", "Edit");
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
                        else if (dt.Columns[i].ColumnName.Equals("工序编码"))
                        {
                            tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                        }
                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                text += string.Format(@"<tr><td>
<input type='checkbox' name='subBox' value='{9}'/></td>{1}  
<td>

<a href='###'  value='{0}' onclick=""OpenDialog('{2}?ProductNumber={0}&Version={7}&WorkSnNumber={8}&RowNumber={9}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>
</span>
</td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, dr[2], Server.UrlEncode(dr[3].ToString()), dr["序号"]);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

    }
}
//<span style='display:{10};'>