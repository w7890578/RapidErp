using System;
using System.Collections.Generic;
using System.Text;
using Model;
using DAL;
using System.Data;
using System.Web.UI.WebControls;

namespace BLL
{
   public class PerformanceReviewLogManager
    {
       public static string sql = string.Empty;
       public static string error = string.Empty;
       /// DataTable 转对象
       /// </summary>
       /// <param name="sql"></param>
       /// <returns></returns>
       public static PerformanceReviewLog ConvertDataTableToModel(string sql)
       {

           DataTable dt = SqlHelper.GetTable(sql);
           DataRow dr = dt.Rows[0];
           PerformanceReviewLog performancereviewlog = new PerformanceReviewLog();
           performancereviewlog.Year = dr["Year"] == null ? "" : dr["Year"].ToString();
           performancereviewlog.Month = dr["Month"] == null ? "" : dr["Month"].ToString();
           performancereviewlog.PerformanceReviewItem = dr["PerformanceReviewItem"] == null ? "" : dr["PerformanceReviewItem"].ToString();
           performancereviewlog.RowNumber = Convert.ToInt32(dr["RowNumber"] == null ? "" : dr["RowNumber"].ToString());
           performancereviewlog.FullScore = Convert.ToInt32(dr["FullScore"] == null ? "" : dr["FullScore"].ToString());
           performancereviewlog.Deduction = Convert.ToInt32(dr["Deduction"] == null ? "" : dr["Deduction"].ToString());
           performancereviewlog.Score = Convert.ToInt32(dr["Score"] == null ? "" : dr["Score"].ToString());
           performancereviewlog.Description = dr["Description"] == null ? "" : dr["Description"].ToString();
           performancereviewlog.StatMode = Convert.ToInt32(dr["StatMode"] == null ? "" : dr["StatMode"].ToString());
           performancereviewlog.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
           performancereviewlog.Name = dr["Name"] == null ? "" : dr["Name"].ToString();
           return performancereviewlog;

       }

       /// <summary>
       /// 检测是否有该编号
       /// </summary>
       /// <param name="userNumber"></param>
       /// <returns></returns>
       private static bool IsExit(string Year,string Month, string PerformanceReviewItem,string Name)
       {
           error = string.Empty;
           sql = string.Format(" select count(*) from PerformanceReviewLog where Year='{0}' and Month='{1}' and PerformanceReviewItem='{2}' and Name='{3}'", Year,Month,PerformanceReviewItem,Name);
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
       /// 添加员工绩效上报表
       /// </summary>
       /// <param name="error"></param>
       /// <returns></returns>
       public static bool AddPerformanceReviewLogManager(Model.PerformanceReviewLog performancereviewlog, ref string error)
       {
           if (IsExit(performancereviewlog.Year,performancereviewlog.Month,performancereviewlog.PerformanceReviewItem,performancereviewlog.Name))
           {
               error = "已存在的员工绩效信息，请重新填写！";
               return false;
           }

           if (string.IsNullOrEmpty(performancereviewlog.Year) || string.IsNullOrEmpty(performancereviewlog.Month)
               || string.IsNullOrEmpty(performancereviewlog.PerformanceReviewItem)
               || string.IsNullOrEmpty(performancereviewlog.RowNumber.ToString())|| string.IsNullOrEmpty(performancereviewlog.FullScore.ToString())
               ||string.IsNullOrEmpty(performancereviewlog.Deduction.ToString())||string .IsNullOrEmpty(performancereviewlog.Score.ToString())
               ||string.IsNullOrEmpty(performancereviewlog.Description) || string.IsNullOrEmpty(performancereviewlog.StatMode.ToString())||
               string.IsNullOrEmpty(performancereviewlog.Name.ToString()))
           {
               error = "员工绩效信息不完整！";
               return false;
           }
           string sql = string.Format(@" insert into PerformanceReviewLog (Year,Month,PerformanceReviewItem,RowNumber,FullScore,Deduction,Score,Description,StatMode,Remark,Name) values 
           ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", performancereviewlog.Year,
          performancereviewlog.Month, performancereviewlog.PerformanceReviewItem, performancereviewlog.RowNumber, performancereviewlog.FullScore, performancereviewlog.Deduction,
           performancereviewlog.Score, performancereviewlog.Description, performancereviewlog.StatMode,performancereviewlog.Remark,performancereviewlog.Name);
           return SqlHelper.ExecuteSql(sql, ref error);
       }

       /// <summary>
       /// 修改考员工绩效上报表
       /// </summary>
       /// <param name="error"></param>
       /// <returns></returns>
       public static bool EditPerformanceReviewLogManager(Model.PerformanceReviewLog performancereviewlog, ref string error, string Year, string Month, string PerformanceReviewItem,string Name)
       {
           if (string.IsNullOrEmpty(performancereviewlog.Year) || string.IsNullOrEmpty(performancereviewlog.Month)
               || string.IsNullOrEmpty(performancereviewlog.PerformanceReviewItem)
               || string.IsNullOrEmpty(performancereviewlog.RowNumber.ToString()) || string.IsNullOrEmpty(performancereviewlog.FullScore.ToString())
               || string.IsNullOrEmpty(performancereviewlog.Deduction.ToString()) || string.IsNullOrEmpty(performancereviewlog.Score.ToString())
               ||string.IsNullOrEmpty(performancereviewlog.Name.ToString()))
           {
               error = "员工绩效信息不完整！";
               return false;
           }
           string sql = string.Format(@" update PerformanceReviewLog set RowNumber={0},FullScore={1},Deduction={2},Score={3},Description='{4}',StatMode='{5}',Remark='{6}'  where Year='{7}' and Month='{8}' and PerformanceReviewItem='{9}' and Name='{10}'",
           performancereviewlog.RowNumber, performancereviewlog.FullScore, performancereviewlog.Deduction,
           performancereviewlog.Score, performancereviewlog.Description, performancereviewlog.StatMode, performancereviewlog.Remark,Year,Month,PerformanceReviewItem,performancereviewlog.Name);
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

       #region  年度
       public static string GetYear()
       {
           return GetOption(" select  distinct (Year) from PerformanceReviewLog ", "Year", "Year");
       }
       #endregion

       #region  月份
       public static string GetMonth()
       {
           return GetOption(" select  distinct (Month) from PerformanceReviewLog ", "Month", "Month");
       }
       #endregion

       #region  考核项目
       public static string GetPerformanceReviewItem()
       {
           return GetOption(" select  distinct (PerformanceReviewItem) from PerformanceReviewLog ", "PerformanceReviewItem", "PerformanceReviewItem");
       }
       #endregion

       #region  姓名
       public static string GetName()
       {
           return GetOption(" select  distinct (Name) from PerformanceReviewLog ", "Name", "Name");
       }
       #endregion


       #region 序号
       public static string GetRowNumber()
       {
           return GetOption(" select  distinct (RowNumber) from PerformanceReviewLog ", "RowNumber", "RowNumber");
       }
       #endregion

    }
}
