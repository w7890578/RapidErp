using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;


namespace Rapid.PurchaseManager
{
    public partial class PrepaidAccountsApplication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("sq"))//选选择
                {
                    string error = string.Empty;
                    string guids = ToolManager.GetQueryString("sq");
                    string sql = string.Format(@" 
update T_AccountsPayable_Main set IsApplicationed='是' where guid in ({0}) ", guids);
                    string result = SqlHelper.ExecuteSql(sql, ref error) ? "1" : error;
                    Response.Write(result);
                    Response.End();
                    return;
                }
                if (Request["guid"] != null)
                {
                    string ordersNumber = string.Empty;
                    string error = string.Empty;
                    string sql = string.Format(" select * from T_AccountsPayable_Main where guid='{0}'", Request["guid"]);
                    DataTable db = SqlHelper.GetTable(sql);
                    if (db != null && db.Rows.Count > 0) { ordersNumber = db.Rows[0]["OrdersNumber"].ToString(); }
                    sql = string.Format(" delete T_AccountsPayable_Main where guid='{0}'", Request["guid"]);
                    bool result = SqlHelper.ExecuteSql(sql, ref error);
                    Tool.WriteLog(Tool.LogType.Operating, "删除采购预付账款申请信息" + ordersNumber, "删除" + result.ToString());

                    Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { msg = error, result = result }));
                    Response.End();
                }
                Bind();
            }

        }

        private void Bind()
        {
            string condinton = " where 是否为预付='是' and 是否已审批='否' and 是否已被申请='否'";
            if (txtOdersNumber.Text != "")
            {
                condinton += " and 采购订单号 like '%" + txtOdersNumber.Text.Trim() + "%'";
            }
            if (txtSupplierName.Text != "")
            {
                condinton += " and 供应商名称 like '%" + txtSupplierName.Text.Trim() + "%'";
            }
            if (drpPayType.SelectedValue != "")
            {
                condinton += " and 付款类型='" + drpPayType.SelectedValue + "'";
            }
            if (txtHDnumber.Text != "")
            {
                condinton += " and 采购合同号 like '%" + txtHDnumber.Text + "%'";
            }
            string sql = string.Format(@"select * from V_T_AccountsPayable_Main_ForYF {0}
union all
select '合计','',SUM(订单总价未税),SUM(到货总价未税),'','','','','','','','','','','','','','','','' from V_T_AccountsPayable_Main_ForYF {0}", condinton);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
