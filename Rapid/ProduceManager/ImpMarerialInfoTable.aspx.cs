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

namespace Rapid.ProduceManager
{
    public partial class ImpMarerialInfoTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string error = string.Empty; 
            bool result = MarerialInfoTableManager.BatchAddMaterialInfo(FU_Excel ,Server ,ref error );
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
            //DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
            //if (ds == null)
            //{
            //    lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
            //    return;
            //}
            //List<MarerialInfoTable> marerialinfotables = new List<MarerialInfoTable>();
            //foreach (DataRow dr in ds.Tables[0].Rows)
            //{

                //MarerialInfoTable marerialinfotable = new MarerialInfoTable();
                //marerialinfotable.MaterialNumber = dr["原材料编号"] == null ? "" : dr["原材料编号"].ToString();
                //marerialinfotable.MaterialName = dr["名称"] == null ? "" : dr["名称"].ToString();
                //marerialinfotable.Description = dr["描述"]== null ? "" : dr["描述"].ToString();

                //marerialinfotable.Type = dr["类别"] == null ? "" : dr["类别"].ToString();
                //marerialinfotable.Brand = dr["品牌"] == null ? "" : dr["品牌"].ToString();
                //if (dr["库存安全值"] == null)
                //{
                //    marerialinfotable.StockSafeQty = 0;
                //}
                //else {
                //    marerialinfotable.StockSafeQty = Convert.ToInt32(dr["库存安全值"].ToString());
                //}
                //if (dr["采购价格"] == null )
                //{
                //    marerialinfotable.ProcurementPrice = Convert.ToDecimal(0.00);
                //}
                //else
                //{
                //    marerialinfotable.ProcurementPrice = Convert.ToDecimal(dr["采购价格"].ToString());
                //}
                //marerialinfotable.MaterialPosition = dr["原材料仓位"] == null ? "" : dr["原材料仓位"].ToString();
                //marerialinfotable.MinPacking = dr["最小包装"] == null ? "" : dr["最小包装"].ToString();
                //marerialinfotable.MinOrderQty = dr["最小起订量"] == null ? "" : dr["最小起订量"].ToString();
                //marerialinfotable.ScrapPosition = dr["废品仓位"] == null ? "" : dr["废品仓位"].ToString();
                //marerialinfotable.Remark = dr["备注"]== null ? "" : dr["备注"].ToString();
                //marerialinfotables.Add(marerialinfotable);
            //}
           // bool result = MarerialInfoTableManager.BatchAddData(marerialinfotables, ref error);
            
        }
    }
}
