using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;

namespace Rapid.StoreroomManager
{
    public partial class V_PackingTips_Table : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Guid"))
                {
                    string guid = ToolManager.GetQueryString("Guid");
                    string sql = string.Empty;
                    string error = string.Empty;
                    //sql = string.Format(@" select Quantity from TradingOrderDetail where Guid='{0}'", guid);
                    int num = Convert.ToInt32(ToolManager.GetQueryString("Num"));
                    //string qty = SqlHelper.GetScalar(sql);
                    int quantity = Convert.ToInt32(ToolManager.GetQueryString("Qty"));

                    if (num > quantity)
                    {
                        Response.Write("0");
                        Response.End();
                        return;
                    }
                    else
                    {
                        sql = string.Format(@"update TradingOrderDetail set FinishedQty={0} where Guid='{1}'", num,guid);
                        if (SqlHelper.ExecuteSql(sql, ref error))
                        {
                            Response.Write("1");
                            Response.End();
                            return;
                        }
                        else
                        {
                            Response.Write(error);
                            Response.End();
                            return;
                        }
                    }
                }
               
                Bind();
            }
        }


        private string GetSql()
        {
            string conditon = " where 1=1";
            if (txtCustomerName.Text.Trim() != "")
            {
                conditon += " and t.客户名称 like '%" + txtCustomerName.Text.Trim() + "%'";
            }
            if (txtDate.Text.Trim() != "")
            {
                conditon += " and t.交期= '" + txtDate.Text.Trim() + "'";
            }
            if (txtMateriNumber.Text.Trim() != "")
            {
                conditon += " and t.原材料编号 like '%" + txtMateriNumber.Text.Trim() + "%'";
            }
            if (txtCustomerMateriNumber.Text.Trim() != "")
            {
                conditon += " and t.客户物料编号 like '%" + txtCustomerMateriNumber.Text.Trim() + "%'";
            }
//            string sql = string.Format(@"
//select * from (
// select * from V_PackingTips_Table_New
//
//) t {0} ", conditon);
//            sql += string.Format(@" union all
// select '合计','','','','','',sum(数量),sum(未交数量),sum(已交数量),sum(已完成数量),'2099-12-10','','' from ({0}) a", sql);
//            sql += string.Format(" select * from ({0})t order by t.交期", sql);
            string sql = string.Format(@"select * from (
select * from (
 select * from V_PackingTips_Table_New

) t  {0}  union all
 select '合计','','','','','',sum(数量),sum(未交数量),sum(已交数量),sum(已完成数量),'','','' from (
select * from (
 select * from V_PackingTips_Table_New

) t  {0} ) a)t order by t.交期 desc", conditon);
            return sql;
        }

        private void Bind()
        {
            string sql = GetSql();
            this.Repeater1.DataSource = SqlHelper.GetTable(sql);
            this.Repeater1.DataBind();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string sql = GetSql();
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "未完销售订单（贸易）包装提示表");
        }
    }
}
