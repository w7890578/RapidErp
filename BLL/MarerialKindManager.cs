using System;
using System.Collections.Generic;
using System.Text;
using Model;
using DAL;
using System.Data;

namespace BLL
{
    public class MarerialKindManager
    {
        //类内全局变量
        private static string sql = string.Empty;
        private static string error = string.Empty;
        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static MarerialKind ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            MarerialKind mareriakind = new MarerialKind();
          
            mareriakind.Kind = dr["Kind"] == null ? "" : dr["Kind"].ToString();
            return mareriakind;

        }
        /// <summary>
        /// 检测是否有该种类
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public static bool IsExit(string Kind)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from MarerialKind where Kind='{0}' ", Kind);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
      
        /// <summary>
        /// 添加原材料种类
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddMarerialKind(Model.MarerialKind marerialkind, ref string error)
        {
            if (string.IsNullOrEmpty(marerialkind.Kind))
            {
                error = "原材料种类信息不完整！";
                return false;
            }

            string sql = string.Format(@" insert into MarerialKind (IKind) values ('{0}')", marerialkind.Kind);
            return SqlHelper.ExecuteSql(sql, ref error);

        }

        /// <summary>
        /// 删除原材料种类
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            List<string> list = new List<string>();
            string sql = string.Format(@" delete MarerialKind where Id in ('{0}') ", ids);
            list.Add(sql);
            return SqlHelper.BatchExecuteSql(list, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 编辑原材料种类
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditMarerialKind(MarerialKind mareialkind,string kind, ref string error)
        {
            sql = string.Format(@" update MarerialKind set Kind='{0}' where Kind='{1}' ", mareialkind.Kind, kind);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 查询原材料种类
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectMarerialKind()
        {
            sql = " select * from MarerialKind ";
            return SqlHelper.GetTableToSqlserver(sql, ref error);
        }
    }
}
