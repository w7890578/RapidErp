using System;
using System.Collections.Generic;
using System.Text;
using Model;
using DAL;
using System.Data;

namespace BLL
{
    public class ProcessTestingTableManager
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
            sql = string.Format(@" select count(*) from ProcessTestingTable where Id='{0}' ", Id);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //<summary>
        //检测是否有该开工单号和客户物料编号
        //</summary>
        //<param name="userNumber"></param>
        //<returns></returns>
        private static bool IsExit(string plannumber,string customerproductnumber,string version,string fileName)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from ProcessTestingTable where ProductionOrderNumber='{0}' and CustomerProductNumber='{1}' and Version='{2}'
 and FileName='{3}'", plannumber, customerproductnumber,version,fileName);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region 删除过程检验
        public static string DeleteMarerialScrapLog(string ids)
        {
            string error = string.Empty;
            string sql = string.Format(@" delete ProcessTestingTable where Id  in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }
        #endregion

        /// <summary>
        /// 添加过程检验
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddProcessTestingTable(Model.ProcessTestingTable processtestingtable, ref string error)
        {
            string sql = string.Empty;
            if (IsExit(processtestingtable.Id))
            {
                error = "已存在该编号！请重新填写编号。";
                return false;
            }
            if (IsExit(processtestingtable.ProductionOrderNumber, processtestingtable.CustomerProductNumber,processtestingtable.Version,processtestingtable.FileName))
            {
                error = "已存在此记录！";
                return false;
            }
            if (string.IsNullOrEmpty(processtestingtable.ProductionOrderNumber) || string.IsNullOrEmpty(processtestingtable.ProductNumber)
                || string.IsNullOrEmpty(processtestingtable.ImportTime) || string.IsNullOrEmpty(processtestingtable.ImportPerson))
            {
                error = "过程检验信息不完整！";
                return false;
            }

           sql = string.Format(@" insert into ProcessTestingTable (Id,ProductionOrderNumber,ProductNumber,CustomerProductNumber,
                ImportTime,ImportPerson,Remark,FileName,ImgUrl,Version) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}') ",
            processtestingtable.Id, processtestingtable.ProductionOrderNumber, processtestingtable.ProductNumber, processtestingtable.CustomerProductNumber,
            processtestingtable.ImportTime, processtestingtable.ImportPerson, processtestingtable.Remark,processtestingtable.FileName,processtestingtable.ImgUrl,processtestingtable.Version);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static ProcessTestingTable ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            ProcessTestingTable processtestingtable = new ProcessTestingTable();
            processtestingtable.Id = dr["Id"] == null ? "" : dr["Id"].ToString();
            processtestingtable.ProductionOrderNumber = dr["ProductionOrderNumber"] == null ? "" : dr["ProductionOrderNumber"].ToString();
            processtestingtable.ProductNumber = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
            processtestingtable.CustomerProductNumber = dr["CustomerProductNumber"] == null ? "" : dr["CustomerProductNumber"].ToString();
            processtestingtable.ImportTime = dr["ImportTime"] == null ? "" : dr["ImportTime"].ToString();
            processtestingtable.ImportPerson = dr["ImportPerson"] == null ? "" : dr["ImportPerson"].ToString();
            processtestingtable.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
            processtestingtable.FileName = dr["FileName"] == null ? "" : dr["FileName"].ToString();
            processtestingtable.ImgUrl = dr["ImgUrl"] == null ? "" : dr["ImgUrl"].ToString();
            processtestingtable.Version = dr["Version"] == null ? "" : dr["Version"].ToString();
            return processtestingtable;
        }
        /// <summary>
        /// 编辑过程检验
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditProcessTestingTable(ProcessTestingTable model, ref string error)
        {
            sql = string.Format(@" update ProcessTestingTable set Remark='{0}',ImgUrl='{2}',FileName='{3}',ImportTime='{4}',ImportPerson='{5}' where Id='{1}' ",

           model.Remark, model.Id, model.ImgUrl,model.FileName,model.ImportTime,model.ImportPerson);
            return SqlHelper.ExecuteSql(sql, ref error);
        }


      

    }
}
