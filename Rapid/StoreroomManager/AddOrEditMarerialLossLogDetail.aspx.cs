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
    public partial class AddOrEditMarerialLossLogDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MarerialLossLogManager.BindTakeMaterialPerson(this.drpTakeMaterialPerson);
                if (ToolManager.CheckQueryString("WarehouseNumber") && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version") && ToolManager.CheckQueryString("MaterialNumber"))
                {
                    DataTable dt = HasDeleted();
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        lbWarehouseNumber.Text = dr["WarehouseNumber"] == null ? "" : dr["WarehouseNumber"].ToString();
                        txtCustomerProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                        txtVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                        txtUniversalNumber.Text = dr["MaterialNumber"] == null ? "" : dr["MaterialNumber"].ToString();
                        txtDate.Text = dr["LogDate"] == null ? "" : dr["LogDate"].ToString();
                        txtQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        drpTakeMaterialPerson.SelectedValue = dr["TakeMaterialPerson"] == null ? "" : dr["TakeMaterialPerson"].ToString();
                        this.lbTeam.Text =getTeam(this.drpTakeMaterialPerson.SelectedValue);
                        txtLossReason.Text = dr["LossReason"] == null ? "" : dr["LossReason"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();

                    }
                    this.txtCustomerProductNumber.ReadOnly = true;
                    this.txtVersion.ReadOnly = true;
                    this.txtUniversalNumber.ReadOnly = true;
                    btnSubmit.Text = "修改";

                }
                else
                {
                    btnSubmit.Text = "添加";
                    this.lbWarehouseNumber.Text = ToolManager.GetQueryString("WarehouseNumber");

                }

            }
        }
        private string getTeam(string TakeMaterialPerson)
        {
            string sql = string.Format("  select Team FROM pm_user WHERE USER_ID='{0}'", TakeMaterialPerson);
            string team = SqlHelper.GetScalar(sql);
            if (team != "" && team != null)
            {
                return team;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 检查该条记录是否被删除
        /// </summary>
        /// <returns></returns>
        private DataTable HasDeleted()
        {
            string error = string.Empty;
            string sql = string.Format(@"
            select * from MarerialLossLog where WarehouseNumber ='{0}' and ProductNumber='{1}' and Version='{2}' and MaterialNumber='{3}'",
           ToolManager.GetQueryString("WarehouseNumber"), ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"), ToolManager.GetQueryString("MaterialNumber"));
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count <= 0)
            {
                Response.Write("异常：该条记录已被删除！");
                Response.End();
                return null;
            }
            else
            {
                return dt;
            }
        }
        //BOM信息表中是否存在
        private bool isexitBOM(string productnumber, string version, string materialnumber)
        {
            string sql = string.Empty;
            sql = string.Format(@" select * from BOMInfo where productnumber='{0}' and version='{1}' and MaterialNumber='{2}' ",
                   productnumber, version, materialnumber);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count <= 0)
            {
                lbSubmit.Text = "BOM信息表中没有匹配信息，请重新填写！！！";
                return false;

            }
            return true;
        }

        private bool isexit(string warehousenumber, string productnumber, string version, string materialnumber)
        {
            string sql = string.Empty;
            sql = string.Format(@" select * from MarerialLossLog where productnumber='{0}' and version='{1}' and MaterialNumber='{2}' and warehousenumber='{3}'",
                   productnumber, version, materialnumber, warehousenumber);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count > 0)
            {
                lbSubmit.Text = "原材料损耗表中已存在该条数据，请重新填写！！！";
                return false;

            }
            return true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string sql = string.Empty;
            string productnumber=string.Empty;
            string warehousenumber = this.lbWarehouseNumber.Text.Trim();
            string customerproductnumber = txtCustomerProductNumber.Text.Trim();
            string version = txtVersion.Text.ToUpper().Trim();
            string materialnumber =string.Empty;
            string logdate = this.txtDate.Text.Trim();
            string qty = txtQty.Text.Trim();
            string takematerialpersion = this.drpTakeMaterialPerson.SelectedValue;
            string team = this.lbTeam.Text.Trim();
            string lossreason = this.txtLossReason.Text.Trim();
            string remark = txtRemark.Text.Trim();
            
            bool result = false;
            if (btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@"select ProductNumber from ProductCustomerProperty 
where CustomerProductNumber='{0}' and Version='{1}'", customerproductnumber, version);
                if (SqlHelper.GetScalar(sql).Equals(""))
                {
                    lbSubmit.Text = "未知的客户产成品编号及版本！";
                    return;
                }
                else
                {
                    productnumber = SqlHelper.GetScalar(sql);
                }

                if (drpType.SelectedValue == "供应商物料编号")
                {
                    sql = string.Format(@"select top 1 MaterialNumber from MaterialSupplierProperty where SupplierMaterialNumber='{0}'", txtUniversalNumber.Text.Trim());
                    if (SqlHelper.GetScalar(sql).Equals(""))
                    {
                        lbSubmit.Text = "未知的供应商物料编号！";
                        return;
                    }
                    else
                    {
                        materialnumber = SqlHelper.GetScalar(sql);
                    }
                }
                if (drpType.SelectedValue == "客户物料编号")
                {
                    sql = string.Format(@"select top 1 MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", txtUniversalNumber.Text.Trim());
                    if (SqlHelper.GetScalar(sql).Equals(""))
                    {
                        lbSubmit.Text = "未知的客户物料编号！";
                        return;
                    }
                    else
                    {
                        materialnumber = SqlHelper.GetScalar(sql);
                    }
                }
                if (drpType.SelectedValue == "瑞普迪编号")
                {
                    sql = string.Format(@"select top 1 MaterialNumber from MarerialInfoTable where MaterialNumber='{0}'", txtUniversalNumber.Text.Trim());
                    if (SqlHelper.GetScalar(sql).Equals(""))
                    {
                        lbSubmit.Text = "未知的瑞普迪编号！";
                        return;
                    }
                    else 
                    {
                        materialnumber = SqlHelper.GetScalar(sql);
                    }
                }

                if (!isexitBOM(productnumber, version, materialnumber))
                {
                    lbSubmit.Text = "BOM信息表中没有匹配信息，请重新填写！！！";
                    return;
                }
                if (!isexit(warehousenumber, productnumber, version, materialnumber))
                {
                    lbSubmit.Text = "原材料损耗表中已存在该条数据，请重新填写！！！";
                    return;

                }
                sql = string.Format(@" insert into MarerialLossLog (WarehouseNumber ,ProductNumber ,Version ,
                MaterialNumber,LogDate,Qty,TakeMaterialPerson,Team ,LossReason,Remark)
                values('{0}','{1}','{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}')", warehousenumber, productnumber, version,
                materialnumber, logdate, qty, takematerialpersion, team, lossreason, remark);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料损耗出库信息" + warehousenumber, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加原材料损耗出库信息" + warehousenumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                string Warehousenumber = this.lbWarehouseNumber.Text.Trim();
                sql = string.Format(@" update MarerialLossLog set LogDate='{0}',Qty={1},TakeMaterialPerson='{2}',
               Team='{3}',LossReason='{4}',Remark ='{5}' where WarehouseNumber='{6}' and ProductNumber ='{7}' and Version='{8}' and MaterialNumber='{9}'",
              logdate, qty, takematerialpersion, team, lossreason, remark,
               Warehousenumber, txtCustomerProductNumber.Text, version, txtUniversalNumber.Text);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功！" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料损耗出库信息" + warehousenumber, "编辑成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑原材料损耗出库信息" + warehousenumber, "编辑失败！原因" + error);
                    return;
                }
            }
        }
        protected void drpTakeMaterialPerson_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lbTeam.Text = getTeam(this.drpTakeMaterialPerson.SelectedValue);
        }
    }
}

