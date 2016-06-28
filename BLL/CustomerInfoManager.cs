using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Model;
using System.Data;

namespace BLL
{
    /// <summary>
    /// 客户信息管理
    /// </summary>
    public class CustomerInfoManager
    {
        private static string error = string.Empty;
        private static string sql = string.Empty;

        /// <summary>
        /// 检测是否有该客户编号
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        private static bool IsExit(string CustomerId)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from Customer where CustomerId='{0}' ", CustomerId);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 删除客户
        public static string DeleteCustomer(string ids)
        {
            string error = string.Empty;
            string sql = string.Format(@" delete Customer where CustomerId  in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }
        #endregion


        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddCustomer(Model.Customer customer, ref string error)
        {
            if (IsExit(customer.CustomerId))
            {
                error = "已存在该客户编号！请重新填写客户编号。";
                return false;
            }

            if (string.IsNullOrEmpty(customer.CustomerId) || string.IsNullOrEmpty(customer.CustomerName))
            {
                error = "客户信息不完整！";
                return false;
            }

            string sql = string.Format(@" insert into Customer (CustomerId,CustomerName,RegisteredAddress,
LegalPerson,Contacts,RegisteredPhone,ContactTelephone,Fax,
MobilePhone,ZipCode,SparePhone,Email,QQ,AccountBank,SortCode,BankAccount,TaxNo,WebsiteAddress,
DeliveryAddress,Paymentdays,PercentageInAdvance,Remark,factoryaddress,MakeCollectionsModeId,ReceiveType) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',
'{18}','{19}','{20}','{21}','{22}','{23}','{24}') ", customer.CustomerId, customer.CustomerName, customer.RegisteredAddress, customer.LegalPerson, 
                                     customer.Contacts, customer.RegisteredPhone, customer.ContactTelephone, customer.Fax, 
                                     customer.MobilePhone, customer.ZipCode, customer.SparePhone, customer.Email, customer.QQ, 
                                     customer.AccountBank, customer.SortCode,
                               customer.BankAccount, customer.TaxNo, customer.WebsiteAddress, customer.DeliveryAddress, 
                               customer.Paymentdays, customer.PercentageInAdvance, customer.Remark,customer.FactoryAddress,customer.MakeCollectionsModeId,customer.ReceiveType);
            return SqlHelper.ExecuteSql(sql, ref error);

        }
        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static Customer ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            Customer customer = new Customer();
            customer.CustomerId = dr["CustomerId"] == null ? "" : dr["CustomerId"].ToString();
            customer.CustomerName = dr["CustomerName"] == null ? "" : dr["CustomerName"].ToString();
            customer.RegisteredAddress = dr["RegisteredAddress"] == null ? "" : dr["RegisteredAddress"].ToString(); ;
            customer.LegalPerson = dr["LegalPerson"] == null ? "" : dr["LegalPerson"].ToString();
            customer.Contacts = dr["Contacts"] == null ? "" : dr["Contacts"].ToString();
            customer.RegisteredPhone = dr["RegisteredPhone"] == null ? "" : dr["RegisteredPhone"].ToString();
            customer.ContactTelephone = dr["ContactTelephone"] == null ? "" : dr["ContactTelephone"].ToString();
            customer.Fax = dr["Fax"] == null ? "" : dr["Fax"].ToString();
            customer.MobilePhone = dr["MobilePhone"] == null ? "" : dr["MobilePhone"].ToString();
            customer.ZipCode = dr["ZipCode"] == null ? "" : dr["ZipCode"].ToString();
            customer.SparePhone = dr["SparePhone"] == null ? "" : dr["SparePhone"].ToString();
            customer.Email = dr["Email"] == null ? "" : dr["Email"].ToString();
            customer.QQ = dr["QQ"] == null ? "" : dr["QQ"].ToString();
            customer.AccountBank = dr["AccountBank"] == null ? "" : dr["AccountBank"].ToString();
            customer.SortCode = dr["SortCode"] == null ? "" : dr["SortCode"].ToString();
            customer.BankAccount = dr["BankAccount"] == null ? "" : dr["BankAccount"].ToString();
            customer.TaxNo = dr["TaxNo"] == null ? "" : dr["TaxNo"].ToString();
            customer.WebsiteAddress = dr["WebsiteAddress"] == null ? "" : dr["WebsiteAddress"].ToString();
            customer.DeliveryAddress = dr["DeliveryAddress"] == null ? "" : dr["DeliveryAddress"].ToString();
            customer.Paymentdays = dr["Paymentdays"] == null ? 0 : Convert.ToInt32(dr["Paymentdays"].ToString());
            customer.PercentageInAdvance = dr["PercentageInAdvance"] == null ? 0.00 : Convert.ToDouble(dr["PercentageInAdvance"].ToString());
            customer.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
            customer.FactoryAddress = dr["FactoryAddress"] == null ? "" : dr["FactoryAddress"].ToString();
            customer.MakeCollectionsModeId = dr["MakeCollectionsModeId"] == null ? "" : dr["MakeCollectionsModeId"].ToString();
            customer.ReceiveType = dr["ReceiveType"] == null ? "" : dr["ReceiveType"].ToString();
            return customer;
        }
        /// <summary>
        /// 编辑客户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditCustomer(Customer customer, ref string error)
        {
            sql = string.Format(@" update Customer set  CustomerName ='{0}',RegisteredAddress ='{1}',LegalPerson ='{2}',Contacts ='{3}',RegisteredPhone ='{4}',
ContactTelephone='{5}',Fax ='{6}',MobilePhone ='{7}',ZipCode ='{8}',SparePhone='{9}',Email='{10}',QQ='{11}',AccountBank='{12}',
SortCode='{13}',BankAccount='{14}',TaxNo='{15}',WebsiteAddress='{16}',DeliveryAddress='{17}',Remark='{18}',Paymentdays='{19}',
PercentageInAdvance='{20}',factoryaddress='{21}',MakeCollectionsModeId='{22}',ReceiveType='{24}' where CustomerId ='{23}'",
   customer.CustomerName, customer.RegisteredAddress, customer.LegalPerson, customer.Contacts, customer.RegisteredPhone, 
   customer.ContactTelephone, customer.Fax, customer.MobilePhone, customer.ZipCode, customer.SparePhone, customer.Email, 
   customer.QQ, customer.AccountBank, customer.SortCode, customer.BankAccount, customer.TaxNo, customer.WebsiteAddress, 
   customer.DeliveryAddress, customer.Remark, customer.Paymentdays, customer.PercentageInAdvance,customer.FactoryAddress,
   customer.MakeCollectionsModeId,customer.CustomerId,customer.ReceiveType);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="users"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BatchAddData(List<Customer> customers, ref string error)
        {
            int i = 0;
            string tempError = string.Empty;
            if (customers.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (Customer customer in customers)
            {
                tempError = "";
                if (!AddCustomer(customer, ref tempError))
                {
                    i++;
                    error += string.Format("客户{0}&nbsp;&nbsp;添加失败：原因--{1}<br/>", customer.CustomerName, tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (customers.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }

    }
}
