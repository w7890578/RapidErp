using JingBaiHui.Common;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class BaseDAL
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public static DataBase db = DataBaseFactory.Create(Const.DBName);
    }
}
