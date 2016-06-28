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
    public partial class ProductBlueprintPropertyList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0113", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0113", "Delete");
                if (!ToolManager.CheckQueryString("Id"))
                {
                    Response.Write("请选择产成品、版本！");
                    Response.End();
                    return;
                }
                if (!ToolManager.CheckQueryString("Version"))
                {
                    Response.Write("请选择产成品、版本！");
                    Response.End();
                    return;
                }
                string error = string.Empty;
                string sql = " select * from Product where ProductNumber='" + ToolManager.GetQueryString("Id") + "' and Version='" + ToolManager.GetQueryString("Version") + "'";
                DataTable dt = DAL.SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count <= 0)
                {
                    Response.Write("已删除该产成品编号、版本！请重新选择产成品、版本！");
                    Response.End();
                    return;
                }

                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string temp = ProductBlueprintPropertyManager.DeleteData(ToolManager.GetQueryString("ids"));
                    string id = ToolManager.GetQueryString("Id");
                    bool restult = temp == "1" ? true : false;
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除产成品图纸信息" + ToolManager.ReplaceSingleQuotesToBlank(id), "删除成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除产成品图纸信息" + ToolManager.ReplaceSingleQuotesToBlank(id), "删除失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageHidden("AddOrEditProductBlueprintProperty.aspx", "btnSearch", "380", "600");
                }
            }
        }
        // 重写GetPage(隐藏一列)
        private void GetPageHidden(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0113", "Edit");
            string sql = string.Empty;
            int pageCount = 0;//总页数 
            int totalRecords = 0;//总行数
            string error = string.Empty;
            string text = string.Empty;
            string editTemp = string.Empty;
            string tdTextTemp = string.Empty;
            string pageIndex = ToolManager.GetQueryString("pageIndex");
            string pageSize = ToolManager.GetQueryString("pageSize");
            string sortName = ToolManager.GetQueryString("sortName");
            string sortDirection = ToolManager.GetQueryString("sortDirection");
            string querySql = ToolManager.GetQueryString("querySql") + " and ISNULL (要求修改人,'')=''";
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);
            int columCount = dt.Columns.Count;
            foreach (DataRow dr in dt.Rows)
            {
                editTemp = "";
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    //第一列为序号
                    if (i == 0 )
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        //Guid列
                        if (dt.Columns[i].ColumnName.Equals("Guid") || dt.Columns[i].ColumnName.Equals("上传路径") || dt.Columns[i].ColumnName.Equals("操作指导书上传路径"))
                        {
                            tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                        }
                        else if (dt.Columns[i].ColumnName.Equals("文件名称"))
                        {
                            tdTextTemp += string.Format("<td><a  target ='_blank' style='color:blue;' href='{1}'>{0}</a></td>", dr[i], dr["上传路径"]);
                        }
                        else if (dt.Columns[i].ColumnName.Equals("操作指导书"))
                        {
                            tdTextTemp += string.Format("<td><a  target ='_blank' style='color:blue;' href='{1}'>{0}</a></td>", dr[i], dr["操作指导书上传路径"]);
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
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?ProductNumber={0}&Version={7}&FileName={8}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span>
</td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, dr[2], Server.UrlEncode(dr[3].ToString()), dr["guid"], hasEdit);

                sql = ToolManager.GetQueryString("querySql") + " and ISNULL (要求修改人,'')!='' " + string.Format("and  文件名称='{0}'", dr["文件名称"]);
                DataTable dtEdit = SqlHelper.GetTable(sql);
                foreach (DataRow drEdit in dtEdit.Rows)
                {
                    editTemp += string.Format(@" <tr><td></td> 
<td>{0}</td>
<td>{1}</td>
<td>{2}</td>
<td>{5}</td>
<td></td>
<td>{3}</td><td>{4}</td><td></td><td></td><td></td><td></td><td></td><td></td><td></td>", dr["产成品编号"], dr["版本"], dr["文件名称"], drEdit["修改时间"], drEdit["要求修改人"],drEdit ["操作指导书"]);
                }
                text += editTemp;
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

    }
}
