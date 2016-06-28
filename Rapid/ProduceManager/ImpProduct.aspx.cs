using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Model;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class ImpProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
            if (ds == null)
            {
                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
                return;
            }
            List<Product> products = new List<Product>();
            List<ProductCustomerProperty> pcps = new List<ProductCustomerProperty>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Product product = new Product();
                product.ProductNumber = dr["产成品编号"] == null ? "" : dr["产成品编号"].ToString();
                product.Version = dr["版本"] == null ? "" : dr["版本"].ToString(); ;
                product.ProductName = dr["名称"] == null ? "" : dr["名称"].ToString();
                product.Type = dr["类别"] == null ? "" : dr["类别"].ToString();
                product.Description = dr["描述"] == null ? "" : dr["描述"].ToString();
                if (dr["额定工时"] == null || dr["额定工时"].ToString() == "")
                {
                    product.RatedManhour = 0;
                }
                else
                {
                    product.RatedManhour = Convert.ToInt32(dr["额定工时"].ToString());
                }
                if (dr["报价工时"] == null || dr["报价工时"].ToString() == "")
                {
                    product.QuoteManhour = 0;

                }
                else
                {
                    product.QuoteManhour = Convert.ToInt32(dr["报价工时"].ToString());
                }
                if (dr["成本价"] == null || dr["成本价"].ToString() == "")
                {
                    product.CostPrice = Convert.ToDecimal(0.00);
                }
                else
                {
                    product.CostPrice = Convert.ToDecimal(dr["成本价"].ToString());
                }
                if (dr["销售报价"] == null || dr["销售报价"].ToString() == "")
                {
                    product.SalesQuotation = Convert.ToDecimal(0.00);

                }
                else
                {
                    product.SalesQuotation = Convert.ToDecimal(dr["销售报价"].ToString());
                }

                product.HalfProductPosition = dr["半成品仓位"] == null ? "" : dr["半成品仓位"].ToString();
                product.FinishedGoodsPosition = dr["产成品仓位"] == null ? "" : dr["产成品仓位"].ToString();
                product.Remark = dr["备注"] == null ? "" : dr["备注"].ToString();
                products.Add(product);

                ProductCustomerProperty pcp = new ProductCustomerProperty();
                pcp.ProductNumber = dr["产成品编号"] == null ? "" : dr["产成品编号"].ToString();
                pcp.Version = dr["版本"] == null ? "" : dr["版本"].ToString();
                pcp.CustomerId = dr["客户编号"] == null ? "" : dr["客户编号"].ToString();
                pcp.CustomerProductNumber = dr["客户产成品编号"] == null ? "" : dr["客户产成品编号"].ToString();
                pcps.Add(pcp);
            }
            bool result = ProductManager.BatchAddData(products, ref error);
            bool resultCustomerProduct = ProductCustomerPropertyManager.BatchAddData(pcps, ref error);
            if (result == true || resultCustomerProduct == true)
            {
                lbMsg.Text = "导入成功！";

            }
            else
            {
                lbMsg.Text = "导入失败！原因" + error;

            }
            if (result == true || resultCustomerProduct == true)
            {

                Tool.WriteLog(Tool.LogType.Operating, "导入产品信息", "导入成功！");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入产品信息", "导入失败！原因" + error);
                return;
            }

        }
    }
}
