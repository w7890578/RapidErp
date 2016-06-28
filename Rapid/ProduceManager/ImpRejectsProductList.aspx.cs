using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BLL;
using Model;
using Rapid.ToolCode;
using DAL;

namespace Rapid.ProduceManager
{
    public partial class ImpRejectsProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error=string.Empty ;
            bool result = ProductManager.ImpRejectsProduct(FU_Excel ,Server ,ref error ); 
            if (result == true)
            {
                lbMsg.Text = "导入成功";
                Tool.WriteLog(Tool.LogType.Operating, "导入不合格品信息", "导入成功！");
                return;
            }
            else
            {
                lbMsg.Text = error;
                Tool.WriteLog(Tool.LogType.Operating, "导入不合格品信息", "导入失败！原因" + error);
                return;
            }
//            string msgValue = string.Empty;
//            string error = string.Empty;
//            string sql = string.Empty;
//            DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
//            if (ds == null)
//            {
//                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
//                return;
//            }
//            int i = 0;
//            List<string> sqls = new List<string>();
//            foreach (DataRow dr in ds.Tables[0].Rows)
//            {
//                sql = string.Format(@"select count(*) from Product where ProductNumber='{0}' and Version='{1}'", dr["客户产成品编号"], dr["版本"]);
//                if (SqlHelper.GetScalar(sql).Equals("0"))
//                {
//                    msgValue += string.Format("客户产成品{0}添加失败！原因：编号不存在<br/>", dr["客户产成品编号"]);
//                    i++;
//                    continue;
//                }
//                sql = string.Format(@" insert into RejectsProduct (ReportTime ,ProductNumber ,Version ,
//                Qty,RepairReason,RepairDate ,RepairInspectionDate ,Name ,Remark,WhetherBatch,Team)
//                values('{0}','{1}','{2}',{3},'{4}','{5}','{6}','{7}','{8}','{9}','{10}')", DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss")
//                 , dr["客户产成品编号"], dr["版本"], dr["数量"], dr["返修原因"], dr["返修日期"], dr["修回检验日期"], dr["姓名"], dr["备注"], dr["是否成批"], dr["班组"]);
//                if (!SqlHelper.ExecuteSql(sql, ref error))
//                {
//                    msgValue += string.Format("客户产成品{0}添加失败！原因：{1}<br/>", dr["客户产成品编号"], error);
//                    i++;
//                }
//            }
//            bool result = false;
//            if (i == 0)
//            {
//                result = true;
//            }

//            if (result == true)
//            {
//                lbMsg.Text = "导入成功！";
//            }
//            else
//            {
//                lbMsg.Text = "导入失败！<br/>" + msgValue;
//            }
           
        }
    }
}
