using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class QueLiaoDetail : System.Web.UI.Page
    {
        public static string show = "none";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("productNumber") && ToolManager.CheckQueryString("version") && ToolManager.CheckQueryString("qty"))
                {
                    string productNumber = ToolManager.GetQueryString("productNumber");
                    string version = ToolManager.GetQueryString("version");
                    string qty = ToolManager.GetQueryString("qty");
                    string customerProductNumber = ToolManager.GetQueryString("customerProductNumber");
                    string sql = string.Format(@"select Type  from Product  where ProductNumber ='{0}' and Version ='{1}'", productNumber, version);
                    if (SqlHelper.GetScalar(sql).Equals("包"))
                    {
                        rpList.DataSource = WorkOrderManager.GetQuLiaoDetailForBao(productNumber, qty, customerProductNumber);
                        rpList.DataBind();
                        show = "inline";
                    }
                    else
                    {
                        show = "none";
                        rpList.DataSource = WorkOrderManager.GetQuLiaoDetail(productNumber, version, qty, customerProductNumber);
                        rpList.DataBind();
                    }
                    return;
                }
            }
        }
    }
}
