using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.PurchaseManager
{
    public partial class ImpCertificateOrdersNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string userId = ToolCode.Tool.GetUser().UserNumber;
            bool result = BLL.PurchaseManager.ImpCertificateOrdersList_New(FU_Excel, userId, Server, ref error);
            if (result)
            {
                lbMsg.Text = "导入成功";
            }
            else
            {
                lbMsg.Text = "导入失败！原因：" + error;
            }
        }
    }
}
