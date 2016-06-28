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
    public partial class SampleCRKDetail : System.Web.UI.Page
    {
        public static string type = string.Empty;
        public static string warehouseNumber = string.Empty;
        public static string checkStatus = "inline"; //审核状态
        public static string sumQty = string.Empty;


        public static string showDocumentNumber = "inline";
        public static string showRowNumber = "inline";
        public static string showLeadTime = "inline";
        public static string showSupplNumber = "inline";
        public static string showCustomerNumber = "inline";


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("key"))
                {
                    string error = string.Empty;
                    string guid = ToolManager.GetQueryString("key").Trim();
                    string sql = string.Format(@" 
 delete MaterialWarehouseLogDetail where Guid ='{0}'", guid);
                    Response.Write(SqlHelper.ExecuteSql(sql, ref error) ? "1" : error);
                    Response.End();
                    return;
                }

                if (ToolManager.CheckQueryString("WarehouseNumber"))
                {
                    warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                    type = Server.UrlDecode(ToolManager.GetQueryString("type")).Trim();
                    if (type.Equals("样品入库"))
                    {
                        showDocumentNumber = "none";
                        showRowNumber = "none";
                        showLeadTime = "none";
                        showSupplNumber = "inline";
                        showCustomerNumber = "none";
                    }
                    else
                    {
                        showDocumentNumber = "inline";
                        showRowNumber = "inline";
                        showLeadTime = "inline";
                        showSupplNumber = "none";
                        showCustomerNumber = "inline";
                    }

                    string sql = string.Format(@" 
 select ISNULL (CheckTime ,'') from MarerialWarehouseLog  where WarehouseNumber='{0}' ", warehouseNumber);
                    checkStatus = string.IsNullOrEmpty(SqlHelper.GetScalar(sql)) ? "inline" : "none";
                }
                else
                {
                    Response.Write("未知的出入库编号！");
                    Response.End();
                    return;
                }
                Bind();
            }
        }

        private void Bind()
        {
            string conditon = " where 出入库编号='" + warehouseNumber + "'";
            if (txtMaterialNumber.Text != "")
            {
                conditon += " and 原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select * from V_SampleCRKDetail {0}", conditon);
            sumQty = SqlHelper.GetScalar(string.Format(" select sum(t.数量) from ({0}) t ", sql));
            //string kcQty = SqlHelper.GetScalar(string.Format(" select sum(t.库存数量) from ({0}) t ", sql));
            //            sql += string.Format(@"union 
            //select '','','合计','','','','','',{0},{1},'',''", qty, kcQty);
            //            sql = string.Format(" select * from ({0}) h order by 原材料编号 asc ", sql);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string userName = ToolCode.Tool.GetUser().UserName;
            string error = string.Empty;
            warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            type = Server.UrlDecode(ToolManager.GetQueryString("type")).Trim();
            bool result = StoreroomToolManager.SHYPCRK(warehouseNumber, type, userName, ref error);
            if (result)
            {
                Response.Redirect("SampleCRK.aspx");
            }
            else
            {
                lbMsg.Text = "审核失败！原因：" + error;
            }
        }
    }
}
