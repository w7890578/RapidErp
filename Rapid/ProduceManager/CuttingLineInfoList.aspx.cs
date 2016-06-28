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
    public partial class CuttingLineInfoList : System.Web.UI.Page
    {
        public static string plannumber = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!ToolManager.CheckQueryString("PlanNumber"))
                {
                    Response.Write("未知开工单！");
                    Response.End();
                    return;
                }

            }
            Bind();

        }

        private void Bind()
        {
            plannumber = ToolManager.GetQueryString("PlanNumber");
            string condition = "where 1=1 and vce.开工单号='" + plannumber + "'";
            if (txtMarerilNumber.Text != "")
            {
                condition += " and vce.原材料编号='" + txtMarerilNumber.Text + "'";
            }
            if (txtProductNumber.Text != "")
            {
                condition += " and vce.产成品编号='" + txtProductNumber.Text + "'";
            }

            string sql = string.Format(@"
select vce.*,mit.Description as 原材料描述,cast( vce.长度*1000  as int )as 长度新 from V_CuttingLineInfo_New vce 
inner join MarerialInfoTable mit on vce.原材料编号=mit.MaterialNumber {0}", condition);
            //string sql = string.Format("select *,cast( 长度*1000  as int )as 长度新 from V_CuttingLineInfo_New " + condition);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
