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
    public partial class AddOrEditSaleOder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                drpOdersType.Items.Add("正常订单");
                drpOdersType.Items.Add("加急订单");
                drpOdersType.Items.Add("维修订单");
                drpOdersType.Items.Add("临时订单");
                drpOdersType.Items.Add("样品订单");
                drpOdersType.Items.Add("MRP订单");
                drpOdersType.Items.Add("包装生产订单");
                ControlBindManager.BindDrp("select CustomerId,CustomerName from Customer", drpClient, "CustomerId", "CustomerName");
                //用户名称
                ControlBindManager.BindDrp("select user_id, USER_NAME from PM_USER", this.drpContact, "user_id", "USER_NAME");
                ControlBindManager.BindDrp("select id,makecollectionsmode from MakeCollectionsMode", this.drpMakeCollectionsMode, "id", "makecollectionsmode");
                if (ToolManager.CheckQueryString("id"))
                {
                    string odersNumber = ToolManager.GetQueryString("id");
                    string error = string.Empty;
                    string sql = string.Format(" select * from SaleOder where OdersNumber='{0}'", odersNumber);
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    if (dt.Rows.Count <= 0)
                    {
                        Response.Write("未知订单，该订单不存在或已被删除！");
                        Response.End();
                        return;
                    }
                    DataRow dr = dt.Rows[0];
                    txtOrdersDate.Text = dr["OrdersDate"] == null ? "" : dr["OrdersDate"].ToString();
                    drpOdersType.SelectedValue = dr["OdersType"] == null ? "" : dr["OdersType"].ToString();
                    drpProductType.SelectedValue = dr["ProductType"] == null ? "" : dr["ProductType"].ToString();
                    drpIsMinusStock.SelectedValue = dr["IsMinusStock"] == null ? "" : dr["IsMinusStock"].ToString();
                    this.drpMakeCollectionsMode.SelectedValue = dr["MakeCollectionsMode"] == null ? "" : dr["MakeCollectionsMode"].ToString();
                    drpClient.SelectedValue = dr["CustomerId"] == null ? "" : dr["CustomerId"].ToString();
                    drpContact.SelectedValue = dr["ContactId"] == null ? "" : dr["ContactId"].ToString();
                    drpStatus.SelectedValue = dr["OrderStatus"] == null ? "" : dr["OrderStatus"].ToString();
                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    txtCustomerOrderNumber.Text = dr["CustomerOrderNumber"] == null ? "" : dr["CustomerOrderNumber"].ToString();
                    tdStatus.Visible = true;
                    btnSubmit.Text = "修改";

                    //txtOrderNumber.Text = odersNumber;
                    //txtOrderNumber.ReadOnly = true;
                }
                else
                {
                    tdStatus.Visible = false;
                    //txtOrderNumber.ReadOnly = false;
                    btnSubmit.Text = "添加";
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string sql = string.Empty;
            string ordersDate = txtOrdersDate.Text.Trim();
            string odersType = drpOdersType.SelectedValue;
            string productType = drpProductType.SelectedValue;
            string isMinusStock = drpIsMinusStock.SelectedValue;
            string makeCollectionsMode = this.drpMakeCollectionsMode.SelectedValue;
            string customerId = drpClient.SelectedValue;
            string contactId = drpContact.SelectedValue;
            string status = drpStatus.SelectedValue;
            string remark = txtRemark.Text.Trim();
            string customerordernumber = txtCustomerOrderNumber.Text;
            string khddH = txtKhddH.Text;

            if (btnSubmit.Text.Equals("添加"))
            {
                string odersnumber = string.Empty;
                //if (odersType.Equals("临时订单"))
                //{
                //    odersnumber = "XS" + DateTime.Now.ToString("yyyyMMddHHmmss");
                //}
                //else
                //{
                //    if (string.IsNullOrEmpty(odersnumber))
                //    {
                //        lbSubmit.Text = "请输入销售订单号";
                //        return;
                //    }
                //}
                odersnumber = "XS" + DateTime.Now.ToString("yyyyMMddHHmmss");
                //if (CheckIsExit(odersnumber))
                //{
                //    lbSubmit.Text = "已存在该销售订单！请重新填写";
                //    return;
                //}


                sql = string.Format(@"insert into SaleOder (OdersNumber ,OrdersDate,OdersType,ProductType
,IsMinusStock,MakeCollectionsMode,CustomerId,ContactId ,OrderStatus,CreateTime ,Remark,CustomerOrderNumber ,KhddH)
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','未完成','{8}','{9}','{10}','{11}')", odersnumber,
     ordersDate, odersType, productType, isMinusStock, makeCollectionsMode, customerId, contactId
     , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remark, customerordernumber, khddH);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加销售订单" + odersnumber, "增加成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加销售订单" + odersnumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                string odersNumber = ToolManager.GetQueryString("id");
                sql = string.Format(@"update SaleOder set OrdersDate='{0}',OdersType='{1}',ProductType='{2}',IsMinusStock='{3}'
,MakeCollectionsMode='{4}',CustomerId='{5}',ContactId ='{6}',OrderStatus='{7}' 
,Remark ='{8}',CustomerOrderNumber='{10}',KhddH='{11}' where OdersNumber ='{9}'", ordersDate, odersType, productType, isMinusStock, makeCollectionsMode
, customerId, contactId, status, remark, odersNumber, customerordernumber, khddH);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑销售订单" + odersNumber, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑销售订单" + odersNumber, "编辑失败！原因" + error);
                    return;
                }
            }
        }

        protected void drpProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            drpOdersType.Items.Clear();
            if (drpProductType.SelectedValue == "加工")
            {
                drpOdersType.Items.Add("正常订单");
                drpOdersType.Items.Add("加急订单");
                drpOdersType.Items.Add("维修订单");
                drpOdersType.Items.Add("临时订单");
                drpOdersType.Items.Add("样品订单");
                drpOdersType.Items.Add("MRP订单");
                drpOdersType.Items.Add("包装生产订单"); 
            }
            if (drpProductType.SelectedValue == "贸易")
            {
                drpOdersType.Items.Add("正常订单");
                drpOdersType.Items.Add("加急订单");
                drpOdersType.Items.Add("维修订单");
                drpOdersType.Items.Add("样品订单");
            }
        }

        protected void drpOdersType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string value = drpOdersType.SelectedValue;
            //if (value.Equals("临时订单"))
            //{
            //    trOrderNumber.Visible = false;
            //}
            //else
            //{
            //    trOrderNumber.Visible = true;
            //}
        }

        private bool CheckIsExit(string orderNumber)
        {
            string sql = string.Format("  select COUNT(*) from SaleOder where OdersNumber='{0}'", orderNumber);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }

        protected void drpClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Empty;
            sql = string.Format(@" select MakeCollectionsModeId from customer where CustomerId='{0}'", this.drpClient.SelectedValue);
            this.drpMakeCollectionsMode.SelectedValue = SqlHelper.GetScalar(sql);
        }
    }
}
