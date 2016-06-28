using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Threading;
using BLL;

namespace Rapid.Index
{
    public partial class Welcome : System.Web.UI.Page
    {
        public static string userId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            userId = ToolCode.Tool.GetUser().UserNumber;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Thread.Sleep(3000); 
            //;
            //lbResult.Text = ToolManager.ZDJC() ? "系统数据修复成功！" : "修复失败";
            lbResult.Text = "系统已停止此功能，请联系管理员！";
        }
    }
}
