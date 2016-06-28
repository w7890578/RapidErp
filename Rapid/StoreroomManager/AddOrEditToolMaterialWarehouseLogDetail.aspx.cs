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
    public partial class AddOrEditToolMaterialWarehouseLogDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (!IsPostBack)
            {
                lbWarehouseNumber.Text = ToolManager.GetQueryString("WarehouseNumber");
                Warehouse();
                ControlBindManager.BindSupplier(this.drpGYSID);
            }
        }

        private void Warehouse()
        {
            if (!ToolManager.CheckQueryString("WarehouseNumber"))
            {
               
                Response.Write("没有此出入库编号！");
                Response.End();
                return;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Warehouse();
            string sql = string.Empty;
            string error = string.Empty;
            string warehousenumber = lbWarehouseNumber.Text;
            string materialnumber = txtMaterialNumber.Text;
            string gysid = drpGYSID.SelectedValue;
            string qty = txtQty.Text;
            string remark = txtRemark.Text;
            string suppliermaterialnumber=txtSupplierMaterialNumber.Text;
            sql = string.Format(@"select count(*) from MaterialWarehouseLogDetail 
where  WarehouseNumber='{0}' and DocumentNumber='无' and MaterialNumber='{1}'", warehousenumber, materialnumber);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = "已有此记录，请重新填写！";
                return;
            }
            sql = string.Format(@"insert into MaterialWarehouseLogDetail(WarehouseNumber,DocumentNumber,MaterialNumber,GYSID,SupplierMaterialNumber,Qty,Remark,LeadTime,RowNumber) 
values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','无','无')", warehousenumber, '无', materialnumber, gysid, suppliermaterialnumber, qty, remark);
           bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "添加成功" : "添加失败，原因：" + error;
            if (result)
            {

                Tool.WriteLog(Tool.LogType.Operating, "增加样品入库信息" + warehousenumber, "增加成功");
                return;

            }
            else
            {

                Tool.WriteLog(Tool.LogType.Operating, "增加样品出库信息" + warehousenumber, "增加失败！原因"+error);
                return;
            }
        }
    }
}
