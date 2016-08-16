using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BLL
{
    public class ProductCustomerPropertyManager
    {
        private static string error = string.Empty;
        private static string sql = string.Empty;

        /// <summary>
        /// 添加客户产成品基本信息
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddPcp(Model.ProductCustomerProperty pcp, ref string error)
        {
            if (IsExit(pcp.ProductNumber, pcp.Version, pcp.CustomerId))
            {
                error = "已存在该编号、版本、客户编号！请重新填写！";
                return false;
            }
            string sql = string.Format(@" insert into ProductCustomerProperty (ProductNumber,Version,CustomerId,CustomerProductNumber,remark)
             values ('{0}','{1}','{2}','{3}','{4}')", pcp.ProductNumber, pcp.Version, pcp.CustomerId, pcp.CustomerProductNumber, pcp.Remark);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        public static bool BatchAddData(List<ProductCustomerProperty> pcps, ref string error)
        {
            int i = 0;
            string tempError = string.Empty;
            if (pcps.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (ProductCustomerProperty pcp in pcps)
            {
                tempError = "";
                if (!AddPcp(pcp, ref tempError))
                {
                    i++;
                    error += string.Format("客户产成品{0},版本{1},客户产成品编号{2}&nbsp;&nbsp;添加失败：原因--{3}<br/>",
                        pcp.ProductNumber, pcp.Version, pcp.CustomerProductNumber, tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>添加产成品失败:<br/>{2}", (pcps.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }

        /// <summary>
        /// 删除产成品客户属性
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            sql = string.Format(@" delete ProductCustomerProperty where guid in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 获取所有产成品的客户名称
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, string>> GetProductCustomerNames()
        {
            Dictionary<string, Dictionary<string, string>> ProductCustomerNames = new Dictionary<string, Dictionary<string, string>>();
            sql = @"
select p.ProductNumber,p.Version,p.CustomerId,c.CustomerName from ProductCustomerProperty p inner join  Customer c on p.CustomerId=c.CustomerId
order by p.ProductNumber,p.Version
";
            DataTable dt = SqlHelper.GetTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                string key = dr["ProductNumber"] + "|" + dr["Version"];
                if (ProductCustomerNames.ContainsKey(key))
                {
                    ProductCustomerNames[key].Add(dr["CustomerId"].ToString().Trim(), dr["CustomerName"].ToString().Trim());
                    //marerialCustomerNames[dr["MaterialNumber"].ToString()] += "," + dr["CustomerName"].ToString();
                }
                else
                {
                    Dictionary<string, string> temp = new Dictionary<string, string>();
                    temp.Add(dr["CustomerId"].ToString().Trim(), dr["CustomerName"].ToString().Trim());
                    ProductCustomerNames.Add(key, temp);
                }
            }
            return ProductCustomerNames;
        }

        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="users"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        ///
        /// <summary>
        /// 检测是否有该行记录
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public static bool IsExit(string ProductNumber, string Version, string CustomerId)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from ProductCustomerProperty where ProductNumber='{0}' and Version='{1}' and CustomerId='{2}'",
                ProductNumber, Version, CustomerId);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}