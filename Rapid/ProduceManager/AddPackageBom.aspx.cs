using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using BLL;
using Rapid.ToolCode;


namespace Rapid.ProduceManager
{
    public partial class AddPackageBom : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
            lblPackageNumber.Text = ToolManager.GetQueryString("PackageNumber");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string packagenumber = lblPackageNumber.Text;
            string productnumber = string.Empty;
            string version = string.Empty;
            string singledose = txtSingleDose.Text;
            string sql = string.Empty;
            string customerProductNumber = txtCustomerProductNumber.Text.Trim();
            sql = string.Format(@" 
select ProductNumber,Version  from ProductCustomerProperty where CustomerProductNumber='{0}' ", customerProductNumber);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count == 0)
            {
                lbSubmit.Text = string.Format(" 系统不存在客户产成品编号:{0} ", customerProductNumber);
                return;
            }
            productnumber = dt.Rows[0]["ProductNumber"].ToString();
            version = dt.Rows[0]["Version"].ToString();
            string error = string.Empty;
            string num = string.Empty;
            sql = string.Format(@"select count(*) from Product where  ProductNumber='{0}' and Version='{1}' and Type!='包'", productnumber, version);
            num = SqlHelper.GetScalar(sql);
            if (num == "0")
            {
                lbSubmit.Text = "未知的产品及版本！";
                return;
            }
            sql = string.Format(@"select count(*) from PackageAndProductRelation where PackageNumber='{0}' and  ProductNumber='{1}' and Version='{2}'", packagenumber, productnumber, version);
            num = SqlHelper.GetScalar(sql);
            if (num != "0")
            {
                lbSubmit.Text = "已存此产品及版本，请重新填写！";
                return;
            }
            sql = string.Format(@"insert into PackageAndProductRelation(PackageNumber,ProductNumber,Version,SingleDose)
values ('{0}','{1}','{2}','{3}')", packagenumber, productnumber, version, singledose);
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加包与产品对应关系信息" + packagenumber, "增加成功！");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "增加包与产品对应关系信息" + packagenumber, "增加失败！原因：" + error);
                return;
            }
        }
    }
}
