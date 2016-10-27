using BLL;
using DAL;
using Rapid.ToolCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.PurchaseManager
{
    public partial class CertificateOrdersListDetail : System.Web.UI.Page
    {
        public static string checkStatus = string.Empty;
        public static string hasDelete = "none";
        public static string hasEdit = "none";
        public static string number = string.Empty;
        public static string showPay = "none";
        public static string userId = "";

        public string CCTCOrdersNumber = string.Empty;

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0203", "Add");
                //spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0203", "Print");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0203", "Edit");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0203", "Delete");

                //超级管理员操作
                if (Request["isEditQty"] != null && Request["isEditQty"] != "")//编辑数量
                {
                    string guid = Request["guid"];
                    string qty = Request["qty"];
                    //检查数量是否小于已交数量
                    string sqlCheck = string.Format(@"
 select  COUNT(0) from CertificateOrdersDetail where {0}<DeliveryQty and Guid='{1}'
", qty, guid);
                    if (!SqlHelper.GetScalar(sqlCheck).Equals("0"))
                    {
                        Response.Write("数量不能小于已交货数量");
                        Response.End();
                        return;
                    }
                    string errorEdit = "";
                    List<string> sqlEdits = new List<string>();
                    string sqlEdit = string.Format(@"
update CertificateOrdersDetail
 set
 OrderQty={0},
 NonDeliveryQty={0}-DeliveryQty,
 SumPrice=UnitPrice*{0},
 SumPrice_C=UnitPrice_C*{0},
 Status=case when ({0}-DeliveryQty)=0 then '已完成' else '未完成' end
 where Guid='{1}'
", qty, guid);
                    sqlEdits.Add(sqlEdit);

                    sqlEdits.Add(@"
 update CertificateOrders set OrderStatus='已完成'
 where OrdersNumber in (
  select OrdersNumber from CertificateOrdersDetail group by OrdersNumber having(SUM (NonDeliveryQty ))=0)");
                    sqlEdits.Add(@"
 update CertificateOrders set OrderStatus='未完成'
 where OrdersNumber not  in (
  select OrdersNumber from CertificateOrdersDetail group by OrdersNumber having(SUM (NonDeliveryQty ))=0)");
                    bool result = SqlHelper.BatchExecuteSql(sqlEdits, ref errorEdit);
                    Response.Write(result ? "1" : errorEdit);
                    Response.End();
                    return;
                }

                if (!ToolManager.CheckQueryString("OrdersNumber"))
                {
                    Response.Write("未知采购单！");
                    Response.End();
                    return;
                }

                number = ToolManager.GetQueryString("OrdersNumber");
                string sql = string.Format(@"
select co.PaymentMode,si.SupplierName,si.Email,si.FactoryAddress,si.ContactTelephone,
si.Fax,co.OrdersDate,co.OrdersNumber,pu.USER_NAME,co.HTNumber,pm.PaymentMode as PayName
,co.CCTCOrdersNumber from
CertificateOrders co left join PM_USER pu on co.ContactId=pu.USER_ID
left join SupplierInfo si on si.SupplierId=co.SupplierId
inner join PaymentMode pm on co.PaymentMode =pm.Id
 where co.OrdersNumber='{0}'", number);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    lblContactName.Text = dr["USER_NAME"] == null ? "" : dr["USER_NAME"].ToString();
                    lblSupplieId.Text = dr["SupplierName"] == null ? "" : dr["SupplierName"].ToString();
                    lblSupplierName.Text = dr["SupplierName"] == null ? "" : dr["SupplierName"].ToString();
                    lblOrdersDate.Text = dr["OrdersDate"] == null ? "" : dr["OrdersDate"].ToString();
                    lblOrdersNumber.Text = dr["HTNumber"] == null ? "" : dr["HTNumber"].ToString();
                    lblEmail.Text = dr["Email"] == null ? "" : dr["Email"].ToString();
                    lblFactoryAddress.Text = dr["FactoryAddress"] == null ? "" : dr["FactoryAddress"].ToString();
                    lblContactTelephone.Text = dr["ContactTelephone"] == null ? "" : dr["ContactTelephone"].ToString();
                    lbPayMode.Text = dr["PayName"] == null ? "" : dr["PayName"].ToString();
                    showPay = dr["PaymentMode"] == null ? "" : dr["PaymentMode"].ToString();
                    CCTCOrdersNumber = dr["CCTCOrdersNumber"] == null ? "" : dr["CCTCOrdersNumber"].ToString();
                    if (showPay.Equals("YFBF"))
                    {
                        showPay = "inline";
                    }
                    else
                    {
                        showPay = "none";
                    }
                }
                Bind();
            }
        }

        private void Bind()
        {
            userId = ToolCode.Tool.GetUser().UserNumber;
            string sql = string.Empty;
            string error = string.Empty;
            string ordersNumber = ToolManager.GetQueryString("OrdersNumber");
            if (ToolManager.CheckQueryString("guid"))
            {
                sql = string.Format("delete CertificateOrdersDetail where Guid='{0}'", ToolManager.GetQueryString("guid"));
                if (SqlHelper.ExecuteSql(sql, ref error))
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除采购单明细" + ordersNumber, "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除采购单明细" + ordersNumber, "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }
            sql = string.Format(@"select cast(ROW_NUMBER ()over(order by cod.RowNumber asc) as varchar(10)) as num,cod.OrdersNumber,cod.MaterialNumber,cod.LeadTime,
cod.SupplierMaterialNumber,cod.OrderQty,cod.NonDeliveryQty,
cod.DeliveryQty,cast(cod.UnitPrice as varchar(10)) as UnitPrice,cod.SumPrice ,cod.Status,
cod.Remark,cod.CreateTime,cast(cod.UnitPrice_C as varchar(10)) as UnitPrice_C,cod.SumPrice_C,
cod.MinOrderQty,cast(cod.PayOne as varchar(10)) as PayOne,cast(cod.PayTwo as varchar(10)) as PayTwo,cod.status Status_New,
mi.MaterialName ,cod.Guid
from CertificateOrdersDetail cod inner join MarerialInfoTable mi on cod.MaterialNumber =mi.MaterialNumber
 where cod.OrdersNumber='{0}'
 union
 select '合计','','',
'','',sum(cod.OrderQty),sum(cod.NonDeliveryQty),
sum(cod.DeliveryQty),'',sum(cod.SumPrice),'',
'','','',sum(cod.SumPrice_C),
'','','','',
'',''
from CertificateOrdersDetail cod inner join MarerialInfoTable mi on cod.MaterialNumber =mi.MaterialNumber  where cod.OrdersNumber='{0}'
", ordersNumber);
            //            sql = string.Format(@" select * , Status   Status_New
            // from CertificateOrdersDetail where OrdersNumber='{0}' order by RowNumber asc", ordersNumber);
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();
            checkStatus = BLL.PurchaseManager.GetCheckStatus(ordersNumber) ? "none" : "inline";
            hdnumber.Value = ordersNumber;
        }
    }
}