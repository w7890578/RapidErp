using BLL;
using DAL;
using Rapid.ToolCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.StoreroomManager
{
    public partial class AddBackFeeding : System.Web.UI.Page
    {
        public static string titleName = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }
        private void LoadPage()
        {
            if (!ToolManager.CheckQueryString("WarehouseNumber"))
            {
                Response.Write("未知出入库单");
                Response.End();
                return;
            }
            lbWarehouseNumber.Text = ToolManager.GetQueryString("WarehouseNumber");
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            titleName = type;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string materilaNumber = string.Empty;
            string number = txtNumber.Text.Trim();
            string customerMaterialNumber = string.Empty;
            string supplierMaterialNumber = string.Empty;
            string warehouseNumber = lbWarehouseNumber.Text;
            string KGNumber = txtKGNumber.Text.Trim();
            string qty = txtQty.Text.Trim();
            string error = string.Empty;

            string sql = string.Format(@"
select top 1 isnull( MaterialNumber,'') from MaterialCustomerProperty where CustomerMaterialNumber='{0}'
", number);
            materilaNumber = SqlHelper.GetScalar(sql);


            if (string.IsNullOrEmpty(materilaNumber))
            {
                sql = string.Format(@"
select top 1 isnull( MaterialNumber,'') from MaterialSupplierProperty where SupplierMaterialNumber='{0}'
", number);
                materilaNumber = SqlHelper.GetScalar(sql);
                supplierMaterialNumber = number;
            }
            else
            {
                customerMaterialNumber = number;
            }
            if (string.IsNullOrEmpty(materilaNumber))
            {
                lbSubmit.Text = "客户物料编号或供应商物料编号不存在";
                return;
            }

            sql = string.Format(@"
select count(0) from MaterialWarehouseLogDetail where WarehouseNumber='{0}' and MaterialNumber='{1}'"
                , warehouseNumber, materilaNumber);
            if (SqlHelper.GetScalar(sql) != "0")
            {
                lbSubmit.Text = "改物料编号已存在当前生产退料入库单内！";
                return;
            }

            if (!string.IsNullOrEmpty(KGNumber))
            {
                sql = string.Format(@"
select COUNT(0) from ProductPlan where PlanNumber='{0}'
", KGNumber);
                if (SqlHelper.GetScalar(sql) == "0")
                {
                    lbSubmit.Text = "开工单号不存在！";
                    return;
                }
            }


            sql = string.Format(@"
insert into MaterialWarehouseLogDetail (WarehouseNumber,DocumentNumber,MaterialNumber,RowNumber,LeadTime,
CreateTime,SupplierMaterialNumber,CustomerMaterialNumber,Qty)
values('{0}','{1}','{2}','','',getdate(),'{3}','{4}',{5})"
, warehouseNumber, KGNumber, materilaNumber, supplierMaterialNumber, customerMaterialNumber, qty);
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "添加成功！" : "添加失败！原因是:" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加" + titleName + "信息" + warehouseNumber, "增加成功");
                //ToolCode.Tool.ResetControl(this.Controls);
                Response.Write(@" <script >
                                    window.close();
                                </script>");
                return;

            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加" + titleName + "信息" + warehouseNumber, "增加失败！原因" + error);
                return;
            }
        }
    }
}