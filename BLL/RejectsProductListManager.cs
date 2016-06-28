using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;
using System.Web.UI.WebControls;

namespace BLL
{
   public class RejectsProductListManager
    {
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


        #region 产品编号（不合格品上报表）
        public static string GetProductNumber()
        {
            return GetOption(" select distinct ProductNumber from RejectsProduct ", "ProductNumber", "ProductNumber");
        }
        #endregion 

        #region 版本（不合格品上报表）
        public static string GetVersion()
        {
            return GetOption(" select distinct Version from RejectsProduct ", "Version", "Version");
        }
        #endregion 

        #region 客户产成品编号（不合格品上报表）
        public static string GetCustomerProductNumber()
        {
            return GetOption(" select distinct CustomerProductNumber from RejectsProduct ", "CustomerProductNumber", "CustomerProductNumber");
        }
        #endregion 

        #region 姓名（不合格品上报表）
        public static string GetName()
        {
            return GetOption(" select distinct Name from RejectsProduct ", "Name", "Name");
        }
        #endregion 

        #region 客户名称
        public static string GetCustomerName()
        {
            return GetOption(" select distinct c.CustomerName as CustomerName  from RejectsProduct rp left join Customer c on rp.CustomerId=c.CustomerId ", "CustomerName", "CustomerName");
        }
        #endregion 

       
    }
}
