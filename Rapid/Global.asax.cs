using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Rapid
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            Application.Add("online", 0);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            if ((HttpContext.Current.Session["User"] == null && (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != "~/Login/LoginCheck.aspx")))
            {
                 ToolCode.Tool.LoginTimeout();
            }
            //Application.Lock();
            //int iNum = Int32.Parse(Application["online"].ToString()) + 1;
            //Application.Set("online", iNum);                  //修改对象的值，为自身加1
            //Application.UnLock();
        }
        protected void Session_End(object sender, EventArgs e)
        {
            //Application.Lock();
            //int iNum = Int32.Parse(Application["online"].ToString()) - 1;
            //Application.Set("online", iNum);
            //Application.UnLock();
            //if ((HttpContext.Current.Session["User"] == null && (HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != "~/Login/LoginCheck.aspx")))
            //{
            //    ToolCode.Tool.LoginTimeout();
            //}
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
           
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }


        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}