using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Model;
using DAL;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class AddOrEditWarehouseInfo : System.Web.UI.Page
    {
        public static string warehouseName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string WarehouseNumber = Server.UrlDecode(ToolManager.GetQueryString("WarehouseNumber"));
                string error = string.Empty;
                string sql = string.Format(" select * from WarehouseInfo where WarehouseNumber='{0}' ", WarehouseNumber);
                if (!ToolManager.CheckQueryString("WarehouseNumber"))
                {
                    this.btnSubmit.Text = "添加";
                    this.lbWarehouseNumber.Visible = false;
                }
                else
                {
                    this.btnSubmit.Text = "修改";
                    this.txtWarehouseNumber.Visible = false;
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        lbWarehouseNumber.Text = dr["WarehouseNumber"] == null ? "" : dr["WarehouseNumber"].ToString();
                        txtWarehouseName.Text = dr["WarehouseName"] == null ? "" : dr["WarehouseName"].ToString();
                        txtPosition.Text = dr["Position"] == null ? "" : dr["Position"].ToString();
                        drpType.SelectedValue = dr["Type"] == null ? "" : dr["Type"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        this.btnSubmit.Text = "修改";
                    }
                }

            }
           

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string sql = string.Empty;
            string WarehouseNumber = this.txtWarehouseNumber.Text.Trim();
            string WarehouseName = this.txtWarehouseName.Text.Trim();
            string Position = this.txtPosition.Text.Trim();
            string Type = this.drpType.SelectedValue;
            string Remark = txtRemark.Text.Trim();
            bool result = false;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from WarehouseInfo  where WarehouseName='{0}'", WarehouseName);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该仓库名称！请重新填写！";
                    return;
                }
                sql = string.Format(@" select * from WarehouseInfo  where WarehouseNumber='{0}'", WarehouseNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该编号！请重新填写！";
                    return;
                }
                sql = string.Format(@" insert into WarehouseInfo (WarehouseNumber,WarehouseName,Position,Type,Remark )
 values('{0}','{1}','{2}','{3}','{4}')", WarehouseNumber, WarehouseName, Position, Type, Remark);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败！原因是:" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加仓库信息" + WarehouseNumber, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加仓库信息" + WarehouseNumber, "增加失败！原因"+error);
                    return;
                }

            }
            else
            {
                 
                sql = string.Format(@" update WarehouseInfo set  WarehouseName ='{0}',Position='{1}',Type='{2}',Remark ='{3}'
 where WarehouseNumber='{4}'", WarehouseName, Position, Type, Remark, this.lbWarehouseNumber.Text.Trim());
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败，原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑仓库信息" + this.lbWarehouseNumber.Text, "编辑成功");
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑仓库信息" + this.lbWarehouseNumber.Text, "编辑失败！原因"+error);
                    return;
                }
            }
        }
    }
}
