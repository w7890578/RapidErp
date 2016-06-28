using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;


namespace Rapid.StoreroomManager
{
    public partial class PackagingPickingList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0409","Print");
                ControlBindManager.BindDrp(@"select distinct OdersNumber from SaleOder where  ProductType='加工' and OrderStatus='未完成' and isnull(CheckTime,'')!=''", drpOdersNumber, "OdersNumber", "OdersNumber");
                PageSearch();
            }
        }

        private void PageSearch()
        {
            string sql = string.Format(@"exec P_PackagingPicking '" + drpOdersNumber.SelectedValue + "'");
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();
        }
       

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PageSearch(); 
        }
    }
}
