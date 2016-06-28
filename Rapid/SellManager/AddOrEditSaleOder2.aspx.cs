using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Text;
namespace Rapid.SellManager
{
    public partial class AddOrEditSaleOder2 : System.Web.UI.Page
    {
        public DataTable dtClient = new DataTable();
        public DataTable dtContact = new DataTable();
        public DataTable dtMakeCollectionsMode = new DataTable();
        public SaleOderEntity model = new SaleOderEntity();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = string.Empty;

                sql = "select CustomerId,CustomerName from Customer(nolock)";
                dtClient = SqlHelper.GetTable(sql);

                sql = "select user_id, USER_NAME from PM_USER(nolock)";
                dtContact = SqlHelper.GetTable(sql);

                sql = "select id,makecollectionsmode from MakeCollectionsMode(nolock)";
                dtMakeCollectionsMode = SqlHelper.GetTable(sql);

                if (ToolManager.CheckQueryString("id"))
                {
                    string odersNumber = ToolManager.GetQueryString("id");
                    string error = string.Empty;
                    sql = string.Format(@" select s.*,c.CustomerName from SaleOder s
 inner join Customer c on s.CustomerId=c.CustomerId
  where s.OdersNumber='{0}'", odersNumber);
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    if (dt.Rows.Count <= 0)
                    {
                        Response.Write("未知订单，该订单不存在或已被删除！");
                        Response.End();
                        return;
                    }
                    DataRow dr = dt.Rows[0];
                    model.OdersNumber = odersNumber;
                    model.OrdersDate = dr["OrdersDate"] == null ? "" : dr["OrdersDate"].ToString();
                    model.OdersType = dr["OdersType"] == null ? "" : dr["OdersType"].ToString();
                    model.ProductType = dr["ProductType"] == null ? "" : dr["ProductType"].ToString();
                    model.MakeCollectionsMode = dr["MakeCollectionsMode"] == null ? "" : dr["MakeCollectionsMode"].ToString();
                    model.CustomerId = dr["CustomerId"] == null ? "" : dr["CustomerId"].ToString();
                    model.ContactId = dr["ContactId"] == null ? "" : dr["ContactId"].ToString();
                    model.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    model.KhddH = dr["KhddH"] == null ? "" : dr["KhddH"].ToString();
                    model.CustomerName = dr["CustomerName"] == null ? "" : dr["CustomerName"].ToString();
                    model.CustomerOrderNumber = dr["CustomerOrderNumber"] == null ? "" : dr["CustomerOrderNumber"].ToString();

                }
            }
        }

        public class SaleOderEntity
        {
            public string CustomerName { get; set; }
            private string productType = string.Empty;
            private string odersType = string.Empty;
            private string customerOrderNumber = string.Empty;
            private string khddH = string.Empty;
            private string ordersDate = string.Empty;
            private string customerId = string.Empty;
            private string contactId = string.Empty;
            private string makeCollectionsMode = string.Empty;
            private string remark = string.Empty;
            private string odersNumber = string.Empty;

            public string OdersNumber
            {
                get { return odersNumber; }
                set { odersNumber = value; }
            }
            public string Remark
            {
                get { return remark; }
                set { remark = value; }
            }

            public string MakeCollectionsMode
            {
                get { return makeCollectionsMode; }
                set { makeCollectionsMode = value; }
            }

            public string ContactId
            {
                get { return contactId; }
                set { contactId = value; }
            }

            public string CustomerId
            {
                get { return customerId; }
                set { customerId = value; }
            }

            public string OrdersDate
            {
                get { return ordersDate; }
                set { ordersDate = value; }
            }
            public string KhddH
            {
                get { return khddH; }
                set { khddH = value; }
            }

            public string CustomerOrderNumber
            {
                get { return customerOrderNumber; }
                set { customerOrderNumber = value; }
            }

            public string OdersType
            {
                get { return odersType; }
                set { odersType = value; }
            }
            public string ProductType
            {
                get { return productType; }
                set { productType = value; }
            }
        }
    }
}