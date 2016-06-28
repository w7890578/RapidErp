using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;
using System.Web.Util;

namespace Rapid.ProduceManager
{
    public partial class EmployeeAttendance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack )
            {
                Bind();
            }
        }

        private string GetSql()
        {
            string conditoin = " where 1=1";

            if (drpYear.SelectedValue != "")
            {
                conditoin += " and Year='" + drpYear.SelectedValue + "'";
            }
            if (drpMonth.SelectedValue != "")
            {
                conditoin += " and Month='" + drpMonth.SelectedValue + "'";
            }
            if (txtName.Text != "")
            {
                conditoin += " and Name like '%" + txtName.Text.Trim() + "%'";
            }
            string sql = string.Format(@"select Year as 年度,Month as 月份,Name as 姓名,EntryDate as 入职日期,
JobNumber as 工号,
case when  CHARINDEX('.',ShouldAttendanceDays )=1 then '0'+ShouldAttendanceDays  else ShouldAttendanceDays  end  as 应出勤天,
case when  CHARINDEX('.',AccumulatedAnnualLeaveDays )=1 then '0'+AccumulatedAnnualLeaveDays  else AccumulatedAnnualLeaveDays  end  as 累计已休年假天,
case when  CHARINDEX('.',MonthSabbatical)=1 then '0'+MonthSabbatical else MonthSabbatical end as 当月休年假,
case when  CHARINDEX('.',MonthMaternityLeave)=1 then '0'+MonthMaternityLeave else MonthMaternityLeave end as 当月休产假,
case when  CHARINDEX('.',KeptOffDays)=1 then '0'+KeptOffDays else KeptOffDays end as 存休天,
case when  CHARINDEX('.',InjuryDays)=1 then '0'+InjuryDays else InjuryDays end as 工伤天,
case when  CHARINDEX('.',SickDays)=1 then '0'+SickDays else SickDays end as 病假天,
case when  CHARINDEX('.',BereavementLeave)=1 then '0'+BereavementLeave else BereavementLeave end as 丧假,
case when  CHARINDEX('.',WeekdaysLeaveForAFewdays)=1 then '0'+WeekdaysLeaveForAFewdays else WeekdaysLeaveForAFewdays end as 平日事假天数,
case when  CHARINDEX('.',LateAndLeaveEarly)=1 then '0'+LateAndLeaveEarly else LateAndLeaveEarly end as 迟到早退,
case when  CHARINDEX('.',OvertimeHours)=1 then '0'+OvertimeHours else OvertimeHours end as 平时加班, case when  CHARINDEX('.',LeaveAndOvertimeOrLeaveOffset)=1 then '0'+LeaveAndOvertimeOrLeaveOffset else LeaveAndOvertimeOrLeaveOffset end as 请假与加班年假冲抵,
case when  CHARINDEX('.',ActualAttendanceTotal)=1 then '0'+ActualAttendanceTotal else ActualAttendanceTotal end as 实际出勤总计,
case when  CHARINDEX('.',SaturdayandSundayOvertime)=1 then '0'+SaturdayandSundayOvertime else SaturdayandSundayOvertime end as 周六日加班,
case when  CHARINDEX('.',OvertimeTotal)=1 then '0'+OvertimeTotal else OvertimeTotal end as 六日加班总,
case when  CHARINDEX('.',BottomRemainingDaysOfAnnualLeave)=1 then '0'+BottomRemainingDaysOfAnnualLeave else BottomRemainingDaysOfAnnualLeave end as 截至2014年底年假剩余天数,
case when  CHARINDEX('.',BeforeMonthEndLeaveDays)=1 then '0'+BeforeMonthEndLeaveDays else BeforeMonthEndLeaveDays end as 截至上月底年假剩余天数,
case when  CHARINDEX('.',MonthEndLeaveDays)=1 then '0'+MonthEndLeaveDays else MonthEndLeaveDays end as 截至本月底年假剩余天数,
case when  CHARINDEX('.',MonthKeptDays)=1 then '0'+MonthKeptDays else MonthKeptDays end as 截至本月底存休剩余天数,
case when  CHARINDEX('.',BeforeMonthKeptDays)=1 then '0'+BeforeMonthKeptDays else BeforeMonthKeptDays end as 截至上月底存休剩余天数,
Remark as 备注,LateTimes as 迟到次数 
 from EmployeeAttendance {0} order by year desc,month desc ,name asc ", conditoin);
            return sql;
        }

        private void Bind()
        {
            
            rpList.DataSource = SqlHelper.GetTable(GetSql());
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (drpMonth.SelectedValue == "" || drpMonth.SelectedValue == "")
            {
                lbDelete.Text = "请选择年度和月份！";
                return;
            }
            string error = string.Empty;
            string sql = string.Format(@"delete EmployeeAttendance where Year='{0}' and Month='{1}'", drpYear.SelectedValue, drpMonth.SelectedValue);
            if (SqlHelper.ExecuteSql(sql,ref error))
            {
                lbDelete.Text = "删除成功！";
                Bind();
                return;
            }
            else
            {
                lbDelete.Text = "删除失败！原因："+error;
                return;
            }
           
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(GetSql()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(GetSql(), "考勤记录");
        }
    }
}
