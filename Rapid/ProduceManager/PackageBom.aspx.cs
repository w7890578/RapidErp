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
    public partial class PackageBom : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = string.Empty;
            string error = string.Empty;
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("PackageNumber") && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
                {
                    string packagenumber = ToolManager.GetQueryString("PackageNumber");
                    string productnumber = ToolManager.GetQueryString("ProductNumber");
                    string version = ToolManager.GetQueryString("Version");

                    sql = string.Format(@"delete PackageAndProductRelation where PackageNumber='{0}' and ProductNumber='{1}' and Version='{2}'", packagenumber, productnumber, version);
                    bool result = SqlHelper.ExecuteSql(sql, ref error);
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除包与产品对应关系信息" + ToolManager.ReplaceSingleQuotesToBlank(packagenumber), "删除成功");
                        Response.Write("1");
                        Response.End();
                        return;

                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "删除包与产品对应关系信息" + ToolManager.ReplaceSingleQuotesToBlank(packagenumber), "删除失败！原因" + error);
                        Response.Write(error);
                        Response.End();
                        return;

                    }
                }
                Bind();
            }

            // sql = string.Format(@"select PackageNumber as 包编号,ProductNumber as 产成品编号,Version as 版本 from PackageAndProductRelation");
            //rpList.DataSource = SqlHelper.GetTable(sql);
            //rpList.DataBind();
        }
        private void Bind()
        {
            string sql = string.Empty;
            string error = string.Empty;
            string packagenumber = ToolManager.GetQueryString("Id");
            sql = string.Format(@"
select 
par.PackageNumber ,
pcp.CustomerProductNumber as 客户包号,
par.ProductNumber,
par.Version,
bom.customnerProductNumber as 客户产成品号,
bom.MaterialNumber,
bom.CustomerMaterialNumber,
case when bom.Unit='mm' then
bom.SingleDose*1000 else bom.SingleDose end SingleDose,
bom.Unit
from PackageAndProductRelation par
inner join BOMInfo bom  on par.ProductNumber =bom.ProductNumber and par.Version =bom.Version
left join (select top 1* from  ProductCustomerProperty ) pcp on par.PackageNumber =pcp.ProductNumber 
where par.PackageNumber ='{0}'
 ", packagenumber);
            if (txtCustomnerProductNumber.Text.Trim() != "")
            {
                sql += string.Format(" and bom.CustomnerProductNumber like '%{0}%' ", txtCustomnerProductNumber.Text.Trim());
            }
            if (txtCustomerMaterialNumber.Text.Trim() != "")
            {
                sql += string.Format(" and bom.CustomerMaterialNumber like '%{0}%' ", txtCustomerMaterialNumber.Text.Trim());
            }
            sql += @"
order by  
par.PackageNumber asc,
par.ProductNumber asc,
bom.MaterialNumber asc";
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
