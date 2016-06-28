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
    public partial class StockInventoryLogList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0405", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0405", "Delete");
                //divExp.Visible = ToolCode.Tool.GetUserMenuFunc("L0405", "Exp");
//                //删除
//                if (ToolManager.CheckQueryString("ids"))
//                {
//                    List<string> sqls = new List<string>();
//                    string ids = ToolManager.GetQueryString("ids");
//                    string error = string.Empty;
//                    string sql = string.Format(@" 
// delete StockInventoryLogDetail where InventoryNumber in ({0})", ids);
//                    sqls.Add(sql);
//                    sql = string.Format(" delete StockInventoryLog where InventoryNumber in ({0})", ids);
//                    sqls.Add(sql);
//                    bool restult = SqlHelper.BatchExecuteSql(sqls, ref error);
//                    if (restult)
//                    {
//                        Tool.WriteLog(Tool.LogType.Operating, "删除库存盘点信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
//                        Response.Write("1");
//                        Response.End();
//                        return;
//                    }
//                    else
//                    {
//                        Tool.WriteLog(Tool.LogType.Operating, "删除库存盘点信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + error);
//                        Response.Write(error);
//                        Response.End();
//                        return;
//                    }
//                }
//                //查询
//                if (ToolManager.CheckQueryString("pageIndex"))
//                { 
//                    GetPage("AddOrEditStockInventoryLog.aspx", "TransferForQuoteInfo", "btnSearch", "180", "600");
//                }
            }
        }


//        //带有详细
//        private void GetPage(string editUrl, string detailFunctionForJS, string btnId, string height, string width)
//        {
//            int pageCount = 0;//总页数 
//            int totalRecords = 0;//总行数
//            string error = string.Empty;
//            string text = string.Empty;
//            string tdTextTemp = string.Empty;
//            string pageIndex = ToolManager.GetQueryString("pageIndex");
//            string pageSize = ToolManager.GetQueryString("pageSize");
//            string sortName = ToolManager.GetQueryString("sortName");
//            string sortDirection = ToolManager.GetQueryString("sortDirection");
//            string querySql = ToolManager.GetQueryString("querySql");
//            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);
//            int columCount = dt.Columns.Count;
//            foreach (DataRow dr in dt.Rows)
//            {
//                tdTextTemp = "";
//                for (int i = 0; i < columCount; i++)
//                {
//                    //第一列为序号
//                    if (i == 0)
//                    {
//                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
//                    }
//                    else
//                    {
//                        //Guid列
//                        if (dt.Columns[i].ColumnName.Equals("Guid"))
//                        {
//                            tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
//                        }
//                        else
//                        {
//                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
//                        }
//                    }
//                }
//                text += string.Format(@"<tr><td>
//<input type='checkbox' name='subBox' value='{0}'/></td>{1}  
//<td><a href='###'  value='{0}' onclick=""OpenDialog('{2}?InventoryNumber={0}&date={3}','{4}','{5}','{6}')""> 
//<img src='../Img/037.gif' width='9' height='9' />
//<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>
//
//<a href='###'  onclick=""{7}('{0}')""> 
//<img src='../Img/detail.png' width='9' height='9' />
//<span > [</span>   <label class='edit'>详细</label> <span >]</span></a>
//</td></tr>", dr["盘点编号"], tdTextTemp, editUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), btnId, height, width, detailFunctionForJS);
//            }
//            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
//            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
//            Response.Write(responseValue);
//            Response.End();
//            return;
//        }




        protected void Button1_Click1(object sender, EventArgs e)
        {
            string sql = saveInfo.Value;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "库存盘点列表");
        }
    }
}
