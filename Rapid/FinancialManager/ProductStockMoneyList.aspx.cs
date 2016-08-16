using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.FinancialManager
{
    public partial class ProductStockMoneyList : System.Web.UI.Page
    {
        public static string ConverHtml(string str)
        {
            return str.Replace(",", "<br/>");
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            DataTable dtResult = GetTable();
            if (dtResult != null)
            {
                ToolCode.Tool.ExpExcel(dtResult, "产成品库存总金额表");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0503", "Print");
                Bind();
            }
        }

        private void Bind()
        {
            DataTable dtResult = GetTable();
            this.rpList.DataSource = dtResult;
            this.rpList.DataBind();
        }

        private string GetCustomerNames(Dictionary<string, Dictionary<string, string>> productCustomerNames, string key)
        {
            string result = string.Empty;
            if (productCustomerNames.ContainsKey(key))
            {
                var temp = productCustomerNames[key];
                foreach (var item in temp)
                {
                    result += "," + item.Value;
                }
            }
            return result;
        }

        private string GetSql()
        {
            string conection = "where 1=1";
            if (txtProductnumber.Text != "")
            {
                conection = "and t.ProductNumber='" + txtProductnumber.Text + "'";
            }
            //            string sql = string.Format(@"
            //select p.ProductNumber as 产成品编号,p.Version as 版本,p.Description as 描述 ,t.库存数量,
            //CAST( p.CostPrice as varchar(10)) as 单价,t.库存数量*p.CostPrice as 库存金额 from Product p
            //inner join
            //(
            //select SUM(StockQty) as 库存数量,ProductNumber,Version from ProductStock connection group by ProductNumber,Version)t on
            //p.ProductNumber=t.ProductNumber and p.Version=t.Version {0}
            //union
            //select '合计','','',SUM(a.库存数量),'',SUM(a.库存金额) from
            //(select p.ProductNumber as 产成品编号,p.Version as 版本,p.Description as 描述 ,
            //t.库存数量,p.CostPrice as 单价,t.库存数量*p.CostPrice as 库存金额 from Product p
            //inner join
            //(
            //select SUM(StockQty) as 库存数量,ProductNumber,Version from ProductStock connection group by ProductNumber,Version)t on
            //p.ProductNumber=t.ProductNumber and p.Version=t.Version  {0})a
            //" , conection);

            //            union
            //select '合计','','',SUM(a.库存数量),'',SUM(a.库存金额) from
            //(select p.产成品编号 as 产成品编号, p.版本 as 版本, p.描述 as 描述,
            //t.库存数量, p.成本价 as 单价, t.库存数量 * p.成本价 as 库存金额 from V_Product p
            //    inner
            //                                            join
            //                                      (
            //select SUM(StockQty) as 库存数量, ProductNumber, Version from ProductStock connection group by ProductNumber, Version)t on
            //p.产成品编号 = t.ProductNumber and p.版本 = t.Version  { 0})a

            string sql = string.Format(@"
            select p.产成品编号 as 产成品编号,p.版本 as 版本,p.描述 as 描述 ,t.库存数量,
CAST( p.成本价 as varchar(10)) as 单价,t.库存数量*p.成本价 as 库存金额 from V_Product p
inner join
(
select SUM(StockQty) as 库存数量,ProductNumber,Version from ProductStock   group by ProductNumber,Version)t on
p.产成品编号=t.ProductNumber and p.版本=t.Version {0}

            ", conection);
            return sql;
        }

        private DataTable GetTable()
        {
            Dictionary<string, Dictionary<string, string>> productCustomerNames = ProductCustomerPropertyManager.GetProductCustomerNames();

            DataTable dtResult = SqlHelper.GetTable(GetSql());
            dtResult.Columns.Add("CustomerNames");
            foreach (DataRow dr in dtResult.Rows)
            {
                string key = dr["产成品编号"] + "|" + dr["版本"];
                if (productCustomerNames.ContainsKey(key))
                {
                    dr["CustomerNames"] = GetCustomerNames(productCustomerNames, key); ;
                }
            }
            string customerName = txtCustomerName.Text.Trim();
            if (!string.IsNullOrEmpty(customerName))
            {
                DataRow[] tempRows = dtResult.Select("CustomerNames like '%" + customerName + "%'");

                if (tempRows != null && tempRows.Length > 0)
                {
                    dtResult = tempRows.CopyToDataTable<DataRow>();
                }
                else
                {
                    dtResult.Rows.Clear();
                }
            }
            if (dtResult != null && dtResult.Rows != null && dtResult.Rows.Count > 0)
            {
                double sumStockQty = Convert.ToDouble(dtResult.Compute("sum(库存数量)", "true"));
                double sumStockPrice = Convert.ToDouble(dtResult.Compute("sum(库存金额)", "true"));

                DataRow sumRow = dtResult.NewRow();
                sumRow["库存数量"] = sumStockQty;
                sumRow["库存金额"] = sumStockPrice;
                dtResult.Rows.Add(sumRow);
            }
            return dtResult;
        }
    }
}