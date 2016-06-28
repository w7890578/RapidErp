using System;
using System.Collections.Generic;
using System.Text;
using DAL;

namespace BLL
{
    public class MaterialBlueprintPropertyManager
    {
        //类内全局变量
        private static string sql = string.Empty;
        private static string error = string.Empty;
        /// <summary>
        /// 删除原材料图纸属性
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            string sql = string.Format(@" delete MaterialBlueprintProperty where guid in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

    }
}
