using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0302", "Add");
                //divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0302", "Delete");
                //divPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0302", "Print");
                //divImp.Visible = ToolCode.Tool.GetUserMenuFunc("L0302", "Imp");
                //divExp.Visible = ToolCode.Tool.GetUserMenuFunc("L0302", "Exp");
                //if (ToolManager.CheckQueryString("ChoosedVlaue"))
                //{
                //    string selectedValuePid = ToolManager.GetQueryString("ChoosedVlaue");
                //    string sql = string.Format(@" select Type from MareriaType where Pid='{0}'", selectedValuePid);
                //    Response.Write(ControlBindManager.GetOption(sql, "Type", "Type"));
                //    Response.End();
                //    return;
                //}


            }


            //删除
            if (ToolManager.CheckQueryString("id") && ToolManager.CheckQueryString("ids"))
            {
                string id = ToolManager.GetQueryString("id");
                string temp = ProductManager.DeleteData(ToolManager.GetQueryString("id"), ToolManager.GetQueryString("ids"));
                bool restult = temp == "1" ? true : false;
                if (restult)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除产成品信息" + ToolManager.ReplaceSingleQuotesToBlank(id), "删除成功");
                    Response.Write(temp);
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除产成品信息" + ToolManager.ReplaceSingleQuotesToBlank(id), "删除失败！原因" + temp);
                    Response.Write(temp);
                    Response.End();
                    return;
                }
            }
            //查询
            if (ToolManager.CheckQueryString("pageIndex"))
            {
                GetPage("AddOrEditProduct.aspx", "ProductDetailedList.aspx", "btnSearch", "460", "600");
            }
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
            ToolCode.Tool.ExpExcel(sql, "产成品基本信息列表");

        }

        private void GetPage(string editUrl, string detailFunctionForJS, string btnId, string height, string width)
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
                    //第一列为序号
                    if (i == 0)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        //Guid列
                        if (dt.Columns[i].ColumnName.Equals("Guid") || dt.Columns[i].ColumnName.Equals("是否为旧版本"))
                        {
                            tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                        }
                        else
                        {
                            if (dr["是否为旧版本"].ToString().Equals("是"))
                            {
                                tdTextTemp += string.Format("<td style='background-color:#FF8B2F;'>{0}</td>", dr[i]);
                            }
                            else
                            {
                                tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                            }

                        }
                    }
                }
                text += string.Format(@"<tr><td>
<input type='checkbox' name='subBoxProductNumber' value='{0}'/></td><input type='checkbox' name='subBoxVersion' value='{8}' style='display:none;'/>{1}  
<td>


<a href='###' style='display:{10};' value='{0}' onclick=""OpenDialog('{2}?Id={0}&version={9}&date={3}','{4}','{5}','{6}')""> 
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>


</td></tr>", dr[1], tdTextTemp, editUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), btnId, height, width, detailFunctionForJS, dr[2], dr["版本"]
           , (Session["User_Func"] as System.Collections.Generic.List<string>).Contains("L0113|Edit") ? "inline" : "none");
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        //        protected void btnExpWrokSn_Click(object sender, EventArgs e)
        //        {
        //            string sql = string.Format(@"
        //select 
        //pwp.ProductNumber as 产成品编号,
        //t.CustomerProductNumber as 客户产成品编号,
        //pwp.Version as 版本, 
        //ws.WorkSnName as 工序名称,
        //pwp.WorkSnNumber as 工序编号,
        //ws.Sn as 工序序号,
        //pwp.RatedManhour as 工序工时
        //from ProductWorkSnProperty pwp
        //inner join WorkSn ws on pwp.WorkSnNumber=ws.WorkSnNumber
        //inner join 
        //(
        //select 
        //ProductNumber,
        //Version,
        //MAX(CustomerProductNumber) 
        //CustomerProductNumber 
        //from ProductCustomerProperty
        //group by ProductNumber,Version
        //) t 
        //on t.ProductNumber =pwp.ProductNumber and t.Version =pwp.Version 
        //order by pwp.ProductNumber,pwp.version,RowNumber asc
        //");
        //            ToolCode.Tool.ExpExcel(sql, "产成品工序工时表");
        //        }

    }
}
