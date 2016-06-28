using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class ProductWarehouseLogList1 : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0406", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0406", "Delete");
                //divCheck.Visible = ToolCode.Tool.GetUserMenuFunc("L0406", "Audit");
                string error = string.Empty;
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string ids = ToolManager.GetQueryString("ids");
                    error = string.Empty;
                    List<string> sqls = new List<string>();

                    string sqlOne = string.Format(@" delete ProductWarehouseLogDetail where WarehouseNumber in ({0})", ids);
                    sqls.Add(sqlOne);
                    string sqlTwo = string.Format(@" delete ProductWarehouseLog where WarehouseNumber in ({0})", ids);
                    sqls.Add(sqlTwo);
                    bool restult = SqlHelper.BatchExecuteSql(sqls, ref error);
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除产成品出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除产成品出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("AddOrEditProductWarehouseLog.aspx", "btnSearch", "230", "600");
                }
                //审核
                if (ToolManager.CheckQueryString("check"))
                {
                    string check = ToolManager.GetQueryString("check");
                    string temp = checkProductHouse(check);
                    bool result = temp == "1" ? true : false;
                    if (result)
                    {

                        Tool.WriteLog(Tool.LogType.Operating, "审核产成品出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(check), "审核成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "审核产成品出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(check), "审核失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
            }
        }

        private string checkProductHouse(string check)
        {

            string auditor = ToolCode.Tool.GetUser().UserNumber;
            //string CheckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string error = string.Empty;
            //string sql = string.Format(@" update ProductWarehouseLog set Auditor='{0}' , CheckTime='{1}' where WarehouseNumber in ({2})",
            //    Auditor, CheckTime, check);
            //return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
            return StoreroomToolManager.AutiorProductWarehouseLog(check, auditor);
        }

        private void GetPage(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0406", "Edit");
            int pageCount = 0;//总页数 
            int totalRecords = 0;//总行数
            string error = string.Empty;
            string text = string.Empty;
            string tdTextTemp = string.Empty;
            string pageIndex = ToolManager.GetQueryString("pageIndex");
            string pageSize = ToolManager.GetQueryString("pageSize");
            string sortName = ToolManager.GetQueryString("sortName");
            string sortDirection = ToolManager.GetQueryString("sortDirection");
            string condition = ToolManager.GetQueryString("condition");
            string conditionTwo = ToolManager.GetQueryString("conditionTwo");
            string sql = "";
            if (!conditionTwo.Equals("where vpll.是否确认='是'"))
            {
                sql = string.Format(@" select distinct vpll.出入库编号 from  V_ProductWarehouseLogList  vpll
 inner join V_Tool_ProductWarehouseLogDetail   pwld  on vpll.出入库编号= pwld.出入库编号   {0}", conditionTwo);
                sql = string.Format("  select * from V_ProductWarehouseLogList where 出入库编号 in ({0}) ", sql);
            }
            else
            {
                sql = string.Format("  select * from V_ProductWarehouseLogList {0} ", condition);
            }


            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, sql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);
            int columCount = dt.Columns.Count;
            string temp = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    if (i == 0 || dt.Columns[i].ColumnName.Equals("是否确认"))
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {

                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                temp = dr["审核时间"] == null ? "" : dr["审核时间"].ToString();
                if (string.IsNullOrEmpty(temp))
                {
                    temp = string.Format("<input type='checkbox' name='subBox' value='{0}'/>", dr[1]);
                }
                else
                {
                    temp = "";
                }
                text += string.Format(@"<tr><td>{11}
</td>{1}  
<td>
<span style='display:{12};'>
<a style='display:{7};' href='###'   onclick=""OpenDialog('{2}?WarehouseNumber={0}&date={3}','{4}','{5}','{6}')""> 
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span>

<a  onclick=""Transfer('{8}','{9}','{10}')"" > 
<img src='../Img/detail.png' width='9' height='9' />
 [ <span class='edit'>详细</span> ]</a>
</td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width,
  dr["审核人"].ToString() != "" ? "none" : "inline",
 dr["变动方向"].ToString() == "入库" ? "InLibrary" : "OutLibrary", dr["出入库类型"], dr["出入库编号"], temp, hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

    }
}
