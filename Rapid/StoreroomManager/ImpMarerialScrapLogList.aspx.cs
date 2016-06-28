using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rapid.ToolCode;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class ImpMarerialScrapLogList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            bool result = StoreroomToolManager.ImpMarerialScrapLogList(FU_Excel, Server, ref error);
            
            if (result == true)
            {
                lbMsg.Text = "导入成功";
                Tool.WriteLog(Tool.LogType.Operating, "导入报废信息", "导入成功！");
                return;
            }
            else
            {
                lbMsg.Text = error;
                Tool.WriteLog(Tool.LogType.Operating, "导入报废信息", "导入失败！原因" + error);
                return;
            }
        }
    }
}
