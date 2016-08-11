using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.ProduceManager
{
    public partial class WorkOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WorkOrderManager.RepairData();
                //设置工序开工班组
                if (ToolManager.CheckParams("GXPRdouctId"))
                {
                    string ids = ToolManager.GetParamsString("GXPRdouctId");
                    string team = ToolManager.GetParamsString("Team");
                    SetGXTeam(ids, team);
                }
                //显示工序信息
                if (ToolManager.CheckParams("IsLoadGX"))
                {
                    SetGX();
                    Response.Write(LoadGX());
                    Response.End();
                    return;
                }
                //生成工序开工单
                if (ToolManager.CheckQueryString("IsGenerateGX"))
                {
                    string userId = ToolCode.Tool.GetUser().UserNumber;
                    WorkOrderManager.RepairData();
                    var result = WorkOrderManager.GenerateGX(userId);
                    WorkOrderManager.RepairData();

                    Response.Write(result);
                    Response.End();
                    return;
                }
                //生成小组开工单
                if (ToolManager.CheckQueryString("IsGenerateXZ"))
                {
                    string userId = ToolCode.Tool.GetUser().UserNumber;
                    WorkOrderManager.RepairData();
                    var result = WorkOrderManager.GenerateXZ(userId);
                    WorkOrderManager.RepairData();
                    Response.Write(result);
                    Response.End();
                    return;
                }
                //更新小组开工单班组【制造】
                if (ToolManager.CheckParams("SetType"))
                {
                    string setType = ToolManager.GetParamsString("SetType");
                    UpdateTem(setType);
                }
                //检验
                if (ToolManager.CheckParams("IsJY"))
                {
                    LoadXZ(false, ToolManager.GetParamsString("ProductId"));
                }
                //加载小组临时数据[制造]
                if (ToolManager.CheckParams("ProductId"))
                {
                    LoadXZ(true, ToolManager.GetParamsString("ProductId"));
                }
                //第四步【小组开工单，不拆分】
                if (ToolManager.CheckParams("Four"))
                {
                    Four();
                }
                //往工序临时表插入数据
                if (ToolManager.CheckQueryString("GetGX"))
                {
                    SetGX();
                }

                //加载未完成订单明细
                LoadPage();
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

        //第四步
        private void Four()
        {
            string team = ToolManager.GetParamsString("Team");//班组
            string products = ToolManager.GetParamsString("Numbers");//第一步选择的要开工的产品
            string userId = ToolCode.Tool.GetUser().UserNumber;
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string sql = string.Format(" delete T_XZWorkOrderTemp where UserId='{0}'", userId);
            sqls.Add(sql);
            sql = string.Format(@"select t.销售订单号+'^'+t.产品编号+'^'+t.版本+'^'+t.交期+'^'+t.行号+'^'+'{1}' as Id,
t.销售订单号,t.产品编号,t.版本,
t.未交货数量,t.库存数量,t.在制品数量,t.需要生产数量,t.交期,t.行号,t.客户产品编号 from ({0})t where t.需要生产数量>0", WorkOrderManager.GetOrderNofinesfinishedDetail(), userId);

            sql = string.Format(@"
    insert into T_XZWorkOrderTemp(Id,OrdersNumber,ProductNumber,Version
,NonDeliveryQty,StockQty,WorkInProgressQty,NeedToProduceQty,LeadTime,RowNumber,CustomerProductNumber,UserId,ZZTeam)
select *,'{2}','{3}' from ({0})t where t.Id in  ({1})", sql, products, userId, team);
            sqls.Add(sql);
            SqlHelper.BatchExecuteSql(sqls, ref error);
            Response.Write(LoadData(true) + "~" + LoadData(false));
            Response.End();
            return;
        }

        /// <summary>
        /// 加载临时表数据
        /// </summary>
        /// <param name="isZZ">是否是制造组</param>
        private string LoadData(bool isZZ)
        {
            string name = isZZ ? "subBoxXZ" : "subBoxJYXZ";
            string show = isZZ ? "inline" : "none";
            string showJY = isZZ ? "none" : "inline";
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string result = string.Empty;
            foreach (DataRow dr in SqlHelper.GetTable(string.Format("select * from T_XZWorkOrderTemp where UserId='{0}'", userId)).Rows)
            {
                result += string.Format(@"<tr >
                            <td>
                                <input type='checkbox' name='{11}' value='{0}' />
                            </td>
                            <td style='display:{12};'> {1} </td>
                            <td style='display:{13};'> {2} </td>
                            <td> {3} </td>
                            <td>{14}</td>
                            <td> {4} </td>
                            <td> {5} </td>
                            <td> {6} </td>
                            <td> {7} </td>
                            <td> {8} </td>
                            <td> {9} </td>
                            <td> {10} </td>
                        </tr> ", dr["Id"], dr["ZZTeam"], dr["JYTeam"], dr["OrdersNumber"], dr["ProductNumber"], dr["Version"], dr["NonDeliveryQty"],
                               dr["StockQty"], dr["WorkInProgressQty"], dr["NeedToProduceQty"], dr["LeadTime"], name, show, showJY, dr["CustomerProductNumber"]);
            }
            return result;
        }

        private string LoadGX()
        {
            string tempCheckBox = string.Empty;
            string tempId = string.Empty;//制造临时主键
            string error = string.Empty;
            string result = string.Empty;
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string sql = string.Format(@"select  OrdersNumber ,ProductNumber ,Version,CustomerProductNumber,NonDeliveryQty,StockQty,WorkInProgressQty
,NeedToProduceQty ,LeadTime ,RowNumber   from T_Process_Temp
where UserId ='{0}'
group by   OrdersNumber ,ProductNumber ,Version,CustomerProductNumber,NonDeliveryQty,StockQty,WorkInProgressQty
,NeedToProduceQty ,LeadTime ,RowNumber", userId);
            DataTable dtTotal = SqlHelper.GetTable(sql);
            DataTable dt = null;
            foreach (DataRow drTotal in dtTotal.Rows)
            {
                //第一行
                tempId = string.Format("{0}-{1}-{2}-{3}-{4}-{5}", userId, drTotal["OrdersNumber"], drTotal["ProductNumber"], drTotal["CustomerProductNumber"], drTotal["Version"], drTotal["RowNumber"]);
                result += string.Format(@" <tr id='{13}'  >
                            <td style='background-color :yellow;'> {0} </td>
                            <td style='background-color :yellow;'>{1}</td>
                            <td style='background-color :yellow;'>{2}</td>
                            <td style='background-color :yellow;'>{3}</td>
                            <td style='background-color :yellow;'>{4}</td>
                            <td style='background-color :yellow;'>{5}</td>
                            <td style='background-color :yellow;'>{6}</td>
                            <td style='background-color :yellow;'>{7}</td>
                            <td style='background-color :yellow;'>{8}</td>
                            <td style='background-color :yellow;'>{9}</td>
                            <td style='background-color :yellow;'>{10}</td>
                            <td style='background-color :yellow;'>{11}</td>
                            <td style='background-color :yellow;'>{12}</td>
                        </tr>", "", "", drTotal["OrdersNumber"], drTotal["CustomerProductNumber"]
    , drTotal["ProductNumber"], drTotal["Version"], drTotal["NonDeliveryQty"], drTotal["StockQty"], drTotal["WorkInProgressQty"],
    drTotal["NeedToProduceQty"], drTotal["LeadTime"], "", drTotal["RowNumber"], tempId);
                sql = string.Format(@"select * from T_Process_Temp where UserId ='{0}'
and OrdersNumber ='{1}' and ProductNumber ='{2}' and Version ='{3}' and CustomerProductNumber='{4}' and RowNumber ='{5}' and isnull( WorkSnNumber,'')!=''
order by OrdersNumber asc ,ProductNumber asc,Version ,RowNumber asc ,SN asc", userId, drTotal["OrdersNumber"]
, drTotal["ProductNumber"], drTotal["Version"], drTotal["CustomerProductNumber"], drTotal["RowNumber"]);
                dt = SqlHelper.GetTable(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    tempCheckBox = dr["WorkSnNumber"] == null ? "" : dr["WorkSnNumber"].ToString();
                    if (string.IsNullOrEmpty(tempCheckBox)) //如果没有工序
                    {
                        tempCheckBox = "&nbsp;";
                    }
                    else
                    {
                        tempCheckBox = string.Format("<input type='checkbox' name='subBoxGX' value='{0}' />", dr["Id"]);
                    }
                    result += string.Format(@" <tr class='{13}'   id='{14}'>
                            <td> {0} </td>
                            <td>{1}</td>
                            <td>{2}</td>
                            <td>{3}</td>
                            <td>{4}</td>
                            <td>{5}</td>
                            <td>{6}</td>
                            <td>{7}</td>
                            <td>{8}</td>
                            <td>{9}</td>
                            <td>{10}</td>
                            <td>{11}</td>
                            <td>{12}</td>
                        </tr>", tempCheckBox,
                              dr["Team"].ToString().Equals("") ? "&nbsp;" : dr["Team"],
                              dr["OrdersNumber"].ToString().Equals("") ? "&nbsp;" : dr["OrdersNumber"],
                              dr["CustomerProductNumber"].ToString().Equals("") ? "&nbsp;" : dr["CustomerProductNumber"]
                              , dr["ProductNumber"].ToString().Equals("") ? "&nbsp;" : dr["ProductNumber"],
                              dr["Version"].ToString().Equals("") ? "&nbsp;" : dr["Version"], "&nbsp;", "&nbsp;", "&nbsp;",
                              "&nbsp;", "&nbsp;", dr["WorkSnName"].ToString().Equals("") ? "&nbsp;" : dr["WorkSnName"],
    "&nbsp;", tempId, tempId + "-" + dr["WorkSnNumber"].ToString());
                }
            }

            // sql = string.Format(" select * from  T_Process_Temp where userId='{0}' order by OrdersNumber asc ,ProductNumber asc,Version ,RowNumber asc ,SN asc", userId);
            //            DataTable dt = SqlHelper.GetTable(sql);
            //            foreach (DataRow dr in dt.Rows)
            //            {
            //                result += string.Format(@" <tr>
            //                            <td> <input type='checkbox' name='subBoxGX' value='{0}' /> </td>
            //                            <td>{1}</td>
            //                            <td>{2}</td>
            //                            <td>{3}</td>
            //                            <td>{4}</td>
            //                            <td>{5}</td>
            //                            <td>{6}</td>
            //                            <td>{7}</td>
            //                            <td>{8}</td>
            //                            <td>{9}</td>
            //                            <td>{10}</td>
            //                            <td>{11}</td>
            //                            <td>{12}</td>
            //                        </tr>", dr["Id"], dr["Team"], dr["OrdersNumber"], dr["CustomerProductNumber"]
            //, dr["ProductNumber"], dr["Version"], dr["NonDeliveryQty"], dr["StockQty"], dr["WorkInProgressQty"],
            //dr["NeedToProduceQty"], dr["LeadTime"], dr["WorkSnName"], dr["RowNumber"]);
            //            }
            return result;
        }

        /// <summary>
        /// 第一步数据
        /// </summary>
        private void LoadPage()
        {
            string userId = ToolCode.Tool.GetUser().UserNumber;
            //            string sql = string.Format(@"select t.销售订单号+'^'+t.产品编号+'^'+t.版本+'^'+t.交期+'^'+t.行号+'^'+'{1}' as Id, t.销售订单号,t.产品编号,t.版本,
            //t.未交货数量,t.库存数量,t.在制品数量,t.需要生产数量,t.交期,t.行号,t.客户产品编号 from ({0})t where t.需要生产数量>0", WorkOrderManager.GetOrderNofinesfinishedDetail(), userId);

            //            rpList.DataSource = SqlHelper.GetTable(sql);
            //            rpList.DataBind();

            string sql = string.Format(" select distinct Team from PM_USER where ISNULL (team,'')!='' and team!='检验'");
            lbZZXZ.DataSource = SqlHelper.GetTable(sql);
            lbZZXZ.DataTextField = "Team";
            lbZZXZ.DataValueField = "Team";
            lbZZXZ.DataBind();

            drpTeam.DataSource = SqlHelper.GetTable(sql);
            drpTeam.DataTextField = "Team";
            drpTeam.DataValueField = "Team";
            drpTeam.DataBind();

            sql = string.Format(" select distinct Team from PM_USER where ISNULL (team,'')!='' and team='检验'");
            lbJYXZ.DataSource = SqlHelper.GetTable(sql);
            lbJYXZ.DataTextField = "Team";
            lbJYXZ.DataValueField = "Team";
            lbJYXZ.DataBind();

            sql = string.Format("select distinct Team from PM_USER where ISNULL (team,'')!='' ");
            liGXTeam.DataSource = SqlHelper.GetTable(sql);
            liGXTeam.DataTextField = "Team";
            liGXTeam.DataValueField = "Team";
            liGXTeam.DataBind();

            SetActualProductQty();
            //            string tempSql = string.Format(@"
            //select t.销售订单号+'^'+t.产品编号+'^'+t.版本+'^'+t.交期+'^'+t.行号+'^'+'{1}' as Id,
            //t.销售订单号,t.产品编号,t.版本,
            //case when t.未交货数量 <0 then 0 else t.未交货数量 end as 未交货数量 ,
            //t.库存数量,t.在制品数量,t.需要生产数量,t.交期,t.行号,t.客户产品编号 from ({0})t where 1=1",
            //WorkOrderManager.GetOrderNofinesfinishedDetail(), userId);

            //            List<string> sqls = new List<string>();
            //            string error = string.Empty;

            //            sql = string.Format("delete T_WorkOrder_Temp where user_id='{0}'", userId);
            //            sqls.Add(sql);

            //            sql = string.Format(@"
            //insert into T_WorkOrder_Temp(id,Qty,User_id)
            //select t.Id,t.需要生产数量,'{1}' from ({0})t
            //", tempSql, userId);
            //            sqls.Add(sql);

            //            SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        //添加小组临时表信息
        private void LoadXZ(bool isZZ, string productId)
        {
            string userId = ToolCode.Tool.GetUser().UserNumber;
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string sql = string.Format(" delete T_XZWorkOrderTemp where UserId='{0}'", userId);
            sqls.Add(sql);
            sql = string.Format(@"select t.销售订单号+'^'+t.产品编号+'^'+t.版本+'^'+t.交期+'^'+t.行号+'^'+'{1}' as Id,
t.销售订单号,t.产品编号,t.版本,
t.未交货数量,t.库存数量,t.在制品数量,t.需要生产数量,t.交期,t.行号,t.客户产品编号 from ({0})t where t.需要生产数量>0", WorkOrderManager.GetOrderNofinesfinishedDetail(), userId);

            sql = string.Format(@"
    insert into T_XZWorkOrderTemp(Id,OrdersNumber,ProductNumber,Version
,NonDeliveryQty,StockQty,WorkInProgressQty,NeedToProduceQty,LeadTime,RowNumber,CustomerProductNumber,UserId)
select *,'{2}' from ({0})t where t.Id in  ({1})", sql, productId, userId);
            sqls.Add(sql);
            SqlHelper.BatchExecuteSql(sqls, ref error);
            Response.Write(LoadData(isZZ));
            Response.End();
            return;
        }

        /// <summary>
        /// 写入工序临时表
        /// </summary>
        private void SetGX()
        {
            string error = string.Empty;
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string ids = ToolManager.GetParamsString("Ids");
            string sql = string.Format(" delete  T_Process_Temp where userId='{0}'", userId);
            SqlHelper.ExecuteSql(sql, ref error);

            string tempSql = string.Format(@"select * from (
select t.销售订单号+'^'+t.产品编号+'^'+t.版本+'^'+t.交期+'^'+t.行号 as Id,t.* from ({0}
) t)h where h.Id+'^'+'{2}' in ({1})", WorkOrderManager.GetOrderNofinesfinishedDetail(), ids, userId);
            tempSql = string.Format(@" select two.*,twt.Qty as 实际生产数量 from ({0})two inner join
 T_WorkOrder_Temp twt on twt.Id =two.Id +'^'+'{1}'
 where  twt.User_id ='{1}' ", tempSql, userId);

            sql = string.Format(@"
insert into T_Process_Temp(Id,UserId ,Team ,OrdersNumber ,ProductNumber ,Version ,CustomerProductNumber ,NonDeliveryQty ,
 StockQty ,WorkInProgressQty ,NeedToProduceQty,LeadTime ,RowNumber ,WorkSnNumber,WorkSnName,SN,qty)
select a.Id ,'{1}','',a.销售订单号,a.产品编号,a.版本,a.客户产品编号,a.未交货数量,a.库存数量,a.在制品数量,
a.需要生产数量,a.交期,a.行号,a.工序编号,ws.WorkSnName,a.工序序号,a.实际生产数量 from (
select '{1}'+'-'+t.销售订单号+'-'+t.产品编号+'-'+t.客户产品编号+'-'+t.版本+'-'+t.行号+'-'+ISNULL ( pwsp.WorkSnNumber,'') as Id,
t.销售订单号,t.产品编号,t.版本,
t.未交货数量,t.库存数量,t.在制品数量,t.需要生产数量,t.交期,t.行号,t.客户产品编号,ISNULL ( pwsp.WorkSnNumber,'') as 工序编号 ,pwsp.RowNumber as 工序序号,t.实际生产数量 from (
{0}
 )t left join ProductWorkSnProperty pwsp on t.产品编号=pwsp .ProductNumber and t.版本=pwsp .Version
  where t.需要生产数量>0) a left join WorkSn ws on a.工序编号=ws.WorkSnNumber
order by a.销售订单号 asc,a.产品编号,a.版本,a.行号 asc ,a.工序序号 asc ", tempSql, userId);
            SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 设置工序开工班组
        /// </summary>
        private void SetGXTeam(string ids, string team)
        {
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string error = string.Empty;
            string sql = string.Format(" update T_Process_Temp set Team ='{0}' where Id in ({1}) and userId='{2}'", team, ids, userId);
            SqlHelper.ExecuteSql(sql, ref error);

            sql = string.Format(@"  select Team , OrdersNumber ,ProductNumber ,Version,
  RowNumber   from T_Process_Temp where UserId ='{0}' and ISNULL ( Team,'')!='' group by Team , OrdersNumber ,ProductNumber ,Version,
  RowNumber  having(COUNT(*))>4", userId);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count > 0)
            {
                sql = string.Format(" update T_Process_Temp set Team ='' where Id in ({0}) and userId='{1}'", ids, userId);
                SqlHelper.ExecuteSql(sql, ref error);
                Response.Write("0");
                Response.End();
                return;
            }
            else
            {
                Response.Write("1");
                Response.End();
                return;
            }
        }

        //更新班组
        private void UpdateTem(string setType)
        {
            string error = string.Empty;
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string team = ToolManager.GetParamsString("Team");
            string check = ToolManager.GetParamsString("NoProductId");
            string cloumName = setType.Equals("zz") ? "ZZTeam" : "JYTeam";
            string sql = string.Format(@"update T_XZWorkOrderTemp set {3}='{0}' where Id in({1})
and UserId='{2}'", team, check, userId, cloumName);
            SqlHelper.ExecuteSql(sql, ref error);
            if (setType.Equals("zz"))
            {
                Response.Write(LoadData(true));
                Response.End();
                return;
            }
            else
            {
                Response.Write(LoadData(false));
                Response.End();
                return;
            }
        }
    }
}