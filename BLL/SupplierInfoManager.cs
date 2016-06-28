using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Model;
using System.Data;

namespace BLL
{
    public class SupplierInfoManager
    {
        private static string error = string.Empty;
        private static string sql = string.Empty;

        public static string GetSupplierNames()
        {
            sql = string.Format(@"
select SupplierName from SupplierInfo");
            DataTable dt = SqlHelper.GetTable(sql);
            string tempStr = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                tempStr += dr[0].ToString() + ",";
            }
            return tempStr.TrimEnd(',');
        }

        /// <summary>
        /// 检测是否有该供应商编号
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        private static bool IsExit(string SupplierId)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from SupplierInfo where SupplierId='{0}' ", SupplierId);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 删除供应商
        public static string DeleteSupplierInfo(string ids)
        {
            string error = string.Empty;
            string sql = string.Format(@" delete SupplierInfo where SupplierId  in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }
        #endregion


        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddSupplierInfo(Model.SupplierInfo supplierinfo, ref string error)
        {
            if (IsExit(supplierinfo.SupplierId))
            {
                error = "已存在该供应商编号！请重新填写供应商编号。";
                return false;
            }

            if (string.IsNullOrEmpty(supplierinfo.SupplierId) || string.IsNullOrEmpty(supplierinfo.SupplierName) || string.IsNullOrEmpty(supplierinfo.PaymentMode))
            {
                error = "供应商信息不完整！";
                return false;
            }

            string sql = string.Format(@" insert into SupplierInfo (SupplierId,SupplierName,RegisteredAddress,
LegalPerson,Contacts,RegisteredPhone,ContactTelephone,Fax,
MobilePhone,ZipCode,SparePhone,Email,QQ,AccountBank,BankRowNumber,BankAccount,TaxNo,WebsiteAddress,
DeliveryAddress,Remark,Paymentdays,PercentageInAdvance,factoryaddress,PayType,PaymentMode) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}',
'{18}','{19}',{20},{21},'{22}','{23}','{24}') ", supplierinfo.SupplierId, supplierinfo.SupplierName, supplierinfo.RegisteredAddress, supplierinfo.LegalPerson, supplierinfo.Contacts, supplierinfo.RegisteredPhone, supplierinfo.ContactTelephone,
            supplierinfo.Fax, supplierinfo.MobilePhone, supplierinfo.ZipCode, supplierinfo.SparePhone, supplierinfo.Email, supplierinfo.QQ,
            supplierinfo.AccountBank, supplierinfo.BankRowNumber, supplierinfo.BankAccount, supplierinfo.TaxNo, supplierinfo.WebsiteAddress,
            supplierinfo.DeliveryAddress, supplierinfo.Remark, supplierinfo.paymentdays, supplierinfo.percentageInAdvance,
            supplierinfo.FactoryAddress, supplierinfo.PayType, supplierinfo.PaymentMode);
            return SqlHelper.ExecuteSql(sql, ref error);

        }
        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static SupplierInfo ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            SupplierInfo supplierinfo = new SupplierInfo();
            supplierinfo.SupplierId = dr["SupplierId"] == null ? "" : dr["SupplierId"].ToString();
            supplierinfo.SupplierName = dr["SupplierName"] == null ? "" : dr["SupplierName"].ToString();
            supplierinfo.RegisteredAddress = dr["RegisteredAddress"] == null ? "" : dr["RegisteredAddress"].ToString();
            supplierinfo.LegalPerson = dr["LegalPerson"] == null ? "" : dr["LegalPerson"].ToString();
            supplierinfo.Contacts = dr["Contacts"] == null ? "" : dr["Contacts"].ToString();
            supplierinfo.RegisteredPhone = dr["RegisteredPhone"] == null ? "" : dr["RegisteredPhone"].ToString();
            supplierinfo.ContactTelephone = dr["ContactTelephone"] == null ? "" : dr["ContactTelephone"].ToString();
            supplierinfo.Fax = dr["Fax"] == null ? "" : dr["Fax"].ToString();
            supplierinfo.MobilePhone = dr["MobilePhone"] == null ? "" : dr["MobilePhone"].ToString();
            supplierinfo.ZipCode = dr["ZipCode"] == null ? "" : dr["ZipCode"].ToString();
            supplierinfo.SparePhone = dr["SparePhone"] == null ? "" : dr["SparePhone"].ToString();
            supplierinfo.Email = dr["Email"] == null ? "" : dr["Email"].ToString();
            supplierinfo.QQ = dr["QQ"] == null ? "" : dr["QQ"].ToString();
            supplierinfo.AccountBank = dr["AccountBank"] == null ? "" : dr["AccountBank"].ToString();
            supplierinfo.BankRowNumber = dr["BankRowNumber"] == null ? "" : dr["BankRowNumber"].ToString();
            supplierinfo.BankAccount = dr["BankAccount"] == null ? "" : dr["BankAccount"].ToString();
            supplierinfo.TaxNo = dr["TaxNo"] == null ? "" : dr["TaxNo"].ToString();
            supplierinfo.WebsiteAddress = dr["WebsiteAddress"] == null ? "" : dr["WebsiteAddress"].ToString();
            supplierinfo.DeliveryAddress = dr["DeliveryAddress"] == null ? "" : dr["DeliveryAddress"].ToString();
            supplierinfo.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
            supplierinfo.paymentdays = dr["Paymentdays"] == null ? 0 : Convert.ToInt32(dr["Paymentdays"].ToString());
            supplierinfo.percentageInAdvance = dr["PercentageInAdvance"] == null ? 0.00 : Convert.ToDouble(dr["PercentageInAdvance"].ToString());
            supplierinfo.FactoryAddress = dr["FactoryAddress"] == null ? "" : dr["FactoryAddress"].ToString();
            supplierinfo.PayType = dr["PayType"] == null ? "" : dr["PayType"].ToString();
            supplierinfo.PaymentMode = dr["PaymentMode"] == null ? "" : dr["PaymentMode"].ToString();
            return supplierinfo;
        }
        /// <summary>
        /// 编辑供应商
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditSupplierInfo(SupplierInfo supplierinfo, ref string error)
        {
            sql = string.Format(@" update SupplierInfo set  SupplierName ='{0}',RegisteredAddress ='{1}',LegalPerson ='{2}',Contacts ='{3}',
RegisteredPhone ='{4}',ContactTelephone='{5}',Fax ='{6}',MobilePhone ='{7}',ZipCode ='{8}',SparePhone='{9}',Email='{10}',QQ='{11}',
AccountBank='{12}',BankRowNumber='{13}',BankAccount='{14}',TaxNo='{15}',WebsiteAddress='{16}',DeliveryAddress='{17}',Remark='{18}',
Paymentdays={19},PercentageInAdvance={20},FactoryAddress='{21}',PayType='{23}',PaymentMode='{24}' where SupplierId ='{22}' ",
   supplierinfo.SupplierName, supplierinfo.RegisteredAddress, supplierinfo.LegalPerson, supplierinfo.Contacts, supplierinfo.RegisteredPhone,
   supplierinfo.ContactTelephone, supplierinfo.Fax, supplierinfo.MobilePhone, supplierinfo.ZipCode, supplierinfo.SparePhone,
   supplierinfo.Email, supplierinfo.QQ, supplierinfo.AccountBank, supplierinfo.BankRowNumber, supplierinfo.BankAccount,
   supplierinfo.TaxNo, supplierinfo.WebsiteAddress, supplierinfo.DeliveryAddress, supplierinfo.Remark, supplierinfo.paymentdays,
   supplierinfo.percentageInAdvance, supplierinfo.FactoryAddress, supplierinfo.SupplierId, supplierinfo.PayType, supplierinfo.PaymentMode);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="users"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool BatchAddData(List<SupplierInfo> supplierinfos, ref string error)
        {
            int i = 0;
            string tempError = string.Empty;
            if (supplierinfos.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (SupplierInfo supplierinfo in supplierinfos)
            {
                tempError = "";
                if (!AddSupplierInfo(supplierinfo, ref tempError))
                {
                    i++;
                    error += string.Format("&nbsp;添加失败：原因--{0}<br/>", tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (supplierinfos.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }

    }
}
