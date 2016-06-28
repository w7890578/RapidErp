using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.StoreroomManager
{
    public partial class EditQty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string key = ToolManager.GetQueryString("Guid");
                string sql = string.Format(@"select Qty  
from HalfProductWarehouseLogDetail where Guid='{0}'", key);
                txtQty.Text = SqlHelper.GetScalar(sql);

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string key = ToolManager.GetQueryString("Guid");
            string sql = string.Format(@" 
update HalfProductWarehouseLogDetail set Qty ={0} where Guid='{1}' ", txtQty.Text, key);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                lbSubmit.Text = "修改失败！原因：" + error;
            }
        }
    }
}
