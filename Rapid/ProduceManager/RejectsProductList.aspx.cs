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
    public partial class RejectsProductList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0308","Add");
            divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0308", "Delete");
            //删除
            if (ToolManager.CheckQueryString("ids"))
            {

                string ids = ToolManager.GetQueryString("ids");
                string error = string.Empty;
                List<string> sqls = new List<string>();
                string sqlOne = string.Format(@" delete RejectsProduct where Guid in ({0})", ids);
                sqls.Add(sqlOne);
                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除不合格信息" , "删除成功");
                    Response.Write("1");
                    Response.End();
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除不合格信息", "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                }
            }
            //查询
            if (ToolManager.CheckQueryString("pageIndex"))
            {
                GetPage("AddOrEditRejectsProduct.aspx", "btnSearch", "410", "600");
                this.hdProductNumber.Value = ToolManager.GetQueryString("ProductNumber");
                this.hdVersion.Value = ToolManager.GetQueryString("Version");
            }
        }
        private void GetPage(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0308", "Edit"); 
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
                    else
                    {
                        //Guid列
                        if (dt.Columns[i].ColumnName.Equals("Guid"))
                        {
                            //tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                        }
                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                text += string.Format(@"<tr><td>
<input type='checkbox' name='subBox' value='{11}'/></td>{1}  
</tr>", dr[1], tdTextTemp, url, dr["上报时间"], dr["Guid"], dr["客户产成品编号"], dr["版本"], DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, dr["Guid"],hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
//            <span style='display:{12};'>
//<a href='###'  value='{0}' onclick=""OpenDialog('{2}?ReportTime={3}&Guid={4}&ProductNumber={5}&Version={6}&date={7}','{8}','{9}','{10}')""> <img src='../Img/037.gif' width='9' height='9' />
//<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span>
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string sql = saveInfo.Value;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "不合格品报表");
        }
    }
}
