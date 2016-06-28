using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class EditWorkOrderQty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Id"))
                {
                    string userId = ToolCode.Tool.GetUser().UserNumber;
                    string id = ToolManager.GetQueryString("Id");
                    string sql = string.Format(@"select Qty  from T_WorkOrder_Temp 
where Id='{0}' and User_id='{1}'", id, userId);
                    txtQty.Text = SqlHelper.GetScalar(sql);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string id = ToolManager.GetQueryString("Id");
            string needQty = ToolManager.GetQueryString("Qty");
            if (Convert.ToInt32(txtQty.Text) > Convert.ToInt32(needQty))
            {
                lbSubmit.Text = "不能大于需要生产数量";
                return;
            }
            string sql = string.Format(@" update T_WorkOrder_Temp 
set Qty={2} where Id ='{0}' and User_id='{1}'", id, userId, txtQty.Text);
            string error = string.Empty;
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            if (result)
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                lbSubmit.Text = "编辑失败！原因：" + error;
                return;
            }
        }
    }
}
