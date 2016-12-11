using BLL;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Rapid.ToolCode
{
    /// <summary>
    /// 通用代码
    /// </summary>
    public class Tool
    {
        /// <summary>
        /// 检测当前登录用户的页面功能权限
        /// </summary>
        /// <param name="menuId">页面Id</param>
        /// <param name="funId">功能Id</param>
        /// <returns>返回检测结果ture有权限false没有权限</returns>
        public static bool GetUserMenuFunc(string menuId, string funId)
        {
            if (string.Compare("sysAdmin", GetUser().UserNumber, true) == 0)
            {
                return true;
            }
            else
            {
                List<string> userMenuFuncs = HttpContext.Current.Session["User_Func"] as List<string>;
                string temp = userMenuFuncs.Find(x => string.Compare(x.ToString(), menuId + "|" + funId, true) == 0);

                return temp != null;
            }
            //            string sql = string.Format(@"select * from (
            //select pu.USER_ID ,pufr.MENU_ID ,pufr.FUNCTION_ID from PM_USER pu
            //inner join PM_USER_FUNC_PERMISSION pufr on pu.USER_ID =pufr.USER_ID
            //where pufr .APP_ID ='Rapid-Erp'
            //union
            //select pu.USER_ID ,prfp .MENU_ID,prfp.FUNCTION_ID from PM_USER pu
            //inner join PM_ROLE_FUNC_PERMISSION prfp on pu.RoleId =prfp.ROLE_ID
            // where   prfp.APP_ID='Rapid-Erp') t where t.USER_ID ='{0}' and MENU_ID ='{1}' and FUNCTION_ID ='{2}'", GetUser() == null ? "" : GetUser().UserNumber, menuId, funId);
            //            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }

        /// <summary>
        /// 检测当前登录用户的页面功能权限
        /// </summary>
        /// <param name="menuId">页面Id</param>
        /// <param name="funId">功能Id</param>
        /// <returns>返回检测结果inline有权限none没有权限</returns>
        public static string GetUserMenuFuncStr(string menuId, string funId)
        {
            if (string.Compare("sysAdmin", GetUser().UserNumber, true) == 0)
            {
                return "";
            }
            else
            {
                List<string> userMenuFuncs = HttpContext.Current.Session["User_Func"] as List<string>;
                string temp = userMenuFuncs.Find(x => string.Compare(x.ToString(), menuId + "|" + funId, true) == 0);

                return temp != null ? "" : "none";
            }
            // return "inline";
            //bool result = GetUserMenuFunc(menuId, funId);
            //return result ? "inline" : "none";
        }

        #region 获取母版页内控件

        public static Control GetMasterControl(Page page, string ControlId)
        {
            return (Control)page.Master.FindControl(ControlId);
        }

        public static DropDownList GetMasterDropDownList(Page page, string ControlId)
        {
            return (DropDownList)page.Master.FindControl(ControlId);
        }

        public static Label GetMasterLabel(Page page, string ControlId)
        {
            return (Label)page.Master.FindControl(ControlId);
        }

        public static TextBox GetMasterTextBox(Page page, string ControlId)
        {
            return (TextBox)page.Master.FindControl(ControlId);
        }

        #endregion 获取母版页内控件

        #region 日志枚举

        public enum LogType
        {
            //登录日志
            Login,

            //操作日志
            Operating
        }

        #endregion 日志枚举

        #region 获取当前登录用户信息

        /// <summary>
        /// 获取当前登录用户信息
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetUser()
        {
            //u.UserNumber = "admin";
            UserInfo u = HttpContext.Current.Session["User"] as UserInfo;
            if (u == null)
            {
                //string transefer = "../Login/login.html";
                //string temp = string.Format(@"<script type='text/javascript' >alert('登录超时，请重新登录！'); window.top.location.href = '{0}'; </script>"
                //    , transefer);
                //System.Web.HttpContext.Current.Response.Write(temp);
                //System.Web.HttpContext.Current.Response.End();
            }
            //UserInfo u = new UserInfo();
            //u.UserNumber = "admin";
            //u.UserName = "管理员";
            return u;
        }

        #endregion 获取当前登录用户信息

        #region 登录超时跳转

        /// <summary>
        /// 登录超时跳转
        /// </summary>
        public static void LoginTimeout()
        {
            string transefer = "../Login/login.html";
            string temp = string.Format(@"<script type='text/javascript' >alert('登录超时，请重新登录！'); window.top.location.href = '{0}'; </script>"
                , transefer);
            System.Web.HttpContext.Current.Response.Write(temp);
            System.Web.HttpContext.Current.Response.End();
        }

        #endregion 登录超时跳转

        #region 导出Excel通用方法

        /// <summary>
        /// 导出Execl通用方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="fileName"></param>
        public static void ExpExcel(string sql, string fileName)
        {
            string error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Columns.Contains("Guid"))
            {
                dt.Columns.Remove("Guid");
            }
            ExcelHelper.Instance.ExpExcel(dt, fileName);
            //GridView gvOrders = new GridView();
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //HttpContext.Current.Response.Charset = "GB2312";
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8).ToString());
            //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");  //设置输出流为简体中文
            //System.Web.UI.Page page = new System.Web.UI.Page();
            //page.EnableViewState = false;
            //HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=GB2312\">");
            //System.IO.StringWriter sw = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            //gvOrders.AllowPaging = false;
            //gvOrders.AllowSorting = false;
            //gvOrders.DataSource = dt;
            //gvOrders.DataBind();
            //gvOrders.RenderControl(hw);
            //HttpContext.Current.Response.Write(sw.ToString());
            //HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 导出Execl通用方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="fileName"></param>
        public static void ExpExcel(DataTable dt, string fileName)
        {
            ExcelHelper.Instance.ExpExcel(dt, fileName);
            //string error = string.Empty;
            ////DataTable dt = SqlHelper.GetTable(sql, ref error);
            //if (dt.Columns.Contains("Guid"))
            //{
            //    dt.Columns.Remove("Guid");
            //}
            //GridView gvOrders = new GridView();
            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            //HttpContext.Current.Response.Charset = "GB2312";
            //HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8).ToString());
            //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");  //设置输出流为简体中文
            //System.Web.UI.Page page = new System.Web.UI.Page();
            //page.EnableViewState = false;
            //HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=GB2312\">");
            //System.IO.StringWriter sw = new System.IO.StringWriter();
            //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            //gvOrders.AllowPaging = false;
            //gvOrders.AllowSorting = false;
            //gvOrders.DataSource = dt;
            //gvOrders.DataBind();
            //gvOrders.RenderControl(hw);
            //HttpContext.Current.Response.Write(sw.ToString());
            //HttpContext.Current.Response.End();
        }

        #endregion 导出Excel通用方法

        #region 清空页面控件值

        public static void ResetControl(ControlCollection cc)
        {
            foreach (Control ctr in cc)
            {
                if (ctr.HasControls())
                {
                    ResetControl(ctr.Controls);
                }
                if (ctr is TextBox)
                {
                    ((TextBox)ctr).Text = string.Empty;
                }
                else if (ctr is DropDownList)
                {
                    ((DropDownList)ctr).SelectedIndex = 0;
                }
            }
        }

        #endregion 清空页面控件值

        #region 写日志

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logType">日志类型：登录日志，操作日志</param>
        /// <param name="eventContent">日志内容</param>
        /// <param name="remark">备注</param>
        public static void WriteLog(LogType type, string eventContent, string remark)
        {
            string error = string.Empty;
            string logtype = type == LogType.Login ? "登录日志" : "操作日志";
            string ip = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null) // 服务器， using proxy
            {
                //得到真实的客户端地址
                ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();  // Return real client IP.
            }
            else//如果没有使用代理服务器或者得不到客户端的ip  not using proxy or can't get the Client IP
            {
                //得到服务端的地址
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP.
            }
            string sql = string.Format(@"insert into PM_USER_LOG (APP_ID ,USER_ID ,LOG_TIME ,IP_ADDRESS ,EVENT_CONTENT,LOG_TYPE,AUDIT_DATE,AUDIT_PERSON,REMARK )
values('Rapid-Erp','{0}','{1}','{2}','{3}','{4}','','','{5}')", GetUser() == null ? "" : GetUser().UserNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ip, eventContent, logtype, remark);
            SqlHelper.ExecuteSql(sql, ref error);
        }

        #endregion 写日志

        #region 获取数据通用方法（阳俊）

        /// <summary>
        /// 获取数据通用方法
        /// </summary>
        /// <param name="url">请求的页面路径</param>
        /// <param name="btnName">查询按钮Id</param>
        /// <param name="height">编辑弹出窗口高度</param>
        /// <param name="width">编辑弹出窗口宽度</param>
        public static void GetPage(string url, string btnId, string height, string width)
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
            if (dt == null)
            {
                HttpContext.Current.Response.Write("0^^^");
                HttpContext.Current.Response.End();
                return;
            }

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
<input type='checkbox' name='subBox' value='{0}'/></td>{1}
<td><a href='###'  value='{0}' onclick=""OpenDialog('{2}?Id={0}&date={3}','{4}','{5}','{6}')""> <img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a></td></tr>", dr[1], tdTextTemp, url, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), btnId, height, width);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        //带有详细
        public static void GetPage(string editUrl, string detailFunctionForJS, string btnId, string height, string width)
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
                    if (i == 0 || i == 11)
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
                        else if (dt.Columns[i].ColumnName.Equals("报价单号"))
                        {
                            tdTextTemp += string.Format(@" <td><a href='###' title='点击进入详细' style='color:blue;'
onclick=""{1}('{0}','{2}')"">{0}</a>  </td>", dr[1], detailFunctionForJS, dr["报价单类型"]);
                        }
                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                text += string.Format(@"<tr><td>
<input type='checkbox' name='subBox' value='{0}'/></td>{1}
<td><a href='###'  value='{0}' onclick=""OpenDialog('{2}?Id={0}&date={3}','{4}','{5}','{6}')"">
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>

<a href='###'  onclick=""{8}('{0}','{7}')"">
<img src='../Img/detail.png' width='9' height='9' />
<span > [</span>   <label class='edit'>详细</label> <span >]</span></a>
</td></tr>", dr[1], tdTextTemp, editUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), btnId, height, width, dr["报价单类型"], detailFunctionForJS);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        #endregion 获取数据通用方法（阳俊）

        public static void LocalLog(string msg)
        {
            string fileDirectory = "C:/rapiderp.log";
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }
            string filePath = "C:/rapiderp.log/log.txt";

            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(msg);
                sw.Close();
                fs.Close();
            }
        }

        /// <summary>
        /// repeater合并单元格(注：在控件绑定完毕之后调用)
        /// </summary>
        /// <param name="rptInfo">repeater控件</param>
        /// <param name="columName">列名</param>
        public static void MergeCells(Repeater rptInfo, string columName)
        {
            // 合并单元格
            for (int i = rptInfo.Items.Count - 1; i > 0; i--)
            {
                HtmlTableCell oCell_previous = rptInfo.Items[i - 1].FindControl(columName) as HtmlTableCell;
                HtmlTableCell oCell = rptInfo.Items[i].FindControl(columName) as HtmlTableCell;

                oCell.RowSpan = (oCell.RowSpan == -1) ? 1 : oCell.RowSpan;
                oCell_previous.RowSpan = (oCell_previous.RowSpan == -1) ? 1 : oCell_previous.RowSpan;

                if (oCell.InnerText == oCell_previous.InnerText)
                {
                    oCell.Visible = false;
                    oCell_previous.RowSpan += oCell.RowSpan;
                }
            }
        }
    }
}