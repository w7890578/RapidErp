using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.PurchaseManager
{
    public partial class CertificateOrdersList : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string error = string.Empty;
                    List<string> sqls = new List<string>();
                    string deliveryNumbers = ToolManager.GetQueryString("ids");
                    string sql = string.Format(" delete CertificateOrdersDetail where OrdersNumber in ({0}) ", deliveryNumbers);
                    sqls.Add(sql);
                    sql = string.Format("delete CertificateOrders where OrdersNumber in ({0})", deliveryNumbers);
                    sqls.Add(sql);
                    if (SqlHelper.BatchExecuteSql(sqls, ref error))
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除采购单信息" + ToolManager.ReplaceSingleQuotesToBlank(deliveryNumbers), "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除采购单信息" + ToolManager.ReplaceSingleQuotesToBlank(deliveryNumbers), "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }

                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageOperation("btnSearch", "AddOrEditCertificateOrdersList.aspx", "CertificateOrdersListDetail.aspx");
                }

                //审核
                if (ToolManager.CheckQueryString("Check"))
                {
                    string numbers = ToolManager.GetQueryString("Check");
                    string temp = BLL.PurchaseManager.AuditorPurchase(numbers, ToolCode.Tool.GetUser().UserNumber);
                    bool result = temp == "1" ? true : false;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "审核采购单信息" + ToolManager.ReplaceSingleQuotesToBlank(numbers), "审核成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "审核采购单信息" + ToolManager.ReplaceSingleQuotesToBlank(numbers), "审核失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }

                }
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0203", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0203", "Delete");
                divAuditor.Visible = ToolCode.Tool.GetUserMenuFunc("L0203", "Audit");
                //Bind();
            }
        }

        //private void Bind()
        //{
        //    string sortname = ToolManager.CheckQueryString("sortname") ? ToolManager.GetQueryString("sortname") : "OrdersNumber";
        //    string sortdirection = ToolManager.CheckQueryString("sortdirection") ? ToolManager.GetQueryString("sortdirection") : "desc";
        //    string orderName = "order by " + sortname + " " + sortdirection;
        //    string sql = string.Format("select * from V_CertificateOrders");
        //    string pageIndex = ToolManager.GetQueryString("pageindex");
        //    if (string.IsNullOrEmpty(pageIndex))
        //    {
        //        pageIndex = "1";
        //    }
        //    int totalRecords = 0;
        //    if (ToolManager.CheckQueryString("pageSize"))
        //    {
        //        txtPageSize.Value = ToolManager.GetQueryString("pageSize");
        //    }
        //    string pageSize = txtPageSize.Value;
        //    DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, sql, orderName, ref totalRecords);
        //    this.rpList.DataSource = dt;
        //    this.rpList.DataBind();
        //    pageing.InnerHtml = ToolManager.PagerGet("CertificateOrdersList.aspx?pageSize=" + pageSize, totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize));
        //    hdSortName.Value = sortname;
        //    hdSortDirection.Value = sortdirection;


        //}

        private void GetPageOperation(string btnId, string editUrl, string detailedUrl)
        {
            string hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0203", "Edit");

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


            string conditionTwo = ToolManager.GetQueryString("conditionTwo");
            if (!conditionTwo.Equals("where 1=1"))
            {
                querySql += string.Format(@"and OrdersNumber in 
( select distinct OrdersNumber from CertificateOrdersDetail {0}) ", conditionTwo);
            }


            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);
            int columCount = dt.Columns.Count;
            string temp = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    if (i == 0)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else if (dt.Columns[i].ColumnName.Equals("OrdersNumber"))
                    {
                        tdTextTemp += string.Format(@" <td><a href='{1}?OrdersNumber={0}' title='点击进入详细' style='color:blue;text-decoration:none;' >{0}</a>  </td>", dr[1], detailedUrl);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                temp = dr["CheckTime"] == null ? "" : dr["CheckTime"].ToString();
                if (string.IsNullOrEmpty(temp))
                {
                    temp = string.Format(" <input type='checkbox' name='subBox' value='{0}'/>  ", dr["OrdersNumber"]);
                }
                else
                {
                    temp = "";
                }
                text += string.Format(@"<tr>
<td>{6}</td>
{1}  
                        <td>
<span style='display:{7};'>
<a style='display:{5};' href='###'   
onclick=""OpenDialogWithscroll('{2}?OrdersNumber={0}&date={4}','btnSearch','320','600')"">  
<img src='../Img/037.gif' width='9' height='9' />
<span> [</span> <label class='edit'>编辑</label>  <span >]</span>
</a>
</span>
&nbsp;
<a href='{3}?OrdersNumber={0}' class='edit'>
<img src='../Img/037.gif' width='9' height='9' /><span > [</span> 详细 <span >]</span>
</a></td></tr>",
dr[1], tdTextTemp, editUrl, detailedUrl, DateTime.Now, temp == "" ? "none" : "inline", temp, hasEdit);

            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
    }
}
