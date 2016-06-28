using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.OracleClient;
using System.Collections;
using System.Data.OleDb;

namespace DAL
{
    /// <summary>
    /// 通用数据访问类[Sqlserver And Oracle]
    /// </summary>
    public static class SqlHelper
    {

        #region 分页获取数据
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">一页显示条数</param>
        /// <param name="sql">查询语句</param>
        /// <param name="sortName">排序Sql 例:order by username desc</param>
        /// <param name="pageCount">返回总行数</param>
        /// <returns></returns>
        public static DataTable GetDataForPage(string pageIndex, string pageSize, string sql, string sortSql, ref int totalRecords)
        {
            totalRecords = 0;
            string error = string.Empty;
            int startIndex = (Convert.ToInt32(pageIndex) - 1) * Convert.ToInt32(pageSize) + 1;
            int endIndex = startIndex + Convert.ToInt32(pageSize) - 1;

            string pageCountSql = string.Format(" select count(*) from ( {0} ) as temp", sql);
            DataTable countTable = GetTable(pageCountSql, ref error);
            if (countTable != null && countTable.Rows.Count > 0)
            {
                object temp = countTable.Rows[0][0];
                totalRecords = temp == null ? 0 : Convert.ToInt32(temp);
            }
            sql = string.Format(@"   select * from ( select ROW_NUMBER () over({0}) as rownum,* from ({1}) as temp1 ) 
			as  templist where rownum between {2} and {3} ", sortSql, sql, startIndex, endIndex);

            return GetTable(sql);
        }
        #endregion

        #region 获取首行首列
        public static string GetScalar(string sql)
        {
            string result = string.Empty;
            if (GetDatabaseType().Equals("Sqlserver"))
            {
                result = GetScalarForSqlServer(sql);
            }
            return result;
        }
        #endregion

        #region 获取table
        /// <summary>
        /// 获取table
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetTable(string sql, ref string error)
        {
            error = string.Empty;
            DataTable dt = null;
            if (GetDatabaseType().Equals("Sqlserver"))
            {
                dt = GetTableToSqlserver(sql, ref error);
            }
            else
            {
                dt = GetTableToOracle(sql, ref error);
            }
            return dt;

        }
        public static DataTable GetTable(string sql)
        {
            string error = string.Empty;
            DataTable dt = null;
            if (GetDatabaseType().Equals("Sqlserver"))
            {
                dt = GetTableToSqlserver(sql, ref error);
            }
            else
            {
                dt = GetTableToOracle(sql, ref error);
            }
            return dt;

        }
        #endregion

        #region 执行sql
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSql(string sql)
        {
            string error = string.Empty;
            bool result = false;
            if (GetDatabaseType().Equals("Sqlserver"))
            {
                result = ExecuteSqlToSqlserver(sql);
            }
            else
            {
                result = ExecuteSqlToOracle(sql, ref error);
            }
            return result;

        }
        #endregion

        #region 执行sql
        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSql(string sql, ref string error)
        {
            error = string.Empty;
            bool result = false;
            if (GetDatabaseType().Equals("Sqlserver"))
            {
                result = ExecuteSqlToSqlserver(sql, ref error);
            }
            else
            {
                result = ExecuteSqlToOracle(sql, ref error);
            }
            return result;

        }
        #endregion

        #region 批量执行sql
        /// <summary>
        /// 批量执行sql
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public static bool BatchExecuteSql(List<string> sqls, ref string error)
        {
            error = string.Empty;
            bool result = false;
            if (GetDatabaseType().Equals("Sqlserver"))
            {
                result = BatchExecuteSqlToSqlserver(sqls, ref error);
            }
            else
            {
                result = BatchExecuteSqlToOracle(sqls, ref error);
            }
            return result;
        }
        #endregion

        #region 执行存储过程
        public static bool ExecutePrc(string sql, ref string error)
        {
            error = string.Empty;
            bool result = false;
            if (GetDatabaseType().Equals("Sqlserver"))
            {
                result = ExecutePrcToSqlserver(sql, ref error);
            }
            else
            {
                result = false;
            }
            return result;

        }
        #endregion

        #region 获取首行首列(SqlServer)
        public static string GetScalarForSqlServer(string sql)
        {

            SqlConnection con = new SqlConnection(GetConnectStr());
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                object ds = cmd.ExecuteScalar();
                con.Close();
                return ds == null ? "" : ds.ToString();
            }
            catch (Exception ex)
            {
                return "";
                throw ex;
            }
            finally
            {
                con.Dispose();
            }
        }

        #endregion

        #region 获取table(Sqlserver)
        /// <summary>
        /// 获取table(Sqlserver)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetTableToSqlserver(string sql, ref  string error)
        {
            SqlConnection con = new SqlConnection(GetConnectStr());
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds == null ? null : ds.Tables[0];
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
                //throw ex;
            }
            finally
            {
                con.Dispose();
            }

        }
        #endregion

