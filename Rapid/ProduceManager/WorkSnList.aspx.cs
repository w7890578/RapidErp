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
    public partial class WorkSnList : System.Web.UI.Page
    {
        public string sql = string.Empty;
        public string error = string.Empty;
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                spAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0305", "Add");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0305", "Edit");
                DeleteWorkSn();
            }

        }
        private void DeleteWorkSn()
        {
            //if (ToolManager.CheckQueryString("WorkSnNumber"))
            //{
            //    string WorkSnNumber = ToolManager.GetQueryString("WorkSnNumber");
            //    sql = string.Format("delete WorkSn where WorkSnNumber='" + WorkSnNumber + "'");
            //    if (SqlHelper.ExecuteSql(sql, ref error))
            //    {
            //        Tool.WriteLog(Tool.LogType.Operating, "删除基础工序信息" +ToolManager.ReplaceSingleQuotesToBlank( WorkSnNumber), "删除成功");
            //        Response.Write("1");
            //        Response.End();
            //        return;
            //    }
            //    else
            //    {
            //        Tool.WriteLog(Tool.LogType.Operating, "删除基础工序" + ToolManager.ReplaceSingleQuotesToBlank(WorkSnNumber), "删除成功！原因" + error);
            //        Response.Write(error);
            //        Response.End();
            //        return;
            //    }
            //}
            sql = string.Format(@" select sn as 工序序号, WorkSnNumber as 工序编号,WorkSnName as 工序名称 from WorkSn order by sn asc");
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            DeleteWorkSn();
        }


    }
}
