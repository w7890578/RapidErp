using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class ProductStockPic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MarerialScrapLogListManager.BindDrpForYear(drpYear);
                GetMonthForYear(drpYear.SelectedValue, drpMonth);
                Bind();
            }
        }
        /// <summary>
        /// 根据年份绑定12个月的月中月末
        /// </summary>
        /// <param name="year">选中的年份</param>
        /// <param name="drp">要绑定的月份控件</param>
        public void GetMonthForYear(string year, DropDownList drp)
        {
            drp.Items.Clear();
            string temp = string.Empty;
            for (int i = 1; i < 13; i++)
            {
                temp = i.ToString();
                if (i < 10)
                {
                    temp = "0" + temp;
                }

                drp.Items.Add(new ListItem(i.ToString() + "月中", year + "-" + temp + "-15"));
                drp.Items.Add(new ListItem(i.ToString() + "月末", year + "-" + temp + "-" + MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(year), i)));

            }
            drp.Items.Insert(0, new ListItem("- 请选择 -", ""));
        }

        private string GetSql()
        {
            string sql = string.Empty;
            string condition = " where 1=1";
            if (drpMonth.SelectedValue != "")
            {
                condition += " and t.时段='" + drpMonth.SelectedItem.Text + "'";
            }
            sql = string.Format(@"

select t.时段,t.库存总数 from  
(select '1月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-1-1' as datetime  )
and CAST ('{0}-1-15' as datetime  ) 
union all
select '1月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-1-1' as datetime  )
and CAST ('{0}-1-{1}' as datetime  )
union all
select '2月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-2-1' as datetime  )
and CAST ('{0}-2-15' as datetime  ) 
union all
select '2月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-2-1' as datetime  )
and CAST ('{0}-2-{2}' as datetime  ) 
union all
select '3月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-3-1' as datetime  )
and CAST ('{0}-3-15' as datetime  )
union all
select '3月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-3-1' as datetime  )
and CAST ('{0}-3-{3}' as datetime  )
union all
select '4月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-4-1' as datetime  )
and CAST ('{0}-4-15' as datetime  ) 
union all
select '4月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-4-1' as datetime  )
and CAST ('{0}-4-{4}' as datetime  ) 
union all
select '5月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-5-1' as datetime  )
and CAST ('{0}-5-15' as datetime  )
union all
select '5月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-5-1' as datetime  )
and CAST ('{0}-5-{5}' as datetime  )
union all
select '6月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-6-1' as datetime  )
and CAST ('{0}-6-15' as datetime  ) 
union all
select '6月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-6-1' as datetime  )
and CAST ('{0}-6-{6}' as datetime  ) 
union all
select '7月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-7-1' as datetime  )
and CAST ('{0}-7-15' as datetime  )
union all
select '7月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-7-1' as datetime  )
and CAST ('{0}-7-{7}' as datetime  ) 
union all
select '8月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-8-1' as datetime  )
and CAST ('{0}-8-15' as datetime  ) 
union all
select '8月中末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-8-1' as datetime  )
and CAST ('{0}-8-{8}' as datetime  ) 
union all
select '9月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-9-1' as datetime  )
and CAST ('{0}-9-15' as datetime  )
union all
select '9月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-9-1' as datetime  )
and CAST ('{0}-9-{9}' as datetime  ) 
union all
select '10月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-10-1' as datetime  )
and CAST ('{0}-10-15' as datetime  ) 
union all
select '10月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-10-1' as datetime  )
and CAST ('{0}-10-{10}' as datetime  ) 
union all
select '11月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-11-1' as datetime  )
and CAST ('{0}-11-15' as datetime  )
union all
select '11月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-11-1' as datetime  )
and CAST ('{0}-11-{11}' as datetime  ) 
union all 
select '12月中' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-12-1' as datetime  )
and CAST ('{0}-12-15' as datetime  ) 
union all
select '12月末' as 时段, SUM (isnull(StockQty,0)) as 库存总数 from  ProductStock where CAST  (UpdateTime  as datetime) between 
CAST ('{0}-12-1' as datetime  )
and CAST ('{0}-12-{12}' as datetime  ) )t {13}", drpYear.SelectedValue, MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 1),
   MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 2), MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 3),
   MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 4), MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 5),
   MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 6), MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 7),
   MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 8), MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 9),
   MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 10), MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 11),
   MarerialScrapLogListManager.GetDaysForMonthAndYear(Convert.ToInt32(drpYear.SelectedValue), 12), condition);
            return sql;
        }

        private void Bind()
        {
            DataTable dt = SqlHelper.GetTable(GetSql());
            rpList.DataSource = dt;
            rpList.DataBind();
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
            ToolCode.Tool.ExpExcel(GetSql(), "成品库存报表");
        }
    }
}
