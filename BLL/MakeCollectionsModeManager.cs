using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using System.Data;
using Model;

namespace BLL
{
    /// <summary>
    /// 收款方式管理
    /// </summary>
    public static class MakeCollectionsModeManager
    {
        //类内全局变量
        private static string sql = string.Empty;
        private static string error = string.Empty;
        /// <summary>
        /// 删除收款方式
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            sql = string.Format(@" delete MakeCollectionsMode where Id in ('{0}') ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 检查是否含有该收款方式
        /// </summary>
        /// <param name="modeName"></param>
        /// <returns></returns>
        public static bool CheckHave(string modeName)
        {
            sql = string.Format(@" select * from MakeCollectionsMode where MakeCollectionsMode='{0}'", modeName);
            return SqlHelper.GetTable(sql, ref error).Rows.Count >= 1 ? true : false;
        }
        /// <summary>
        /// 添加收款方式
        /// </summary>
        /// <param name="MakeCollectionsMode">收款方式</param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddMakeCollectionsMode(Model.MakeCollectionsModes MakeCollectionsMode, ref string error)
        {
            if (IsExit(MakeCollectionsMode.Id))
            {
                error = "已存在该编号！请重新填写编号。";
                return false;
            }

            if (string.IsNullOrEmpty(MakeCollectionsMode.Id) || string.IsNullOrEmpty(MakeCollectionsMode.MakeCollectionsMode))
            {
                error = "收款方式信息不完整！";
                return false;
            }
            string sql = string.Format(@" insert into MakeCollectionsMode (Id,MakeCollectionsMode) values ('{0}','{1}')", MakeCollectionsMode.Id, MakeCollectionsMode.MakeCollectionsMode);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 检测是否有该编号
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        private static bool IsExit(string Id)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from MakeCollectionsMode where Id='{0}' ", Id);
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
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static MakeCollectionsModes ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            MakeCollectionsModes makecollectionsmode = new MakeCollectionsModes();
            makecollectionsmode.Id = dr["Id"] == null ? "" : dr["Id"].ToString();
            makecollectionsmode.MakeCollectionsMode = dr["MakeCollectionsMode"] == null ? "" : dr["MakeCollectionsMode"].ToString();
            return makecollectionsmode;

        }

        /// <summary>
        /// 编辑收款方式
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditMakeCollectionsMode(MakeCollectionsModes makecollectionsmode, ref string error)
        {
            sql = string.Format(@" update MakeCollectionsMode set MakeCollectionsMode='{0}' where Id='{1}'", makecollectionsmode.MakeCollectionsMode, makecollectionsmode.Id);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 查询收款方式
        /// </summary>
        /// <returns></returns>
        public static DataTable CheckHave()
        {
            sql = " select * from MakeCollectionsMode ";
            return SqlHelper.GetTableToSqlserver(sql, ref error);
        }
    }
}
