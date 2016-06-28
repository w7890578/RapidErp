using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI;

namespace BLL
{
    public class ProductPlan
    {
        public static string check(string check)
        {
            //string Auditor = ToolCode.Tool.GetUser().UserName;
            string Auditor = "Admin";
            string CheckTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string error = string.Empty;
            string sql = string.Format(@" update ProductPlan set Auditor='{0}' , CheckTime='{1}' where PlanNumber in ({2})",
                Auditor, CheckTime, check);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }
    }
}
