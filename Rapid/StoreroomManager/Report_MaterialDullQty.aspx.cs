using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.StoreroomManager
{
    public partial class MaterialDullQty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        public void Bind()
        {
            DataTable dt = SqlHelper.GetTable(GetSql());
            this.rpList.DataSource = dt;
            this.rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        public string GetSql()
        {
            string sql = " select * from Report_MaterialDullQty where 1=1";
            string txtMaterialNumber = this.txtMaterialNumber.Text.Trim();
            string txtMaterialName = this.txtMaterialName.Text.Trim();
            string txtDescription = this.txtDescription.Text.Trim();
            if (!string.IsNullOrEmpty(txtMaterialNumber))
            {
                sql += string.Format(" and MaterialNumber like '%{0}%' ", txtMaterialNumber);
            }
            if (!string.IsNullOrEmpty(txtMaterialName))
            {
                sql += string.Format(" and MaterialName like '%{0}%' ", txtMaterialName);
            }
            if (!string.IsNullOrEmpty(txtDescription))
            {
                sql += string.Format(" and Description like '%{0}%' ", txtDescription);
            }
            return sql;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            DataTable dt = SqlHelper.GetTable(GetSql());
            ExcelHelper.Instance.ExpExcel(dt, "呆滞物料表");
        }
    }
}