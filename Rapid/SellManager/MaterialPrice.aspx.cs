using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Model;
using Rapid.ToolCode;

namespace Rapid.SellManager
{
    public partial class MaterialPrice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            bool result = BatchAddMaterialInfo(FU_Excel, Server, ref error);
            lbMsg.Text = result == true ? "导入成功！" : "导入失败！原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入原材料信息", "导入成功！");
                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "导入原材料信息", "导入失败！原因" + error);
                return;
            }
        }

        /// <summary>
        /// 批量添加原材料
        /// </summary>
        /// <param name="FU_Excel"></param>
        /// <param name="server"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool BatchAddMaterialInfo(FileUpload FU_Excel, HttpServerUtility server, ref string error)
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
                if (!AddAddMaterialInfo(dr, ref tempError))
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

        /// <summary>
        /// 批量添加原材料之单条添加【仅仅适用于批量添加】
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool AddAddMaterialInfo(DataRow dr, ref string error)
        {
            List<string> sqls = new List<string>();
            string sql = string.Format(" select COUNT(*) from MarerialInfoTable where MaterialNumber='{0}'", dr["原材料编号"]);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                error = "未知的原材料编号：" + dr["原材料编号"].ToString();
                return false;
            }

            sql = string.Format(@"update MarerialInfoTable set ProcurementPrice={0} where MaterialNumber='{1}'",dr["采购价格"],dr["原材料编号"]);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {

                return true;
            }

            else
            {

                return false;
            }

        }
    }
}
