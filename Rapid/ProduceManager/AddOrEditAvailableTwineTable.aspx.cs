using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditAvailableTwineTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }
        private void LoadPage()
        {

            string sql = string.Empty;
            string error = string.Empty;
            if (ToolManager.CheckQueryString("Guid"))
            {
                string guid = Server.UrlDecode(ToolManager.GetQueryString("Guid"));
                sql = string.Format(@"select * from AvailableTwineTable where Guid='{0}'",guid);
                  DataTable dt = SqlHelper.GetTable(sql);
                  if (dt.Rows.Count > 0)
                  {
                      DataRow dr = dt.Rows[0];
                      this.lblCustomerMaterialNumber.Text = dr["CutomerMaterialNumber"] == null ? "" : dr["CutomerMaterialNumber"].ToString();
                      this.lblLength.Text = dr["Length"] == null ? "" : dr["Length"].ToString();
                      this.txtQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                      this.txtDescription.Text = dr["Description"] == null ? "" : dr["Description"].ToString();
                      this.txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                      btnSubmit.Text = "修改";
                      txtCustomerMaterialNumber.Visible = false;
                      txtLength.Visible = false;
                  }
               

            }
            else
            {
                btnSubmit.Text = "添加";
                lblLength.Visible = false;
                lblCustomerMaterialNumber.Visible = false;
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            string error = string.Empty;
            string materialnumber = string.Empty;
            string customermaterialnumber = this.txtCustomerMaterialNumber.Text.Trim();
            string length = this.txtLength.Text.Trim();
            string qty = this.txtQty.Text.Trim();
            string description = this.txtDescription.Text.Trim();
            string remark = this.txtRemark.Text.Trim();
            string guid = ToolManager.GetQueryString("Guid");
            if (btnSubmit.Text == "添加")
            {
                sql = string.Format(@"select MaterialNumber from MaterialCustomerProperty where CustomerMaterialNumber='{0}'", customermaterialnumber);
                materialnumber = SqlHelper.GetScalar(sql);
                if (materialnumber.Equals(""))
                {
                    lbSubmit.Text = "未知的客户物料编号！";
                    return;
                }
                sql = string.Format(@"insert into AvailableTwineTable(MaterialNumber,CutomerMaterialNumber,Length,Qty,Description,Remark) 
values('{0}','{1}','{2}',{3},'{4}','{5}')", materialnumber, customermaterialnumber, length, qty, description, remark);
                if (SqlHelper.ExecuteSql(sql, ref error))
                {
                    lbSubmit.Text = "添加成功！";
                    return;
                }
                else
                {
                    lbSubmit.Text = "添加失败！原因:" + error;
                    return;
                }
            }
            else
            {
                sql = string.Format(@"update AvailableTwineTable set Qty={0},Description='{1}',Remark='{2}' where Guid='{3}'",
                    qty, description, remark, guid);
                if (SqlHelper.ExecuteSql(sql, ref error))
                {
                    lbSubmit.Text = "修改成功！";
                    return;
                }
                else
                {
                    lbSubmit.Text = "修改失败！原因:" + error;
                    return;
                }
            }
        }
    }
}
