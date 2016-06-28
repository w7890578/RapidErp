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
    public partial class AddOrEditScarpWarehouseLogDetail : System.Web.UI.Page
    {
        public static string MaterialNumber = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string WarehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                MaterialNumber = Server.UrlDecode(ToolManager.GetQueryString("MaterialNumber"));
                lbWarehouseNumber.Text = WarehouseNumber;
                string error = string.Empty;
                string sql = string.Format(@" select * from ScarpWarehouseLogDetail where WarehouseNumber='{0}' and MaterialNumber='{1}'", WarehouseNumber, MaterialNumber);
                if (!ToolManager.CheckQueryString("MaterialNumber"))
                {
                    this.btnSubmit.Text = "添加";
                   
                }
                else
                {
                    this.btnSubmit.Text = "修改";
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        //this.lbWarehouseNumber.Text = dr["WarehouseNumber"] == null ? "" : dr["WarehouseNumber"].ToString();
                        this.txtQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        lblCustomerMarterilNumber.Text = dr["CustomerMarterilNumber"] == null ? "" : dr["CustomerMarterilNumber"].ToString();
                        this.btnSubmit.Text = "修改";
                    }
                    this.txtCustomerMarterilNumber.Visible = false;

                }

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string WarehouseNumber = this.lbWarehouseNumber.Text.Trim();
            string Qty = this.txtQty.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string CustomerMaterialnumber = txtCustomerMarterilNumber.Text;
            string sql = string.Empty;
            bool result = false;
            if (this.btnSubmit.Text.Equals("添加"))
            {

                sql = string.Format(@"select top 1 MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", CustomerMaterialnumber);
                string materialnumber = SqlHelper.GetScalar(sql);
                if (string.IsNullOrEmpty(materialnumber))
                {
                    lbSubmit.Text = "未知的原材料编号和客户物料编号！";
                    return;
                }

                sql = string.Format(@" select * from ScarpWarehouseLogDetail where WarehouseNumber='{0}' and MaterialNumber='{1}'", WarehouseNumber, materialnumber);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "已存在该原材料编号！请重新选择！";
                    return;
                }
                sql = string.Format(@" insert into ScarpWarehouseLogDetail (WarehouseNumber,MaterialNumber,Qty,Remark,CustomerMarterilNumber )
 values('{0}','{1}','{2}','{3}','{4}')", WarehouseNumber, materialnumber, Qty, Remark, CustomerMaterialnumber);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败，原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加废品出入库明细" + WarehouseNumber, "增加成功");
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加废品出入库明细" + WarehouseNumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {

                sql = string.Format(@" select * from ScarpWarehouseLogDetail where WarehouseNumber='{0}' and MaterialNumber='{1}'", WarehouseNumber, Server.UrlDecode(ToolManager.GetQueryString("MaterialNumber")));
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该废品出入库明细表信息已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" update ScarpWarehouseLogDetail set Qty='{0}',
                Remark='{1}' where WarehouseNumber='{2}' and MaterialNumber='{3}'",
                 Qty, Remark, WarehouseNumber, Server.UrlDecode(ToolManager.GetQueryString("MaterialNumber")));
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败，原因：" + error;
                if (result)
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑废品出入库明细" + WarehouseNumber, "编辑成功");
                    return;

                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "编辑废品出入库明细" + WarehouseNumber, "编辑失败！原因" + error);
                    return;
                }
            }
        }

    }
}
