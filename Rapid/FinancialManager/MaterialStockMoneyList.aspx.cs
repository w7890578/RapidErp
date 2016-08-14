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
    public partial class MaterialStockMoneyList : System.Web.UI.Page
    {
        public static string ConverHtml(string str)
        {
            return str.Replace(",", "<br/>");
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = GetTable();
            if (dt != null)
            {
                ToolCode.Tool.ExpExcel(dt, "原材料库存金额报表");
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
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0505", "Print");
                Bind();
            }
        }

        private void Bind()
        {
            this.rpList.DataSource = GetTable();
            this.rpList.DataBind();
        }

        private string GetCustomerNames(Dictionary<string, Dictionary<string, string>> marerialCustomerNames, string key)
        {
            string filerCustomerIds = "R025,R028,R026";
            string result = string.Empty;
            if (marerialCustomerNames.ContainsKey(key))
            {
                var temp = marerialCustomerNames[key];
                foreach (var item in temp)
                {
                    if (filerCustomerIds.Contains(item.Key))
                    {
                        result += "," + item.Value;
                    }
                }
            }
            return result;
        }

        private string GetSql()
        {
            string conection = "where 1=1";
            if (txtMaterialNumber.Text != "")
            {
                conection += " and m.MaterialNumber like'%" + txtMaterialNumber.Text + "%'";
            }
            if (txtCargoType.Text.Trim() != "")
            {
                conection += string.Format(" and m.CargoType like '%{0}%'", txtCargoType.Text.Trim());
            }

            //            string sql = string.Format(@" select m.MaterialNumber as 原材料编号, m.Description as 描述 ,
            // t.库存数量,
            // CAST(a.单价 as varchar(10)) as 单价,
            // t.库存数量*a.单价 as 库存金额
            // from MarerialInfoTable m inner join
            //(select SUM(StockQty) as 库存数量,MaterialNumber from MaterialStock  group by MaterialNumber )t
            //on m.MaterialNumber=t.MaterialNumber
            //inner join (select MaterialNumber,MIN (Prcie ) as 单价 from MaterialSupplierProperty group by MaterialNumber) a
            //on a.MaterialNumber =m.MaterialNumber  {0}
            //union
            //
            //select '合计','',SUM(a.库存数量),'',SUM (a.库存金额) from (
            //select m.MaterialNumber as 原材料编号, m.Description as 描述 ,
            // t.库存数量,
            // m.ProcurementPrice as 单价,
            // t.库存数量*m.ProcurementPrice as 库存金额
            // from MarerialInfoTable m inner join
            //(select SUM(StockQty) as 库存数量,MaterialNumber from MaterialStock  group by MaterialNumber)t
            //on m.MaterialNumber=t.MaterialNumber  {0}) a" ,conection);
            string sql = string.Format(@"
select m.MaterialNumber as 原材料编号, m.Description as 描述 ,
 t.库存数量,
 CAST(a.单价 as varchar(10)) as 单价,
 t.库存数量*a.单价 as 库存金额,ISNULL( m.CargoType,'') as 货物类型
 from MarerialInfoTable m inner join
(select SUM(StockQty) as 库存数量,MaterialNumber from MaterialStock  group by MaterialNumber )t
on m.MaterialNumber=t.MaterialNumber
inner join (select MaterialNumber,MIN (Prcie ) as 单价 from MaterialSupplierProperty group by MaterialNumber) a
on a.MaterialNumber =m.MaterialNumber  {0}
union all
select '合计','',SUM (t.库存数量),'',SUM( t.库存金额),'' from (select m.MaterialNumber as 原材料编号, m.Description as 描述 ,
 t.库存数量,
 CAST(a.单价 as varchar(10)) as 单价,
 t.库存数量*a.单价 as 库存金额,ISNULL( m.CargoType,'') as 货物类型
 from MarerialInfoTable m inner join
(select SUM(StockQty) as 库存数量,MaterialNumber from MaterialStock  group by MaterialNumber )t
on m.MaterialNumber=t.MaterialNumber
inner join (select MaterialNumber,MIN (Prcie ) as 单价 from MaterialSupplierProperty group by MaterialNumber) a
on a.MaterialNumber =m.MaterialNumber  {0})t", conection);
            return sql;
        }

        private DataTable GetTable()
        {
            Dictionary<string, Dictionary<string, string>> marerialCustomerNames = MaterialCustomerPropertyManager.GetMarerialCustomerNames();

            DataTable dtResult = SqlHelper.GetTable(GetSql());
            dtResult.Columns.Add("CustomerNames");
            foreach (DataRow dr in dtResult.Rows)
            {
                if (marerialCustomerNames.ContainsKey(dr["原材料编号"].ToString().Trim()))
                {
                    dr["CustomerNames"] = GetCustomerNames(marerialCustomerNames, dr["原材料编号"].ToString().Trim()); ;
                }
            }
            return dtResult;
        }
    }
}