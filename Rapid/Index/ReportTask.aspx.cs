using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.Index
{
    public partial class ReportTask : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { Load(); }
        }

        public void Load()
        {
            if (Request["type"] != null && Request["type"] == "MaterialDull")
            {
                List<string> sqls = new List<string>();
                string sql = string.Format(@"
delete Report_MaterialDullQty
                    ");
                sqls.Add(sql);
                sql = @"
insert into Report_MaterialDullQty
select * from V_MaterialDullQty";
                sqls.Add(sql);
                string error = string.Empty;
                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                ToolCode.Tool.WriteLog(ToolCode.Tool.LogType.Operating, "统计呆滞物料", "运行" + result.ToString()+ error);
                Response.Write("1");
                Response.End();
                return;
            }

        }
    }
}