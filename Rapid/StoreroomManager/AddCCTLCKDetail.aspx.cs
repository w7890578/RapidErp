using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class AddCCTLCKDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            string cgOrderNumber = txtCGorderNumber.Text.Trim();
            string supplMareitNumber = txtSuppliMateriNumber.Text.Trim();
            string qty = txtQty.Text;
            string remark = txtRemark.Text;
            if (string.IsNullOrEmpty(cgOrderNumber) || string.IsNullOrEmpty(supplMareitNumber)
                || string.IsNullOrEmpty(qty))
            {
                lbSubmit.Text = "信息填写不完整";
                return;
            }

            string error = string.Empty;
            bool result = StoreroomToolManager.AddCCTLCKDetail(warehouseNumber, cgOrderNumber, supplMareitNumber, qty, remark, ref error);

            if (result)
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                lbSubmit.Text = "添加失败！原因：" + error;
            }
        }
    }
}
