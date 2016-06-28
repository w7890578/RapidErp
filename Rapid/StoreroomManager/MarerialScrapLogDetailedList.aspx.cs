using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class MarerialScrapLogDetailed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                onload();
            }
        }
        private void onload()
        {
            string sql = string.Empty;
            string error = string.Empty;
            if (ToolManager.CheckQueryString("Id"))
            {
                sql = string.Format(@" select * from MarerialScrapLog where Id='{0}' ", ToolManager.GetQueryString("Id"));
                Model.MarerialScrapLog marerialscraplog = MarerialScrapLogManager.ConvertDataTableToModel(sql);
                this.lbId.InnerText = marerialscraplog.Id;
                this.lbCreateTime.InnerText = marerialscraplog.CreateTime;
                this.lbProductNumber.InnerText = marerialscraplog.ProductNumber;
                this.lbMaterialNumber.InnerText = marerialscraplog.MaterialNumber;
                this.lbScrapDate.InnerText = marerialscraplog.ScrapDate;
                this.lbTeam.InnerText = marerialscraplog.Team;
                this.lbCount.InnerText = marerialscraplog.Count;
                this.lbResponsiblePerson.InnerText = marerialscraplog.ResponsiblePerson;
                this.lbScrapReason.InnerText = marerialscraplog.ScrapReason;
                this.lbRemark.InnerText = marerialscraplog.Remark;
            }
        }
    }
}
