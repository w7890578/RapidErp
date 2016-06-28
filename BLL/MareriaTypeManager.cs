using System;
using System.Collections.Generic;
using System.Text;
using Model;
using DAL;
using System.Data;

namespace BLL
{
    public class MareriaTypeManager
    {
        //类内全局变量
        private static string sql = string.Empty;
        private static string error = string.Empty;
        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static MareriaType ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            MareriaType mareriatype = new MareriaType();
            mareriatype.Type = dr["Type"] == null ? "" : dr["Type"].ToString();
            mareriatype.Id = dr["Id"] == null ? "" : dr["Id"].ToString();
            return mareriatype;

        }
        /// <summary>
        /// 检测是否有该类型
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public static bool IsExit(string Type, string Pid)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from MareriaType where Type='{0}' and Pid='{1}'", Type, Pid);
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
        /// 检测是否有该编号
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public static bool IsExitId(string Id)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from MareriaType where Id='{0}' ", Id);
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
        /// 添加原材料类别
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddMareriaType(Model.MareriaType mareriatype, ref string error)
        {
            if (string.IsNullOrEmpty(mareriatype.Type))
            {
                error = "原材料类别信息不完整！";
                return false;
            }
            string sql = string.Format(@" insert into MareriaType (Type) values ('{0}')", mareriatype.Type);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 删除原材料类别
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            sql = string.Format(@" delete MareriaType where type in ('{0}') ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 编辑原材料类别
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditMareriaType(MareriaType mareriatype,string type, ref string error)
        {
            sql = string.Format(@" update MareriaType set Type='{0}' where Type='{1}' ", mareriatype.Type, type);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 查询原材料类别
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectMareriaType()
        {
            sql = " select mt.Id as Id,mt.Type as Type from MareriaType mt inner join MarerialKind mk on mt.Pid=mk.Id";
            return SqlHelper.GetTableToSqlserver(sql, ref error);
        }

    }
}
