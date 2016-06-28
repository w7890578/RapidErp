using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class EditCustomerDeliveryDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Guid"))
                {
                    string error = string.Empty;
                    string guid = ToolManager.GetQueryString("Guid");
                    string sql = string.Format(@" select cdd.Guid as Guid, cdd.ImportTime,cdd.CustomerId,cdd.Number,
              cdd.Description,cdd.AdvanceQty,cdd.ReplyDate,
              10 as WipQty,cdd.NonDeliveryQty,cdd.StockQty,
              case when  cdd.IsMeetDelivery>=0 then '满足' else '不满足' end as IsMeetDelivery,
              case when cdd.OrderContrast>=0 then '正常' else '不正常' end as OrderContrast,cdd.Remark
              from CustomerDeliveryDetail cdd where cdd.guid='{0}'", guid);
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    DataRow dr = dt.Rows[0];
                    lbImportTime.Text = dr["ImportTime"] == null ? "" : dr["ImportTime"].ToString();
                    lbCustomerId.Text = dr["CustomerId"] == null ? "" : dr["CustomerId"].ToString();
                    lbNumber.Text = dr["Number"] == null ? "" : dr["Number"].ToString();
                    lbDescription.Text = dr["Description"] == null ? "" : dr["Description"].ToString();
                    lbAdvanceQty.Text = dr["AdvanceQty"] == null ? "" : dr["AdvanceQty"].ToString();
                    txtReplyDate.Text = dr["ReplyDate"] == null ? "" : dr["ReplyDate"].ToString();
                    lbWipQty.Text = dr["WipQty"] == null ? "" : dr["WipQty"].ToString();
                    lbNonDeliveryQty.Text = dr["NonDeliveryQty"] == null ? "" : dr["NonDeliveryQty"].ToString();
                    lbStockQty.Text = dr["StockQty"] == null ? "" : dr["StockQty"].ToString();
                    lbIsMeetDelivery.Text = dr["IsMeetDelivery"] == null ? "" : dr["IsMeetDelivery"].ToString();
                    lbOrderContrast.Text = dr["OrderContrast"] == null ? "" : dr["OrderContrast"].ToString();
                    txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    btnSubmit.Text = "修改";
                }

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string sql = string.Empty;
            string replydate = this.txtReplyDate.Text.Trim();
            string remark = this.txtRemark.Text.Trim();
            string guid = ToolManager.GetQueryString("Guid");
            sql += string.Format(@" select * from CustomerDeliveryDetail where guid ='{0}'", guid);
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count <= 0)
            {
                lbSubmit.Text = "该条数据已经删除！请重新添加！！！";
            }
            sql = string.Format(@" update CustomerDeliveryDetail set ReplyDate='{0}', remark='{1}' where guid ='{2}' ",
                replydate, remark, guid);
            bool result= SqlHelper.ExecuteSql(sql, ref error);
            lbSubmit.Text = result== true ? "修改成功" : "修改失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating,"编辑客户提前要货明细","编辑成功");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑客户提前要货明细", "编辑失败！原因" + error);
                return;
            }
        }
    }
}