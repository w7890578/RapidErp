using System;
using System.Collections.Generic;
using System.Text;
using Model;
using DAL;
using System.Data;


namespace BLL
{
    public class WorkSnManager
    {
        //类内全局变量
        private static string sql = string.Empty;
        private static string error = string.Empty;
        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static WorkSn ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            WorkSn worksn = new WorkSn();
            worksn.WorkSnNumber = dr["WorkSnNumber"] == null ? "" : dr["WorkSnNumber"].ToString();
            worksn.WorkSnName = dr["WorkSnName"] == null ? "" : dr["WorkSnName"].ToString();
            worksn.Sn = dr["Sn"] == null ? "0" : dr["Sn"].ToString();
            return worksn;

        }
        /// <summary>
        /// 检测是否有该工序
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public static bool IsExitWorkSnName(string WorkSnName)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from WorkSn where WorkSnName='{0}' ", WorkSnName);
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
        /// 检测是否有该工序编码
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        public static bool IsExitWorkSnNumber(string WorkSnNumber)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from WorkSn where WorkSnNumber='{0}' ", WorkSnNumber);
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
        /// 添加工序
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddWorkSn(Model.WorkSn worksn, ref string error)
        {
            if (string.IsNullOrEmpty(worksn.WorkSnNumber) || string.IsNullOrEmpty(worksn.WorkSnName))
            {
                error = "工序信息不完整！";
                return false;
            }
            if (IsExitWorkSnName(worksn.WorkSnName))
            {
                error = "已有该工序，请重新填写！！！";
                return false;
            }
            if (IsExitWorkSnNumber(worksn.WorkSnNumber))
            {
                error = "已有该工序编码，请重新填写！！！";
                return false;
            }
            string sql = string.Format(@" insert into WorkSn (WorkSnNumber,WorkSnName) values ('{0}','{1}')", worksn.WorkSnNumber, worksn.WorkSnName);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 删除工序
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            sql = string.Format(@" delete WorkSn where WorkSnNumber in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 编辑工序
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditWorkSn(WorkSn worksn, ref string error)
        {
            sql = string.Format(@" update WorkSn set WorkSnName='{0}',sn='{2}' where WorkSnNumber='{1}' ", worksn.WorkSnName, worksn.WorkSnNumber, worksn.Sn);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 查询基础工序
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectWorkSn()
        {
            sql = " select * from WorkSn ";
            return SqlHelper.GetTableToSqlserver(sql, ref error);
        }

    }
}
