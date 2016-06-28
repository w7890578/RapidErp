using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;
using BLL;
using System.Data;
using Rapid.ToolCode;
using DAL;
using System.IO;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditProcessTestingTable : System.Web.UI.Page
    {
        public static string show = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadPage();
            }
        }
        private void LoadPage()
        {

            string sql = string.Empty;
            string error = string.Empty;
            if (ToolManager.CheckQueryString("Id"))
            {
                sql = string.Format(@" select * from ProcessTestingTable where Id='{0}' ", ToolManager.GetQueryString("Id"));
                ProcessTestingTable processtestingtable = ProcessTestingTableManager.ConvertDataTableToModel(sql);
                this.lblProductionOrderNumber.Text = processtestingtable.ProductionOrderNumber;
                this.lblProductNumber.Text = processtestingtable.ProductNumber;
                this.lblCustomerProductNumber.Text = processtestingtable.CustomerProductNumber;
                this.txtRemark.Text = processtestingtable.Remark;
                this.lblVersion.Text = processtestingtable.Version;
                btnSubmit.Text = "修改";
                txtCustomerProductNumber.Visible = false;
                txtProductionOrderNumber.Visible = false;
                txtProductNumber.Visible = false;
                show = "none";
            }
            else
            {
                show = "";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            Model.ProcessTestingTable processtestingtable = new ProcessTestingTable();
            processtestingtable.ProductionOrderNumber = this.txtProductionOrderNumber.Text.Trim();
            processtestingtable.ProductNumber = this.txtProductNumber.Text.Trim();
            processtestingtable.CustomerProductNumber = this.txtCustomerProductNumber.Text.Trim();
            processtestingtable.ImportTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            processtestingtable.ImportPerson = ToolCode.Tool.GetUser().UserNumber;
            processtestingtable.Remark = this.txtRemark.Text.Trim();
            processtestingtable.Version = txtVersion.Text.ToUpper().Trim();
            bool result = false;
            string sql = string.Empty;
            DataTable dt;
            if (btnSubmit.Text.Equals("添加"))
            {
                processtestingtable.Id = Guid.NewGuid().ToString();

                if (string.IsNullOrEmpty(processtestingtable.ProductionOrderNumber)
                || string.IsNullOrEmpty(processtestingtable.ImportTime) || string.IsNullOrEmpty(processtestingtable.ImportPerson))
                {
                    lbSubmit.Text = "请将带*号的内容填写完整！";
                    return;
                }

                sql = string.Format("select count(*) from ProductPlanDetail where PlanNumber='{0}'", processtestingtable.ProductionOrderNumber);
                string num = SqlHelper.GetScalar(sql);
                if (num == "0")
                {
                    lbSubmit.Text = "未知的开工单编号！";
                    return;
                }
                sql = string.Format("select count(*) from ProductPlanDetail where CustomerProductNumber='{0}' and PlanNumber='{1}' and Version='{2}'",
                    processtestingtable.CustomerProductNumber, processtestingtable.ProductionOrderNumber, processtestingtable.Version);
                num = SqlHelper.GetScalar(sql);
                if (num == "0")
                {
                    lbSubmit.Text = "未知的客户产成品编号！";
                    return;
                }
                sql = string.Format(@"select  ProductNumber from ProductPlanDetail  where CustomerProductNumber='{0}' and PlanNumber='{1}' and Version='{2}'",
                    processtestingtable.CustomerProductNumber, processtestingtable.ProductionOrderNumber, processtestingtable.Version);
                dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    processtestingtable.ProductNumber = dt.Rows[0]["ProductNumber"].ToString();
                }
                else
                {
                    lbSubmit.Text = "未知的客户产成品编号";
                    return;
                }
                string strFileName = this.fuFileUrl.FileName;
                if (strFileName == "")
                {
                    lbSubmit.Text = "请选择文件！";
                    return;
                }
                string dataName = string.Concat(DateTime.Now.ToString("yyyyMMddHHmmss"), Path.GetExtension(this.fuFileUrl.PostedFile.FileName));

                this.fuFileUrl.SaveAs(Server.MapPath(string.Concat("/UpLoadFileData/", dataName)));
                processtestingtable.ImgUrl = string.Concat("../UpLoadFileData/", dataName);


                processtestingtable.FileName = strFileName;

                result = ProcessTestingTableManager.AddProcessTestingTable(processtestingtable, ref error);
                lbSubmit.Text = result == true ? "添加成功！" : "添加失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加过程检验信息" + processtestingtable.Id, "增加成功");
                    ToolCode.Tool.ResetControl(this.Controls);
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加过程检验信息" + processtestingtable.Id, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                processtestingtable.Id = ToolManager.GetQueryString("Id");
                result = ProcessTestingTableManager.EditProcessTestingTable(processtestingtable, ref error);
                lbSubmit.Text = result == true ? "修改成功！" : "修改失败：原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑过程检验信息" + processtestingtable.Id, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑过程检验信息" + processtestingtable.Id, "编辑成功！原因" + error);
                    return;
                }
            }
        }
    }
}
