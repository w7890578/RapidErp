using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using BLL;
using System.Data;
using DAL;
using Model;

namespace Rapid.Master
{
    /// <summary>
    /// $codebehindclassname$ 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetStockQty : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            string sql = string.Empty;
            //取原材料或者产品
            if (ToolManager.CheckQueryString("IsMaterial"))
            {
                //原材料
                if (ToolManager.GetQueryString("IsMaterial").Equals("true"))
                {
                    sql = string.Format(@" select top 20  MaterialNumber,MaterialName from MarerialInfoTable ");

                    sql += string.Format(@"  where
MaterialNumber like '%{0}%' or MaterialNumber like'%{0}' or MaterialNumber like '{0}%' or
MaterialName like '%{0}%' or MaterialName like'%{0}' or MaterialName like '{0}%' 
order by MaterialNumber asc", ToolManager.GetQueryString("MaterialNumber"));

                    DataTable dt = SqlHelper.GetTable(sql);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            result += string.Format(" <tr><td>{0}</td><td>{1}</td></tr>", dr["MaterialName"], dr["MaterialNumber"]);
                        }
                    }
                }
                else//产品
                { }

            }
            //取库存数量
            if (ToolManager.CheckQueryString("TableName"))
            {
                string tableName = ToolManager.GetQueryString("TableName");
                string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber"); //出入库编号 
                string supplierMaterialNumberStr = string.Empty;                        //供应商物料编号字符串
                string customerMaterialNumberStr = string.Empty;                        //客户物料编号字符串
                string qty = string.Empty;//库存数量
                //原材料
                if (ToolManager.GetQueryString("IsMaterialForQty").Equals("true"))
                {
                    string materialNumber = ToolManager.GetQueryString("MaterialNumber"); //原材料编号
                    sql = string.Format(@"select SupplierMaterialNumber
from MaterialSupplierProperty where MaterialNumber='{0}'", materialNumber);
                    supplierMaterialNumberStr = ControlBindManager.GetOption(sql, "SupplierMaterialNumber", "SupplierMaterialNumber");
                    sql = string.Format(@"
select CustomerMaterialNumber from MaterialCustomerProperty where MaterialNumber='{0}'", materialNumber);
                    customerMaterialNumberStr = ControlBindManager.GetOption(sql, "CustomerMaterialNumber", "CustomerMaterialNumber");
                    qty = StoreroomToolManager.GetinventoryQty(materialNumber, "", "", warehouseNumber, tableName, ToolEnum.ProductType.Marerial);
                    result = supplierMaterialNumberStr+"^"+customerMaterialNumberStr+"^"+qty ;
                }
                else //产品
                { }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
