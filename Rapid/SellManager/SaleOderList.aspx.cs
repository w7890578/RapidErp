using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class SaleOderList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                 

                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string error = string.Empty;
                    List<string> sqls = new List<string>();
                    string orderNumbers = ToolManager.GetQueryString("ids");
                    string sql = string.Format(" delete TradingOrderDetail where OdersNumber in ({0}) ", orderNumbers);
                    sqls.Add(sql);
                    sql = string.Format("delete MachineOderDetail where OdersNumber in ({0})", orderNumbers);
                    sqls.Add(sql);
                    sql = string.Format(" delete SaleOder where OdersNumber in ({0})", orderNumbers);
                    sqls.Add(sql);
                    bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除销售订单" + ToolManager.ReplaceSingleQuotesToBlank(orderNumbers), "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除销售订单" + ToolManager.ReplaceSingleQuotesToBlank(orderNumbers), "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }

                //转成正式订单
                if (ToolManager.CheckQueryString("IsChangeType"))
                {
                    string orderNumbers = ToolManager.GetQueryString("id");

                    string sql = string.Format(@"update SaleOder set OdersType='正常订单' ,CustomerOrderNumber='{1}'  
where OdersNumber ='{0}' ", orderNumbers, ToolManager.GetQueryString("CustomerOrderNumber"));
                    string error = string.Empty;
                    if (SqlHelper.ExecuteSql(sql, ref error))
                    {
                        Response.Write("ok");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Response.Write(error);
                        Response.End();
                        return;
                    }

                }
                //审核
                if (ToolManager.CheckQueryString("check"))
                {
                    string orderNumbers = ToolManager.GetQueryString("check");
                    Response.Write(SaleOderManager.CheckSaleOrder(ToolCode.Tool.GetUser().UserNumber, orderNumbers));
                    Response.End();
                    return;
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("AddOrEditSaleOder2.aspx", "Transfer", "btnSearch", "350", "900");
                }

                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0103", "Add");
                divCheck.Visible = ToolCode.Tool.GetUserMenuFunc("L0103", "Audit");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0103", "Delete");
                divExp.Visible = ToolCode.Tool.GetUserMenuFunc("L0103", "Exp");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0103", "Edit");
            }
        }


        private static void GetPage(string editUrl, string detailFunctionForJS, string btnId, string height, string width)
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
            string customerProductNumber = ToolManager.GetQueryString("customerProductNumber");
            string customerMateriNumber = ToolManager.GetQueryString("customerMateriNumber");
            if (!string.IsNullOrEmpty(customerProductNumber))
            {
                querySql += string.Format(@" and 销售订单号 in (
select distinct OdersNumber  from MachineOderDetail where CustomerProductNumber ='{0}'
)", customerProductNumber);

            }
            if (!string.IsNullOrEmpty(customerMateriNumber))
            {
                querySql += string.Format(@" and 销售订单号 in (
select distinct OdersNumber  from TradingOrderDetail where CustomerMaterialNumber ='{0}'
)", customerMateriNumber);
            }
            string loginUser = ToolCode.Tool.GetUser().UserNumber;

             
            string changeStr = "";
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);

            int columCount = dt.Columns.Count;
            //dt.Columns.Remove(dt.Rows[0]["交期状态"].ToString());
            string temp = string.Empty;
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
                        if (dt.Columns[i].ColumnName.Equals("Guid") || dt.Columns[i].ColumnName.Equals("交期状态"))
                        {
                            tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                        }
                        else if (dt.Columns[i].ColumnName.Equals("销售订单号"))
                        {
                            tdTextTemp += string.Format(@" <td><a href='###' title='点击进入详细' style='color:blue;' 
onclick=""{1}('{0}','{2}')"">{0}</a>  </td>", dr["销售订单号"], detailFunctionForJS, dr["生产类型"]);

                        }
                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                temp = dr["审核时间"] == null ? "" : dr["审核时间"].ToString();
                if (string.IsNullOrEmpty(temp))
                {
                    temp = string.Format(" <input type='checkbox' name='subBox' value='{0}'/>  ", dr["销售订单号"]);
                }
                else
                {
                    temp = "";
                }
                if (dr["订单类型"].ToString().Equals("临时订单") && loginUser.Equals("sysAdmin"))
                {
                    changeStr = "转成正式订单";
                }
                else
                {
                    changeStr = "";
                }
                text += string.Format(@"<tr><td>{10}
 </td>
{1} 
<td>
<span style='display:{11};'>
<a style='display:{9};' href='###'  value='{0}' onclick=""OpenDialog('{2}?Id={0}&date={3}','{4}','{5}','{6}')""> 
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'  >编辑</label> <span >]</span></a>
</span>
<a href='###'  onclick=""{8}('{0}','{7}')"">
<img src='../Img/detail.png' width='9' height='9' />
<span > [</span>   <label class='edit'>详细</label> <span >]</span> <a  href='###' onclick=""Change('{0}')"">{12}</a></a>
</td></tr>", dr[1], tdTextTemp, editUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), btnId, height, width, dr["生产类型"],
detailFunctionForJS, temp == "" ? "none" : "inline", temp, hasEdit, changeStr);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
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
            ToolCode.Tool.ExpExcel(sql, "销售订单列表");

        }

    }
}
