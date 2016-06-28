using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using Model;
using System.Text;
using BLL;
namespace Rapid.SellManager
{
    public partial class AjaxGetCustomerJson : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = string.Format(@" select CustomerId,CustomerName from Customer ");
            DataTable dt = SqlHelper.GetTable(sql);
            Response.Write(ToolManager.ConvertToJsonStr(dt));
            Response.End();
            return;
            ////List<Customer> cus = new List<Customer>();
            //StringBuilder result = new StringBuilder();
            //result.Append("[");
            ////                " [
            ////{ "firstName":"Bill" , "lastName":"Gates" },
            ////{ "firstName":"George" , "lastName":"Bush" },
            ////{ "firstName":"Thomas" , "lastName": "Carter" }
            ////]";
            //foreach (DataRow dr in dt.Rows)
            //{
            //    result.AppendFormat("{{'CustomerId':'{0}',CustomerName:'{1}'}},", dr["CustomerId"].ToString(), dr["CustomerName"].ToString());
            //    //cus.Add(new Customer() { CustomerId = dr["CustomerId"].ToString (), CustomerName = dr["CustomerName"].ToString () });
            //}
            //result.Remove(result.Length - 1, 1);
            //result.Append("]"); 
            //JsonConvert.SerializeObject(list)； 
        }
    }
}
