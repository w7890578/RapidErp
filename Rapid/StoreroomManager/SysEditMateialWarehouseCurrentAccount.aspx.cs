using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class SysEditMateialWarehouseCurrentAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sql = string.Format(@"
select * from {1} where guid='{0}'
", ToolManager.GetQueryString("guid"),
 ToolManager.CheckQueryString("isProduct") ? "ProductWarehouseCurrentAccount" : "MateialWarehouseCurrentAccount");

                DataRow dr = SqlHelper.GetTable(sql).Rows[0];
                txtIncome.Text = dr["Income"] == null ? "" : dr["Income"].ToString();
                txtIssue.Text = dr["Issue"] == null ? "" : dr["Issue"].ToString();
                txtBalances.Text = dr["Balances"] == null ? "" : dr["Balances"].ToString();
                txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                if (ToolManager.CheckQueryString("isProduct"))
                {
                    txtMoveTime.Text = dr["MoveTimer"] == null ? "" : dr["MoveTimer"].ToString();
                }
                else
                {
                    txtMoveTime.Text = dr["MoveTime"] == null ? "" : dr["MoveTime"].ToString();
                }

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string appendSql = "";
            if (ToolManager.CheckQueryString("isProduct"))
            {
                appendSql = string.Format(",MoveTimer='{0}'", txtMoveTime.Text.Trim());

            }
            else
            {
                appendSql = string.Format(",MoveTime='{0}'", txtMoveTime.Text.Trim());
            }
            string sql = string.Format(@"update {5}
 set Income={1},
 Issue={2},
 Balances={3},
Remark='{4}'{6}
 where guid='{0}'", ToolManager.GetQueryString("guid"), txtIncome.Text, txtIssue.Text, txtBalances.Text, txtRemark.Text
                  , ToolManager.CheckQueryString("isProduct") ? "ProductWarehouseCurrentAccount" : "MateialWarehouseCurrentAccount"
                  , appendSql);




            string error = string.Empty;
            lbSubmit.Text = SqlHelper.ExecuteSql(sql, ref error) ? "修改成功" : "修改失败！原因：" + error;
        }


    }
}
