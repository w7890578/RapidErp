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
    public partial class AddOrEditHalfProductWarehouseLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.trCheckTime.Visible = false;
                this.trCreateTime.Visible = false;
                this.trAuditor.Visible = false;
                string WarehouseNumber = Server.UrlDecode(ToolManager.GetQueryString("WarehouseNumber"));
                ScarpWarehouseLogManager.BindWarehouseName(this.drpWarehouseName);
                string error = string.Empty;
                string sql = string.Format(@" select * from HalfProductWarehouseLog where WarehouseNumber='{0}' ", WarehouseNumber);
                if (!ToolManager.CheckQueryString("WarehouseNumber"))
                {
                    this.btnSubmit.Text = "添加";
                }
                else
                {
                    this.btnSubmit.Text = "修改";
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        this.txtWarehouseNumber.Text = dr["WarehouseNumber"] == null ? "" : dr["WarehouseNumber"].ToString();
                        this.drpWarehouseName.SelectedValue = dr["WarehouseName"] == null ? "" : dr["WarehouseName"].ToString();
                        this.drpChangeDirection.SelectedValue = dr["ChangeDirection"] == null ? "" : dr["ChangeDirection"].ToString();
                        this.txtCreator.Text = dr["Creator"] == null ? "" : dr["Creator"].ToString();
                        this.txtCreateTime.Text = dr["CreateTime"] == null ? "" : dr["CreateTime"].ToString();
                        this.txtCheckTime.Text = dr["CheckTime"] == null ? "" : dr["CheckTime"].ToString();
                        this.txtAuditor.Text = dr["Auditor"] == null ? "" : dr["Auditor"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        this.btnSubmit.Text = "修改";
                        this.txtWarehouseNumber.Enabled = false;
                    }

                }

            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string WarehouseNumber = this.txtWarehouseNumber.Text.Trim();
            string WarehouseName = this.drpWarehouseName.SelectedValue;
            string ChangeDirection = this.drpChangeDirection.SelectedValue;
            string Creator = this.txtCreator.Text.Trim();
            string CreateTime = DateTime.Now.ToString();
            string CheckTime = this.txtCheckTime.Text.Trim();
            string Auditor = this.txtAuditor.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string sql = string.Empty;
            bool result = false;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from HalfProductWarehouseLog where WarehouseNumber='{0}' ", WarehouseNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该出入库编号！请重新填写！";
                    return;
                }
                sql = string.Format(@" insert into HalfProductWarehouseLog (WarehouseNumber,WarehouseName,ChangeDirection,Creator,CreateTime,CheckTime,Auditor,Remark )
 values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", WarehouseNumber, WarehouseName, ChangeDirection, Creator, CreateTime, CheckTime, Auditor, Remark);
                result= SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text =result == true ? "添加成功" : "添加失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加半成品出入库信息"+WarehouseNumber, "增加成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加半成品出入库信息" + WarehouseNumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {

                sql = string.Format(@" select * from HalfProductWarehouseLog where WarehouseNumber='{0}' ", Server.UrlDecode(ToolManager.GetQueryString("WarehouseNumber")));
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该半成品出入库主表信息已被删除，请刷新页面后进行添加！";
                    return;
                }
                string warehournumber= Server.UrlDecode(ToolManager.GetQueryString("WarehouseNumber"));
                sql = string.Format(@" update HalfProductWarehouseLog set WarehouseNumber ='{8}',WarehouseName='{1}',ChangeDirection='{2}',
                Creator='{3}',CreateTime='{4}',CheckTime='{5}',Auditor='{6}',Remark ='{7}'
                where WarehouseNumber='{0}' ",warehournumber, WarehouseName, ChangeDirection, Creator, CreateTime, CheckTime, Auditor, Remark, WarehouseNumber);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑半成品出入库信息" + warehournumber, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑半成品出入库信息" + warehournumber, "编辑失败！原因" + error);
                    return;
                }
            }
        }
    }
}
