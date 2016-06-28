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
    public partial class ProductPlanList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckParams("IsDelete"))
                {
                    List<string> sqls = new List<string>();
                    string planNumber = ToolManager.GetParamsString("PlanNumber");
                    string sql = string.Format(@"delete ProductPlanSubDetail where PlanNumber='{0}' ", planNumber);
                    sqls.Add(sql);
                    sql = string.Format(@" delete ProductPlanSub where PlanNumber='{0}' ", planNumber);
                    sqls.Add(sql);
                    sql = string.Format(@"  delete ProductPlanDetail where PlanNumber='{0}' ", planNumber);
                    sqls.Add(sql);
                    sql = string.Format(@" delete ProductPlan where PlanNumber='{0}' ", planNumber);
                    sqls.Add(sql);
                    string error = string.Empty; 
                    string result = SqlHelper.BatchExecuteSql(sqls, ref error) ? "ok" : error;
                    Tool.WriteLog(Tool.LogType.Operating, "删除开工单" + planNumber, result);
                    Response.Write(result);
                    Response.End();
                    return;

                }
                //确认领料
                if (ToolManager.CheckQueryString("IsConfirmCollar"))
                {
                    string type = ToolManager.GetQueryString("IsConfirmCollar");
                    type = type.Equals("sc") ? "生产出库" : "包装出库";
                    string error = string.Empty;
                    string result = string.Empty;
                    string userId = ToolCode.Tool.GetUser().UserNumber;
                    string[] array = ToolManager.GetQueryString("PlanNumbers").Split(',');

                    foreach (string str in array)
                    {
                        if (!WorkOrderManager.EnterKGD(str, userId, ref error, type))
                        {
                            result += error;
                        }
                    }
                    if (string.IsNullOrEmpty(result))
                    {
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Response.Write(result);
                        Response.End();
                        return;
                    }
                }
                //查询

                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageOperation("btnSearch", "EditProductPlan.aspx", "ProductPlanDetailList.aspx", "ProductPlanSubList.aspx");
                }
                //审核
                if (ToolManager.CheckQueryString("check"))
                {
                    string check = ToolManager.GetQueryString("check");
                    string temp = ProductPlan.check(ToolManager.GetQueryString("check"));
                    bool result = temp == "1" ? true : false;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "审核开工单信息" + ToolManager.ReplaceSingleQuotesToBlank(check), "审核成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "审核开工单信息" + ToolManager.ReplaceSingleQuotesToBlank(check), "审核失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
            }
        }


        public static void GetPageOperation(string btnId, string editUrl, string planpetailUrl, string plansubsetailLUrl)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0316", "Edit");

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
            string autorTime = string.Empty;
            string temp = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    if (i == 0 || dt.Columns[i].ColumnName.Equals("审核人") || dt.Columns[i].ColumnName.Equals("审核时间"))
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                autorTime = dr["审核时间"] == null ? "" : dr["审核时间"].ToString();
                if (string.IsNullOrEmpty(autorTime))
                {
                    temp = string.Format("<input type='checkbox' name='subBox' value='{0}'/>", dr["开工单号"]);
                }
                else
                {
                    temp = "";
                }
                text += string.Format(@"<tr>
<td>{7}</td>
{1}    
                        <td> 
<span style='display:{6};'>
<a  style='display:{8};' href='###'   onclick=""OpenDialogWithscroll('{2}?PlanNumber={0}&date={5}','btnSearch','320','600')"">  
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span> <label class='edit'>编辑</label>  <span >]</span>
</a></span>&nbsp;
<a href='{3}?PlanNumber={0}' class='edit'> <img src='../Img/037.gif' width='9' height='9' /><span > [</span> 总表详细 <span >]</span></a>

<a href='{4}?PlanNumber={0}' class='edit'> <img src='../Img/037.gif' width='9' height='9' /><span > [</span> 分表详细 <span >]</span></a>
 
<a  onclick=""DeletePlan('{0}')"" class='edit' style='display:{9};'> <img src='../Img/037.gif' width='9' height='9' /><span > [</span> 删除开工单  <span >]</span></a>

</span>
</td></tr>", dr[1], tdTextTemp, editUrl, planpetailUrl, plansubsetailLUrl, DateTime.Now, hasEdit, temp, dr["审核人"].ToString() != "" ? "none" : "inline"
           , ToolCode.Tool.GetUser().UserNumber.Equals("sysAdmin") ? "inline" : "none");
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

    }
}
