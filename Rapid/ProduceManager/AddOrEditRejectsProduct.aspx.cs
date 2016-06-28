using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditRejectsProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBindManager.BindDrp("select USER_ID,USER_NAME from PM_USER", this.drpName, "USER_ID", "USER_NAME");
                ControlBindManager.BindCustomer(this.drpCustomerId);
                this.trReportTime.Visible = false;
                if (ToolManager.CheckQueryString("ReportTime") && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
                {

                    DataTable dt = HasDeleted();
                    if (dt != null)
                    {
                        DataRow dr = dt.Rows[0];
                        lblCustomerId.Text = dr["CustomerName"] == null ? "" : dr["CustomerName"].ToString();
                        lbReportTime.Text = dr["ReportTime"] == null ? "" : dr["ReportTime"].ToString();
                        lblProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                        lblVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                        txtCustomerProductNumber.Text = dr["CustomerProductNumber"] == null ? "" : dr["CustomerProductNumber"].ToString();
                        txtQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        txtRepairReason.Text = dr["RepairReason"] == null ? "" : dr["RepairReason"].ToString();
                        txtRepairDate.Text = dr["RepairDate"] == null ? "" : dr["RepairDate"].ToString();
                        txtRepairInspectionDate.Text = dr["RepairInspectionDate"] == null ? "" : dr["RepairInspectionDate"].ToString();
                        drpName.Text = dr["Name"] == null ? "" : dr["Name"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        BindCustomer(txtProductNumber.Text, txtVersion.Text, drpCustomerId.Text, dr["CustomerProductNumber"] == null ? "" : dr["CustomerProductNumber"].ToString());
                    }
                    btnSubmit.Text = "修改";
                    this.txtReportTime.Visible = false;
                    this.lbReportTime.Visible = true;
                    this.txtProductNumber.Enabled = false;
                    this.txtVersion.Enabled = false;
                    this.drpCustomerId.Visible = false;
                    this.txtProductNumber.Visible = false;
                    this.txtVersion.Visible = false;
                }
                else
                {
                    btnSubmit.Text = "添加";
                    this.lbReportTime.Visible = false;
                    this.txtReportTime.Visible = true;
                    this.lblCustomerId.Visible = false;

                    if (ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version") && ToolManager.CheckQueryString("CustomerId"))
                    {
                        string productnumber = ToolManager.GetQueryString("ProductNumber");
                        string version = ToolManager.GetQueryString("Version");
                        string customerid = ToolManager.GetQueryString("CustomerId");
                        string sql = string.Format(@"
                    select CustomerProductNumber from ProductCustomerProperty where ProductNumber='{0}' and Version ='{1}' and CustomerId='{2}'", productnumber, version, customerid);
                        Response.Write(SqlHelper.GetScalar(sql));
                        Response.End();
                    }
                }
                if (ToolManager.CheckQueryString("m"))
                {
                    string result = string.Empty;
                    string sql = string.Format(@" select top 20  ProductNumber,[Version] ,ProductName from Product ");
                    if (ToolManager.CheckQueryString("contion"))
                    {
                        sql += string.Format(@"  where
ProductNumber like '%{0}%' or ProductNumber like'%{0}' or ProductNumber like '{0}%' or
ProductName like '%{0}%' or ProductName like'%{0}' or ProductName like '{0}%' 
order by ProductNumber asc", ToolManager.GetQueryString("contion"));
                    }
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            result += string.Format(" <tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", dr["ProductName"], dr["ProductNumber"], dr["Version"]);
                        }
                    }
                    Response.Write(result);
                    Response.End();
                    return;
                }

            }
        }
        /// <summary>
        /// 绑定客户编号
        /// </summary>
        /// <param name="materialNumber"></param>
        /// <param name="customerMaterialNumber"></param>
        private void BindCustomer(string materialNumber, string version, string customerid, string customerMaterialNumber)
        {
            //            string sql = string.Format(@"
            //select * from ProductCustomerProperty where ProductNumber='{0}' and Version ='{1}'", materialNumber, version);
            //            ControlBindManager.BindDrp(sql, this.lblCustomerProductNumber, "CustomerProductNumber", "CustomerProductNumber");
            this.txtCustomerProductNumber.Text = customerMaterialNumber;
            this.txtVersion.Text = version;
        }

        /// <summary>
        /// 检查该条记录是否被删除
        /// </summary>
        /// <returns></returns>
        private DataTable HasDeleted()
        {
            string error = string.Empty;
            string sql = string.Format(@"
            select rp.ReportTime ,rp.ProductNumber,rp.Version,rp.Qty,rp.RepairReason,
            rp.RepairDate,rp.RepairInspectionDate,rp.Name,rp.Remark,rp.Guid 
           from RejectsProduct rp where ReportTime ='{0}' and ProductNumber='{1}' and Version='{2}' and guid='{3}'",
           ToolManager.GetQueryString("ReportTime"), ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"), ToolManager.GetQueryString("Guid"));
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            string error = string.Empty;
            string sql = string.Empty;
            string reporttime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string productnumber = txtProductNumber.Text.Trim();
            string customerid = drpCustomerId.SelectedValue;
            string customermareial = string.Empty;
            string qty = txtQty.Text.Trim();
            string repairreason = txtRepairReason.Text.Trim();
            string repairdate = txtRepairDate.Text.Trim();
            string repairinspectiondate = txtRepairInspectionDate.Text.Trim();
            string name = drpName.SelectedValue.Trim();
            string remark = txtRemark.Text.Trim();

            if (btnSubmit.Text.Equals("添加"))
            {
                customermareial = Request.Form["txtCustomerProductNumber"].ToString();
                string version = Request.Form["txtVersion"].ToString();
                sql = string.Format(@" insert into RejectsProduct (ReportTime ,ProductNumber ,Version ,
                CustomerProductNumber,Qty,RepairReason,RepairDate ,RepairInspectionDate ,Name ,Remark,customerid)
                values('{0}','{1}','{2}','{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}')", reporttime, productnumber, version,
                customermareial, qty, repairreason, repairdate, repairinspectiondate, name, remark, customerid);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    BindCustomer(productnumber, version, customerid, customermareial);
                    Tool.WriteLog(Tool.LogType.Operating, "增加不合格品信息" + productnumber, "增加成功");
                    return;
                }
                else
                {
                    BindCustomer(productnumber, version, customerid, customermareial);
                    Tool.WriteLog(Tool.LogType.Operating, "增加不合格品信息" + productnumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {

                customermareial = txtCustomerProductNumber.Text;
                string Version = this.lblVersion.Text.Trim();
                sql = string.Format(@" update RejectsProduct set Qty='{0}',RepairReason='{1}',RepairDate='{2}',RepairInspectionDate='{3}',
              Name='{4}',Remark ='{5}' where ReportTime='{6}' and ProductNumber ='{7}' and Version='{8}'",
  qty, repairreason, repairdate, repairinspectiondate, name, remark,
                  this.lbReportTime.Text.Trim(), ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"));
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    BindCustomer(productnumber, Version, customerid, customermareial);
                    Tool.WriteLog(Tool.LogType.Operating, "编辑不合格品信息" + ToolManager.GetQueryString("ProductNumber"), "编辑成功");
                    return;
                }
                else
                {
                    BindCustomer(productnumber, Version, customerid, customermareial);
                    Tool.WriteLog(Tool.LogType.Operating, "编辑不合格品信息" + ToolManager.GetQueryString("ProductNumber"), "编辑失败！原因：" + error);
                    return;
                }
            }
        }
    }
}
