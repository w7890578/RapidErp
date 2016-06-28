using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;
using Model;

namespace Rapid.ProduceManager
{
    public partial class EditProductPlanSubDetail : System.Web.UI.Page
    {
        public static string finishqtys = string.Empty;
        public static string qty = string.Empty;
        public static int yqty = 0;
        public static int allqty = 0;
        public static string show = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string sql = string.Empty;
            string error = string.Empty;
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("PlanNumber"))
                {
                    Response.Write("未知开工单！");
                    Response.End();
                    return;
                }
                string type = Server.UrlDecode(ToolManager.GetQueryString("type"));
                if (type == "小组")
                {
                    show = "none";
                }
                //                完成数量,
                //TakeLine as 交线情况
                lblPlanNumber.Text = ToolManager.GetQueryString("PlanNumber");
                lblTeam.Text = Server.UrlDecode(ToolManager.GetQueryString("Team"));
                lblOrdersNumber.Text = ToolManager.GetQueryString("OrdersNumber");
                lblProductNumber.Text = ToolManager.GetQueryString("ProductNumber");
                lblVersion.Text = ToolManager.GetQueryString("Version");
                lblRowNumber.Text = ToolManager.GetQueryString("RowNumber");
                sql = string.Format(@"select * from  V_ProductPlanSubDetail where 开工单号='{0}' and 
班组='{1}' and 销售订单号='{2}' and 产成品编号='{3}' and 版本='{4}' and 行号='{5}'", lblPlanNumber.Text,
                 lblTeam.Text, lblOrdersNumber.Text, lblProductNumber.Text, lblVersion.Text, lblRowNumber.Text);
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    //txtFinishQty.Text = dr["完成数量"] == null ? "0" : dr["完成数量"].ToString();
                    txtFinishQty.Text = "0";
                    finishqtys = txtFinishQty.Text;
                    yqty = string.IsNullOrEmpty(finishqtys) ? 0 : Convert.ToInt32(finishqtys);
                    txtTakeLine.Text = dr["交线情况"] == null ? "" : dr["交线情况"].ToString();
                    txtRemark.Text = dr["备注"] == null ? "" : dr["备注"].ToString();
                    txtNextTeam.Text = dr["交接班组"] == null ? "" : dr["交接班组"].ToString();
                    lblCustomerProductNumber.Text = dr["客户产成品编号"] == null ? "" : dr["客户产成品编号"].ToString();
                    qty = dr["套数"] == null ? "0" : dr["套数"].ToString();
                    allqty = string.IsNullOrEmpty(qty) ? 0 : Convert.ToInt32(qty);

                    txtOldQty.Text = dr["套数"] == null ? "0" : dr["套数"].ToString();
                    txtOldFQty.Text = dr["完成数量"] == null ? "0" : dr["完成数量"].ToString();
                    if (lblTeam.Text.Equals("检验"))
                    {
                        drpHalf.Items.Clear();
                        drpHalf.Items.Add(new ListItem("否", "否"));
                        trQL.Visible = false;
                    }

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string plannumber = lblPlanNumber.Text;
            string team = lblTeam.Text;
            string ordersnumber = lblOrdersNumber.Text;
            string productnumber = lblProductNumber.Text;
            string version = lblVersion.Text;
            string rownumber = lblRowNumber.Text;
            string finishqty = txtFinishQty.Text;
            int fiqty = string.IsNullOrEmpty(finishqty) ? 0 : Convert.ToInt32(finishqty);
            string takeline = txtTakeLine.Text;
            string remark = txtRemark.Text;
            string nextteam = txtNextTeam.Text;
            string sql = string.Empty;
            string error = string.Empty;
            List<string> sqls = new List<string>();
            ProductPlanSubDetail productplansubdetail = new ProductPlanSubDetail();
            productplansubdetail.IsHalfProduct = drpHalf.SelectedValue == "是" ? true : false;
            productplansubdetail.OrdersNumber = ordersnumber;
            productplansubdetail.PlanNumber = plannumber;
            productplansubdetail.ProductNumber = productnumber;
            productplansubdetail.Version = version;
            productplansubdetail.Team = team;
            productplansubdetail.QLMareialNumbers = txtQL.Text;
            productplansubdetail.RowNumber = rownumber;
            string userId = ToolCode.Tool.GetUser().UserNumber;
            int oldQty = string.IsNullOrEmpty(txtOldQty.Text) ? 0 : Convert.ToInt32(txtOldQty.Text);
            int oldFQty = string.IsNullOrEmpty(txtOldFQty.Text) ? 0 : Convert.ToInt32(txtOldFQty.Text);
            int nowFQty = string.IsNullOrEmpty(txtFinishQty.Text) ? 0 : Convert.ToInt32(txtFinishQty.Text);
            if ((nowFQty + oldFQty) > oldQty)
            {
                lbSubmit.Text = "完成数量<=套数-已完成数量";
                return;
            }
            else
            {
                nowFQty += oldFQty;
            }
            yqty = nowFQty;

            //int fqty = allqty - yqty;

            //if (fiqty > fqty)
            //{
            //    lbSubmit.Text = "完成数量<=套数-已完成数量";
            //    return;
            //}
            //if (yqty == 0)
            //{
            //    yqty = fiqty;//如果完成数量为0，则已完成数量等入输入的数量
            //}
            //else
            //{
            //    yqty = yqty + fiqty;//已完成数量=完成数量+已经完成数量
            //}

            sql = string.Format(@"update ProductPlanSubDetail set FinishQty={0},TakeLine='{1}',Remark='{2}' 
,NextTeam='{9}',UpdateTime='{10}' where PlanNumber='{3}' and Team='{4}' and OrdersNumber='{5}' and ProductNumber='{6}' and Version='{7}' and RowNumber='{8}'",
 yqty, takeline, remark, plannumber, team, ordersnumber, productnumber, version, rownumber, nextteam, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sqls.Add(sql);
            productplansubdetail.Qty = txtFinishQty.Text.Trim().Equals("") ? "0" : txtFinishQty.Text.Trim();

            sqls.Add(WorkOrderManager.GetRecordSql(productplansubdetail));

            
            if (team == "检验")
            {
                List<string> temps = WorkOrderManager.GetGenerateProductWarehouseLogSql(productplansubdetail, userId);
                sqls.AddRange(temps);

            }
            else if (team != "检验" && drpHalf.SelectedValue == "是")
            {

                if (txtQL.Text == "")
                {
                    lbSubmit.Text = "缺料原材料编号不允许为空！";
                    return;
                }
                 
            }


            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑开工单分表明细" + lblPlanNumber.Text, "编辑成功");
                Response.Write(@" <script>
         window.close(); 
     </script>");
                Response.End();
                return;
            }
            else
            {

                Tool.WriteLog(Tool.LogType.Operating, "编辑开工单分表明细" + lblPlanNumber.Text, "编辑失败！原因：" + error);
                return;
            }

        }

        protected void drpHalf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drpHalf.SelectedValue.Equals("是"))
            {
                trQL.Visible = true;
            }
            else
            {
                trQL.Visible = false;
            }
        }
    }
}
