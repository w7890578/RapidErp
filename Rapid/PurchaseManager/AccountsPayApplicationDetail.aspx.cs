using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.PurchaseManager
{
    public partial class AccountsPayApplicationDetail : System.Web.UI.Page
    {
        public string show = "inline";

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("SQ"))
                {
                    show = "none";
                }
                if (ToolManager.CheckQueryString("SP"))
                {
                    show = "none";
                }

                //Guid, InvoiceNumber, BillingDate, Remark
                if (Request["Guid"] != null && Request["Guid"] != "")
                {
                    string sql = string.Format(@"
update
    T_AccountsPayable_Detail
set
    InvoiceNumber='{0}',
    BillingDate='{1}',
    Remark='{2}'
where
    Guid='{3}'", Request["InvoiceNumber"], Request["BillingDate"], Request["Remark"], Request["Guid"]);
                    string error = string.Empty;
                    Response.Write(SqlHelper.ExecuteSql(sql, ref error) ? "ok" : error);
                    Response.End();
                    return;
                }
                if (!ToolManager.CheckQueryString("OrdersNumber"))
                {
                    Response.Write("未知的采购订单号！");
                    Response.End();
                    return;
                }
                if (ToolManager.CheckQueryString("js"))//审批通过
                {
                    string ordersnumber = ToolManager.GetQueryString("OrdersNumber");
                    string createtime = ToolManager.GetQueryString("CreateTime");
                    List<string> sqls = new List<string>();
                    string error = string.Empty;
                    string guids = ToolManager.GetQueryString("js");
                    string sql = string.Format(@"
update T_AccountsPayable_Detail set PaymentAmount=SumPrice,PaymentDate='{0}',IsSettlement='是' where guid in ({1}) ", DateTime.Now.ToString("yyyy-MM-dd"), guids);
                    sqls.Add(sql);
                    sql = string.Format(@" update T_AccountsPayable_Main set ActualPaymentsAmount=(
select SUM(PaymentAmount) from T_AccountsPayable_Detail where PurchaseOrderNumber='{0}' and CreateTime='{1}' and IsSettlement='是')
where OrdersNumber='{0}' and CreateTime ='{1}'", ordersnumber, createtime);
                    sqls.Add(sql);
                    string result = SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
                    Response.Write(result);
                    Response.End();
                    return;
                }
                Bind();
            }
        }

        private void Bind()
        {
            string ordersnumber = ToolManager.GetQueryString("OrdersNumber");
            string createtime = ToolManager.GetQueryString("CreateTime");
            hdOrdersNumber.Value = ordersnumber;
            hdCreateTime.Value = createtime;
            string condition = " where 采购订单号='" + ordersnumber + "' and 创建时间='" + createtime + "'";
            if (txtHDnumber.Text != "")
            {
                condition += " and 采购合同号 like '%" + txtHDnumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@" select * from [V_AccountsPayDetail] {0} union all
select '合计','','','','','',SUM(采购数量),SUM(到货数量),'',SUM(总价),'','','','','','','','','','' from V_AccountsPayDetail {0}", condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }
    }
}