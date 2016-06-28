using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class QuoteInfoList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string temp = QutoInfoManager.Delete(ToolManager.GetQueryString("ids"));
                    bool result = temp == "1" ? true : false;
                    string ids = ToolManager.GetQueryString("ids");
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除报价单信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除成功");
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除报价单信息" + ToolManager.ReplaceSingleQuotesToBlank(ids), "删除失败！原因" + temp);
                        Response.Write(temp);
                        Response.End();
                        return;
                    }
                }
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {

                    ToolCode.Tool.GetPage("AddOrEditQuoteInfo.aspx", "TransferForQuoteInfo", "btnSearch", "290", "600");
                }
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0101", "Add");
                divDelete.Visible = ToolCode.Tool.GetUserMenuFunc("L0101", "Delete");
                divExp.Visible = ToolCode.Tool.GetUserMenuFunc("L0101", "Exp");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0101", "Edit");
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            string sql = saveInfo.Value;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "报价单列表");

        }
    }
}
