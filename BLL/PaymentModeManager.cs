using System;
using System.Collections.Generic;
using System.Text;
using Model;
using DAL;
using System.Data;

namespace BLL
{
    public class PaymentModeManager
    {
        //类内全局变量
        private static string sql = string.Empty;
        private static string error = string.Empty;
        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static PaymentModes ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            PaymentModes paymentmode = new PaymentModes();
            paymentmode.Id = dr["Id"] == null ? "" : dr["Id"].ToString();
            paymentmode.PaymentMode = dr["PaymentMode"] == null ? "" : dr["PaymentMode"].ToString();
            return paymentmode;

        }
        /// <summary>
        /// 检测是否有该编号
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        private static bool IsExit(string Id)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from PaymentMode where Id='{0}' ", Id);
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
        /// 添加付款方式
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddPaymentMode(Model.PaymentModes paymentmode, ref string error)
        {
            if (IsExit(paymentmode.Id))
            {
                error = "已存在该编号！请重新填写编号。";
                return false;
            }

            if (string.IsNullOrEmpty(paymentmode.Id) || string.IsNullOrEmpty(paymentmode.PaymentMode))
            {
                error = "付款方式信息不完整！";
                return false;
            }
            string sql = string.Format(@" insert into PaymentMode (Id,PaymentMode) values ('{0}','{1}')", paymentmode.Id, paymentmode.PaymentMode);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 删除付款方式
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            sql = string.Format(@" delete PaymentMode where Id in ('{0}') ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 编辑付款方式
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditPaymentMode(PaymentModes paymentmode, ref string error)
        {
            sql = string.Format(@" update PaymentMode set PaymentMode='{0}' where Id='{1}'", paymentmode.PaymentMode, paymentmode.Id);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 查询付款方式
        /// </summary>
        /// <returns></returns>
        public static DataTable SelectPaymentMode()
        {
            sql = " select * from PaymentMode ";
            return SqlHelper.GetTableToSqlserver(sql, ref error);
        }
        /// <summary>
        /// 检查是否含有该付款方式
        /// </summary>
        /// <param name="modeName"></param>
        /// <returns></returns>
        public static bool CheckHave(string modeName)
        {
            sql = string.Format(@" select * from PaymentMode where PaymentMode='{0}'", modeName);
            return SqlHelper.GetTable(sql, ref error).Rows.Count >= 1 ? true : false;
        }
    }
}
