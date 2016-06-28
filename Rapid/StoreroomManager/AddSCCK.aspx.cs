using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.StoreroomManager
{
    public partial class AddSCCK : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string planNumber = txtPlanNumber.Text.Trim();
            string materialNumber = txtMaterialNumber.Text.Trim();
            string customerProductNumber = txtCustomerMaterilNumber.Text.Trim();
            string qty = txtQty.Text.Trim();
            string remark = txtRemark.Text.Trim();

            string sql = string.Format(@"
select count(0) from    ProductPlanSubDetail  where  PlanNumber='{0}' ", planNumber);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                this.lbSubmit.Text = "开工单号不存在，请重新输入";
                return;
            }

            sql = string.Format(@"
select count(0) from MaterialCustomerProperty where MaterialNumber='{0}' and CustomerMaterialNumber='{1}'
", materialNumber, customerProductNumber);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                this.lbSubmit.Text = "原材料编号或客户物料编号不存在，请重新输入";
                return;
            }

            sql = string.Format(@"
select count(0) from MaterialWarehouseLogDetail where WarehouseNumber='{0}' and DocumentNumber='{1}'
and MaterialNumber='{2}'
", Request["WarehouseNumber"], planNumber, materialNumber);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                this.lbSubmit.Text = "原材料编号已存在，请勿重新输入";
                return;
            }

            sql = string.Format(@" 
insert into  MaterialWarehouseLogDetail (WarehouseNumber,DocumentNumber,MaterialNumber,RowNumber,LeadTime,
CreateTime,CustomerMaterialNumber
,Qty,InventoryQty,Remark,Guid,IsGeneratingCope,CompleteQty)
values('{0}','{1}','{2}','1','1',getdate(),'{3}',{4},0,'{5}',NEWID(),0,0)"
, Request["WarehouseNumber"], planNumber, materialNumber, customerProductNumber, qty, remark);

            string error = string.Empty;
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
            }
            else
            {
                this.lbSubmit.Text = "添加失败！原因：" + error;
                return;
            }
        }
    }
}