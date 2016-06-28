using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class SL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string result = Session["SLText"].ToString();
                //Response.Write(result);
                //Response.End();
                //return;
                LoadPage();

            }


        }

        private void LoadPage()
        {
            DataTable dttemp = SqlHelper.GetTable(GetBindSql());
            rpList.DataSource = dttemp;
            rpList.DataBind();
        }
        private string GetSql()
        {
            double tempQty = 0;//临时变量
            double tempStockQty = 0;// 临时库存数量
            string tempSql = string.Empty;
            string actualQty = string.Empty;//实际生产数量
            string error = string.Empty;
            string userId = ToolCode.Tool.GetUser().UserNumber;
            //创建虚拟库存
            string sql = string.Format("  delete T_MaterialStock_Temp where userId='{0}' ", userId);
            SqlHelper.ExecuteSql(sql, ref error);
            sql = string.Format(@"  insert into T_MaterialStock_Temp
 select MaterialNumber,库存数量,'{0}' from V_RemoveWorkInProgress_MaterialStock_Qty ", userId);
            SqlHelper.ExecuteSql(sql, ref error);

            //开始逻辑处理
            string result = Session["SLText"].ToString();
            string[] resultArray = result.Split(',');
            string qtySql = string.Empty;
            sql = "";
            foreach (string str in resultArray)
            {

                qtySql = string.Format(@" 
 select Qty  from T_WorkOrder_Temp  where User_id='{0}' and Id ='{1}' ", userId, str);
                actualQty = SqlHelper.GetScalar(qtySql);

                string[] tempArray = str.Split('^');
                //XS20140612160417^R1C00000000156-2^00^2014-06-13^10^admin
                sql += string.Format(@" select '{0}' as 销售订单号,'{1}' as 产品编号, 
'{2}' as 版本,'{3}' as 交期,'{4}' as 行号 ,{5} as 实际生产数量
union all", tempArray[0], tempArray[1], tempArray[2], tempArray[3], tempArray[4], actualQty);
            }
            sql = sql.TrimEnd(new char[] { 'u', 'n', 'i', 'o', 'n', ' ', 'a', 'l', 'l' });

            sql = string.Format(" select * from ({0}) t order by t.交期 asc ", sql);
            DataTable dt = SqlHelper.GetTable(sql);
            //遍历
            foreach (DataRow dr in dt.Rows)
            {
                sql = string.Format(@" select Type  from 
Product  where ProductNumber ='{0}' and Version ='{1}'  ", dr["产品编号"], dr["版本"]);
                if (SqlHelper.GetScalar(sql).Equals("包"))
                {
                    sql = string.Format(@"select bom.PackageNumber  as 包号, 
 bom.ProductNumber as 产成品编号,
 bom.CustomnerProductNumber as 客户产成品编号,
 bom.Version  as 版本,
 bom.MaterialNumber as 原材料编号,
 bom.CustomerMaterialNumber as 客户物料编号,
 bom.SingleDose as 单机用量,
 bom.Unit as 单位, 
 {1} as 实际生产数量,
 mit.Kind as 种类
 from V_Pack_BomInfo bom 
 inner join MarerialInfoTable mit on mit.MaterialNumber=bom.MaterialNumber  
 where bom.PackageNumber ='{0}' order by bom.PackageNumber,bom.ProductNumber, bom.Version,bom.MaterialNumber", dr["产品编号"], dr["实际生产数量"]);

                    //按行递减
                    foreach (DataRow drNew in SqlHelper.GetTable(sql).Rows)
                    {
                        sql = string.Format(@" 
 select stockQty from  T_MaterialStock_Temp
  where MaterialNumber='{0}' and UserId='{1}'  ", drNew["原材料编号"], userId);
                        tempStockQty = Convert.ToDouble(SqlHelper.GetScalar(sql));


                        tempSql += string.Format(@"  select '{13}' as 销售订单号,t.*,
'{10}' as 交期,
'{11}' as 行号,
 cast ({9} as decimal(18,2) ) as 库存数量,
case when t.种类='线材' and t.单位='mm' then {9}-(t.单机用量/1.00)*t.实际生产数量
 else {9}-t.单机用量*t.实际生产数量 end as 计算结果
  from (
 select '{12}'as 包号,'{0}' as 产成品编号 ,'{1}' as 客户产成品编号,'{2}' as 版本,'{3}' as 原材料编号,'{4}'as 客户物料编号,
 '{5}'as 单位,'{6}' as 种类,{7} as 单机用量,{8} as 实际生产数量) t 
left join  T_MaterialStock_Temp tmst on t.原材料编号=tmst.MaterialNumber
 union all ", drNew["产成品编号"], drNew["客户产成品编号"], drNew["版本"], drNew["原材料编号"], drNew["客户物料编号"]
            , drNew["单位"], drNew["种类"], drNew["单机用量"], drNew["实际生产数量"], tempStockQty, dr["交期"], dr["行号"], drNew["包号"], dr["销售订单号"]);
                        //单位换算
                        if (drNew["种类"].ToString().Equals("线材") &&
                            drNew["单位"].ToString().Equals("mm"))
                        {
                            tempQty = (Convert.ToDouble(drNew["单机用量"]) / 1.00) * Convert.ToInt32(drNew["实际生产数量"]);
                        }
                        else
                        {
                            tempQty = (Convert.ToDouble(drNew["单机用量"]) / 1.00) * Convert.ToInt32(drNew["实际生产数量"]);
                        }

                        //更新虚拟库存数量
                        sql = string.Format(@" 
 update T_MaterialStock_Temp set StockQty=StockQty-{0} where MaterialNumber='{1}' and UserId='{2}' ", tempQty, drNew["原材料编号"], userId);
                        SqlHelper.ExecuteSql(sql, ref error);
                    }

                }
                else
                {
                    sql = string.Format(@" select '' as 包号, 
 bom.ProductNumber as 产成品编号,
 bom.CustomnerProductNumber as 客户产成品编号,
 bom.Version  as 版本,
 bom.MaterialNumber as 原材料编号,
 bom.CustomerMaterialNumber as 客户物料编号,
 bom.SingleDose as 单机用量,
 bom.Unit as 单位, 
 {2} as 实际生产数量,
 mit.Kind as 种类
 from BOMInfo bom 
 inner join MarerialInfoTable mit on mit.MaterialNumber=bom.MaterialNumber  
 where bom. ProductNumber ='{0}' and bom.Version ='{1}' order by bom.ProductNumber, bom.Version, bom.MaterialNumber", dr["产品编号"], dr["版本"], dr["实际生产数量"]);
                    //按行递减
                    foreach (DataRow drNew in SqlHelper.GetTable(sql).Rows)
                    {
                        sql = string.Format(@" 
 select stockQty from  T_MaterialStock_Temp
  where MaterialNumber='{0}' and UserId='{1}'  ", drNew["原材料编号"], userId);
                        tempStockQty = Convert.ToDouble(SqlHelper.GetScalar(sql));
                        tempSql += string.Format(@"  select '{12}' as 销售订单号,t.*,
'{10}' as 交期,
'{11}' as 行号,
 cast ({9} as decimal(18,2) ) as 库存数量,
case when t.种类='线材' and t.单位='mm' then {9}-(t.单机用量/1.00)*t.实际生产数量
 else {9}-t.单机用量*t.实际生产数量 end as 计算结果
  from (
 select ''as 包号,'{0}' as 产成品编号 ,'{1}' as 客户产成品编号,'{2}' as 版本,'{3}' as 原材料编号,'{4}'as 客户物料编号,
 '{5}'as 单位,'{6}' as 种类,{7} as 单机用量,{8} as 实际生产数量) t 
left join  T_MaterialStock_Temp tmst on t.原材料编号=tmst.MaterialNumber
 union all ", drNew["产成品编号"], drNew["客户产成品编号"], drNew["版本"], drNew["原材料编号"], drNew["客户物料编号"]
            , drNew["单位"], drNew["种类"], drNew["单机用量"], drNew["实际生产数量"], tempStockQty, dr["交期"], dr["行号"], dr["销售订单号"]);
                        //单位换算
                        if (drNew["种类"].ToString().Equals("线材") &&
                            drNew["单位"].ToString().Equals("mm"))
                        {
                            tempQty = (Convert.ToDouble(drNew["单机用量"]) / 1.00) * Convert.ToInt32(drNew["实际生产数量"]);
                        }
                        else
                        {
                            tempQty = (Convert.ToDouble(drNew["单机用量"]) / 1.00) * Convert.ToInt32(drNew["实际生产数量"]);
                        }

                        //更新虚拟库存数量
                        sql = string.Format(@" 
 update T_MaterialStock_Temp set StockQty=StockQty-{0} where MaterialNumber='{1}' and UserId='{2}' ", tempQty, drNew["原材料编号"], userId);
                        SqlHelper.ExecuteSql(sql, ref error);

                    }
                }
            }

            tempSql = string.Format(@" select t.*,mi.Description as 描述,cast(t.计算结果 as decimal(18,2)) as 计算结果新,
            cast(t.库存数量 as decimal(18,2)) as 库存数量新, cast(t.单机用量 as decimal(18,3) ) as 单机用量新,
case when t.单位='mm' then 'm' else t.单位 end 单位新 ,case  when cast(t.计算结果 as decimal(18,2)) <0 then '是' else '否' end 是否缺料
            from ({0})t inner  join MarerialInfoTable  mi on t.原材料编号=mi.MaterialNumber", tempSql.TrimEnd(new char[] { 'u', 'n', 'i', 'o', 'n', ' ', 'a', 'l', 'l' }));
            tempSql = string.Format(@" select distinct * from ({0}) t  ", tempSql);

            return tempSql;

        }

        private string GetBindSql()
        {
            string conditon = " where 1=1";
            if (drpQL.SelectedValue != "")
            {
                conditon+=" and  t.是否缺料='"+drpQL.SelectedValue+"'"; 
            }
            if(txtCustomerOrderNumer.Text!="")
            {
                conditon+=" and so.CustomerOrderNumber like '%"+txtCustomerOrderNumer.Text.Trim()+"%'";
            }
            if (txtOrderNumber.Text != "")
            {
                conditon += " and t.原材料编号 like '%" + txtOrderNumber.Text.Trim() + "%'";
            }
            string sql = GetSql();
            sql = string.Format(" select *,so.CustomerOrderNumber as 客户采购订单号 from ({0})t  left join SaleOder so on t.销售订单号=so.OdersNumber {1}", sql,conditon);
            return sql;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            rpList.DataSource =SqlHelper .GetTable( GetBindSql());
            rpList.DataBind();
        }
    }
}
