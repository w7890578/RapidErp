using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.ProduceManager
{
    public partial class AddOrEditBOMInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version"))
                {
                    string ProductNumber = ToolManager.GetQueryString("ProductNumber");
                    string Version = ToolManager.GetQueryString("Version");
                    string MaterialNumber = Server.UrlDecode(ToolManager.GetQueryString("MaterialNumber"));
                    string error = string.Empty;
                    string sql = string.Format(@" select * from BOMInfo where ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}'", ProductNumber, Version, MaterialNumber);
                    if (!ToolManager.CheckQueryString("MaterialNumber"))
                    {
                        this.btnSubmit.Text = "添加";
                        //this.lbMaterialNumber.Visible = false;
                    }
                    else
                    {
                        this.btnSubmit.Text = "修改";
                        this.txtMaterialNumber.Visible = false;
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            lbProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                            lbVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                            //lbMaterialNumber.Text = dr["MaterialNumber"] == null ? "" : dr["MaterialNumber"].ToString();

                            txtSingleDose.Text = dr["SingleDose"] == null ? "" : dr["SingleDose"].ToString();
                            txtUnit.Text = dr["Unit"] == null ? "" : dr["Unit"].ToString();
                            txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                            this.btnSubmit.Text = "修改";
                            // txtMaterialNumberNew.Visible = false;
                            //txtMaterialNumberNew.Text =
                        }
                    }

                    this.lbProductNumber.Text = ProductNumber;
                    this.lbVersion.Text = Version;
                }
                else
                {
                    Response.Write("请选择产成品编号、版本！");
                    Response.End();
                    return;
                }
            }
            if (ToolManager.CheckQueryString("m"))
            {
                string result = string.Empty;
                string sql = string.Format(@" select top 20  MaterialNumber,MaterialName from MarerialInfoTable ");
                if (ToolManager.CheckQueryString("contion"))
                {
                    sql += string.Format(@"  where
MaterialNumber like '%{0}%' or MaterialName like '%{0}%' 
order by MaterialNumber asc", ToolManager.GetQueryString("contion"));
                }
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        result += string.Format(" <tr><td>{0}</td><td>{1}</td></tr>", dr["MaterialName"], dr["MaterialNumber"]);
                    }
                }
                Response.Write(result);
                Response.End();
                return;
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            string error = string.Empty;
            if (string.IsNullOrEmpty(lbProductNumber.Text) || string.IsNullOrEmpty(lbVersion.Text))
            {
                Response.Write("请选择产成品编号、版本！！");
                Response.End();
            }
            string ProductNumber = this.lbProductNumber.Text.Trim();
            string Version = this.lbVersion.Text.Trim();
            string MaterialNumber = string.Empty;

            string SingleDose = this.txtSingleDose.Text.Trim();
            string Unit = this.txtUnit.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string sql = string.Empty;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                string customerMaterialNumber = txtCustomerMaterialNumber.Text;
                sql = string.Format(@" select * from MaterialCustomerProperty where CustomerMaterialNumber ='{0}'", customerMaterialNumber);

                DataTable dtNew = SqlHelper.GetTable(sql);
                if (dtNew.Rows.Count <= 0)
                {
                    lbSubmit.Text = "系统不存在该客户物料编号！请重新填写！";
                    return;
                }
                MaterialNumber = dtNew.Rows[0]["MaterialNumber"].ToString();
                if (Unit.Equals("mm"))
                {
                    SingleDose = (Convert.ToDouble(SingleDose) / 1000.00).ToString();
                }

                //sql = string.Format(@" select * from BOMInfo where ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}' and SingleDose='{3}'", ProductNumber, Version, MaterialNumber, SingleDose);
                //if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                //{
                //    lbSubmit.Text = "已存在相同记录！请重新填写！";
                //    return;
                //}
                DataTable dtNewTwo = SqlHelper.GetTable(string.Format(" select * from MarerialInfoTable where MaterialNumber='{0}' ", MaterialNumber));
                DataRow dr = dtNewTwo.Rows[0];
                if (dr["kind"].ToString().Equals("线材"))
                {
                    sql = string.Format(@"
insert into ProductCuttingLineInfo (ProductNumber ,Version ,MaterialNumber ,Length ,Qty )
values('{0}','{1}','{2}','{3}',1)", ProductNumber, Version, MaterialNumber, SingleDose);
                    list.Add(sql);
                }
                sql = string.Format(@" select top 1 CustomerProductNumber 
from ProductCustomerProperty where ProductNumber ='{0}' and Version ='{1}'", ProductNumber, Version);
                string customerProductNumber = SqlHelper.GetScalar(sql);

                //添加bom信息
                sql = string.Format(@" insert into BOMInfo (ProductNumber,Version,MaterialNumber,CustomerMaterialNumber,SingleDose,Unit,Remark,CustomnerProductNumber )
 values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", ProductNumber, Version, MaterialNumber, customerMaterialNumber, SingleDose, Unit, Remark, customerProductNumber);
                list.Add(sql);
                ////改变产品成本价
                //sql = new BLL.ToolChangeProduct().changeProductCostPrice(ProductNumber, Version);
                //list.Add(sql);

                //                //更新客户物料编号和客户产成品编号
                //                sql = string.Format(@"
                //update BOMInfo set CustomerMaterialNumber ='{0}',CustomnerProductNumber=(
                //select top 1 CustomerProductNumber from ProductCustomerProperty where ProductNumber ='{1}' and Version ='{2}' )"
                //  , customerMaterialNumber, ProductNumber, Version);
                //                list.Add(sql);

                bool result = SqlHelper.BatchExecuteSql(list, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                string tempError = string.Empty;
                //更新成本价
                SqlHelper.ExecuteSql(" exec P_UpdateProductCostPrice", ref tempError);

                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加BOM信息" + MaterialNumber, "增加成功");
                }
                else
                {

                    Tool.WriteLog(Tool.LogType.Operating, "增加BOM信息" + MaterialNumber, "增加失败！原因：" + error);
                }
                // new BLL.ToolChangeProduct().changeproduct(ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"));
                return;
            }
            else
            {
                //                //MaterialNumber = lbMaterialNumber.Text;
                //                //sql = string.Format(@" select * from BOMInfo where ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}'",
                //                //    ProductNumber, Version, this.lbMaterialNumber.Text.Trim());
                //                //if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                //                //{
                //                //    lbSubmit.Text = "该产成品的BOM信息已被删除，请刷新页面后进行添加！";
                //                //    return;
                //                //}
                //                sql = string.Format(@" select COUNT(*) from Product where ProductNumber='{0}' and Version='{1}' ", ProductNumber, Version);
                //                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                //                {
                //                    lbSubmit.Text = "该产成品编号或版本不存在，请重新填写！";
                //                    return;
                //                }
                //                List<string> sqls = new List<string>();
                //                //编辑bom信息
                //                sql = string.Format(@"update BOMInfo set SingleDose='{0}',Unit='{1}',Remark ='{2}'
                //                where ProductNumber='{3}' and Version='{4}' and MaterialNumber='{5}'",
                //                SingleDose, Unit, Remark, ProductNumber, Version, ToolManager.GetQueryString("MaterialNumber"));
                //                sqls.Add(sql);
                //                sql = new BLL.ToolChangeProduct().changeProductCostPrice(ProductNumber, Version);
                //                sqls.Add(sql);
                //                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                //                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                //                if (result)
                //                {
                //                    Tool.WriteLog(Tool.LogType.Operating, "编辑BOM信息" + ToolManager.GetQueryString("MaterialNumber"), "编辑成功");
                //                }
                //                else
                //                {

                //                    Tool.WriteLog(Tool.LogType.Operating, "编辑BOM信息" + ToolManager.GetQueryString("MaterialNumber"), "编辑失败！原因：" + error);
                //                }

                //                new BLL.ToolChangeProduct().changeproduct(ToolManager.GetQueryString("ProductNumber"), ToolManager.GetQueryString("Version"));
                //                return;
            }
        }

        private bool IsExitMareilNumber(string number)
        {
            string sql = string.Format(" select COUNT(*) from MarerialInfoTable where MaterialNumber ='{0}' ", number);
            return SqlHelper.GetScalar(sql).Equals("0") ? false : true;
        }

    }
}
