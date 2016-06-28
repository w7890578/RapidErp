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
    public partial class AddOrEditHalfOutProductWarehouseLogDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string error = string.Empty;
                string sql = string.Empty;
                if (!ToolManager.CheckQueryString("WarehouseNumber"))
                {
                    lbSubmit.Text = "未知入库编号！";
                    return;
                }
                else
                {
                    if (!ToolManager.CheckQueryString("Guid"))
                    {
                        this.btnSubmit.Text = "添加";
                        this.lbWarehouseNumber.Text = ToolManager.GetQueryString("WarehouseNumber");
                    }
                    else
                    {
                        sql = string.Format(@" select * from HalfProductWarehouseLogDetail where WarehouseNumber='{0}' and guid='{1}'",
                            ToolManager.GetQueryString("WarehouseNumber"), ToolManager.GetQueryString("Guid"));
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            lbWarehouseNumber.Text = dr["WarehouseNumber"] == null ? "" : dr["WarehouseNumber"].ToString();
                            lbDocumentNumber.Text = dr["DocumentNumber"] == null ? "" : dr["DocumentNumber"].ToString();
                            txtMaterialNumner.Text = dr["MaterialNumber"] == null ? "" : dr["MaterialNumber"].ToString();
                            txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                            this.btnSubmit.Text = "修改";
                            this.txtDocumentNumber.Visible = false;

                        }

                    }

                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string warehousenumber = this.lbWarehouseNumber.Text.Trim();
            string documentnumber = this.txtDocumentNumber.Text.Trim();
            string materialnumber = this.txtMaterialNumner.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string guid = ToolManager.GetQueryString("Guid");
            string warehournumber = ToolManager.GetQueryString("WarehouseNumber");
            string sql = string.Empty;
            bool result = false;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from HalfProductWarehouseLogDetail where guid='{0}' ", guid);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该出入库编号！请重新填写！";
                    return;
                }
                sql = string.Format(@" select * from ProductPlanDetail where PlanNumber='{0}'", documentnumber);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {

                    string OrdersNumber = dt.Rows[0]["OrdersNumber"].ToString();
                    string ProductNumber = dt.Rows[0]["ProductNumber"].ToString();
                    string Version = dt.Rows[0]["Version"].ToString();
                    string LeadTime = dt.Rows[0]["LeadTime"].ToString();
                    string Qty = dt.Rows[0]["Qty"].ToString();
                    List<string> sqls = new List<string>();
                    sql = string.Format(@" insert into HalfProductWarehouseLogDetail (warehousenumber,DocumentNumber,ProductNumber,Version,
                    MaterialNumber,SailOrderNumber,LeadTime,Qty,Remark) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},'{8}')",
                    warehournumber, documentnumber, ProductNumber, Version, materialnumber, OrdersNumber, LeadTime, Qty, Remark);
                    sqls.Add(sql);
                    //sql = StoreroomToolManager.GetUpdateInventoryQtySql("", ProductNumber, Version, warehournumber, Qty, Model.ToolEnum.ProductType.Product);
                    //sqls.Add(sql);
                    result = SqlHelper.BatchExecuteSql(sqls, ref error);
                    lbSubmit.Text = result == true ? "添加成功" : "添加失败，原因：" + error;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加半成品入库信息" + warehournumber, "增加成功");
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加半成品入库信息" + warehournumber, "增加失败！原因" + error);
                        return;
                    }

                }
                else
                {

                    lbSubmit.Text = "没有该开工单号！请重新填写！";
                    return;

                }

            }
            else
            {

                sql = string.Format(@" select * from HalfProductWarehouseLogDetail where guid='{0}' ", guid);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该条记录已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" update HalfProductWarehouseLogDetail set materialnumber ='{0}',remark='{1}'
                where guid='{2}' ", materialnumber, Remark, guid);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑半成品入库信息" + warehournumber, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑半成品入库信息" + warehournumber, "编辑失败！原因" + error);
                    return;
                }
            }
        }
    }
}
