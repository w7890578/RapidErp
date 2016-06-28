using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.SellManager
{
    public partial class DeliveryNoteDetailed_two : System.Web.UI.Page
    {
        public static string show = "inline";
        public static string deliverynumber = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                if (!ToolManager.CheckQueryString("id"))
                {
                    Response.Write("未知送货单");
                    Response.End();
                    return;
                }

                Bind();
            }
        }

        private string GetSql()
        {
            deliverynumber = ToolManager.GetQueryString("id");
            DataTable dt;
            string sql = string.Empty;
            sql = string.Format(@"select count(*) from DeliveryBill where ConfirmTime!='' and DeliveryNumber='{0}'", deliverynumber);
            //int num =Convert.ToInt32(SqlHelper.GetScalar(sql));
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                show = "none";
            }
            else
            {
                show = "inline";
            }
            sql = string.Format(@"select DeliveryDate,pu.USER_NAME,Tel,ContactsName from DeliveryBill db left join PM_USER pu on db.DeliveryPerson=pu.USER_ID where DeliveryNumber='{0}' ", ToolManager.GetQueryString("id"));
            dt = SqlHelper.GetTable(sql);
            lblDeliveryDate.Text = dt.Rows[0]["DeliveryDate"] == null ? "" : dt.Rows[0]["DeliveryDate"].ToString();
            lblDeliveryPerson.Text = dt.Rows[0]["USER_NAME"] == null ? "" : dt.Rows[0]["USER_NAME"].ToString();
            lblTel.Text = dt.Rows[0]["Tel"] == null ? "" : dt.Rows[0]["Tel"].ToString();
            lblContactsName.Text = dt.Rows[0]["ContactsName"] == null ? "" : dt.Rows[0]["ContactsName"].ToString();
            sql = string.Format(@"select Contacts,ContactTelephone,Fax,Email,FactoryAddress,CustomerName from Customer where CustomerId=(select CustomerId from DeliveryBill where DeliveryNumber='{0}')", ToolManager.GetQueryString("id"));
            dt = SqlHelper.GetTable(sql);
            lblContacts.Text = dt.Rows[0]["Contacts"] == null ? "" : dt.Rows[0]["Contacts"].ToString();
            lblContactTelephone.Text = dt.Rows[0]["ContactTelephone"] == null ? "" : dt.Rows[0]["ContactTelephone"].ToString();
            lblFax.Text = dt.Rows[0]["Fax"] == null ? "" : dt.Rows[0]["Fax"].ToString();
            lblEmail.Text = dt.Rows[0]["Email"] == null ? "" : dt.Rows[0]["Email"].ToString();
            lblCustomerName.Text = dt.Rows[0]["CustomerName"] == null ? "" : dt.Rows[0]["CustomerName"].ToString();
            lblFactoryAddress.Text = dt.Rows[0]["FactoryAddress"] == null ? "" : dt.Rows[0]["FactoryAddress"].ToString();


            sql = string.Format(@"select cast(ROW_NUMBER () over(order by 序号 asc) as varchar(10)) as 序号 ,送货单号,
产成品编号,客户产成品编号,版本,描述,行号,CAST(发货数量 as varchar(10)) as 发货数量,
CAST(实收数量 as varchar(10)) as 实收数量,销售订单号,客户订单号,
CAST(订单总量 as varchar(10)) as 订单总量,客户号,物料编号,备注,单位
 from [V_DeliveryNoteDetailed_two_New ] where 送货单号='{0}'

union all
select '合计','',
'','','','','',sum(发货数量) as 发货数量,
sum(实收数量) as 实收数量,'','',
sum(订单总量) as 订单总量,'','','',''
 from [V_DeliveryNoteDetailed_two_New ] where 送货单号='{0}'", deliverynumber);
            return sql;
        }

        private void Bind()
        {
            
            //if (dt.Rows.Count > 0)
            //{
            //    deliverynumber = dt.Rows[0]["送货单号"].ToString();
            //}
            //else
            //{
            //    return;
            //}
            string sql = GetSql();
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnE_Click(object sender, EventArgs e)
        {
            if (ToolManager.CheckQueryString("id"))
            {
                string error = string.Empty;
                string deliveryNumber = ToolManager.GetQueryString("id");
                string sql = string.Format(@" update DeliveryNoteDetailed set ArriveQty=DeliveryQty,ConformenceQty=DeliveryQty where  DeliveryNumber='{0}'", deliveryNumber);
                if (SqlHelper.ExecuteSql(sql, ref error))
                {
                    Bind();
                }
                else
                {
                    lblResult.Text = "数量填充失败！" + error;
                    return;
                }
            }

        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string sql = GetSql();
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "收货明细表");
        }
    }
}
