using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ProductWarehouseLogDetailModel
    {
        public string WarehouseNumber { get; set; }
        public string DocumentNumber { get; set; }
        public string ProductNumber { get; set; }
        public string Version { get; set; }
        public string RowNumber { get; set; }
        public string OrdersNumber { get; set; }
        public string CustomerProductNumber { get; set; }
        public int Qty { get; set; }

        public int InventoryQty { get; set; }

        public string Guid { get; set; }

        public string Remark { get; set; }
    }
    public class ProductWarehouseLogDetailManager
    {
        private static BLL.ProductWarehouseLogDetailManager _instance = null;
        public static BLL.ProductWarehouseLogDetailManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BLL.ProductWarehouseLogDetailManager();
                }
                return _instance;
            }
        }
        private ProductWarehouseLogDetailManager()
        {
        }

        public ProductWarehouseLogDetailModel Get(string guid)
        {
            ProductWarehouseLogDetailModel model = new ProductWarehouseLogDetailModel();
            string sql = string.Format(@"select top 1 * from  ProductWarehouseLogDetail where guid='{0}'", guid);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                model.Guid = guid;
                model.WarehouseNumber = dr["WarehouseNumber"] == null ? string.Empty : dr["WarehouseNumber"].ToString();
                model.ProductNumber = dr["ProductNumber"] == null ? string.Empty : dr["ProductNumber"].ToString();
                model.Version = dr["Version"] == null ? string.Empty : dr["Version"].ToString();
                model.CustomerProductNumber = dr["CustomerProductNumber"] == null ? string.Empty : dr["CustomerProductNumber"].ToString();
                model.Remark = dr["Remark"] == null ? string.Empty : dr["Remark"].ToString();
                model.Qty = dr["Qty"] == null ? 0 : Convert.ToInt32(dr["Qty"].ToString());
            }
            return model;
        }

        public string GetTypeName(string warehouseNumber)
        {
            string sql = string.Format(@"  
select type from ProductWarehouseLog where warehousenumber='{0}'
", warehouseNumber);
            return SqlHelper.GetScalar(sql);
        }

        public bool CanAutior(string warehouseNumber)
        {
            string sql = string.Format(@"  
select CheckTime from ProductWarehouseLog where warehousenumber='{0}'
", warehouseNumber);

            return string.IsNullOrEmpty(SqlHelper.GetScalar(sql));
        }

        public void Create(ProductWarehouseLogDetailModel model)
        {
            string sql = string.Format(@"
 insert into ProductWarehouseLogDetail (WarehouseNumber,DocumentNumber,ProductNumber,Version,
RowNumber,OrdersNumber,Qty,InventoryQty,Remark,CustomerProductNumber)
values('{0}','','{1}','{2}','','',{3},0,'{4}','{5}')
", model.WarehouseNumber, model.ProductNumber, model.Version, model.Qty, model.Remark, model.CustomerProductNumber);
            SqlHelper.ExecuteSql(sql);
        }

        public void Delete(string guid)
        {
            string sql = string.Format(@"delete ProductWarehouseLogDetail where guid='{0}'", guid);
            SqlHelper.ExecuteSql(sql);
        }

        public void Update(int qty, string remark, string guid)
        {
            string sql = string.Format(@"
update ProductWarehouseLogDetail set Qty ={0},Remark = '{1}'
where Guid = '{2}'", qty, remark, guid);
            SqlHelper.ExecuteSql(sql);
        }

        public DataTable GetTable(string warehouseNumber)
        {
            string sql = string.Format(@"
select ProductNumber as '产成品编号',Version as '版本',CustomerProductNumber as '客户产成品编号',Qty as '数量',
Remark as '备注',Guid from ProductWarehouseLogDetail where WarehouseNumber='{0}'", warehouseNumber);
            return SqlHelper.GetTable(sql);
        }

    }
}
