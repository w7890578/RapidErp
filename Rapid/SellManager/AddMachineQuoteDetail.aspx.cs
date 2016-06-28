using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class AddMachineQuoteDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("QuoteNumber"))
                {
                    this.lbSubmit.Text = "未知报价单！";
                    return;
                }
                LoadPage();
            }

        }
        private void LoadPage()
        {
            this.lbQuoteNumber.Text = ToolManager.GetQueryString("QuoteNumber");
            string sql = string.Format(@" select distinct(ProductName+'|'+Version) as Text, (ProductNumber+'|'+Version ) as Value 
            from Product p right join V_MachineQuoteDetailProduct vm on p.ProductNumber=vm.产品编号 and p.Version=vm.版本");
            this.liPackageNumber.DataSource = SqlHelper.GetTable("select distinct 包号,包名称 from V_MachineQuoteDetail");
            this.liPackageNumber.DataTextField = "包名称";
            this.liPackageNumber.DataValueField = "包号";
            this.liPackageNumber.DataBind();
            this.liProductNumberAndVersion.DataSource = SqlHelper.GetTable(sql);
            this.liProductNumberAndVersion.DataTextField = "Text";
            this.liProductNumberAndVersion.DataValueField = "Value";
            this.liProductNumberAndVersion.DataBind();

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ToolManager.CheckQueryString("QuoteNumber"))
            {
                this.lbSubmit.Text = "未知报价单！";
                return;
            }
            string quotenumber = ToolManager.GetQueryString("QuoteNumber");
            string error = string.Empty;
            string packagenumber = string.Empty;
            string productandversion = string.Empty;//列表选中的值
            foreach (ListItem item in liPackageNumber.Items) //按类型listitem读取listbox中选定项，包号
            {
                if (item.Selected) //判断是否选中包号
                {
                    packagenumber += string.Format("'{0}',", item.Value);

                }

            }

            foreach (ListItem item in liProductNumberAndVersion.Items)
            {
                if (item.Selected) //判断是否选中产品、版本
                {
                    productandversion += string.Format("{0},", item.Value);

                }

            }
            if (packagenumber.Length <= 0 && productandversion.Length <= 0)
            {

                lbSubmit.Text = "请选择要添加的包或产品！！！";

            }
            List<string> list = new List<string>();
            if (packagenumber.Length > 0)
            {
                packagenumber = packagenumber.Substring(0, packagenumber.Length - 1);
                string[] packagenumbers = packagenumber.Split(',');
                foreach (string packageNumber in packagenumbers)
                {
                    packagenumber = string.Format(" exec P_MachineQuoteDetail '" + quotenumber + "', {0} ", packageNumber);
                    list.Add(packagenumber);
                }
                bool result = SqlHelper.BatchExecuteSql(list, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加加工报价单明细包信息" + packagenumber, "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加加工报价单明细包信息" + packagenumber, "增加失败！原因：" + error);
                    return;
                }

            }
            List<string> li = new List<string>();
            if (productandversion.Length > 0)
            {
                productandversion = productandversion.Substring(0, productandversion.Length - 1);
                string[] productandversions = productandversion.Split(',');//（产品编号|版本）的一个数组
                foreach (string productandVersion in productandversions)
                {
                    string[] productandversionreal = productandVersion.Split('|');//（产品编号 版本）的一个数组
                    productandversion = string.Format(@" exec P_MachineQuoteDetailProduct '" + quotenumber + "','{0}','{1}' ",
                    productandversionreal[0], productandversionreal[1]);
                    li.Add(productandversion);

                }
                bool result = SqlHelper.BatchExecuteSql(li, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加加工报价单明细产品信息" + productandversion, "增加成功");
                    return;
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加加工报价单明细产品信息" + productandversion, "增加失败！原因：" + error);
                    return;
                }

            }

        }
    }
}