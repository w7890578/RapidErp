using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.StoreroomManager
{
    public partial class T_ExaminationLog_KFList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpYear.SelectedValue = DateTime.Now.Year.ToString();
                drpMonth.SelectedValue = DateTime.Now.Month.ToString();
                Bind();
            }
        }
        private void Bind()
        {
            string year = drpYear.SelectedValue;
            string month = drpMonth.SelectedValue;
            string name = txtName.Text.Trim();
            string condition = " where 1=1 ";
            if (!string.IsNullOrEmpty(year))
            {
                condition += string.Format(" and Year='{0}' ", year);
            }
            if (!string.IsNullOrEmpty(month))
            {
                condition += string.Format(" and Month='{0}' ", month);
            }
            if (!string.IsNullOrEmpty(name))
            {
                condition += string.Format(" and Name like '%{0}%' ", name);
            }
            string sql = string.Format(" select * from T_ExaminationLog_KF {0} ", condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
            return;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string year = drpYear.SelectedValue;
            string month = drpMonth.SelectedValue;
            string error = string.Empty;
            string sql = string.Format(" delete T_ExaminationLog_KF where Year='{0}' and Month='{1}' ", year, month);
            lbMsg.Text = SqlHelper.ExecuteSql(sql, ref error) ? "删除成功" : "删除失败！原因：" + error;
            Bind();   
        }
    }
}
