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
    public partial class AddOrEditMachineOrderDetail : System.Web.UI.Page
    {
        public static bool edit = false;
        public static string show = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Check();
                DataTable dt;
                if (ToolManager.CheckQueryString("isEdit"))
                {
                    dt = HasDeleted();
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];

                        txtSN.Text = dr["SN"] == null ? "" : dr["SN"].ToString();
                        txtProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                        txtVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                        txtRowNumber.Text = dr["RowNumber"] == null ? "" : dr["RowNumber"].ToString();
                        txtQuantity.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        txtUnitPrice.Text = dr["UnitPrice"] == null ? "" : dr["UnitPrice"].ToString();
                        txtDelivery.Text = dr["LeadTime"] == null ? "" : dr["LeadTime"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        BindCustomer(txtProductNumber.Text, txtVersion.Text, dr["CustomerProductNumber"] == null ? "" : dr["CustomerProductNumber"].ToString());
                        lbProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                        lbVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                        lbCustomerProductNumber.Text = dr["CustomerProductNumber"] == null ? "" : dr["CustomerProductNumber"].ToString();
                        lbDelivery.Text = dr["LeadTime"] == null ? "" : dr["LeadTime"].ToString();
                        txtReceiveOne.Text = dr["ReceiveOne"] == null ? "" : dr["ReceiveOne"].ToString();
                        txtReceiveTwo.Text = dr["ReceiveTwo"] == null ? "" : dr["ReceiveTwo"].ToString();
                        btnSubmit.Text = "修改";
                        txtProductNumber.Visible = false;
                        txtVersion.Visible = false;
                        txtCustomerProductNumber.Visible = false;
                        lbDelivery.Visible = false;
                        txtRowNumber.Enabled = false;
                        show = "inline";
                    }


                }
                else
                {
                    btnSubmit.Text = "添加";
                    lbProductNumber.Visible = false;
                    lbVersion.Visible = false;
                    lbCustomerProductNumber.Visible = false;
                    lbDelivery.Visible = false;
                    show = "none";
                }
            }

            if (ToolManager.CheckQueryString("m"))
            {
                string result = string.Empty;
                string sql = string.Format(@" select top 20  ProductNumber,[Version] ,ProductName from Product ");
                if (ToolManager.CheckQueryString("contion"))
                {
                    sql += string.Format(@"  where
ProductNumber like '%{0}%' or ProductNumber like'%{0}' or ProductNumber like '{0}%' or
ProductName like '%{0}%' or ProductName like'%{0}' or ProductName like '{0}%' 
order by ProductNumber asc", ToolManager.GetQueryString("contion"));
                }
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result += string.Format(" <tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", dr["ProductName"], dr["ProductNumber"], dr["Version"]);
                    }
                }
                Response.Write(result);
                Response.End();
                return;
            }
            if (ToolManager.CheckQueryString("IsGetCustomerProductNumber"))
            {
                string productNumber = ToolManager.GetQueryString("productNumber");
                string version = ToolManager.GetQueryString("version");
                string odersNumber = ToolManager.GetQueryString("OdersNumber");
                string sql = string.Format(@" select CustomerProductNumber from ProductCustomerProperty where ProductNumber='{0}' and [Version]='{1}' and 
CustomerId=( select CustomerId from SaleOder where OdersNumber='{2}')", productNumber, version, odersNumber);
                Response.Write(SqlHelper.GetScalar(sql));
                Response.End();
                return;
            }
        }
        /// <summary>
        /// 绑定客户编号
        /// </summary>
        /// <param name="materialNumber"></param>
        /// <param name="customerMaterialNumber"></param>
        private void BindCustomer(string materialNumber, string version, string customerMaterialNumber)
        {
            this.txtCustomerProductNumber.Text = customerMaterialNumber;
            this.txtVersion.Text = version;
        }
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
            hdOderNumber.Value = ToolManager.GetQueryString("OdersNumber");
            string sql = string.Format(@"select OdersType from SaleOder where OdersNumber='{0}'", ToolManager.GetQueryString("OdersNumber"));
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows[0]["OdersType"].ToString() == "样品订单")
            {
                txtUnitPrice.Text = "0";
                txtUnitPrice.ReadOnly = true;
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
            string productnumber = ToolManager.GetQueryString("ProductNumber");
            string version = ToolManager.GetQueryString("Version");
            string rownumber = ToolManager.GetQueryString("RowNumber");
            string error = string.Empty;
            string sql = string.Format(@"
select * from MachineOderDetail where OdersNumber ='{0}' and ProductNumber='{1}' and Version='{2}' and RowNumber='{3}'", ToolManager.GetQueryString("OdersNumber"), productnumber, version, rownumber);
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
            string OdersNumber = ToolManager.GetQueryString("OdersNumber");
            string sn = txtSN.Text.Trim();
            string productNumber = txtProductNumber.Text.Trim();
            string version = string.Empty;
            string customerMareial = txtCustomerProductNumber.Text;
            string rowNumber = txtRowNumber.Text.Trim();
            string quantity = txtQuantity.Text.Trim();
            int qty = string.IsNullOrEmpty(quantity) ? 0 : Convert.ToInt32(quantity);
            string unitPrice = txtUnitPrice.Text.Trim();
            decimal uprice = string.IsNullOrEmpty(unitPrice) ? 0 : Convert.ToDecimal(unitPrice);
            string delivery = txtDelivery.Text.Trim();
            string remark = txtRemark.Text.Trim();
            string receiveone = txtReceiveOne.Text.Trim().Equals("") ? "0" : txtReceiveOne.Text.Trim();
            string receivetwo = txtReceiveTwo.Text.Trim().Equals("") ? "0" : txtReceiveTwo.Text.Trim();

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
            DataTable dt;
            decimal sumprice = 0;
            sql = string.Format(@"select count(*) from ProductCustomerProperty where CustomerProductNumber='{0}'", customerMareial);
            string num = SqlHelper.GetScalar(sql);
            if (num == "0")
            {
                lbSubmit.Text = "未知的客户产成品！";
                return;
            }

           

            if (btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@"select ProductNumber,Version from ProductCustomerProperty where CustomerProductNumber='{0}' and CustomerId=(select CustomerId from SaleOder where OdersNumber='{1}') and Version='{2}'",
             customerMareial, OdersNumber, txtVersion.Text.ToUpper());
                dt = SqlHelper.GetTable(sql);
                //if (dt.Rows.Count > 0)
                //{
                //    productNumber = dt.Rows[0]["ProductNumber"].ToString();
                //    version = dt.Rows[0]["Version"].ToString();
                //    sql = string.Format(@"select SalesQuotation from Product where ProductNumber='{0}' and Version='{1}'", productNumber, version);
                //    dt = SqlHelper.GetTable(sql, ref error);
                //    if (dt.Rows.Count > 0)
                //    {
                //        unitPrice = dt.Rows[0]["SalesQuotation"].ToString();
                //        uprice = string.IsNullOrEmpty(unitPrice) ? 0 : Convert.ToDecimal(unitPrice);
                //        txtUnitPrice.Text = unitPrice;


                //    }
                //}
                if (dt.Rows.Count > 0)
                {
                    productNumber = dt.Rows[0]["ProductNumber"].ToString();
                    version = dt.Rows[0]["Version"].ToString();


                    if (!ProductManager.HasBOM(productNumber, version))
                    {
                        lbSubmit.Text = "该产成品编号和版本找不到BOM信息，请先录入BOM！";
                        return;
                    }

                    sql = string.Format(@" select 单价未税 from V_FindLastNewPriceForMachineQuoteDeatil where 产成品编号='{0}' and 版本='{1}' and 
CustomerId=(select CustomerId from SaleOder where OdersNumber='{2}' )",
 productNumber, txtVersion.Text.ToUpper(), OdersNumber);
                    dt = SqlHelper.GetTable(sql, ref error);
                    if (dt.Rows.Count > 0)
                    {
                        unitPrice = dt.Rows[0]["单价未税"].ToString();
                        uprice = string.IsNullOrEmpty(unitPrice) ? 0 : Convert.ToDecimal(unitPrice);
                        txtUnitPrice.Text = unitPrice;


                    }
                }
                else
                {
                    lbSubmit.Text = "未知的产品和版本！";
                    return;
                }
                sumprice = uprice * qty;//单价*数量
                // version = Request.Form["txtVersion"].ToString();
                customerMareial = Request.Form["txtCustomerProductNumber"].ToString();

                sql = string.Format(@" 
select COUNT(*) from  MachineOderDetail where OdersNumber ='{0}'  and rownumber='{1}'", ToolManager.GetQueryString("OdersNumber"), rowNumber);
                if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "已存在相同行号！请重新填写";
                    BindCustomer(productNumber, version, customerMareial);
                    return;
                }


                sql = string.Format(@"insert into MachineOderDetail (OdersNumber ,SN ,ProductNumber ,CustomerProductNumber ,[Version]
,RowNumber ,Qty,NonDeliveryQty,DeliveryQty ,UnitPrice ,SumPrice ,LeadTime ,Remark ,CreateTime,ReceiveOne,ReceiveTwo  )
values('{0}',{1},'{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},'{11}','{12}','{13}','{14}','{15}')"
, OdersNumber, sn, productNumber, customerMareial, version, rowNumber, qty, qty, 0,
uprice, sumprice, delivery, remark, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), receiveone, receivetwo);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加加工销售订单明细" + OdersNumber, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加加工销售订单明细" + OdersNumber, "增加失败！原因" + error);
                    BindCustomer(productNumber, version, customerMareial);
                    return;
                }
            }
            else
            {
                HasDeleted();
                sumprice = uprice * qty;
                string rownumber = ToolManager.GetQueryString("RowNumber");
                productNumber = lbProductNumber.Text;
                version = lbVersion.Text;
                customerMareial = lbCustomerProductNumber.Text;
                sql = string.Format(@" update MachineOderDetail set SN={0},
Qty ={1},UnitPrice={2},SumPrice ={3},LeadTime='{4}',
Remark ='{5}',NonDeliveryQty='{10}',ReceiveOne='{11}',ReceiveTwo='{12}'
where OdersNumber='{6}' and  ProductNumber='{7}' and Version='{8}' and rownumber='{9}'", sn,
qty, uprice, sumprice, delivery, remark, ToolManager.GetQueryString("OdersNumber"), productNumber, lbVersion.Text, rownumber, quantity, receiveone, receivetwo);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {

                    Response.Write(ToolManager.GetClosePageJS());
                    Tool.WriteLog(Tool.LogType.Operating, "编辑加工销售订单明细" + ToolManager.GetQueryString("OdersNumber"), "编辑成功");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑加工销售订单明细" + ToolManager.GetQueryString("OdersNumber"), "编辑失败！原因" + error);
                    BindCustomer(productNumber, version, customerMareial);
                    return;
                }
            }
        }
    }
}
