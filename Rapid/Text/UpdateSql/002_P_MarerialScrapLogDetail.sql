--ԭ���ϱ�����ϸ
--2015-12-24
alter proc [dbo].[P_MarerialScrapLogDetail](
@scrapdates varchar(20),--��ʼ����
@scrapdatee varchar(20),--��ֹ����
@startDateTime varchar(30)='',-- ��ʼ����ʱ��
@endDateTime varchar(30)=''   --��������ʱ��
)
as 
begin
if(@startDateTime!=''and @endDateTime!='')
begin
--��ѯ�����磨2014-04-01~2014-04-15������ĩ��2014-04-15~2014-04-30��
select msl.ProductNumber as ����Ʒ���,msl.MaterialNumber as ԭ���ϱ��,msl.ScrapDate as ����,
msl.Team as ����,msl.Count as ����,msl.ResponsiblePerson as ������,msl.ScrapReason as ����ԭ��,
msl.ScrapDate as ��������,msl.Remark as ��ע,mt.���� as ����,  msl.Count*mt.���� as �ܼ�,
mt.Description as ԭ��������,mt.MaterialName as ԭ��������
   from MarerialScrapLog msl
    left join (select mit.*,a.���� from (select MaterialNumber,MIN (Prcie ) as ���� from MaterialSupplierProperty 
	group by MaterialNumber) a inner join MarerialInfoTable mit on a.MaterialNumber=mit.MaterialNumber ) mt on msl.MaterialNumber=mt.MaterialNumber
   where ScrapDate between cast(@startDateTime  as datetime) and  cast(@endDateTime  as datetime)
end
else 
begin
--��ѯ�����磨2014-04-01~2014-04-15������ĩ��2014-04-15~2014-04-30��
select msl.ProductNumber as ����Ʒ���,msl.MaterialNumber as ԭ���ϱ��,msl.ScrapDate as ����,
msl.Team as ����,msl.Count as ����,msl.ResponsiblePerson as ������,msl.ScrapReason as ����ԭ��,
msl.ScrapDate as ��������,msl.Remark as ��ע,mt.���� as ����,  msl.Count*mt.���� as �ܼ�,
mt.Description as ԭ��������,mt.MaterialName as ԭ��������
   from MarerialScrapLog msl
    left join (select mit.*,a.���� from (select MaterialNumber,MIN (Prcie ) as ���� from MaterialSupplierProperty 
	group by MaterialNumber) a inner join MarerialInfoTable mit on a.MaterialNumber=mit.MaterialNumber ) mt on msl.MaterialNumber=mt.MaterialNumber
   where ScrapDate between @scrapdates and @scrapdatee
   end
   end
