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
    public partial class ImpExaminationLog : System.Web.UI.Page
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

            bool result = ProductManager.ImpExaminationLog(year, month, FU_Excel, Server, ref error);
            if (result == true)
            {
                lbMsg.Text = "导入成功！";
            }
            else
            {
                lbMsg.Text = "导入失败！<br/>" + error;
            }
            if (result == true)
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入员工考试成绩上报信息", "导入成功！");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入员工考试成绩上报信息", "导入失败！原因" + error);
                return;
            }
//            sql = string.Format("select COUNT(*) from ExaminationLog  where YEAR ='{0}' and MONTH ='{1}' ", year, month);
//            if (!SqlHelper.GetScalar(sql).Equals("0"))
//            {
//                lbMsg.Text = string.Format("已上报过{0}年{1}月的考试成绩", year, month);
//                return;
//            }
//            DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
//            if (ds == null)
//            {
//                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
//                return;
//            }
          
//            List<string> sqls = new List<string>();
//            //List<ExaminationLog> examinationlogs = new List<ExaminationLog>();
//            foreach (DataRow dr in ds.Tables[0].Rows)
//            {
//                sql = string.Format(@"insert into ExaminationLog(Year ,Month ,Name ,Score,Operation,TotalScore )
//values('{0}','{1}','{2}',{3},{4},{3}+{4})", year, month, dr["姓名"], dr["笔试得分"], dr["实操得分"]);
//                sqls.Add(sql);
//                //                ExaminationLog exam = new ExaminationLog();
//                //                exam.Year = this.drpYear.SelectedValue;
//                //                exam.Month = this.drpMonth.SelectedValue;
//                //                sql = string.Format(@" SELECT USER_ID FROM PM_USER WHERE USER_NAME='{0}' ", dr["姓名"] == null ? "" : dr["姓名"].ToString());
//                //                exam.Name = SqlHelper.GetScalar(sql);
//                //                if (dr["平日事假天数"] == null || dr["平日事假天数"].ToString() == "" || dr["平日事假天数"].ToString() == "0")
//                //                {
//                //                    exam.WorkAttendance = 20;
//                //                }
//                //                else
//                //                {
//                //                    exam.WorkAttendance = 0;

//                //                }
//                //                sql = string.Format(@"  select * from ExaminationLog
//                //                where year='{0}' and month='{1}' and name='{2}' ", exam.Year, exam.Month, exam.Name);
//                //                DataTable dt = SqlHelper.GetTable(sql);
//                //                if (dt.Rows.Count > 0)
//                //                {
//                //                    sql = string.Format(@"  delete ExaminationLog  where year='{0}' and month='{1}' and name='{2}'",
//                //                        exam.Year, exam.Month, exam.Name);
//                //                    SqlHelper.ExecuteSql(sql, ref error);

//                //                }
//                //                examinationlogs.Add(exam);
//            }

//           
        }
    }
}
