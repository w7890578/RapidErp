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
    public partial class EditMachineQuoteDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("Guid"))
                {
                    Response.Write("该报价单不存在！");
                    Response.End();
                    return;
                }
                this.trSN.Visible = false;
                string guid = ToolManager.GetQueryString("Guid");
                string error = string.Empty;
                string sql = string.Format(" select * from MachineQuoteDetail where  Guid='{0}'", ToolManager.GetQueryString("Guid"));
                DataTable dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtSN.Text = dr["SN"] == null ? "" : dr["SN"].ToString();
                    lbProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                    lbCustomerProductNumber.Text = dr["CustomerMaterialNumber"] == null ? " " : dr["CustomerMaterialNumber"].ToString();
                    lbBOMAmount.Text = dr["BOMAmount"] == null ? "" : dr["BOMAmount"].ToString();
                    lbHierarchy.Text = dr["Hierarchy"] == null ? "" : dr["Hierarchy"].ToString();
                    lbDescription.Text = dr["Description"] == null ? "" : dr["Description"].ToString();
                    txtMaterialPrcie.Text = dr["MaterialPrcie"] == null ? "" : dr["MaterialPrcie"].ToString();
                    lbMaterialPrcie.Text = dr["MaterialPrcie"] == null ? "" : dr["MaterialPrcie"].ToString();

                    //工时费
                    if (lbHierarchy.Text.Trim() == "0")
                    {
                        sql = string.Format(@" select SUM(p.QuoteManhour) from PackageAndProductRelation pap inner join PackageInfo pi 
                        on pap.PackageNumber=pi.PackageNumber inner join Product p on pap.ProductNumber=p.ProductNumber
                        and pap.Version=p.Version where pi.PackageNumber='{0}'", dr["PackageNumber"]);
                        string timeCharge = SqlHelper.GetScalar(sql);
                        lbTimeCharge.Text = timeCharge;
                    }
                    else
                    {
                        sql = string.Format(@" select isnull(QuoteManhour,0) from product where productnumber='{0}' and version='{1}'",
                             lbProductNumber.Text.Trim(), dr["Version"]);
                        string TimeCharge = SqlHelper.GetScalar(sql);
                        lbTimeCharge.Text = TimeCharge;

                    }

                    txtProfit.Text = dr["Profit"] == null ? "0" : dr["Profit"].ToString();
                    txtManagementPrcie.Text = dr["ManagementPrcie"] == null ? "" : dr["ManagementPrcie"].ToString();
                    txtLossPrcie.Text = dr["LossPrcie"] == null ? "" : dr["LossPrcie"].ToString();
                    txtUnitPrice.Text = dr["UnitPrice"] == null ? "" : dr["UnitPrice"].ToString();
                    txtFixedLeadTime.Text = dr["FixedLeadTime"] == null ? " " : dr["FixedLeadTime"].ToString();
                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();


                    this.txtMarerialPriceManagementPrcie.Text = lbMaterialPrcie.Text.Trim();
                    this.txtTimeChargeManagementPrcie.Text = this.lbTimeCharge.Text.Trim();
                    this.txtMarerialPriceLossPrcie.Text = lbMaterialPrcie.Text.Trim();
                    this.txtMarerialPriceUnitPrice.Text = lbMaterialPrcie.Text.Trim();
                    this.txtTimeChargeUnitPrice.Text = lbTimeCharge.Text.Trim();
                    this.txtManagementPrcieUnitPrice.Text = txtManagementPrcie.Text.Trim();
                    this.txtLossPrcieUnitPrice.Text = txtLossPrcie.Text.Trim();
                    this.txtProfitUnitPrice.Text = txtProfit.Text.Trim();
                    btnSubmit.Text = "修改";

                    txtMarerialOnePrice.Text = txtMaterialPrcie.Text.Trim();
                    txtBOMAmountUnitPrice.Text = lbBOMAmount.Text.Trim();
                    if (Convert.ToInt32(lbHierarchy.Text.Trim()) == 0 || Convert.ToInt32(lbHierarchy.Text.Trim()) == 1)
                    {
                        //隐藏span
                        this.sUnitPriceTwo.Visible = false;
                        this.sUnitPriceOne.Visible = true;
                        this.txtMaterialPrcie.Visible = false;
                    }

                    if (Convert.ToInt32(lbHierarchy.Text.Trim()) == 2)
                    {
                        //隐藏span
                        this.txtMaterialPrcie.ReadOnly = false;
                        this.sUnitPriceTwo.Visible = true;
                        this.sUnitPriceOne.Visible = false;
                        this.trTimeCharge.Visible = false;
                        this.trProfit.Visible = false;
                        this.trManagementPrcie.Visible = false;
                        this.trLossPrcie.Visible = false;
                        this.lbMaterialPrcie.Visible = false;

                    }

                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ToolManager.CheckQueryString("Guid"))
            {
                Response.Write("未知报价单！");
                Response.End();
            }
            string guid = ToolManager.GetQueryString("Guid");
            string error = string.Empty;
            string sql = string.Empty;
            string sn = txtSN.Text.Trim();
            string productNumber = lbProductNumber.Text.Trim();
            string customerProductNumber = lbCustomerProductNumber.Text.Trim();
            bool result = false;
            if (customerProductNumber == null || customerProductNumber == "")
            {
                customerProductNumber = " ";
            }
            string bomamount = lbBOMAmount.Text.Trim();
            string description = lbDescription.Text.Trim();
            string fixedleadtime = this.txtFixedLeadTime.Text.Trim();
            string remark = txtRemark.Text.Trim();
            //阶层
            string hierarchy = lbHierarchy.Text.Trim();
            if (Convert.ToInt32(hierarchy) == 0 || Convert.ToInt32(hierarchy) == 1)
            {
                //工时费
                string timecharge = this.lbTimeCharge.Text.Trim();
                //利润
                string profit = this.txtProfit.Text.Trim();
                //原材料单价未税
                string materialprice = txtMaterialPrcie.Text.Trim();
                //管销研费用txtManagementPrcie
                Double coefficientmanagementprcie0 = Convert.ToDouble(this.txtCoefficientManagementPrcie0.Text.Trim());
                Double coefficientmanagementprcie1 = Convert.ToDouble(this.txtCoefficientManagementPrcie1.Text.Trim());
                Double Amanagementprcie = Convert.ToDouble(this.txtMarerialPriceManagementPrcie.Text.Trim() == null ? "0" :
                           this.txtMarerialPriceManagementPrcie.Text.Trim()) * coefficientmanagementprcie0 +
                           Convert.ToDouble(this.txtTimeChargeManagementPrcie.Text.Trim() == null ? "0" :
                           this.txtTimeChargeManagementPrcie.Text.Trim()) * coefficientmanagementprcie1;
                string managementprcie = Amanagementprcie.ToString();
                //损耗
                Double coefficientlossprcie = Convert.ToDouble(this.txtCoefficientLossPrcie.Text.Trim());
                Double Alossprcie = Convert.ToDouble(this.txtMarerialPriceLossPrcie.Text.Trim() == null ? "0" :
                         this.txtMarerialPriceLossPrcie.Text.Trim()) * coefficientlossprcie;
                string lossprcie = Alossprcie.ToString();
                //单价未税
                Double Aunitprice = Convert.ToDouble(materialprice == null ? "0" : materialprice) + Convert.ToDouble(timecharge == null ? "0" : timecharge) +
                        Convert.ToDouble(profit == null ? "0" : profit) + Convert.ToDouble(managementprcie) + Convert.ToDouble(lossprcie);
                string unitprice = Aunitprice.ToString();
                sql = string.Format(@"
            update MachineQuoteDetail set Hierarchy='{0}',ProductNumber='{1}',CustomerMaterialNumber='{2}',description='{3}',
            BOMAmount={4},MaterialPrcie={5},TimeCharge={6},Profit={7},ManagementPrcie={8},LossPrcie={9},UnitPrice={10},
            FixedLeadTime='{11}',Remark='{12}' where guid='{13}'", hierarchy, productNumber, customerProductNumber,
            description, bomamount, materialprice, timecharge, profit, managementprcie, lossprcie, unitprice, fixedleadtime, remark, guid);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑加工报价单明细", "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑加工报价单明细", "编辑失败！原因" + error);
                    return;
                }
            }

            if (Convert.ToInt32(hierarchy) == 2)
            {

                string BOMamount = this.lbBOMAmount.Text.Trim();
                string unitprice = txtMaterialPrcie.Text.Trim();
                string fiexedleadtime = this.txtFixedLeadTime.Text.Trim();
                //单价未税
                Double Aunitprice = Convert.ToDouble(BOMamount == null ? "0" : BOMamount) *
                    Convert.ToDouble(unitprice == null ? "0" : unitprice);
                string materialprcie = Aunitprice.ToString();
                sql = string.Format(@"
            update MachineQuoteDetail set Hierarchy='{0}',ProductNumber='{1}',CustomerMaterialNumber='{2}',description='{3}',
            BOMAmount={4},MaterialPrcie={5},UnitPrice={6},
            FixedLeadTime='{7}',Remark='{8}' where guid='{9}'", hierarchy, productNumber, customerProductNumber,
            description, BOMamount, unitprice, materialprcie, fiexedleadtime, remark, guid);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑加工报价单明细", "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑加工报价单明细", "编辑失败！原因" + error);
                    return;
                }
            }

        }
    }
}
