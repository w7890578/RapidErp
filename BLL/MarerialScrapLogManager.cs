using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Model;
using System.Data;

namespace BLL
{
    public class MarerialScrapLogManager
    {
        private static string error = string.Empty;
        private static string sql = string.Empty;

        //<summary>
        //检测是否有该编号
        //</summary>
        //<param name="userNumber"></param>
        //<returns></returns>
        private static bool IsExit(string Id)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from MarerialScrapLog where Id='{0}' ", Id);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 删除原材料报废上报表
        public static string DeleteMarerialScrapLog(string ids)
        {
            string error = string.Empty;
            string sql = string.Format(@" delete MarerialScrapLog where Id  in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }
        #endregion


        /// <summary>
        /// 添加原材料报废
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddMarerialScrapLog(Model.MarerialScrapLog marerialscraplog, ref string error)
        {
            if (IsExit(marerialscraplog.Id))
            {
                error = "已存在该编号！请重新填写编号。";
                return false;
            }
            if (string.IsNullOrEmpty(marerialscraplog.CreateTime) || string.IsNullOrEmpty(marerialscraplog.MaterialNumber)
                || string.IsNullOrEmpty(marerialscraplog.ProductNumber) || string.IsNullOrEmpty(marerialscraplog.ScrapDate)
                || string.IsNullOrEmpty(marerialscraplog.Team) || string.IsNullOrEmpty(marerialscraplog.Count)
                || string.IsNullOrEmpty(marerialscraplog.ResponsiblePerson) || string.IsNullOrEmpty(marerialscraplog.ScrapReason))
            {
                error = "原材料报废上报信息不完整！";
                return false;
            }
            string sql = string.Format(@" insert into MarerialScrapLog (Id,CreateTime,ProductNumber,MaterialNumber,
                ScrapDate,Team,Count,ResponsiblePerson,ScrapReason,Remark) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ",
            marerialscraplog.Id, marerialscraplog.CreateTime, marerialscraplog.ProductNumber, marerialscraplog.MaterialNumber,
            marerialscraplog.ScrapDate, marerialscraplog.Team, marerialscraplog.Count, marerialscraplog.ResponsiblePerson,
            marerialscraplog.ScrapReason, marerialscraplog.Remark);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static MarerialScrapLog ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            MarerialScrapLog marerialscraplog = new MarerialScrapLog();
            marerialscraplog.Id = dr["Id"] == null ? "" : dr["Id"].ToString();
            marerialscraplog.CreateTime = dr["CreateTime"] == null ? "" : dr["CreateTime"].ToString();
            marerialscraplog.ProductNumber = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
            marerialscraplog.MaterialNumber = dr["MaterialNumber"] == null ? "" : dr["MaterialNumber"].ToString();
            marerialscraplog.ScrapDate = dr["ScrapDate"] == null ? "" : dr["ScrapDate"].ToString();
            marerialscraplog.Team = dr["Team"] == null ? "" : dr["Team"].ToString();
            marerialscraplog.Count = dr["Count"] == null ? "" : dr["Count"].ToString();
            marerialscraplog.ResponsiblePerson = dr["ResponsiblePerson"] == null ? "" : dr["ResponsiblePerson"].ToString();
            marerialscraplog.ScrapReason = dr["ScrapReason"] == null ? "" : dr["ScrapReason"].ToString();
            marerialscraplog.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
            return marerialscraplog;
        }
        /// <summary>
        /// 编辑原材料报废上报表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditMarerialScrapLog(MarerialScrapLog marerialscraplog, ref string error)
        {
            sql = string.Format(@" update MarerialScrapLog set  CreateTime ='{0}',ProductNumber ='{1}',ScrapDate ='{2}',Team ='{3}',Count ='{4}',
                ResponsiblePerson='{5}',ScrapReason ='{6}',Remark ='{7}',MaterialNumber ='{8}' where Id='{9}' ",
            marerialscraplog.CreateTime, marerialscraplog.ProductNumber, marerialscraplog.ScrapDate, marerialscraplog.Team,
            marerialscraplog.Count, marerialscraplog.ResponsiblePerson, marerialscraplog.ScrapReason, marerialscraplog.Remark, marerialscraplog.MaterialNumber, marerialscraplog.Id);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

    }
}

