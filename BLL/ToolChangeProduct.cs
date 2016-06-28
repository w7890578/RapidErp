using System;
using System.Collections.Generic;
using System.Text;
using DAL;

namespace BLL
{
    public class ToolChangeProduct
    {
        string sql = string.Empty;
        string error = string.Empty;
        public void changeproduct(string productnumber, string version)
        {
            //成本价
            sql = string.Format(@" select isnull(CONVERT(decimal(18,2), sum(b.SingleDose*mt.ProcurementPrice)),0) as 成本价 from Product p 
                left join BOMInfo b on p.ProductNumber=b.ProductNumber and p.Version=b.Version
                left join MarerialInfoTable mt on b.MaterialNumber=mt.MaterialNumber
                where b.ProductNumber='{0}' and b.Version='{1}'", productnumber, version);
            string costprice = SqlHelper.GetScalar(sql);
            //额定工时
            sql = string.Format(@"   select ISNULL(sum(pwp.RatedManhour),0) as 额定工时 from Product p left join 
                ProductWorkSnProperty pwp on p.ProductNumber=pwp.ProductNumber
                and p.Version=pwp.Version where pwp.ProductNumber='{0}' and pwp.Version='{1}'", productnumber, version);
            string ratedmanhour = SqlHelper.GetScalar(sql);
            if (costprice != null && costprice != "" && ratedmanhour != null && ratedmanhour != "")
            {
                sql = string.Format(@" update Product set RatedManhour='{0}',CostPrice='{1}' 
            where ProductNumber='{2}' and Version='{3}'", ratedmanhour, costprice, productnumber, version);
                SqlHelper.ExecuteSql(sql, ref error);
            }

        }
        //改变bom单机用量
        public string changeBomSingleDose(string productnumber, string version, string materialnumber)
        {
            sql = string.Format(@" update BOMInfo set SingleDose=(select sum(p.Length*p.Qty) from ProductCuttingLineInfo p
             where  ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}') where  
            ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}'",
                       productnumber, version, materialnumber);
            return sql;

        }
        //改变产品成本价
        public string changeProductCostPrice(string productnumber, string version)
        {
            sql = string.Format(@" update product set CostPrice=(select isnull(CONVERT(decimal(18,2), sum(b.SingleDose*mt.ProcurementPrice)),0) as 成本价
                from Product p left join BOMInfo b on p.ProductNumber=b.ProductNumber and p.Version=b.Version
                left join MarerialInfoTable mt on b.MaterialNumber=mt.MaterialNumber
                where b.ProductNumber='{0}' and b.Version='{1}') where productnumber='{0}' and version='{1}'",
                        productnumber, version);
            return sql;

        }

    }
}
