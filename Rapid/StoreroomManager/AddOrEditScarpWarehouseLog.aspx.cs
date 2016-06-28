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
    public partial class AddOrEditScarpWarehouseLog : System.Web.UI.Page
    {
        public static string warehousenumber= string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string WarehouseNumber = Server.UrlDecode(ToolManager.GetQueryString("WarehouseNumber"));
                string error = string.Empty;
                string sql = string.Format(@" select * from ScarpWarehouseLog where WarehouseNumber='{0}' ", WarehouseNumber);
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
                        warehousenumber = dr["WarehouseNumber"] == null ? "" : dr["WarehouseNumber"].ToString();
                        this.drpChangeDirection.SelectedValue = dr["ChangeDirection"] == null ? "" : dr["ChangeDirection"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        this.btnSubmit.Text = "修改";
                    }

                }

            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string WarehouseNumber = warehousenumber;
            string ChangeDirection = this.drpChangeDirection.SelectedValue;
            string CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string Remark = txtRemark.Text.Trim();
            string WarehouseName = string.Empty;
            string sql = string.Empty;
            string Creator = ToolCode.Tool.GetUser().UserNumber;
            bool result = false;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                WarehouseNumber = "FPCRK" + DateTime.Now.ToString("yyyyMMddHHmmss");
                WarehouseName = "fpk";
                sql = string.Format(@" select * from ScarpWarehouseLog where WarehouseNumber='{0}' ", WarehouseNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该出入库编号！请重新填写！";
                    return;
                }
                sql = string.Format(@" insert into ScarpWarehouseLog (WarehouseNumber,WarehouseName,ChangeDirection,Creator,CreateTime,Remark )
 values('{0}','{1}','{2}','{3}','{4}','{5}')", WarehouseNumber, WarehouseName, ChangeDirection, Creator, CreateTime, Remark);
                result = SqlHelper.ExecuteSql(sql, ref error); 
                lbSubmit.Text =result== true ? "添加成功" : "添加失败，原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加废品出入库信息" + WarehouseNumber, "增加成功");
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加废品出入库信息" + WarehouseNumber, "增加失败！原因"+error);
                    return;
                }

            }
            else
            {

                sql = string.Format(@" select * from ScarpWarehouseLog where WarehouseNumber='{0}' ", Server.UrlDecode(ToolManager.GetQueryString("WarehouseNumber")));
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该废品出入库主表信息已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" update ScarpWarehouseLog set ChangeDirection='{0}',
                Remark ='{1}'
                where WarehouseNumber='{2}' ",ChangeDirection,Remark, WarehouseNumber);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result ==  true ? "修改成功" : "修改失败，原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑废品出入库信息" + WarehouseNumber, "编辑成功");
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑废品出入库信息" + WarehouseNumber, "编辑失败！原因"+error);
                    return;
                }

            }
        }
    }
}
