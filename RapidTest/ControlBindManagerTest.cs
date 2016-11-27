using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidTest
{
    [TestClass]
    public class ControlBindManagerTest
    {
        [TestMethod]
        public void TableTest()
        {
            string sql = @"
select OrdersNumber,OrdersNumber+' ('+HTNumber+')' as text
from CertificateOrders
where OrderStatus ='未完成' and ISNULL (CheckTime ,'') !=''
order by CreateTime asc";
            DataTable dt = SqlHelper.GetTable(sql);
            List<string> removeValues = new List<string>() { "CG20150211132219" };
            DataTable dtResult = dt.Clone();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    var value = dr["OrdersNumber"] == null ? "" : dr["OrdersNumber"].ToString();
                    if (removeValues.Contains(value))
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }
                //foreach (DataRow dr in dt.Rows)
                //{
                //    var value = dr["OrdersNumber"] == null ? "" : dr["OrdersNumber"].ToString();
                //    if (!removeValues.Contains(value))
                //    {
                //        DataRow newRow = dtResult.NewRow();
                //        newRow = dr;
                //        dtResult.Rows.Add(newRow);
                //    }
                //}
            }
        }
    }
}