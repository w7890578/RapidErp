using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Data;
using BLL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class EditProductPlanSub : System.Web.UI.Page
    {
        public static string PersonQty = string.Empty;
        public static string FinishManhour = string.Empty;
        public static string FactTotalManhour = string.Empty;
        public static int personqty = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("PlanNumber"))
                {
                    Response.Write("未知开工单号！");
                    Response.End();
                    return;
                }
                string plannumber = ToolManager.GetQueryString("PlanNumber");
                string team = ToolManager.GetQueryString("Team");
                lblPlanNumber.Text = plannumber;
                string sql = string.Format(@"select * from V_ProductPlanSub where 开工单号='{0}' and 班组='{1}'", plannumber, team);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtFactEndTime.Text = dr["实际结束时间"] == null ? "" : dr["实际结束时间"].ToString();
                    txtFactStartTime.Text = dr["实际开始时间"] == null ? "" : dr["实际开始时间"].ToString();
                    txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                    PersonQty = dr["人数"] == null ? "" : dr["人数"].ToString();
                    personqty = string.IsNullOrEmpty(PersonQty) ? 0 : Convert.ToInt32(PersonQty);
                }
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string plannumber = ToolManager.GetQueryString("PlanNumber");
            List<string> sqls = new List<string>();
            string factendtime = this.txtFactEndTime.Text;
            string factstarttime = this.txtFactStartTime.Text;
            string remark = this.txtRemark.Text;
            double factTotalManhour = ProductManager.GetTime(factstarttime, factendtime);
            double finishManhour = factTotalManhour / Convert.ToDouble(personqty);
            string team = Server.UrlDecode(ToolManager.GetQueryString("Team"));
            string sql = string.Empty;
            string error = string.Empty;
            sql = string.Format(@" 
            update ProductPlanSub set FactStartTime='{0}' ,FactEndTime='{1}',FactTotalManhour={2},FactFinishManhour={3}
            where PlanNumber ='{4}' and Team ='{5}'", factstarttime, factendtime, factTotalManhour, finishManhour, plannumber, team);
            sqls.Add(sql);

            sql = string.Format(@"update ProductPlan set FactTotalManhour=t.FactTotalManhour ,FactFinishManhour=t.FactFinishManhour from 
(
select  SUM(pps.FactTotalManhour)FactTotalManhour , SUM (pps.FactFinishManhour) FactFinishManhour from 
 ProductPlan pp inner join ProductPlanSub pps on pp.PlanNumber=pps.PlanNumber
where pp.PlanNumber='{0}')t  where PlanNumber='{0}'", plannumber);
            sqls.Add(sql);

            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑开工单分表信息" + lblPlanNumber.Text, "编辑成功");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑开工单分表信息" + lblPlanNumber.Text, "编辑失败！原因：" + error);
                return;
            }
            //string fstYMD = Convert.ToDateTime(factstarttime).ToString("yyyy-MM-dd");//实际开始日期
            //string fetYMD = Convert.ToDateTime(factendtime).ToString("yyyy-MM-dd");//实际结束日期
            //string fstHMS = Convert.ToDateTime(factstarttime).ToString("HH:mm:ss");//实际开始时间
            //string fetHMS = Convert.ToDateTime(factendtime).ToString("HH:mm:ss");//实际结束时间
            //string daye = Convert.ToDateTime(factendtime).ToString("dd");//实际结束的天数
            //string days = Convert.ToDateTime(factstarttime).ToString("dd");//实际开始的天数
            //DateTime fst = Convert.ToDateTime(factstarttime);
            //DateTime fet = Convert.ToDateTime(factendtime);
            //DateTime xst=Convert.ToDateTime("12:00:00");//中午休息开始时间
            //DateTime xet=Convert.ToDateTime("13:00:00");//中午休息结束时间
            ////实际开始日期与实际结束日期相比较，如果实际结束日期大于实际开始日期则实际完成工时
            ////（如果超过一天的话，就是开始的天的(下班时间-实际开始时间)+超过那一天的(下班时间-上班时间)
            ////+最后一天的(实际结束时间-上班时间)=实际完成时间）

            //if (DateTime.Compare(fet, fst) > 0)
            //{

            //    TimeSpan time1 = Convert.ToDateTime(fetHMS) - Convert.ToDateTime(ToolManager.GetGoOnfWorkTime());
            //    double timese = time1.TotalSeconds;//实际结束时间-上班时间
            //    //if (DateTime.Compare(xst, fst) > 0)
            //    //{
            //    //    timese = time1.TotalSeconds-3600;
            //    //}
            //    TimeSpan times = Convert.ToDateTime(ToolManager.GetGoOffWorkTime()) - Convert.ToDateTime(ToolManager.GetGoOnfWorkTime());
            //    int hours = times.Hours-1;//下班时间减去上班时间
            //    string day= ((Convert.ToInt32(daye)-Convert.ToInt32(days) - 1) * hours * 3600).ToString();//下班时间-上班时间乘以天数
            //    TimeSpan timee = Convert.ToDateTime(ToolManager.GetGoOffWorkTime()) - Convert.ToDateTime(fstHMS);
            //    double houre = timee.TotalSeconds;//下班时间-实际开时间
            //    //if (DateTime.Compare(fet, xet) > 0)
            //    //{
            //    //    houre = timee.TotalSeconds-3600;
            //    //}
            //    double factfinishmanhour = (timese + Convert.ToDouble(day) + houre) / 3600;
            //    FinishManhour = factfinishmanhour.ToString();//实际完成工时
            //}
            //    //如果实际结束日期小于实际开始日期，实际结束时间-实际开始时间=实际完成工时
            //else
            //{
            //    if (DateTime.Compare(fet, xst) > 0)
            //    {
            //        TimeSpan time = Convert.ToDateTime(fetHMS) - Convert.ToDateTime(fstHMS);
            //        FinishManhour = time.Hours.ToString();//实际完成工时
            //        FinishManhour = (Convert.ToDouble(FinishManhour) - 1).ToString();
            //    }
            //    else
            //    {
            //        TimeSpan time = Convert.ToDateTime(fetHMS) - Convert.ToDateTime(fstHMS);
            //        FinishManhour = time.Hours.ToString();//实际完成工时

            //    }
            //}
            //int Qty = Convert.ToInt32(personqty);//实际总工时（人数*实际完成工时）
        }
    }
}
