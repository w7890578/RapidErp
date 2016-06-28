using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class AddExaminationLogListForUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string year = drpYear.SelectedValue;
            string month = drpMonth.SelectedValue;
            string error = "";
            string userName = txtUserName.Text.Trim();
            string sql = string.Format(" select count(*) from pm_user where user_name='{0}' ", userName);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = string.Format("系统不存在该姓名：{0}", userName);
                return;
            }

            sql = string.Format(@"select COUNT(*) from ExaminationLog  where YEAR ='{0}' 
            and MONTH ='{1}' 
            and Name='{2}'", year, month, userName);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = string.Format("已上报过{0}年{1}月{2}的考试成绩", year, month, userName);
                return;
            }

            sql = string.Format(@"insert into ExaminationLog(Year ,Month ,Name ,Score,Operation,TotalScore )
values('{0}','{1}','{2}',{3},{4},{3}+{4})", year, month, userName, txtBS.Text.Trim(), txtSC.Text.Trim());
            lbSubmit.Text = SqlHelper.ExecuteSql(sql, ref error) ? "上报成功" : "上报失败！原因：" + error;
        }
    }
}