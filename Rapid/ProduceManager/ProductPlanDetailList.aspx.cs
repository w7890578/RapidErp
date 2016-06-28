using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class ProductPlanDetailList : System.Web.UI.Page
    {
        public static string type = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = string.Empty;
            string error = string.Empty;
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("PlanNumber"))
                {
                    Response.Write("未知开工单号！");
                    Response.End();
                    return;
                }
                string plannumber = ToolManager.GetQueryString("PlanNumber");
                sql = string.Format(@"select * from V_ProductPlan where 开工单号='{0}'", plannumber);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    lblFactEndTime.Text = dr["实际结束时间"] == null ? "" : dr["实际结束时间"].ToString();
                    lblFactFinishManhour.Text = dr["实际完成工时"] == null ? "" : dr["实际完成工时"].ToString();
                    lblFactStartTime.Text = dr["实际开始时间"] == null ? "" : dr["实际开始时间"].ToString();
                    lblFactTotalManhour.Text = dr["实际总工时"] == null ? "" : dr["实际总工时"].ToString();
                    lblPersonQty.Text = dr["人数"] == null ? "" : dr["人数"].ToString();
                    lblPlanEndTime.Text = dr["计划结束时间"] == null ? "" : dr["计划结束时间"].ToString();
                    lblPlanStartTime.Text = dr["计划开始时间"] == null ? "" : dr["计划开始时间"].ToString();
                    lblRatedTotalManhour.Text = dr["额定总工时"] == null ? "" : dr["额定总工时"].ToString();
                    lblTargetFinishManhour.Text = dr["目标完成工时"] == null ? "" : dr["目标完成工时"].ToString();
                    lblAuditor.Text = dr["审核人"] == null ? "" : dr["审核人"].ToString();
                    type = dr["开工单类型"] == null ? "" : dr["开工单类型"].ToString();

                }
                Bind();
//                sql = string.Format(@" delete ProductPlan where PlanNumber ='{0}'",
//plannumber);
//                bool result = SqlHelper.ExecuteSql(sql, ref error);
//                if (result)
//                {
//                    Tool.WriteLog(Tool.LogType.Operating, "删除开工单明细" +ToolManager.ReplaceSingleQuotesToBlank(plannumber) , "删除成功");
//                    return;
//                }
//                else
//                {
//                    Tool.WriteLog(Tool.LogType.Operating, "删除开工单明细" + ToolManager.ReplaceSingleQuotesToBlank(plannumber), "删除失败！原因" + error);
//                    return;
//                }
            }


        }

        private void Bind()
        {
            string sql = string.Empty;
            string error = string.Empty;
            string plannumber = ToolManager.GetQueryString("PlanNumber");
            string condition = "where PlanNumber='" + plannumber + "'";
            sql = string.Format(@"select * from V_ProductPlanDetail where 开工单号='{0} '
union 
select '合计','','','','',sum(套数),0,sum(合计工时),'',''from V_ProductPlanDetail where 开工单号='{0}'", plannumber);
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();
        }
    }
}