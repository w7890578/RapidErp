--原材料报废列表
--2015-12-24
 alter proc [dbo].[P_MarerialScrapLog](
@dates varchar(20),
@datee varchar(20), 
@startDateTime varchar(30)='',-- 开始日期时间
@endDateTime varchar(30)=''   --结束日期时间
)
as 
begin
if(@startDateTime!=''and @endDateTime!='')
begin

--查询月中如（2014-04-01~2014-04-15）和月末（2014-04-15~2014-04-30）并根据班组进行分组
select t.班组,SUM(t.数量) as 数量,SUM(t.金额) as 金额 from
(
select 
msl.Count as 数量,
msl.Team as 班组,
mt.单价 as 单价,
msl.Count*mt.单价 as 金额 
from MarerialScrapLog msl 
left join (select MaterialNumber,MIN (Prcie ) as 单价 from MaterialSupplierProperty group by MaterialNumber) mt on msl.MaterialNumber=mt.MaterialNumber 
where ScrapDate 
between  cast(@startDateTime  as datetime) and  cast(@endDateTime  as datetime)
)t group by t.班组
end 
else 
begin

--查询月中如（2014-04-01~2014-04-15）和月末（2014-04-15~2014-04-30）并根据班组进行分组
select t.班组,SUM(t.数量) as 数量,SUM(t.金额) as 金额 from
(
select 
msl.Count as 数量,
msl.Team as 班组,
mt.单价 as 单价,
msl.Count*mt.单价 as 金额 
from MarerialScrapLog msl 
left join (select MaterialNumber,MIN (Prcie ) as 单价 from MaterialSupplierProperty group by MaterialNumber) mt on msl.MaterialNumber=mt.MaterialNumber 
where ScrapDate 
between @dates and @datee
)t group by t.班组
end


end