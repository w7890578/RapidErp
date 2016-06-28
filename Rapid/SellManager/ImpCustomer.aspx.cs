using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Model;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class ImpCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
            if (ds == null)
            {
                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
                return;
            }
            List<Customer> customerss = new List<Customer>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Customer customer = new Customer();
                customer.CustomerId = dr["客户编号"]== null ? "" : dr["客户编号"].ToString();
                customer.CustomerName = dr["客户名称"] == null ? "" : dr["客户名称"].ToString();
                customer.RegisteredAddress = dr["注册地址"] == null ? "" : dr["注册地址"].ToString();
                customer.LegalPerson = dr["法人代表"] == null ? "" : dr["法人代表"].ToString();
                customer.Contacts = dr["联系人"] == null ? "" : dr["联系人"].ToString();
                customer.RegisteredPhone = dr["注册电话"]== null ? "" : dr["注册电话"].ToString();
                customer.ContactTelephone = dr["联系电话"] == null ? "" : dr["联系电话"].ToString();
                customer.Fax = dr["传真"] == null ? "" : dr["传真"].ToString();
                customer.MobilePhone = dr["手机"]== null ? "" : dr["手机"].ToString();
                customer.ZipCode = dr["邮编"] == null ? "" : dr["邮编"].ToString();
                customer.SparePhone = dr["备用电话"] == null ? "" : dr["备用电话"].ToString();
                customer.Email = dr["Email"] == null ? "" : dr["Email"].ToString();
                customer.QQ = dr["QQ"]== null ? "" : dr["QQ"].ToString();
                customer.AccountBank = dr["开户银行"] == null ? "" : dr["开户银行"].ToString();
                customer.SortCode = dr["银行行号"]== null ? "" : dr["银行行号"].ToString();
                customer.BankAccount = dr["银行账号"] == null ? "" : dr["银行账号"].ToString();
                customer.TaxNo = dr["纳税号"]== null ? "" : dr["纳税号"].ToString();
                customer.WebsiteAddress = dr["网址"]== null ? "" : dr["网址"].ToString();
                customer.DeliveryAddress = dr["送货地点"]== null ? "" : dr["送货地点"].ToString();
                customer.FactoryAddress = dr["工厂地址"] == null ? "" : dr["工厂地址"].ToString();
                
                string sql = string.Format(@" select id from MakeCollectionsMode where MakeCollectionsMode='{0}'",
                    dr["收款方式"] == null ? "" : dr["收款方式"].ToString());
                customer.MakeCollectionsModeId = SqlHelper.GetScalar(sql);

                customer.Remark = dr["备注"] == null ? "" : dr["备注"].ToString();
                customer.Paymentdays = 0;
                customer.PercentageInAdvance = 0;
                //if (dr["账期"] == null || dr["账期"].ToString() == "")
                //{
                //    customer.Paymentdays = 0;

                //}
                //else
                //{
                //    customer.Paymentdays = Convert.ToInt32(dr["账期"].ToString());
                //}
                //if (dr["预付百分比"] == null || dr["预付百分比"].ToString() == "")
                //{

                //    customer.PercentageInAdvance = 1.00;
                //}
                //else
                //{
                //    customer.PercentageInAdvance = Convert.ToDouble(dr["预付百分比"]);
                //}
                customerss.Add(customer);
            }
            bool restult = CustomerInfoManager.BatchAddData(customerss, ref error);
            lbMsg.Text = restult == true ? "导入成功！" : error;
            if (restult)
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入客户信息", "导入成功！");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入客户信息", "导入失败！原因" + error);
                return;
            }

        }
    }
}
