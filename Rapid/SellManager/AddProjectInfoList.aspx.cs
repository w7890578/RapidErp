using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.SellManager
{
    public partial class AddProjectInfoList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string projectName = txtProjectName.Text.Trim();
            string productNumber = txtProductNumber.Text.Trim();
            string version = txtVersion.Text.Trim();
            string customerProductNumber = txtCustomerProductNumber.Text;
            string single = txtSingle.Text.Trim();
            string remark = txtRemark.Text;
            string sql = string.Empty;
            sql = string.Format(@"select count(*) from ProductCustomerProperty where CustomerProductNumber='{0}' and Version='{1}'", txtCustomerProductNumber.Text,version);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = "未知的客户产成品编号和版本！";
                return;
            }
            sql = string.Format(@"select ProductNumber,Version from ProductCustomerProperty where CustomerProductNumber='{0}' and Version='{1}'", txtCustomerProductNumber.Text,version );
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count > 0)
            {
                productNumber = dt.Rows[0]["ProductNumber"].ToString();
                version = dt.Rows[0]["Version"].ToString();
            }
            else
            {
                lbSubmit.Text = "未知的产成品编号及版本！";
                return;
            }
             sql = string.Format(@"select COUNT(*) from T_ProjectInfo where ProjectName ='{0}' 
and ProductNumber ='{1}' and Version ='{2}' and CustomerProductNumber ='{3}'", projectName, productNumber, version, customerProductNumber);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = "已存在相同记录，请重新填写！";
                return;
            }
            sql = string.Format(@" select count(*)  from Product  
where ProductNumber ='{0}' and Version ='{1}' ",productNumber ,version );
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = "系统不存在该产成品信息，请在产成品信息中添加该产品。";
                return;
            }
            sql = string.Format(@"insert into T_ProjectInfo (ProjectName,ProductNumber,Version ,CustomerProductNumber,Hierarchy ,Description ,Single,Remark )
select '{0}','{1}','{2}','{3}', case Type when '包' then '0'  else '1' end as 阶层,Description as 描述,'{4}','{5}'   from Product  
where ProductNumber ='{1}' and Version ='{2}'", projectName, productNumber, version, customerProductNumber, single, remark);
            string error = string.Empty;
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            if (result)
            {
                lbSubmit.Text = "添加成功";
                return;
            }
            else
            {
                lbSubmit.Text = "添加失败！原因：" + error;
                return;
            }
        }
    }
}
