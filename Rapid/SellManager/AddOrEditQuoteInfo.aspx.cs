using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class AddOrEditQuoteInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindCustomer(drpCustomer);
               // ControlBindManager.BindDrp(" select  distinct (USER_NAME) ,User_Id from PM_USER ", this.drpCustomerContact, "User_Id", "USER_NAME");
                if (ToolManager.CheckQueryString("id"))
                {
                    string error = string.Empty;
                    string sql = string.Format(" select * from  QuoteInfo where QuoteNumber='{0}'", ToolManager.GetQueryString("id"));
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    DataRow dr = dt.Rows[0];
                    txtQuoteTime.Text = dr["QuoteTime"] == null ? "" : dr["QuoteTime"].ToString();
                    drpType.SelectedValue = dr["QuoteType"] == null ? "" : dr["QuoteType"].ToString();

                    drpCustomer.SelectedValue = dr["CustomerId"] == null ? "" : dr["CustomerId"].ToString();
                    //用户名
                    //ControlBindManager.BindDrp(" select  distinct (USER_NAME) ,User_Id from PM_USER ", this.drpCustomerContact, "User_Id", "USER_NAME");
                    txtCustomerContact.Text = dr["ContactId"] == null ? "" : dr["ContactId"].ToString();
                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    btnSubmit.Text = "修改";
                }
            }

        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            string error = string.Empty;
            string quoteTime = txtQuoteTime.Text.Trim();
            string type = drpType.SelectedValue;
            string customer = drpCustomer.SelectedValue;
            string customerContact = txtCustomerContact.Text;
            string remark = txtRemark.Text;
            string userName = ToolCode.Tool.GetUser().UserName;
            if (btnSubmit.Text.Equals("添加"))
            {
                string number = "BJ" + DateTime.Now.ToString("yyyyMMddHHmmss");
                sql = string.Format(@"insert into QuoteInfo (QuoteNumber ,QuoteTime,QuoteType,CustomerId,ContactId,CreateDateTime,Remark,QuoteUser )
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", number, quoteTime, type, customer
, customerContact, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remark,userName );
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加报价单" + number, "增加成功");
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加报价单" + number, "增加失败！原因：" + error);
                }
                return;
            }
            else
            {
                sql = string.Format(@"update QuoteInfo
set QuoteTime ='{0}',QuoteType ='{1}',CustomerId='{2}',ContactId='{3}',Remark ='{4}'
where QuoteNumber='{5}'", quoteTime, type, customer, customerContact, remark, ToolManager.GetQueryString("id"));
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑报价单" + ToolManager.GetQueryString("id"), "编辑成功");
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑报价单" + ToolManager.GetQueryString("id"), "编辑失败！原因：" + error);
                }
                return;
            }
        }
    }
}
