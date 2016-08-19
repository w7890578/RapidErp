using BLL;
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
    public class MonthModel
    {
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
        public static DataTable DtResult = new DataTable();

        protected void btnExp_Click(object sender, EventArgs e)
        {
            DtResult = GetTable();
            ExcelHelper.Instance.ExpExcel(DtResult, "客户产成品销售月度报表");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { Bind(); }
        }

        private void Bind()
        {
            DtResult = GetTable();
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
    where 版本!=''and 送货日期 like '%{0}%'
),
--总数量
 B as (
	select dnd.ProductNumber,dnd.Version,dnd.CustomerProductNumber,  sum(ISNULL( dnd.DeliveryQty,0)) Qty  from A dnd  group by   dnd.ProductNumber,dnd.Version,dnd.CustomerProductNumber
),
--客户名称
C as (
	select   CustomerProductNumber,Version ,c.CustomerName from ProductCustomerProperty p inner join Customer c on p.CustomerId=c.CustomerId
),
--销售未税单价
D as (
	select t.productNumber,t.Version,isnull(m.UnitPrice,0) UnitPrice,m.QuoteNumber from (
	select productNumber,version,MAX(QuoteNumber) QuoteNumber from MachineQuoteDetail  where    Hierarchy =0 and ISNULL(Version,'')!=''
	group by productNumber,version
	) t left join MachineQuoteDetail m on t.ProductNumber=m.ProductNumber and t.Version=m.Version and t.QuoteNumber=m.QuoteNumber
	where m.Hierarchy =0
)
--各个月份
{1}
select
--C.CustomerName as 客户名称,
B.ProductNumber as 产成品编号,
B.CustomerProductNumber as 客户产成品编号,
B.Version as 版本
{2},
B.Qty  as 数量合计,
D.UnitPrice as 销售未税单价,
B.Qty*D.UnitPrice as 销售未税合计,
D.QuoteNumber as 最新报价单

from B
left join D on B.ProductNumber=D.ProductNumber and B.Version=D.Version
left join C on B.CustomerProductNumber=C.CustomerProductNumber and B.Version=C.Version
{3}
{4}
order by B.CustomerProductNumber,B.Version
", year, sbMonthSql.ToString(), sbMonthTitleSql.ToString(), sbMonthLeftJoinSql.ToString(), condition.ToString());
            DataTable dt = SqlHelper.GetTable(sql);
            object obj = dt.Compute("sum(销售未税合计)", "true");
            DataRow drSumRow = dt.NewRow();
            drSumRow[0] = "合计";
            drSumRow["销售未税合计"] = obj;
            dt.Rows.Add(drSumRow);

            return dt;
        }
    }
}