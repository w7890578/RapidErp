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
    public partial class MachineOderDetail : System.Web.UI.Page
    {
        public static string rp = "none";
        public static string show = "inline";
        public static string showBtn = "none";
        public static string hasEdit = "inline";
        public static string hasDelete = "inline";
        public static string userId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SaleOderManager.DatatRecovery();

                if (Request["isEditQty"] != null && Request["isEditQty"] != "")//编辑数量
                {
                    string ordersNumber = Request["ordersNumber"];
                    string productnumber = Request["productnumber"];
                    string version = Request["Version"];
                    string rowNumber = Request["RowNumber"];
                    string qty = Request["qty"];
                    //检查数量是否小于已交数量
                    string sqlCheck = string.Format(@"  
select COUNT(0) from MachineOderDetail where {0}<DeliveryQty 
and OdersNumber='{1}' and ProductNumber='{2}' and Version='{3}' and RowNumber='{4}'
", qty, ordersNumber, productnumber, version, rowNumber);
                    if (!SqlHelper.GetScalar(sqlCheck).Equals("0"))
                    {
                        Response.Write("数量不能小于已交货数量");
                        Response.End();
                        return;
                    }
                    string errorEdit = "";
                    List<string> sqlEdits = new List<string>();
                    string sqlEdit = string.Format(@"
update MachineOderDetail set 
 Qty={0},
NondeliveryQty={0}-DeliveryQty,
SumPrice=UnitPrice*{0}
where OdersNumber='{1}' and ProductNumber='{2}' and Version='{3}' and RowNumber='{4}'
", qty, ordersNumber, productnumber, version, rowNumber);
                    sqlEdits.Add(sqlEdit);
                    sqlEdits.AddRange(GetChangeQtySqlList()); //更新 
                    bool result = SqlHelper.BatchExecuteSql(sqlEdits, ref errorEdit);
                    Response.Write(result ? "1" : errorEdit);
                    Response.End();
                    return;

                }

                //ordersNumber: ordersNumber, ProductNumber: productnumber, Version: version, RowNumber: rownumber,qty:qty
                if (Request["isEditYJ"] != null && Request["isEditYJ"] != "")//编辑已交数量
                {
                    string ordersNumber = Request["ordersNumber"];
                    string productnumber = Request["productnumber"];
                    string version = Request["Version"];
                    string rowNumber = Request["RowNumber"];
                    string qty = Request["qty"];
                    string errorEdit = "";
                    List<string> sqlEdits = new List<string>();
                    string sqlEdit = string.Format(@"
update MachineOderDetail set DeliveryQty={0}
,NondeliveryQty=Qty-{0}
where OdersNumber='{1}' and ProductNumber='{2}' and Version='{3}' and RowNumber='{4}'
", qty, ordersNumber, productnumber, version, rowNumber);
                    sqlEdits.Add(sqlEdit);
                    sqlEdits.AddRange(GetChangeQtySqlList());
                    bool result = SqlHelper.BatchExecuteSql(sqlEdits, ref errorEdit);
                    Response.Write(result ? "1" : errorEdit);
                    Response.End();
                    return;

                }


                if (ToolManager.CheckQueryString("NewNumber"))
                {
                    string oldNumber = ToolManager.GetQueryString("OldNumber");
                    string newNumber = ToolManager.GetQueryString("NewNumber");
                    hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0103", "Edit");
                    hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0103", "Delete");
                    string result = SaleOderManager.ConvertNumber(oldNumber, newNumber);
                    if (result.Equals("1"))
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "加工临时销售订单" + oldNumber + "转成正式订单" + newNumber, "转换成功");

                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "加工临时销售订单" + oldNumber + "转成正式订单" + newNumber, "转换失败！原因:" + result);

                    }
                    Response.Write(result);
                    Response.End();
                    return;
                }

                string sql = string.Empty;
                spAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0103", "Add");
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0103", "Print");
                if (!ToolManager.CheckQueryString("id"))
                {
                    Response.Write("未知销售订单");
                    Response.End();
                    return;
                }
                string odersnumber = ToolManager.GetQueryString("Id");
                sql = string.Format(@"select c.MakeCollectionsModeId from SaleOder so inner join Customer c on so.CustomerId=c.CustomerId
inner join MakeCollectionsMode mcm on c.MakeCollectionsModeId=mcm.Id where so.OdersNumber='{0}'", odersnumber);
                string makecollectionsmode = SqlHelper.GetScalar(sql);
                if (makecollectionsmode.Equals("YSBF"))
                {
                    rp = "inline";
                }
                else
                {
                    rp = "none";
                }

                string error = string.Empty;
                if (ToolManager.CheckQueryString("isDelete"))
                {

                    string productnumber = ToolManager.GetQueryString("ProductNumber");
                    string version = ToolManager.GetQueryString("Version");
                    string rownumber = ToolManager.GetQueryString("RowNumber");
                    sql = string.Format("delete MachineOderDetail where OdersNumber='{0}' and  ProductNumber='{1}' and Version='{2}' and RowNumber='{3}'", odersnumber, productnumber, version, rownumber);
                    bool result = SqlHelper.ExecuteSql(sql, ref error);
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除加工销售订单明细" + odersnumber, "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除加工销售订单明细" + odersnumber, "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                sql = string.Format(@"select cu.CustomerName,cu.ContactTelephone,cu.Contacts,so.CustomerOrderNumber,cu.Fax,so.OrdersDate,so.OdersNumber,pu.USER_NAME from SaleOder so left join PM_USER pu on so.ContactId=pu.USER_ID
left join Customer cu on so.CustomerId=cu.CustomerId where so.OdersNumber='{0}'", odersnumber);
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
        /// <summary>
        /// 状态更新
        /// </summary>
        /// <returns></returns>
        private List<string> GetChangeQtySqlList()
        {
            List<string> sqlEdits = new List<string>();
            sqlEdits.Add("update MachineOderDetail set Status ='已完成' where NonDeliveryQty =0");
            sqlEdits.Add("update MachineOderDetail set Status ='未完成' where NonDeliveryQty !=0");
            sqlEdits.Add(@" update SaleOder set OrderStatus ='已完成' where OdersNumber in (
	                    select OdersNumber from TradingOrderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0
	                    union
                      select OdersNumber from MachineOderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0)");
            sqlEdits.Add(@" update SaleOder set OrderStatus ='未完成' where OdersNumber not in (
	                    select OdersNumber from TradingOrderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0
	                    union
                      select OdersNumber from MachineOderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0)");
            return sqlEdits;
        }


        private void Bind()
        {
            userId = ToolCode.Tool.GetUser().UserNumber;
            string sql = string.Empty;
            string error = string.Empty;
            string odersnumber = ToolManager.GetQueryString("id");
            sql = string.Format(@"
 select modt.OdersNumber,modt.ProductNumber,modt.Version,
modt.LeadTime,modt.RowNumber,cast(modt.SN  as varchar(10)) as SN,modt.CustomerProductNumber,
modt.Qty,modt.NonDeliveryQty,modt.DeliveryQty,cast(modt.UnitPrice as varchar(10)) as UnitPrice,
modt.SumPrice,modt.Remark,modt.CreateTime,modt.Status,
cast(modt.ReceiveOne as varchar(10)) as ReceiveOne,cast(modt.ReceiveTwo as varchar(10)) as ReceiveTwo,
p.Description,p.IsOldVersion, tpi.ProjectName,case when CAST( modt.LeadTime as  date)<= cast (so.CreateTime as date) then 'red' else 'black' end color ,
cast(ROW_NUMBER () over(order by CAST( modt.RowNumber as int  )asc)  as varchar(10)) as num   from MachineOderDetail modt inner join SaleOder so on modt.OdersNumber =so.OdersNumber 
 left join T_ProjectInfo tpi on modt.ProductNumber=tpi.ProductNumber and modt.Version=tpi.version
 left join Product p on modt.ProductNumber=p.ProductNumber and modt.Version=p.Version
where modt.OdersNumber='{0}' ", odersnumber);
            string tempsql = string.Format(@"
  union all 
 select '合计','','','','1000000','','',SUM (t.Qty ),SUM (t.NonDeliveryQty ),
SUM (DeliveryQty),'',SUM (SumPrice ),'','','','','','','','','','' from 
 ({0})t", sql);
            sql = string.Format(" select * from ({0})t order by CAST( t.RowNumber as int  )asc ", sql + " " + tempsql);

            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();
            sql = string.Format("select CheckTime  from SaleOder where OdersNumber='{0}'", odersnumber);
            show = SqlHelper.GetScalar(sql) == "" ? "inline" : "none";

            sql = string.Format(" select OdersType  from SaleOder where OdersNumber ='{0}' ", odersnumber);
            showBtn = SqlHelper.GetScalar(sql) == "临时订单" ? "inline" : "none";

            hdnumber.Value = ToolManager.GetQueryString("id");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnExp_Click(object sender, EventArgs e)
        {
            string odersnumber = ToolManager.GetQueryString("id");
            string sql = string.Format(@"
 select modt.OdersNumber as 销售订单号,modt.ProductNumber as 产成品编号,modt.Version as 版本
 ,modt.LeadTime as 交期,modt.RowNumber as 行号,modt.SN as 序号,modt.CustomerProductNumber as 客户产成品编号,modt.Qty as 数量
 ,modt.NonDeliveryQty as 未交数量,modt.DeliveryQty as 已交数量,modt.UnitPrice as 单价,modt.SumPrice as 总价,modt.Status as  未完成,
 p.Description as 描述,p.IsOldVersion as 是否是旧版本, tpi.ProjectName  as 项目名称     from MachineOderDetail modt inner join SaleOder so on modt.OdersNumber =so.OdersNumber 
 left join T_ProjectInfo tpi on modt.ProductNumber=tpi.ProductNumber and modt.Version=tpi.version
 left join Product p on modt.ProductNumber=p.ProductNumber and modt.Version=p.Version
where modt.OdersNumber='{0}' order by CAST( modt.RowNumber as int  )asc", odersnumber);
            ToolCode.Tool.ExpExcel(sql, "加工销售订单");
        }
    }
}
