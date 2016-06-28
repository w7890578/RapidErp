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
    public partial class BOMInfoList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0302", "Add");
                //divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0302", "Delete");
                if (!ToolManager.CheckQueryString("Id"))
                {
                    Response.Write("请选择产成品、版本！");
                    Response.End();
                    return;
                }
                if (!ToolManager.CheckQueryString("Version"))
                {
                    Response.Write("请选择产成品、版本！");
                    Response.End();
                    return;
                }
                string error = string.Empty;
                string sql = " select * from Product where ProductNumber='" + ToolManager.GetQueryString("Id") + "' and Version='" + ToolManager.GetQueryString("Version") + "'";
                DataTable dt = DAL.SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count <= 0)
                {
                    Response.Write("已删除该产成品编号、版本！请重新选择产成品、版本！");
                    Response.End();
                    return;
                }

                //删除
                if (ToolManager.CheckQueryString("id") && ToolManager.CheckQueryString("version") && ToolManager.CheckQueryString("ids"))
                {
                    List<string> sqls = new List<string>();
                    string productNumber = ToolManager.GetQueryString("id");
                    string version = ToolManager.GetQueryString("version");
                    string temp = ToolManager.GetQueryString("ids");
                    string tempMaterialNumber = string.Empty;
                    string tempSingleDose = string.Empty;
                    string[] tempSu = temp.Split(',');
                    foreach (string str in tempSu)
                    {
                        //                        string[] tempArray = str.Split('|');
                        //                        sql = string.Format(@" delete ProductCuttingLineInfo where ProductNumber ='{0}' 
                        //and Version ='{1}' and MaterialNumber='{2}' and Length ={3} ", productNumber, version, tempArray[0], tempArray[1]);
                        //                        sqls.Add(sql);
                        //                        sql = string.Format(@"delete BOMInfo where ProductNumber ='{0}' and Version ='{1}' 
                        //and MaterialNumber='{2}' and SingleDose ={3} ", productNumber, version, tempArray[0], tempArray[1]);
                        sql = string.Format(" delete  BOMInfo where Guid='{0}'", str);
                        sqls.Add(sql);

                    }

                    error = string.Empty;

                    //BOM信息表--裁线信息维护表
                    //                    string Guid = string.Format(@" select guid from BOMInfo where ProductNumber='{0}' 
                    //                    and Version='{1}' and MaterialNumber in ({2}) and SingleDose in ({3})", id, version, tempMaterialNumber ,tempSingleDose);
                    //                    sql = string.Format(@" delete BOMInfo where guid in ({0})", Guid);
                    //                    sqls.Add(sql);
                    //                    string ProductCuttingLineInfoGuid = " select pc.Guid as guid from dbo.BOMInfo bom inner join ProductCuttingLineInfo pc on bom.ProductNumber=pc.ProductNumber and bom.Version=pc.Version and bom.MaterialNumber=pc.MaterialNumber";
                    //                    sql = string.Format(@" delete ProductCuttingLineInfo where guid in ({0})", ProductCuttingLineInfoGuid);
                    //                    sqls.Add(sql);
                    //改变产品成本价
                    // sql = new BLL.ToolChangeProduct().changeProductCostPrice(productNumber, version);
                    //sqls.Add(sql);
                    bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                    if (result)
                    {
                        //更新成本价
                        SqlHelper.ExecuteSql(" exec P_UpdateProductCostPrice", ref error);
                        Tool.WriteLog(Tool.LogType.Operating, "删除BOM信息" + ToolManager.ReplaceSingleQuotesToBlank(productNumber), "删除成功");
                        Response.Write("1");
                        Response.End();
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除BOM信息" + ToolManager.ReplaceSingleQuotesToBlank(productNumber), "删除失败！原因：" + error);
                        Response.Write(error);
                        Response.End();
                    }
                    //new BLL.ToolChangeProduct().changeproduct(ToolManager.GetQueryString("id"), ToolManager.GetQueryString("version"));
                    return;

                }

                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPageHidden("AddOrEditBOMInfo.aspx", "btnSearch", "310", "700");
                }
            }
        }
        private void GetPageHidden(string url, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0302", "Edit");
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
                        if (dt.Columns[i].ColumnName.Equals("Guid"))
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
<input type='checkbox' name='subBox' value='{10}'/></td>{1}  
<td style='display:none;'>
<span>
<span style='display:{9};'>
<a href='###'  value='{0}' onclick=""OpenDialog('{2}?ProductNumber={0}&Version={7}&MaterialNumber={8}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>
</span></span>
</td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width, dr["版本"], dr["原材料编号"], hasEdit, dr["Guid"]);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

    }
}
