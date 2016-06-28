using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.StoreroomManager
{
    public partial class ImpMaterialSafeValue : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            int successCount = 0;
            int errorCount = 0;
            string sql = string.Empty;
            string error = string.Empty;
            DataSet ds = ToolManager.ImpExcel(FU_Excel, Server);
            int num = 0;
            string resultStr = string.Empty;
            if (ds == null)
            {
                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
                return;
            }
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    sql = string.Format(@"
 select COUNT(0) from  MarerialInfoTable where MaterialNumber='{0}'", row["原材料编号"]);
                    if (SqlHelper.GetScalar(sql).Equals("0"))
                    {
                        error += "不存在原材料编号：" + row["原材料编号"] + "<br/>";
                       
                    }
                    else
                    {
                        int.TryParse(row["库存安全值"] == null ? "0" : row["库存安全值"].ToString(), out num);
                        sql = string.Format(@"
 update MarerialInfoTable set StockSafeQty={0} where MaterialNumber='{1}'", num, row["原材料编号"]);
                        if (SqlHelper.ExecuteSql(sql))
                        {
                            successCount += 1;
                        }
                    }
                }
                if (successCount > 0)
                {
                    resultStr += "&nbsp;&nbsp;导入成功" + successCount + "条";

                }
                errorCount = dt.Rows.Count - successCount;
                if (errorCount > 0)
                {
                    resultStr += "&nbsp;&nbsp;导入失败" + successCount + "条";
                }
                resultStr += "<br/>" + error;
                lbMsg.Text = resultStr;

            }

        }
    }
}