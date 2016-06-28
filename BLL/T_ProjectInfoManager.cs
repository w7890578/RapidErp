using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;
using System.Web;
using System.Web.UI.WebControls;

namespace BLL
{
    public class T_ProjectInfoManager
    {

        /// <summary>
        /// 不合格品上报单条添加
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool AddProjectInfo(DataRow dr, ref string error)
        {
            string version = dr["版本"].ToString().ToUpper();
            if (version.Equals(""))
            {
                version = "WU";
            }
            string sql = string.Format(@"select ProductNumber 
from ProductCustomerProperty where CustomerProductNumber ='{0}' and Version='{1}'", dr["客户产成品编号"], version);
            DataTable dt = SqlHelper.GetTable(sql);
            if (dt.Rows.Count == 0)
            {
                error = string.Format("系统不存在该客户产成品编号:{0},版本{1}", dr["客户产成品编号"], version);
                return false;
            }

            string productNumber = dt.Rows[0]["ProductNumber"].ToString();

            sql = string.Format(@"select COUNT(*) from T_ProjectInfo where ProjectName ='{0}' 
and ProductNumber ='{1}' and Version ='{2}' and CustomerProductNumber ='{3}' and Hierarchy ='{4}'", dr["项目名称"], productNumber, version, dr["客户产成品编号"], dr["阶层"]);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = string.Format("已存在相同记录，请重新导入！");
                return false;
            }
            sql = string.Format(@"insert into T_ProjectInfo (ProjectName,ProductNumber,Version ,CustomerProductNumber,Hierarchy ,Description ,Single,Remark )
select '{0}','{1}','{2}','{3}', '{6}',Description as 描述,'{4}','{5}'   from Product  
where ProductNumber ='{1}' and Version ='{2}'", dr["项目名称"], productNumber, version, dr["客户产成品编号"], dr["单机"], dr["备注"], dr["阶层"]);

            return SqlHelper.ExecuteSql(sql, ref error);
        }

        /// <summary>
        /// 导入不合格品
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool ImpProjectInfo(FileUpload FU_Excel, HttpServerUtility server, ref string error)
        {
            DataSet ds = ToolManager.ImpExcel(FU_Excel, server);
            if (ds == null)
            {
                error = "选择的文件为空或不是标准的Excel文件！";
                return false;
            }
            DataTable dt = ds.Tables[0];
            int i = 0;
            string tempError = string.Empty;
            if (dt.Rows.Count <= 0)
            {
                error = "没有要添加的数据";
                return false;
            }
            foreach (DataRow dr in dt.Rows)
            {
                tempError = "";
                if (!AddProjectInfo(dr, ref tempError))
                {
                    i++;
                    error += string.Format("添加失败：原因--{0}<br/>", tempError);
                }
            }
            bool result = i > 0 ? false : true;
            if (!result)
            {
                error = string.Format("添加成功{0}条，添加失败{1}条。<br/>{2}", (dt.Rows.Count - i).ToString(), i.ToString(), error);
            }
            return result;

        }
    }
}
