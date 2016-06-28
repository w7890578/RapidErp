using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using BLL;
using System.Data;
using DAL;
using System.Text;

namespace Rapid.SellManager
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AjaxTextDrp : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
            Bind(context);
        }
        private void Bind(HttpContext context)
        {
            BindOrdersNumber(context);
            BindProductNumber(context);
            BindVersion(context);
            BindCustomerNumber(context);
        }
        private void BindCustomerNumber(HttpContext context)
        {
            if (ToolManager.CheckQueryString("versionP") && ToolManager.CheckQueryString("productNumberP"))
            {
                string error = string.Empty;
                string sql = string.Format(@"
select CustomerProductNumber from ProductCustomerProperty where ProductNumber='{0}' and Version='{1}'", ToolManager.GetQueryString("productNumberP"), ToolManager.GetQueryString("versionP"));
                string result = ControlBindManager.GetOption(sql, "CustomerProductNumber", "CustomerProductNumber").Trim();
                sql = string.Format("select ProductName from Product where ProductNumber='{0}' and Version ='{1}'", ToolManager.GetQueryString("productNumberP"), ToolManager.GetQueryString("versionP"));
                DataTable dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    string temp = dt.Rows[0]["ProductName"] == null ? "" : dt.Rows[0]["ProductName"].ToString();
                    result += "^" + temp;
                }
                context.Response.ContentType = "text/plain";
                context.Response.Write(result);
                context.Response.End();
                return;
            }

        }
        private void BindOrdersNumber(HttpContext context)
        {
            if (ToolManager.CheckQueryString("OrdersNumber"))
            {
                // 查询的参数名称默认为term
                string query = ToolManager.GetQueryString("term");
                string error = string.Empty;
                string sql = string.Format(@" 
            select   distinct  top 10 OdersNumber  from SaleOder where OdersNumber like '{0}%' or OdersNumber
            like '%{0}' or OdersNumber  like '%{0}%' order by OdersNumber desc", query);
                DataTable dt = SqlHelper.GetTable(sql, ref error);

                StringBuilder builder = new StringBuilder();
                builder.Append("[");

                foreach (DataRow dr in dt.Rows)
                {
                    builder.AppendFormat("\"{0}\",", dr[0]);
                }
                if (builder.Length > 1)
                    builder.Length = builder.Length - 1;
                builder.Append("]");

                context.Response.ContentType = "text/javascript";
                context.Response.Write(builder.ToString());
                context.Response.End();
                return;

            }
        }
        private void BindProductNumber(HttpContext context)
        {
            if (ToolManager.CheckQueryString("ProductNumber"))
            {
                // 查询的参数名称默认为term
                string query = ToolManager.GetQueryString("term");
                string error = string.Empty;
                string sql = string.Format(@" 
            select   distinct  top 10 ProductNumber  from Product where ProductNumber like '{0}%' or ProductNumber
            like '%{0}' or ProductNumber  like '%{0}%'", query);
                DataTable dt = SqlHelper.GetTable(sql, ref error);

                StringBuilder builder = new StringBuilder();
                builder.Append("[");

                foreach (DataRow dr in dt.Rows)
                {
                    builder.AppendFormat("\"{0}\",", dr[0]);
                }
                if (builder.Length > 1)
                    builder.Length = builder.Length - 1;
                builder.Append("]");

                context.Response.ContentType = "text/javascript";
                context.Response.Write(builder.ToString());
                context.Response.End();
                return;
            }
        }
        private void BindVersion(HttpContext context)
        {
            if (ToolManager.CheckQueryString("Version"))
            {
                // 查询的参数名称默认为term
                string query = ToolManager.GetQueryString("term");
                string error = string.Empty;
                string sql = string.Format(@" 
            select   distinct top 10 p.Version as 版本  from Product p where p.[Version] like '{0}%' or p.[Version]
            like '%{0}' or p.[Version]  like '%{0}%'", query);
                DataTable dt = SqlHelper.GetTable(sql, ref error);
                StringBuilder builder = new StringBuilder();
                builder.Append("[");

                foreach (DataRow dr in dt.Rows)
                {
                    builder.AppendFormat("\"{0}\",", dr[0]);
                }
                if (builder.Length > 1)
                    builder.Length = builder.Length - 1;
                builder.Append("]");

                context.Response.ContentType = "text/javascript";
                context.Response.Write(builder.ToString());
                context.Response.End();
                return;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
