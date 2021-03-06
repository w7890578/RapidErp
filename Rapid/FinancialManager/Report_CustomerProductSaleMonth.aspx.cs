﻿using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.FinancialManager
{
    public class MonthMateril
    {
        public double April { get; set; }
        public double August { get; set; }
        public double Count { get; set; }
        public double CountPrice { get; set; }
        public string CustomerMaterilNumber { get; set; }
        public double December { get; set; }
        public double February { get; set; }
        public double January { get; set; }
        public double July { get; set; }
        public double June { get; set; }
        public double March { get; set; }
        public double May { get; set; }
        public double November { get; set; }
        public double October { get; set; }
        public double September { get; set; }
        public double UnitPrice { get; set; }
    }

    public class MonthModel
    {
        public Dictionary<string, double> MaterialCounts = new Dictionary<string, double>();

        public MonthModel(string englishName, string month, string chineseName)
        {
            this.EnglishName = englishName;
            this.Month = month;
            this.ChineseName = chineseName;
        }

        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public string Month { get; set; }
    }

    public partial class Report_CustomerProductSaleMonth : System.Web.UI.Page
    {
        public static DataTable DtBomInfo = new DataTable();
        public static DataTable DtProduct = new DataTable();
        public static DataTable DtResult = new DataTable();
        public static List<MonthMateril> MonthMaterils = new List<MonthMateril>();

        protected void btnBom_Click(object sender, EventArgs e)
        {
            SetMonthMaterils();
        }

        protected void btnExp_Click(object sender, EventArgs e)
        {
            string filePath = Server.MapPath("~/Upload/客户产成品销售月度报表.xls");
            Dictionary<string, DataTable> results = new Dictionary<string, DataTable>();
            SetMonthMaterils();
            DataTable dtBOM = ConverToDataTable(MonthMaterils);
            results.Add("客户产成品销售月报表", DtResult);
            results.Add("产成品分解为原材料销售月度报表", dtBOM);
            ExcelHelper.Instance.ExpExcel(filePath, results);
            //ExcelHelper.Instance.ExpExcel(DtResult, "客户产成品销售月度报表");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            MonthMaterils = new List<MonthMateril>();
            Bind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { Bind(); }
        }

        private void Bind()
        {
            MonthMaterils = new List<MonthMateril>();
            DtResult = GetTable();
        }

        private DataTable ConverToDataTable(List<MonthMateril> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("客户物料号");
            dt.Columns.Add("1月份");
            dt.Columns.Add("2月份");
            dt.Columns.Add("3月份");
            dt.Columns.Add("4月份");
            dt.Columns.Add("5月份");
            dt.Columns.Add("6月份");
            dt.Columns.Add("7月份");
            dt.Columns.Add("8月份");
            dt.Columns.Add("9月份");
            dt.Columns.Add("10月份");
            dt.Columns.Add("11月份");
            dt.Columns.Add("12月份");
            dt.Columns.Add("数量合计");
            dt.Columns.Add("采购单价");
            dt.Columns.Add("采购单价合计");
            var temp = list.OrderByDescending(t => t.Count).ToList();
            foreach (MonthMateril model in temp)
            {
                DataRow dr = dt.NewRow();
                dr[0] = model.CustomerMaterilNumber;
                dr[1] = model.January;
                dr[2] = model.February;
                dr[3] = model.March;
                dr[4] = model.April;
                dr[5] = model.May;
                dr[6] = model.June;
                dr[7] = model.July;
                dr[8] = model.August;
                dr[9] = model.September;
                dr[10] = model.October;
                dr[11] = model.November;
                dr[12] = model.December;
                dr[13] = model.Count;
                dr[14] = model.UnitPrice;
                dr[15] = model.CountPrice;
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private DataTable GetBomInfo()
        {
            string sql = @"
select 包号 PackgeName,产成品编号 ProductNumber,版本 Version,原材料编号 MateriNumber,单机用量 UnitQty,客户物料号 CustomerMateriNumber
 from [V_BOM_Count]";
            return SqlHelper.GetTable(sql);
        }

        private Dictionary<string, double> GetMateriNumbers(string productNumber, string version, double qty, string customerProductNumber)
        {
            Dictionary<string, double> materiNumbers = new Dictionary<string, double>();
            //原材料没有version
            if (string.IsNullOrEmpty(version))
            {
                if (materiNumbers.ContainsKey(customerProductNumber))
                {
                    materiNumbers[customerProductNumber] += qty;
                }
                else
                {
                    materiNumbers.Add(customerProductNumber, qty);
                }
                return materiNumbers;
            }

            DataRow[] drs = DtProduct.Select("ProductNumber='" + productNumber + "' and Version='" + version + "'");
            if (drs != null && drs.Count() > 0)
            {
                string type = drs[0]["Type"].ToString();
                DataRow[] drTemps = null;
                if (type.Equals("包"))
                {
                    drTemps = DtBomInfo.Select("PackgeName='" + productNumber + "'");
                }
                else
                {
                    drTemps = DtBomInfo.Select("ProductNumber='" + productNumber + "' and Version='" + version + "'");
                }

                if (drTemps != null && drTemps.Count() > 0)
                {
                    foreach (DataRow tempRow in drTemps)
                    {
                        if (materiNumbers.ContainsKey(tempRow["CustomerMateriNumber"].ToString()))
                        {
                            materiNumbers[tempRow["CustomerMateriNumber"].ToString()] +=
                                Convert.ToDouble(tempRow["UnitQty"].ToString()) * qty;
                        }
                        else
                        {
                            materiNumbers.Add(tempRow["CustomerMateriNumber"].ToString(), Convert.ToDouble(tempRow["UnitQty"].ToString()) * qty);
                        }
                    }
                }
            }
            return materiNumbers;
        }

        private DataTable GetProduct()
        {
            string sql = "select ProductNumber,Version,Type from Product";
            return SqlHelper.GetTable(sql);
        }

        /// <summary>
        /// 获取客户物料编号对应的单价
        /// </summary>
        /// <returns></returns>
        private List<MonthMateril> GetRsultModel()
        {
            List<MonthMateril> list = new List<MonthMateril>();
            //            string sql = @"
            //select m. MaterialNumber,ISNULL(t.Prcie,0) Prcie from MarerialInfoTable m left join
            //(select MaterialNumber,max(Prcie)  Prcie from MaterialSupplierProperty group by MaterialNumber)
            //t on t.MaterialNumber=m.MaterialNumber";
            string sql = @"select t.CustomerMaterialNumber,max(t.Prcie) Prcie from (
select distinct mp.CustomerMaterialNumber,isnull( ms.Prcie,0) Prcie from MaterialCustomerProperty  mp
inner join MaterialSupplierProperty ms
on mp.MaterialNumber=ms.MaterialNumber)t group by t.CustomerMaterialNumber";
            DataTable dt = SqlHelper.GetTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(new MonthMateril()
                {
                    CustomerMaterilNumber = dr["CustomerMaterialNumber"].ToString(),
                    UnitPrice = Convert.ToDouble(dr["Prcie"].ToString())
                });
            }
            return list;
        }

        private DataTable GetTable()
        {
            string year = drpYear.SelectedValue;
            string customerName = txtCustomerName.Text.Trim();

            StringBuilder condition = new StringBuilder(" where 1=1 ");
            StringBuilder sbMonthSql = new StringBuilder();
            StringBuilder sbMonthTitleSql = new StringBuilder();
            StringBuilder sbMonthLeftJoinSql = new StringBuilder();
            if (!string.IsNullOrEmpty(customerName))
            {
                condition.AppendFormat(" and C.CustomerName like '%{0}%'", customerName);
            }

            List<MonthModel> months = new List<MonthModel>() {
                new MonthModel ("January","01","1月份"),
                new MonthModel ("February","02","2月份"),
                new MonthModel ("March","03","3月份"),
                new MonthModel ("April","04","4月份"),
                new MonthModel ("May","05","5月份"),
                new MonthModel ("June","06","6月份"),
                new MonthModel ("July","07","7月份"),
                new MonthModel ("August","08","8月份"),
                new MonthModel ("September","09","9月份"),
                new MonthModel ("October","10","10月份"),
                new MonthModel ("November","11","11月份"),
                new MonthModel ("December","12","12月份")
            };

            foreach (MonthModel item in months)
            {
                sbMonthSql.AppendFormat(@",{2} as (
select CustomerProductNumber, ProductNumber, Version, sum(ISNULL(DeliveryQty, 0)) Qty from A where DeliveryDate like '%{0}-{1}%'
group by CustomerProductNumber, ProductNumber, Version)", year, item.Month, item.EnglishName);
                sbMonthTitleSql.AppendFormat(",ISNULL({0}.Qty,0)  as '{1}'", item.EnglishName, item.ChineseName);
                sbMonthLeftJoinSql.AppendFormat(" left join {0} on B.ProductNumber={0}.ProductNumber and B.CustomerProductNumber={0}.CustomerProductNumber and B.Version={0}.Version", item.EnglishName);
            }

            string sql = string.Format(@"
--总数据
with A as(
    select
	    产成品编号         as  ProductNumber,
	    客户产成品编号      as CustomerProductNumber,
	    版本              as version,
	    送货日期           as DeliveryDate,
	    数量              as  DeliveryQty
    from  V_DeliveryBill_Reprot
    where    送货日期 like '%{0}%'
),
--总数量
 B as (
	select dnd.ProductNumber,dnd.Version,dnd.CustomerProductNumber,  sum(ISNULL( dnd.DeliveryQty,0)) Qty  from A dnd  group by   dnd.ProductNumber,dnd.Version,dnd.CustomerProductNumber
),
--客户名称
C as (
	select   CustomerProductNumber,Version ,c.CustomerName from ProductCustomerProperty p inner join Customer c on p.CustomerId=c.CustomerId
),
--产成品销售未税单价
D as (
	select t.productNumber,t.Version,isnull(m.UnitPrice,0) UnitPrice,m.QuoteNumber from (
	select productNumber,version,MAX(QuoteNumber) QuoteNumber from MachineQuoteDetail  where    Hierarchy =0 and ISNULL(Version,'')!=''
	group by productNumber,version
	) t left join MachineQuoteDetail m on t.ProductNumber=m.ProductNumber and t.Version=m.Version and t.QuoteNumber=m.QuoteNumber
	where m.Hierarchy =0
),
--原材料销售未税单价
E as (
	select t.productNumber,'' as Version,isnull(tq.UnitPrice,0) UnitPrice,tq.QuoteNumber from (
	select ProductNumber,MAX(QuoteNumber) QuoteNumber  from TradingQuoteDetail
	group by productNumber)t inner join TradingQuoteDetail tq on t.ProductNumber=tq.ProductNumber and t.QuoteNumber=tq.QuoteNumber
)
--各个月份
{1}
select
--C.CustomerName as 客户名称,
B.CustomerProductNumber as 客户产成品编号,
B.ProductNumber as 产成品编号,
B.Version as 版本
{2},
B.Qty  as 数量合计,
case
    when isnull(B.Version,'')=''
        then E.UnitPrice
    else
        D.UnitPrice
    end  as 销售未税单价,
case
    when isnull(B.Version,'')=''
        then E.UnitPrice*B.Qty
    else
        D.UnitPrice*B.Qty
    end  as 销售未税合计
--,
--D.QuoteNumber as 最新报价单

from B
left join E on B.ProductNumber=E.ProductNumber and B.Version=E.Version
left join D on B.ProductNumber=D.ProductNumber and B.Version=D.Version
left join C on B.CustomerProductNumber=C.CustomerProductNumber and B.Version=C.Version
{3}
{4}
order by B.CustomerProductNumber,B.Version
", year, sbMonthSql.ToString(), sbMonthTitleSql.ToString(), sbMonthLeftJoinSql.ToString(), condition.ToString());
            DataTable dt = SqlHelper.GetTable(sql);
            List<string> columnNames = new List<string>();
            foreach (DataColumn item in dt.Columns)
            {
                columnNames.Add(item.ColumnName);
            }

            DataTable dtCopy = dt.Copy();

            DataView dv = dt.DefaultView;
            dv.Sort = "数量合计 DESC";
            dt = dv.ToTable(true, columnNames.ToArray());

            object obj = dt.Compute("sum(销售未税合计)", "true");
            DataRow drSumRow = dt.NewRow();
            drSumRow[0] = "合计";
            drSumRow["销售未税合计"] = obj;
            dt.Rows.Add(drSumRow);

            return dt;
        }

        private void SetMonthMaterils()
        {
            List<MonthMateril> resultList = GetRsultModel();

            List<MonthModel> months = new List<MonthModel>() {
                new MonthModel ("January","01","1月份"),
                new MonthModel ("February","02","2月份"),
                new MonthModel ("March","03","3月份"),
                new MonthModel ("April","04","4月份"),
                new MonthModel ("May","05","5月份"),
                new MonthModel ("June","06","6月份"),
                new MonthModel ("July","07","7月份"),
                new MonthModel ("August","08","8月份"),
                new MonthModel ("September","09","9月份"),
                new MonthModel ("October","10","10月份"),
                new MonthModel ("November","11","11月份"),
                new MonthModel ("December","12","12月份")
            };
            DtResult = GetTable();
            DtProduct = GetProduct();
            DtBomInfo = GetBomInfo();
            for (int i = 1; i <= 12; i++)
            {
                foreach (DataRow dr in DtResult.Rows)
                {
                    if (!dr[0].ToString().Equals("合计"))
                    {
                        string columnName = i.ToString() + "月份";
                        //dr[columnName]
                        //months[i-1].MaterialCounts
                        //获取产成品下的原材料数量
                        var value = GetMateriNumbers(dr["产成品编号"].ToString(), dr["版本"].ToString(), Convert.ToDouble(dr[columnName].ToString()), dr["客户产成品编号"].ToString());
                        SetValue(months[i - 1], value);
                    }
                }
            }

            Union(resultList, months);
            MonthMaterils = resultList;
        }

        private void SetValue(MonthModel monthModel, Dictionary<string, double> value)
        {
            var oldValue = monthModel.MaterialCounts;
            foreach (var item in value)
            {
                if (oldValue.ContainsKey(item.Key))
                {
                    oldValue[item.Key] += item.Value;
                }
                else
                {
                    oldValue[item.Key] = item.Value;
                }
            }
        }

        private void SetValues(MonthMateril monthMateril, List<MonthModel> months)
        {
            //var materialCounts = months[0].MaterialCounts;
            if (months[0].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.January = months[0].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[1].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.February = months[1].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[2].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.March = months[2].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[3].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.April = months[3].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[4].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.May = months[4].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[5].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.June = months[5].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[6].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.July = months[6].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[7].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.August = months[7].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[8].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.September = months[8].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[9].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.October = months[9].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[10].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.November = months[10].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
            if (months[11].MaterialCounts.ContainsKey(monthMateril.CustomerMaterilNumber))
            {
                monthMateril.December = months[11].MaterialCounts[monthMateril.CustomerMaterilNumber];
            }
        }

        private void Union(List<MonthMateril> resultList, List<MonthModel> months)
        {
            foreach (var model in resultList)
            {
                SetValues(model, months);
                model.Count = model.January
                    + model.February
                    + model.March
                    + model.April
                    + model.May
                    + model.June
                    + model.July
                    + model.August
                    + model.September
                    + model.October
                    + model.November
                    + model.December;
                model.CountPrice = model.UnitPrice * model.Count;
            }
        }
    }
}