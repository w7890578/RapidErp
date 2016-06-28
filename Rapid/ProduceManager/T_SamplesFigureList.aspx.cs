using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.IO;

namespace Rapid.ProduceManager
{
    public partial class T_SamplesFigureList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindListBox();
            }
        }
        private void BindListBox()
        {
            string sql = string.Format(@"
select distinct CustomerProductNumber from T_SamplesFigure ");
            lbNumber.DataSource = SqlHelper.GetTable(sql);
            lbNumber.DataTextField = "CustomerProductNumber";
            lbNumber.DataValueField = "CustomerProductNumber";
            lbNumber.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string number = txtNumber.Text.Trim();
            if (string.IsNullOrEmpty(number))
            {
                lbMsg.Text = "请输入图纸号";
                return;
            }
            string sql = string.Format(@" select count(*) from T_SamplesFigure  where CustomerProductNumber='{0}' ", number);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbMsg.Text = "已存在该图纸号";
                return;
            }
            sql = string.Format(@"insert into T_SamplesFigure (CustomerProductNumber)
values('{0}')", number);
            lbMsg.Text = SqlHelper.ExecuteSql(sql, ref error) ? "添加成功" : "添加失败！原因：" + error;
            BindListBox();
        }

        protected void btnImp_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string number = txtNumberImp.Text;
            if (string.IsNullOrEmpty(number))
            {
                lbMsgImp.Text = "请选择图纸号！";
                return;
            }
            string strFileName = this.fuFileUrl.FileName;
            if (string.IsNullOrEmpty(strFileName))
            {
                lbMsgImp.Text = "请上传图片！";
                return;
            }
            strFileName = Path.GetFileName(fuFileUrl.PostedFile.FileName);//文件名加扩展名
            string FileUrl = Server.MapPath("/UpLoadFileData/" + strFileName);
            this.fuFileUrl.SaveAs(FileUrl);
            string urlData = "../UpLoadFileData/" + strFileName;
            string sql = string.Format(@" select count(*) 
from T_SamplesFigure where CustomerProductNumber='{0}' and url='{1}' ", number, urlData);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbMsgImp.Text = "已上传过该张图片！";
                return;
            }
            sql = string.Format(" insert into  T_SamplesFigure values('{0}','{1}','{2}')", number, urlData, strFileName);
            lbMsgImp.Text = SqlHelper.ExecuteSql(sql, ref error) ? "导入成功" : "导入失败！原因：" + error;
            BindRp();
        }

        protected void lbNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRp();
        }
        private void BindRp()
        {
            string number = lbNumber.SelectedValue;
            txtNumberImp.Text = number;
            string sql = string.Format("select * from T_SamplesFigure where CustomerProductNumber='{0}' and isnull(url,'')!='' ", number);
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();

        }


    }
}
