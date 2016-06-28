using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class ScarpWarehouseLogDetail : System.Web.UI.Page
    {
        public static string warehousenumber = string.Empty;
        public static string checkStatus = string.Empty;
        public static string sumQty = string.Empty;
        public static string changeDirection = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("WarehouseNumber"))
                {
                    Response.Write("未知的出入库编号！");
                    Response.End();
                    return;
                }
                warehousenumber = ToolManager.GetQueryString("WarehouseNumber");
                string sql = string.Format("  select  CheckTime   from ScarpWarehouseLog where WarehouseNumber='{0}' ", warehousenumber);
                checkStatus = string.IsNullOrEmpty(SqlHelper.GetScalar(sql)) ? "inline" : "none";
                sql = string.Format(@"  
 select ChangeDirection from ScarpWarehouseLog where WarehouseNumber='{0}'", warehousenumber);
                changeDirection = SqlHelper.GetScalar(sql);
                if (ToolManager.CheckQueryString("MaterialNumber"))
                {
                    Delete();
                    return;
                }
                Bind();
            }
        }

        private void Bind()
        {
            string sql = string.Empty;
            string error = string.Empty;
            warehousenumber = ToolManager.GetQueryString("WarehouseNumber");
            sql = string.Format(@"select * from V_ScarpWarehouseLogDetail where 出入库编号='{0}'", warehousenumber);
            sumQty = SqlHelper.GetScalar(string.Format(" select sum(t.数量) from ({0}) t ", sql));
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }
        private void Delete()
        {
            warehousenumber = ToolManager.GetQueryString("WarehouseNumber");
            if (ToolManager.CheckQueryString("MaterialNumber"))
            {
                string error = string.Empty;
                string materialnumber = ToolManager.GetQueryString("MaterialNumber");
                string sql = string.Format("delete ScarpWarehouseLogDetail where WarehouseNumber='{0}' and MaterialNumber ='{1}'", warehousenumber, materialnumber);
                if (SqlHelper.ExecuteSql(sql, ref error))
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除采购单明细" + warehousenumber, "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除采购单明细" + warehousenumber, "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string userName = ToolCode.Tool.GetUser().UserName;
            warehousenumber = ToolManager.GetQueryString("WarehouseNumber");
            bool result = StoreroomToolManager.FPCRKCheck(warehousenumber, userId, userName, ref error);
            if (result)
            {
                Response.Redirect("ScarpWarehouseLogList.aspx");
            }
            else
            {
                lbMsg.Text = "审核失败！原因：" + error;
                return;
            }
        }
    }
}
