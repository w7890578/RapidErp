using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.SellManager
{
    public partial class ImpQuteListory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string userId = ToolCode.Tool.GetUser().UserName ;
            bool result = false;
            if (drpType.SelectedValue.Equals("0"))
            {
                result = QutoInfoManager.BacthAddQuoteInfoMachineForListory(FU_Excel, Server, userId, ref error);
            }
            else
            {
                result = QutoInfoManager.BacthAddQuoteInfoTradingForListory (FU_Excel, Server, userId, ref error);
            }
            if (result)
            {
                lbMsg.Text = "导入成功";
            }
            else
            {
                lbMsg.Text = "导入失败：原因<br/>" + error;
            }
        }
    }
}
