using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class MareriaTypeList : System.Web.UI.Page
    {
        public string sql = string.Empty;
        public string error = string.Empty;
        public static string hasEdit = "inline";
        public static string hasDelete = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0303", "Add");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0303", "Edit");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0303", "Delete"); 
                DeleteType();
            }
        }

        private void DeleteType()
        {
            //if (ToolManager.CheckQueryString("Id"))
            //{
            //    string id = ToolManager.GetQueryString("Id");
            //    sql = string.Format("delete MareriaType where id='" + id + "'");
            //    bool result = SqlHelper.ExecuteSql(sql, ref error);
            //    if (result)
            //    {
            //        Tool.WriteLog(Tool.LogType.Operating, "删除原材料类型" + ToolManager.ReplaceSingleQuotesToBlank(id), "删除成功");
            //        Response.Write("1");
            //        Response.End();
            //        return;
            //    }
            //    else
            //    {
            //        Tool.WriteLog(Tool.LogType.Operating, "删除原材料类型" + ToolManager.ReplaceSingleQuotesToBlank(id), "删除失败！原因" + error);
            //        Response.Write(error);
            //        Response.End();
            //        return;
            //    }
            //}
            sql = string.Format(@"select type as 原材料类别 from  MareriaType ");
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DeleteType();
        }

    }
}
