using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;
using System.Web.UI.WebControls;

namespace BLL
{
    public static class ProductProcessManager
    {
        #region 下拉框获取内容通用函数
        /// <summary>
        /// 下拉框通用获取内容函数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="keyCloumName">key字段名</param>
        /// <param name="valueCloumName">value字段名</param>
        /// <returns></returns>
        public static string GetOption(string sql, string keyCloumName, string valueCloumName)
        {
            string error = string.Empty;
            string result = "<option value =\"\">- - - - - 请 选 择 - - - - -</option>";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            foreach (DataRow dr in dt.Rows)
            {
                result += string.Format(" <option value ='{0}'>{1}</option> ", dr[keyCloumName], dr[valueCloumName]);
            }
            return result;
        }

        public static void BindDrp(string sql, DropDownList drp, string valueName, string textName)
        {
            drp.Items.Clear();
            string error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                drp.DataSource = dt;
                drp.DataValueField = valueName;
                drp.DataTextField = textName;
                drp.DataBind();
            }
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));


        }
        /// <summary>
        /// 绑定ListBox并设置为可多选
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="lb">控件</param>
        /// <param name="valueName">值字段</param>
        /// <param name="textName">文本字段</param>
        public static void BindListBox(string sql, ListBox lb, string valueName, string textName)
        {
            lb.Items.Clear();
            string error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                lb.DataSource = dt;
                lb.DataValueField = valueName;
                lb.DataTextField = textName;
                lb.DataBind();
            }
            lb.SelectionMode = ListSelectionMode.Multiple;
        }
        #endregion

        #region 销售订单号（生产进度表）
        public static string GetProductProcessOdersNumber()
        {
            string sqlorder = string.Format(@" select distinct t.销售订单号  from (select t.*,so.CustomerId ,p.Description from ({0})t   
        left join SaleOder so on t.销售订单号=so.OdersNumber left join Product p on t.产品编号=p.ProductNumber and t.版本=p.Version  ) t 
        left join Customer c on t.CustomerId=c.CustomerId left join ProductCustomerProperty pcp on t.CustomerId=pcp.CustomerId and 
        t.产品编号=pcp.ProductNumber and t.版本=pcp.Version", WorkOrderManager.GetOrderNofinesfinishedDetail());
            return GetOption(sqlorder, "销售订单号", "销售订单号");
        }
        #endregion

        #region 产成品编号（生产进度表）
        public static string GetProductProcessProductNumber()
        {
            string sqlproduct = string.Format(@" select distinct t.产品编号  from (select t.*,so.CustomerId ,p.Description from ({0})t   
        left join SaleOder so on t.销售订单号=so.OdersNumber left join Product p on t.产品编号=p.ProductNumber and t.版本=p.Version  ) t 
        left join Customer c on t.CustomerId=c.CustomerId left join ProductCustomerProperty pcp on t.CustomerId=pcp.CustomerId and 
        t.产品编号=pcp.ProductNumber and t.版本=pcp.Version", WorkOrderManager.GetOrderNofinesfinishedDetail());
            return GetOption(sqlproduct, "产品编号", "产品编号");
        }
        #endregion

        #region 版本（生产进度表）
        public static string GetProductProcessVersion()
        {
            string sqlversion = string.Format(@" select distinct t.版本  from (select t.*,so.CustomerId ,p.Description from ({0})t   
        left join SaleOder so on t.销售订单号=so.OdersNumber left join Product p on t.产品编号=p.ProductNumber and t.版本=p.Version  ) t 
        left join Customer c on t.CustomerId=c.CustomerId left join ProductCustomerProperty pcp on t.CustomerId=pcp.CustomerId and 
        t.产品编号=pcp.ProductNumber and t.版本=pcp.Version", WorkOrderManager.GetOrderNofinesfinishedDetail());
            return GetOption(sqlversion, "版本", "版本");
        }
        #endregion

        #region 客户产成品编号（生产进度表）
        public static string GetProductProcessCustomerProductNumber()
        {
            string sqlcustomerproductnumber = string.Format(@" select distinct pcp.CustomerProductNumber as 客户产成品编号  from (select t.*,so.CustomerId ,p.Description from ({0})t   
        left join SaleOder so on t.销售订单号=so.OdersNumber left join Product p on t.产品编号=p.ProductNumber and t.版本=p.Version  ) t 
        left join Customer c on t.CustomerId=c.CustomerId left join ProductCustomerProperty pcp on t.CustomerId=pcp.CustomerId and 
        t.产品编号=pcp.ProductNumber and t.版本=pcp.Version", WorkOrderManager.GetOrderNofinesfinishedDetail());
            return GetOption(sqlcustomerproductnumber, "客户产成品编号", "客户产成品编号");
        }
        #endregion

        #region 客户名称（生产进度表）
        public static string GetProductProcessCustomerName()
        {
            string sqlcustomername = string.Format(@" select distinct c.CustomerName as 客户名称  from (select t.*,so.CustomerId ,p.Description from ({0})t   
        left join SaleOder so on t.销售订单号=so.OdersNumber left join Product p on t.产品编号=p.ProductNumber and t.版本=p.Version  ) t 
        left join Customer c on t.CustomerId=c.CustomerId left join ProductCustomerProperty pcp on t.CustomerId=pcp.CustomerId and 
        t.产品编号=pcp.ProductNumber and t.版本=pcp.Version", WorkOrderManager.GetOrderNofinesfinishedDetail());
            return GetOption(sqlcustomername, "客户名称", "客户名称");
        }
        #endregion

        #region 产品描述（生产进度表）
        public static string GetProductProcessProductDescription()
        {
            string sqldescription = string.Format(@" select distinct t.Description as 产品描述 from (select t.*,so.CustomerId ,p.Description from ({0})t   
        left join SaleOder so on t.销售订单号=so.OdersNumber left join Product p on t.产品编号=p.ProductNumber and t.版本=p.Version  ) t 
        left join Customer c on t.CustomerId=c.CustomerId left join ProductCustomerProperty pcp on t.CustomerId=pcp.CustomerId and 
        t.产品编号=pcp.ProductNumber and t.版本=pcp.Version", WorkOrderManager.GetOrderNofinesfinishedDetail());
            return GetOption(sqldescription, "产品描述", "产品描述");
        }
        #endregion
    }
}
