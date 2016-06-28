using System;
using System.Collections.Generic;
using System.Text;
using Model;
using DAL;
using System.Data;
using System.Web.UI.WebControls;

namespace BLL
{
    public class PerformanceReviewStandardManager
    {
        private static string sql = string.Empty;
        private static string error = string.Empty;
        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static PerformanceReviewStandard ConvertDataTableToModel(string sql)
        {

            DataTable dt = SqlHelper.GetTable(sql);
            DataRow dr = dt.Rows[0];
            PerformanceReviewStandard performancereviewstandard = new PerformanceReviewStandard();
            performancereviewstandard.StandardName = dr["StandardName"] == null ? "" : dr["StandardName"].ToString();
            performancereviewstandard.PerformanceReviewItem = dr["PerformanceReviewItem"] == null ? "" : dr["PerformanceReviewItem"].ToString();
            performancereviewstandard.RowNumber = Convert.ToInt32(dr["RowNumber"] == null ? "" : dr["RowNumber"].ToString());
            performancereviewstandard.FullScore = Convert.ToInt32(dr["FullScore"] == null ? "" : dr["FullScore"].ToString());
            performancereviewstandard.Description = dr["Description"] == null ? "" : dr["Description"].ToString();
            performancereviewstandard.StatMode = Convert.ToInt32(dr["StatMode"] == null ? "" : dr["StatMode"].ToString());
            performancereviewstandard.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
            performancereviewstandard.Type = dr["Type"] == null ? "" : dr["Type"].ToString();
            return performancereviewstandard;

        }


        /// <summary>
        /// 检测是否有该编号
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        private static bool IsExit(string StandardName, string PerformanceReviewItem)
        {
            error = string.Empty;
            sql = string.Format(" select count(*) from PerformanceReviewStandard where StandardName='{0}' and PerformanceReviewItem='{1}' ", StandardName, PerformanceReviewItem);
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
        /// 添加考核心标准维护表
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddPerformanceReviewStandardManager(Model.PerformanceReviewStandard performancereviewstandard, ref string error)
        {
            if (IsExit(performancereviewstandard.StandardName, performancereviewstandard.PerformanceReviewItem))
            {
                error = "已存在的标准考核名称或考核项目，请重新填写！";
                return false;
            }

            if (string.IsNullOrEmpty(performancereviewstandard.StandardName) || string.IsNullOrEmpty(performancereviewstandard.PerformanceReviewItem)
                || string.IsNullOrEmpty(performancereviewstandard.Description) || string.IsNullOrEmpty(performancereviewstandard.FullScore.ToString())
                || string.IsNullOrEmpty(performancereviewstandard.StatMode.ToString()) || string.IsNullOrEmpty(performancereviewstandard.RowNumber.ToString()))
            {
                error = "核心标准维护信息不完整！";
                return false;
            }
            string sql = string.Format(@" insert into PerformanceReviewStandard (StandardName,PerformanceReviewItem,RowNumber,FullScore,Description,StatMode,Remark,Type) values 
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", performancereviewstandard.StandardName,
           performancereviewstandard.PerformanceReviewItem, performancereviewstandard.RowNumber, performancereviewstandard.FullScore, performancereviewstandard.Description, performancereviewstandard.StatMode,
            performancereviewstandard.Remark, performancereviewstandard.Type);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 修改考核心标准维护表
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditPerformanceReviewStandardManager(Model.PerformanceReviewStandard performancereviewstandard, ref string error, string StandardName, string PerformanceReviewItem)
        {
            //if (IsExit(performancereviewstandard.StandardName, performancereviewstandard.PerformanceReviewItem))
            //{
            //    error = "已存在的标准考核名称或考核项目，请重新填写！";
            //    return false;
            //}

            if (string.IsNullOrEmpty(performancereviewstandard.StandardName) || string.IsNullOrEmpty(performancereviewstandard.PerformanceReviewItem)
                || string.IsNullOrEmpty(performancereviewstandard.Description) || string.IsNullOrEmpty(performancereviewstandard.FullScore.ToString())
                || string.IsNullOrEmpty(performancereviewstandard.StatMode.ToString()) || string.IsNullOrEmpty(performancereviewstandard.RowNumber.ToString()))
            {
                error = "核心标准维护信息不完整！";
                return false;
            }
            string sql = string.Format(@" update PerformanceReviewStandard 
set RowNumber={0},FullScore={1},Description='{2}',StatMode={3},Remark='{4}',Type='{7}' 
where StandardName='{5}' and  PerformanceReviewItem='{6}'",
           performancereviewstandard.RowNumber, performancereviewstandard.FullScore, performancereviewstandard.Description, performancereviewstandard.StatMode,
            performancereviewstandard.Remark, StandardName, PerformanceReviewItem,
            performancereviewstandard.Type);
            return SqlHelper.ExecuteSql(sql, ref error);
        }

        #region 下拉框获取内容通用函数
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
            string result = "<option value =\"\">- - - - - 请 选 择 - - - - -</option>";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            foreach (DataRow dr in dt.Rows)
            {
                result += string.Format(" <option value ='{0}'>{1}</option> ", dr[keyCloumName], dr[valueCloumName]);
            }
            return result;
        }

        public static void BindDrp(string sql, DropDownList drp, string valueName, string textName)
        {
            drp.Items.Clear();
            string error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                drp.DataSource = dt;
                drp.DataValueField = valueName;
                drp.DataTextField = textName;
                drp.DataBind();
            }
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));


        }

        #endregion

        #region  考核标准名称
        public static string GetStandardName()
        {
            return GetOption(" select  distinct (StandardName) from PerformanceReviewStandard ", "StandardName", "StandardName");
        }
        #endregion

        #region  考核项目
        public static string GetPerformanceReviewItem()
        {
            return GetOption(" select  distinct (PerformanceReviewItem) from PerformanceReviewStandard ", "PerformanceReviewItem", "PerformanceReviewItem");
        }
        #endregion

        #region 序号
        public static string GetRowNumber()
        {
            return GetOption(" select  distinct (RowNumber) from PerformanceReviewStandard ", "RowNumber", "RowNumber");
        }
        #endregion
    }
}
