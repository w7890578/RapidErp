using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BLL;
using Rapid.ToolCode;

namespace Rapid.PurchaseManager
{
    public partial class AddMrp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }
        private void LoadPage()
        {
            this.liOrder.DataSource = SqlHelper.GetTable(@"select OdersNumber from V_UnfinishedOder
union
select OdersNumber  from  SaleOder where OdersType ='MRP订单'");
            this.liOrder.DataTextField = "OdersNumber";
            this.liOrder.DataValueField = "OdersNumber";
            this.liOrder.DataBind();
            SelectAll();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string items = string.Empty;
            foreach (ListItem item in liOrder.Items) //按类型listitem读取listbox中选定项
            {
                if (item.Selected) //判断是否选中
                {
                    items += string.Format("'{0}',", item.Value);
                }
            }
            if (string.IsNullOrEmpty(items))
            {
                lbSubmit.Text = "请选择销售订单！";
                return;
            }
            items = items.TrimEnd(',');
            List<string> sqls = new List<string>();
            sqls.Add("delete SupplyAndDemandBalance");
            sqls.Add(string.Format(@"insert into SupplyAndDemandBalance (MaterialNumber ,OrdersDemandQty)
select t.MaterialNumber,t.总订单需求数 from (
select mit.MaterialNumber,isnull(a.需求数量一,0)+ISNULL( b.需求数量二,0)+ISNULL (c.需求数量三,0) as 总订单需求数 
from MarerialInfoTable mit  left join (
select  t.原材料编号,sum(t.需求数量一) as 需求数量一 from(
select vbc.原材料编号,vbc.单机用量*a.数量 as 需求数量一 from (
select t.ProductNumber ,t.Version ,t.NonDeliveryQty  as 数量 from (
select ProductNumber ,Version ,SUM (NonDeliveryQty) as NonDeliveryQty from MachineOderDetail where OdersNumber in ({0})
group by ProductNumber ,Version 
) t  where t.NonDeliveryQty >0
) a  inner  join V_BOM_Count vbc on a.ProductNumber=vbc.产成品编号 and a.Version=vbc.版本
and isnull(vbc.包号,'')=''
) t group by t.原材料编号
) a on mit.MaterialNumber=a.原材料编号
left join (
select   t.原材料编号,sum(t.需求数量二) as 需求数量二 from(
select vbc.原材料编号,vbc.单机用量*a.数量 as 需求数量二 from (
select t.ProductNumber ,t.Version ,t.NonDeliveryQty as 数量 from (
select ProductNumber ,Version ,SUM (NonDeliveryQty) as NonDeliveryQty from MachineOderDetail where OdersNumber in ({0})
group by ProductNumber ,Version 
) t  
where t.NonDeliveryQty >0
) a  inner  join V_BOM_Count vbc on a.ProductNumber=vbc.包号 
and isnull(vbc.包号,'')!=''
)t group by t.原材料编号
) b on mit.MaterialNumber=b.原材料编号
left join (
select ProductNumber,SUM (NonDeliveryQty) as 需求数量三  from TradingOrderDetail where OdersNumber in ({0})
group by ProductNumber ) c on mit.MaterialNumber=c.ProductNumber
where isnull(a.需求数量一,0)+ISNULL( b.需求数量二,0)+ISNULL (c.需求数量三,0)>0
) t ", items));
            sqls.Add("delete T_SupplyAndDemandBalance_Tmep");
           // 2015年3月22日改，阳俊
//--实际需求数=订单需求数（产品销售订单未交货数量分解成原材料+贸易销售订单数量）-（产成品库存分解成原材料+生产线上瞬时在制产品分解成原材料）
            sqls.Add(@"
 insert into T_SupplyAndDemandBalance_Tmep (MaterialNumber,FactQty,OrdersDemandQty)
 select v.原材料编号,v.订单需求数-(isnull(vt.第二库存数,0)),v.订单需求数 from V_SupplyAndDemandBalanceList v left join 
 V_TwoStockMaterial vt on v.原材料编号=vt.MaterialNumber");
//            sqls.Add(string.Format(@"insert into T_SupplyAndDemandBalance_Tmep (MaterialNumber,FactQty,OrdersDemandQty)
//select t.MaterialNumber,t.总实际需求数,isnull( vsbl.订单需求数,0) from (
//select mit.MaterialNumber,isnull(a.需求数量一,0)+ISNULL( b.需求数量二,0)+ISNULL (c.需求数量三,0) as 总实际需求数 from MarerialInfoTable mit  left join (
//select  t.原材料编号,sum(t.需求数量一) as 需求数量一 from(
//select vbc.原材料编号,vbc.单机用量*a.数量 as 需求数量一 from (
//select t.ProductNumber ,t.Version ,t.NonDeliveryQty - isnull(vps.库存数量,0) as 数量 from (
//select ProductNumber ,Version ,SUM (NonDeliveryQty) as NonDeliveryQty from MachineOderDetail where OdersNumber in ({0})
//group by ProductNumber ,Version 
//) t  left join V_ProductStock_Sum vps on t.ProductNumber =vps.ProductNumber and vps.Version=t.Version
//where t.NonDeliveryQty - isnull(vps.库存数量,0)>0
//) a  inner  join V_BOM_Count vbc on a.ProductNumber=vbc.产成品编号 and a.Version=vbc.版本
//and isnull(vbc.包号,'')=''
//) t group by t.原材料编号
//) a on mit.MaterialNumber=a.原材料编号
//left join (
//select   t.原材料编号,sum(t.需求数量二) as 需求数量二 from(
//select vbc.原材料编号,vbc.单机用量*a.数量 as 需求数量二 from (
//select t.ProductNumber ,t.Version ,t.NonDeliveryQty - isnull(vps.库存数量,0) as 数量 from (
//select ProductNumber ,Version ,SUM (NonDeliveryQty) as NonDeliveryQty from MachineOderDetail where OdersNumber in ({0})
//group by ProductNumber ,Version 
//) t  left join V_ProductStock_Sum vps on t.ProductNumber =vps.ProductNumber and vps.Version=t.Version
//where t.NonDeliveryQty - isnull(vps.库存数量,0)>0
//) a  inner  join V_BOM_Count vbc on a.ProductNumber=vbc.包号 
//and isnull(vbc.包号,'')!=''
//)t group by t.原材料编号
//) b on mit.MaterialNumber=b.原材料编号
//left join (
//select ProductNumber,SUM (NonDeliveryQty) as 需求数量三  from TradingOrderDetail where OdersNumber in ({0})
//group by ProductNumber ) c on mit.MaterialNumber=c.ProductNumber
//where isnull(a.需求数量一,0)+ISNULL( b.需求数量二,0)+ISNULL (c.需求数量三,0)>0
//) t left join V_SupplyAndDemandBalanceList vsbl on t.MaterialNumber= vsbl.原材料编号", items));

            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);







            if (result)
            {
                //Response.Write(ToolManager.GetClosePageJS());
                //Response.End();
                lbSubmit.Text = "运行成功！";
                Tool.WriteLog(Tool.LogType.Operating, "运行MRP", "运行成功");

                return;
            }
            else
            {
                lbSubmit.Text = "运行失败！原因：" + error;
                Tool.WriteLog(Tool.LogType.Operating, "运行MRP", "运行失败！原因" + error);
                return;
            }
        }

        private void SelectAll()
        {
            foreach (ListItem item in liOrder.Items) //按类型listitem读取listbox中选定项
            {
                item.Selected = true;
            }
        }
        protected void btnSelectAll_Click(object sender, EventArgs e)
        {
            SelectAll();
        }
    }
}
