using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;
using BLL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class EditProductPlan : System.Web.UI.Page
    {
        public static string PersonQty = string.Empty;//人数
        public static string FinishManhour = string.Empty;//实际完成工时
        public static string FactTotalManhour = string.Empty;//实际总工时
        protected void Page_Load(object FinishManhoursender, EventArgs e)
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
                lblPlanNumber.Text = plannumber;
                string sql = string.Format(@"select * from V_ProductPlan where 开工单号='{0}'", plannumber);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtFactEndTime.Text = dr["实际结束时间"] == null ? "" : dr["实际结束时间"].ToString(); 
                    txtFactStartTime.Text = dr["实际开始时间"] == null ? "" : dr["实际开始时间"].ToString();
                    txtPlanEndTime.Text = dr["计划结束时间"] == null ? "" : dr["计划结束时间"].ToString();
                    txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                    txtPlanStartTime.Text = dr["计划开始时间"] == null ? "" : dr["计划开始时间"].ToString();
                    PersonQty = dr["人数"] == null ? "" : dr["人数"].ToString();
                    //TargetFinishManhour = dr["目标完成工时"] == null ? "" : dr["目标完成工时"].ToString();
                }

            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string factendtime = this.txtFactEndTime.Text;
            string factstarttime = this.txtFactStartTime.Text;
            string planendtime = this.txtPlanEndTime.Text;
            string remark = this.txtRemark.Text;
            string planstarttime = txtPlanStartTime.Text;
            //string fstYMD = Convert.ToDateTime(factstarttime).ToString("yyyy-MM-dd");//实际开始日期
            //string fetYMD = Convert.ToDateTime(factendtime).ToString("yyyy-MM-dd");//实际结束日期
            //string fstHMS = Convert.ToDateTime(factstarttime).ToString("HH:mm:ss");//实际开始时间
            //string fetHMS = Convert.ToDateTime(factendtime).ToString("HH:mm:ss");//实际结束时间
            //string daye = Convert.ToDateTime(factendtime).ToString("dd");//实际结束的天数
            //string days = Convert.ToDateTime(factstarttime).ToString("dd");//实际开始的天数
            //DateTime fst = Convert.ToDateTime(factstarttime);
            //DateTime fet = Convert.ToDateTime(factendtime);
            ////实际开始日期与实际结束日期相比较，如果实际结束日期大于实际开始日期则实际完成工时
            ////（如果超过一天的话，就是开始的天的(下班时间-实际开始时间)+超过那一天的(下班时间-上班时间)
            ////+最后一天的(实际结束时间-上班时间)=实际完成时间）
            //if (DateTime.Compare(fet, fst) > 0)
            //{

            //    TimeSpan time1 = Convert.ToDateTime(fetHMS) - Convert.ToDateTime(ToolManager.GetGoOnfWorkTime());
            //    double timese = time1.TotalSeconds;//实际结束时间-上班时间
            //    TimeSpan times = Convert.ToDateTime(ToolManager.GetGoOffWorkTime()) - Convert.ToDateTime(ToolManager.GetGoOnfWorkTime());
            //    int hours = times.Hours;//下班时间减去上班时间
            //    string day = ((Convert.ToInt32(daye) - Convert.ToInt32(days) - 1) * hours * 3600).ToString();//下班时间-上班时间乘以天数
            //    TimeSpan timee = Convert.ToDateTime(ToolManager.GetGoOffWorkTime()) - Convert.ToDateTime(fstHMS);
            //    double houre = timee.TotalSeconds;//下班时间-实际开时间
            //    double factfinishmanhour = (timese + Convert.ToDouble(day) + houre) / 3600;
            //    FinishManhour = factfinishmanhour.ToString();//实际完成工时
            //}
            ////如果实际结束日期小于实际开始日期，实际结束时间-实际开始时间=实际完成工时
            //else
            //{
            //    TimeSpan time = Convert.ToDateTime(fetHMS) - Convert.ToDateTime(fstHMS);
            //    FinishManhour = time.Hours.ToString();//实际完成工时
            //}
            int Qty = (Convert.ToInt32(PersonQty));//实际总工时（人数*实际完成工时）
            string sql = string.Empty;
            string error = string.Empty;
//            sql = string.Format(@"update ProductPlan set FactEndTime='{0}',FactStartTime='{1}',
//Remark='{2}',PlanEndTime='{3}',PlanStartTime='{4}',FactFinishManhour=CAST ( ROUND ('{6}',2) as decimal(18,2)),FactTotalManhour=CAST ( ROUND ('{6}',2) as decimal(18,2))*{7} where PlanNumber='{5}'", factendtime, factstarttime, remark, planendtime, planstarttime, lblPlanNumber.Text, FinishManhour,Qty);
            sql = string.Format(@"update ProductPlan set FactEndTime='{0}',FactStartTime='{1}',
Remark='{2}',PlanEndTime='{3}',PlanStartTime='{4}' where PlanNumber='{5}'", factendtime, factstarttime, remark, planendtime, planstarttime, lblPlanNumber.Text);
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑开工单总表信息" + lblPlanNumber.Text, "编辑成功");
               
            }
            else
            {

                Tool.WriteLog(Tool.LogType.Operating, "编辑开工单总表信息" + lblPlanNumber.Text, "编辑失败！原因：" + error);
                
            }

            new BLL.ToolChangeProduct().changeproduct(ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"));
            return;
        }
    }
}