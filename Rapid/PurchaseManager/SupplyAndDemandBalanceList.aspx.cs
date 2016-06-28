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
    public partial class SupplyAndDemandBalanceList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                //查询
                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    //运算MRP
                    Operation();
                    GetPageOperation("btnSearch");
                } 
                divAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0202", "Add");
            }
        }
        private void GetPageOperation(string btnId)
        {
            int pageCount = 0;//总页数 
            int totalRecords = 0;//总行数
            string error = string.Empty;
            string text = string.Empty;
            string tdTextTemp = string.Empty;
            string pageIndex = ToolManager.GetQueryString("pageIndex");
            string pageSize = ToolManager.GetQueryString("pageSize");
            string sortName = ToolManager.GetQueryString("sortName");
            string sortDirection = ToolManager.GetQueryString("sortDirection");
            string querySql = ToolManager.GetQueryString("querySql");
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, string.Format(" order by {0} {1},原材料编号 desc", sortName, sortDirection), ref totalRecords);
            int columCount = dt.Columns.Count;
           //if(dt.Columns.Contains ("GUID"))
           //{}
           // if(dt.Columns .Contains (""))
           // {}
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    if (i==0)
                    {
                        tdTextTemp += "";
                       // tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        if (dt.Columns[i].ColumnName.Equals("平衡结果")&&dr[i].ToString ().Equals ("需要采购"))
                        {
                            tdTextTemp += string.Format("<td > <label class='labelRed'>{0}</label></td>", dr[i]);
                        }
                        //MaterialActualQtyDetail
                        else if (dt.Columns[i].ColumnName.Equals("实际需求数"))
                        {
                            tdTextTemp += string.Format("<td >  <a title='点击查看明细' href='MaterialActualQtyDetail.aspx?MaterialNumber={1}'>{0}</a></td>", dr[i], dr["原材料编号"]);
                        } 
                        else if (dt.Columns[i].ColumnName.Equals("在途数量"))
                        {
                            tdTextTemp += string.Format("<td >  <a title='点击查看在途明细' href='ZTDetail.aspx?MateriNumber={1}'>{0}</a></td>", dr[i], dr["原材料编号"]);
                        }
                        else if (dt.Columns[i].ColumnName.Equals("订单需求数"))
                        {
                            tdTextTemp += string.Format("<td >  <a target='_blank' title='点击查看订单需求明细' href='DDDetail.aspx?MateriNumber={1}'>{0}</a></td>", dr[i], dr["原材料编号"]);
                        }
                        //else if (dt.Columns[i].ColumnName.Equals("实际库存数量"))
                        //{
                        //    tdTextTemp += string.Format("<td >  <a   title='公式：MRP中的库存数量-原材料库房出入库类型为“生产出库、包装出库、损耗出库、销售出库（贸易）、维修出库”的且未点确认的出库单中的“数量”' href='###'>{0}</a></td>", dr[i] );
                        //}
                        else
                        {

                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                text += string.Format(@"<tr>
              
                 {1}    </tr>", dr[1], tdTextTemp);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string sql = saveInfo.Value;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql+" order by 计算结果 asc ", "供需平衡表");
        }

        /// <summary>
        /// 直接运算MRP
        /// </summary>
        private void Operation()
        {
            string error = "";
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
) t ", "select OdersNumber from V_UnfinishedOder"));
            sqls.Add("delete T_SupplyAndDemandBalance_Tmep");
            // 2015年3月22日改，阳俊
            //--实际需求数=订单需求数（产品销售订单未交货数量分解成原材料+贸易销售订单数量）-（产成品库存分解成原材料+生产线上瞬时在制产品分解成原材料）
            // 2015年10月11日改，阳俊
            //--实际需求数=产品销售订单未交货数量-产成品库存数-生产线上瞬时在制产品的数量 
            sqls.Add(@"
 insert into T_SupplyAndDemandBalance_Tmep (MaterialNumber,FactQty,OrdersDemandQty)
 select v.原材料编号,v.订单需求数-(isnull(vt.第二库存数,0)),v.订单需求数 from V_SupplyAndDemandBalanceList v left join 
 V_TwoStockMaterial vt on v.原材料编号=vt.MaterialNumber");
            SqlHelper.BatchExecuteSql(sqls, ref error);
        }
    }
}
