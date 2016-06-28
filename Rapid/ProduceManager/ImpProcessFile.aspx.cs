using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class ImpProcessFile : System.Web.UI.Page
    {
        public ProcessTestingTable model = new ProcessTestingTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("Id"))
                {
                    string sql = string.Format(@" select * from ProcessTestingTable where Id='{0}' ", ToolManager.GetQueryString("Id"));
                    model = ProcessTestingTableManager.ConvertDataTableToModel(sql);
                    lbId.Text = model.Id;
                    lbProductionOrderNumber.Text = model.ProductionOrderNumber;
                    lbProductNumber.Text = model.ProductNumber;
                    lbVersion.Text = model.Version;
                    lbCustomerProductNumber.Text = model.CustomerProductNumber;
                    txtRemark.Text = model.Remark;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {


            string fileName = this.fuFileUrl.FileName;
            if (string.IsNullOrEmpty(fileName))
            {
                lbSubmit.Text = "请选择文件";
                return;
            }
            string sql = string.Format(@" select * from ProcessTestingTable where Id='{0}' ", lbId.Text);
            model = ProcessTestingTableManager.ConvertDataTableToModel(sql);
            model.ImportTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            model.ImportPerson = ToolCode.Tool.GetUser().UserNumber;

            string dataName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), Path.GetExtension(fileName));

            this.fuFileUrl.SaveAs(Server.MapPath(string.Concat("/UpLoadFileData/", dataName)));
            model.ImgUrl = string.Concat("../UpLoadFileData/", dataName);
            model.FileName = fileName;
            string error = string.Empty;
            bool result = ProcessTestingTableManager.EditProcessTestingTable(model, ref error);
            lbSubmit.Text = result == true ? "修改成功！" : "修改失败：原因：" + error;
            if (result)
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑过程检验信息" + model.Id, "编辑成功");
                Response.Write(ToolManager.GetClosePageJS());
                Response.End();

                return;
            }
            else
            {
                Tool.WriteLog(Tool.LogType.Operating, "编辑过程检验信息" + model.Id, error);
                return;
            }
        }
    }
}