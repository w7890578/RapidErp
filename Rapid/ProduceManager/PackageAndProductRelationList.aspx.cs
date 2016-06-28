using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class PackageAndProductRelationList : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        public static string hasDelete = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0304", "Add");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0304", "Edit");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0304", "Delete");
                if (!ToolManager.CheckQueryString("PackageNumber"))
                {
                    Response.Write("未知包编码！");
                    Response.End();
                    return;
                }



                Bind();
            }
        }
        private void Bind()
        {
            string sql = string.Empty;
            string error = string.Empty;
            if (ToolManager.CheckQueryString("PackageNumber") && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
            {
                string packagenumber = ToolManager.GetQueryString("PackageNumber");
                sql = string.Format("delete PackageAndProductRelation where PackageNumber ='{0}' and ProductNumber='{1}' and Version='{2}'",
                ToolManager.GetQueryString("PackageNumber"), ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"));
                bool result=SqlHelper.ExecuteSql(sql, ref error) ;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除包与产品对应关系" +ToolManager.ReplaceSingleQuotesToBlank(packagenumber), "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除包与产品对应关系"+ToolManager.ReplaceSingleQuotesToBlank(packagenumber), "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }
            sql = string.Format(@"
           select * from PackageAndProductRelation where PackageNumber='{0}'", ToolManager.GetQueryString("PackageNumber"));
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();
            hdProductNumber.Value = ToolManager.GetQueryString("ProductNumber");
            hdVersion.Value = ToolManager.GetQueryString("Version");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
    }
}
