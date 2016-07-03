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
    public partial class AjaxGetWorkOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //获取数据
                if (ToolManager.CheckQueryString("sortName"))
                {
                    GetPage();
                }
            }
        }

        private static void SetActualProductQty()
        {
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string tempSql = string.Format(@"
            select t.销售订单号+'^'+t.产品编号+'^'+t.版本+'^'+t.交期+'^'+t.行号+'^'+'{1}' as Id, 
            t.销售订单号,t.产品编号,t.版本,
            case when t.未交货数量 <0 then 0 else t.未交货数量 end as 未交货数量 ,
            t.库存数量,t.在制品数量,t.需要生产数量,t.交期,t.行号,t.客户产品编号 from ({0})t where 1=1",
WorkOrderManager.GetWorkOrderSql(), userId);

            List<string> sqls = new List<string>();
            string error = string.Empty;

            string sql = string.Format("delete T_WorkOrder_Temp where user_id='{0}'", userId);
            sqls.Add(sql);

            sql = string.Format(@"
            insert into T_WorkOrder_Temp(id,Qty,User_id)
            select t.Id,t.需要生产数量,'{1}' from ({0})t  
            ", tempSql, userId);
            sqls.Add(sql);

            SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        private static void GetPage()
        {
            ToolManager.ZDJC();
            SetActualProductQty();
            
            string error = string.Empty;
            string tdTextTemp = string.Empty;
            string text = string.Empty;
            string temp = string.Empty;
            string sortName = ToolManager.GetQueryString("sortName");
            string sortDirection = ToolManager.GetQueryString("sortDirection");
            string condition = ToolManager.GetQueryString("condition");
            string userId = ToolCode.Tool.GetUser().UserNumber;

            string tempSql = string.Format(@"
                    select 
                        t.销售订单号+'^'+t.产品编号+'^'+t.版本+'^'+t.交期+'^'+t.行号+'^'+'{1}' as Id, 
                        t.销售订单号,
                        t.产品编号,
                        t.版本,
                        case when  t.未交货数量 <0 then 0 else t.未交货数量  end as 未交货数量 ,
                        t.库存数量,
                        t.在制品数量,
                        t.未入库数量,
                        t.送货单未确认数量,
                        t.需要生产数量,
                        t.交期,
                        t.行号,
                        t.客户产品编号 
                    from ({0})t 
                    where 1=1"
             , WorkOrderManager.GetWorkOrderSql(), userId);
            
            string sql = string.Format(@" 
                select 
                        t.Id,
                        t.销售订单号,
                        so.CustomerOrderNumber as 客户采购订单号,
                        so.OdersType as 订单类型,
                        t.客户产品编号,
                        t.产品编号,
                        t.版本, 
                        t.未交货数量,
                        t.库存数量 as 实时库存数量,
                        isnull( t.在制品数量,0),
                        t.未入库数量,
                        t.送货单未确认数量,
                        t.需要生产数量,
                        twt.Qty as 实际生产数量,
                        t.交期,
                        t.行号 
                from 
                ({0}) t 
                left join SaleOder so on so.OdersNumber=t.销售订单号 
                left join T_WorkOrder_Temp twt on twt.Id=t.Id 
                {3} 
                --and  t.需要生产数量>0
                order by {1} {2}"
                , tempSql, sortName, sortDirection, condition);
            int columCount = 0;
            DataTable dt = SqlHelper.GetTable(sql);


            if (dt != null)
            {
                columCount = dt.Columns.Count;
            }
            else
            {
                HttpContext.Current.Response.Write(text);
                HttpContext.Current.Response.End();
                return;
            }


            string color = string.Empty;
            string titileName = string.Empty;



            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                temp = "";
                for (int i = 0; i < columCount; i++)
                {
                    //if (i == 7)
                    //{
                    //    string sqlGetQty = string.Format("select StockQty from  ProductStock where ProductNumber='{0}' and  Version='{1}'", dr["产品编号"], dr["版本"]);
                    //    object ob = SqlHelper.GetScalar(sqlGetQty);
                    //    ob = ob == null ? 0 : ob;
                    //    tdTextTemp += string.Format("<td>{0}</td><td>{1}</td>", ob, dr[i]);
                    //}
                    //else
                    //{
                    if (!dt.Columns[i].ColumnName.Equals("Id"))
                    {
                        if (dt.Columns[i].ColumnName.Equals("客户产品编号"))
                        {
                            color = HasGX(dr["产品编号"].ToString(), dr["版本"].ToString()) ? "black" : "red";
                            titileName = HasGX(dr["产品编号"].ToString(), dr["版本"].ToString()) ? "" : "title='该产品没有工序信息'";
                            tdTextTemp += string.Format("<td><span style='color:{1};' {2}>{0}</span> </td>", dr[i], color, titileName);
                        }
                        else if (dt.Columns[i].ColumnName.Equals("产品编号"))
                        {
                            color = HasBOM(dr["产品编号"].ToString(), dr["版本"].ToString()) ? "black" : "red";
                            titileName = HasBOM(dr["产品编号"].ToString(), dr["版本"].ToString()) ? "" : "title='该产品没有BOM信息'";
                            tdTextTemp += string.Format("<td><span style='color:{1};' {2}>{0}</span> </td>", dr[i], color, titileName);

                        }
                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                    //}
                }
                //bool isQueLiao = GetIsQueLiao(dr["产品编号"].ToString(), dr["版本"].ToString(), dr["实际生产数量"].ToString());
                //if (isQueLiao)
                //{
                //    tdTextTemp += string.Format("<td><a href='###' title='点击查看缺料明细' onclick=\"Check('{0}','{1}','{2}','{3}')\">是</a></td>", dr["产品编号"].ToString(), dr["版本"].ToString(), dr["实际生产数量"].ToString(), dr["客户产品编号"]);
                //}
                //else
                //{
                //    tdTextTemp += string.Format("<td>否</td>");
                //}


                tdTextTemp += string.Format(@" <td><a href='###' onclick=""EditQty('{0}','{1}')"">编辑</a> </td>", dr["Id"], dr["需要生产数量"]);

                temp = string.Format(" <input type='checkbox' name='subBox' value='{0}'/> ", dr["Id"]);
                text += string.Format(@"<tr><td>{0}</td>{1}</tr>", temp, tdTextTemp);
            }
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
            return;
        }

        //private void bool IsHasGX(string productNumber,string version)
        //{

        //}


        /// <summary>
        /// 检查某产品是否有工序
        /// </summary>
        /// <param name="productNumber"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private static bool HasGX(string productNumber, string version)
        {
            string sql = string.Format(" select COUNT(*) from ProductWorkSnProperty where ProductNumber ='{0}' and Version ='{1}' ", productNumber, version);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }

        /// <summary>
        /// 检查某产品是否有BOM
        /// </summary>
        /// <param name="productNumber"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        private static bool HasBOM(string productNumber, string version)
        {
            string sql = string.Format("  select COUNT(*) from BOMInfo where ProductNumber ='{0}' and Version ='{1}'", productNumber, version);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }

        /// <summary>
        /// 检测某一产品是否缺料
        /// </summary>
        /// <param name="productNumber"></param>
        /// <param name="version"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        private static bool GetIsQueLiao(string productNumber, string version, string qty)
        {
            string sql = string.Format(@" 
select Type  from Product  where ProductNumber ='{0}' and Version ='{1}' ", productNumber, version);
            if (SqlHelper.GetScalar(sql).Equals("包"))
            {
                //                sql = string.Format(@" select COUNT(*)  from V_BOM_Sum_SingleDose bom 
                // inner join (select ProductNumber ,Version ,SingleDose as 产品单机 
                //   from  PackageAndProductRelation where PackageNumber='{0}') a 
                //   on a.ProductNumber =bom .ProductNumber 
                //   and a.Version =bom.Version
                //   left join V_MaterialStock_Qty vsq on vsq.MaterialNumber =bom.MaterialNumber 
                //   where (bom.SingleDose*a.产品单机*1 )-vsq.qty>0", productNumber);
                sql = string.Format(@"

select COUNT(*) from (
select  
'' as 客户包号,
a.PackageNumber as 包号,
bom.CustomnerProductNumber as 客户产成品编号,
a.ProductNumber as 产成品编号,
a.Version as 版本,
a.产品单机,
bom.CustomerMaterialNumber as 原材料编号,
bom.CustomerMaterialNumber as 客户物料号,
 bom.SingleDose as 物料单机用量,
 bom .Unit as 单位,
 {1} as 实际生产数量,
 bom.SingleDose*a.产品单机*{1} as 总实际生产数量, 
 vsq.qty as 库存数量,
 vsq.qty-(bom.SingleDose*a.产品单机*{1} ) as 缺料数量   
 from V_BOM_Sum_SingleDose bom 
 inner join (select PackageNumber,ProductNumber ,Version ,SingleDose as 产品单机 
   from  PackageAndProductRelation where PackageNumber='{0}') a 
   on a.ProductNumber =bom .ProductNumber 
   and a.Version =bom.Version
   left join V_MaterialStock_Qty vsq on vsq.MaterialNumber =bom.MaterialNumber
   ) b where b.缺料数量<0", productNumber, qty);
                return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
            }
            else
            {
                //                sql = string.Format(@"select COUNT (*)  from 
                //(select ProductNumber ,Version , MaterialNumber ,SingleDose*{0} as 需要生产数量 from V_BOM_Sum_SingleDose)a
                //left join (
                //select ProductNumber ,Version ,bom.MaterialNumber ,ISNULL (vmq.qty ,0) as 库存数量 from V_BOM_Sum_SingleDose bom left join 
                //V_MaterialStock_Qty vmq on  bom.MaterialNumber =vmq.MaterialNumber ) b on a.ProductNumber=b.ProductNumber 
                //and a.Version =b.Version and a.MaterialNumber =b.MaterialNumber 
                //where (a.需要生产数量-b.库存数量)>0 and a.ProductNumber ='{1}' and a.Version ='{2}'", qty, productNumber, version);

                sql = string.Format(@"
select COUNT (*) from (
select 
'' as 客户包号,
'' as  包号,
a.CustomnerProductNumber as 客户产成品编号,
a.ProductNumber as 产成品编号,
a.Version as 版本,
1 as 产品单机,
a.CustomerMaterialNumber as 客户物料号,
a.MaterialNumber as 原材料编号,
a.SingleDose as 物料单机用量,
a.Unit as 单位,
{0} as 实际生产数量,
{0}*a.SingleDose as 总实际生产数量,
vmq.qty as 库存数量,
(vmq.qty -a.SingleDose*{0}) as 缺料数量  from (
select CustomnerProductNumber,ProductNumber,Version ,CustomerMaterialNumber,MaterialNumber,
SUM (SingleDose ) SingleDose,Unit  from BOMInfo 
where ProductNumber ='{1}' and Version ='{2}'
group by CustomnerProductNumber , ProductNumber ,Version, CustomerMaterialNumber ,MaterialNumber,SingleDose,unit 
) a left join  V_MaterialStock_Qty vmq on a.MaterialNumber =vmq.MaterialNumber
) b where b.缺料数量<0", qty, productNumber, version);

                return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
            }
        }


    }
}
