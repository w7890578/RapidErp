using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;
using System.Data;

namespace Rapid.SellManager
{
    public partial class TradingOrderDetail : System.Web.UI.Page
    {
        public static string show = "inline";
        public static string rp = "none";
        public static string hasEdit = "inline";
        public static string hasDelete = "inline";
        public static string userId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = string.Empty;
                string error = string.Empty;
                spAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0103", "Add");
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0103", "Print");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0103", "Delete");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0103", "Edit");
                //超级管理员操作
                if (Request["isEditYJQty"] != null && Request["isEditYJQty"] != "")
                {
                    string ordersNumber = Request["ordersNumber"];
                    string productnumber = Request["productnumber"];
                    string rowNumber = Request["RowNumber"];
                    string qty = Request["qty"];
                    List<string> sqlEdits = new List<string>();
                    string sqlEdit = string.Format(@" 
 update TradingOrderDetail set 
  DeliveryQty={0}
 ,NonDeliveryQty=Quantity-{0}
 where OdersNumber='{1}' and ProductNumber ='{2}' and RowNumber='{3}' 
", qty, ordersNumber, productnumber, rowNumber);
                    sqlEdits.Add(sqlEdit);
                    sqlEdits.Add("update TradingOrderDetail set Status ='已完成' where NonDeliveryQty =0");
                    sqlEdits.Add("update TradingOrderDetail set Status ='未完成' where NonDeliveryQty !=0");
                    sqlEdits.Add(@" update SaleOder set OrderStatus ='已完成' where OdersNumber in (
	                    select OdersNumber from TradingOrderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0
	                    union
                      select OdersNumber from MachineOderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0)");
                    sqlEdits.Add(@" update SaleOder set OrderStatus ='未完成' where OdersNumber not in (
	                    select OdersNumber from TradingOrderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0
	                    union
                      select OdersNumber from MachineOderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0)");
                    string errorEdit = "";
                    bool result = SqlHelper.BatchExecuteSql(sqlEdits, ref errorEdit);
                    ToolCode.Tool.WriteLog(Tool.LogType.Operating, "超级管理员编辑销售订单已交数量"
                        , string.Format("ordersNumber:{0},productnumber:{1},rowNumber:{2},qty:{3},操作结果：{4}"
                        , ordersNumber, productnumber, rowNumber, qty, result));
                    Response.Write(result ? "1" : errorEdit);
                    Response.End();
                    return;
                }


                //超级管理员操作
                if (Request["isEditQty"] != null && Request["isEditQty"] != "")//编辑数量
                {
                    string ordersNumber = Request["ordersNumber"];
                    string productnumber = Request["productnumber"];
                    string rowNumber = Request["RowNumber"];
                    string qty = Request["qty"];
                    //检查数量是否小于已交数量
                    string sqlCheck = string.Format(@"  
select COUNT(0) from TradingOrderDetail  where {0}<DeliveryQty and 
 OdersNumber='{1}' and ProductNumber ='{2}' and RowNumber='{3}' 
", qty, ordersNumber, productnumber, rowNumber);
                    if (!SqlHelper.GetScalar(sqlCheck).Equals("0"))
                    {
                        Response.Write("数量不能小于已交货数量");
                        Response.End();
                        return;
                    }
                    string errorEdit = "";
                    List<string> sqlEdits = new List<string>();
                    string sqlEdit = string.Format(@" 
 update TradingOrderDetail set 
 Quantity={0}
 ,NonDeliveryQty={0}-DeliveryQty
 ,TotalPrice=UnitPrice*{0}
 where OdersNumber='{1}' and ProductNumber ='{2}' and RowNumber='{3}' 
", qty, ordersNumber, productnumber, rowNumber);
                    sqlEdits.Add(sqlEdit);
                    sqlEdits.Add("update TradingOrderDetail set Status ='已完成' where NonDeliveryQty =0");
                    sqlEdits.Add("update TradingOrderDetail set Status ='未完成' where NonDeliveryQty !=0");
                    sqlEdits.Add(@" update SaleOder set OrderStatus ='已完成' where OdersNumber in (
	                    select OdersNumber from TradingOrderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0
	                    union
                      select OdersNumber from MachineOderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0)");
                    sqlEdits.Add(@" update SaleOder set OrderStatus ='未完成' where OdersNumber not in (
	                    select OdersNumber from TradingOrderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0
	                    union
                      select OdersNumber from MachineOderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0)");
                    bool result = SqlHelper.BatchExecuteSql(sqlEdits, ref errorEdit);
                    ToolCode.Tool.WriteLog(Tool.LogType.Operating, "超级管理员编辑销售订单数量"
                        , string.Format("ordersNumber:{0},productnumber:{1},rowNumber:{2},qty:{3},操作结果：{4}"
                        , ordersNumber, productnumber, rowNumber, qty, result));
                    
                    Response.Write(result ? "1" : errorEdit);
                    Response.End();
                    return;

                }



                if (!ToolManager.CheckQueryString("id"))
                {
                    Response.Write("未知销售订单");
                    Response.End();
                    return;
                }
                string OdersNumber = ToolManager.GetQueryString("Id");
                sql = string.Format(@"select c.MakeCollectionsModeId from SaleOder so inner join Customer c on so.CustomerId=c.CustomerId
inner join MakeCollectionsMode mcm on c.MakeCollectionsModeId=mcm.Id where  so.OdersNumber='{0}'", OdersNumber);
                string makecollectionsmode = SqlHelper.GetScalar(sql);
                if (makecollectionsmode.Equals("YSBF"))
                {
                    rp = "inline";
                }
                else
                {
                    rp = "none";
                }
                //标识删除
                if (ToolManager.CheckQueryString("IsDelete"))
                {


                    string productNumber = ToolManager.GetQueryString("ProductNumber");
                    string rownumber = ToolManager.GetQueryString("RowNumber");
                    sql = string.Format("delete TradingOrderDetail where OdersNumber ='{0}' and ProductNumber='{1}' and RowNumber='{2}'", OdersNumber, productNumber, rownumber);
                    bool result = SqlHelper.ExecuteSql(sql, ref error);
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除贸易销售订单明细" + OdersNumber, "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除贸易销售订单明细" + OdersNumber, "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                sql = string.Format(@"select cu.CustomerName,cu.ContactTelephone,cu.Contacts,cu.Fax,so.OrdersDate,so.CustomerOrderNumber,so.OdersNumber,pu.USER_NAME from SaleOder so left join PM_USER pu on so.ContactId=pu.USER_ID
left join Customer cu on so.CustomerId=cu.CustomerId where so.OdersNumber='{0}'", OdersNumber);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {

                    DataRow dr = dt.Rows[0];
                    lblContacts.Text = dr["Contacts"] == null ? "" : dr["Contacts"].ToString();
                    lblCustomerName.Text = dr["CustomerName"] == null ? "" : dr["CustomerName"].ToString();
                    lblCutomerId.Text = dr["CustomerName"] == null ? "" : dr["CustomerName"].ToString();
                    lblOrdersDate.Text = dr["OrdersDate"] == null ? "" : dr["OrdersDate"].ToString();
                    lblOdersNumber.Text = dr["CustomerOrderNumber"] == null ? "" : dr["CustomerOrderNumber"].ToString();
                    lblFax.Text = dr["Fax"] == null ? "" : dr["Fax"].ToString();
                    lblUserName.Text = dr["USER_NAME"] == null ? "" : dr["USER_NAME"].ToString();
                    lblContactTelephone.Text = dr["ContactTelephone"] == null ? "" : dr["ContactTelephone"].ToString();


                }
                Bind();
            }
        }

        private void Bind()
        {
            userId = ToolCode.Tool.GetUser().UserNumber;
            string sql = string.Empty;
            string error = string.Empty;
            string odersnumber = ToolManager.GetQueryString("id");
            sql = string.Format(@" 
select modt.OdersNumber,modt.ProductNumber,
 modt.RowNumber,modt.Delivery,modt.SN,modt.ProductModel,
 modt.CustomerMaterialNumber,modt.MaterialName,
 modt.Brand,modt.Quantity,modt.NonDeliveryQty,
 modt.DeliveryQty,cast(modt.UnitPrice as varchar(10)) as UnitPrice,
 modt.TotalPrice,modt.CreateTime,modt.Remark,modt.Guid,
 modt.Status,modt.FinishedQty,CAST(modt.ReceiveOne as varchar(10)) as ReceiveOne,
 CAST(modt.ReceiveTwo as varchar(10)) as ReceiveTwo,
 case when CAST( modt.Delivery as datetime)<=CAST( so.CreateTime as datetime) then 'red' else 'black' end color ,
 cast(ROW_NUMBER ()over(order by modt.sn asc )as varchar(10)) as num
  from TradingOrderDetail modt inner join SaleOder so on modt.OdersNumber =so.OdersNumber where modt.OdersNumber='{0}' ", odersnumber);

            string tempsql = sql + string.Format(@"
union all
select '合计','','10000000','','','','','','',SUM(t.Quantity ),SUM (NonDeliveryQty),SUM (DeliveryQty),'',SUM(TotalPrice ),'','','','','','','','','' from ( {0} )t", sql);
            sql = string.Format("select *, row_number() over(order by CAST( t.RowNumber as int  )asc) as numNew from ({0}) t   order by CAST( t.RowNumber as int  )asc", tempsql);
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();
            sql = string.Format("select CheckTime  from SaleOder where OdersNumber='{0}'", odersnumber);
            show = SqlHelper.GetScalar(sql) == "" ? "inline" : "none";
            hdnumber.Value = ToolManager.GetQueryString("id");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
