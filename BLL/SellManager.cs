using System;
using System.Collections.Generic;
using System.Text;
using DAL;

namespace BLL
{
    /// <summary>
    /// 销售管理类
    /// </summary>
    public class SellManager
    {
        /// <summary>
        /// 根据客户产成品编号找出最新版本号
        /// </summary>
        /// <param name="customerProductNumber"></param>
        /// <returns></returns>
        public static string GetMostNewVersion(string customerProductNumber)
        {
            string version = string.Empty;
            //先从不是字母的里面找【就是全是数字的】
            string sql = string.Format(@"   SELECT top 1 Version  FROM ProductCustomerProperty pcp  
WHERE pcp.Version  COLLATE Chinese_PRC_CS_AS   
not like '%[ABCDEFGHIJKLMNOPQRSTUVWXYZ]%'  and CustomerProductNumber ='{0}' and Version !='WU' order by Version desc
   ", customerProductNumber);
            version = SqlHelper.GetScalar(sql);
            if (string.IsNullOrEmpty(version)) //如果没有数字的就从字母里面找
            {
                sql = string.Format(@"   
  SELECT top 1 Version  FROM ProductCustomerProperty pcp  
WHERE pcp.Version  COLLATE Chinese_PRC_CS_AS   
 like '%[ABCDEFGHIJKLMNOPQRSTUVWXYZ]%'  and CustomerProductNumber ='{0}' and Version !='WU' order by Version desc 
  ", customerProductNumber);
                version = SqlHelper.GetScalar(sql);
                if (string.IsNullOrEmpty(version)) //字母和数字都没有就是“WU”
                {
                    version = "WU";
                } 
            }
            return version; 
        }
    }
}
