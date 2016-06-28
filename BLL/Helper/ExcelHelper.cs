using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using DAL;

namespace BLL
{
    public class ExcelHelper
    {
        public static ExcelHelper Instance
        {
            get
            {
                return new ExcelHelper();
            }
        }
        private ExcelHelper()
        {

        }

        public void ExpExcel(DataTable dt, string fileName)
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
            GridView gvOrders = new GridView();
            gvOrders.RowDataBound += gridview1_RowDataBound;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Charset = "GB2312";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8).ToString());
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");  //设置输出流为简体中文 
            System.Web.UI.Page page = new System.Web.UI.Page();
            page.EnableViewState = false;
            HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=GB2312\">");
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
            gvOrders.AllowPaging = false;
            gvOrders.AllowSorting = false;
            gvOrders.DataSource = dt;
            gvOrders.DataBind();


            gvOrders.RenderControl(hw);
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }

        protected void gridview1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].Attributes.Add("style", "vnd.ms-excel.numberformat:@");
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
            }
        }
    }
}
