using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BLL
{
    public class MaterialCustomerPropertyManager
    {
        private static string error = string.Empty;
        private static string sql = string.Empty;

        public static string Delete(string packagenumber, string ids)
        {
            sql = string.Format(@" delete PackageInfoCustomerProperty where PackageNumber='{0}' and CustomerId in ({1}) ", packagenumber, ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 删除原材料客户属性
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            sql = string.Format(@" delete MaterialCustomerProperty where guid in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

        public static string GetCustomerMarerialNames()
        {
            string MarerialNames = string.Empty;
            string sql = @"
select distinct CustomerMaterialNumber from MaterialCustomerProperty  where isnull (CustomerMaterialNumber,'')!=''";
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MarerialNames += dr[0] + ",";
                }
            }
            return MarerialNames.TrimEnd(',');
        }

        /// <summary>
        /// 获取所有原材料的客户名称
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> GetMarerialCustomerNames()
        {
            Dictionary<string, Dictionary<string, string>> marerialCustomerNames = new Dictionary<string, Dictionary<string, string>>();
            sql = @"
select m.MaterialNumber,m.CustomerId,c.CustomerName from MaterialCustomerProperty m inner join  Customer c on m.CustomerId=c.CustomerId
order by m.MaterialNumber
";
            DataTable dt = SqlHelper.GetTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                if (marerialCustomerNames.ContainsKey(dr["MaterialNumber"].ToString().Trim()))
                {
                    marerialCustomerNames[dr["MaterialNumber"].ToString()].Add(dr["CustomerId"].ToString().Trim(), dr["CustomerName"].ToString().Trim());
                    //marerialCustomerNames[dr["MaterialNumber"].ToString()] += "," + dr["CustomerName"].ToString();
                }
                else
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    temp.Add(dr["CustomerId"].ToString().Trim(), dr["CustomerName"].ToString().Trim());
                    marerialCustomerNames.Add(dr["MaterialNumber"].ToString(), temp);
                }
            }
            return marerialCustomerNames;
        }

        /// <summary>
        /// 根据客户物料编号和客户Id获取原材料编号
        /// </summary>
        /// <param name="customerMaterialNumber"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public static string GetMaterialNumber(string customerMaterialNumber, string customerId)
        {
            sql = string.Format(@"select top 1 MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}' and CustomerId='{1}' ", customerMaterialNumber, customerId);
            return SqlHelper.GetScalar(sql);
        }

        /// <summary>
        /// 根据客户物料编号和订单Id获取原材料编号
        /// </summary>
        /// <param name="customerMaterialNumber"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public static string GetMaterialNumberByOderNumberAndCustomerMN(string customerMaterialNumber, string saleOderNumber)
        {
            string customerId = SaleOderManager.GetCustomerIdByOrdersNumber(saleOderNumber);
            sql = string.Format(@"select top 1 MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}' and CustomerId='{1}' ", customerMaterialNumber, customerId);
            return SqlHelper.GetScalar(sql);
        }
    }
}