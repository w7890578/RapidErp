using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace BLL
{
    /// <summary>
    /// 开工单管理类
    /// </summary>
    public class WorkOrderManager
    {
        /// <summary>
        /// 开工单确认生成原材料出库单
        /// </summary>
        /// <param name="planNumber">开工单号</param>
        /// <param name="userId">当前登录用户ID</param>
        /// <param name="CRKType">出入库类型</param>
        /// <param name="error">异常信息</param>
        /// <returns></returns>
        public static bool EnterKGD(string planNumber, string userId, ref string error, string CRKType)
        {
            string sql = string.Format(@"select Isconfirm from ProductPlan where PlanNumber ='{0}'", planNumber);
            if (SqlHelper.GetScalar(sql).Equals("已确认"))
            {
                error = string.Format("开工单{0}已领料<br/>", planNumber);
                return false;
            }
            sql = string.Format(@"select COUNT(*) from ProductPlanDetail ppd
 inner join SaleOder so  on ppd.OrdersNumber =so.OdersNumber
  where ppd.PlanNumber ='{0}' and so.OdersType ='维修订单'", planNumber);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                CRKType = "维修出库";
            }
            string isConfrim = CRKType.Equals("维修出库") ? "否" : "是";
            //string isConfrim = "是";
            List<string> sqls = new List<string>();
            string warehouseNumber = "YCLCK" + DateTime.Now.ToString("yyyyMMddHHmmss");
            //生成原材料出库单
            sql = string.Format(@"
            insert into MarerialWarehouseLog(WarehouseNumber,WarehouseName,ChangeDirection ,Type ,Creator,CreateTime ,Remark ,IsConfirm)
            values('{0}','ycl','出库','{3}','{1}','{2}','','{4}')", warehouseNumber, userId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), CRKType, isConfrim);
            sqls.Add(sql);
            if (CRKType.Equals("生产出库"))
            {
                sql = string.Format(@"
insert into MaterialWarehouseLogDetail (WarehouseNumber,DocumentNumber  ,MaterialNumber ,CustomerMaterialNumber ,Qty,RowNumber,LeadTime )
select '{0}','{1}',a.MaterialNumber ,mcp.CustomerMaterialNumber,a.数量,'1','1' from (
select t.MaterialNumber ,t.CustomerId,SUM (t.数量) as 数量 from (
select bom.MaterialNumber ,so.CustomerId ,
ppd.Qty*bom.SingleDose as 数量   from ProductPlanDetail ppd inner join BOMInfo bom
on ppd.ProductNumber =bom.ProductNumber and ppd.Version =bom.Version
inner join SaleOder so on ppd.OrdersNumber =so.OdersNumber
where ppd.PlanNumber ='{1}'
union all
select bom.原材料编号 ,so.CustomerId ,
ppd.Qty*bom.单机用量 as 数量   from ProductPlanDetail ppd inner join V_BOM_Count bom
on ppd.ProductNumber =bom.包号
inner join SaleOder so on ppd.OrdersNumber =so.OdersNumber
where ppd.PlanNumber ='{1}') t group by t.MaterialNumber ,t.CustomerId
) a left join MaterialCustomerProperty mcp on a.MaterialNumber =mcp.MaterialNumber
and a.CustomerId=mcp.CustomerId
", warehouseNumber, planNumber);
            }
            else
            {
                sql = string.Format(@"
insert into MaterialWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,MaterialNumber ,RowNumber ,
LeadTime ,ProductNumber,CustomerMaterialNumber ,Qty ,CustomerProductNumber,Version,SingleDose)
select * from (
select '{1}' as WarehouseNumber,t.PlanNumber ,t.MaterialNumber,t.RowNumber ,t.LeadTime ,t.ProductNumber ,
mcp.CustomerMaterialNumber,t.Qty*SingleDose as Qty  ,t.CustomnerProductNumber,t.Version,SingleDose from (
select ppd.PlanNumber ,bom.MaterialNumber ,ppd.RowNumber,ppd.LeadTime ,
bom.ProductNumber,bom.CustomnerProductNumber,ppd.Version ,so.CustomerId ,
ppd.Qty ,bom.SingleDose   from ProductPlanDetail ppd inner join BOMInfo bom
on ppd.ProductNumber =bom.ProductNumber and ppd.Version =bom.Version
inner join SaleOder so on ppd.OrdersNumber =so.OdersNumber
where ppd.PlanNumber ='{0}') t  left join MaterialCustomerProperty mcp on t.MaterialNumber =mcp.MaterialNumber
and t.CustomerId=mcp.CustomerId
union all
select '{1}',t.PlanNumber ,t.原材料编号,t.RowNumber ,t.LeadTime ,t.包号 ,
mcp.CustomerMaterialNumber,t.Qty*t.单机用量 as Qty ,t.客户包号,'',t.单机用量 from (
select ppd.PlanNumber ,bom.原材料编号 ,ppd.RowNumber,ppd.LeadTime ,
bom.包号,bom.客户包号,'' as version ,so.CustomerId ,
ppd.Qty ,bom.单机用量   from ProductPlanDetail ppd inner join V_BOM_Count  bom
on ppd.ProductNumber =bom.包号
inner join SaleOder so on ppd.OrdersNumber =so.OdersNumber
where ppd.PlanNumber ='{0}') t  left join MaterialCustomerProperty mcp on t.原材料编号 =mcp.MaterialNumber
and t.CustomerId=mcp.CustomerId) a ", planNumber, warehouseNumber);
            }
            sqls.Add(sql);

            sql = string.Format(" update ProductPlan set IsConfirm ='已确认' where PlanNumber ='{0}' ", planNumber);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        /// <summary>
        /// 生成工序开工单
        /// </summary>
        /// <param name="userId">当前登录用户</param>
        /// <returns>执行结果</returns>
        public static string GenerateGX(string userId)
        {
            List<string> lsSqls = new List<string>();
            string sql = string.Format(@" select COUNT (*) from  T_Process_Temp where ISNULL (Team ,'')!='' and UserId ='{0}'", userId);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                return "请设置班组开工";
            }
            string kgNumber = "KG" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string summaryTableSql = string.Empty; ;//总表sql
            string summaryTableDetailSql = string.Empty;//总表明细sql
            string tempSummaryTableDetailSql = string.Empty; //总表明细临时Sql
            List<string> summaryTableDetailSqls = new List<string>();

            string pointsTableSql = string.Empty;//分表sql
            string tempPointsTableSql = string.Empty;//分表临时sql

            List<string> pointsTableDetailSqls = new List<string>();
            string pointsTableDetailSql = string.Empty;//分表明细sql
            string tempPointsTableDetailSql = string.Empty;//分表明细临时Sql

            string edGS = string.Empty; //额定工时
            string persionQty = string.Empty; //总人数
            string targetGS = string.Empty;//目标完成工时
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string workSnOneNumber = string.Empty; //工序一
            string workSnTwoNumber = string.Empty; //工序二
            string workSnThreeNumber = string.Empty; //工序三
            string workSnFourNumber = string.Empty;  //工序四

            SqlHelper.ExecuteSql(string.Format(" update T_Process_Temp set NeedToProduceQty =Qty where UserId='{0}'", userId), ref error);

            DataTable dt = null;
            //插入分表明细
            //先找出订单产品班组按人员，订单，产品，班组分组
            sql = string.Format(@" select UserId ,OrdersNumber ,ProductNumber ,Version ,CustomerProductNumber,RowNumber ,NeedToProduceQty ,Team,LeadTime from T_Process_Temp
 where UserId='{0}'
  group by UserId ,OrdersNumber ,ProductNumber ,Version ,CustomerProductNumber,RowNumber ,NeedToProduceQty ,Team,LeadTime ", userId);
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                //找出该班组做这个产品的哪几道工序
                sql = string.Format(@" select  WorkSnNumber from T_Process_Temp where UserId='{0}'
    and OrdersNumber ='{1}'
  and ProductNumber ='{2}' and Version ='{3}'
 and CustomerProductNumber='{4}' and RowNumber ='{5}'
and  Team ='{6}'  order by SN asc", userId, dr["OrdersNumber"], dr["ProductNumber"]
        , dr["Version"], dr["CustomerProductNumber"], dr["RowNumber"], dr["Team"]);
                dt = SqlHelper.GetTable(sql);

                workSnOneNumber = string.Empty; //工序一
                workSnTwoNumber = string.Empty; //工序二
                workSnThreeNumber = string.Empty; //工序三
                workSnFourNumber = string.Empty;  //工序四

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            workSnOneNumber = dt.Rows[0]["WorkSnNumber"].ToString();
                            break;

                        case 1:
                            workSnTwoNumber = dt.Rows[1]["WorkSnNumber"].ToString();
                            break;

                        case 2:
                            workSnThreeNumber = dt.Rows[2]["WorkSnNumber"].ToString();
                            break;

                        case 3:
                            workSnFourNumber = dt.Rows[3]["WorkSnNumber"].ToString();
                            break;
                    }
                }

                //几道工序工时加起来就是单套工时
                sql = string.Format(@" select SUM ( pwsp.RatedManhour)   from (
 select ProductNumber ,Version ,WorkSnNumber from T_Process_Temp where UserId='{0}' and OrdersNumber ='{1}'
  and ProductNumber ='{2}' and Version ='{3}'
 and CustomerProductNumber='{4}' and RowNumber ='{5}'  and Team ='{6}'
 )t  inner  join ProductWorkSnProperty  pwsp on t.ProductNumber =pwsp.ProductNumber and t.Version =pwsp .Version
 and t.WorkSnNumber=pwsp .WorkSnNumber", userId, dr["OrdersNumber"], dr["ProductNumber"]
        , dr["Version"], dr["CustomerProductNumber"], dr["RowNumber"], dr["Team"]);
                edGS = SqlHelper.GetScalar(sql); //单套工时(分钟)

                //插入分表明细Sql遍历添加到list
                sql = string.Format(@"insert into ProductPlanSubDetail(PlanNumber ,Team ,OrdersNumber ,ProductNumber ,Version
,RowNumber ,CustomerProductNumber,Qty ,RatedManhour ,TotalManhour,WorkSn1 ,WorkSn2,WorkSn3,WorkSn4, LeadTime )
values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9},'{10}','{11}','{12}','{13}','{14}')
", kgNumber, dr["Team"], dr["OrdersNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"], dr["CustomerProductNumber"],
        dr["NeedToProduceQty"], Convert.ToDouble(edGS) / 60, Convert.ToInt32(dr["NeedToProduceQty"]) * (Convert.ToDouble(edGS) / 60), workSnOneNumber, workSnTwoNumber, workSnThreeNumber, workSnFourNumber, dr["LeadTime"]);
                pointsTableDetailSqls.Add(sql);                                 //分钟转换小时

                //存储分表明细信息
                tempPointsTableDetailSql += string.Format(@"union all select '{0}' as 开工单号,'{1}' as 班组,'{2}' as  销售订单号,'{3}' as 产品编号,
'{4}' as 版本,'{5}' as 行号,'{6}' as 客户产品编号,{7} as 套数,{8} as 单套工时,{9} as 合计工时,'{10}' as 工序1,'{11}' as 工序2,'{12}' as 工序3,'{13}' as 工序4,'{14}' as 交期 ",
        kgNumber, dr["Team"], dr["OrdersNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"], dr["CustomerProductNumber"],
        dr["NeedToProduceQty"], Convert.ToDouble(edGS) / 60, Convert.ToInt32(dr["NeedToProduceQty"]) * (Convert.ToDouble(edGS) / 60), workSnOneNumber, workSnTwoNumber, workSnThreeNumber, workSnFourNumber, dr["LeadTime"]);
                //分钟转换小时
                //获取该产品的额定工时
                sql = string.Format(@"select ISNULL( RatedManhour,0) from
Product where ProductNumber='{0}' and Version ='{1}'", dr["ProductNumber"], dr["Version"]);
                edGS = SqlHelper.GetScalar(sql);
                edGS = string.IsNullOrEmpty(edGS) ? "0" : edGS;

                //                //总表明细插入
                //                sql = string.Format(@"insert into ProductPlanDetail
                //(PlanNumber ,OrdersNumber ,ProductNumber ,Version ,RowNumber ,CustomerProductNumber ,Qty ,LeadTime ,RatedManhour ,TotalManhour )
                //values('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}',{8},{9})
                //", kgNumber, dr["OrdersNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"], dr["CustomerProductNumber"]
                // , dr["NeedToProduceQty"], dr["LeadTime"], Convert.ToDouble(edGS) / 60, (Convert.ToDouble(edGS) / 60) * Convert.ToInt32(dr["NeedToProduceQty"]));
                //                summaryTableDetailSqls.Add(sql);

                tempSummaryTableDetailSql += string.Format(@"union
select '{0}' as 开工单号,'{1}' as 销售订单号,'{2}' as 产品编号,'{3}' as 版本,'{4}' as 行号,'{5}' as 客户产品编号,
{6} as 套数,'{7}' as 交期 ,{8} as 单套工时,{9} as 合计工时 ", kgNumber, dr["OrdersNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"], dr["CustomerProductNumber"]
        , dr["NeedToProduceQty"], dr["LeadTime"], Convert.ToDouble(edGS) / 60, (Convert.ToDouble(edGS) / 60) * Convert.ToInt32(dr["NeedToProduceQty"]));

                sql = string.Format(@" update ProductPlanSubDetail set TotalManhour =Qty*RatedManhour
 where PlanNumber ='{0}' ", kgNumber);
                lsSqls.Add(sql);
            }

            //去除开头union all
            tempPointsTableDetailSql = tempPointsTableDetailSql.TrimStart(new char[] { 'u', 'n', 'i', 'o', 'n', ' ', 'a', 'l', 'l' });
            tempSummaryTableDetailSql = tempSummaryTableDetailSql.TrimStart(new char[] { 'u', 'n', 'i', 'o', 'n' });

            // 总表明细插入
            sql = string.Format(@"insert into ProductPlanDetail
(PlanNumber ,OrdersNumber ,ProductNumber ,Version ,RowNumber ,CustomerProductNumber ,Qty ,LeadTime ,RatedManhour ,TotalManhour )
{0}  ", tempSummaryTableDetailSql);
            summaryTableDetailSqls.Add(sql);

            //分表从明细中汇总
            tempPointsTableSql = string.Format(@" select '{1}' as 开工单号,a.班组,b.人数,
        cast( round( cast(a.总额定工时 as  decimal(18,2) ),2) as decimal(18,2)) as 额定总工时,
        cast( round( cast(a.总额定工时 as  decimal(18,2) )/b.人数/1,2) as decimal(18,2)) as 目标完成工时
        from
        ( select t.班组,sum(t.合计工时) as 总额定工时 from ({0})t group by t.班组) a inner join
        ( select Team ,COUNT (*) as 人数 from PM_USER  group by Team) b on a.班组=b.Team", tempPointsTableDetailSql, kgNumber);
            //分表插入sql
            pointsTableSql = string.Format(@"insert into ProductPlanSub
(PlanNumber ,Team ,PersonQty ,RatedTotalManhour ,TargetFinishManhour) {0}", tempPointsTableSql);

            //总表额定工时
            sql = string.Format(@" select sum(t.合计工时) from ({0})t ", tempSummaryTableDetailSql);
            edGS = SqlHelper.GetScalar(sql);
            //总表总人数
            sql = string.Format("select sum(t.人数) from ({0})t ", tempPointsTableSql);
            persionQty = SqlHelper.GetScalar(sql);
            //目标完成工时=额定工时/人数
            targetGS = (Convert.ToDouble(edGS) / Convert.ToInt32(persionQty) / 1).ToString();
            summaryTableSql = string.Format(@"
insert into ProductPlan(PlanNumber ,Type ,CreateTime ,Creator
,PersonQty ,RatedTotalManhour ,TargetFinishManhour) values('{0}','{1}','{2}','{3}',{4},{5},{6})", kgNumber, "工序",
        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userId, persionQty, edGS, targetGS);
            //插入顺序 总表 》 总表明细 》 分表 》 分表明细
            sqls.Add(summaryTableSql);
            sqls.AddRange(summaryTableDetailSqls);
            sqls.Add(pointsTableSql);
            sqls.AddRange(pointsTableDetailSqls);

            //生成裁线信息表
            //            sql = string.Format(@"
            //insert into CuttingLineInfo(PlanNumber ,ProductNumber ,Version ,MaterialNumber ,Length ,CustomerProductNumber,CustomerMaterialNumber,Qty ,RatedManhour,Remark)
            //select '{0}',c.ProductNumber ,c.Version ,c.MaterialNumber ,c.[Length]
            //,c.CustomerProductNumber,c.CustomerMaterialNumber,c.Qty ,pwsp.RatedManhour,'生成工序开工单时产生' from (
            //select b.*,mcp.CustomerMaterialNumber from (
            //select pcli.ProductNumber ,pcli.Version ,pcli.MaterialNumber ,pcli.Length
            //,a.CustomerProductNumber,pcli.Qty,so.CustomerId,a.WorkSnNumber from (
            //select UserId ,OrdersNumber ,ProductNumber ,Version ,CustomerProductNumber,WorkSnNumber  from T_Process_Temp
            // where UserId='{1}' and WorkSnNumber='cx'
            //  group by UserId ,OrdersNumber ,ProductNumber ,Version ,CustomerProductNumber,WorkSnNumber
            //  )
            //  a  inner join ProductCuttingLineInfo pcli on a.ProductNumber =pcli.ProductNumber and a.Version =pcli.Version
            //  inner join SaleOder so on so.OdersNumber =a.OrdersNumber
            //  ) b inner join MaterialCustomerProperty mcp on b.CustomerId =mcp.CustomerId
            //  and b.MaterialNumber=mcp.MaterialNumber ) c
            //  inner join ProductWorkSnProperty pwsp on pwsp.ProductNumber =c.ProductNumber
            //  and pwsp.Version =c.Version  and c.WorkSnNumber =pwsp.WorkSnNumber ", kgNumber, userId);
            //不是包的生成裁线信息表
            sql = string.Format(@"  insert into CuttingLineInfo(PlanNumber ,ProductNumber ,Version ,MaterialNumber ,Length ,
CustomerProductNumber,Qty,CustomerId)
  select '{0}',bom.ProductNumber ,bom.Version ,bom.MaterialNumber ,bom.SingleDose ,bom.CustomnerProductNumber
   ,b.needQty,so.CustomerId   from
  (
  select a.UserId ,a.OrdersNumber ,a.ProductNumber ,a.Version ,
  CustomerProductNumber,WorkSnNumber,SUM(NeedToProduceQty ) as needQty
  from T_Process_Temp
  a inner join Product p on p.ProductNumber=a.ProductNumber and a.Version =p.Version
  where p.Type !='包'
  and a.UserId='{1}' and WorkSnNumber='cx'
  group by a.UserId ,a.OrdersNumber ,a.ProductNumber ,a.Version ,a.CustomerProductNumber,a.WorkSnNumber
  ) b
  inner  join
  (select bom.*,mit.Kind  from BOMInfo bom inner join  MarerialInfoTable mit on bom.MaterialNumber =mit.MaterialNumber)
   bom on bom.ProductNumber =b.ProductNumber and bom.Version =b.Version
   inner join SaleOder so on so.OdersNumber =b.OrdersNumber
   where bom.Kind ='线材' and bom.unit='mm' ", kgNumber, userId);
            sqls.Add(sql);

            //是包的生成裁线信息表

            sql = string.Format(@"    insert into CuttingLineInfo(PlanNumber ,ProductNumber ,Version ,MaterialNumber ,Length ,
CustomerProductNumber,Qty,CustomerId)
  select '{0}',bom.ProductNumber ,bom.Version ,bom.MaterialNumber ,bom.SingleDose ,bom.CustomnerProductNumber
   ,b.needQty,so.CustomerId   from
  (
  select a.UserId ,a.OrdersNumber ,a.ProductNumber ,a.Version ,
  CustomerProductNumber,WorkSnNumber,SUM(NeedToProduceQty ) as needQty
  from T_Process_Temp
  a inner join Product p on p.ProductNumber=a.ProductNumber and a.Version =p.Version
  where p.Type ='包'
  and a.UserId='{1}'  and WorkSnNumber='cx'
  group by a.UserId ,a.OrdersNumber ,a.ProductNumber ,a.Version ,a.CustomerProductNumber,a.WorkSnNumber
  ) b
  inner  join
  ( select bom.*,mit.Kind  from V_Pack_BomInfo bom inner join  MarerialInfoTable mit on bom.MaterialNumber =mit.MaterialNumber)
   bom on bom.PackageNumber =b.ProductNumber
   inner join SaleOder so on so.OdersNumber =b.OrdersNumber
   where bom.Kind ='线材' and bom.unit='mm'", kgNumber, userId);
            sqls.Add(sql);

            //            string warehouseNumber = "YCLCK" + DateTime.Now.ToString("yyyyMMddHHmmss");
            //            //生成原材料生产出库单
            //            sql = string.Format(@"
            //insert into MarerialWarehouseLog(WarehouseNumber,WarehouseName,ChangeDirection ,Type ,Creator,CreateTime ,Remark )
            //values('{0}','ycl','出库','生产出库','{1}','{2}','由生成工序开工单时产生')", warehouseNumber, userId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //            sqls.Add(sql);
            //            //生成原材料生产出库单明细
            //            sql = string.Format(@"
            //  insert into MaterialWarehouseLogDetail(WarehouseNumber ,DocumentNumber ,MaterialNumber ,RowNumber ,LeadTime ,CustomerMaterialNumber ,Qty )
            //  select '{0}','{1}', c.MaterialNumber,'0','0',c.CustomerMaterialNumber,SUM (c.数量) as 数量 from (
            //  select  b.MaterialNumber ,b.数量,mcp.CustomerMaterialNumber from (
            //  select  bom.MaterialNumber,so.CustomerId,a.NeedToProduceQty *bom.SingleDose as 数量 from (
            //  select  OrdersNumber,ProductNumber ,Version ,CustomerProductNumber,RowNumber ,NeedToProduceQty ,LeadTime from T_Process_Temp
            //  where UserId='{2}'
            //  group by OrdersNumber, ProductNumber ,Version ,CustomerProductNumber,RowNumber ,NeedToProduceQty ,LeadTime) a
            //  inner join BOMInfo bom on a.ProductNumber=bom.ProductNumber and a.Version=bom.Version
            //  inner join SaleOder so on so.OdersNumber =a.OrdersNumber ) b  inner join
            //  MaterialCustomerProperty mcp on b.CustomerId =mcp.CustomerId  and b.MaterialNumber=mcp.MaterialNumber) c group by c.MaterialNumber,c.CustomerMaterialNumber", warehouseNumber, kgNumber, userId);
            //            sqls.Add(sql);
            sqls.AddRange(lsSqls);
            //各种工时更新
            sql = string.Format(@"
 exec p_UpdatePlanManhour '{0}' ", kgNumber);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }

        /// <summary>
        /// 生成小组开工单
        /// </summary>
        /// <returns></returns>
        public static string GenerateXZ(string userId)
        {
            string sql = string.Format(@"select COUNT (*) from T_XZWorkOrderTemp where UserId ='{0}' and ISNULL ( ZZTeam ,'')!=''", userId);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                return "请设置制造组开工";
            }
            sql = string.Format(@"select COUNT (*) from T_XZWorkOrderTemp where UserId ='{0}' and ISNULL ( JYTeam ,'')!=''", userId);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                return "请设置检验组检验";
            }
            sql = string.Format(@"select COUNT (*) from T_XZWorkOrderTemp twot inner join SaleOder so on twot.OrdersNumber =so.OdersNumber
where twot .UserId ='{0}' and so.OdersType='包装生产订单'", userId);
            string productType = ""; //产品类型
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                productType = "电缆";
            }
            else
            {
                productType = "包装";
            }

            string kgNumber = "KG" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string summaryTableSql = string.Empty; ;//总表sql
            string summaryTableDetailSql = string.Empty;//总表明细sql
            string tempSummaryTableDetailSql = string.Empty;//临时sql
            string pointsTableSql = string.Empty;//分表sql
            string tempPointsTableSql = string.Empty;//分表
            string tempPointsTableDetailSql = string.Empty;//分表明细临时sql
            string pointsTableDetailSql = string.Empty;//分表明细sql

            string edGS = string.Empty; //额定工时
            string persionQty = string.Empty; //总人数
            string targetGS = string.Empty;//目标完成工时
            List<string> sqls = new List<string>();
            string error = string.Empty;
            sql = @"  update T_XZWorkOrderTemp set NeedToProduceQty=twt.Qty  from T_XZWorkOrderTemp two inner join
 T_WorkOrder_Temp twt on twt.Id =two.Id and two.UserId =twt.User_id";
            SqlHelper.ExecuteSql(sql, ref error);

            tempPointsTableDetailSql = string.Format(@"select '{1}' as 开工单号,txot.ZZTeam as 班组,txot .OrdersNumber ,txot.ProductNumber ,txot.Version ,txot.RowNumber,txot.CustomerProductNumber,
txot.NeedToProduceQty ,CAST( p.制造组总额定工时 as decimal(18,2))/60 as 单套工时,txot.NeedToProduceQty*(CAST( p.制造组总额定工时 as decimal(18,2))/60) as 合计工时,txot.LeadTime  from T_XZWorkOrderTemp txot
left join V_Product_Manhour_ZZ p on txot .ProductNumber =p.ProductNumber and txot.Version =p.Version
where txot.UserId='{0}' and ISNULL ( txot.ZZTeam,'')!=''
union all
select '{1}',txot.JYTeam,txot .OrdersNumber ,txot.ProductNumber ,txot.Version ,txot.RowNumber,txot.CustomerProductNumber,
txot.NeedToProduceQty ,CAST( p.检验组总额定工时 as decimal(18,2))/60,txot.NeedToProduceQty*(CAST( p.检验组总额定工时 as decimal(18,2))/60) ,txot.LeadTime from T_XZWorkOrderTemp txot
left join V_Product_Manhour_JY p on txot .ProductNumber =p.ProductNumber
and txot.Version =p.Version
where txot.UserId='{0}' and ISNULL ( txot.JYTeam,'')!=''", userId, kgNumber);
            //插入分表明细
            pointsTableDetailSql = string.Format(@"insert into ProductPlanSubDetail(PlanNumber ,Team ,OrdersNumber ,ProductNumber ,Version
,RowNumber ,CustomerProductNumber,Qty ,RatedManhour ,TotalManhour ,LeadTime ) {0}
", tempPointsTableDetailSql);

            tempPointsTableSql = string.Format(@"select '{1}' as 开工单号,a.班组,  CAST ( ROUND ( a.额定总工时,2) as decimal(18,2)) as 额定总工时,
b.人数,CAST( round((cast(a.额定总工时 as decimal(18,2) )/b.人数)/1 ,2) as decimal(18,2))as 目标完成工时 from (
select t.班组,    SUM ( t.合计工时) as    额定总工时 from ({0}) t group by t.班组) a
left join ( select Team ,COUNT (*) as 人数 from PM_USER  group by Team) b on a.班组=b.Team  ", tempPointsTableDetailSql, kgNumber);
            //插入分表
            pointsTableSql = string.Format(@"insert into ProductPlanSub (PlanNumber ,Team ,RatedTotalManhour,PersonQty ,TargetFinishManhour)
 {0}", tempPointsTableSql);

            tempSummaryTableDetailSql = string.Format(@"select '{1}' as 开工单号,toxt .OrdersNumber ,toxt .ProductNumber ,toxt .Version ,toxt .RowNumber,
toxt .CustomerProductNumber,
toxt .NeedToProduceQty ,CAST(p.RatedManhour as decimal(18,2)) as 单套工时  ,toxt .NeedToProduceQty*CAST(p.RatedManhour as decimal(18,2)) as 合计工时,toxt.LeadTime  from T_XZWorkOrderTemp  toxt left join Product p
on toxt .ProductNumber =p.ProductNumber and toxt .Version =p.Version
where  toxt .UserId='{0}' and ISNULL (  toxt .ZZTeam,'')!=''", userId, kgNumber);
            //总表明细
            summaryTableDetailSql = string.Format(@"insert into ProductPlanDetail(PlanNumber ,OrdersNumber ,ProductNumber ,Version
,RowNumber,CustomerProductNumber,Qty ,RatedManhour ,TotalManhour ,LeadTime ) {0}
", tempSummaryTableDetailSql);

            //计算开工单总表额定总工时【从总表明细统计】
            sql = string.Format(@"select CAST ( ROUND ( CAST( SUM (isnull( t.合计工时,0)) as decimal(18,2) )/60,2) as decimal(18,2)) from ({0}) t", tempSummaryTableDetailSql);
            edGS = SqlHelper.GetScalar(sql);//开工单总表额定总工时
                                            //计算总开工人数【从分表统计】
            sql = string.Format(@"select isnull( SUM (t.人数) ,0)from (
select a.班组,a.额定总工时,b.人数,CAST( round((cast(a.额定总工时 as decimal(18,2) )/b.人数)/1 ,2) as decimal(18,2))as 目标完成工时 from (
select t.班组,SUM (  t.额定总工时)/60 as 额定总工时 from ({0}) t group by t.班组) a
left join ( select Team ,COUNT (*) as 人数 from PM_USER  group by Team) b on a.班组=b.Team )t", tempPointsTableSql);
            persionQty = SqlHelper.GetScalar(sql);
            //目标完成工时=额定总工时/人数
            targetGS = (Convert.ToDouble(edGS) / Convert.ToInt32(persionQty) / 1).ToString();

            summaryTableSql = string.Format(@"insert into ProductPlan (PlanNumber ,Type ,CreateTime ,Creator ,PersonQty ,RatedTotalManhour,TargetFinishManhour)
values('{0}','小组','{1}','{2}', {3},CAST( ROUND ( {4},2) as decimal(18,2)),CAST( ROUND ( {5},2) as decimal(18,2)))",
        kgNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), userId, persionQty, edGS, targetGS);
            //生成开工单 （先插入总表信息》插入总表明细》插入分表》插入分表明细）
            sqls.Add(summaryTableSql);
            sqls.Add(summaryTableDetailSql);
            sqls.Add(pointsTableSql);
            sqls.Add(pointsTableDetailSql);

            //生成裁线信息表
            //            sql = string.Format(@"insert into CuttingLineInfo(PlanNumber,ProductNumber,Version,MaterialNumber,Length,CustomerProductNumber,
            //CustomerMaterialNumber,Qty,RatedManhour,Remark)
            // select '{0}',b.ProductNumber ,b.Version ,b.MaterialNumber ,b.Length ,
            // b.CustomerProductNumber,mcp.CustomerMaterialNumber,b.Qty,pws.RatedManhour,'生成小组开工单时产生'  from (
            // --从裁线信息表找出相关信息
            // select pcli.*,a.CustomerId,a.CustomerProductNumber  from (
            // --按客户、产品分组
            // select  so.CustomerId ,tx.ProductNumber ,tx.Version ,tx.CustomerProductNumber from T_XZWorkOrderTemp tx
            // inner join SaleOder so on so.OdersNumber =tx.OrdersNumber
            // where tx.UserId ='{1}' and ISNULL ( tx.JYTeam,'')!=''
            // group by  so.CustomerId , tx. ProductNumber , tx.Version , tx.CustomerProductNumber) a
            // inner join ProductCuttingLineInfo pcli on pcli.ProductNumber =a.ProductNumber and pcli .Version =a.Version ) b
            // inner join ProductWorkSnProperty pws on pws.ProductNumber =b.ProductNumber and pws.Version =b.Version
            // left join MaterialCustomerProperty mcp on mcp.CustomerId =b.CustomerId  and mcp.MaterialNumber=b.MaterialNumber
            // where pws.WorkSnNumber='cx'
            // --最后找出裁线的单套工时，客户物料编号", kgNumber, userId);
            //不是包的产生裁线信息
            sql = string.Format(@" insert into CuttingLineInfo(PlanNumber,ProductNumber,Version,MaterialNumber,Length,
Qty,CustomerId)
 select '{0}',bom.ProductNumber ,bom.Version,bom.MaterialNumber ,bom.SingleDose  ,a.needQty,a.CustomerId  from (
 --按客户、产品分组
 select  tx.ProductNumber ,tx.Version ,SUM( tx.NeedToProduceQty ) as needQty,so.CustomerId
 from T_XZWorkOrderTemp tx
 inner join SaleOder so  on so.OdersNumber=tx.OrdersNumber
 inner join Product p on tx.ProductNumber =p.ProductNumber and tx.Version =p.Version
 where tx.UserId ='{1}' and ISNULL ( tx.JYTeam,'')!='' and p.Type !='包'
 group by  tx. ProductNumber , tx.Version ,so.CustomerId ) a
 inner join
( select bom.*,mit.Kind  from BOMInfo  bom inner join  MarerialInfoTable mit on bom.MaterialNumber =mit.MaterialNumber)
 bom
 on bom.ProductNumber =a.ProductNumber and bom.Version =a.Version
 where     bom.Kind ='线材' and bom.unit='mm' ", kgNumber, userId);
            sqls.Add(sql);
            //是包的产生裁线信息
            sql = string.Format(@" insert into CuttingLineInfo(PlanNumber,ProductNumber,Version,MaterialNumber,Length,
Qty,CustomerId)
 select '{0}',bom.ProductNumber ,bom.Version,bom.MaterialNumber ,bom.SingleDose  ,a.needQty,a.CustomerId  from (
 --按客户、产品分组
 select  tx.ProductNumber ,tx.Version ,SUM( tx.NeedToProduceQty ) as needQty,so.CustomerId
 from T_XZWorkOrderTemp tx
 inner join SaleOder so  on so.OdersNumber=tx.OrdersNumber
 inner join Product p on tx.ProductNumber =p.ProductNumber and tx.Version =p.Version
 where tx.UserId ='{1}' and ISNULL ( tx.JYTeam,'')!='' and p.Type ='包'
 group by  tx. ProductNumber , tx.Version ,so.CustomerId ) a
 inner join
( select bom.*,mit.Kind  from V_Pack_BomInfo  bom inner join  MarerialInfoTable mit on bom.MaterialNumber =mit.MaterialNumber)
 bom
 on bom.PackageNumber  =a.ProductNumber
 where     bom.Kind ='线材' and bom.unit='mm' ", kgNumber, userId);
            sqls.Add(sql);
            //string warehouseNumber = "YCLCK" + DateTime.Now.ToString("yyyyMMddHHmmss");
            //            //生成原材料生产出库单
            //            sql = string.Format(@"
            //insert into MarerialWarehouseLog(WarehouseNumber,WarehouseName,ChangeDirection ,Type ,Creator,CreateTime ,Remark )
            //values('{0}','ycl','出库','生产出库','{1}','{2}','由生成小组 开工单时产生')", warehouseNumber, userId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //            sqls.Add(sql);
            //            //生成原材料生产出库单明细
            //            sql = string.Format(@"  insert into MaterialWarehouseLogDetail(WarehouseNumber ,DocumentNumber ,MaterialNumber ,ProductNumber ,CustomerMaterialNumber,Qty,RowNumber,LeadTime)
            //  select '{1}','{2}',bom.MaterialNumber,a.ProductNumber ,bom.CustomerMaterialNumber,a.总产品数量*bom.SingleDose,'0','0' from  (
            // select  tx.ProductNumber ,tx.Version ,tx.CustomerProductNumber,SUM (tx.NeedToProduceQty ) as 总产品数量 from T_XZWorkOrderTemp tx
            // inner join SaleOder so on so.OdersNumber =tx.OrdersNumber
            // where tx.UserId ='{0}' and ISNULL ( tx.JYTeam,'')!=''
            // group by   tx. ProductNumber , tx.Version , tx.CustomerProductNumber,tx.NeedToProduceQty ) a inner join BOMInfo bom
            // on bom.ProductNumber =a.ProductNumber and bom .Version =a.Version", userId, warehouseNumber, kgNumber);
            //            sqls.Add(sql);
            sql = string.Format(@" update ProductPlanSubDetail set TotalManhour =Qty*RatedManhour
 where PlanNumber ='{0}' ", kgNumber);
            sqls.Add(sql);
            //更新产品类型
            sql = string.Format(@" update ProductPlanDetail set productType='{0}' where PlanNumber ='{1}'", productType, kgNumber);
            sqls.Add(sql);

            //各种工时更新
            sql = string.Format(@"
 exec p_UpdatePlanManhour '{0}' ", kgNumber);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
        }

        /// <summary>
        /// 获取生成产成品入库单和半成品入库单的sql语句
        /// </summary>
        /// <param name="planDetail">实体</param>
        /// <param name="userId">当前登录用户</param>
        /// <returns>sql语句集合List</returns>
        public static List<string> GetGenerateProductWarehouseLogSql(ProductPlanSubDetail planDetail, string userId)
        {
            string sql = string.Empty;
            string warehouseNumber = string.Empty;
            string dateTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            List<string> sqls = new List<string>();
            //如果是半成品就入半成品库
            if (planDetail.IsHalfProduct)
            {
                warehouseNumber = string.Format("BCPRK{0}", DateTime.Now.ToString("yyyyMMdd"));
                sql = string.Format(@"select count(*) from HalfProductWarehouseLog where WarehouseNumber='{0}'", warehouseNumber);
                if (SqlHelper.GetScalar(sql).Equals("0"))//如果没有这个单号
                {
                    sql = string.Format(@"
insert into HalfProductWarehouseLog(WarehouseNumber,WarehouseName,ChangeDirection,Creator,CreateTime ,Remark )
values('{0}','{1}','{2}','{3}','{4}','{5}')", warehouseNumber, "bcpk", "入库", userId, dateTimeNow, "");
                    sqls.Add(sql);
                }
                else
                {
                    sql = string.Format(" select CheckTime  from HalfProductWarehouseLog where WarehouseNumber='{0}'  ", warehouseNumber);
                    if (!string.IsNullOrEmpty(SqlHelper.GetScalar(sql)))//如果这个单子已经审核过了
                    {
                        warehouseNumber = GetNumberForHalfProductWarehouseLog(warehouseNumber);
                        sql = string.Format(@"
insert into HalfProductWarehouseLog(WarehouseNumber,WarehouseName,ChangeDirection,Creator,CreateTime ,Remark )
values('{0}','{1}','{2}','{3}','{4}','{5}')", warehouseNumber, "bcpk", "入库", userId, dateTimeNow, "");
                        sqls.Add(sql);
                    }
                }
                sql = string.Format(@"select count(*) from HalfProductWarehouseLogDetail  where WarehouseNumber='{0}'
and DocumentNumber='{1}' and ProductNumber='{2}' and Version='{3}' and  MaterialNumber='{4}' and RowNumber ='{5}'",
        warehouseNumber, planDetail.PlanNumber,
        planDetail.ProductNumber, planDetail.Version, planDetail.QLMareialNumbers, planDetail.RowNumber);
                string num = SqlHelper.GetScalar(sql);
                if (num == "0")
                {
                    sql = string.Format(@"insert into HalfProductWarehouseLogDetail (WarehouseNumber ,DocumentNumber ,ProductNumber ,Version ,
MaterialNumber ,SailOrderNumber ,LeadTime ,Qty,Remark ,RowNumber )
select '{0}',PlanNumber ,ProductNumber ,Version ,'{1}',OrdersNumber ,LeadTime ,{2},'{3}',RowNumber from ProductPlanSubDetail
where PlanNumber='{4}' and Team ='{5}' and OrdersNumber ='{6}' and ProductNumber ='{7}' and Version ='{8}'
 and RowNumber ='{9}'", warehouseNumber, planDetail.QLMareialNumbers, planDetail.Qty, "", planDetail.PlanNumber,
                          planDetail.Team, planDetail.OrdersNumber
                          , planDetail.ProductNumber, planDetail.Version, planDetail.RowNumber);
                    sqls.Add(sql);
                }
                else
                {
                    sql = string.Format(@"update HalfProductWarehouseLogDetail set Qty={0} where WarehouseNumber='{1}'
and DocumentNumber='{2}' and ProductNumber='{3}' and Version='{4}' and  MaterialNumber='{5}' and RowNumber='{6}'",
        planDetail.Qty, warehouseNumber, planDetail.PlanNumber,
        planDetail.ProductNumber, planDetail.Version, planDetail.QLMareialNumbers, planDetail.RowNumber);
                    sqls.Add(sql);
                }
            }
            else //否则入成品库
            {
                /////////////////////////////出入库主表生成//////////////////////////////////

                //判定出入库类型
                sql = string.Format(@"select COUNT(*) from ProductPlanSub where PlanNumber ='{0}'
and ltrim(rtrim(Team)) in ('包装','包装组')", planDetail.PlanNumber);
                string rkType = SqlHelper.GetScalar(sql).Equals("0") ? "生产入库" : "包装入库";

                //warehouseNumber = string.Format("CCPRK{0}", DateTime.Now.ToString("yyyyMMdd"));
                sql = string.Format(@"
select WarehouseNumber from ProductWarehouseLog where WarehouseNumber like '%{0}%'
and ISNULL(CheckTime,'')='' and
Type='{1}'", DateTime.Now.ToString("yyyyMMdd"), rkType);
                warehouseNumber = SqlHelper.GetScalar(sql);
                if (string.IsNullOrEmpty(warehouseNumber))
                {
                    warehouseNumber = GetNumberForProductWarehouseLog();
                    sql = string.Format(@"insert into ProductWarehouseLog (WarehouseNumber,WarehouseName ,
                                ChangeDirection ,Type ,Creator,CreateTime ,Remark)
                                values('{0}','cpk','入库','{4}','{1}','{2}','{3}')", warehouseNumber, userId, dateTimeNow, "", rkType);
                    sqls.Add(sql);
                }

                //                sql = string.Format(@"
                //             IF NOT EXISTS(SELECT * FROM ProductWarehouseLog WHERE WarehouseNumber = '{0}' and type='{4}' )
                //             begin
                //             insert into ProductWarehouseLog (WarehouseNumber,WarehouseName ,
                //             ChangeDirection ,Type ,Creator,CreateTime ,Remark)
                //             values('{0}','cpk','入库','{4}','{1}','{2}','{3}')
                //             end
                //            ", warehouseNumber, userId, dateTimeNow, "", rkType);
                //                //                        sqls.Add(sql);

                //                //首先看它在不在
                //                sql = string.Format(@"
                //                select COUNT (*) from ProductWarehouseLog where WarehouseNumber='{0}'  ", warehouseNumber);
                //                if (SqlHelper.GetScalar(sql).Equals("0")) //如果不存在则直接创建
                //                {
                //                    sql = string.Format(@"insert into ProductWarehouseLog (WarehouseNumber,WarehouseName ,
                //                ChangeDirection ,Type ,Creator,CreateTime ,Remark)
                //                values('{0}','cpk','入库','{4}','{1}','{2}','{3}')", warehouseNumber, userId, dateTimeNow, "", rkType);
                //                    sqls.Add(sql);
                //                }
                //                else  //存在则再进行分析
                //                {
                //                    sql = string.Format(" select CheckTime  from ProductWarehouseLog where WarehouseNumber='{0}'  ", warehouseNumber);
                //                    if (!string.IsNullOrEmpty(SqlHelper.GetScalar(sql)))//已经审核了
                //                    {
                //                        warehouseNumber = GetNumberForProductWarehouseLog();
                //                        sql = string.Format(@"insert into ProductWarehouseLog (WarehouseNumber,WarehouseName ,
                //                ChangeDirection ,Type ,Creator,CreateTime ,Remark)
                //                values('{0}','cpk','入库','{4}','{1}','{2}','{3}')", warehouseNumber, userId, dateTimeNow, "", rkType);
                //                        sqls.Add(sql);
                //                    }
                //                    else //还没审核
                //                    {
                //                        sql = string.Format(@"
                //                select Type from ProductWarehouseLog where WarehouseNumber='{0}'  ", warehouseNumber);
                //                        if (!SqlHelper.GetScalar(sql).Equals(rkType))//如果类型不一致
                //                        {
                //                            warehouseNumber = GetNumberForProductWarehouseLog();
                //                            sql = string.Format(@"insert into ProductWarehouseLog (WarehouseNumber,WarehouseName ,
                //                ChangeDirection ,Type ,Creator,CreateTime ,Remark)
                //                values('{0}','cpk','入库','{4}','{1}','{2}','{3}')", warehouseNumber, userId, dateTimeNow, "", rkType);
                //                            sqls.Add(sql);
                //                        }
                //                    }
                //                }

                /////////////////////////////出入库明细生成//////////////////////////////////
                sql = string.Format(@"select count(*) from ProductWarehouseLogDetail  where WarehouseNumber='{0}'
and DocumentNumber='{1}' and ProductNumber='{2}' and Version='{3}' and RowNumber='{4}' and OrdersNumber='{5}'",
        warehouseNumber, planDetail.PlanNumber,
        planDetail.ProductNumber, planDetail.Version, planDetail.RowNumber, planDetail.OrdersNumber);
                string num = SqlHelper.GetScalar(sql);
                if (num == "0")
                {
                    sql = string.Format(@"insert into ProductWarehouseLogDetail(WarehouseNumber ,DocumentNumber ,ProductNumber ,Version,RowNumber
                ,OrdersNumber ,CustomerProductNumber,LeadTime ,Qty )
                select '{0}',PlanNumber ,ProductNumber ,Version ,RowNumber ,OrdersNumber ,CustomerProductNumber,LeadTime
                ,{1} from ProductPlanSubDetail  where PlanNumber ='{2}' and Team ='{3}' and OrdersNumber ='{4}'
and ProductNumber ='{5}'
                and Version='{6}' and RowNumber ='{7}' ", warehouseNumber, planDetail.Qty, planDetail.PlanNumber, planDetail.Team,
        planDetail.OrdersNumber, planDetail.ProductNumber, planDetail.Version, planDetail.RowNumber);
                    sqls.Add(sql);
                }
                else
                {
                    sql = string.Format(@"update ProductWarehouseLogDetail set Qty=Qty+{0} where WarehouseNumber='{1}'
and DocumentNumber='{2}' and ProductNumber='{3}' and Version='{4}' and RowNumber='{5}' and OrdersNumber='{6}'",
        planDetail.Qty, warehouseNumber, planDetail.PlanNumber,
        planDetail.ProductNumber, planDetail.Version, planDetail.RowNumber, planDetail.OrdersNumber);
                    sqls.Add(sql);
                }
            }
            return sqls;

            //                    //看该出入库类型存不存在
            //                    sql = string.Format(@"
            //select COUNT (*) from ProductWarehouseLog where WarehouseNumber='{0}' and Type='{1}'", warehouseNumber, rkType);
            //                    if (SqlHelper.GetScalar(sql).Equals("0"))
            //                    {
            //                    }
            //                    else
            //                    {
            //                    }
            //                sql = string.Format(@"select COUNT(*) from ProductPlanSub where PlanNumber ='{0}'
            //and Team ='包装组'", planDetail.PlanNumber);
            //                string rkType = SqlHelper.GetScalar(sql).Equals("0") ? "生产入库" : "包装入库";

            //                sql = string.Format(@"
            //select COUNT (*) from ProductWarehouseLog where WarehouseNumber='{0}' and Type='{1}'", warehouseNumber, rkType);

            //                if (SqlHelper.GetScalar(sql).Equals("0"))
            //                {
            //                    sql = string.Format(@"insert into ProductWarehouseLog (WarehouseNumber,WarehouseName ,
            //ChangeDirection ,Type ,Creator,CreateTime ,Remark)
            //values('{0}','cpk','入库','{4}','{1}','{2}','{3}')", warehouseNumber, userId, dateTimeNow, "", rkType);
            //                    sqls.Add(sql);
            //                }
            //                else
            //                {
            //                    sql = string.Format(" select CheckTime  from ProductWarehouseLog where WarehouseNumber='{0}'  ", warehouseNumber);
            //                    if (!string.IsNullOrEmpty(SqlHelper.GetScalar(sql)))//如果这个单子已经审核过了
            //                    {
            //                        warehouseNumber = GetNumberForProductWarehouseLog(warehouseNumber);

            //                        sql = string.Format(@"
            // IF NOT EXISTS(SELECT * FROM ProductWarehouseLog WHERE WarehouseNumber = '{0}'  )
            //begin
            //insert into ProductWarehouseLog (WarehouseNumber,WarehouseName ,
            //ChangeDirection ,Type ,Creator,CreateTime ,Remark)
            //values('{0}','cpk','入库','{4}','{1}','{2}','{3}')
            //end
            //", warehouseNumber, userId, dateTimeNow, "", rkType);
            //                        sqls.Add(sql);
            //                    }
            //                }
        }

        public static Dictionary<string, int> GetNoAddQty()
        {
            Dictionary<string, int> noAddQty = new Dictionary<string, int>();
            string sql = @"
                select ProductNumber, Version, SUM(qty) as qty from ProductWarehouseLogDetail where WarehouseNumber in (
 select WarehouseNumber from ProductWarehouseLog where ChangeDirection = '入库' and ISNULL(CheckTime, '') = ''
 ) group by ProductNumber,Version
                ";
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                noAddQty.Add(dr["ProductNumber"] + "|" + dr["Version"], Convert.ToInt32(dr["qty"]));
            }
            return noAddQty;
        }

        public static Dictionary<string, int> GetNoConfirmQty()
        {
            Dictionary<string, int> qty = new Dictionary<string, int>();
            string sql = @"
select  ProductNumber,Version, sum(isnull(DeliveryQty,0)) qty from DeliveryNoteDetailed
where isnull(Version,'')!='' and DeliveryNumber in
(select distinct DeliveryNumber from DeliveryBill where IsConfirm!='已确认')
group by ProductNumber,Version
                ";
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                qty.Add(dr["ProductNumber"] + "|" + dr["Version"], Convert.ToInt32(dr["qty"]));
            }
            return qty;
        }

        /// <summary>
        /// 获得未完成的加工销售订单的明细 sql
        /// </summary>
        public static string GetOrderNofinesfinishedDetail()
        {
            StringBuilder longSql = new StringBuilder();
            DataTable dt = GetWorkOrderTable();
            foreach (DataRow dr in dt.Rows)
            {
                longSql.AppendFormat(@"union all
select '{0}' as 销售订单号, '{1}' as 产品编号, '{2}' as 版本,{3} as 订单数量,{4} as 已交货数量,
{5} as 未交货数量,{6} as 库存数量,{7} as 在制品数量, {8} as 需要生产数量,
'{9}' as 交期,'{10}' as 行号,'{11}' as 客户产品编号
                    ", dr["OdersNumber"], dr["ProductNumber"], dr["Version"]
                    , dr["Qty"], dr["DeliveryQty"], dr["NonDeliveryQty"], dr["StockQty"], dr["ProductingQty"]
                    , dr["NeedProductQty"], dr["LeadTime"], dr["RowNumber"], dr["CustomerProductNumber"]);
            }
            return longSql.ToString().TrimStart(new char[] { 'u', 'n', 'i', 'o', 'n', ' ', 'a', 'l', 'l' });
        }

        public static Dictionary<string, int> GetProductingQty()
        {
            Dictionary<string, int> productingQty = new Dictionary<string, int>();
            //            string sql = @"
            //select  ProductNumber,Version ,
            //case when
            //sum(Qty)-SUM(StorageQty)<0 then 0
            //else sum(Qty)-SUM(StorageQty) end as productQty
            // from  ProductPlanDetail
            // group by ProductNumber,Version
            //";
            string sql = @"
select  ProductNumber,Version ,
case when
sum(Qty)-SUM(ISNULL(FinishQty,0))<0 then 0
else sum(Qty)-SUM(ISNULL(FinishQty,0)) end as productQty
 from  ProductPlanSubDetail  where Team='检验'
 group by ProductNumber,Version
";
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                productingQty.Add(dr["ProductNumber"] + "|" + dr["Version"], Convert.ToInt32(dr["productQty"]));
            }
            return productingQty;
        }

        /// <summary>
        /// 查询产品缺料明细
        /// </summary>
        /// <param name="productNumber"></param>
        /// <param name="version"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public static DataTable GetQuLiaoDetail(string productNumber, string version, string qty, string customerProductNumber)
        {
            //            string sql = string.Format(@"select distinct   a.ProductNumber as 产成品编号 ,a.Version as 版本,a.MaterialNumber as 原材料编号 ,(b.库存数量-a.需要生产数量) as 缺料数量  from
            //(select ProductNumber ,Version , MaterialNumber ,SingleDose*{2} as 需要生产数量 from BOMInfo)a
            //left join (
            //select ProductNumber ,Version ,bom.MaterialNumber ,ISNULL (vmq.qty ,0) as 库存数量 from BOMInfo bom left join
            //V_MaterialStock_Qty vmq on  bom.MaterialNumber =vmq.MaterialNumber ) b on a.ProductNumber=b.ProductNumber
            //and a.Version =b.Version and a.MaterialNumber =b.MaterialNumber
            // and a.ProductNumber ='{0}' and a.Version ='{1}'", productNumber, version, qty);

            //            sql = string.Format(@"select ''as 包号, '{2}' as 客户产成品编号,
            //bom.ProductNumber as 产成品编号 ,bom.Version 版本,1 as 产品单机,bom.MaterialNumber 原材料编号,
            //bom.CustomerMaterialNumber 客户物料号,
            //SingleDose as 物料单机用量,Unit as 单位,{1} as 实际生产数量,SingleDose*{1} as 总实际生产数量,h.缺料数量 from BOMInfo bom
            //left join ({0})h on h.原材料编号=bom.MaterialNumber ", sql, qty, customerProductNumber);
            string sql = string.Format(@"select
'' as 客户包号,
'' as  包号,
a.CustomnerProductNumber as 客户产成品编号,
a.ProductNumber as 产成品编号,
a.Version as 版本,
1 as 产品单机,
a.CustomerMaterialNumber as 客户物料号,
a.MaterialNumber as 原材料编号,
a.物料单机用量 ,
a.SingleDose as 总物料单机用量,
a.Unit as 单位,
{0} as 实际生产数量,
{0}*a.SingleDose as 总实际生产数量,
vmq.qty as 库存数量,
(vmq.qty -a.SingleDose*{0}) as 缺料数量  from (
select CustomnerProductNumber,ProductNumber,Version ,CustomerMaterialNumber,MaterialNumber,
SUM (SingleDose ) SingleDose  ,SingleDose as 物料单机用量,Unit  from BOMInfo
where ProductNumber ='{1}' and Version ='{2}'
group by CustomnerProductNumber , ProductNumber ,Version, CustomerMaterialNumber ,MaterialNumber,SingleDose,unit
) a left join  V_MaterialStock_Qty vmq on a.MaterialNumber =vmq.MaterialNumber", qty, productNumber, version);
            return SqlHelper.GetTable(sql);
        }

        /// <summary>
        /// 查询包缺料明细
        /// </summary>
        /// <param name="productNumber"></param>
        /// <param name="qty"></param>
        /// <param name="customerProductNumber"></param>
        /// <returns></returns>
        public static DataTable GetQuLiaoDetailForBao(string productNumber, string qty, string customerProductNumber)
        {
            //            string sql = string.Format(@"  select  a.PackageNumber as 包号,'{1}' as 客户产成品编号
            //,a.ProductNumber as 产成品编号,a.Version as 版本,a.产品单机,bom.CustomerMaterialNumber as 原材料编号
            // ,bom.CustomerMaterialNumber as 客户物料号,bom.SingleDose as 物料单机用量,bom .Unit as 单位,{2} as 实际生产数量,
            // bom.SingleDose*a.产品单机*{2} as 总实际生产数量, vsq.qty-(bom.SingleDose*a.产品单机*{2} ) as 缺料数量   from BOMInfo bom
            // inner join (select PackageNumber,ProductNumber ,Version ,SingleDose as 产品单机
            //   from  PackageAndProductRelation where PackageNumber='{0}') a
            //   on a.ProductNumber =bom .ProductNumber
            //   and a.Version =bom.Version
            //   left join V_MaterialStock_Qty vsq on vsq.MaterialNumber =bom.MaterialNumber", productNumber, customerProductNumber, qty);
            //            return SqlHelper.GetTable(sql);
            string sql = string.Format(@"
select
'{2}' as 客户包号,
a.PackageNumber as 包号,
bom.CustomnerProductNumber as 客户产成品编号,
a.ProductNumber as 产成品编号,
a.Version as 版本,
a.产品单机,
bom.CustomerMaterialNumber as 原材料编号,
bom.CustomerMaterialNumber as 客户物料号,
bom.物料单机用量,
 bom.SingleDose as 总物料单机用量,
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
   left join V_MaterialStock_Qty vsq on vsq.MaterialNumber =bom.MaterialNumber", productNumber, qty, customerProductNumber);
            return SqlHelper.GetTable(sql);
        }

        /// <summary>
        /// 记录数量
        /// </summary>
        /// <param name="planDetail"></param>
        /// <returns></returns>
        public static string GetRecordSql(ProductPlanSubDetail model)
        {
            string sql = string.Format(@"
INSERT INTO ProductPlanSubDetail_Record
            (PlanNumber,
             Team,
             OrdersNumber,
             ProductNumber,
             [Version],
             RowNumber,
             CustomerProductNumber,
             Qty,
             RatedManhour,
             TotalManhour,
             WorkSn1,
             WorkSn2,
             WorkSn3,
             WorkSn4,
             NextTeam,
             FinishQty,
             TakeLine,
             LeadTime,
             Remark,
             UpdateTime)
SELECT PlanNumber,
       Team,
       OrdersNumber,
       ProductNumber,
       [Version],
       RowNumber,
       CustomerProductNumber,
       Qty,
       RatedManhour,
       TotalManhour,
       WorkSn1,
       WorkSn2,
       WorkSn3,
       WorkSn4,
       NextTeam,
       {0},
       TakeLine,
       LeadTime,
       Remark,
       UpdateTime
FROM   ProductPlanSubDetail
WHERE  PlanNumber = '{1}'
       AND Team = '{2}'
       AND OrdersNumber = '{3}'
       AND ProductNumber = '{4}'
       AND [Version] = '{5}'
       AND RowNumber = '{6}'
", model.Qty, model.PlanNumber, model.Team, model.OrdersNumber, model.ProductNumber, model.Version, model.RowNumber);
            return sql;
        }

        public static Dictionary<string, int> GetStockQty()
        {
            Dictionary<string, int> stockQty = new Dictionary<string, int>();
            string sql = @"
                select * from V_ProductStock_Sum
                ";
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                stockQty.Add(dr["ProductNumber"] + "|" + dr["Version"], Convert.ToInt32(dr["库存数量"]));
            }
            return stockQty;
        }

        public static Dictionary<string, int> GetSumQty()
        {
            Dictionary<string, int> sumQty = new Dictionary<string, int>();
            string sql = @"
with A as (
    select  ProductNumber,Version ,
    case when
    sum(Qty)-SUM(ISNULL(FinishQty,0))<0 then 0
    else sum(Qty)-SUM(ISNULL(FinishQty,0)) end as Qty
     from  ProductPlanSubDetail  where Team='检验'
     group by ProductNumber,Version
 ),
 B as (
	 select ProductNumber, Version, SUM(qty) as Qty from ProductWarehouseLogDetail where WarehouseNumber in (
	 select WarehouseNumber from ProductWarehouseLog where ChangeDirection = '入库' and ISNULL(CheckTime, '') = ''
	 ) group by ProductNumber,Version
 ),
 C as
 (
	select ProductNumber,Version,库存数量 as Qty from V_ProductStock_Sum
 )
 ,D as
 (
	 select  ProductNumber,Version, sum(isnull(DeliveryQty,0)) Qty from DeliveryNoteDetailed
	where isnull(Version,'')!='' and DeliveryNumber in
	(select distinct DeliveryNumber from DeliveryBill where IsConfirm!='已确认')
	group by ProductNumber,Version
)
 select
   p.ProductNumber,
   p.Version,
   isnull(A.Qty,0) as 在制品数量,
   isnull(B.Qty,0) as 未入库数量,
   isnull(C.Qty,0) as 库存数量 ,
   isnull(D.Qty,0) as 送货单未确认数量 ,
   isnull(A.Qty,0) +isnull(B.Qty,0)+isnull(C.Qty,0)+ isnull(D.Qty,0) as 总数量
 from   Product  p
 left join A on A.ProductNumber=p.ProductNumber and A.Version=p.Version
 left join B on B.ProductNumber=p.ProductNumber and B.Version=p.Version
 left join C on C.ProductNumber=p.ProductNumber and C.Version=p.Version
 left join D on D.ProductNumber=p.ProductNumber and D.Version=p.Version

";
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                sumQty.Add(dr["ProductNumber"] + "|" + dr["Version"], Convert.ToInt32(dr["总数量"]));
            }
            return sumQty;
        }

        public static string GetWorkOrderSql()
        {
            StringBuilder longSql = new StringBuilder();
            DataTable dt = GetWorkOrderTable();
            foreach (DataRow dr in dt.Rows)
            {
                longSql.AppendFormat(@"union all
select '{0}' as 销售订单号, '{1}' as 产品编号, '{2}' as 版本,{3} as 订单数量,{4} as 已交货数量,
{5} as 未交货数量,{6} as 库存数量,{7} as 在制品数量,{8} as 未入库数量,{9} as 送货单未确认数量,{10} as 需要生产数量,
'{11}' as 交期,'{12}' as 行号,'{13}' as 客户产品编号
                    ", dr["OdersNumber"], dr["ProductNumber"], dr["Version"]
                    , dr["Qty"], dr["DeliveryQty"], dr["NonDeliveryQty"], dr["StockQty"], dr["ProductingQty"], dr["NoAddQty"], dr["NoConfirmQty"]
                    , dr["NeedProductQty"], dr["LeadTime"], dr["RowNumber"], dr["CustomerProductNumber"]);
            }
            return longSql.ToString().TrimStart(new char[] { 'u', 'n', 'i', 'o', 'n', ' ', 'a', 'l', 'l' });
        }

        public static DataTable GetWorkOrderTable()
        {
            var productingQtys = GetProductingQty();
            var noAddQtys = GetNoAddQty();
            var stockQtys = GetStockQty();
            var noConfirmQtys = GetNoConfirmQty();
            var sumQtys = GetSumQty();

            string sql = @"
select *  from  V_MachineOderDetail_Product_Nofinesfinished_Detail  vpnd
where vpnd.NonDeliveryQty>0
order by vpnd.OdersNumber asc, vpnd.LeadTime  asc
";
            DataTable dtMain = SqlHelper.GetTable(sql);
            dtMain.Columns.Add(new DataColumn("ProductingQty", typeof(int)));
            dtMain.Columns.Add(new DataColumn("NoAddQty", typeof(int)));
            dtMain.Columns.Add(new DataColumn("StockQty", typeof(int)));
            dtMain.Columns.Add(new DataColumn("NeedProductQty", typeof(int)));
            dtMain.Columns.Add(new DataColumn("NoConfirmQty", typeof(int)));
            dtMain.Columns.Add(new DataColumn("SumQty", typeof(int)));

            string key = string.Empty;

            foreach (DataRow dr in dtMain.Rows)
            {
                key = dr["ProductNumber"] + "|" + dr["Version"];

                if (productingQtys.ContainsKey(key))
                {
                    dr["ProductingQty"] = productingQtys[key];
                }
                else
                {
                    dr["ProductingQty"] = 0;
                }
                if (noAddQtys.ContainsKey(key))
                {
                    dr["NoAddQty"] = noAddQtys[key];
                }
                else
                {
                    dr["NoAddQty"] = 0;
                }
                if (stockQtys.ContainsKey(key))
                {
                    dr["StockQty"] = stockQtys[key];
                }
                else
                {
                    dr["StockQty"] = 0;
                }
                if (noConfirmQtys.ContainsKey(key))
                {
                    dr["NoConfirmQty"] = noConfirmQtys[key];
                }
                else
                {
                    dr["NoConfirmQty"] = 0;
                }

                if (sumQtys.ContainsKey(key))
                {
                    dr["SumQty"] = sumQtys[key];
                }
                else
                {
                    dr["SumQty"] = 0;
                }

                //库存+在制+未入库+送货单未确认数量-未交数量=需要生产数量
                dr["NeedProductQty"] = Convert.ToInt32(dr["NonDeliveryQty"]) - Convert.ToInt32(dr["SumQty"]);

                int needProductQty = Convert.ToInt32(dr["NeedProductQty"]);

                if (needProductQty > 0) //需要生产
                {
                    sumQtys[key] = 0;
                }
                else //不需要生产
                {
                    sumQtys[key] = sumQtys[key] - Math.Abs(Convert.ToInt32(dr["NonDeliveryQty"]));
                }
            }

            return dtMain;
        }

        /// <summary>
        /// 在半成品入库已审核的情况下再生成新的半成品入库单
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <returns></returns>
        private static string GetNumberForHalfProductWarehouseLog(string warehouseNumber)
        {
            string result = "";
            string sql = string.Empty;
            for (int i = 1; i < 50; i++)
            {
                warehouseNumber = warehouseNumber + "_" + i.ToString();
                sql = string.Format(@"select count(*) from HalfProductWarehouseLog where WarehouseNumber='{0}' and ISNULL (CheckTime ,'') !='' ", warehouseNumber);
                if (SqlHelper.GetScalar(sql).Equals("0"))
                {
                    result = warehouseNumber;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 在产成品入库已审核的情况下再生成新的产成品入库单
        /// </summary>
        /// <param name="warehouseNumber"></param>
        /// <returns></returns>
        private static string GetNumberForProductWarehouseLog()
        {
            string warehouseNumber = string.Format("CCPRK{0}", DateTime.Now.ToString("yyyyMMdd"));

            string result = "";
            string sql = string.Empty;
            for (int i = 1; i < 50; i++)
            {
                result = warehouseNumber + "_" + i.ToString();
                sql = string.Format(@"select count(*) from ProductWarehouseLog where WarehouseNumber='{0}'  ", result);
                if (SqlHelper.GetScalar(sql).Equals("0"))
                {
                    // result = warehouseNumber;
                    break;
                }
            }
            return result;
        }
    }
}