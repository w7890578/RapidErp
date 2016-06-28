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
    public partial class ProductCuttingLineInfoList : System.Web.UI.Page
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
                    Response.Write("<script>alert('请选择产成品、版本、原材料！');</script>");
                    Response.End();
                    return;
                }
                if (!ToolManager.CheckQueryString("Version"))
                {
                    Response.Write("<script>alert('请选择产成品、版本、原材料！');</script>");
                    Response.End();
                    return;
                }
                if (!ToolManager.CheckQueryString("MaterialNumber"))
                {
                    Response.Write("<script>alert('请选择产成品、版本、原材料！');</script>");
                    Response.End();
                    return;
                }

                string error = string.Empty;
                string sql = " select * from BOMInfo where ProductNumber='" + ToolManager.GetQueryString("ProductNumber") + "' and Version='" + ToolManager.GetQueryString("Version") + "' and MaterialNumber='" + ToolManager.GetQueryString("MaterialNumber") + "'";
                DataTable dt = DAL.SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count <= 0)
                {
                    Response.Write("已删除该产成品编号、版本、原材料！请重新选择产成品、版本、原材料！");
                    Response.End();
                    return;
                }

                //删除
                if (ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version") && ToolManager.CheckQueryString("MaterialNumber") && ToolManager.CheckQueryString("ids"))
                {
                    string productnumber = ToolManager.GetQueryString("ProductNumber");
                    string version = ToolManager.GetQueryString("Version");
                    string materialnumber = ToolManager.GetQueryString("MaterialNumber");
                    error = string.Empty;
                    List<string> sqls = new List<string>();
                    string Guid = string.Format(@" select guid from ProductCuttingLineInfo where ProductNumber='{0}' and version='{1}' and materialnumber='{2}' and length in ({3})",
                  productnumber, version, materialnumber, ToolManager.GetQueryString("ids"));
                    string sqlOne = string.Format(@" delete ProductCuttingLineInfo where guid in ({0})", Guid);
                    sqls.Add(sqlOne);
                    //改变bom单机用量
                    sql = new BLL.ToolChangeProduct().changeBomSingleDose(productnumber, version, materialnumber);
                    sqls.Add(sql);
                    //改变产品成本价
                    sql = new BLL.ToolChangeProduct().changeProductCostPrice(productnumber, version);
                    sqls.Add(sql);
                    bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                    if (result)
                    {
                        sql = string.Format(@" update BOMInfo set SingleDose=(select sum(p.Length*p.Qty) from ProductCuttingLineInfo p) where  ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}'",
                 ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"), ToolManager.GetQueryString("MaterialNumber"));
                        SqlHelper.ExecuteSql(sql, ref error);
                        Tool.WriteLog(Tool.LogType.Operating, "删除裁线信息" + ToolManager.ReplaceSingleQuotesToBlank(productnumber), "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除裁线信息" + ToolManager.ReplaceSingleQuotesToBlank(productnumber), "删除失败！原因：" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageHidden("AddOrEditProductCuttingLineInfo.aspx", "btnSearch", "320", "600");
                }
            }
        }

        private void GetPageHidden(string url, string btnId, string height, string width)
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
                        if (dt.Columns[i].ColumnName.Equals("Guid") || dt.Columns[i].ColumnName.Equals("数量"))
                        {
                            tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                        }
                        else if (dt.Columns[i].ColumnName.Equals("文件名称"))
                        {
                            tdTextTemp += string.Format("<td><a  target ='_blank' style='color:blue;' href='{1}'>{0}</a></td>", dr[i], dr["上传路径"]);
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
<span style='display:{10};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?ProductNumber={0}&Version={7}&MaterialNumber={8}&Length={9}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span>
</td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, dr[2], Server.UrlEncode(dr[3].ToString()), dr[4], hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
