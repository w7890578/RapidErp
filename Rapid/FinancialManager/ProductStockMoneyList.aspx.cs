using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.FinancialManager
{
    public partial class ProductStockMoneyList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0503", "Print");
                Bind();
            }
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
            string sql = string.Format(@"
            select p.产成品编号 as 产成品编号,p.版本 as 版本,p.描述 as 描述 ,t.库存数量,
CAST( p.成本价 as varchar(10)) as 单价,t.库存数量*p.成本价 as 库存金额 from V_Product p 
inner join 
(
select SUM(StockQty) as 库存数量,ProductNumber,Version from ProductStock   group by ProductNumber,Version)t on
p.产成品编号=t.ProductNumber and p.版本=t.Version {0}
union
select '合计','','',SUM(a.库存数量),'',SUM(a.库存金额) from 
(select p.产成品编号 as 产成品编号,p.版本 as 版本,p.描述 as 描述 ,
t.库存数量,p.成本价 as 单价,t.库存数量*p.成本价 as 库存金额 from V_Product p 
inner join 
(
select SUM(StockQty) as 库存数量,ProductNumber,Version from ProductStock connection group by ProductNumber,Version)t on
p.产成品编号=t.ProductNumber and p.版本=t.Version  {0})a
            ", conection);
            return sql;
        }

        private void Bind()
        { 
            this.rpList.DataSource = SqlHelper.GetTable(GetSql());
            this.rpList.DataBind();
           
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(), "产成品库存总金额表");
        }
    }
}

