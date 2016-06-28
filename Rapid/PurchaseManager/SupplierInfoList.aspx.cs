using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;


namespace Rapid.PurchaseManager
{
    public partial class SupplierInfoList : System.Web.UI.Page
    {
        public static string hasEdit = "none";
        protected void Page_Load(object sender, EventArgs e)
        {
          
            //删除
            if (ToolManager.CheckQueryString("ids"))
            {
                string temp = SupplierInfoManager.DeleteSupplierInfo(ToolManager.GetQueryString("ids"));
                string ids = ToolManager.GetQueryString("ids");
                bool result = temp == "1" ? true : false;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除供应商信息"+ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                    Response.Write(temp);
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除供应商信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + temp);
                    Response.Write(temp);
                    Response.End();
                    return;
                }
            }
            //查询
            if (ToolManager.CheckQueryString("pageIndex"))
            {
                GetPageOperation("btnSearch", "AddOrEditSupplierInfo.aspx", "SupplierDetailedInfoList.aspx");
            }
            divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0201", "Add");
            divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0201", "Delete");
            divImp.Visible = ToolCode.Tool.GetUserMenuFunc("L0201", "Imp");
            divExp.Visible = ToolCode.Tool.GetUserMenuFunc("L0201", "Exp");
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
            ToolCode.Tool.ExpExcel(sql, "供应商列表");

        }
        private void GetPageOperation(string btnId, string editUrl, string detailedUrl)
        {
            hasEdit=ToolCode.Tool.GetUserMenuFuncStr("L0201", "Edit");
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
                    if (i == 0)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else if (dt.Columns[i].ColumnName.Equals("供应商编号"))
                    {
                        tdTextTemp += string.Format(@" <td><a href='{1}?Id={0}' title='点击进入详细' style='color:blue;' >{0}</a>  </td>", dr[1], detailedUrl);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
//                 text += string.Format(@"<tr>
//<td><input type='checkbox' name='subBox' value='{0}'/></td>
//{1}    
//                        <td> 
//<a href='###'   onclick=""OpenDialogWithscroll('{2}?Id={0}&date={4}')"">  
//<img src='../Img/037.gif' width='9' height='9' />
//<span > [</span> <label class='edit'>编辑</label>  <span >]</span>
//</a>&nbsp;
//<a href='{3}?Id={0}' class='edit'> <img src='../Img/037.gif' width='9' height='9' /><span > [</span> 详细 <span >]</span></a></td></tr>", dr[1], tdTextTemp, editUrl, detailedUrl,DateTime .Now );
//            }
                text += string.Format(@"<tr>
<td><input type='checkbox' name='subBox' value='{0}'/>{1}</td>

                        <td> <a href='###'   onclick=""OpenDialogWithscroll('{2}?Id={0}&date={4}','btnSearch','650','600')"">  
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span> <label class='edit'>编辑</label>  <span >]</span>
</a>&nbsp;
<a href='{3}?Id={0}' class='edit'> <img src='../Img/037.gif' width='9' height='9' /><span > [</span> 详细 <span >]</span></a></td></tr>", dr[1], tdTextTemp, editUrl, detailedUrl,DateTime .Now.ToString() );

            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