        #region 执行存储过程(Sqlserver)
        public static bool ExecutePrcToSqlserver(string sql, ref  string error)
        {
            SqlConnection con = new SqlConnection(GetConnectStr());
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                int i = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                //throw ex;
            }
            finally
            {
                con.Dispose();
            }
        }
        #endregion

        #region 执行sql(Sqlserver)

        /// <summary>
        /// 执行sql(Sqlserver)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSqlToSqlserver(string sql)
        {
            SqlConnection con = new SqlConnection(GetConnectStr());
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                int i = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                // error = ex.Message.Replace('\'', ' ');
                return false;
                throw ex;
            }
            finally
            {
                con.Dispose();
            }
        }
        #endregion

        #region 执行sql(Sqlserver)

        /// <summary>
        /// 执行sql(Sqlserver)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSqlToSqlserver(string sql, ref  string error)
        {
            SqlConnection con = new SqlConnection(GetConnectStr());
            try
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                int i = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message.Replace('\'', ' ');
                return false;
                //throw ex;
            }
            finally
            {
                con.Dispose();
            }
        }
        #endregion



        #region 批量执行sql(Sqlserver)

        /// <summary>
        /// 批量执行sql(Sqlserver)
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public static bool BatchExecuteSqlToSqlserver(List<string> sqls, ref string error)
        {
            SqlConnection con = new SqlConnection(GetConnectStr());
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            SqlTransaction tc = con.BeginTransaction();
            try
            {
                cmd.CommandType = CommandType.Text;

                foreach (string sql in sqls)
                {
                    cmd.Transaction = tc;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                tc.Commit();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                tc.Rollback();
                error = ex.Message.Replace('\'', ' ');
                return false;
                //throw ex;
            }
            finally
            {
                con.Dispose();
            }
        }
        #endregion

        #region  获取table(Oracle)
        /// <summary>
        /// 获取table(Oracle)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataTable GetTableToOracle(string sql, ref string error)
        {

            OracleConnection con = new OracleConnection(GetConnectStr());
            try
            {
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                con.Close();
                return ds == null ? null : ds.Tables[0];
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
                // throw ex;
            }
            finally
            {
                con.Dispose();
            }
        }
        #endregion

        #region 执行sql(Oracle)
        /// <summary>
        /// 执行sql(Oracle)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ExecuteSqlToOracle(string sql, ref string error)
        {
            OracleConnection con = new OracleConnection(GetConnectStr());
            try
            {
                con.Open();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                int i = cmd.ExecuteNonQuery();
                con.Close();
                return i > 0 ? true : false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                //throw ex;
            }
            finally
            {
                con.Dispose();
            }
        }
        #endregion

        #region 批量执行sql(Oracle)
        /// <summary>
        /// 批量执行sql(Oracle)
        /// </summary>
        /// <param name="sqls"></param>
        /// <returns></returns>
        public static bool BatchExecuteSqlToOracle(List<string> sqls, ref string error)
        {
            OracleConnection con = new OracleConnection(GetConnectStr());
            con.Open();
            OracleCommand cmd = con.CreateCommand();
            OracleTransaction tc = con.BeginTransaction();
            try
            {
                cmd.CommandType = CommandType.Text;
                foreach (string sql in sqls)
                {
                    cmd.CommandText = sql;
                    cmd.Transaction = tc;
                    cmd.ExecuteNonQuery();
                }

                tc.Commit();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                tc.Rollback();
                error = ex.Message;
                return false;
                //throw ex;
            }
            finally
            {
                con.Dispose();
            }
        }
        #endregion

        private static string GetDatabaseType()
        {
            return ConfigurationManager.AppSettings["DefaultDatabase"] == null ? "" : ConfigurationManager.AppSettings["DefaultDatabase"].ToString();
        }

        private static string GetConnectStr()
        {
            string keyName = GetDatabaseType();
            return ConfigurationManager.ConnectionStrings[keyName] == null ? "" : ConfigurationManager.ConnectionStrings[keyName].ConnectionString;
        }

        #region 从Excel导入数据
        //该方法实现从Excel中导出数据到DataSet中，其中filepath为Excel文件的绝对路径， sheetname为excel文件中的表名
        public static DataSet ExcelDataSource(string filepath, string sheetname)
        {
            string strConn;
            strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + filepath + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
            OleDbConnection conn = new OleDbConnection(strConn);
            OleDbDataAdapter oada = new OleDbDataAdapter("select * from [" + sheetname + "]", strConn);
            DataSet ds = new DataSet();
            oada.Fill(ds);
            conn.Close();
            return ds;
        }
        #endregion

        #region 获取Excelsheetname
        //获得Excel中的所有sheetname。
        public static ArrayList ExcelSheetName(string filepath)
        {
            ArrayList al = new ArrayList();
            //string strConn;
            ////strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + filepath + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
            //strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + filepath + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
            string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + filepath + ";Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'"; //此连接可以操作.xls与.xlsx文件 (支持Excel2003 和 Excel2007 的连接字符串)
            //备注： "HDR=yes;"是说Excel文件的第一行是列名而不是数据，"HDR=No;"正好与前面的相反。
            //      "IMEX=1 "如果列中的数据类型不一致，使用"IMEX=1"可必免数据类型冲突。
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable
            (System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();
            foreach (DataRow dr in sheetNames.Rows)
            {
                al.Add(dr[2]);
            }
            return al;
        }
        #endregion
    }
}
