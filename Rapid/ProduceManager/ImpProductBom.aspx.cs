using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class ImpProductBom : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //lbMsg.Text = "正在导入.....";
            bool result = false;
            string error = string.Empty;
            string type = drpType.SelectedValue;
            try
            {
                if (type.Equals("0"))//基本信息
                {
                    result = ProductManager.ImpProduct(FU_Excel, Server, ref error);
                }
                else if (type.Equals("1"))//2级BOM
                {
                    result = ProductManager.ImpProductTwoBOM(FU_Excel, Server, ref error); 
                }
                else if (type.Equals("2"))//3级BOM
                {
                    result = ProductManager.ImpProductThreeBOM(FU_Excel, Server, ref error);
                 
                }
                else //导入工序工时
                {
                    result = ProductManager.ImpProductWorkSN(FU_Excel, Server, ref error);
                }
                lbMsg.Text = result ? "导入成功！" : "导入失败<br/>" + error;
                //更新成本价
                SqlHelper.ExecuteSql(" exec P_UpdateProductCostPrice", ref error);
            }
            catch (Exception ex)
            {
                lbMsg.Text = "请检查导入类型和所选的excel模板是否一致！" + ex.Message;
                return;
            }

        }
    }
}
