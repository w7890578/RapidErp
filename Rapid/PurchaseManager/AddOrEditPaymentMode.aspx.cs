using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Model;
using Rapid.ToolCode;

namespace Rapid.PurchaseManager
{
    public partial class AddOrEditPaymentMode : System.Web.UI.Page
    {
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
                sql = string.Format(@" select * from PaymentMode where Id='{0}' ", ToolManager.GetQueryString("Id"));
                //sql = string.Format(@" select * from PaymentMode where Id='{0}' ", "1");
                this.trNumber.Visible = false;
                PaymentModes paymentmodel = PaymentModeManager.ConvertDataTableToModel(sql);
                this.txtNumber.Text = paymentmodel.Id;
                this.txtPaymentMode.Text = paymentmodel.PaymentMode;
                btnSubmit.Text = "修改";
            }
            else
            {

                btnSubmit.Text = "添加";
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            Model.PaymentModes paymentmode = new PaymentModes();
            paymentmode.Id = this.txtNumber.Text.Trim();
            paymentmode.PaymentMode = this.txtPaymentMode.Text.Trim();
            if (string.IsNullOrEmpty(paymentmode.Id) || string.IsNullOrEmpty(paymentmode.PaymentMode))
            {
                lbSubmit.Text = "请将带*号的内容填写完整！";
                return;
            }
            if (PaymentModeManager.CheckHave(paymentmode.PaymentMode))
            {
                lbSubmit.Text = "已经有该付款方式，请重新填写！";
                return;
            }
            else
            {
                bool result;
                if (btnSubmit.Text.Equals("添加"))
                {
                    result = PaymentModeManager.AddPaymentMode(paymentmode, ref error);
                    lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加付款方式" + paymentmode.Id, "增加成功");
                        ToolCode.Tool.ResetControl(this.Controls);
                        return;
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "增加付款方式" + paymentmode.Id, "增加失败！原因" + error);
                        return;
                    }
                }
                else
                {
                    result = PaymentModeManager.EditPaymentMode(paymentmode, ref error);
                    lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                    if (result)
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "编辑付款方式" + paymentmode.Id, "编辑成功");
                        lbSubmit.Text = "修改成功！";
                    }
                    else
                    {
                        Tool.WriteLog(Tool.LogType.Operating, "编辑付款方式" + paymentmode.Id, "编辑失败！原因" + error);
                        lbSubmit.Text = "修改失败！";
                        return;
                    }

                }
            }
        }
    }
}