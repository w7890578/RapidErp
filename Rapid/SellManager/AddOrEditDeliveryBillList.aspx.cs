using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class AddOrEditDeliveryBillList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindDrp("select distinct USER_ID,USER_NAME from PM_USER", this.drpDeliveryPerson, "USER_ID", "USER_NAME");
                if (ToolManager.CheckQueryString("id"))
                {
                    ControlBindManager.BindDrp(@"select CustomerId,CustomerName from Customer", this.drpCustomerName, "CustomerId", "CustomerName");
                    string deliveryNumber = ToolManager.GetQueryString("id");
                    string error = string.Empty;
                    string sql = string.Format("select * from DeliveryBill where DeliveryNumber='{0}'", deliveryNumber);
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    if (dt.Rows.Count <= 0)
                    {
                        Response.Write("未知送货单，该送货单不存在或已被删除！");
                        Response.End();
                        return;
                    }
                    DataRow dr = dt.Rows[0];
                    drpDeliveryPerson.SelectedValue = dr["DeliveryPerson"] == null ? "" : dr["DeliveryPerson"].ToString();
                    txtDeliveryDate.Text = dr["DeliveryDate"] == null ? "" : dr["DeliveryDate"].ToString();
                    drpIsConfirm.SelectedValue = dr["IsConfirm"] == null ? "" : dr["IsConfirm"].ToString();
                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    drpCustomerName.SelectedValue = dr["CustomerId"] == null ? "" : dr["CustomerId"].ToString();
                    txtTel.Text = dr["Tel"] == null ? "" : dr["Tel"].ToString();
                    txtContactsName.Text = dr["ContactsName"] == null ? "" : dr["ContactsName"].ToString();
                    drpIsConfirm.Enabled = true;
                    btnSubmit.Text = "修改";
                }
                else
                {
                    drpIsConfirm.Enabled = false;
                    btnSubmit.Text = "添加";
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string sql = string.Empty;
            string deliveryPerson =drpDeliveryPerson.SelectedValue;
            string deliveryDate = txtDeliveryDate.Text;
            string isConfirm = drpIsConfirm.SelectedValue;
            string remark = txtRemark.Text;
            string customerid=drpCustomerName.SelectedValue;
            string tel = this.txtTel.Text;
            string contactsname = this.txtContactsName.Text;
            if (btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@"insert into DeliveryBill (DeliveryNumber,DeliveryPerson,DeliveryDate,IsConfirm,CreateTime ,Remark,CustomerId,Tel,ContactsName)
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", "SH" + DateTime.Now.ToString("yyyyMMddHHmmss"),
     deliveryPerson, deliveryDate, isConfirm, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remark, customerid,tel,contactsname);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加送货单信息", "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加送货单信息", "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                string deliveryNumber = ToolManager.GetQueryString("id");
                sql = string.Format(@"update DeliveryBill set DeliveryPerson='{0}',DeliveryDate='{1}',IsConfirm='{2}',
Remark ='{3}',CustomerId='{4}',Tel='{6}',ContactsName='{7}' where DeliveryNumber='{5}'", deliveryPerson, deliveryDate, isConfirm, remark, customerid, deliveryNumber, tel,contactsname);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "修改送货单信息", "修改成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "修改送货单信息", "修改失败！原因：" + error);
                    return;
                }
            }
        }
    }
}
