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
    public partial class MarerialWarehouseLogList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                string error = string.Empty;
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string ids = ToolManager.GetQueryString("ids");
                    string temp = StoreroomToolManager.MarerialWarehouseLogListDelete(ids);
                    bool restult = temp == "1" ? true : false;
                    if (restult)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除原材料出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除原材料出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("AddOrEditMarerialWarehouseLog.aspx", "btnSearch", "Transfer", "260", "600");
                }
                //审核
                if (ToolManager.CheckQueryString("check"))
                {
                    string check = ToolManager.GetQueryString("check");
                    string temp = MarerialWarehouseLogListManager.AuditorMarerialWarehouseLog(ToolCode.Tool.GetUser().UserNumber, check);
                    bool result = temp == "1" ? true : false;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "审核原材料出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(check), "审核成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "审核原材料出入库信息" + ToolManager.ReplaceSingleQuotesToBlank(check), "审核失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0404", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0404", "Delete");
                //divCheck.Visible = ToolCode.Tool.GetUserMenuFunc("L0404", "Audit");
            }
        }

        //private string checkMaterialHouse(string check)
        //{
        //    string Auditor = ToolCode.Tool.GetUser().UserNumber;
        //    string CheckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    string error = string.Empty;
        //    string sql = string.Empty;

        //    sql = string.Format(@" update MarerialWarehouseLog set Auditor='{0}' , CheckTime='{1}' where WarehouseNumber in ({2}) and (CheckTime ='' or CheckTime = null)",
        //      Auditor, CheckTime, check);


        //    return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        //}

        private void GetPage(string url, string btnId, string detailFunctionForJS, string height, string width)
        {
            hasEdit = "inline";
            //hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0404", "Edit");
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
            string tempCondtion = ToolManager.GetQueryString("tempCondtion");
            string hgnumber = ToolManager.GetQueryString("KGNumber");
            string sql = string.Empty;

            if (tempCondtion.Equals("where 1=1"))
            {
                sql = string.Format(" select * from V_MarerialWarehouseLogList {0} ", querySql);
            }

            else
            {
                sql = string.Format(@"
select distinct WarehouseNumber from MaterialWarehouseLogDetail mwld  inner join MarerialInfoTable
 mit on mwld.MaterialNumber =mit.MaterialNumber {0}", tempCondtion);
                sql = string.Format(@"
select * from (
select * from V_MarerialWarehouseLogList where 出入库编号 in ({0}))t {1}
", sql, querySql);
            }


            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, sql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);
            int columCount = dt.Columns.Count;
            string autorTime = string.Empty;
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

                    else if (dt.Columns[i].ColumnName.Equals("出入库编号"))
                    {
                        tdTextTemp += string.Format(@" <td><a href='###' title='点击进入详细' style='color:blue;' 
onclick=""{1}('{0}','{2}','{3}')"">{0}</a>  </td>", dr["出入库编号"], detailFunctionForJS, dr["变动方向"], dr["出入库类型"]);

                    }
                    else
                    {
                        tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                    }
                }
                autorTime = dr["审核时间"] == null ? "" : dr["审核时间"].ToString();
                if (string.IsNullOrEmpty(autorTime))
                {
                    temp = string.Format("<input type='checkbox' name='subBox' value='{0}'/>", dr["出入库编号"]);
                }
                else
                {
                    temp = "";
                }
                //if (dr["出入库类型"].ToString().Equals("维修出库"))
                //{
                //    hasEdit = "none";
                //}
                //else
                //{
                //    hasEdit = "inline";
                //}


                text += string.Format(@"<tr><td>{11}
</td> {1}  
<td>
<span style='display:{12};'>
<a style='display:{7};' href='###'  value='{0}' onclick=""OpenDialog('{2}?WarehouseNumber={0}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></span>


<a href='###'  onclick=""{8}('{0}','{9}','{10}')""> 
<img src='../Img/detail.png' width='9' height='9' />
<span > [</span>   <label class='edit'>详细</label> <span >]</span></a>
</td></tr>",

           dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, dr["审核人"].ToString() != "" ? "none" : "inline", detailFunctionForJS, dr[3], dr[4], temp, hasEdit);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string sql = txtSql.Text;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            DataTable dt = SqlHelper.GetTable(sql);
            ExcelHelper.Instance.ExpExcel(dt, "统计结果");
            //ToolCode.Tool.ExpExcel(sql, "原材料库房流水帐");
        }
    }
}
