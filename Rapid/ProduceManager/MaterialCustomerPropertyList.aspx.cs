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
    public partial class MaterialCustomerPropertyList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0301", "Add");
                //divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0301", "Delete");
                if (!ToolManager.CheckQueryString("Id"))
                {
                    Response.Write("请选择原材料！");
                    Response.End();
                    return;
                }
                string error = string.Empty;
                string sql = " select * from MarerialInfoTable where materialnumber='" + ToolManager.GetQueryString("Id") + "'";
                DataTable dt = DAL.SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count <= 0)
                {
                    Response.Write("已删除该原材料编号！请刷新后重新选择原材料！");
                    Response.End();
                    return;
                }
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string temp = MaterialCustomerPropertyManager.DeleteData(ToolManager.GetQueryString("ids"));
                    string id = ToolManager.GetQueryString("Id");
                    bool restult = temp == "1" ? true : false;
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除原材料客户信息" + ToolManager.ReplaceSingleQuotesToBlank(id), "删除成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除原材料客户信息" + ToolManager.ReplaceSingleQuotesToBlank(id), "删除失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageMaterialCustomer("AddOrEditMaterialCustomerProperty.aspx", "btnSearch", "260", "600");
                }
            }
        }
        
        public  void GetPageMaterialCustomer(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0301", "Edit");
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
                        if (dt.Columns[i].ColumnName.Equals("Guid") || dt.Columns[i].ColumnName.Equals("客户编号"))
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
<input type='checkbox' name='subBox' value='{8}'/></td>{1}  
<td style='display:none;'>
<span style='display:{9};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?MaterialNumber={0}&CustomerId={7}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>
</span>
</td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, Server.UrlEncode(dr["客户编号"].ToString()), dr["Guid"],hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

    }
}