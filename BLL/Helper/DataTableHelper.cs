using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DataTableHelper
    {
        public static DataTableHelper Instance
        {
            get
            {
                return new DataTableHelper();
            }
        }
        private DataTableHelper()
        {

        }
        /// <summary>
        /// DataTable查询
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public DataTable Select(DataTable dt, string condition)
        {
            DataTable newdt = new DataTable();
            newdt = dt.Clone();
            DataRow[] dr = dt.Select(condition);
            for (int i = 0; i < dr.Length; i++)
            {
                newdt.ImportRow((DataRow)dr[i]);
            }
            return newdt;//返回的查询结果
        }
    }
}
