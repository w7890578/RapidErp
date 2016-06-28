using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;

namespace Rapid.SellManager.ajax
{
    public partial class addoreditSaleOder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = string.Empty;
            string error = string.Empty;

            string ProductType = ToolManager.GetParamsString("ProductType");
            string OdersType = ToolManager.GetParamsString("OdersType");
            string CustomerOrderNumber = ToolManager.GetParamsString("CustomerOrderNumber");
            string KhddH = ToolManager.GetParamsString("KhddH");
            string OrdersDate = ToolManager.GetParamsString("OrdersDate");
            string CustomerId = ToolManager.GetParamsString("CustomerId");
            string CustomerName = ToolManager.GetParamsString("CustomerName");
            string ContactId = ToolManager.GetParamsString("ContactId");
            string Remark = ToolManager.GetParamsString("Remark");
            string Id = ToolManager.GetParamsString("Id");
            string makeCollectionsMode = ToolManager.GetParamsString("makeCollectionsMode");

            CustomerId = GetCustomerId(CustomerName);


            if (!ExitContactId(ContactId))
            {
                Response.Write("业务员不存在！请选择正确的业务员名称");
                Response.End();
                return;
            }
            if (string.IsNullOrEmpty(CustomerId))
            {
                Response.Write("客户不存在！请选择正确的客户名称");
                Response.End();
                return;
            }


            if (!string.IsNullOrEmpty(Id))
            {
                string odersNumber = Id;
                sql = string.Format(@"update SaleOder set OrdersDate='{0}',OdersType='{1}',ProductType='{2}',IsMinusStock='{3}'
,MakeCollectionsMode='{4}',CustomerId='{5}',ContactId ='{6}',OrderStatus='{7}' 
,Remark ='{8}',CustomerOrderNumber='{10}',KhddH='{11}' where OdersNumber ='{9}'", OrdersDate, OdersType, ProductType, "是", makeCollectionsMode
, CustomerId, ContactId, "未完成", Remark, odersNumber, CustomerOrderNumber, KhddH);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑销售订单" + odersNumber, "编辑成功");
                    Response.Write("ok");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑销售订单" + odersNumber, "编辑失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }
            else
            {
                sql = string.Format(@"
select MakeCollectionsModeId from customer where CustomerId='{0}'", CustomerId);
                makeCollectionsMode = SqlHelper.GetScalar(sql);
                string odersnumber = string.Empty;
                odersnumber = "XS" + DateTime.Now.ToString("yyyyMMddHHmmss");
                sql = string.Format(@"insert into SaleOder (OdersNumber ,OrdersDate,OdersType,ProductType
,IsMinusStock,MakeCollectionsMode,CustomerId,ContactId ,OrderStatus,CreateTime ,Remark,CustomerOrderNumber ,KhddH)
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','未完成','{8}','{9}','{10}','{11}')", odersnumber,
   OrdersDate, OdersType, ProductType, "是", makeCollectionsMode, CustomerId, ContactId
   , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Remark, CustomerOrderNumber, KhddH);
                bool result = SqlHelper.ExecuteSql(sql, ref error);

                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加销售订单" + odersnumber, "增加成功");
                    Response.Write("ok");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加销售订单" + odersnumber, "增加失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }


        }

        private string GetCustomerId(string customerName)
        {
            string sql = "select CustomerId from Customer(nolock) where CustomerName='" + customerName + "'";
            return SqlHelper.GetScalar(sql);
        }

        private bool ExitCustomerId(string CustomerId)
        {
            string sql = "select count(0) from Customer(nolock) where CustomerId='" + CustomerId + "'";
            object ob = SqlHelper.GetScalar(sql);
            if (ob != null && Convert.ToInt32(ob) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private bool ExitContactId(string user_id)
        {
            string sql = "select count(0) from PM_USER(nolock) where user_id='" + user_id + "'";
            object ob = SqlHelper.GetScalar(sql);
            if (ob != null && Convert.ToInt32(ob) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}