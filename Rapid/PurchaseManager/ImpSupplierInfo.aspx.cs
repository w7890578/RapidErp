using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Model;
using Rapid.ToolCode;


namespace Rapid.PurchaseManager
{
    public partial class ImpSupplierInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
             //DataTable dt = NPOIExcelHelper.ReadExcel(this.FU_Excel);
            DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
            if (ds == null)
            {
                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
                return;
            }
            List<SupplierInfo> supplierinfos = new List<SupplierInfo>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                SupplierInfo supplierinfo = new SupplierInfo();
                supplierinfo.SupplierId = dr["供应商编号"] == null ? "" : dr["供应商编号"].ToString();
                supplierinfo.SupplierName = dr["供应商名称"] == null ? "" : dr["供应商名称"].ToString();
                supplierinfo.RegisteredAddress = dr["注册地址"] == null ? "" : dr["注册地址"].ToString();
                supplierinfo.LegalPerson = dr["法人代表"] == null ? "" : dr["法人代表"].ToString();
                supplierinfo.Contacts = dr["联系人"] == null ? "" : dr["联系人"].ToString();
                supplierinfo.RegisteredPhone = dr["注册电话"] == null ? "" : dr["注册电话"].ToString();
                supplierinfo.ContactTelephone = dr["联系电话"] == null ? "" : dr["联系电话"].ToString();
                supplierinfo.Fax = dr["传真"] == null ? "" : dr["传真"].ToString();
                supplierinfo.MobilePhone = dr["手机"] == null ? "" : dr["手机"].ToString();
                supplierinfo.ZipCode = dr["邮编"] == null ? "" : dr["邮编"].ToString();
                supplierinfo.SparePhone = dr["备用电话"] == null ? "" : dr["备用电话"].ToString();
                supplierinfo.Email = dr["Email"] == null ? "" : dr["Email"].ToString();
                supplierinfo.QQ = dr["QQ"] == null ? "" : dr["QQ"].ToString();
                supplierinfo.AccountBank = dr["开户银行"] == null ? "" : dr["开户银行"].ToString();
                supplierinfo.BankRowNumber = dr["银行行号"] == null ? "" : dr["银行行号"].ToString();
                supplierinfo.BankAccount = dr["银行帐号"] == null ? "" : dr["银行帐号"].ToString();
                supplierinfo.TaxNo = dr["纳税号"] == null ? "" : dr["纳税号"].ToString();
                supplierinfo.WebsiteAddress = dr["网址"] == null ? "" : dr["网址"].ToString();
                supplierinfo.DeliveryAddress = dr["送货地点"] == null ? "" : dr["送货地点"].ToString();
                supplierinfo.FactoryAddress = dr["工厂地址"] == null ? "" : dr["工厂地址"].ToString();
                supplierinfo.Remark = dr["备注"] == null ? "" : dr["备注"].ToString();
                supplierinfo.paymentdays = 0;
                supplierinfo.percentageInAdvance = 0;
                supplierinfo.PayType = dr["付款类型"] == null ? "" : dr["付款类型"].ToString();
                supplierinfo.PaymentMode = dr["付款方式"] == null ? "" : dr["付款方式"].ToString();
                //if (dr["账期"] == null || dr["账期"].ToString() == "")
                //{
                //    supplierinfo.paymentdays = 0;

                //}
                //else
                //{
                //    supplierinfo.paymentdays = Convert.ToInt32(dr["账期"]);
                //}
                //if (dr["预付百分比"] == null || dr["预付百分比"].ToString() == "")
                //{
                //    supplierinfo.percentageInAdvance = 1.00;

                //}
                //else
                //{
                //    supplierinfo.percentageInAdvance = Convert.ToDouble(dr["预付百分比"]);
                //}
                supplierinfos.Add(supplierinfo);
            }
            bool result = SupplierInfoManager.BatchAddData(supplierinfos, ref error);
            lbMsg.Text = result == true ? "导入成功！" : "导入失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入供应商信息", "导入成功！");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入供应商信息", "导入失败！原因" + error);
                return;
            }

        }
    }
}
