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
    public partial class AddProductInReturn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbWarehouseNumber.Text = Request["WarehouseNumber"];
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string warehouseNumber = Request["WarehouseNumber"];
            string orderNumber = txtOrderNumber.Text.Trim();

            List<string> sqls = new List<string>();
            string error = string.Empty;

            string sql = string.Format(@"
select count(*) from ProductWarehouseLogDetail where WarehouseNumber='{0}' and DocumentNumber in ('{1}')
", warehouseNumber, orderNumber);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = "该订单已进行过出入库操作";
                return;
            }
            sql = string.Format(@"insert into ProductWarehouseLogDetail
(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,CustomerProductNumber ,Qty,InventoryQty,RowNumber)
select '{0}',OdersNumber ,ProductNumber ,Version  ,CustomerProductNumber,Qty ,0,'0'   from MachineOderDetail
where OdersNumber  in ('{1}')
", warehouseNumber, orderNumber);
            sqls.Add(sql);
            bool restult = SqlHelper.BatchExecuteSql(sqls, ref error);
            if (restult)
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加退货入库信息", "增加成功");
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加退货入库信息", "增加失败！原因" + error);
                lbSubmit.Text = "增加失败！原因" + error;
                return;
            }
        }
    }
}