using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class TradingQuoteDetail : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        public static string hasDelete = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0101", "Add");
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0101", "Print");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0101", "Delete");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0101", "Edit");
                string ss = ToolManager.GetQueryString("Id");
                if (!ToolManager.CheckQueryString("Id"))
                {
                    Response.Write("未知报价单");
                    Response.End();
                    return;
                }
                Bind();
            }
        }

        private void Bind()
        {
            string sql = string.Empty;
            string error = string.Empty;

            if (ToolManager.CheckQueryString("SN"))
            {
               string  quotenumber=ToolManager.GetQueryString("Id");
                string sn=ToolManager.GetQueryString("SN");
                string guid=ToolManager.GetQueryString("Guid");
                sql = string.Format("delete TradingQuoteDetail where QuoteNumber ='{0}' and SN='{1}' and Guid='{2}'",quotenumber,sn,guid );
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除贸易报价单明细" + quotenumber, "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除贸易报价单明细" + quotenumber, "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }

            sql = string.Format(@"
 select ROW_NUMBER () over(order by sn asc) as num,t.*,m.Description,m.Brand as BrandNew from TradingQuoteDetail t 
inner join MarerialInfoTable m on t.ProductNumber=m.MaterialNumber where QuoteNumber ='{0}' 
 ", ToolManager.GetQueryString("Id"));
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();
            hdnumber.Value = ToolManager.GetQueryString("Id");
            lbNumber.InnerHtml = ToolManager.GetQueryString("Id");
            lbDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            sql = string.Format(@" select c.CustomerName from QuoteInfo qi inner join Customer c on qi.CustomerId =c.CustomerId 
  where qi.QuoteNumber='{0}'", ToolManager.GetQueryString("Id"));
            lbCustomerName.Text = SqlHelper.GetScalar(sql);

        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string quteNumber = ToolManager.GetQueryString("Id");
            if (QutoInfoManager.BacthAddQuoteInfoMY(quteNumber, FU_Excel, Server, ref  error))
            {
                lbMsg.Text = "导入成功";

            }
            else
            {
                lbMsg.Text = "导入失败！<br/>" + error;
            }
            Bind();
        }
    }
}
