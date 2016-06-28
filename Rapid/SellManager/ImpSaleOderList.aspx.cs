using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;
using System.Data;
using Model;

namespace Rapid.SellManager
{
    public partial class ImpSaleOderList : System.Web.UI.Page
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
            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string odersnumber = "XS" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string error = string.Empty;
            string sql = string.Empty;
            string ordersDate = txtOrdersDate.Text.Trim();
            string odersType = drpOdersType.SelectedValue;
            string productType = drpProductType.SelectedValue;
            string isMinusStock = drpIsMinusStock.SelectedValue;
            string makeCollectionsMode = this.drpMakeCollectionsMode.SelectedValue;
            string customerId = drpClient.SelectedValue;
            string contactId = drpContact.SelectedValue;
           
            string remark = txtRemark.Text.Trim();
            string customerordernumber = txtCustomerOrderNumber.Text;
            string khddH = txtKhddH.Text;
           // List<string> sqls = new List<string>();
       
            string brand = string.Empty;
            string description = string.Empty;
            string unitPrice = string.Empty;
            string quantity = string.Empty;
        
           
            string name = string.Empty;
            SaleOder so = new SaleOder();
            so.OrdersNumber = odersnumber;
            so.OrdersDate = ordersDate;
            so.OdersType = odersType;
            so.ProductType = productType;
            so.MakeCollectionsMode = makeCollectionsMode;
            so.CustomerId = customerId;
            so.ContactId = contactId;
            so.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            so.CustomerOrderNumber = customerordernumber;
            so.KhddH = khddH;
            so.Remark = remark;
            so.CustomerName = drpClient.SelectedItem.Text.Trim();
            string userId = ToolCode.Tool.GetUser().UserNumber;
            bool result = SaleOderManager.BacthAddSaleOrderNew(so, FU_Excel, Server,userId , ref error);
            if (result == true)
            {
                lbMsg.Text = "导入成功";
                Tool.WriteLog(Tool.LogType.Operating, "导入销售订单", "导入成功！");
                return;
            }
            else
            {
                lbMsg.Text = error;
                Tool.WriteLog(Tool.LogType.Operating, "导入销售订单", "导入失败！原因" + lbSubmit.Text);
                return;
            }


            //            sql = string.Format(@"insert into SaleOder (OdersNumber ,OrdersDate,OdersType,ProductType
            //,IsMinusStock,MakeCollectionsMode,CustomerId,ContactId ,OrderStatus,CreateTime ,Remark,CustomerOrderNumber ,KhddH)
            //values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','未完成','{8}','{9}','{10}','{11}')", odersnumber,
            // ordersDate, odersType, productType, isMinusStock, makeCollectionsMode, customerId, contactId
            // , DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remark, customerordernumber, khddH);
            //            sqls.Add(sql);

            //DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
            //if (ds == null)
            //{
            //    lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
            //    return;
            //}

            //            foreach (DataRow dr in ds.Tables[0].Rows)
            //            {
            //                quantity =dr["订单数量"].ToString();
            //                sql = string.Format(@"select count(*) from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", dr["客户物料编号"]);
            //                string num = SqlHelper.GetScalar(sql);
            //                if (num == "0")
            //                {
            //                    lbSubmit.Text += string.Format("贸易销售订单{0}添加失败！原因：该原材料不存在客户物料编号<br/>", dr["客户物料编号"]);
            //                    i++;
            //                    continue;
            //                }
            //                sql = string.Format(@"   select MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}' and CustomerId=(select CustomerId from SaleOder where OdersNumber='{1}')
            //", dr["客户物料编号"], odersnumber);
            //               string  productNumber = SqlHelper.GetScalar(sql);
            //                if (productNumber.Equals(""))
            //                {
            //                    lbSubmit.Text += string.Format("贸易销售订单{0}添加失败！原因：该客户原材料编号不存在<br/>", dr["客户物料编号"]);
            //                    i++;
            //                    continue;

            //                }
            //                sql = string.Format(@"select Description,Brand,ProcurementPrice,MaterialName from MarerialInfoTable where MaterialNumber='{0}'", productNumber);
            //                dt = SqlHelper.GetTable(sql, ref error);
            //                if (dt.Rows.Count > 0)
            //                {
            //                    brand = dt.Rows[0]["Brand"].ToString();
            //                    description = dt.Rows[0]["Description"].ToString();
            //                    unitPrice = dt.Rows[0]["ProcurementPrice"].ToString();
            //                    name=dt.Rows[0]["MaterialName"].ToString();
            //                    qty = string.IsNullOrEmpty(quantity) ? 0 : Convert.ToInt32(quantity);
            //                    uprice = string.IsNullOrEmpty(unitPrice) ? 0 : Convert.ToDecimal(unitPrice);
            //                    totalPrice = qty * uprice;

            //                }

            //                sql = string.Format(@"
            //select * from TradingOrderDetail where OdersNumber ='{0}' and ProductNumber='{1}' and RowNumber='{2}'", odersnumber, productNumber, dr["行号"]);
            //                dt = SqlHelper.GetTable(sql, ref error);
            //                if (dt.Rows.Count > 0)
            //                {
            //                    lbSubmit.Text += string.Format("贸易销售订单{0}添加失败！原因：此记录已存在<br/>", odersnumber);
            //                    i++;
            //                    continue;
            //                }


            //                sql = string.Format(@" insert into TradingOrderDetail (OdersNumber,SN,ProductNumber,RowNumber ,CustomerMaterialNumber,MaterialName
            //,Brand,Quantity,NonDeliveryQty,DeliveryQty,UnitPrice,TotalPrice,Delivery,CreateTime,Remark,Status,ProductModel)
            //values('{0}',{1},'{2}','{3}','{4}','{5}','{6}',{7},{8},{9},{10},{11},'{12}','{13}','{14}','{15}','{16}')"
            //  , odersnumber, dr["序号"], productNumber, dr["行号"], dr["客户物料编号"], description, brand, qty, qty, 0
            //  , uprice, totalPrice, dr["交期"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remark,"未完成",name);
            //                sqls.Add(sql);
            //            }
            //            bool result = false;
            //            if (i == 0)
            //            {
            //                result = true;
            //            }

            //            if (result == true)
            //            {
            //                lbMsg.Text = "导入成功！";
            //            }
            //            else
            //            {
            //                lbMsg.Text = "导入失败！<br/>" + lbSubmit.Text;
            //            }
            //if (result == true)
            //{
            //    Tool.WriteLog(Tool.LogType.Operating, "导入贸易销售订单", "导入成功！");
            //    return;
            //}
            //else
            //{
            //    Tool.WriteLog(Tool.LogType.Operating, "导入贸易销售订单", "导入失败！原因" + lbSubmit.Text);
            //    return;
            //}

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

        protected void drpClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = string.Empty;
            sql = string.Format(@" select MakeCollectionsModeId from customer where CustomerId='{0}'", this.drpClient.SelectedValue);
            this.drpMakeCollectionsMode.SelectedValue = SqlHelper.GetScalar(sql);
        }
    }
}
