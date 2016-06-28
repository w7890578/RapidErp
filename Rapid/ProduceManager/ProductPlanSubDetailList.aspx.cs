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

    public partial class ProductPlanSubDetailList : System.Web.UI.Page
    {
        //    V_ProductPlanSubDetail
        public static string worksn1 = "inline";
        public static string worksn2 = "inline";
        public static string worksn3 = "inline";
        public static string worksn4 = "inline";
        public static string nextteam = "inline";
        public static string finishqty = "inline";
        public static string takeline = "inline";
        public static string plannumber = string.Empty;

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
                plannumber = ToolManager.GetQueryString("PlanNumber");
                ControlBindManager.BindDrp(@"select 班组 from V_ProductPlanSub where 开工单号='" + plannumber + "'", this.drpTeam, "班组", "班组");
                string team = Server.UrlDecode(ToolManager.GetQueryString("Team"));
                drpTeam.SelectedValue = team;


                //string ordersnumber = ToolManager.GetQueryString("OrdersNumber");
                //string productnumber = ToolManager.GetQueryString("ProductNumber");
                //string version = ToolManager.GetQueryString("Version");
                //string rownumber = ToolManager.GetQueryString("RowNumber");

                Bind();

                ////审核人
                //sql = string.Format("select Auditor from ProductPlan where PlanNumber='{0}'", plannumber);
                //dt = SqlHelper.GetTable(sql);
                //if (dt.Rows.Count > 0)
                //{
                //    lblAuditor.Text = dt.Rows[0]["Auditor"].ToString();
                //}


                //sql = string.Format(@"select * from V_ProductPlan where 开工单号='{0}'", plannumber);
                //dt = SqlHelper.GetTable(sql);
                //if (dt.Rows.Count > 0)
                //{
                //    DataRow dr = dt.Rows[0];
                //    type = dr["开工单类型"] == null ? "" : dr["开工单类型"].ToString();
                //}
                //sql = string.Format(@"select * from V_ProductPlanSub where 开工单号='{0}' and 班组='{1}'", plannumber, team);
                //dt = SqlHelper.GetTable(sql);
                //if (dt.Rows.Count > 0)
                //{

                //    DataRow dr = dt.Rows[0];
                //    lblFactEndTime.Text = dr["实际结束时间"] == null ? "" : dr["实际结束时间"].ToString();
                //    lblFactFinishManhour.Text = dr["实际完成工时"] == null ? "" : dr["实际完成工时"].ToString();
                //    lblFactStartTime.Text = dr["实际开始时间"] == null ? "" : dr["实际开始时间"].ToString();
                //    lblFactTotalManhour.Text = dr["实际总工时"] == null ? "" : dr["实际总工时"].ToString();
                //    lblRatedTotalManhour.Text = dr["额定总工时"] == null ? "" : dr["额定总工时"].ToString();
                //    lblTargetFinishManhour.Text = dr["目标完成工时"] == null ? "" : dr["目标完成工时"].ToString();
                //    lblTeam.Text = dr["班组"] == null ? "" : dr["班组"].ToString();


                //}
                //                sql = string.Format(@" delete ProductPlanSubDetail where PlanNumber ='{0}' and Team='{1}'
                //                and OrdersNumber='{2}' and ProductNumber='{3}' and Version='{4}' and RowNumber='{5}'",
                //plannumber, team, ordersnumber, productnumber, version, rownumber);
                //                bool result = SqlHelper.ExecuteSql(sql, ref error);
                //                if (result)
                //                {
                //                    Tool.WriteLog(Tool.LogType.Operating, "删除开工单分表明细" + ToolManager.ReplaceSingleQuotesToBlank(plannumber), "删除成功");
                //                    return;
                //                }
                //                else
                //                {
                //                    Tool.WriteLog(Tool.LogType.Operating, "删除开工单分表明细" + ToolManager.ReplaceSingleQuotesToBlank(plannumber), "删除失败！原因" + error);
                //                    return;
                //                }
            }
        }

        private void Bind()
        {
            string sql = string.Empty;
            string error = string.Empty;
            string plannumber = ToolManager.GetQueryString("PlanNumber");
            sql = string.Format(@"select * from V_ProductPlan where 开工单号='{0}'", plannumber);
            DataTable dt = SqlHelper.GetTable(sql);
            string type = dt.Rows[0]["开工单类型"].ToString();
            if (type == "小组")
            {
                worksn1 = "none";
                worksn2 = "none";
                worksn3 = "none";
                worksn4 = "none";
                nextteam = "none";
                //finishqty = "none";
                //takeline = "none";
            }
            else
            {
                worksn1 = "inline";
                worksn2 = "inline";
                worksn3 = "inline";
                worksn4 = "inline";
                nextteam = "inline";
                finishqty = "inline";
                takeline = "inline";
            }


            string conditions = "where 开工单号='" + plannumber + "' and 班组='" + this.drpTeam.SelectedValue + "'";
            string rpsql = string.Format(@"select * from V_ProductPlanSubDetail " + conditions);
            sql = string.Format(@" select PersonQty from ProductPlanSub where PlanNumber='{0}' and Team='{1}'", plannumber, drpTeam.SelectedValue);
            lblPersonQty.Text = SqlHelper.GetScalar(sql);
            lblTeam.Text = this.drpTeam.SelectedValue;
            rpsql = string.Format(@"{0}
union
select '合计','','','','','','',SUM(t.套数),0.00,SUM(t.合计工时),'','','','','','','','','',
SUM (t.完成数量),'','','' from ({0})t
", rpsql);
            rpsql = string.Format(" select *,case when 单套工时=0.00 then '' else cast( 单套工时 as varchar(8)) end  单套工时新 from ({0})t ", rpsql);
            this.rpList.DataSource = SqlHelper.GetTable(rpsql);
            this.rpList.DataBind();

            sql = string.Format(@"select * from V_ProductPlanSub where 开工单号='{0}' and 班组='{1}'", plannumber, drpTeam.SelectedValue);
            dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count > 0)
            {

                DataRow dr = dt.Rows[0];
                lblFactEndTime.Text = dr["实际结束时间"] == null ? "" : dr["实际结束时间"].ToString();
                lblFactFinishManhour.Text = dr["实际完成工时"] == null ? "" : dr["实际完成工时"].ToString();
                lblFactStartTime.Text = dr["实际开始时间"] == null ? "" : dr["实际开始时间"].ToString();
                lblFactTotalManhour.Text = dr["实际总工时"] == null ? "" : dr["实际总工时"].ToString();
                lblRatedTotalManhour.Text = dr["额定总工时"] == null ? "" : dr["额定总工时"].ToString();
                lblTargetFinishManhour.Text = dr["目标完成工时"] == null ? "" : dr["目标完成工时"].ToString();
                lblTeam.Text = dr["班组"] == null ? "" : dr["班组"].ToString();


            }
        }

        protected void drpTeam_SelectedIndexChanged(object sender, EventArgs e)
        {
            //            string sql = string.Empty;
            //            string error = string.Empty;
            //            this.lblTeam.Text = this.drpTeam.SelectedValue;
            //            string plannumber = ToolManager.GetQueryString("PlanNumber");
            //            string condition = "where 开工单号='" + plannumber + "'";
            //            if (drpTeam.SelectedItem.Text != "")
            //            {
            //                condition += " and 班组='" + this.drpTeam.SelectedValue + "'";
            //            }
            //            sql = string.Format(@" select PersonQty from ProductPlanSub where PlanNumber='{0}' and Team='{1}'", plannumber, this.drpTeam.SelectedValue);
            //            lblPersonQty.Text = SqlHelper.GetScalar(sql);
            //            sql = string.Format(@"select * from V_ProductPlanSubDetail " + condition);
            //            sql = string.Format(@"{0}
            //union
            //select '合计','','','','','','',SUM(t.套数),SUM (t.单套工时),SUM(t.合计工时),'','','','','','','','','',
            //SUM (t.完成数量),'','','' from ({0})t
            //", sql);
            //            this.rpList.DataSource = SqlHelper.GetTable(sql);
            //            this.rpList.DataBind();
            Bind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

    }
}
