using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class AddOrEditMachineQuote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("QuoteNumber"))
                {
                    Response.Write("该报价单不存在！");
                    Response.End();
                    return;
                }
                string quoteNumber = ToolManager.GetQueryString("QuoteNumber");
                hdQuoteNumber.Value = quoteNumber;
                if (ToolManager.CheckQueryString("sn"))
                {
                    string error = string.Empty;

                    string sql = string.Format("select * from MachineQuoteDetail where QuoteNumber ='{0}' and SN='{1}'", quoteNumber, ToolManager.GetQueryString("sn"));
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    DataRow dr = dt.Rows[0];
                    txtSN.Text = dr["SN"] == null ? "" : dr["SN"].ToString();
                    txtProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                    txtCustomerProductNumber.Text = dr["CustomerMaterialNumber"] == null ? "" : dr["CustomerMaterialNumber"].ToString();
                    txtHierarchy.Text = dr["Hierarchy"] == null ? "" : dr["Hierarchy"].ToString();
                    txtVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                    txtBOMAmount.Text = dr["BOMAmount"] == null ? "" : dr["BOMAmount"].ToString();
                    txtMaterialPrcie.Text = dr["MaterialPrcie"] == null ? "" : dr["MaterialPrcie"].ToString();
                    txtTimeCharge.Text = dr["TimeCharge"] == null ? "" : dr["TimeCharge"].ToString();
                    txtProfit.Text = dr["Profit"] == null ? "" : dr["Profit"].ToString();
                    txtManagementPrcie.Text = dr["ManagementPrcie"] == null ? "" : dr["ManagementPrcie"].ToString();
                    txtLossPrcie.Text = dr["LossPrcie"] == null ? "" : dr["LossPrcie"].ToString();
                    txtUnitPrice.Text = dr["UnitPrice"] == null ? "" : dr["UnitPrice"].ToString();
                    txtFixedLeadTime.Text = dr["FixedLeadTime"] == null ? "" : dr["FixedLeadTime"].ToString();
                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString(); 
                    btnSubmit.Text = "修改";
                }
                else
                { 
                    btnSubmit.Text = "添加";
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

                if (ToolManager.CheckQueryString("productNumber") && ToolManager.CheckQueryString("version") && ToolManager.CheckQueryString("QuoteNumber"))
                {
                    string productNumber=ToolManager.GetQueryString("productNumber");
                    string version=ToolManager.GetQueryString("version");
                    string QuoteNumber = ToolManager.GetQueryString("QuoteNumber");
                    string sql = string.Format(@"select CustomerProductNumber from ProductCustomerProperty where ProductNumber='{0}' and  [Version]='{1}' and  CustomerId=(select CustomerId from QuoteInfo where QuoteNumber='{2}')", productNumber, version, QuoteNumber);
                    Response.Write(SqlHelper.GetScalar(sql));
                    Response.End();
                    return;
                }
            }
        }

        private void BindCustomer(string productNumber, string version, string customerProductNumber)
        {
            this.txtCustomerProductNumber.Text = customerProductNumber;
            txtVersion.Text = version;
        }
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ToolManager.CheckQueryString("QuoteNumber"))
            {
                Response.Write("未知订单");
                Response.End();
            }
            string quoteNumber = ToolManager.GetQueryString("QuoteNumber");
            string error = string.Empty;
            string sql = string.Empty;
            string sn = txtSN.Text.Trim();
            string productNumber = txtProductNumber.Text.Trim();
            string customerProductNumber = Request.Form["txtCustomerProductNumber"].ToString();
            string version = Request.Form["txtVersion"].ToString();
            string hierarchy = txtHierarchy.Text.Trim();
            string bomAmount = txtBOMAmount.Text.Trim();
            string materialPrcie = txtMaterialPrcie.Text.Trim();
            string timeCharge = txtTimeCharge.Text.Trim();
            string profit = txtProfit.Text.Trim();
            string managementPrcie = txtManagementPrcie.Text.Trim();
            string lossPrice = txtLossPrcie.Text.Trim();
            string unitPrice = txtUnitPrice.Text.Trim();
            string fixedLeadTime = txtFixedLeadTime.Text.Trim();
            string remark = txtRemark.Text;
            if (!ProductManager.IsExit(productNumber, version))
            {
                lbSubmit.Text = "该产品编号和版本不存在，请重新填写！";
                return;
            }
            if (btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format("select COUNT(*) from MachineQuoteDetail where QuoteNumber='{0}' and SN={1}", quoteNumber, sn);
                if (!SqlHelper.GetTable(sql).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "已存在该序号，请重新填写序号！";
                    BindCustomer(productNumber, version, customerProductNumber);
                    return;
                }
                sql = string.Format(@" insert into MachineQuoteDetail (QuoteNumber ,SN,ProductNumber,
Hierarchy,CustomerMaterialNumber,[Version] 
 ,BOMAmount ,MaterialPrcie,TimeCharge,Profit,ManagementPrcie,LossPrcie ,UnitPrice ,FixedLeadTime ,
 Remark ) values('{0}',{1},'{2}','{3}','{4}','{5}',{6},{7},{8},{9},{10},{11},{12},'{13}','{14}')",
quoteNumber, sn, productNumber, hierarchy, customerProductNumber
, version, bomAmount, materialPrcie, timeCharge, profit, managementPrcie, lossPrice, unitPrice, fixedLeadTime, remark);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加加工报价单信息" + quoteNumber, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加加工报价单信息" + quoteNumber, "增加成失败！原因"+error);
                    BindCustomer(productNumber, version, customerProductNumber);
                    return;
                }

            }
            else
            {
                string getSN = ToolManager.GetQueryString("sn");
                sql = string.Format(@"select COUNT(*) from  MachineQuoteDetail where QuoteNumber='{0}'  and SN !={1} and SN ={2}", quoteNumber, getSN, sn);
                if (!SqlHelper.GetTable(sql).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "已存在该序号，请重新填写序号！";
                    BindCustomer(productNumber, version, customerProductNumber);
                    return;
                }
                sql = string.Format(@"
 update MachineQuoteDetail set SN={0},ProductNumber='{1}',Hierarchy='{2}'
 ,CustomerMaterialNumber='{3}',[Version]='{4}',BOMAmount={5},MaterialPrcie={6},
TimeCharge={7},Profit={8},ManagementPrcie={9},LossPrcie={10},UnitPrice={11},
FixedLeadTime='{12}',Remark='{13}' where QuoteNumber='{14}' and SN={15}", sn, productNumber, hierarchy, customerProductNumber
 , version, bomAmount, materialPrcie, timeCharge, profit, materialPrcie, lossPrice, unitPrice
 , fixedLeadTime, remark, quoteNumber, getSN);
                bool result= SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑加工报价单信息" + hdQuoteNumber.Value, "编辑成功");
                    BindCustomer(productNumber, version, customerProductNumber);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑加工报价单信息" + hdQuoteNumber.Value, "编辑成失败！原因" + error);
                    BindCustomer(productNumber, version, customerProductNumber);
                    return;
                }
            }
        }


    }
}
