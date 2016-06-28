using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;


namespace BLL
{
    public class ScarpWarehouseLogManager
    {
        private static string sql = string.Empty;
        private static string error = string.Empty;

        //删除废品出入库
        public static string DeleteData(string ids)
        {
            error = string.Empty;
            List<string> sqls = new List<string>();
            string guid = string.Format(@" select guid from ScarpWarehouseLogDetail where WarehouseNumber in ({0})", ids);
            string sqlOne = string.Format(@" delete ScarpWarehouseLogDetail where guid in ({0})", guid);

            sql = string.Format(@" delete ScarpWarehouseLog where WarehouseNumber in ({0}) ", ids);
            sqls.Add(sqlOne);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error) == true ? "1" : error;
        }
        #region 绑定仓库名称
        public static void BindWarehouseName(DropDownList drp)
        {
            string error = string.Empty;
            string sql = " select WarehouseNumber,WarehouseName  from WarehouseInfo";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            drp.DataSource = dt;
            drp.DataValueField = "WarehouseNumber";
            drp.DataTextField = "WarehouseName";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));
        }
        #endregion
        //审核
        public static string check(string check)
        {
            //string Auditor = ToolCode.Tool.GetUser().UserName;
            string Auditor = "Admin";
            string CheckTime = DateTime.Now.ToString();
            error = string.Empty;
            string sql = string.Format(@" update ScarpWarehouseLog set Auditor='{0}' , CheckTime='{1}' where WarehouseNumber in ({2})",
                Auditor, CheckTime, check);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
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


        #region  出入库编号
        public static string GetWarehouseNumber()
        {
            return GetOption(" select  distinct (WarehouseNumber) from ScarpWarehouseLog ", "WarehouseNumber", "WarehouseNumber");
        }
        #endregion

        #region  仓库名称
        public static string GetWarehouseName()
        {
            return GetOption(" select distinct wi.WarehouseName as WarehouseName from ScarpWarehouseLog sw left join WarehouseInfo wi on sw.WarehouseName=wi.WarehouseNumber ", "WarehouseName", "WarehouseName");
        }
        #endregion

        #region  变动方向
        public static string GetChangeDirection()
        {
            return GetOption(" select  distinct (ChangeDirection) from ScarpWarehouseLog ", "ChangeDirection", "ChangeDirection");
        }
        #endregion
    }
}
