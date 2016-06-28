using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.PurchaseManager
{
    public partial class ZTDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!ToolManager.CheckQueryString("MateriNumber"))
                //{
                //    Response.Write("未知的原材料编号！");
                //    Response.End();
                //    return;
                //}

                Bind();
            }
        }

        private void Bind()
        {
            string Materialnumber = ToolManager.GetQueryString("MateriNumber");
            string sql = string.Empty;
            sql = string.Format("select * from V_CertificateOrdersDetail_NonFinished where 原材料编号='{0}'", Materialnumber);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();
        }

    }
}
