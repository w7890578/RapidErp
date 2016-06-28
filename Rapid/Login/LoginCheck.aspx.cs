using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using BLL;

namespace Rapid.Login
{
    public partial class LoginCheck : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }
        private void LoadPage()
        {

            string userNumber = ToolManager.GetQueryString("userNumber");
            string result = string.Empty;
            try
            {
                string error = string.Empty;
                string sql = string.Format(@"select PASSWORD from PM_USER where STATUS ='启用' and USER_ID ='{0}'", userNumber);
                DataTable dt = SqlHelper.GetTable(sql, ref error);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["PASSWORD"].ToString().Equals(ToolManager.MdshashString(ToolManager.GetQueryString("pwd"))))
                    {
                        result = "1";
                        sql = string.Format(" select * from pm_user where User_id='{0}' ", userNumber);
                        Session["User"] = UserInfoManager.ConvertDataTableToModel(sql);

                        sql = string.Format(@"select (MENU_ID+'|'+FUNCTION_ID) as funcs from PM_USER_FUNC_PERMISSION  where APP_ID='Rapid-Erp' and USER_ID='{0}'", userNumber);
                        List<string> userFuncs = new List<string>();
                        foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
                        {
                            userFuncs.Add(dr["funcs"].ToString());
                        }

                        Session["User_Func"] = userFuncs;

                        Session.Timeout = 60;
                        ToolCode.Tool.WriteLog(Rapid.ToolCode.Tool.LogType.Login, userNumber + "登录Erp", "登录成功");
                    }
                    else
                    {
                        result = "密码错误！";
                        ToolCode.Tool.WriteLog(Rapid.ToolCode.Tool.LogType.Login, userNumber + "登录Erp", result);
                    }
                }
                else
                {
                    result = "该用户不存在或处于禁用状态！";
                    ToolCode.Tool.WriteLog(Rapid.ToolCode.Tool.LogType.Login, userNumber + "登录Erp", result);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                ToolCode.Tool.WriteLog(Rapid.ToolCode.Tool.LogType.Login, userNumber + "登录Erp", result);
            }
            Response.Write(result);
            Response.End();
            return;
        }
    }
}
