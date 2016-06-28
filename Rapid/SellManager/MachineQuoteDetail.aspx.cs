using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;
using System.Data;

namespace Rapid.SellManager
{
    public partial class MachineQuoteDetail : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        public static string hasDelete = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                spAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0101", "Add");
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0101", "Print");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0101", "Edit");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0101", "Delete");
                if (!ToolManager.CheckQueryString("id"))
                {
                    Response.Write("未知报价单");
                    Response.End();
                    return;
                }
                this.lbQuoteNumber.InnerText = ToolManager.GetQueryString("id");
                Bind();
            }
        }

        private void Bind()
        {
            lbMsg.Text = "";
            string sql = string.Empty;
            string error = string.Empty;
            if (ToolManager.CheckQueryString("guid") && ToolManager.CheckQueryString("id"))
            {
                sql = string.Format(@" delete MachineQuoteDetail where guid ='{0}' and QuoteNumber='{1}'",
                    ToolManager.GetQueryString("guid"), ToolManager.GetQueryString("id"));
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除加工报价单明细" + ToolManager.GetQueryString("id"), "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除加工报价单明细" + ToolManager.GetQueryString("id"), "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }
            //UpdatePrice();
            string qutonumber = ToolManager.GetQueryString("id");
            sql = string.Format(@"select
       c.CustomerName as 客户名称, qi.ContactId as 客户联系人,qi.ContactId,qi.QuoteUser,
Contacts,ContactTelephone,Fax,c.Email from QuoteInfo qi 
left join Customer c on qi.CustomerId =c.CustomerId 
 where QuoteNumber='{0}'", qutonumber);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count > 0)
            {
                lblContacts.Text = dt.Rows[0]["ContactId"].ToString();
                lblContactTelephone.Text = dt.Rows[0]["ContactTelephone"].ToString();
                lblDatetime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                lblFax.Text = dt.Rows[0]["Fax"].ToString();
                lblPlanName.Text = dt.Rows[0]["客户名称"].ToString();
                lblUserName.Text = dt.Rows[0]["QuoteUser"] == null ? "" : dt.Rows[0]["QuoteUser"].ToString();
                lblEmail.Text = dt.Rows[0]["Email"].ToString();
            }
            //            sql = string.Format(@" select ROW_NUMBER () over( order by mqd.PackageNumber asc, mqd.ProductNumber desc,mqd.Hierarchy asc) as 序号,mqd.Hierarchy as 阶层, 
            //            case when  mqd.ProductNumber!='' then ' ' end as 产成品编号,
            //                case 
            //            when  Hierarchy='0' then PackageNumber 
            //            when Hierarchy='1' then ProductNumber
            //            when  Hierarchy='2'  then CustomerMaterialNumber
            //               end as 客户物料编码,
            //            mqd.Description as 描述,mqd.BOMAmount as BOM用量,mqd.MaterialPrcie as 原材料单价,
            //           mqd.TimeCharge as 工时费,mqd.Profit as 利润,mqd.ManagementPrcie as 管销费用,mqd.LossPrcie as 损耗,
            //            mqd.UnitPrice as 单价,mqd.FixedLeadTime as 固定提前期,mqd.Remark as 备注,mqd.Guid as Guid
            //            from MachineQuoteDetail mqd where mqd.QuoteNumber='{0}'
            //            order by mqd.PackageNumber asc, mqd.ProductNumber desc,mqd.Hierarchy asc  ", ToolManager.GetQueryString("id"));
            //order by CustomerProductNumber desc,Hierarchy asc 
            //            sql = string.Format(@"  select * , , 
            //cast((MaterialPrcie*0.06+ TimeCharge*0.1) as decimal(18,2) ) as ManagementPrcieNew,
            //cast((MaterialPrcie*0.08) as decimal(18,2) ) as LossPrcieNew from MachineQuoteDetail where QuoteNumber='{0}'   ", qutonumber);
            //            sql = string.Format(@" select *,ISNULL ( vmq.qty,0) as 库存数量,isnull(t.ProcurementPrice,0) as 采购价格 from ({0}) mqd  
            //left join ( select mcp.CustomerMaterialNumber,vms.qty  from V_MaterialStock_Qty vms 
            //inner join MaterialCustomerProperty mcp on vms.MaterialNumber=mcp.MaterialNumber ) vmq
            //on mqd.CustomerMaterialNumber =vmq.CustomerMaterialNumber 
            //left join (
            // select mcp.CustomerMaterialNumber,mif.ProcurementPrice from MarerialInfoTable mif 
            // inner join MaterialCustomerProperty mcp on 
            // mif.MaterialNumber=mcp.MaterialNumber) t on t.CustomerMaterialNumber=mqd.CustomerMaterialNumber
            //order by CustomerProductNumber desc,Hierarchy asc ", sql);
            sql = string.Format(@"select mqd.*,vmq.qty as 库存数量,mit.ProcurementPrice as 采购价格 from MachineQuoteDetail mqd left join V_MaterialStock_Qty vmq
on mqd.MaterialNumber=vmq.MaterialNumber
left join MarerialInfoTable mit on mit.MaterialNumber=mqd.MaterialNumber 
where QuoteNumber='{0}'
order by  mqd.CustomerProductNumber desc,mqd.Version desc,mqd.Hierarchy  asc", qutonumber);
            this.rpList.DataSource = SqlHelper.GetTable(sql);
            this.rpList.DataBind();
            hdnumber.Value = ToolManager.GetQueryString("id");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string quteNumber = ToolManager.GetQueryString("id");
            if (QutoInfoManager.BacthAddQuoteInfoMachine(quteNumber, FU_Excel, Server, ref error))
            {
                lbMsg.Text = "导入成功";
                Bind();
            }
            else
            {
                lbMsg.Text = "导入失败！原因：" + error;
                //Bind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string quteNumber = ToolManager.GetQueryString("id");
            if (QutoInfoManager.SaveQuteInfo(quteNumber, "加工报价单", ref error))
            {
                lbMsg.Text = "保存成功";
                Response.Redirect("QuoteInfoList.aspx");
            }
            else
            {
                lbMsg.Text = "保存失败！原因：" + error;
            }
        }

        /// <summary>
        /// 更新产品报价
        /// </summary>
        private void UpdatePrice()
        {
            string error = string.Empty;
            string qutonumber = ToolManager.GetQueryString("id");
            string sql = string.Format(@"
update Product set SalesQuotation=a.priceNew from Product p inner join 
 (
select pcp.ProductNumber ,pcp.Version ,t.priceNew from ProductCustomerProperty pcp  inner join
(select CustomerProductNumber,
 cast( (MaterialPrcie*BOMAmount +TimeCharge+Profit+ManagementPrcie+LossPrcie) as decimal(18,2))as priceNew
  from MachineQuoteDetail where QuoteNumber='{0}' and IsMaril='否') t
 on  pcp.CustomerProductNumber=t.CustomerProductNumber) a on p.ProductNumber=a.ProductNumber
 and p.Version =a.Version ", qutonumber);
            SqlHelper.ExecuteSql(sql, ref error);
        }

        protected void btnDC_Click(object sender, EventArgs e)
        {
            string qutonumber = ToolManager.GetQueryString("id");
            string sql = string.Format(@"  select * ,
cast( (MaterialPrcie*BOMAmount +TimeCharge+Profit+ManagementPrcie+LossPrcie) as decimal(18,2))as priceNew , 
cast((MaterialPrcie*0.06+ TimeCharge*0.1) as decimal(18,2) ) as ManagementPrcieNew,
cast((MaterialPrcie*0.08) as decimal(18,2) ) as LossPrcieNew from MachineQuoteDetail where QuoteNumber='{0}'   ", qutonumber);
            sql = string.Format(@" select *,ISNULL ( vmq.qty,0) as 库存数量,isnull(t.ProcurementPrice,0) as 采购价格 from ({0}) mqd  
left join ( select mcp.CustomerMaterialNumber,vms.qty  from V_MaterialStock_Qty vms 
inner join MaterialCustomerProperty mcp on vms.MaterialNumber=mcp.MaterialNumber ) vmq
on mqd.CustomerMaterialNumber =vmq.CustomerMaterialNumber 
left join (
 select mcp.CustomerMaterialNumber,mif.ProcurementPrice from MarerialInfoTable mif 
 inner join MaterialCustomerProperty mcp on 
 mif.MaterialNumber=mcp.MaterialNumber) t on t.CustomerMaterialNumber=mqd.CustomerMaterialNumber
order by CustomerProductNumber desc,Hierarchy asc ", sql);
            sql = string.Format(" select * from ({0}) t ", sql);
            ToolCode.Tool.ExpExcel(sql, "加工报价单" + DateTime.Now.ToString("yyyy-MM-dd"));
        }
    }
}
