using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class T_LessMaterialBreakdownDetail : System.Web.UI.Page
    {
        public static string userId = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("isEdit")) //修改欠料数量
                {
                    string qty = ToolManager.GetQueryString("qty");
                    string guid = ToolManager.GetQueryString("guid");
                    string sql = string.Format(@" update T_LessMaterialBreakdown
set LessMaterialQty={0} where guid='{1}'", qty, guid);
                    string error = string.Empty;
                    if (SqlHelper.ExecuteSql(sql, ref error))
                    {
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                if (ToolManager.CheckQueryString("materilaNumber"))//还料
                {
                    string materilaNumber = ToolManager.GetQueryString("materilaNumber");
                    string qty = ToolManager.GetQueryString("qty");
                    string key = ToolManager.GetQueryString("key");
                    //int num = Convert.ToInt32(ToolManager.GetQueryString("Num"));
                    string error = string.Empty;
                    List<string> sqls = new List<string>();
                    string sql = string.Format(@"
update T_LessMaterialBreakdown set LessMaterialQty=LessMaterialQty+({1}) 
where Guid='{0}'", key, ToolManager.GetQueryString("Num"));
                    sqls.Add(sql);
                    //                    string sql = string.Format(@" 
                    //update T_LessMaterialBreakdown set LessMaterialQty =0 where guid='{0}' ", key);
                    //sqls.Add(sql);
                    ////                    sql = string.Format(@"  update MaterialStock set StockQty =StockQty-({0}),UpdateTime ='{1}' 
                    ////where  MaterialNumber ='{2}' and WarehouseName='ycl' ", qty, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), materilaNumber);
                    ////                    sqls.Add(sql);
                    if (SqlHelper.BatchExecuteSql(sqls, ref error))
                    {
                        Response.Write("1");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Response.Write(error);
                        Response.End();
                        return;
                    }
                }
                Bind();
            }
        }

        private void Bind()
        {
            userId = ToolCode.Tool.GetUser().UserNumber;
            string condition = "where vtl.欠料数量<0 ";
            if (txtMaterialNumber.Text != "")
            {
                condition += " and vtl.原材料编号 like '%" + txtMaterialNumber.Text + "%'";
            }
            if (txtCustomerMaterialNumber.Text != "")
            {
                condition += " and vtl.客户物料编号 like '%" + txtCustomerMaterialNumber.Text + "%'";
            }
            if (txtKGNumber.Text.Trim() != "")
            {
                condition += " and vtl.单据编号 like  '%" + txtKGNumber.Text.Trim() + "%'";
            }
            string sql = string.Format(@" select vtl.*,vq.qty as 实时库存数量 from V_MaterialStock_Qty vq inner join V_T_LessMaterialBreakdown vtl on 
 vq.MaterialNumber =vtl.原材料编号 {0} order by vtl.客户物料编号 asc ", condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
