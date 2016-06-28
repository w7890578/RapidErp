using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.ProduceManager
{
    /// <summary>
    /// 通用前台写入session页面
    /// </summary>
    public partial class AjaxWirteSession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string key = ToolManager.GetParamsString("keyName");
                string value = ToolManager.GetParamsString("value");
                Session[key] = value;
            }

        }
    }
}
