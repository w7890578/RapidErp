using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.Ceshi
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request["ajax"]!=null)
            {
                string transefer = "http://www.baidu.com";
                string temp = string.Format(@"<script type='text/javascript' >alert('登录超时，请重新登录！'); window.top.location.href = '{0}'; </script>"
                    , transefer);
                Response.Write(temp);
                Response.End();
            }
        }
    }
}