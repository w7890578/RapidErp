using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using DAL;


namespace Rapid.FinancialManager
{
    public partial class MonthSaleOderPriceAccount : System.Web.UI.Page
    {
        public static string userid = string.Empty;
       public static string year=string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                 userid = ToolCode.Tool.GetUser().UserNumber;
                 year = drpYear.SelectedValue;
                 string sql = string.Empty;
                for (int i = 1; i <= 12; i++)
                {
                    sql = string.Format(@"exec P_MonthSaleOderPrice '" + userid + "','" + drpYear.SelectedValue + "','" + i + "','" + txtCustomerName.Text + "'");
                    if (SqlHelper.ExecuteSql(sql))
                    {

                    }
                }
                Bind();
            }
        }


        private string GetSql()
        {
            string condition = " where 1=1";
            if (txtCustomerName.Text != "")
            {
                condition += " and c.CustomerName like '%" + txtCustomerName.Text + "%'";
            }
            string sql = string.Empty;
            sql = string.Format(@"
 select c.customerName as 客户名称,t.* from (
  select c.CustomerId,  [1月].Sales as [1月销售额],[1月].Cost as [1月成本],isnull([1月].Sales,0)-isnull([1月].Cost,0) as [1月利润],
  [2月].Sales as [2月销售额],[2月].Cost as [2月成本],isnull([2月].Sales,0)-isnull([2月].Cost,0) as [2月利润],
  [3月].Sales as [3月销售额],[3月].Cost as [3月成本],isnull([3月].Sales,0)-isnull([3月].Cost,0) as [3月利润],
  [4月].Sales as [4月销售额],[4月].Cost as [4月成本],isnull([4月].Sales,0)-isnull([4月].Cost,0) as [4月利润],
  [5月].Sales as [5月销售额],[5月].Cost as [5月成本],isnull([5月].Sales,0)-isnull([5月].Cost,0) as [5月利润],
  [6月].Sales as [6月销售额],[6月].Cost as [6月成本],isnull([6月].Sales,0)-isnull([6月].Cost,0) as [6月利润],
  [7月].Sales as [7月销售额],[7月].Cost as [7月成本],isnull([7月].Sales,0)-isnull([7月].Cost,0) as [7月利润],
  [8月].Sales as [8月销售额],[8月].Cost as [8月成本],isnull([8月].Sales,0)-isnull([8月].Cost,0) as [8月利润],
  [9月].Sales as [9月销售额],[9月].Cost as [9月成本],isnull([9月].Sales,0)-isnull([9月].Cost,0) as [9月利润],
  [10月].Sales as [10月销售额],[10月].Cost as [10月成本],isnull([10月].Sales,0)-isnull([10月].Cost,0) as [10月利润],
  [11月].Sales as [11月销售额],[11月].Cost as [11月成本],isnull([11月].Sales,0)-isnull([11月].Cost,0) as [11月利润],
  [12月].Sales as [12月销售额],[12月].Cost as [12月成本],isnull([12月].Sales,0)-isnull([12月].Cost,0) as [12月利润],
  (isnull([1月].Sales,0)+isnull([2月].Sales,0) +isnull([3月].Sales,0) +isnull([4月].Sales,0) +isnull([5月].Sales,0)
   +isnull([6月].Sales,0) +isnull([7月].Sales,0)  +isnull([8月].Sales,0) +isnull([9月].Sales,0)  
   +isnull([10月].Sales,0) +isnull([11月].Sales,0) +isnull([12月].Sales,0)) as 销售额合计,
   (isnull([1月].Cost,0)+isnull([2月].Cost,0) +isnull([3月].Cost,0) +isnull([4月].Cost,0) +isnull([5月].Cost,0)
   +isnull([6月].Cost,0) +isnull([7月].Cost,0)  +isnull([8月].Cost,0) +isnull([9月].Cost,0)  
   +isnull([10月].Cost,0) +isnull([11月].Cost,0) +isnull([12月].Cost,0)) as 成本价合计,
   (isnull([1月].Sales,0)+isnull([2月].Sales,0) +isnull([3月].Sales,0) +isnull([4月].Sales,0) +isnull([5月].Sales,0)
   +isnull([6月].Sales,0) +isnull([7月].Sales,0)  +isnull([8月].Sales,0) +isnull([9月].Sales,0)  
   +isnull([10月].Sales,0) +isnull([11月].Sales,0) +isnull([12月].Sales,0))-
   (isnull([1月].Cost,0)+isnull([2月].Cost,0) +isnull([3月].Cost,0) +isnull([4月].Cost,0) +isnull([5月].Cost,0)
   +isnull([6月].Cost,0) +isnull([7月].Cost,0)  +isnull([8月].Cost,0) +isnull([9月].Cost,0)  
   +isnull([10月].Cost,0) +isnull([11月].Cost,0) +isnull([12月].Cost,0)) as 利润合计
   from Customer c  left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='1'
	) [1月] on c.CustomerId =[1月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='2'
	) [2月] on c.CustomerId =[2月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='3'
	) [3月] on c.CustomerId =[3月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='4'
	) [4月] on c.CustomerId =[4月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='5'
	) [5月] on c.CustomerId =[5月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='6'
	) [6月] on c.CustomerId =[6月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='7'
	) [7月] on c.CustomerId =[7月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='8'
	) [8月] on c.CustomerId =[8月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='9'
	) [9月] on c.CustomerId =[9月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='10'
	) [10月] on c.CustomerId =[10月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='11'
	) [11月] on c.CustomerId =[11月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='12'
	) [12月] on c.CustomerId =[12月].customerId) t inner join customer c on t.customerId=c.customerId {0}
	union all

select '合计','',SUM(t.[1月销售额]), SUM(t.[1月成本]),SUM(t.[1月利润]),
SUM(t.[2月销售额]), SUM(t.[2月成本]),SUM(t.[2月利润]),
SUM(t.[3月销售额]), SUM(t.[3月成本]),SUM(t.[3月利润]),
SUM(t.[4月销售额]), SUM(t.[4月成本]),SUM(t.[4月利润]),
SUM(t.[5月销售额]), SUM(t.[5月成本]),SUM(t.[5月利润]),
SUM(t.[6月销售额]), SUM(t.[6月成本]),SUM(t.[6月利润]),
SUM(t.[7月销售额]), SUM(t.[7月成本]),SUM(t.[7月利润]),
SUM(t.[8月销售额]), SUM(t.[8月成本]),SUM(t.[8月利润]),
SUM(t.[9月销售额]), SUM(t.[9月成本]),SUM(t.[9月利润]),
SUM(t.[10月销售额]), SUM(t.[10月成本]),SUM(t.[10月利润]),
SUM(t.[11月销售额]), SUM(t.[11月成本]),SUM(t.[11月利润]),
SUM(t.[12月销售额]), SUM(t.[12月成本]),SUM(t.[12月利润]),
SUM(t.销售额合计),SUM(t.成本价合计),SUM(t.利润合计)
 from (
 select c.customerName,t.* from (
  select c.CustomerId,  [1月].Sales as [1月销售额],[1月].Cost as [1月成本],isnull([1月].Sales,0)-isnull([1月].Cost,0) as [1月利润],
  [2月].Sales as [2月销售额],[2月].Cost as [2月成本],isnull([2月].Sales,0)-isnull([2月].Cost,0) as [2月利润],
  [3月].Sales as [3月销售额],[3月].Cost as [3月成本],isnull([3月].Sales,0)-isnull([3月].Cost,0) as [3月利润],
  [4月].Sales as [4月销售额],[4月].Cost as [4月成本],isnull([4月].Sales,0)-isnull([4月].Cost,0) as [4月利润],
  [5月].Sales as [5月销售额],[5月].Cost as [5月成本],isnull([5月].Sales,0)-isnull([5月].Cost,0) as [5月利润],
  [6月].Sales as [6月销售额],[6月].Cost as [6月成本],isnull([6月].Sales,0)-isnull([6月].Cost,0) as [6月利润],
  [7月].Sales as [7月销售额],[7月].Cost as [7月成本],isnull([7月].Sales,0)-isnull([7月].Cost,0) as [7月利润],
  [8月].Sales as [8月销售额],[8月].Cost as [8月成本],isnull([8月].Sales,0)-isnull([8月].Cost,0) as [8月利润],
  [9月].Sales as [9月销售额],[9月].Cost as [9月成本],isnull([9月].Sales,0)-isnull([9月].Cost,0) as [9月利润],
  [10月].Sales as [10月销售额],[10月].Cost as [10月成本],isnull([10月].Sales,0)-isnull([10月].Cost,0) as [10月利润],
  [11月].Sales as [11月销售额],[11月].Cost as [11月成本],isnull([11月].Sales,0)-isnull([11月].Cost,0) as [11月利润],
  [12月].Sales as [12月销售额],[12月].Cost as [12月成本],isnull([12月].Sales,0)-isnull([12月].Cost,0) as [12月利润],
  (isnull([1月].Sales,0)+isnull([2月].Sales,0) +isnull([3月].Sales,0) +isnull([4月].Sales,0) +isnull([5月].Sales,0)
   +isnull([6月].Sales,0) +isnull([7月].Sales,0)  +isnull([8月].Sales,0) +isnull([9月].Sales,0)  
   +isnull([10月].Sales,0) +isnull([11月].Sales,0) +isnull([12月].Sales,0)) as 销售额合计,
   (isnull([1月].Cost,0)+isnull([2月].Cost,0) +isnull([3月].Cost,0) +isnull([4月].Cost,0) +isnull([5月].Cost,0)
   +isnull([6月].Cost,0) +isnull([7月].Cost,0)  +isnull([8月].Cost,0) +isnull([9月].Cost,0)  
   +isnull([10月].Cost,0) +isnull([11月].Cost,0) +isnull([12月].Cost,0)) as 成本价合计,
   (isnull([1月].Sales,0)+isnull([2月].Sales,0) +isnull([3月].Sales,0) +isnull([4月].Sales,0) +isnull([5月].Sales,0)
   +isnull([6月].Sales,0) +isnull([7月].Sales,0)  +isnull([8月].Sales,0) +isnull([9月].Sales,0)  
   +isnull([10月].Sales,0) +isnull([11月].Sales,0) +isnull([12月].Sales,0))-
   (isnull([1月].Cost,0)+isnull([2月].Cost,0) +isnull([3月].Cost,0) +isnull([4月].Cost,0) +isnull([5月].Cost,0)
   +isnull([6月].Cost,0) +isnull([7月].Cost,0)  +isnull([8月].Cost,0) +isnull([9月].Cost,0)  
   +isnull([10月].Cost,0) +isnull([11月].Cost,0) +isnull([12月].Cost,0)) as 利润合计
   from Customer c  left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='1'
	) [1月] on c.CustomerId =[1月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='2'
	) [2月] on c.CustomerId =[2月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='3'
	) [3月] on c.CustomerId =[3月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='4'
	) [4月] on c.CustomerId =[4月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='5'
	) [5月] on c.CustomerId =[5月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='6'
	) [6月] on c.CustomerId =[6月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='7'
	) [7月] on c.CustomerId =[7月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='8'
	) [8月] on c.CustomerId =[8月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='9'
	) [9月] on c.CustomerId =[9月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='10'
	) [10月] on c.CustomerId =[10月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='11'
	) [11月] on c.CustomerId =[11月].customerId left join (
	select Sales, Cost,customerId from T_MonthSaleOderPrice_temp where userId='{1}'  and YEAR ='{2}'  and MONTH ='12'
	) [12月] on c.CustomerId =[12月].customerId) t inner join customer c on t.customerId=c.customerId {0}) t ", condition,userid,year);
            return sql;
        }

        private void Bind()
        {
            string sql = GetSql();
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string sql = GetSql();
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "月度销售额统计报表");
        }
    }
}
