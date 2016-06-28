using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;
using System.Web.UI.WebControls;

namespace BLL
{
    public class WarehouseInfoManager
    {
        private static string sql = string.Empty;
        private static string error = string.Empty;
        /// <summary>
        /// 下拉框通用获取内容函数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="keyCloumName">key字段名</param>
        /// <param name="valueCloumName">value字段名</param>
        /// <returns></returns>
        public static string GetOption(string sql, string keyCloumName, string valueCloumName)
        {
            string error = string.Empty;
            string result = "<option value =''>- - - - - 请 选 择 - - - - -</option>";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            foreach (DataRow dr in dt.Rows)
            {
                result += string.Format(" <option value ='{0}'>{1}</option> ", dr[keyCloumName], dr[valueCloumName]);
            }
            return result;
        }

        //获取仓库名称WarehouseName
        public static string GetWarehouseName()
        {
            return GetOption(" select  distinct (WarehouseName) from WarehouseInfo ", "WarehouseName", "WarehouseName");
        }
        //获取仓库位置
        public static string GetPosition()
        {
            return GetOption(" select  distinct (Position) from WarehouseInfo ", "Position", "Position");
        }
        //获取仓库类型
        public static string GetType()
        {
            return GetOption(" select  distinct (Type) from WarehouseInfo ", "Type", "Type");
        }
        //删除仓库信息
        public static string DeleteData(string ids)
        {
            sql = string.Format(@" delete WarehouseInfo where WarehouseNumber in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }
    }
}
