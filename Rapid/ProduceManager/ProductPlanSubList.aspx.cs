using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class ProductPlanSubList : System.Web.UI.Page
    {
        public static string type = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string plannumber = ToolManager.GetQueryString("PlanNumber");
                string sql = string.Format(@"select * from V_ProductPlan where 开工单号='{0}'", plannumber);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    type = dr["开工单类型"] == null ? "" : dr["开工单类型"].ToString();

                }
                //查询

                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageOperation("btnSearch", "EditProductPlanSub.aspx", "ProductPlanSubDetailList.aspx");
                }
            }
        }


        private void GetPageOperation(string btnId, string editUrl, string detailedUrl)
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
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }

                text += string.Format(@"<tr>
<td><input type='checkbox' name='subBox' value='{0}'/></td>
{1}    
                        <td> 
<a href='###'   onclick=""OpenDialogWithscroll('{2}?PlanNumber={0}&date={4}&Team={5}','btnSearch','320','600')"">  
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span> <label class='edit'>编辑</label>  <span >]</span>
</a>&nbsp;
<a href='{3}?PlanNumber={0}&Team={5}' class='edit'> <img src='../Img/037.gif' width='9' height='9' /><span > [</span> 详细 <span >]</span></a></td></tr>",
                                       dr["开工单号"], tdTextTemp, editUrl, detailedUrl, DateTime.Now, Server.UrlEncode(dr["班组"].ToString()));
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
