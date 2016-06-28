--原材料报废明细
--2015-12-24
alter proc [dbo].[P_MarerialScrapLogDetail](
@scrapdates varchar(20),--初始日期
@scrapdatee varchar(20),--截止日期
@startDateTime varchar(30)='',-- 开始日期时间
@endDateTime varchar(30)=''   --结束日期时间
)
as 
begin
if(@startDateTime!=''and @endDateTime!='')
begin
--查询月中如（2014-04-01~2014-04-15）和月末（2014-04-15~2014-04-30）
select msl.ProductNumber as 产成品编号,msl.MaterialNumber as 原材料编号,msl.ScrapDate as 日期,
msl.Team as 班组,msl.Count as 数量,msl.ResponsiblePerson as 责任人,msl.ScrapReason as 报废原因,
msl.ScrapDate as 报废日期,msl.Remark as 备注,mt.单价 as 单价,  msl.Count*mt.单价 as 总价,
mt.Description as 原材料描述,mt.MaterialName as 原材料名称
   from MarerialScrapLog msl
    left join (select mit.*,a.单价 from (select MaterialNumber,MIN (Prcie ) as 单价 from MaterialSupplierProperty 
	group by MaterialNumber) a inner join MarerialInfoTable mit on a.MaterialNumber=mit.MaterialNumber ) mt on msl.MaterialNumber=mt.MaterialNumber
   where ScrapDate between cast(@startDateTime  as datetime) and  cast(@endDateTime  as datetime)
end
else 
begin
--查询月中如（2014-04-01~2014-04-15）和月末（2014-04-15~2014-04-30）
select msl.ProductNumber as 产成品编号,msl.MaterialNumber as 原材料编号,msl.ScrapDate as 日期,
msl.Team as 班组,msl.Count as 数量,msl.ResponsiblePerson as 责任人,msl.ScrapReason as 报废原因,
msl.ScrapDate as 报废日期,msl.Remark as 备注,mt.单价 as 单价,  msl.Count*mt.单价 as 总价,
mt.Description as 原材料描述,mt.MaterialName as 原材料名称
   from MarerialScrapLog msl
    left join (select mit.*,a.单价 from (select MaterialNumber,MIN (Prcie ) as 单价 from MaterialSupplierProperty 
	group by MaterialNumber) a inner join MarerialInfoTable mit on a.MaterialNumber=mit.MaterialNumber ) mt on msl.MaterialNumber=mt.MaterialNumber
   where ScrapDate between @scrapdates and @scrapdatee
   end
   end
