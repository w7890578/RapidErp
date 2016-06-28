using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Model;
using DAL;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BLL
{
    public class ExaminationLogManager
    {
        private static string sql = string.Empty;
        private static string error = string.Empty;
        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static ExaminationLog ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            ExaminationLog examinationlog = new ExaminationLog();

            examinationlog.Year = dr["Year"] == null ? "" : dr["Year"].ToString();
            examinationlog.Month = dr["Month"] == null ? "" : dr["Month"].ToString();
            examinationlog.Name = dr["Name"] == null ? "" : dr["Name"].ToString();
            examinationlog.Score = Convert.ToDecimal(dr["Score"] == null ? "" : dr["Score"].ToString());
            examinationlog.LeaderScore = Convert.ToDecimal(dr["LeaderScore"] == null ? "" : dr["LeaderScore"].ToString());
            examinationlog.TotalScore = Convert.ToDecimal(dr["TotalScore"] == null ? "" : dr["TotalScore"].ToString());
            examinationlog.Operation = Convert.ToDecimal(dr["Operation"] == null ? "" : dr["Operation"].ToString());
            examinationlog.WorkAttendance = Convert.ToDecimal(dr["WorkAttendance"] == null ? "" : dr["WorkAttendance"].ToString());
            examinationlog.WorkState = Convert.ToDecimal(dr["WorkState"] == null ? "" : dr["WorkState"].ToString());
            examinationlog.Teamwork = Convert.ToDecimal(dr["Teamwork"] == null ? "" : dr["Teamwork"].ToString());
            examinationlog.RejectsProduct = Convert.ToDecimal(dr["RejectsProduct"] == null ? "" : dr["RejectsProduct"].ToString());
            examinationlog.Security = Convert.ToDecimal(dr["Security"] == null ? "" : dr["Security"].ToString());
            examinationlog.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
            return examinationlog;

        }

        /// <summary>
        /// 添加员工考试成绩
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddExaminationLog(Model.ExaminationLog examinationlog, ref string error)
        {
            if (string.IsNullOrEmpty(examinationlog.Year)
                || string.IsNullOrEmpty(examinationlog.Month) || string.IsNullOrEmpty(examinationlog.Name))
            {
                error = "员工考试成绩信息不完整！";
                return false;
            }
            string sql = string.Format(@" insert into ExaminationLog (Year,Month,Name,Score,LeaderScore,TotalScore,
           Operation,WorkAttendance,WorkState,Teamwork,RejectsProduct,Security,Remark) values 
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",
            examinationlog.Year, examinationlog.Month, examinationlog.Name, examinationlog.Score, examinationlog.LeaderScore,
            examinationlog.TotalScore, examinationlog.Operation, examinationlog.WorkAttendance, examinationlog.WorkState,
            examinationlog.Teamwork, examinationlog.RejectsProduct, examinationlog.Security, examinationlog.Remark);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 删除员工考试成绩信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            string name = HttpContext.Current.Server.UrlDecode(ids);
            sql = string.Format(@" select  user_id from pm_user where user_name in ({0})",name);
            sql = string.Format(@" delete ExaminationLog where name in ({0}) ",sql);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 编辑员工考试成绩信息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditExaminationLog(ExaminationLog examinationlog, ref string error)
        {
            sql = string.Format(@" select  user_id from pm_user where user_name='{0}'", examinationlog.Name);
            string name = SqlHelper.GetScalar(sql);

            sql = string.Format(@" update ExaminationLog set Score='{0}',
            LeaderScore='{1}',TotalScore='{2}',Operation='{3}',WorkAttendance='{4}',WorkState='{5}',
            Teamwork='{6}',RejectsProduct='{7}',Security='{8}',Remark='{9}' where year='{10}' and month='{11}' and name='{12}' ",
            examinationlog.Score, examinationlog.LeaderScore,
            examinationlog.TotalScore, examinationlog.Operation, examinationlog.WorkAttendance, examinationlog.WorkState,
            examinationlog.Teamwork, examinationlog.RejectsProduct, examinationlog.Security, examinationlog.Remark,
            examinationlog.Year, examinationlog.Month, name);
            return SqlHelper.ExecuteSql(sql, ref error);

        }
        /// <summary>
        /// 批量添加数据
        /// </summary>
        /// <param name="users"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        /// 

        public static bool BatchAddData(List<ExaminationLog> pcps, ref string error)
        {
            int i = 0;
            string tempError = string.Empty;
            if (pcps.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (ExaminationLog pcp in pcps)
            {
                tempError = "";
                if (!AddExaminationLog(pcp, ref tempError))
                {
                    i++;
                    error += string.Format("年份{0},月份{1},姓名{2}&nbsp;&nbsp;添加失败：原因--{3}<br/>",
                        pcp.Year, pcp.Month, pcp.Name, tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>添加员工考试成绩上报失败:<br/>{2}", (pcps.Count - i).ToString(), i.ToString(), error);
            }
            return result;
        }

    }
}
