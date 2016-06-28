using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class EditExaminationLogList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Year") && ToolManager.CheckQueryString("Month") && ToolManager.CheckQueryString("Name"))
                {
                    string year = ToolManager.GetQueryString("Year");
                    string month = ToolManager.GetQueryString("Month");
                    string name = Server.UrlDecode(ToolManager.GetQueryString("Name"));
                    //string team = Server.UrlDecode(ToolManager.GetQueryString("Team"));
                    string sql = string.Empty;
                    string error = string.Empty;
                    sql = string.Format(@"select Score,Operation,Remark from ExaminationLog where Year='{0}' and Month='{1}' and Name='{2}' ", year, month, name);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        lblYear.Text = year;
                        lblMonth.Text = month;
                        lblName.Text = name;
                        //lblTeam.Text = team;
                        txtScore.Text = dr["Score"] == null ? "" : dr["Score"].ToString();
                        txtOperation.Text = dr["Operation"] == null ? "" : dr["Operation"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string score = txtScore.Text;
            string operation = txtOperation.Text;
            string remark = txtRemark.Text;
            string year = ToolManager.GetQueryString("Year");
            string month = ToolManager.GetQueryString("Month");
            string name =Server.UrlDecode( ToolManager.GetQueryString("Name"));
            //string team = Server.UrlDecode(ToolManager.GetQueryString("Team"));
            string sql = string.Empty;
            string error = string.Empty;
            //string TotalScore = (Convert.ToDouble(score) + Convert.ToDouble(operation)).ToString();
            sql = string.Format(@"update ExaminationLog set Score={0},Operation={1},
Remark='{2}',TotalScore={0}+{1}  where Year='{3}' 
and Month='{4}' and Name='{5}' ", score, operation, remark, year, month, name);
            if (SqlHelper.ExecuteSql(sql,ref error))
            {
                lbSubmit.Text = "修改成功！";
                return;
            }
            else
            {
                lbSubmit.Text = "修改失败！原因"+error;
                return;
            }
        }
    }
}
