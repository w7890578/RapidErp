using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Model;

namespace Rapid.StoreroomManager
{
    public partial class ProductWarehouseLogDetail : System.Web.UI.Page
    {
        public static string titleName = string.Empty;
        public static string orderName = string.Empty;
        public static string showDocumentNumber = "inline";
        public static string showReason = "inline";
        public static string showCustomerName = "inline";
        public static string showCustomerProductNumber = "inline";
        public static string number = string.Empty;
        public static string show = "inline";
        public static string showLeadTime = "inline";
        public static string showPlanNumberOrdersNumber = "none";
        public static string showPlanNumberRowNumber = "none";
        public static string hasEdit = "inline";
        public static string hasDelete = "inline";
        public static string showDelete = "inline";
        public static string showOperar = "inline";

        public static string showCustomerOrderNumber = "none";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               // spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0406", "Print");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0406", "Delete");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0406", "Edit");
                if (ToolManager.CheckQueryString("xuan"))//选选择
                {
                    Choose();
                }

                Bind();
                if (ToolManager.CheckQueryString("WarehouseNumber") && ToolManager.CheckQueryString("guid"))
                {
                    List<string> sqls = new List<string>();
                    string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                    string guid = ToolManager.GetQueryString("guid");
                    string sql = string.Format(@"
delete ProductWarehouseLogDetail where WarehouseNumber='{0}'
and  guid='{1}'",
  warehouseNumber, guid);
                    sqls.Add(sql);
                    string error = string.Empty;
                    Response.Write(SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error);
                    Response.End();
                }
            }
        }

        private void Choose()
        {
            string error = string.Empty;
            string guids = ToolManager.GetQueryString("xuan");
            string warehouseNumber = ToolManager.GetQueryString("warehouseNumber");
            string sql = string.Format(@" 
delete ProductWarehouseLogDetail where WarehouseNumber='{0}' and Guid not in ({1}) ", warehouseNumber, guids);
            string result = SqlHelper.ExecuteSql(sql, ref error) ? "1" : error;
            Response.Write(result);
            Response.End();
            return;

        }

        private void Bind()
        { 
            if (ToolManager.CheckQueryString("WarehouseNumber"))
            {
                titleName = Server.UrlDecode(ToolManager.GetQueryString("ChangeDirection"));
                string srotcolumName = titleName.Equals("销售出库") ? "货位" : "客户产成品编号";

                string conditon = " where t.出入库编号='" + ToolManager.GetQueryString("WarehouseNumber") + "'";
                if (txtProductNumber.Text != "")
                {
                    conditon += " and t.产品编号 like '%" + txtProductNumber.Text + "%'";
                }
                if (txtCustomerProductNumber.Text != "")
                {
                    conditon += " and t.客户产成品编号 like '%" + txtCustomerProductNumber.Text + "%'";
                }
                if (txtProductName.Text != "")
                {
                    conditon += " and 产品名称 like '%" + txtProductName.Text.Trim() + "%'";
                }
                if (txtCustomerName.Text != "")
                {
                    conditon += " and 客户名称 like '%" + txtCustomerName.Text.Trim() + "%'";
                }
                number = ToolManager.GetQueryString("WarehouseNumber");
                string sql = string.Format(@"select COUNT (*) 
from ProductWarehouseLog where WarehouseNumber='{0}' and ISNULL (checktime,'')!=''", number);
                show = SqlHelper.GetScalar(sql).Equals("0") ? "inline" : "none";

                sql = string.Format(@"select t.*,c.CustomerName as 客户名称, isnull(vpss.库存数量,0) as 库存数量 from (
select vtp.*,so.CustomerId from  dbo.V_Tool_ProductWarehouseLogDetail vtp left join 
 SaleOder so on vtp.单据编号=so.OdersNumber) t 
 left join  Customer c on t.CustomerId=c.CustomerId
 left join V_ProductStock_Sum vpss on t.产品编号=vpss .ProductNumber and t.版本=vpss.Version
{0} order by t.{1} asc", conditon, srotcolumName);
                rpList.DataSource = SqlHelper.GetTable(sql);
                rpList.DataBind();
               
                hdChangeDirection.Value = ToolManager.GetQueryString("ChangeDirection");
                switch (titleName)
                {
                    case "退货入库":
                        orderName = "销售订单编号";
                        showCustomerName = "none";
                        showCustomerOrderNumber = "none";
                        showOperar = "inline";
                        break;
                    case "包装入库":
                        orderName = "开工单号";
                        showReason = "none";
                        showCustomerName = "none";
                        showCustomerOrderNumber = "none";
                        showOperar = "none";
                        break;
                    case "维修入库":
                        orderName = "维修订单号";
                        showReason = "none";
                        showCustomerName = "none";
                        showCustomerOrderNumber = "none";
                        showOperar = "inline";
                        break;
                    case "销售出库":
                        orderName = "销售订单编号";
                        showReason = "none";
                        showLeadTime = "inline";
                        showPlanNumberRowNumber = "inline";
                        showCustomerOrderNumber = "inline";
                        showOperar = "none";

                        break;
                    case "包装销售出库":
                        orderName = "销售订单编号";
                        showReason = "none";
                        showLeadTime = "inline";
                        showPlanNumberRowNumber = "inline";
                        showCustomerOrderNumber = "inline";
                        showOperar = "inline";
                        break;
                    case "样品出库":
                        orderName = "销售订单编号";
                        showReason = "none";
                        showCustomerOrderNumber = "none";
                        showOperar = "inline";
                        break;
                    case "维修出库":
                        orderName = "维修订单号";
                        showReason = "none";
                        showCustomerOrderNumber = "none";
                        showOperar = "inline";
                        break;
                    case "盘盈入库":
                        orderName = "盘点编号";
                        showReason = "none";
                        showCustomerName = "none";
                        showCustomerProductNumber = "none";
                        showCustomerOrderNumber = "none";
                        break;
                    case "盘亏出库":
                        orderName = "盘点编号";
                        showReason = "none";
                        showCustomerName = "none";
                        showCustomerProductNumber = "none";
                        showCustomerOrderNumber = "none";
                        break;
                    case "报废出库":
                        showDocumentNumber = "none";
                        showCustomerName = "none";
                        showCustomerProductNumber = "none";
                        showCustomerOrderNumber = "none";
                        showOperar = "inline";
                        break;

                    case "样品入库":
                        orderName = "开工单号";
                        showPlanNumberRowNumber = "inline";
                        showPlanNumberOrdersNumber = "inline";
                        showCustomerName = "none";
                        showReason = "none";
                        showCustomerOrderNumber = "none";
                        showOperar = "inline";
                        break;
                    case "生产入库":
                        orderName = "开工单号";
                        showPlanNumberRowNumber = "inline";
                        showPlanNumberOrdersNumber = "inline";
                        showCustomerName = "none";
                        showReason = "none";
                        showDelete = "none";
                        showOperar = "none";
                        show = "none";
                        showCustomerOrderNumber = "none";
                        break;
                }
                hdBackUrl.Value = "ProductWarehouseLogList.aspx";
                sql = string.Format(@" 
select CheckTime  from ProductWarehouseLog where WarehouseNumber ='{0}' ", ToolManager.GetQueryString("WarehouseNumber"));
                if (string.IsNullOrEmpty(SqlHelper.GetScalar(sql)))
                {
                    if (ToolManager.CheckQueryString("IsXS")) //如果是从销售模块跳转过来
                    {
                        showOperar = "inline";
                        sql = string.Format(@" 
 select IsConfirm  from ProductWarehouseLog where WarehouseNumber='{0}' ", ToolManager.GetQueryString("WarehouseNumber"));
                        if (SqlHelper.GetScalar(sql).Equals("是"))
                        {
                            showOperar = "none";
                            show = "none";
                        }
                        else
                        {
                            showOperar = "inline";
                            show = "inline";
                        }
                        btnCheck.Visible = false;
                        hdBackUrl.Value = "../SellManager/MarerialWarehouseLogListForXS.aspx?why=" + DateTime.Now.ToShortTimeString();
                    }
                    else
                    {
                        if (titleName.Equals("销售出库"))
                        {
                            showOperar = "none";
                            show = "none";
                        }
                        else
                        {
                            btnCheck.Visible = true;
                            showOperar = "inline";
                            show = "inline";
                        }
                    }
                }
                else
                {
                    btnCheck.Visible = false;
                    showOperar = "none";
                    show = "none";
                }
            }
            else
            {
                Response.Write("未知出入库单！");
                Response.End();
                return;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string auditor = ToolCode.Tool.GetUser().UserNumber;
            string number = "'" + ToolManager.GetQueryString("WarehouseNumber") + "'";
            string result = StoreroomToolManager.AutiorProductWarehouseLog(number, auditor);
            if (result.Equals("1"))
            { 
                Response.Redirect("ProductWarehouseLogList.aspx"); 
            }
            else
            {
                lbSubmit.Text = "审核失败！原因：" + result;
            }
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string conditon = " where t.出入库编号='" + ToolManager.GetQueryString("WarehouseNumber") + "'";
            if (txtProductNumber.Text != "")
            {
                conditon += " and t.产品编号 like '%" + txtProductNumber.Text + "%'";
            }
            if (txtCustomerProductNumber.Text != "")
            {
                conditon += " and t.客户产成品编号 like '%" + txtCustomerProductNumber.Text + "%'";
            }
            string sql = string.Format(@" 
 select t.客户采购订单号,
 t.项目名称,
 t.行号,
 t.版本,
t.客户产成品编号,
t.产品描述,
t.数量,
 t.货位,
 t.交期,
 t.报废退货原因,
  isnull(vpss.库存数量,0) as 库存数量,
 t.备注
 from (
select vtp.*,so.CustomerId from  dbo.V_Tool_ProductWarehouseLogDetail vtp left join 
 SaleOder so on vtp.单据编号=so.OdersNumber) t 
 left join  Customer c on t.CustomerId=c.CustomerId
 left join V_ProductStock_Sum vpss on t.产品编号=vpss .ProductNumber and t.版本=vpss.Version {0}
 order by t.客户产成品编号 asc", conditon);
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "产成品销售出库单");
        }
        ///// <summary>
        ///// 确认
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void btnConfirm_Click(object sender, EventArgs e)
        //{
        //    string error = string.Empty;
        //    string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
        //    string sql = string.Format(" update ProductWarehouseLog set IsConfirm ='已确认' where WarehouseNumber='{0}' ", warehouseNumber);
        //    bool result = SqlHelper.ExecuteSql(sql, ref error);
        //    if (result)
        //    {
        //        lbSubmit.Text = "确认成功";
        //        lbIsConfim.Text = "是";
        //    }
        //    else
        //    {
        //        lbSubmit.Text = string.Format("确认失败！原因：" + error);
        //        lbIsConfim.Text = "否";
        //    }
        //}
    }
}
