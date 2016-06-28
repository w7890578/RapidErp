--ԭ���ϱ����б�
--2015-12-24
 alter proc [dbo].[P_MarerialScrapLog](
@dates varchar(20),
@datee varchar(20), 
@startDateTime varchar(30)='',-- ��ʼ����ʱ��
@endDateTime varchar(30)=''   --��������ʱ��
)
as 
begin
if(@startDateTime!=''and @endDateTime!='')
begin

--��ѯ�����磨2014-04-01~2014-04-15������ĩ��2014-04-15~2014-04-30�������ݰ�����з���
select t.����,SUM(t.����) as ����,SUM(t.���) as ��� from
(
select 
msl.Count as ����,
msl.Team as ����,
mt.���� as ����,
msl.Count*mt.���� as ��� 
from MarerialScrapLog msl 
left join (select MaterialNumber,MIN (Prcie ) as ���� from MaterialSupplierProperty group by MaterialNumber) mt on msl.MaterialNumber=mt.MaterialNumber 
where ScrapDate 
between  cast(@startDateTime  as datetime) and  cast(@endDateTime  as datetime)
)t group by t.����
end 
else 
begin

--��ѯ�����磨2014-04-01~2014-04-15������ĩ��2014-04-15~2014-04-30�������ݰ�����з���
select t.����,SUM(t.����) as ����,SUM(t.���) as ��� from
(
select 
msl.Count as ����,
msl.Team as ����,
mt.���� as ����,
msl.Count*mt.���� as ��� 
from MarerialScrapLog msl 
left join (select MaterialNumber,MIN (Prcie ) as ���� from MaterialSupplierProperty group by MaterialNumber) mt on msl.MaterialNumber=mt.MaterialNumber 
where ScrapDate 
between @dates and @datee
)t group by t.����
end


end