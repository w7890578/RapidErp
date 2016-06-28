using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Data;
using DAL;
using Microsoft.Win32;
using System.IO;
using System.Data.OleDb;

namespace BLL
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public static class ToolManager
    {

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
            if (dt.Columns.Contains("guid"))
            {
                dt.Columns.Remove("guid");
            }

            ExcelHelper.Instance.ExpExcel(dt, fileName);

            //GridView gvOrders = new GridView();
            //// gvOrders.RowDataBound += gridview1_RowDataBound;
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

        public static void ExpExcel(DataTable dt, string fileName)
        {
            string error = string.Empty;
            if (dt.Columns.Contains("Guid"))
            {
                dt.Columns.Remove("Guid");
            }
            if (dt.Columns.Contains("guid"))
            {
                dt.Columns.Remove("guid");
            }
            ExcelHelper.Instance.ExpExcel(dt, fileName);
            //GridView gvOrders = new GridView();
            ////gvOrders.RowDataBound += gridview1_RowDataBound;
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

        //protected void gridview1_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        for (int i = 0; i < e.Row.Cells.Count; i++)
        //        {
        //            e.Row.Cells[i].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
        //        }
        //    }
        //    else if (e.Row.RowType == DataControlRowType.Footer)
        //    {
        //    }
        //}

        #region 检测URL参数是否存在(Get)
        /// <summary>
        /// 检测URL参数是否存在(Get)
        /// </summary>
        /// <param name="p_strParam"></param>
        /// <returns></returns>
        public static bool CheckQueryString(string p_strParam)
        {
            if (HttpContext.Current.Request.QueryString[p_strParam] != null && HttpContext.Current.Request.QueryString[p_strParam].Trim() != "")
            {
                return true;
            }
            return false;
        }
        #endregion


        #region 检测URL参数是否存在(Post)
        /// <summary>
        /// 检测URL参数是否存在(Post)
        /// </summary>
        /// <param name="p_strParam"></param>
        /// <returns></returns>
        public static bool CheckParams(string p_strParam)
        {
            if (HttpContext.Current.Request.Params[p_strParam] != null && HttpContext.Current.Request.Params[p_strParam].Trim() != "")
            {
                return true;
            }
            return false;
        }
        #endregion


        #region 获得指定Url参数的值(Get)
        /// <summary>
        /// 获得指定Url参数的值(Get)
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.QueryString[strName].Trim();

        }
        #endregion

        public static int GetRequestInt(string requestName)
        {
            object result = HttpContext.Current.Request[requestName];
            result = result == null ? string.Empty : result.ToString().Trim();
            int temp = 0;
            int.TryParse(result.ToString(), out temp);
            return temp;
        }

        public static double GetRequestDouble(string requestName)
        {
            object result = HttpContext.Current.Request[requestName];
            result = result == null ? string.Empty : result.ToString().Trim();
            Double temp = 0.00;
            Double.TryParse(result.ToString(), out temp);
            return temp;
        }

        public static string GetRequestString(string requestName)
        {
            object result = HttpContext.Current.Request[requestName];
            result = result == null ? string.Empty : result.ToString().Trim();
            return result.ToString();
        }

        public static DateTime GetRequestDateTime(string requestName)
        {
            object result = HttpContext.Current.Request[requestName];
            result = result == null ? string.Empty : result.ToString().Trim();
            DateTime temp = DateTime.Parse("1900-01-01");
            DateTime.TryParse(result.ToString(), out temp);
            return temp;
        }


        #region 获取Url参数的值
        /// <summary>
        /// 获取Url参数的值 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetParamsString(string name)
        {
            if (HttpContext.Current.Request[name] == null)
            {
                return "";
            }
            return HttpContext.Current.Request[name].Trim();
        }

        #endregion

        #region 获取cookie对应的int值

        /// <summary>
        /// 获取cookie对应的int值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetCookieInt(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] == null)
            {
                return 1;
            }
            return Convert.ToInt32(HttpContext.Current.Request.Cookies[key].Value);
        }

        #endregion

        #region 获取cookie对应的string值

        /// <summary>
        /// 获取cookie对应的string值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookieStr(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.Cookies[key].Value;
        }

        #endregion

        #region 获取配置文件的值[AppSettings]

        /// <summary>
        /// 获取配置文件的值[AppSettings]
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string GetAppSettingsForWebconfig(string keyName)
        {
            return ConfigurationManager.AppSettings[keyName] == null ? "" : ConfigurationManager.AppSettings[keyName].ToString();
        }

        #endregion

        #region 获取配置文件的值[ConnectionStrings]

        /// <summary>
        /// 获取配置文件的值[ConnectionStrings]
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string GetConnectionStringsForWebconfig(string keyName)
        {
            return ConfigurationManager.ConnectionStrings[keyName] == null ? "" : ConfigurationManager.ConnectionStrings[keyName].ConnectionString;
        }

        #endregion

        #region 获得分页输出HTML

        /// <summary>
        /// 获得分页输出HTML
        /// </summary>
        /// <param name="preUrl"></param>
        /// <param name="totalRecords"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static string PagerGetAjax(string preUrl, int totalRecords, int currentPage, int pageSize, ref int pageCounts)
        {

            int pindex = preUrl.IndexOf("&pageindex=");
            int index = preUrl.IndexOf("?");
            if (pindex > 0)//存在pageindex
                preUrl = preUrl.Remove(pindex);
            if (index < 0)//不存在？
                preUrl += "?1=1";
            preUrl += "&pageindex=";
            if (totalRecords == 0)
                return "";
            int alter = 5;//默认页码数量
            int startPage = 1;
            int endPage = currentPage + alter;
            int totalPages = totalRecords / pageSize;//获取总页数

            //if (currentPage > pageCounts)//如果当前请求页大于总页数
            //{
            //    currentPage = 1;
            //}
            if ((totalRecords % pageSize) > 0)//总记录数量和页码取余有余数则总页数加一
                totalPages++;
            pageCounts = totalPages;

            if (totalPages == 1)//总页数为1则不需要分页
                return "";

            if (currentPage > alter)
            {
                startPage = currentPage - alter;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
            }

            //string strTemp = " <a href='{0}{1}'>{2}</a> ";
            // 每页 10 条&nbsp;&nbsp;共有 30 条记录&nbsp;&nbsp;共 3 页&nbsp;&nbsp;当前第 1 页</div>
            StringBuilder sb = new StringBuilder("");
            sb.Append("<div style='float:left;'>每页显示 <b>" + pageSize.ToString() + "</b> 条&nbsp;&nbsp;共有 <b>" + totalRecords + "</b> 条记录&nbsp;&nbsp;共 <b>" + totalPages + "</b> 页&nbsp;&nbsp;当前第 <b>" + currentPage + "</b> 页</div><div style='float:right;'>");
            if (currentPage != 1)
                sb.Append(" <a  href='###' onclick=\"aClick('第一页')\">第一页</a> ");
            // sb.Append(string.Format(strTemp, preUrl, 1, "第一页"));

            if (currentPage != startPage)
            {
                sb.Append(" <a  href='###' onclick=\"aClick('上一页')\">上一页</a> ");
                //sb.Append(string.Format(strTemp, preUrl, currentPage - 1, "上一页"));

            }

            for (int i = startPage; i <= endPage; i++)
            {
                if (currentPage == i)
                {
                    sb.Append(" <a class='current' href='javascript:;'> " + i + "</a> ");
                }
                else
                {
                    sb.Append(" <a  href='###' onclick=\"aClick('" + i + "')\"> " + i + "</a> ");
                    // sb.Append(string.Format(strTemp, preUrl, i, i));
                }
            }

            if (currentPage != endPage)
            {
                sb.Append(" <a  href='###'  onclick=\"aClick('下一页')\">下一页</a> ");
                //sb.Append(string.Format(strTemp, preUrl, currentPage + 1, "下一页"));


            }
            if (currentPage != totalPages)
                sb.Append(" <a  href='###' onclick=\"aClick('最后一页')\">最后一页</a> ");
            //sb.Append(string.Format(strTemp, preUrl, totalPages, "最后一页"));
            sb.Append("</div>");
            return sb.ToString();
        }

        /// <summary>
        /// 获得分页输出HTML
        /// </summary>
        /// <param name="preUrl"></param>
        /// <param name="totalRecords"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static string PagerGet(string preUrl, int totalRecords, int currentPage, int pageSize)
        {
            int pindex = preUrl.IndexOf("&pageindex=");
            int index = preUrl.IndexOf("?");
            if (pindex > 0)//存在pageindex
                preUrl = preUrl.Remove(pindex);
            if (index < 0)//不存在？
                preUrl += "?1=1";
            preUrl += "&pageindex=";
            if (totalRecords == 0)
                return "";
            int alter = 5;//默认页码数量
            int startPage = 1;
            int endPage = currentPage + alter;
            int totalPages = totalRecords / pageSize;//获取总页数
            if ((totalRecords % pageSize) > 0)//总记录数量和页码取余有余数则总页数加一
                totalPages++;
            if (totalPages == 1)//总页数为1则不需要分页
                return "";

            if (currentPage > alter)
            {
                startPage = currentPage - alter;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
            }

            string strTemp = " <a href='{0}{1}'>{2}</a> ";
            StringBuilder sb = new StringBuilder("");
            sb.Append("<div style='float:left;'>共有 <b>" + totalRecords + "</b> 条记录</div><div style='float:right;'>");
            if (currentPage != 1)
                sb.Append(string.Format(strTemp, preUrl, 1, "第一页"));
            if (currentPage != startPage)
            {
                sb.Append(string.Format(strTemp, preUrl, currentPage - 1, "上一页"));
            }

            for (int i = startPage; i <= endPage; i++)
            {
                if (currentPage == i)
                {
                    sb.Append(" <a class='current' href='javascript:;'> " + i + "</a> ");
                }
                else
                {
                    sb.Append(string.Format(strTemp, preUrl, i, i));
                }
            }

            if (currentPage != endPage)
            {
                sb.Append(string.Format(strTemp, preUrl, currentPage + 1, "下一页"));
            }
            if (currentPage != totalPages)
                sb.Append(string.Format(strTemp, preUrl, totalPages, "最后一页"));
            sb.Append("</div>");
            return sb.ToString();
        }
        #endregion

        #region 发送电子邮件
        /// <summary>
        /// 发送电子邮件
        /// </summary>
        /// <param name="strSmtpServer">smtp服务地址</param>
        /// <param name="strFrom">发送邮箱地址</param>
        /// <param name="strFromPass">登录密码</param>
        /// <param name="strto">接受邮件地址</param>
        /// <param name="strSubject">邮件主题</param>
        /// <param name="strBody">邮件内容</param>
        /// <param name="strPath">记录物理地址</param>
        public static void SendEMail(string strSmtpServer, string strFrom, string strFromPass, string strto, string strSubject, string strBody, string strPath)
        {
            System.Net.Mail.SmtpClient client = new SmtpClient(strSmtpServer, 25);
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(strFrom, strFromPass);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(strFrom, strto, strSubject, strBody);
            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(strPath);
            message.Attachments.Add(attachment);
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            client.Send(message);
        }

        #endregion

        #region 获取月份所在季度

        /// <summary>
        /// 获取月份所在季度
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int Quarter_Get(int month)
        {
            int quarter = 0;
            if (month >= 1 && month <= 3)
            {
                quarter = 1;
            }
            else if (month >= 4 && month <= 6)
            {
                quarter = 2;
            }
            else if (month >= 7 && month <= 9)
            {
                quarter = 3;
            }
            else if (month >= 10 && month <= 12)
            {
                quarter = 4;
            }
            return quarter;
        }
        #endregion

        #region 获得当前完整Url地址

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        #endregion

        #region 测试ping服务器

        /// <summary>
        /// 测试ping服务器
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool CmdPing()
        {
            string ipAddress = GetAppSettingsForWebconfig("ServerIP") == "" ? "192.168.0.3" : GetAppSettingsForWebconfig("ServerIP");
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send(ipAddress, 1000);

            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }
        #endregion

        #region 获取中文星期

        /// <summary>
        /// 获取中文星期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetChineseWeek(DateTime date)
        {
            string week = "";
            switch (date.DayOfWeek.ToString())
            {
                case "Monday":
                    week = "周一";
                    break;
                case "Tuesday":
                    week = "周二";
                    break;
                case "Wednesday":
                    week = "周三";
                    break;
                case "Thursday":
                    week = "周四";
                    break;
                case "Friday":
                    week = "周五";
                    break;
                case "Saturday":
                    week = "周六";
                    break;
                case "Sunday":
                    week = "周日";
                    break;
            }
            return week;
        }
        #endregion

        #region  判断是否为日期格式


        /// <summary>
        /// 判断是否为日期格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateFormat(string date)
        {
            string regstr = @"^(?:(?!0000)[0-9]{4}-(?:(?:0[1-9]|1[0-2])-(?:0[1-9]|1[0-9]|2[0-8])|(?:0[13-9]|1[0-2])-(?:29|30)|(?:0[13578]|1[02])-31)|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)-02-29)$";
            if (System.Text.RegularExpressions.Regex.IsMatch(date, regstr))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 判断是否为手机格式

        /// <summary>
        /// 判断是否为手机格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsMobileFormat(string date)
        {
            string regstr = @"^[1][3-8]+\d{9}$";
            if (System.Text.RegularExpressions.Regex.IsMatch(date, regstr))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 是否为日期型字符串

        /// <summary>
        /// 是否为日期型字符串
        /// </summary>
        /// <param name="StrSource">日期字符串(2008-05-08)</param>
        /// <returns></returns>
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }

        #endregion

        #region 是否为时间型字符串

        /// <summary>
        /// 是否为时间型字符串
        /// </summary>
        /// <param name="source">时间字符串(15:00:00)</param>
        /// <returns></returns>
        public static bool IsTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        #endregion

        #region 是否为日期+时间型字符串

        /// <summary>
        /// 是否为日期+时间型字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDateTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string MdshashString(string pwd)
        {
            pwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(pwd, "MD5");
            return pwd;
        }
        #endregion
        #region 上传文件
        //上传文件方法  
        public static bool Upload(FileUpload myFileUpload)
        {
            bool flag = false;
            //是否允许上载  
            bool fileAllow = false;
            //设定允许上载的扩展文件名类型  
            string[] allowExtensions = { ".xls", ".xlsx" };

            //取得网站根目录路径  
            string path = HttpContext.Current.Request.MapPath("~/Upload/");
            if (myFileUpload.HasFile)
            {
                string fileExtension = System.IO.Path.GetExtension(myFileUpload.FileName).ToLower();
                for (int i = 0; i < allowExtensions.Length; i++)
                {
                    if (fileExtension == allowExtensions[i])
                    {
                        fileAllow = true;
                    }
                }

                if (fileAllow)
                {
                    try
                    {
                        //存储文件到文件夹 
                        myFileUpload.SaveAs(path + myFileUpload.FileName);
                        //Response.Write("<script>alert('文件导入成功')</script>");
                        flag = true;
                    }
                    catch (Exception ex)
                    {
                        //Response.Write("<script>alert('" + ex.Message + "')</script>");
                        flag = false;
                    }
                }
                else
                {
                    HttpContext.Current.Response.Write("<script>alert('不允许上载：" + myFileUpload.PostedFile.FileName + "，只能上传xls的文件，请检查！')</script>");
                    flag = false;
                }
            }
            else
            {
                // lblMes.Text = "请选择要导入的excel文件!";
                flag = false;
            }
            return flag;
        }
        #endregion


        #region 导入Excel
        public static DataSet ImpExcel(FileUpload FU_Excel, HttpServerUtility server)
        {

            bool b = Upload(FU_Excel);  // 上传excel文件 
            if (!b)
            {
                return null;
            }
            string name = FU_Excel.FileName;
            string filepath = server.MapPath("~/Upload/") + name;
            string extension = Path.GetExtension(FU_Excel.PostedFile.FileName);
            //DataSet ds = SqlHelper.ExcelDataSource(filepath, SqlHelper.ExcelSheetName(filepath)[0].ToString());
            DataSet ds = null;
            //); //先尝试用offic ds = ExcelToDataTable(filepathe
            try
            {
                ds = ExcelToDataTable(filepath); //先尝试用office
            }
            catch (Exception ex)
            {
                ToolManager.WriteLog("导入Excel报错(office模式)", string.Format("filepath:{0},ErrorMessage:{1}", filepath, ex.Message));
                ds = NPOIExcelHelper.GetDataFromExcel(filepath, extension);
                if (ds == null || ds.Tables.Count == 0)
                {
                    ToolManager.WriteLog("导入Excel报错(NPOI模式)", string.Format("filepath:{0},extension:{1}", filepath, extension));
                }
            }
            return ds;
        }
        #endregion


        public static DataSet ExcelToDataTable(string strExcelFileName)
        {
            string strConn;
            strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
            // strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + strExcelFileName + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)

            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter("select * from [Sheet1$]", strConn);
            DataSet myDataSet = new DataSet();
            try
            {
                myCommand.Fill(myDataSet, "ExcelInfo");
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message);
            }
            conn.Dispose();
            conn.Close();

            // DataTable table = myDataSet.Tables["ExcelInfo"].DefaultView.ToTable();
            return myDataSet;


            ////源的定义
            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strExcelFileName + ";" + "Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";

            ////Sql语句
            ////string strExcel = string.Format("select * from [{0}$]", strSheetName); 这是一种方法
            //string strExcel = "select * from   [sheet1$]";

            ////定义存放的数据表
            //DataSet ds = new DataSet();

            ////连接数据源
            //OleDbConnection conn = new OleDbConnection(strConn);

            //conn.Open();

            ////适配到数据源
            //OleDbDataAdapter adapter = new OleDbDataAdapter(strExcel, strConn);
            //adapter.Fill(ds, strSheetName);

            //conn.Close();

            //return ds.Tables[strSheetName];
        }



        #region 页面关闭脚本
        /// <summary>
        /// 获取关闭页面脚本
        /// </summary>
        /// <returns></returns>
        public static string GetClosePageJS()
        {
            return "<script>window.close();</script>";
        }
        public static void CloseCurrentPage()
        {

            HttpContext.Current.Response.Write(GetClosePageJS());
            HttpContext.Current.Response.End();
        }
        #endregion

        #region 获取多选的ListBox的值
        public static string GetValueForListBox(ListBox lb)
        {
            string result = string.Empty;
            foreach (ListItem item in lb.Items) //按类型listitem读取listbox中选定项
            {
                if (item.Selected) //判断是否选中
                {
                    result += string.Format("'{0}',", item.Value);
                }
            }
            return result.TrimEnd(',');
        }
        #endregion

        #region  替换字符串中的单引号'
        /// <summary>
        /// 替换字符串中的单引号'
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceSingleQuotesToBlank(string value)
        {
            return value.Replace('\'', ' ');
        }
        #endregion


        #region 获取员工下班时间
        /// <summary>
        /// 获取下班时间
        /// </summary>
        /// <returns></returns>
        public static string GetGoOffWorkTime()
        {
            return "17:30:00";
        }
        #endregion

        #region 获取员工上班时间
        /// <summary>
        /// 获取上班时间
        /// </summary>
        /// <returns></returns>
        public static string GetGoOnfWorkTime()
        {
            return "08:30:00";
        }
        #endregion

        public static void WriteLog(string eventContent, string remark)
        {
            string error = string.Empty;
            string sql = string.Format(@"insert into PM_USER_LOG (APP_ID ,USER_ID ,LOG_TIME ,IP_ADDRESS ,EVENT_CONTENT,LOG_TYPE,AUDIT_DATE,AUDIT_PERSON,REMARK )
values('Rapid-Erp','{0}','{1}','{2}','{3}','{4}','','','{5}')", "admin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "111", eventContent, "系统调试日志", remark);
            SqlHelper.ExecuteSql(sql);
        }


        /// <summary>
        /// 转换为数字
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static int ConvertInt(object ject)
        {
            string result = ject == null ? "" : ject.ToString();
            int value = 0;
            int.TryParse(result, out value);
            return value;
        }
        /// <summary>
        /// 转换double类型
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static double ConvertDouble(object ject)
        {
            string result = ject == null ? "" : ject.ToString();
            double value = 0.00;
            Double.TryParse(result, out value);
            return value;
        }

        /// <summary>
        /// 数据自动恢复错误。
        /// </summary>
        public static bool ZDJC()
        {
            List<string> sqls = new List<string>();
            string sql = @"
update 
	MachineOderDetail 
set 
	DeliveryQty=t.ConformenceQty,
	NonDeliveryQty=Qty-t.ConformenceQty,  
	Status=
	(
		case   when (Qty-t.ConformenceQty)=0
		then   '已完成'
		else   '未完成'
		end
	)
from  
MachineOderDetail m 
inner join 
	( 
	select 
		OrdersNumber,
		ProductNumber ,
		Version ,
		RowNumber,
		SUM(ConformenceQty) ConformenceQty 
	from 
		DeliveryNoteDetailed dnd inner join DeliveryBill db on dnd.DeliveryNumber=db.DeliveryNumber
		where db.IsConfirm='已确认'
	group by 
		OrdersNumber,ProductNumber ,Version ,RowNumber
	) t 
on 
t.OrdersNumber=m.OdersNumber 
and t.ProductNumber=m.ProductNumber
and t.Version=m.Version 
and t.RowNumber=m.RowNumber
where m.DeliveryQty!=t.ConformenceQty 
";
            sqls.Add(sql);
            sql = @"
update 
TradingOrderDetail 
set 
	DeliveryQty=t.ConformenceQty,
	NonDeliveryQty=Quantity-t.ConformenceQty,
	Status = 
		(
			case 
				when 
					(Quantity-t.ConformenceQty)=0 
				then 
					'已完成' 
				else 
					'未完成'
				end
		) 
from  
TradingOrderDetail m 
inner join 
	( 
	select 
		OrdersNumber,
		ProductNumber ,
		RowNumber,
		SUM(ConformenceQty) ConformenceQty 
	from 
		DeliveryNoteDetailed dnd inner join DeliveryBill db on dnd.DeliveryNumber=db.DeliveryNumber
		where db.IsConfirm='已确认'
	group by 
		OrdersNumber,ProductNumber  ,RowNumber
	) t 
on 
t.OrdersNumber=m.OdersNumber 
and t.ProductNumber=m.ProductNumber
and t.RowNumber=m.RowNumber
where 
m.DeliveryQty!=t.ConformenceQty
 
";
            sqls.Add(sql);
            sql = @"update SaleOder set OrderStatus ='已完成' where OdersNumber in (
	                    select OdersNumber from TradingOrderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0
	                    union
                      select OdersNumber from MachineOderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0)";
            sqls.Add(sql);
            sql = string.Format(@"
update SaleOder set OrderStatus ='未完成' where OdersNumber not in (
	                    select OdersNumber from TradingOrderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0
	                    union
                      select OdersNumber from MachineOderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0)");
            sqls.Add(sql);
            sql = string.Format(@"update ProductPlanDetail  set StorageQty=a.总入库数量 from
(
select DocumentNumber,ProductNumber ,Version ,RowNumber,OrdersNumber,SUM(Qty) 总入库数量 from ProductWarehouseLogDetail
where DocumentNumber like'KG%'
group by DocumentNumber,ProductNumber ,Version ,RowNumber,OrdersNumber )
a
left join ProductPlanDetail b 
on a.DocumentNumber=b.PlanNumber
and a.OrdersNumber=b.OrdersNumber
and a.ProductNumber=b.ProductNumber
and a.Version=b.Version
and a.RowNumber=b.RowNumber
where a.总入库数量!=b.StorageQty");
            sqls.Add(sql);
            string error = string.Empty;
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        /// <summary>
        /// 转换为json格式数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertToJsonStr(DataTable dt)
        {
            StringBuilder result = new StringBuilder();
            result.Append("[");
            foreach (DataRow dr in dt.Rows)
            {
                result.Append("{");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    result.AppendFormat("'{0}':'{1}',", dt.Columns[i], dr[dt.Columns[i]]);
                }
                result.Remove(result.Length - 1, 1);
                result.Append("},");
            }
            result.Remove(result.Length - 1, 1);
            result.Append("]");
            return result.ToString();
        }
    }
}
