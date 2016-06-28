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
    public partial class AddOrEditTradingOrderDetail : System.Web.UI.Page
    {
        public static string brand = string.Empty;//品牌
        public static string description = string.Empty;//原材料描述
        public static string show = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Check();

                if (ToolManager.CheckQueryString("ProductNumber"))
                {
                    DataTable dt = HasDeleted();
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        txtSN.Text = dr["SN"] == null ? "" : dr["SN"].ToString();
                        txtMarerial.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                        //txtProductModel.Text = dr["ProductModel"] == null ? "" : dr["ProductModel"].ToString();
                        txtRowNumber.Text = dr["RowNumber"] == null ? "" : dr["RowNumber"].ToString();
                        txtQuantity.Text = dr["Quantity"] == null ? "" : dr["Quantity"].ToString();
                        txtUnitPrice.Text = dr["UnitPrice"] == null ? "" : dr["UnitPrice"].ToString();
                        txtDelivery.Text = dr["Delivery"] == null ? "" : dr["Delivery"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        this.txtUniversalNumber.Text = dr["CustomerMaterialNumber"] == null ? "" : dr["CustomerMaterialNumber"].ToString();
                        //BindCustomer(txtMarerial.Text, dr["CustomerMaterialNumber"] == null ? "" : dr["CustomerMaterialNumber"].ToString());
                        txtReceiveOne.Text = dr["ReceiveOne"] == null ? "" : dr["ReceiveOne"].ToString();
                        txtReceiveTwo.Text = dr["ReceiveTwo"] == null ? "" : dr["ReceiveTwo"].ToString();

                    }
                    btnSubmit.Text = "修改";
                    txtMarerial.Enabled = false;
                    txtRowNumber.Enabled = false;
                    show = "";

                }
                else
                {
                    btnSubmit.Text = "添加";
                    txtUnitPrice.Visible = false;
                    show = "none";
                }
            }
            if (ToolManager.CheckQueryString("MaterialNumber"))
            {
                string materialNumber = ToolManager.GetQueryString("MaterialNumber");
                string odersNumber = ToolManager.GetQueryString("OdersNumber");
                string sql = string.Format(@" select CustomerMaterialNumber from MaterialCustomerProperty where MaterialNumber='{0}' and CustomerId=(
select CustomerId from SaleOder where OdersNumber='{1}')", materialNumber, odersNumber);
                Response.Write(SqlHelper.GetScalar(sql));
                Response.End();
                return;
            }
        }



        ///// <summary>
        ///// 绑定客户编号
        ///// </summary>
        ///// <param name="materialNumber"></param>
        ///// <param name="customerMaterialNumber"></param>
        //private void BindCustomer(string materialNumber, string CustomerMaterialNumber)
        //{
        //    this.txtCustomerMarerial.Text = CustomerMaterialNumber;
        //}
        /// <summary>
        /// 检查销售订单号是否传过来
        /// </summary>
        private void Check()
        {
            if (!ToolManager.CheckQueryString("OdersNumber"))
            {
                Response.Write("未知销售订单！");
                Response.End();
                return;
            }
            lblOdersNumber.Text = ToolManager.GetQueryString("OdersNumber");
            string sql = string.Format(@"select OdersType from SaleOder where OdersNumber='{0}'", ToolManager.GetQueryString("OdersNumber"));
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows[0]["OdersType"].ToString() == "样品订单")
            {
                txtUnitPrice.Text = "0";
                // txtUnitPrice.ReadOnly = true;
            }
            sql = string.Format(@"select c.MakeCollectionsModeId from SaleOder so inner join Customer c on so.CustomerId=c.CustomerId
inner join MakeCollectionsMode mcm on c.MakeCollectionsModeId=mcm.Id where so.OdersNumber='{0}'", ToolManager.GetQueryString("OdersNumber"));
            string makecollectionsmode = SqlHelper.GetScalar(sql);
            if (!makecollectionsmode.Equals("YSBF"))
            {
                trReceiveTwo.Visible = false;
                trReceiveOne.Visible = false;
            }

        }
        /// <summary>
        /// 检查该条记录是否被删除
        /// </summary>
        /// <returns></returns>
        private DataTable HasDeleted()
        {
            string ProductNumber = ToolManager.GetQueryString("ProductNumber");
            string rownumber = ToolManager.GetQueryString("RowNumber");
            string error = string.Empty;
            string sql = string.Format(@"
select * from TradingOrderDetail where OdersNumber ='{0}' and ProductNumber='{1}' and RowNumber='{2}'", ToolManager.GetQueryString("OdersNumber"), ProductNumber, rownumber);
            DataTable dt = SqlHelper.GetTable(sql, ref error);

            if (dt.Rows.Count <= 0)
            {
                Response.Write("异常：该条记录已被删除！");
                Response.End();
                return null;
            }
            else
            {
                return dt;
            }
        }

        /// <summary>
        /// 检查该条销售订单是否被删除
        /// </summary>
        /// <returns></returns>
        private void HasOdersNumber()
        {
            string OdersNumber = ToolManager.GetQueryString("OdersNumber");
            string error = string.Empty;
            string sql = string.Format("select COUNT(*) from SaleOder where OdersNumber='{0}'", OdersNumber);
            if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                Response.Write("该条销售订单不存在！");
                Response.End();
                return;
            }

        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Check();
            HasOdersNumber();
            string error = string.Empty;
            string sql = string.Empty;
            DataTable dt;
            string OdersNumber = ToolManager.GetQueryString("OdersNumber");
            string sn = txtSN.Text.Trim();
            string productNumber = string.Empty;
            //string customerMareial = txtCustomerMarerial.SelectedValue; 
            string customerMareial = txtUniversalNumber.Text;
            //string productModel = txtProductModel.Text.Trim();
            string rowNumber = txtRowNumber.Text.Trim();
            string quantity = Request.Form["txtQuantity"].ToString();
            int qty = string.IsNullOrEmpty(quantity) ? 0 : Convert.ToInt32(quantity);
            string unitPrice = txtUnitPrice.Text;
            decimal uprice = string.IsNullOrEmpty(unitPrice) ? 0 : Convert.ToDecimal(unitPrice);
            string delivery = txtDelivery.Text.Trim();
            string remark = txtRemark.Text.Trim();
            string receiveone = txtReceiveOne.Text.Trim().Equals("") ? "0" : txtReceiveOne.Text.Trim();
            string receivetwo = txtReceiveTwo.Text.Trim().Equals("") ? "0" : txtReceiveTwo.Text.Trim();
            bool result = false;
            decimal totalPrice = 0;
            try
            {
                Convert.ToDouble(receiveone);
                Convert.ToDouble(receivetwo);
            }
            catch (Exception ex)
            {
                lbSubmit.Text = "收款一和收款二都必须输入整数或小数！";
                return;
            }


            if (btnSubmit.Text.Equals("添加"))
            {
                if (HasDbRowNumber(OdersNumber, rowNumber))
                {
                    lbSubmit.Text = "已有此行号请重新填写,行号在整个订单内不能重复";
                    //BindCustomer(productNumber, customerMareial);
                    return;
                }
                if (drpType.SelectedValue == "供应商物料编号")
                {
                    sql = string.Format(@"select top 1 MaterialNumber from MaterialSupplierProperty where SupplierMaterialNumber='{0}'", txtUniversalNumber.Text.Trim());
                    if (SqlHelper.GetScalar(sql).Equals(""))
                    {
                        lbSubmit.Text = "未知的供应商物料编号！";
                        return;
                    }
                    else
                    {
                        productNumber = SqlHelper.GetScalar(sql);
                    }
                }
                if (drpType.SelectedValue == "客户物料编号")
                {
                    string mNumber = MaterialCustomerPropertyManager.GetMaterialNumberByOderNumberAndCustomerMN(
                          txtUniversalNumber.Text.Trim(), OdersNumber);
                    //sql = string.Format(@"select top 1 MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", txtUniversalNumber.Text.Trim());
                    if (string.IsNullOrEmpty(mNumber))
                    {
                        lbSubmit.Text = "未知的客户物料编号！";
                        return;
                    }
                    else
                    {
                        productNumber = mNumber;
                    }
                }
                if (drpType.SelectedValue == "瑞普迪编号")
                {
                    sql = string.Format(@"select top 1 MaterialNumber from MarerialInfoTable where MaterialNumber='{0}'", txtUniversalNumber.Text.Trim());
                    if (SqlHelper.GetScalar(sql).Equals(""))
                    {
                        lbSubmit.Text = "未知的瑞普迪编号！";
                        return;
                    }
                    else
                    {
                        productNumber = SqlHelper.GetScalar(sql);
                    }
                }
                if (drpType.SelectedValue == "物料描述")
                {
                    sql = string.Format(@"select top 1 MaterialNumber from MarerialInfoTable where Description='{0}'", txtUniversalNumber.Text.Trim());
                    if (SqlHelper.GetScalar(sql).Equals(""))
                    {
                        lbSubmit.Text = "未知的物料描述！";
                        return;
                    }
                    else
                    {
                        productNumber = SqlHelper.GetScalar(sql);
                    }
                }

                sql = string.Format(@"select Description,Brand from MarerialInfoTable where MaterialNumber='{0}'", productNumber);
                dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    brand = dt.Rows[0]["Brand"].ToString();
                    description = dt.Rows[0]["Description"].ToString();

                }
                sql = string.Format(@"select * from [V_FindPriceForNewForTrading] 
where  原材料编号='{0}' and  客户编号=( select CustomerId from SaleOder where OdersNumber='{1}')", productNumber, OdersNumber);
                dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    sql = string.Format(@"select 单价 from  V_FindLastNewPriceForTradingQuoteDetail
where 原材料编号='{0}' and customerId=( select CustomerId from SaleOder where OdersNumber='{1}')", productNumber, OdersNumber);

                    unitPrice = SqlHelper.GetScalar(sql);
                    qty = string.IsNullOrEmpty(quantity) ? 0 : Convert.ToInt32(quantity);
                    txtUnitPrice.Text = unitPrice;
                    uprice = string.IsNullOrEmpty(unitPrice) ? 0 : Convert.ToDecimal(unitPrice);
                    totalPrice = qty * uprice;

                }

                sql = string.Format(@"
select * from TradingOrderDetail where OdersNumber ='{0}' and ProductNumber='{1}' and RowNumber='{2}'", OdersNumber, productNumber, rowNumber);
                dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    lbSubmit.Text = "已有此记录请重新填写！";
                    //BindCustomer(productNumber, customerMareial);
                    return;
                }


                sql = string.Format(@" insert into TradingOrderDetail (OdersNumber,SN,ProductNumber,RowNumber ,CustomerMaterialNumber,MaterialName
,Brand,Quantity,NonDeliveryQty,DeliveryQty,UnitPrice,TotalPrice,Delivery,CreateTime,Remark,ReceiveOne,ReceiveTwo)
values('{0}',{1},'{2}','{3}','{4}','{5}','{6}',{7},{8},{9},{10},{11},'{12}','{13}','{14}','{15}','{16}')"
  , OdersNumber, sn, productNumber, rowNumber, customerMareial, description, brand, qty, qty, 0
  , uprice, totalPrice, delivery, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remark, receiveone, receivetwo);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加贸易销售订单明细" + OdersNumber, "增加成功");
                    //BindCustomer(productNumber, customerMareial);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加贸易销售订单明细" + OdersNumber, "增加失败！原因" + error);
                    //BindCustomer(productNumber, customerMareial);
                    return;
                }
            }

            else
            {
                HasDeleted();
                unitPrice = txtUnitPrice.Text.Trim();
                totalPrice = qty * uprice;
                string rownumber = ToolManager.GetQueryString("RowNumber");
                sql = string.Format(@"
select * from TradingOrderDetail where OdersNumber ='{0}' and ProductNumber='{1}' and rownumber='{2}' and rownumber!='{3}'", OdersNumber, productNumber, delivery, rownumber);
                dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    lbSubmit.Text = "已有此记录请重新填写！";
                    //BindCustomer(productNumber, customerMareial);
                    return;
                }
                sql = string.Format(@"  update TradingOrderDetail set SN={0},
Quantity={1},NonDeliveryQty={2},UnitPrice={3},TotalPrice ={4}
 ,Delivery='{5}',Remark ='{6}',ReceiveOne='{10}',ReceiveTwo='{11}' where OdersNumber ='{7}' and RowNumber='{8}' and ProductNumber='{9}' ", sn,
 qty, qty, uprice, totalPrice, delivery, remark, lblOdersNumber.Text, rownumber, txtMarerial.Text, receiveone, receivetwo);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功！" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑贸易销售订单明细" + lblOdersNumber.Text, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑贸易销售订单明细" + lblOdersNumber.Text, "编辑失败！原因" + error);
                    //BindCustomer(productNumber, customerMareial);
                    return;
                }
            }
        }

        //检查是否存在销售订单的行号
        private bool HasDbRowNumber(string ordersNber, string rowNumber)
        {
            string sql = string.Format(@"select COUNT(0) from TradingOrderDetail 
where OdersNumber='{0}' and RowNumber={1}", ordersNber, rowNumber);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }
    }
}