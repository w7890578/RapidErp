using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace Rapid.StoreroomManager
{
    public partial class AddOrEditPackagingProcessInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string productType = drpProductTYpe.SelectedValue;
            string workSnNumber = txtWorkSnNumber.Text.Trim();
            string workSnName = txtWorkSnName.Text.Trim();
            string sn = txtSn.Text.Trim();
            string error = string.Empty;
            string sql = string.Format(" select COUNT(*) from T_PackagingProcessInfo where ProductType='{0}' and worksnNumber='{1}' "
                , productType, workSnNumber);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = "该成品类别下已存相同工序编号，请重新填写工序编号！";
                return;
            }
            sql = string.Format(" select COUNT(*) from T_PackagingProcessInfo where ProductType='{0}' and sn={1} "
                , productType, sn);
            if (!SqlHelper.GetScalar(sql).Equals("0"))
            {
                lbSubmit.Text = "该成品类别下已存相同序号,请重新填写序号！";
                return;
            }
            sql = string.Format(@" insert into T_PackagingProcessInfo (ProductType,WorkSnNumber,WorkSnName,Sn)
values('{0}','{1}','{2}',{3}) ", productType, workSnNumber, workSnName, sn);
            bool result = SqlHelper.ExecuteSql(sql, ref error);
            if (result)
            {
                lbSubmit.Text = "添加成功";
                txtWorkSnName.Text = "";
                txtWorkSnNumber.Text = "";
                return;
            }
            else
            {
                lbSubmit.Text = "添加失败！原因：" + error;
                return;
            }
        }
    }
}
