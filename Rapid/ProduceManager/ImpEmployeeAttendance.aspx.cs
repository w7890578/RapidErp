using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using Rapid.ToolCode;
using System.Data;
using BLL;

namespace Rapid.ProduceManager
{
    public partial class ImpEmployeeAttendance : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                drpMonth.SelectedValue = DateTime.Now.Month.ToString();
            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string sql = string.Empty;
            string year = drpYear.SelectedValue;
            string month = drpMonth.SelectedValue;
            sql = string.Format("select COUNT(*) from EmployeeAttendance  where YEAR ='{0}' and MONTH ='{1}' ", year, month);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbMsg.Text = string.Format("已上报过{0}年{1}月的考试成绩", year, month);
                return;
            }
            DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
            if (ds == null)
            {
                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
                return;
            }

            List<string> sqls = new List<string>();
            //List<ExaminationLog> examinationlogs = new List<ExaminationLog>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                double ycqDay = dr["应出勤天"].ToString().Trim().Equals("") ? 0 : Convert.ToDouble(dr["应出勤天"]);
                double bjDay = dr["病假天"].ToString().Trim().Equals("") ? 0 : Convert.ToDouble(dr["病假天"]);
                double pssjDay = dr["平日事假天数"].ToString().Trim().Equals("") ? 0 : Convert.ToDouble(dr["平日事假天数"]);
                double psjbDay = dr["平时加班(天)"].ToString().Trim().Equals("") ? 0 : Convert.ToDouble(dr["平时加班(天)"]);
                string actallDay = (ycqDay - bjDay - pssjDay + psjbDay).ToString();
                double Beforemonthendshengyu = dr["截至上月底年假剩余天数"].ToString().Trim().Equals("") ? 0 : Convert.ToDouble(dr["截至上月底年假剩余天数"]);
                double allDay = dr["累计已休年假天"].ToString().Trim().Equals("") ? 0 : Convert.ToDouble(dr["累计已休年假天"]);
                string monthendshengyu = (Beforemonthendshengyu - allDay).ToString();
                sql = string.Format(@"insert into EmployeeAttendance(Year,Month,Name,EntryDate,JobNumber,ShouldAttendanceDays,
AccumulatedAnnualLeaveDays,KeptOffDays,
InjuryDays,SickDays,BereavementLeave,
WeekdaysLeaveForAFewdays,LateAndLeaveEarly,
OvertimeHours,LeaveAndOvertimeOrLeaveOffset,
ActualAttendanceTotal,SaturdayandSundayOvertime,
OvertimeTotal,BottomRemainingDaysOfAnnualLeave,
BeforeMonthEndLeaveDays,MonthEndLeaveDays,
MonthKeptDays,BeforeMonthKeptDays,
Remark,LateTimes,MonthSabbatical,MonthMaternityLeave)
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}',
'{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}',
'{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}')",
year, month, dr["姓名"], dr["入职日期"], dr["工号"],
dr["应出勤天"], dr["累计已休年假天"], dr["存休天"], dr["工伤天"], dr["病假天"], dr["丧假"],
dr["平日事假天数"], dr["迟到早退(分)"], dr["平时加班(天)"], dr["请假与加班/年假冲抵(天)"], actallDay,
dr["周六日加班(天)"], dr["六日加班总(天)"], dr["截至上一年年底年假剩余天数"], dr["截至上月底年假剩余天数"],
monthendshengyu, dr["截至本月底存休剩余天数"], dr["截至上月底存休剩余天数"], dr["备注"], dr["迟到次数"],dr["当月休年假"],dr["当月休产假"]);
                 sqls.Add(sql);
                 
            }

            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            if (result == true)
            {
                lbMsg.Text = "导入成功！";
            }
            else
            {
                lbMsg.Text = "导入失败！原因" + error;
            }
            if (result == true)
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入员工考勤信息", "导入成功！");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入员工考勤信息", "导入失败！原因" + error);
                return;
            }
        }
    }

}
