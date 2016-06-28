using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;

namespace Rapid.Index
{
    public partial class UserEditPwd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string pwdOld = txtPwdOld.Text.Trim();
            string pwdNew = txtPwdNew.Text.Trim();
            string pwdNewAgain = txtPwdNewAgain.Text.Trim();
            if (string.IsNullOrEmpty(pwdOld) || string.IsNullOrEmpty(pwdNew) || string.IsNullOrEmpty(pwdNewAgain))
            {
                lbMsg.Text = "请将信息填写完整！";
                return;
            }
            if (!ToolCode.Tool.GetUser().Pwd.Equals(ToolManager.MdshashString(pwdOld)))
            {
                lbMsg.Text = "当前密码输入错误！";
                return;
            }
            if (!pwdNew.Equals(pwdNewAgain))
            {
                lbMsg.Text = "新密码和确认密码输入不一致！";
                return;
            }
            string error = string.Empty;
            string sql = string.Format(" update pm_user set password='{0}' where user_id ='{1}' ", ToolManager.MdshashString(pwdNew), ToolCode.Tool.GetUser().UserNumber);
            if (SqlHelper.ExecuteSql(sql, ref error))
            {
                lbMsg.Text = "密码修改成功！";
                ToolCode.Tool.ResetControl(this.Controls);
                ToolCode.Tool.GetUser().Pwd = ToolManager.MdshashString(pwdNew);
                return;
            }
            else
            {
                lbMsg.Text = "密码修改失败，原因：" + error;
                return;
            }
        }
    }
}
