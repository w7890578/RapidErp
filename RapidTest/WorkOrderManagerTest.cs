using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace RapidTest
{
    [TestClass]
    public class WorkOrderManagerTest
    {
        [TestMethod]
        public void GetWorkOrderSqlTest()
        {
            string str = BLL.WorkOrderManager.GetWorkOrderSql();
        }
    }
}
