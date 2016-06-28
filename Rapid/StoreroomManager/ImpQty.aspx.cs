using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
namespace Rapid.StoreroomManager
{
    public partial class ImpQty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            bool result = false;
            string error = string.Empty;
            string type = drpType.SelectedValue;
            try
            {
                if (type.Equals("0"))//原材料库存数量导入
                {
                    result = StoreroomToolManager.ImpMaterialQty(FU_Excel, Server, ref error);
                }
                else if (type.Equals("1"))//产品库存数量
                {
                    result = StoreroomToolManager.ImpProductQty(FU_Excel, Server, ref error);
                }
                lbMsg.Text = result ? "导入成功！" : "导入失败<br/>" + error;
            }
            catch (Exception ex)
            {
                lbMsg.Text = "请检查导入类型和所选的excel模板是否一致！" + ex.Message;
                return;
            }

        }
    }
}
