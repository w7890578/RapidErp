using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class EditProductWarehouseCurrentAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("guid"))
                {
                    Response.Write("未知的记录！");
                    Response.End();
                    return;
                }
                string guid = ToolManager.GetQueryString("guid");
                string sql = string.Format("select * from ProductWarehouseCurrentAccount where guid='{0}'", guid);
                DataTable dt = SqlHelper.GetTable(sql);
                if(dt.Rows.Count>0)
                {
                    txtRemark.Text=dt.Rows[0]["Remark"].ToString();
                }
              
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string guid = ToolManager.GetQueryString("guid");
            string remark = txtRemark.Text;
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@"update ProductWarehouseCurrentAccount set Remark='{0}' where guid='{1}'", remark, guid);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                lbSubmit.Text = "修改成功";
                return;
            }
            else
            {
                lbSubmit.Text = "修改失败！原因："+error;
                return;
            }
        }
    }
}
