using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.Index
{
    public partial class AjaxSystemExit : System.Web.UI.Page
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
            if (ToolManager.CheckQueryString("time"))
            {
                ToolCode.Tool.WriteLog(Rapid.ToolCode.Tool.LogType.Login, "退出ERP系统", "退出成功");
                Session["User"] = null;
                Response.Write("1");
                Response.End();
                return;
            }
        }

    }
}
