using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Model;
using System.Data;

namespace BLL
{
    public class UserInfoManager
    {
        private static string error = string.Empty;
        private static string sql = string.Empty;

      

        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static UserInfo ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            UserInfo user = new UserInfo(); 
            user.UserNumber = dr["USER_ID"] == null ? "" : dr["USER_ID"].ToString();
            user.UserName = dr["USER_NAME"] == null ? "" : dr["USER_NAME"].ToString();
            user.DepartmentId = dr["DEPARTMENT_ID"] == null ? "" : dr["DEPARTMENT_ID"].ToString();
            user.RoleId = dr["RoleId"] == null ? "" : dr["RoleId"].ToString();
            user.Description = dr["DESCRIPTION"] == null ? "" : dr["DESCRIPTION"].ToString();
            user.OfficeTelePhone = dr["OFFICE_TELEPHONE"] == null ? "" : dr["OFFICE_TELEPHONE"].ToString();
            user.MobileTelePhone = dr["MOBILE_TELEPHONE"] == null ? "" : dr["MOBILE_TELEPHONE"].ToString();
            user.Pwd = dr["PASSWORD"] == null ? "" : dr["PASSWORD"].ToString();
            user.CreateDate = dr["CREATE_DATE"] == null ? "" : dr["CREATE_DATE"].ToString();
            user.Status = dr["STATUS"] == null ? "" : dr["STATUS"].ToString();
            user.Email = dr["EMAIL"] == null ? "" : dr["EMAIL"].ToString();
            user.Remark = dr["REMARK"] == null ? "" : dr["REMARK"].ToString();
            user.Team = dr["Team"] == null ? "" : dr["Team"].ToString();
            return user;
        }


        ///// <summary>
        ///// 批量添加数据
        ///// </summary>
        ///// <param name="users"></param>
        ///// <param name="error"></param>
        ///// <returns></returns>
        //public static bool BatchAddData(List<UserInfo> users, ref string error)
        //{
        //    int i = 0;
        //    string tempError = string.Empty;
        //    if (users.Count <= 0)
        //    {
        //        error = "没有要添加的数据";
        //        return false;
        //    }
        //    foreach (UserInfo user in users)
        //    {
        //        tempError = "";
        //        if (!AddUser(user, ref tempError))
        //        {
        //            i++;
        //            error += string.Format("用户{0}&nbsp;&nbsp;添加失败：原因--{1}<br/>", user.UserName, tempError);
        //        }
        //    }
        //    bool result = i > 0 ? false : true;
        //    if (!result)
        //    {
        //        error = string.Format("添加成功{0}条，添加失败{1}条。<br/>添加失败用户:<br/>{2}", (users.Count - i).ToString(), i.ToString(), error);
        //    }
        //    return result;
        //}
    }
}
