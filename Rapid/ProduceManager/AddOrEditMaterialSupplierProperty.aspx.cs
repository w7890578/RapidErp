using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class MaterialSupplierProperty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("MaterialNumber"))
                {
                    ControlBindManager.BindDrp("select * from SupplierInfo order by SupplierName asc", drpSupplierId, "SupplierId", "SupplierName");
                    string MaterialNumber = ToolManager.GetQueryString("MaterialNumber");
                    string supplierId = Server.UrlDecode(ToolManager.GetQueryString("SupplierId"));
                    string error = string.Empty;
                    string sql = string.Format(@" select * from MaterialSupplierProperty where 
                    MaterialNumber='{0}' and SupplierId='{1}' ", MaterialNumber, supplierId);
                    if (!ToolManager.CheckQueryString("SupplierId"))
                    {

                        this.btnSubmit.Text = "添加";
                        this.lbMaterialNumber.Text = MaterialNumber;
                        this.lbSupplierMaterialNumber.Visible = false;
                        this.txtSupplierMaterialNumber.Visible = true;
                    }
                    else
                    {
                        this.btnSubmit.Text = "修改";
                        this.lbSupplierMaterialNumber.Visible = true;
                        this.txtSupplierMaterialNumber.Visible = false;
                        this.drpSupplierId.Enabled = false;
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            lbSupplierMaterialNumber.Text = dr["SupplierMaterialNumber"] == null ? "" : dr["SupplierMaterialNumber"].ToString();

                            drpSupplierId.SelectedValue = dr["SupplierId"] == null ? "" : dr["SupplierId"].ToString();
                            txtMinOrderQty.Text = dr["MinOrderQty"] == null ? "" : dr["MinOrderQty"].ToString();
                            txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                            txtPrice.Text = dr["Prcie"] == null ? "" : dr["Prcie"].ToString();
                            txtDeliveryCycle.Text = dr["DeliveryCycle"] == null ? "" : dr["DeliveryCycle"].ToString();
                        }
                    }
                    this.lbMaterialNumber.Text = MaterialNumber;
                }
                else
                {
                    Response.Write("请选择原材料！");
                    Response.End();
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string MaterialNumber = this.lbMaterialNumber.Text;
            string SupplierMaterialNumber = txtSupplierMaterialNumber.Text;
            string SupplierId = drpSupplierId.SelectedValue;
            string MinOrderQty = txtMinOrderQty.Text;
            string Remark = txtRemark.Text;
            string price = txtPrice.Text;
            string sql = string.Empty;
            string deliverycycle = txtDeliveryCycle.Text;
            if (this.btnSubmit.Text.Equals("添加"))
            {
           
//                sql = string.Format(@" select * from MaterialSupplierProperty where 
//                    MaterialNumber='{0}' and SupplierId='{1}' ", MaterialNumber, SupplierId);
//                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
//                {
//                    lbSubmit.Text = "已存在该供应商编号！请重新填写！";
//                    return;
//                }
                sql = string.Format(@" insert into MaterialSupplierProperty (MaterialNumber,SupplierMaterialNumber,SupplierId,MinOrderQty,Remark,Prcie,DeliveryCycle) 
            values ('{0}','{1}','{2}','{3}','{4}',{5},'{6}') ", MaterialNumber, SupplierMaterialNumber, SupplierId, MinOrderQty, Remark,price,deliverycycle);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料供应商信息" + MaterialNumber, "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料供应商信息" + MaterialNumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                sql = string.Format(@" select * from SupplierInfo where 
                   SupplierId='{0}' ",SupplierId);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该原材料的供应商属性已被删除";
                    return;
                }
                sql = string.Format(@" select COUNT(*) from MarerialInfoTable where MaterialNumber='{0}' ", MaterialNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该原材料编号不存在，请重新填写！";
                    return;
                }
                sql = string.Format(@" update MaterialSupplierProperty set SupplierMaterialNumber='{0}',
MinOrderQty='{1}',Remark='{2}',Prcie={5},DeliveryCycle='{6}' where MaterialNumber='{3}' and SupplierId='{4}' ", lbSupplierMaterialNumber.Text, MinOrderQty, Remark, MaterialNumber, SupplierId, price,deliverycycle);

                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料供应商信息" + MaterialNumber, "编辑成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料供应商信息" + MaterialNumber, "编辑失败！原因：" + error);
                    return;
                }
            }
        }
    }
}
