using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class DeliveryBillList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckParams("IsCheck"))
                {
                    string numbers = ToolManager.GetParamsString("DNumbers");
                    string sql = string.Format(@" select DeliveryNumber  from DeliveryNoteDetailed 
	 where DeliveryNumber in ({0})
	 group by DeliveryNumber 
	 having SUM(isnull(ConformenceQty,0))=0", numbers);
                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        Response.Write("系统检测出所选送货单中有未填写数量的送货单，请确认是否都填写好了实收数量！");
                        Response.End();
                        return;
                    }
                    else
                    {
                        Response.Write("ok");
                        Response.End();
                        return;
                    }
                }
                //删除
                if (ToolManager.CheckQueryString("ids"))
                {
                    string error = string.Empty;
                    List<string> sqls = new List<string>();
                    string deliveryNumbers = ToolManager.GetQueryString("ids");
                    string sql = string.Format(" delete DeliveryNoteDetailed where DeliveryNumber in ({0}) ", deliveryNumbers);
                    sqls.Add(sql);
                    sql = string.Format("delete DeliveryBill where DeliveryNumber in ({0})", deliveryNumbers);
                    sqls.Add(sql);
                    bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                    if (result)
                    {
                        Response.Write("1");
                        Tool.WriteLog(Tool.LogType.Operating, "删除送货单" + deliveryNumbers, "删除成功");
                        Response.End();
                        return;

                    }
                    else
                    {
                        Response.Write(error);
                        Tool.WriteLog(Tool.LogType.Operating, "删除送货单" + deliveryNumbers, "删除失败！原因" + error);
                        Response.End();
                        return;
                    }

                }
                //查询

                if (ToolManager.CheckQueryString("pageIndex"))
                {
                    GetPage("AddOrEditDeliveryBillList.aspx", "Transfer", "btnSearch", "340", "600");
                }
                //汇总
                if (ToolManager.CheckQueryString("Numbers"))
                {
                    string numbers = ToolManager.GetQueryString("Numbers");
                    CollectNumber(numbers);
                }
                //确认
                if (ToolManager.CheckQueryString("confirmNumbers"))
                {
                    string numbers = ToolManager.GetQueryString("confirmNumbers");
                    ConfirmNumber(numbers);
                }
                divExp.Visible = ToolCode.Tool.GetUserMenuFunc("L0104", "Exp");
            }
        }
        private void GetPage(string editUrl, string detailFunctionForJS, string btnId, string height, string width)
        {
            hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0104", "Edit");
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
            DataTable dt = SqlHelper.GetDataForPage(pageIndex, pageSize, querySql, string.Format(" order by {0} {1}", sortName, sortDirection), ref totalRecords);
            int columCount = dt.Columns.Count;
            string temp = string.Empty;
            string show = "inline";
            foreach (DataRow dr in dt.Rows)
            {
                tdTextTemp = "";
                for (int i = 0; i < columCount; i++)
                {
                    //第一列为序号
                    if (i == 0)
                    {
                        tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                    }
                    else
                    {
                        //Guid列
                        if (dt.Columns[i].ColumnName.Equals("Guid") || dt.Columns[i].ColumnName.Equals("客户编号"))
                        {
                            tdTextTemp += string.Format("<td style='display:none;'>{0}</td>", dr[i]);
                        }
                        else if (dt.Columns[i].ColumnName.Equals("送货单号"))
                        {
                            tdTextTemp += string.Format(@" <td><a href='###' title='点击进入详细' style='color:blue;' 
onclick=""{1}('{0}','{2}','{3}')"">{0}</a>  </td>", dr[1], detailFunctionForJS, dr[3], dr["客户编号"]);

                        }
                        else
                        {
                            tdTextTemp += string.Format("<td>{0}</td>", dr[i]);
                        }
                    }
                }
                temp = dr["确认状态"] == null ? "" : dr["确认状态"].ToString();
                if (temp.Equals("已确认"))
                {
                    temp = "";
                    show = "none";
                }
                else
                {
                    temp = string.Format("<input type='checkbox' name='subBox' value='{0}'/>", dr["送货单号"]);
                    show = "inline";
                }
                text += string.Format(@"<tr><td>{9}
</td>{1} 
<td>
<span style='display:{11};'>
<a href='###'   style='display:{10};'  onclick=""OpenDialog('{2}?Id={0}&date={3}','{4}','{5}','{6}')""> 
<img src='../Img/037.gif' width='9' height='9' />
<span > [</span>   <label class='edit'>编辑</label> <span >]</span></a>
</span>
<a href='###'  onclick=""{8}('{0}','{7}','{12}')""> 
<img src='../Img/detail.png' width='9' height='9' />
<span > [</span>   <label class='edit'>详细</label> <span >]</span></a>
</td></tr>", dr[1], tdTextTemp, editUrl, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), btnId, height, width, dr[3], detailFunctionForJS, temp, show, hasEdit, dr["客户编号"]);
            }
            string pageing = ToolManager.PagerGetAjax("http://www.baidu.com", totalRecords, Convert.ToInt32(pageIndex), Convert.ToInt32(pageSize), ref pageCount);
            string responseValue = pageCount.ToString() + "^" + text + "^" + pageing + "^" + totalRecords;
            HttpContext.Current.Response.Write(responseValue);
            HttpContext.Current.Response.End();
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            string sql = saveInfo.Value;
            if (string.IsNullOrEmpty(sql))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(sql, "送货单列表");

        }

        //确认
        private void ConfirmNumber(string numbers)
        {
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string sql = string.Format(@" select COUNT (*) from DeliveryBill where DeliveryNumber in ({0}) and (ISNULL ( DeliveryPerson,'')='' or ISNULL (DeliveryDate,'')='' )
", numbers);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                Response.Write("请确保所选送货单都已经设置送货人和送货日期！");
                Response.End();
                return;
            }


            string createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sql = string.Format("    update DeliveryBill set IsConfirm ='已确认',ConfirmTime='{1}'  where DeliveryNumber in ({0})", numbers, createtime);
            sqls.Add(sql);
            //            sql = string.Format(@" 
            //            select COUNT (*) from DeliveryNoteDetailed where ConformenceQty=0 and DeliveryNumber in ({0}) ", numbers);
            //            if (!SqlHelper.GetScalar(sql).Equals("0"))
            //            {
            //                Response.Write("请确认送货单实收数量不为0");
            //                Response.End();
            //                return;
            //            }

            sql = string.Format(@"select dnd.*,so.ProductType from DeliveryNoteDetailed dnd inner join  SaleOder so on dnd.OrdersNumber =so.OdersNumber
            where dnd.DeliveryNumber in({0})", numbers);
            //同步更新订单已交货、未交货数量 
            foreach (DataRow dr in SqlHelper.GetTable(sql).Rows)
            {
                if (dr["ProductType"].ToString().Equals("贸易"))
                {
                    sql = string.Format(@"
update 
    TradingOrderDetail 
set 
    DeliveryQty=isnull(DeliveryQty,0)+{0}
where  
    OdersNumber ='{1}' 
    and ProductNumber ='{2}' 
    and RowNumber ='{3}'"
, dr["ConformenceQty"], dr["OrdersNumber"], dr["ProductNumber"], dr["RowNumber"]);
                    sqls.Add(sql);
                    sql = string.Format(@"
update 
    TradingOrderDetail 
set   
    NonDeliveryQty=Quantity -isnull(DeliveryQty,0)
where  
    OdersNumber ='{1}' 
    and ProductNumber ='{2}' 
    and RowNumber ='{3}'"
, dr["ConformenceQty"], dr["OrdersNumber"], dr["ProductNumber"], dr["RowNumber"]);
                    sqls.Add(sql);
                }
                else
                {
                    sql = string.Format(@"
update 
    MachineOderDetail 
set 
    DeliveryQty=isnull(DeliveryQty,0)+{0}  
 where  
    OdersNumber ='{1}' 
    and ProductNumber ='{2}' 
    and Version='{3}' 
    and RowNumber ='{4}'"
 , dr["ConformenceQty"], dr["OrdersNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"]);
                    sqls.Add(sql);
                    sql = string.Format(@"
update 
    MachineOderDetail 
set  
    NonDeliveryQty=Qty -isnull(DeliveryQty,0)
 where  
    OdersNumber ='{1}' 
    and ProductNumber ='{2}' 
    and Version='{3}' 
    and RowNumber ='{4}'"
 , dr["ConformenceQty"], dr["OrdersNumber"], dr["ProductNumber"], dr["Version"], dr["RowNumber"]);
                    sqls.Add(sql);
                }
            }
            //更新加工销售订单已交、未交数量
            //            sql = string.Format(@" 
            //update MachineOderDetail 
            //set 
            //	DeliveryQty=t.ConformenceQty,
            //	NonDeliveryQty=Qty-t.ConformenceQty  
            //from  
            //	MachineOderDetail m 
            //inner join 
            //( 
            //	select 
            //		OrdersNumber,
            //		ProductNumber ,
            //		Version ,
            //		RowNumber,
            //		SUM(ConformenceQty) ConformenceQty 
            //	from 
            //		DeliveryNoteDetailed    
            //	group by 
            //		OrdersNumber,ProductNumber ,Version ,RowNumber
            //) t 
            //on 
            //	t.OrdersNumber=m.OdersNumber 
            //	and t.ProductNumber=m.ProductNumber
            //	and t.Version=m.Version 
            //	and t.RowNumber=m.RowNumber
            //where 
            //	m.DeliveryQty!=t.ConformenceQty
            //", numbers);
            //            sqls.Add(sql);


            //更新贸易销售订单、加工销售订单以及总表完成状态
            //自动排错
            sql = @"update MachineOderDetail set NondeliveryQty=0,DeliveryQty=Qty
where NondeliveryQty<0";
            sqls.Add(sql);

            sql = "update TradingOrderDetail set Status ='已完成' where NonDeliveryQty =0";
            sqls.Add(sql);
            sql = "update MachineOderDetail set Status ='已完成' where NonDeliveryQty =0";
            sqls.Add(sql);
            sql = @"  update SaleOder set OrderStatus ='已完成' where OdersNumber in (
	                    select OdersNumber from TradingOrderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0
	                    union
                      select OdersNumber from MachineOderDetail group by OdersNumber having(SUM (NonDeliveryQty ))=0)";
            sqls.Add(sql);
            List<string> tempList = BLL.FinancialManager.GetSHYSForAll(numbers);
            //if (tempList.Count > 0)
            //{
            //    sqls.AddRange(tempList);
            //}
            //            //产生应收
            //            string tempsql = string.Format(@" 
            //            select t.OrdersNumber ,t.ProductNumber,case  t.Version when '0' then ' ' when '无' then ' ' else ' ' end Version ,
            //            ISNULL ( t.CustomerId,'') as 客户编号,
            //            ISNULL(tod.CustomerMaterialNumber ,'') as 客户物料编号,
            //            ISNULL ( t.总实收数量,0) as 数量,
            //            ISNULL(tod.UnitPrice ,0) as 单价,
            //             ISNULL ( t.总实收数量,0)*ISNULL(tod.UnitPrice ,0) as 总价 ,
            //             ISNULL (t.DeliveryDate,'') as 送货日期
            //             from (
            //            select dnd.OrdersNumber ,dnd.ProductNumber ,dnd.Version,db.CustomerId,db.DeliveryDate,SUM (dnd.ConformenceQty) as 总实收数量 from DeliveryNoteDetailed dnd
            //            inner join DeliveryBill db on dnd.DeliveryNumber =db.DeliveryNumber
            //             where db.DeliveryNumber in ({0})
            //            group by dnd.OrdersNumber ,dnd.ProductNumber ,dnd.Version ,dnd.CustomerProductNumber,db.CustomerId,db.DeliveryDate
            //            ) t inner join TradingOrderDetail tod on t.OrdersNumber=tod.OdersNumber and t.ProductNumber=tod.ProductNumber
            //            union 
            //            select t.OrdersNumber ,t.ProductNumber, t.Version ,
            //            ISNULL ( t.CustomerId,'') as 客户编号,
            //            ISNULL(modt.CustomerProductNumber ,'') as 客户物料编号,
            //            ISNULL ( t.总实收数量,0) as 数量,
            //            ISNULL(modt.UnitPrice ,0) as 单价,
            //             ISNULL ( t.总实收数量,0)*ISNULL(modt.UnitPrice ,0) as 总价,
            //             ISNULL (t.DeliveryDate,'') as 送货日期  from (
            //            select dnd.OrdersNumber ,dnd.ProductNumber ,dnd.Version,db.CustomerId,db.DeliveryDate,SUM (dnd.ConformenceQty) as 总实收数量 from DeliveryNoteDetailed dnd
            //            inner join DeliveryBill db on dnd.DeliveryNumber =db.DeliveryNumber
            //             where db.DeliveryNumber in ({0})
            //            group by dnd.OrdersNumber ,dnd.ProductNumber ,dnd.Version ,dnd.CustomerProductNumber,db.CustomerId,db.DeliveryDate
            //            ) t inner join MachineOderDetail modt on t.OrdersNumber=modt.OdersNumber and t.ProductNumber=modt.ProductNumber
            //            and t.Version=modt.Version  ", numbers);
            //            //预收全款、发货收款、预收部分
            //            sql = string.Format(@"
            //            insert into AccountsReceivable(OrdersNumber ,ProductNumber ,Version,CustomerId,CustomerProductNumber ,Qty ,UnitPrice ,SumPrice,DeliveryDate ,CreateTime )
            //            select f.*,'{1}' from (
            //            select h.* from ({0})h inner join  SaleOder so on h.OrdersNumber =so.OdersNumber
            //            where so.MakeCollectionsMode ='YSQK'  
            //            union
            //            select h.OrdersNumber,h.ProductNumber,h.Version,h.客户编号
            //            ,h.客户物料编号,0,0,h.总价*c.PercentageInAdvance,h.送货日期 from ({0})h inner join  SaleOder so on h.OrdersNumber =so.OdersNumber inner join Customer c on c.CustomerId =h.客户编号
            //            where so.MakeCollectionsMode ='YSBF'
            //            union
            //            select h.* from ({0})h inner join  SaleOder so on h.OrdersNumber =so.OdersNumber inner join Customer c on c.CustomerId =h.客户编号
            //            where so.MakeCollectionsMode ='FHSK'  and c.Paymentdays =0) f where f.总价>0 ", tempsql, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //            sqls.Add(sql);


            //产成预收、应收第二层（根据已确认的送货单）
            sql = string.Format(@"insert into T_AccountsReceivable_Detail(DeliveryNumber,OrdersNumber,CustomerOrderNumber,RapidNumber,
CustomerMaterialNumber,Version,Description,DeliveryQty,Price,SumPrice,RowNumber,Remark,CreateTime)
(select t.* from DeliveryBill db inner join 
(select dnd.DeliveryNumber as 送货单号,
dnd.OrdersNumber as 销售订单号,
so.CustomerOrderNumber as 客户采购订单号,
dnd.ProductNumber as 瑞普通编号,
dnd.CustomerProductNumber as 客户物料编号,
 '' as 版本,
 tod.MaterialName as 描述,
 dnd.ConformenceQty as 交货数量,
 tod.UnitPrice as 单价,
 tod.UnitPrice*dnd.ConformenceQty as 总价,
 dnd.RowNumber as 行号,
 dnd.Remark as 备注,
 '{0}' as 创建时间 from DeliveryNoteDetailed dnd 
left join TradingOrderDetail tod on dnd.OrdersNumber=tod.OdersNumber and dnd.ProductNumber=tod.ProductNumber and dnd.RowNumber=tod.RowNumber
inner join SaleOder so on tod.OdersNumber=so.OdersNumber  where dnd.DeliveryNumber in ({1}))t on db.DeliveryNumber=t.送货单号 where isnull(db.ConfirmTime,'')!=''
union all
select t.* from DeliveryBill db inner join 
(select dnd.DeliveryNumber as 送货单号,
dnd.OrdersNumber as 销售订单号,
so.CustomerOrderNumber as 客户采购订单号,
dnd.ProductNumber as 瑞普通编号,
dnd.CustomerProductNumber as 客户物料编号,
dnd.Version as 版本,
p.Description as 描述,
 dnd.ConformenceQty as 交货数量,
mo.UnitPrice as 单价,
 mo.UnitPrice*dnd.ConformenceQty as 总价,
 dnd.RowNumber as 行号,
 dnd.Remark as 备注,
 '{0}' as 创建时间 from DeliveryNoteDetailed dnd 
left join MachineOderDetail mo on dnd.OrdersNumber=mo.OdersNumber and dnd.ProductNumber=mo.ProductNumber and dnd.RowNumber=mo.RowNumber
and mo.Version=dnd.Version and mo.CustomerProductNumber=dnd.CustomerProductNumber
inner join Product p on mo.ProductNumber=p.ProductNumber and p.Version=mo.Version
inner join SaleOder so on mo.OdersNumber=so.OdersNumber where dnd.DeliveryNumber in ({1}))t on db.DeliveryNumber=t.送货单号 where isnull(db.ConfirmTime,'')!='')", createtime, numbers);
            sqls.Add(sql);
            //产生应收第一层
            //string[] array = numbers.Split(',');
            //foreach (string str in array)
            //{
            sql = string.Format(@"  insert into AccountsReceivable (OrdersNumber,CreateTime,CustomerOrdersNumber,DeliveryNumber,DeliveryQty,CustomerId,PaymentTypes,SKFS,DeliveryDate,IsAdvance) 
(select t.OrdersNumber,
'{1}',
so.CustomerOrderNumber,
t.DeliveryNumber,
t.交货数量,
db.CustomerId ,
c.ReceiveType,
c.MakeCollectionsModeId,
db.DeliveryDate,
 '否'   from (
select DeliveryNumber,OrdersNumber ,SUM (ConformenceQty ) as 交货数量 from DeliveryNoteDetailed  
where DeliveryNumber in ({0})
group by DeliveryNumber,OrdersNumber) t  inner join DeliveryBill db on db.DeliveryNumber =t.DeliveryNumber  
inner join Customer c on db.CustomerId =c.CustomerId 
inner join SaleOder so on so.OdersNumber=t.OrdersNumber
where ISNULL( db.ConfirmTime,'')!=''  and c.MakeCollectionsModeId not in ('YSBF','YSQK'))", numbers, createtime);
            sqls.Add(sql);
            //}
            sql = string.Format(@"update AccountsReceivable set DeliveryPrice=b.交货总价  from   
 AccountsReceivable ar inner join (
 --获取本次送货单内属于预收范围内的销售订单，并对这些销售订单进行明细总价汇总
select OrdersNumber,SUM ( isnull(SumPrice,0)) as 交货总价 from T_AccountsReceivable_Detail where OrdersNumber in (select dnd.OrdersNumber from DeliveryNoteDetailed dnd inner join SaleOder so on dnd.OrdersNumber=so.OdersNumber
inner join Customer c on so.CustomerId=c.CustomerId where (so.MakeCollectionsMode='YSBF' or so.MakeCollectionsMode='YSQK')
and dnd.DeliveryNumber in({0})) group by OrdersNumber 
) b on ar.OrdersNumber =b.OrdersNumber ", numbers);
            sqls.Add(sql);
            sql = string.Format(@"update AccountsReceivable set DeliveryPrice=b.交货总价  from   
 AccountsReceivable ar inner join (
 --获取本次送货单内属于应收范围内的销售订单，并对这些销售订单按照订单号和创建时间进行明细总价汇总
select OrdersNumber,CreateTime ,SUM(ISNULL( SumPrice,0) ) as 交货总价 from T_AccountsReceivable_Detail where OrdersNumber in (select dnd.OrdersNumber from DeliveryNoteDetailed dnd inner join SaleOder so on dnd.OrdersNumber=so.OdersNumber
inner join Customer c on so.CustomerId=c.CustomerId where so.MakeCollectionsMode !='YSBF' and so.MakeCollectionsMode !='YSQK'
and dnd.DeliveryNumber in({0}) ) and CreateTime ='{1}'
group by OrdersNumber ,CreateTime  ) b on ar.OrdersNumber =b.OrdersNumber and ar.CreateTime =b.CreateTime ", numbers, createtime);
            sqls.Add(sql);
            string result = SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
            Tool.WriteLog(Tool.LogType.Operating, "送货单确认", "numbers:"+numbers);
            ToolManager.ZDJC();

            Response.Write(result);
            Response.End();
            return;
        }

        public static void GetValue()
        {
            string numbers = "'001','002','003'";
            string[] array = numbers.Split(',');
            foreach (string str in array)
            {

            }
        }
        //汇总
        private void CollectNumber(string numbers)
        {
            string remark = string.Empty;
            string customerId = string.Empty;//客户编号
            string deliveryPerson = string.Empty;//送货人
            string deliveryNumber = "SH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string sql = string.Format(" select distinct CustomerId from DeliveryBill where DeliveryNumber in({0})", numbers);
            if (SqlHelper.GetTable(sql).Rows.Count > 1)
            {
                Response.Write("相同客户才能进行汇总");
                Response.End();
                return;
            }
            sql = string.Format("select CustomerId,DeliveryNumber,DeliveryPerson  from DeliveryBill   where DeliveryNumber in({0}) ", numbers);
            DataTable dt = SqlHelper.GetTable(sql);
            customerId = dt.Rows[0]["CustomerId"].ToString();
            deliveryPerson = dt.Rows[0] == null ? "" : dt.Rows[0]["DeliveryPerson"].ToString();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    remark += string.Format("{0}、", dr["DeliveryNumber"]);
            //}
            remark = string.Format("由送货单汇总产生", remark.TrimEnd('、'));
            //产生汇总后的送货单
            sql = string.Format(@"insert into DeliveryBill(DeliveryNumber,DeliveryPerson,IsConfirm ,CreateTime ,Remark ,CustomerId )
values('{0}','{1}','{2}','{3}','{4}','{5}') ", deliveryNumber, deliveryPerson, "未确认", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), remark, customerId);
            sqls.Add(sql);
            //            sql = string.Format(@"insert into DeliveryNoteDetailed(DeliveryNumber ,OrdersNumber ,ProductNumber ,Version ,CustomerProductNumber ,RowNumber ,DeliveryQty)
            //select '{1}',OrdersNumber ,ProductNumber,Version,CustomerProductNumber,RowNumber ,SUM(DeliveryQty) from DeliveryNoteDetailed
            // where DeliveryNumber in ({0})
            // group by OrdersNumber ,ProductNumber,Version,CustomerProductNumber,RowNumber ", numbers, deliveryNumber);
            //            sqls.Add(sql);
            sql = string.Format(@"insert into DeliveryNoteDetailed (DeliveryNumber ,OrdersNumber ,ProductNumber,Version,CustomerProductNumber,RowNumber,LeadTime ,SN
 ,MaterialDescription,DeliveryQty,ArriveQty ,ConformenceQty,NGReason ,PassQty ,NgQty ,InspectorNGReason 
 ,RoughCastingCode ,ImportPartsCode ,IsGeneratingCope)
 select '{1}',OrdersNumber ,ProductNumber,Version,CustomerProductNumber,RowNumber,LeadTime ,SN
 ,MaterialDescription,DeliveryQty,ArriveQty ,ConformenceQty,NGReason ,PassQty ,NgQty ,InspectorNGReason 
 ,RoughCastingCode ,ImportPartsCode ,IsGeneratingCope from DeliveryNoteDetailed 
  where DeliveryNumber in ({0})
", numbers, deliveryNumber);
            sqls.Add(sql);
            //删除原来信息
            sql = string.Format(" delete DeliveryNoteDetailed where DeliveryNumber in({0})", numbers);
            sqls.Add(sql);
            sql = string.Format("   delete DeliveryBill where DeliveryNumber in({0})", numbers);
            sqls.Add(sql);
            string result = SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error;
            Response.Write(result);
            Response.End();
            return;

        }
    }
}
