﻿using BLL;
using DAL;
using Rapid.ToolCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.StoreroomManager
{
    public partial class ToolMaterialWarehouseLogDetail : System.Web.UI.Page
    {
        public double CountQty = 0;
        public string documentName = string.Empty;

        public string hasDelete = "inline";

        public string hasEdit = "inline";

        public string number = string.Empty;

        //单据编号 产成品编号 供应商物料编号 客户物料编号 供应商名称  客户名称
        public string show = "inline";

        public string showAdd = "inline";
        public string showCheck = "inline";
        public string showCompleteQty = "none";
        public string showCustomerMaterialNumber = "inline";
        public string showCustomerName = "inline";

        /// <summary>
        /// 是否显示客户订单号
        /// </summary>
        public bool ShowCustomerOrderNumber = false;

        public string showDelete = "inline";
        public string showDocumentNumber = "inline";
        public string showLeadTime = "none";
        public string showOperar = "inline";
        public string showProductNumber = "inline";
        public string showRoadTransport = "none";
        public string showRowNumber = "none";
        public string showSingleDose = "none";
        public string showSupplierMaterialNumber = "inline";
        public string showSupplierName = "inline";

        //单据名称
        public string type = string.Empty;

        //出入库类型
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lbSubmit.Text = string.Empty;
            Bind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;

            string errorTrace = string.Empty;
            int errorCount = 0;
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");

            DataSet ds = ToolManager.ImpExcel(FU_Excel, Server);
            if (ds == null)
            {
                lbSubmit.Text = "选择的文件为空或不是标准的Excel文件！";
                return;
            }
            string insertSqlFormat = @"
select '{0}',OrdersNumber ,MaterialNumber ,SupplierMaterialNumber,NonDeliveryQty ,UnitPrice,LeadTime,'0','{1}','{2}'  from CertificateOrdersDetail
  where OrdersNumber = '{3}' and MaterialNumber = '{4}' and LeadTime = '{5}'  and NonDeliveryQty> 0";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber ,SupplierMaterialNumber
, Qty, UnitPrice, LeadTime, RowNumber, Remark, RoadTransport)");

            //foreach (DataRow row in ds.Tables[0].Rows)
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                DataRow row = ds.Tables[0].Rows[i];

                if (string.IsNullOrEmpty(row["采购订单号"].ToString().Trim())
                    || string.IsNullOrEmpty(row["原材料编号"].ToString().Trim())
                    || string.IsNullOrEmpty(row["交期"].ToString().Trim())
                    )
                {
                    Bind();
                    lbSubmit.Text = "导入失败：信息填写不完整！";
                    return;
                }

                string tempSql = string.Format(insertSqlFormat, warehouseNumber, row["备注"], row["运输号"], row["采购订单号"].ToString().Trim(),
                    row["原材料编号"].ToString().Trim(), Convert.ToDateTime(row["交期"].ToString().Trim()).ToString("yyyy-MM-dd"));

                sb.Append(tempSql);
                sb.AppendLine("");
                sb.Append("union all");

                DataTable dtTemp = SqlHelper.GetTable(tempSql);
                if (dtTemp == null || dtTemp.Rows.Count == 0)
                {
                    errorTrace += string.Format("Excel文件第{0}行导入失败，原因：没有查询到匹配的数据。{1}", i, "<br/>");
                    errorCount += 1;
                }
            }

            string insertSql = sb.ToString().TrimEnd("union all".ToCharArray());
            bool result = SqlHelper.ExecuteSql(insertSql, ref error);
            Bind();
            if (result)
            {
                if (errorCount > 0 || !string.IsNullOrEmpty(errorTrace))
                {
                    lbSubmit.Text = "导入异常！" + "<br/>" + errorTrace;
                    return;
                }
                else
                {
                    lbSubmit.Text = "导入成功！";
                    return;
                }
            }
            else
            {
                lbSubmit.Text = "导入失败!" + error;
                return;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                showAdd = "inline";
                //spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0403", "Print");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0403", "Edit");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0403", "Delete");
                if (ToolManager.CheckQueryString("xuan"))//选选择
                {
                    Choose();
                }

                if (ToolManager.CheckQueryString("IsAutior"))//审核
                {
                    Autior();
                }

                Delete();
                Bind();
            }
        }

        protected void SetQty(object qty, ref double countQty)
        {
            countQty += (qty == null ? 0 : Convert.ToDouble(qty));
        }

        /// <summary>
        /// 审核
        /// </summary>
        private void Autior()
        {
            string autior = ToolCode.Tool.GetUser().UserNumber;
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            Response.Write(MarerialWarehouseLogListManager.AuditorMarerialWarehouseLogForWarehouseNumber(autior, warehouseNumber));
            Response.End();
            return;
        }

        private void Bind()
        {
            string orderbyname = string.Empty;
            if (ToolManager.CheckQueryString("WarehouseNumber"))
            {
                string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                number = warehouseNumber;
                hdnumber.Value = warehouseNumber;
                type = Server.UrlDecode(ToolManager.GetQueryString("Type")); //出入库类型
                string sql = string.Empty;
                //采购入库：采购订单号、原材料编号、供应商物料编号、供应商名称、名称、描述、数量、仓位、备注。
                //生产退料入库：开工单号、产成品编号、原材料编号、名称、描述、数量、仓位、备注。
                //盘盈入库：盘点编号、原材料编号、名称、描述、数量、仓位、备注。
                //样品入库：原材料编号、供应商物料编号、供应商名称、名称、描述、数量、仓位、备注。
                //生产出库：开工单号、原材料编号、客户物料编号、名称、描述、数量、仓位、备注。
                //销售出库：销售订单编号、原材料编号、客户物料编号、客户名称、名称、描述、数量、仓位、备注。
                //采购退料出库：采购订单编号、原材料编号、供应商物料编号、名称、描述、数量、仓位、备注。
                //盘亏出库：盘点编号、原材料编号、名称、描述、数量、仓位、备注。
                //包装出库：销售订单编号、产成品编号、原材料编号、客户物料编号、名称、描述、数量、仓位、备注。
                //样品出库：开工单号、产成品编号、原材料编号、名称、描述、数量、仓位、备注
                //维修出库：维修订单号、产成品编号、原材料编号、名称、描述、数量、仓位、备注

                //共有的： 单据编号  原材料编号 产成品编号 供应商物料编号  供应商名称  客户物料编号 客户名称
                switch (type)
                {
                    case "采购入库":
                        documentName = "采购订单号";
                        showProductNumber = "none";
                        showCustomerMaterialNumber = "none";
                        showCustomerName = "none";
                        showSingleDose = "none";
                        showSupplierMaterialNumber = "inline";
                        showSupplierName = "inline";
                        showRowNumber = "none";
                        showLeadTime = "none";
                        showRoadTransport = "inline";
                        showCompleteQty = "none";
                        showDocumentNumber = "inline";
                        ShowCustomerOrderNumber = false;
                        orderbyname = " order by 供应商物料编号 desc ";
                        break;

                    case "采购退料出库":
                        documentName = "采购订单";
                        showProductNumber = "none";
                        showCustomerMaterialNumber = "none";
                        showCustomerName = "none";
                        showAdd = "inline";
                        showSingleDose = "none";
                        showRowNumber = "none";
                        showLeadTime = "none";
                        showRoadTransport = "none";
                        showCompleteQty = "none";
                        ShowCustomerOrderNumber = false;
                        break;
                    //case "盘盈入库":
                    //    documentName = "盘点编号";
                    //    showProductNumber = "none";
                    //    showCustomerMaterialNumber = "none";
                    //    showCustomerName = "none";
                    //    showSupplierMaterialNumber = "none";
                    //    showSupplierName = "none";
                    //    break;
                    //case "盘亏出库":
                    //    documentName = "盘点编号";
                    //    showProductNumber = "none";
                    //    showCustomerMaterialNumber = "none";
                    //    showCustomerName = "none";
                    //    showSupplierMaterialNumber = "none";
                    //    showSupplierName = "none";
                    //    break;
                    //case "样品出库":
                    //    documentName = "样品订单号";
                    //    showProductNumber = "none";
                    //    showSupplierMaterialNumber = "none";
                    //    showSupplierName = "none";
                    //    break;
                    //case "样品入库":
                    //    documentName = "";
                    //    showProductNumber = "none";
                    //    showCustomerMaterialNumber = "none";
                    //    showCustomerName = "none";
                    //    showDocumentNumber = "none";
                    //    break;
                    case "生产退料入库":
                        documentName = "开工单号";
                        showCustomerMaterialNumber = "none";
                        showCustomerName = "none";
                        showSupplierMaterialNumber = "none";
                        showSupplierName = "none";
                        showSingleDose = "none";
                        showRowNumber = "none";
                        showLeadTime = "none";
                        showRoadTransport = "none";
                        showCompleteQty = "none";
                        showProductNumber = "none";
                        ShowCustomerOrderNumber = false;
                        break;

                    case "生产出库":

                        documentName = "开工单号";
                        showProductNumber = "none";
                        showCustomerMaterialNumber = "inline";
                        showCustomerName = "none";
                        showSupplierMaterialNumber = "none";
                        showSupplierName = "none";
                        showDelete = "none";
                        show = "none";
                        showOperar = "none";
                        showAdd = "none";
                        showSingleDose = "none";
                        showRowNumber = "none";
                        showLeadTime = "none"; showRoadTransport = "none"; showCompleteQty = "none";
                        showDocumentNumber = "inline";
                        ShowCustomerOrderNumber = false;
                        orderbyname = " order by 客户物料编号 desc ";
                        break;

                    case "销售出库（贸易）":
                        documentName = "销售订单号";
                        showProductNumber = "none";
                        showSupplierName = "none";
                        showSupplierMaterialNumber = "none";
                        showSingleDose = "none";
                        showOperar = "inline";
                        showCustomerMaterialNumber = "inline";
                        showCustomerName = "inline";
                        showRowNumber = "inline";
                        showLeadTime = "inline"; showRoadTransport = "none";
                        showCompleteQty = "inline";
                        ShowCustomerOrderNumber = true;
                        orderbyname = " order by 客户物料编号 desc ";
                        break;

                    case "包装出库":
                        documentName = "开工单号";
                        showProductNumber = "inline";
                        showSupplierMaterialNumber = "none";
                        showSupplierName = "none";
                        showCustomerName = "none";
                        showAdd = "none";
                        showOperar = "none";
                        showSingleDose = "none";
                        showRowNumber = "none";
                        showLeadTime = "none";
                        showRoadTransport = "none";
                        showCompleteQty = "none";
                        ShowCustomerOrderNumber = false;
                        break;

                    case "维修出库":
                        documentName = "维修订单号";
                        sql = "";
                        showCustomerMaterialNumber = "inline";
                        showCustomerName = "none";
                        showSupplierMaterialNumber = "none";
                        showSupplierName = "none";
                        showSingleDose = "none";
                        showAdd = "none";
                        showOperar = "none";
                        showRowNumber = "none";
                        showLeadTime = "none";
                        showRoadTransport = "none";
                        showCompleteQty = "none";
                        ShowCustomerOrderNumber = false;
                        break;

                    case "辅料出库":
                        showDocumentNumber = "none";
                        showProductNumber = "none";
                        showSupplierName = "none";
                        showCustomerMaterialNumber = "none";
                        showSingleDose = "none";
                        showRowNumber = "none";
                        showLeadTime = "none";
                        showRoadTransport = "none";
                        showCompleteQty = "none";
                        ShowCustomerOrderNumber = false;
                        break;
                }
                //                if (type.Equals("样品入库"))
                //                {
                //                    string condition = " where mwld.出入库编号='" + warehouseNumber + "'";
                //                    if (txtMaterialNumber.Text != "")
                //                    {
                //                        condition += " and mwld.原材料编号 like '%" + txtMaterialNumber.Text + "%'";
                //                    }
                //                    sql = string.Format(@"select mwld.出入库编号,mwld.单据编号,mwld.原材料编号,mwld.产成品编号,''as客户产成品编号,''as 版本
                //,mwld.客户物料编号,mwld.供应商物料编号,mwld.数量,mwld.原材料名称
                //,mwld.原材料描述,mwld.仓库ID,mwld.备注,mwld.审核时间 ,mwld.样品入库供应商编号 as 供应商编号
                //,ISNULL ( si.SupplierName ,'')as 供应商名称,mwld.客户编号,mwld.客户名称,mwld.仓位 ,mit.Cargo as 货位
                //,vmsq.qty as  库存数量
                // from V_Tool_MaterialWarehouseLogDetail mwld left join
                //SupplierInfo si on  mwld.样品入库供应商编号=si.SupplierId
                //left join MarerialInfoTable mit on mwld.原材料编号=mit.MaterialNumber
                //left join V_MaterialStock_Qty vmsq on mwld.原材料编号=vmsq.MaterialNumber {0}", condition);
                //                }
                //else
                //{
                string condition = " where t.出入库编号='" + warehouseNumber + "' and 数量>0";
                if (txtMaterialNumber.Text.Trim() != "")
                {
                    condition += " and t.原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
                }
                if (txtCustomerMaterialNumber.Text.Trim() != "")
                {
                    condition += " and t.客户物料编号 like '%" + txtCustomerMaterialNumber.Text.Trim() + "%'";
                }
                if (txtSupplierMaterialNumber.Text.Trim() != "")
                {
                    condition += " and t.供应商物料编号 like '%" + txtSupplierMaterialNumber.Text.Trim() + "%'";
                }
                if (txtMaterialDescription.Text.Trim() != "")
                {
                    condition += " and t.原材料描述 like '%" + txtMaterialDescription.Text.Trim() + "%'";
                }
                if (txtYSnumber.Text.Trim() != "")
                {
                    condition += " and t.运输号 like '%" + txtYSnumber.Text.Trim() + "%'";
                }
                if (txtCargoType.Text != "")
                {
                    condition += " and 货物类型 like '%" + txtCargoType.Text.Trim() + "%'";
                }
                if (txtHW.Text != "")
                {
                    condition += " and 货位 like '%" + txtHW.Text.Trim() + "%'";
                }
                //if (type.Equals("采购入库"))
                //{
                //   // sql = string.Format();
                //}
                //else
                //{
                sql = string.Format(@"select t.*,vmsq .qty as 库存数量,b.CustomerOrderNumber as 客户采购订单号 from [V_Tool_MaterialWarehouseLogDetail] t left join V_MaterialStock_Qty vmsq
on t.原材料编号=vmsq.MaterialNumber
left join SaleOder b on t.单据编号=b.OdersNumber
{0}
", condition);
                sql = string.Format("select * from ({0})t  {1}", sql, orderbyname);
                //                sql += string.Format(@"union all
                //select '合计','合计','','','','','','',SUM (t.数量),'','','','',0,'','','','','','','','','','','','','','','','',0,''
                //from ({0}) t", sql);
                //}
                DataTable dt = SqlHelper.GetTable(sql);
                rpList.DataSource = dt;
                rpList.DataBind();
                sql = string.Format("select CheckTime  from MarerialWarehouseLog where WarehouseNumber='{0}'", warehouseNumber);
                show = SqlHelper.GetScalar(sql) == "" ? "inline" : "none";
                hdType.Value = type;
                foreach (DataRow dr in dt.Rows)
                {
                    CountQty += (dr["数量"] == null ? 0 : Convert.ToDouble(dr["数量"]));
                }
            }
            else
            {
                Response.Write("未知入库单！");
                Response.End();
                return;
            }
            hdBackUrl.Value = "MarerialWarehouseLogList.aspx";
            //查看是否已审核
            string tempSql = string.Format(@"
select ISNULL (CheckTime ,'') from MarerialWarehouseLog where WarehouseNumber='{0}'", ToolManager.GetQueryString("WarehouseNumber"));
            if (!string.IsNullOrEmpty(SqlHelper.GetScalar(tempSql)))
            {
                showCheck = "none";
                showAdd = "none";
                showOperar = "none";
            }
            else
            {
                if (ToolManager.CheckQueryString("IsXS"))
                {
                    showCheck = "none";
                    hdBackUrl.Value = "../SellManager/MaterialSaleOderWarehouseOut.aspx?why=" + DateTime.Now.ToShortTimeString();
                    tempSql = string.Format(" select IsConfirm  from MarerialWarehouseLog where WarehouseNumber='{0}' ", ToolManager.GetQueryString("WarehouseNumber"));
                    if (SqlHelper.GetScalar(tempSql).Equals("是")) //如果是已确认状态不能编辑、删除、添加
                    {
                        showAdd = "none";
                        showOperar = "none";
                    }
                    else
                    {
                        showAdd = "inline";
                        showOperar = "inline";
                    }
                }

                //如果是采购模块进入
                else if (ToolManager.CheckQueryString("IsCG"))
                {
                    showCheck = "none";
                    hdBackUrl.Value = "../PurchaseManager/MarerialWarehouseLogListForCG.aspx?why=" + DateTime.Now.ToShortTimeString();
                    tempSql = string.Format(" select IsConfirm  from MarerialWarehouseLog where WarehouseNumber='{0}' ", ToolManager.GetQueryString("WarehouseNumber"));
                    if (SqlHelper.GetScalar(tempSql).Equals("是")) //如果是已确认状态不能编辑、删除、添加
                    {
                        showAdd = "none";
                        showOperar = "none";
                    }
                    else
                    {
                        showAdd = "inline";
                        showOperar = "inline";
                    }
                }
                //如果是维修出库进入
                else if (ToolManager.CheckQueryString("IsRepairReceiptList"))
                {
                    showCheck = "none";
                    hdBackUrl.Value = "../ProduceManager/RepairReceiptList.aspx";
                    tempSql = string.Format(" select IsConfirm  from MarerialWarehouseLog where WarehouseNumber='{0}' ", ToolManager.GetQueryString("WarehouseNumber"));
                    if (SqlHelper.GetScalar(tempSql).Equals("是")) //如果是已确认状态不能编辑、删除、添加
                    {
                        showAdd = "none";
                        showOperar = "none";
                    }
                    else
                    {
                        showAdd = "inline";
                        showOperar = "inline";
                    }
                }
                else
                {
                    showCheck = "inline";
                    showAdd = "inline";
                    showOperar = "inline";
                    showCheck = "inline";
                }
            }

            if (ToolManager.CheckQueryString("IsCG"))
            {
                hdBackUrl.Value = "../PurchaseManager/MarerialWarehouseLogListForCG.aspx?why=" + DateTime.Now.ToShortTimeString();
            }
            else if (ToolManager.CheckQueryString("IsRepairReceiptList"))
            {
                hdBackUrl.Value = "../ProduceManager/RepairReceiptList.aspx";
            }
            else if (ToolManager.CheckQueryString("IsXS"))
            {
                hdBackUrl.Value = "../SellManager/MaterialSaleOderWarehouseOut.aspx?why=" + DateTime.Now.ToShortTimeString();
            }
            else
            {
                hdBackUrl.Value = "MarerialWarehouseLogList.aspx";
            }
        }

        private void Choose()
        {
            string error = string.Empty;
            string guids = ToolManager.GetQueryString("xuan");
            string warehouseNumber = ToolManager.GetQueryString("warehouseNumber");
            string sql = string.Format(@"
delete MaterialWarehouseLogDetail where WarehouseNumber='{0}' and Guid not in ({1}) ", warehouseNumber, guids);
            string result = SqlHelper.ExecuteSql(sql, ref error) ? "1" : error;
            Response.Write(result);
            Response.End();
            return;
        }

        //删除
        private void Delete()
        {
            if (ToolManager.CheckQueryString("guid"))
            {
                string guid = ToolManager.GetQueryString("guid");

                string sql = string.Format(@"delete MaterialWarehouseLogDetail where guid='{0}'  ", guid);
                string error = string.Empty;
                bool restult = SqlHelper.ExecuteSql(sql, ref error);
                if (restult)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除" + type + "明细", "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除" + type + "明细", "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }
        }

        //protected void btnConfim_Click(object sender, EventArgs e)
        //{
        //    string error = string.Empty;
        //    string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
        //    string sql = string.Format(@"update MarerialWarehouseLog set IsConfirm='已确认' where WarehouseNumber='{0}'", warehouseNumber);
        //    bool result = SqlHelper.ExecuteSql(sql, ref error);
        //    if (result)
        //    {
        //        lbSubmit.Text = "确认成功";
        //        lbIsConfim.Text = "是";
        //    }
        //    else
        //    {
        //        lbSubmit.Text = "确认失败！原因：" + error;
        //        lbIsConfim.Text = "否";
        //    }
        //}
    }
}