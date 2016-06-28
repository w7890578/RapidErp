using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MaterialWarehouseLogDetail
    {
        public string WarehouseNumber { get; set; }
        public string DocumentNumber { get; set; }
        public string MaterialNumber { get; set; }
        public string SupplierMaterialNumber { get; set; }
        public string SupplierName { get; set; }
        public double Qty { get; set; }
        public string Remark { get; set; }
        public string Guid { get; set; }
    }
    public class MaterialWarehouseLogDetailManager
    {
        private static BLL.MaterialWarehouseLogDetailManager _instance = null;
        public static BLL.MaterialWarehouseLogDetailManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BLL.MaterialWarehouseLogDetailManager();
                }
                return _instance;
            }
        }
        private MaterialWarehouseLogDetailManager()
        {
        }

        public void Delete(string guid)
        {
            string sql = string.Format(@"delete MaterialWarehouseLogDetail where guid='{0}'", guid);
            SqlHelper.ExecuteSql(sql);
        }

        public MaterialWarehouseLogDetail Get(string guid)
        {
            MaterialWarehouseLogDetail model = new MaterialWarehouseLogDetail();
            string sql = string.Format(@"select top 1 * from  MaterialWarehouseLogDetail where guid='{0}'", guid);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                model.Guid = guid;
                model.WarehouseNumber = dr["WarehouseNumber"] == null ? string.Empty : dr["WarehouseNumber"].ToString();
                model.DocumentNumber = dr["DocumentNumber"] == null ? string.Empty : dr["DocumentNumber"].ToString();
                model.MaterialNumber = dr["MaterialNumber"] == null ? string.Empty : dr["MaterialNumber"].ToString();
                model.SupplierMaterialNumber = dr["SupplierMaterialNumber"] == null ? string.Empty : dr["SupplierMaterialNumber"].ToString();
                model.Remark = dr["Remark"] == null ? string.Empty : dr["Remark"].ToString();
                model.Qty = dr["Qty"] == null ? 0 : Convert.ToDouble(dr["Qty"].ToString());
            }
            return model;
        }

        public string GetTypeName(string warehouseNumber)
        {
            string sql = string.Format(@" 
	select type from MarerialWarehouseLog where warehousenumber='{0}'
", warehouseNumber);
            return SqlHelper.GetScalar(sql);
        }

        public bool CanAutior(string warehouseNumber)
        {
            string sql = string.Format(@" 
	select CheckTime from MarerialWarehouseLog where warehousenumber='{0}'
", warehouseNumber);

            return string.IsNullOrEmpty(SqlHelper.GetScalar(sql));
        }

        public void Create(MaterialWarehouseLogDetail model)
        {
            string sql = string.Format(@"
 insert into MaterialWarehouseLogDetail
  (WarehouseNumber,DocumentNumber,MaterialNumber,CreateTime,SupplierMaterialNumber,Qty,Remark,Guid,supplierId,supplierName)
select '{0}','{1}','{2}','{3}',(select SupplierMaterialNumber from MaterialSupplierProperty where MaterialNumber='{2}' and supplierId=(
select supplierId from SupplierInfo where supplierName='{7}'
)),{4},'{5}','{6}',supplierId,supplierName from SupplierInfo where supplierName='{7}'
", model.WarehouseNumber, model.DocumentNumber, model.MaterialNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
, model.Qty, model.Remark, model.Guid, model.SupplierName);
            SqlHelper.ExecuteSql(sql);
        }

        public void Update(MaterialWarehouseLogDetail model)
        {
            string sql = string.Format(@"
update MaterialWarehouseLogDetail set Qty ={0},Remark = '{1}'
where Guid = '{2}'", model.Qty, model.Remark, model.Guid);
            SqlHelper.ExecuteSql(sql);
        }

        public DataTable GetTable(string warehouseNumber)
        {
            string sql = string.Format(@" 
select 
	 DocumentNumber as '相关单号',
	a.MaterialNumber as '原材料编号',
	a.SupplierMaterialNumber as '供应商物料编号' ,
	b.Cargo as '货位',
	a.[SupplierName] as '供应商名称',
	b.Description as '原材料描述',
	isnull(c.StockQty,0) as '实时库存数量',
	b.CargoType as '货物类型',
	a.Qty as '数量',
	a.Remark as '备注',
	a.guid as 'guid'
 from MaterialWarehouseLogDetail a 
 inner join MarerialInfoTable  b on  a.MaterialNumber=b.MaterialNumber 
 left join MaterialStock  c on c.MaterialNumber=a.MaterialNumber
  where a.WarehouseNumber='{0}'
", warehouseNumber);
            return SqlHelper.GetTable(sql);
        }
    }
}
