using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.PurchaseManager
{
    public partial class DDDetail : System.Web.UI.Page
    {
        public static string sortGuiZe = "DESC";
        public static string sortName = "订单需求数";
        public static string sortSql = string.Empty;
        public static string count = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //订单需求数
            if (!IsPostBack)
            {

                //if (ToolManager.CheckQueryString("MateriNumber"))
                //{
                //    Response.Write("原材料编号为：" + ToolManager.GetQueryString("MateriNumber"));
                //    Response.End();
                //    return;

                //}
                LoadPage();
            }
        }
        private void LoadPage()
        {
            string materiNumber = ToolManager.GetQueryString("MateriNumber");
            string sql = string.Format(@"select SUM(订单需求数) from V_SaleOrder_Demand where 原材料编号='{0}' ", materiNumber);
            count = SqlHelper.GetScalar(sql);
            sortSql = sortName + " " + sortGuiZe;
            sql = string.Format("  select * from V_SaleOrder_Demand where 原材料编号='{0}' order by   {1}", materiNumber, sortSql);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();

            lbordernum.Text = "订单需求数";
            lborder.Text = "销售订单号";
            lbjq.Text = "交期";

            if (sortName.Equals("订单需求数"))
            {
                lbordernum.Text = sortGuiZe.ToUpper() == "DESC" ? "订单需求数▲" : "订单需求数▼";
            }
            else if (sortName.Equals("销售订单号"))
            {
                lborder.Text = sortGuiZe.ToUpper() == "DESC" ? "销售订单号▲" : "销售订单号▼";
            }
            else
            {
                lbjq.Text = sortGuiZe.ToUpper() == "DESC" ? "交期▲" : "交期▼";
            }
        }

        protected void lbordernum_Click(object sender, EventArgs e)
        {
            if (sortGuiZe.ToUpper() == "DESC")
            {
                sortGuiZe = "ASC";
            }
            else
            {
                sortGuiZe = "DESC";
            }
            sortName = "订单需求数";
            sortSql = sortName + sortGuiZe;
            LoadPage();
        }

        protected void lborder_Click(object sender, EventArgs e)
        {
            if (sortGuiZe.ToUpper() == "DESC")
            {
                sortGuiZe = "ASC";
            }
            else
            {
                sortGuiZe = "DESC";
            }
            sortName = "销售订单号";
            sortSql = sortName + sortGuiZe;
            LoadPage();
        }

        protected void lbjq_Click(object sender, EventArgs e)
        {
            if (sortGuiZe.ToUpper() == "DESC")
            {
                sortGuiZe = "ASC";
            }
            else
            {
                sortGuiZe = "DESC";
            }
            sortName = "交期";
            sortSql = sortName + sortGuiZe;
            LoadPage();
        }
    }
}
