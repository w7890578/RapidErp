using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using BLL;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class AddDeliveryBillDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("DeliveryNumber"))
                {
                    Response.Write("该送货单不存在！");
                    Response.End();
                    return;
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string deliverynumber = ToolManager.GetQueryString("DeliveryNumber");
            string ordersnumber = txtOrdersNumber.Text.Trim();
            string rownumber = txtRowNumber.Text.Trim();
            string sn = txtSN.Text.Trim();
            string productnumber = string.Empty;
            string version = string.Empty;
            string customerproductnumber = string.Empty;
            string leadtime = string.Empty;
            string materialdescription= string.Empty;
            string deliveryqty = string.Empty;
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"select ProductNumber,Version,LeadTime,CustomerProductNumber,NonDeliveryQty from MachineOderDetail
where OdersNumber='{0}' and RowNumber='{1}'", ordersnumber, rownumber);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count > 0)
            {
                productnumber = dt.Rows[0]["ProductNumber"].ToString();
                version = dt.Rows[0]["Version"].ToString();
                leadtime = dt.Rows[0]["LeadTime"].ToString();
                customerproductnumber = dt.Rows[0]["CustomerProductNumber"].ToString();
                deliveryqty = dt.Rows[0]["NonDeliveryQty"].ToString();
                sql = string.Format(@"select Description from Product where ProductNumber='{0}' and Version='{1}'", productnumber, version);
                materialdescription = SqlHelper.GetScalar(sql);
            }
            else
            {
                lbSubmit.Text = "未和采购订单号及行号！";
                return;
            }
            sql = string.Format(@"select count(*) from  DeliveryNoteDetailed where OrdersNumber='{0}' and RowNumber='{1}'", ordersnumber, rownumber);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = "此记录已存在！";
                return;
            }
            sql = string.Format(@"insert into DeliveryNoteDetailed(DeliveryNumber,OrdersNumber,ProductNumber,Version,CustomerProductNumber,RowNumber,LeadTime,SN,MaterialDescription,DeliveryQty)
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9})", deliverynumber, ordersnumber, productnumber, version, customerproductnumber, rownumber, leadtime, sn, materialdescription, deliveryqty);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                lbSubmit.Text = "添加成功！";
                return;
            }
            else
            {
                lbSubmit.Text = "添加失败！原因："+error;
                return;
            }
        }
    }
}
