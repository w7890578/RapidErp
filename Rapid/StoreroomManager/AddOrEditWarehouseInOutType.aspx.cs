using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class AddOrEditWarehouseInOutType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string WarehouseInOutType = Server.UrlDecode(ToolManager.GetQueryString("WarehouseInOutType"));
                string ChangeDirection = Server.UrlDecode(ToolManager.GetQueryString("ChangeDirection"));
                string InOutType = Server.UrlDecode(ToolManager.GetQueryString("InOutType"));


                string error = string.Empty;
                string sql = string.Format(@" select * from WarehouseInOutType where WarehouseInOutType='{0}' and ChangeDirection='{1}' and InOutType='{2}'", WarehouseInOutType, ChangeDirection, InOutType);
                if (!ToolManager.CheckQueryString("InOutType"))
                {
                    this.btnSubmit.Text = "添加";
                }
                else
                {
                    this.btnSubmit.Text = "修改";
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        this.drpWarehouseInOutType.SelectedValue = dr["WarehouseInOutType"] == null ? "" : dr["WarehouseInOutType"].ToString();
                        this.drpChangeDirection.SelectedValue = dr["ChangeDirection"] == null ? "" : dr["ChangeDirection"].ToString();

                        txtInOutType.Text = dr["InOutType"] == null ? "" : dr["InOutType"].ToString();
                        this.btnSubmit.Text = "修改";
                    }

                }

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string WarehouseInOutType = this.drpWarehouseInOutType.SelectedValue;
            string ChangeDirection = this.drpChangeDirection.SelectedValue;
            string InOutType = txtInOutType.Text.Trim();
            string sql = string.Empty;
            bool result = false;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from WarehouseInOutType where WarehouseInOutType='{0}' and ChangeDirection='{1}' and InOutType='{2}'", WarehouseInOutType, ChangeDirection, InOutType);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该出入库类型！请重新选择！";
                    return;
                }
                sql = string.Format(@" insert into WarehouseInOutType (WarehouseInOutType,ChangeDirection,InOutType )
 values('{0}','{1}','{2}')", WarehouseInOutType, ChangeDirection, InOutType);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败！原因是:" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加出入库类型"+WarehouseInOutType, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加出入库类型" + WarehouseInOutType, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {

                sql = string.Format(@" select * from WarehouseInOutType where WarehouseInOutType='{0}' and ChangeDirection='{1}' and InOutType='{2}'",
                 Server.UrlDecode(ToolManager.GetQueryString("WarehouseInOutType")), Server.UrlDecode(ToolManager.GetQueryString("ChangeDirection")), Server.UrlDecode(ToolManager.GetQueryString("InOutType")));
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该出入库类型已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" update WarehouseInOutType set WarehouseInOutType='{0}',ChangeDirection='{1}',
                InOutType='{2}' where WarehouseInOutType='{3}' and ChangeDirection='{4}' and InOutType='{5}'",
                WarehouseInOutType, ChangeDirection, InOutType, Server.UrlDecode(ToolManager.GetQueryString("WarehouseInOutType")),
                Server.UrlDecode(ToolManager.GetQueryString("ChangeDirection")), Server.UrlDecode(ToolManager.GetQueryString("InOutType")));
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "编辑成功" : "编辑失败，原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑出入库类型" + Server.UrlDecode(ToolManager.GetQueryString("WarehouseInOutType")), "编辑成功");
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑出入库类型" + Server.UrlDecode(ToolManager.GetQueryString("WarehouseInOutType")), "编辑失败！原因" + error);
                    return;
                }
            }
        }
    }
}
