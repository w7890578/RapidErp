using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;
using Model;

namespace BLL
{
    public class MaterialSupplierPropertyManager
    {
        //类内全局变量
        private static string sql = string.Empty;
        private static string error = string.Empty;

        public static string GetSupplierMarerialNames()
        {
            string MarerialNames = string.Empty;
            string sql = @"   
select distinct SupplierMaterialNumber from MaterialSupplierProperty where isnull (SupplierMaterialNumber,'')!=''  ";
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    MarerialNames += dr[0] + ",";
                }
            }
            return MarerialNames.TrimEnd(',');
        }

        /// <summary>
        /// DataTable 转对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static MaterialSupplierProperty ConvertDataTableToModel(string sql)
        {
            error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            DataRow dr = dt.Rows[0];
            MaterialSupplierProperty materialsupplierproperty = new MaterialSupplierProperty();
            materialsupplierproperty.MaterialNumber = dr["MaterialNumber"] == null ? "" : dr["MaterialNumber"].ToString();
            materialsupplierproperty.SupplierMaterialNumber = dr["SupplierMaterialNumber"] == null ? "" : dr["SupplierMaterialNumber"].ToString();
            materialsupplierproperty.SupplierId = dr["SupplierId"] == null ? "" : dr["SupplierId"].ToString();
            materialsupplierproperty.MinOrderQty = dr["MinOrderQty"] == null ? "" : dr["MinOrderQty"].ToString();
            materialsupplierproperty.Remark = dr["Remark"] == null ? "" : dr["Remark"].ToString();
            return materialsupplierproperty;

        }
        /// <summary>
        /// 检测是否有该编号
        /// </summary>
        /// <param name="userNumber"></param>
        /// <returns></returns>
        private static bool IsExit(string MaterialNumber)
        {
            error = string.Empty;
            sql = string.Format(@" select count(*) from MaterialSupplierProperty where MaterialNumber='{0}' ", MaterialNumber);
            if (!SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        ///添加原材料供应商属性
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddMaterialSupplierProperty(Model.MaterialSupplierProperty materialsupplierproperty, ref string error)
        {
            if (IsExit(materialsupplierproperty.MaterialNumber))
            {
                error = "已存在该原材料编号！请重新填写原材料编号。";
                return false;
            }

            if (string.IsNullOrEmpty(materialsupplierproperty.MaterialNumber) || string.IsNullOrEmpty(materialsupplierproperty.SupplierMaterialNumber)
                || string.IsNullOrEmpty(materialsupplierproperty.SupplierId))
            {
                error = "原材料供应商属性信息不完整！";
                return false;
            }
            string sql = string.Format(@" insert into MaterialSupplierProperty (MaterialNumber,SupplierMaterialNumber,SupplierId,MinOrderQty,Remark) 
            values ('{0}','{1}','{2}','{3}','{4}') ", materialsupplierproperty.MaterialNumber, materialsupplierproperty.SupplierMaterialNumber,
            materialsupplierproperty.SupplierId, materialsupplierproperty.MinOrderQty, materialsupplierproperty.Remark);
            return SqlHelper.ExecuteSql(sql, ref error);
        }
        /// <summary>
        /// 删除原材料供应商属性
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static string DeleteData(string ids)
        {
            string sql = string.Format(@" delete MaterialSupplierProperty where guid in ({0}) ", ids);
            return SqlHelper.ExecuteSql(sql, ref error) == true ? "1" : error;
        }

        /// <summary>
        /// 编辑原材料供应商属性
        /// </summary>
        /// <param name="user"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool EditMaterialSupplierProperty(MaterialSupplierProperty materialsupplierproperty, ref string error)
        {
            sql = string.Format(@" update MaterialSupplierProperty set SupplierMaterialNumber='{0}',SupplierId='{1}',
MinOrderQty='{2}',Remark='{3}' where MaterialNumber='{4}'", materialsupplierproperty.SupplierMaterialNumber,
materialsupplierproperty.SupplierId, materialsupplierproperty.MinOrderQty, materialsupplierproperty.Remark,
materialsupplierproperty.MaterialNumber
                );
            return SqlHelper.ExecuteSql(sql, ref error);
        }


    }
}
