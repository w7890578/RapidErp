using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.PurchaseManager
{
    public partial class AddOrEditCertificateOrdersDetail : System.Web.UI.Page
    {
        public static string show = "inline";
        public static string showPay = "none";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Check();
                if (ToolManager.CheckQueryString("OdersNumber") && ToolManager.CheckQueryString("Guid"))
                {
                    DataTable dt = HasDeleted();
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        txtOrdersNumber.Text = ToolManager.GetQueryString("OdersNumber");
                        txtRowNumber.Text = dr["RowNumber"] == null ? "" : dr["RowNumber"].ToString();
                        txtOrderQty.Text = dr["OrderQty"] == null ? "" : dr["OrderQty"].ToString();
                        lbSupplierMaterialNumber.Text = dr["SupplierMaterialNumber"] == null ? "" : dr["SupplierMaterialNumber"].ToString();
                        txtUnitPrice.Text = dr["UnitPrice"] == null ? "" : dr["UnitPrice"].ToString();
                        txtLeadTime.Text = dr["LeadTime"] == null ? "" : dr["LeadTime"].ToString();
                        txtUnitPrice_C.Text = dr["UnitPrice_C"] == null ? "" : dr["UnitPrice_C"].ToString();
                        txtSumPrice.Text = dr["SumPrice"] == null ? "" : dr["SumPrice"].ToString();
                        txtSumPrice_C.Text = dr["SumPrice_C"] == null ? "" : dr["SumPrice_C"].ToString();
                        txtMinOrderQty.Text = dr["MinOrderQty"] == null ? "" : dr["MinOrderQty"].ToString();
                        txtPayOne.Text = dr["PayOne"] == null ? "" : dr["PayOne"].ToString();
                        txtPayTwo.Text = dr["PayTwo"] == null ? "" : dr["PayTwo"].ToString();

                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    }
                    btnSubmit.Text = "修改";
                    txtSupplierMaterialNumber.Visible = false;
                    drpType.Enabled = false;
                    show = "inline";
                }
                else
                {
                    btnSubmit.Text = "添加";
                    lbSupplierMaterialNumber.Visible = false;
                    show = "none";

                }
                string ordrNumber = ToolManager.GetQueryString("OdersNumber");
                string sql = string.Format("select PaymentMode  from CertificateOrders where OrdersNumber='{0}'", ordrNumber);
                if (SqlHelper.GetScalar(sql).Equals("YFBF"))
                {
                    showPay = "inline";
                }
                else
                {
                    showPay = "none";
                }


            }
            if (ToolManager.CheckQueryString("m"))
            {
                string result = string.Empty;
                string sql = string.Format(@" select top 20  MaterialNumber,MaterialName from MarerialInfoTable ");
                if (ToolManager.CheckQueryString("contion"))
                {
                    sql += string.Format(@"  where
MaterialNumber like '%{0}%' or MaterialNumber like'%{0}' or MaterialNumber like '{0}%' or
MaterialName like '%{0}%' or MaterialName like'%{0}' or MaterialName like '{0}%' 
order by MaterialNumber asc", ToolManager.GetQueryString("contion"));
                }
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result += string.Format(" <tr><td>{0}</td><td>{1}</td></tr>", dr["MaterialName"], dr["MaterialNumber"]);
                    }
                }
                Response.Write(result);
                Response.End();
                return;
            }
            //ajax请求，根据原材料编号获取供应商物料编号
            if (ToolManager.CheckQueryString("MaterialNumber"))
            {
                string materialNumber = ToolManager.GetQueryString("MaterialNumber");
                string odersNumber = ToolManager.GetQueryString("OdersNumber");
                string sql = string.Format(@" select SupplierMaterialNumber from 
MaterialSupplierProperty where SupplierId =(
 select SupplierId  from CertificateOrders where OrdersNumber ='{0}')
 and MaterialNumber='{1}'", odersNumber, materialNumber);
                Response.Write(SqlHelper.GetScalar(sql));
                Response.End();
                return;
            }
        }
        /// <summary>
        /// 绑定供货商物料编号
        /// </summary>
        /// <param name="materialNumber"></param>
        /// <param name="customerMaterialNumber"></param>
        private void BindSupplierMaterial(string materialNumber, string SupplierMaterialNumber)
        {
            this.txtSupplierMaterialNumber.Text = SupplierMaterialNumber;

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Check();
            HasOdersNumber();
            string error = string.Empty;
            string sql = string.Empty;
            string OdersNumber = ToolManager.GetQueryString("OdersNumber");
            string rownumber = txtRowNumber.Text.Trim();
            string materianumber = string.Empty;
            string orderqty = txtOrderQty.Text.Trim();
            string minOrderQty = txtMinOrderQty.Text.Trim();

            string unitprice = txtUnitPrice.Text.Trim();
            string unitprice_C = txtUnitPrice_C.Text.Trim();
            string sumPrice = txtSumPrice.Text.Trim();
            string sumPrice_C = txtSumPrice_C.Text.Trim();
            string payOne = txtPayOne.Text.Trim();
            string payTwo = txtPayTwo.Text.Trim();
            string leadtime = txtLeadTime.Text.Trim();
            string remark = txtRemark.Text.Trim();
          
          

            string suppierId = string.Empty;
            sql = string.Format(" select SupplierId   from CertificateOrders where OrdersNumber='{0}' ", OdersNumber);
            suppierId = SqlHelper.GetScalar(sql);


            if (btnSubmit.Text.Equals("添加"))
            {
                string suppliermaterialnumber = Request.Form["txtSupplierMaterialNumber"].ToString();

                if (drpType.SelectedValue == "供应商物料编号")
                {
                    sql = string.Format(@"select top 1 MaterialNumber from MaterialSupplierProperty where SupplierMaterialNumber='{0}' and SupplierId='{1}'", suppliermaterialnumber.Trim(),suppierId);
                    if (SqlHelper.GetScalar(sql).Equals(""))
                    {
                        lbSubmit.Text = "未知的供应商物料编号！";
                        return;
                    }
                    else
                    {
                        materianumber = SqlHelper.GetScalar(sql);
                        suppliermaterialnumber = Request.Form["txtSupplierMaterialNumber"].ToString().Trim ();
                    }
                }
                if (drpType.SelectedValue == "瑞普迪编号")
                {
                    sql = string.Format(@"select top 1 SupplierMaterialNumber from MaterialSupplierProperty where MaterialNumber='{0}' and SupplierId='{1}'", suppliermaterialnumber.Trim(), suppierId);
                    if (SqlHelper.GetScalar(sql).Equals(""))
                    {
                        lbSubmit.Text = "没有该原材料编号的供应商物料编号！";
                        return;
                    }
                    else
                    {
                        suppliermaterialnumber = SqlHelper.GetScalar(sql);
                        materianumber = Request.Form["txtSupplierMaterialNumber"].ToString().Trim (); 
                    }
                }
                sql = string.Format(@"
select COUNT(*) from CertificateOrdersDetail where OrdersNumber ='{0}' and MaterialNumber='{1}' and LeadTime ='{2}'
", OdersNumber,materianumber ,leadtime );
                if(!SqlHelper .GetScalar (sql).Equals ("0"))
                {
                    lbSubmit.Text = string.Format("已存在相同记录！");
                    return;
                }

                sql = string.Format(@"
select Prcie  from MaterialSupplierProperty 
where MaterialNumber='{0}' and SupplierId ='{1}' and SupplierMaterialNumber='{2}'
", materianumber, suppierId,suppliermaterialnumber );
                unitprice = SqlHelper.GetScalar(sql);

                sql = string.Format(@"insert into CertificateOrdersDetail (OrdersNumber ,
RowNumber ,MaterialNumber ,SupplierMaterialNumber ,
OrderQty
,NonDeliveryQty ,DeliveryQty,UnitPrice ,LeadTime ,Status ,Remark,UnitPrice_C,SumPrice,SumPrice_C,PayOne,PayTwo,MinOrderQty)
values('{0}',{1},'{2}','{3}',{4},{5},{6},{7},'{8}','{9}','{10}',{11}*1.17,{11}*{4},{11}*1.17*{4},{12},{13},'{14}')"
, OdersNumber, rownumber, materianumber, suppliermaterialnumber, orderqty, orderqty, "0",
unitprice, leadtime, "未完成", remark,unitprice ,payOne ,payTwo ,minOrderQty );
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加采购订单明细" + OdersNumber, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    txtOrdersNumber.Text = ToolManager.GetQueryString("OdersNumber");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加采购订单明细" + OdersNumber, "增加失败！原因" + error);
                    BindSupplierMaterial(materianumber, suppliermaterialnumber);
                    return;
                }
            }
            else
            {
                HasDeleted();  
                string suppliermaterialnumber = lbSupplierMaterialNumber.Text;
                sql = string.Format(@"
select COUNT(*) from CertificateOrdersDetail where OrdersNumber ='{0}' and MaterialNumber='{1}' and LeadTime ='{2}'
", OdersNumber, materianumber, leadtime);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    lbSubmit.Text = string.Format("已存在该供应商物料编号：{0}，交期：{1}的数据", suppliermaterialnumber, leadtime);
                    return;
                }
                sql = string.Format(@"update CertificateOrdersDetail 
set LeadTime ='{0}',OrderQty={1},NonDeliveryQty={1} ,UnitPrice={2},UnitPrice_C={3},SumPrice ={4},SumPrice_C ={5}
,MinOrderQty={6},PayOne ={7},PayTwo ={8},Remark='{10}' where Guid='{9}'", leadtime, orderqty, unitprice, unitprice_C, sumPrice, sumPrice_C,
 minOrderQty, payOne, payTwo, ToolManager.GetQueryString("Guid"),remark); 
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑采购订单明细" + OdersNumber, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑采购订单明细" + OdersNumber, "编辑失败！原因" + error);
                    return;
                }
            }
        }


        /// <summary>
        /// 检查采购订单编号是否传过来
        /// </summary>
        private void Check()
        {
            if (!ToolManager.CheckQueryString("OdersNumber"))
            {
                Response.Write("未知采购单！");
                Response.End();
                return;
            }
            hdOderNumber.Value = ToolManager.GetQueryString("OdersNumber");
            txtOrdersNumber.Text = ToolManager.GetQueryString("OdersNumber");
        }
        /// <summary>
        /// 检查该条记录是否被删除
        /// </summary>
        /// <returns></returns>
        private DataTable HasDeleted()
        {

            string error = string.Empty;
            string sql = string.Format(@"
       select * from CertificateOrdersDetail where OrdersNumber='{0}' and Guid='{1}'", ToolManager.GetQueryString("OdersNumber"), ToolManager.GetQueryString("Guid"));
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
        /// 检查该条采购订单是否被删除
        /// </summary>
        /// <returns></returns>
        private void HasOdersNumber()
        {
            string OdersNumber = ToolManager.GetQueryString("OdersNumber");
            string error = string.Empty;
            string sql = string.Format("select COUNT(*) from CertificateOrders where OrdersNumber='{0}'", OdersNumber);
            if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                Response.Write("该条采购订单不存在！");
                Response.End();
                return;
            }
        }
    }
}

