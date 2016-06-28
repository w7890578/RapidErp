using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.Index
{
    public partial class top : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loginUser.InnerHtml = ToolCode.Tool.GetUser() == null ? "" : ToolCode.Tool.GetUser().UserName;
            }
        }
    }
}
