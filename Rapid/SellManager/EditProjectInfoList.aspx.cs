using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.SellManager
{
    public partial class EditProjectInfoList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Id"))
                {
                    string key = ToolManager.GetQueryString("Id");
                    string[] keyTemp = key.Split('|');

                    string sql = string.Format(@" select *  from T_ProjectInfo 
where ProjectName ='{0}' and ProductNumber ='{1}' and Version ='{2}' and CustomerProductNumber='{3}' ",
     keyTemp[0], keyTemp[1], keyTemp[2], keyTemp[3]);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        lbProjectName.Text = dr["ProjectName"].ToString();
                        lbProductNumber.Text = dr["ProductNumber"].ToString();
                        lbVersion.Text = dr["Version"].ToString();
                        lbCustomerProductNumber.Text = dr["CustomerProductNumber"].ToString();
                        txtSingle.Text = dr["Single"] == null ? "" : dr["Single"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string projectName = lbProjectName.Text;
            string productNumber = lbProductNumber.Text;
            string version = lbVersion.Text;
            string customerProductNumber = lbCustomerProductNumber.Text;
            string single = txtSingle.Text;
            string remark = txtRemark.Text;
            string sql = string.Format(@"update T_ProjectInfo set Single ='{4}' ,Remark ='{5}' 
where ProjectName ='{0}' and ProductNumber ='{1}' and Version ='{2}' and CustomerProductNumber='{3}' ",
    projectName, productNumber, version, customerProductNumber, single, remark);
            string error = string.Empty;
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            if (result)
            {
                Response.Write(ToolManager.GetClosePageJS());
                return;
            }
            else
            {
                lbSubmit.Text = "修改失败！原因：" + error;
                return;
            }
        }
    }
}
