using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rapid.ToolCode;
using DAL;
using BLL;
using System.Data;

namespace Rapid.SellManager
{
    public partial class AddOrEditMaterialSaleOderWarehouseOut : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.trCheckTime.Visible = false;
                this.trCreateTime.Visible = false;
                this.trAuditor.Visible = false;
                this.trWarehouseNumber.Visible = false;
                string WarehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                if (!ToolManager.CheckQueryString("IsXS"))
                {
                    this.btnSubmit.Text = "添加";
                    this.lbWarehouseNumber.Visible = false;
                    trWarehouseNumber.Visible = false;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string WarehouseName = "ycl";
            string ChangeDirection = this.drpChangeDirection.SelectedValue;
            string Type = this.drpType.SelectedValue;
            string Creator = ToolCode.Tool.GetUser().UserNumber;
            string CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string CheckTime = this.txtCheckTime.Text.Trim();
            string Auditor = this.txtAuditor.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string sql = string.Empty;
            string isconfim = "否";
            bool result = false;
            string WarehouseNumber = ChangeDirection.Equals("出库") ? "YCLCK" + DateTime.Now.ToString("yyyyMMddHHmmss") : "YCLRK" + DateTime.Now.ToString("yyyyMMddHHmmss");
            sql = string.Format(@" insert into MarerialWarehouseLog (WarehouseNumber,WarehouseName,ChangeDirection,Type,Creator,CreateTime,CheckTime,Auditor,Remark,IsConfirm )
 values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", WarehouseNumber, WarehouseName, ChangeDirection, Type, Creator, CreateTime, CheckTime, Auditor, Remark, isconfim);
            result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "添加成功" : "添加失败，原因：" + error;
            if (result)
            {
                ToolCode.Tool.ResetControl(this.Controls);
                Tool.WriteLog(Tool.LogType.Operating, "增加原材料出入库信息" + WarehouseNumber, "增加成功");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加原材料出入库信息" + WarehouseNumber, "增加失败！原因" + error);
                return;
            }

        }
    }
}
