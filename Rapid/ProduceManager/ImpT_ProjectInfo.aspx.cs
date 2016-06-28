using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Model;
using Rapid.ToolCode;
using DAL;


namespace Rapid.ProduceManager
{
    public partial class ImpT_ProjectInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            bool result =T_ProjectInfoManager.ImpProjectInfo(FU_Excel, Server, ref error);
            if (result == true)
            {
                lbMsg.Text = "导入成功";
                Tool.WriteLog(Tool.LogType.Operating, "导入项目信息", "导入成功！");
                return;
            }
            else
            {
                lbMsg.Text = error;
                Tool.WriteLog(Tool.LogType.Operating, "导入项目信息", "导入失败！原因" + error);
                return;
            }
        }
    }
}
