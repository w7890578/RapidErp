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
    public partial class AddOrEditProductCuttingLineInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version") && ToolManager.CheckQueryString("MaterialNumber"))
                {
                    string ProductNumber = ToolManager.GetQueryString("ProductNumber");
                    string Version = ToolManager.GetQueryString("Version");
                    string MaterialNumber = ToolManager.GetQueryString("MaterialNumber");
                    string Length = ToolManager.GetQueryString("Length");
                    string error = string.Empty;
                    string sql = string.Format(@" select * from ProductCuttingLineInfo where ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}' and Length='{3}'", ProductNumber, Version, MaterialNumber, Length);
                    if (!ToolManager.CheckQueryString("Length"))
                    {
                        this.btnSubmit.Text = "添加";
                        this.lbLength.Visible = false;
                    }
                    else
                    {
                        this.btnSubmit.Text = "修改";
                        this.txtLength.Visible = false;
                        DataTable dt = SqlHelper.GetTable(sql, ref error);
                        if (dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            lbProductNumber.Text = dr["ProductNumber"] == null ? "" : dr["ProductNumber"].ToString();
                            lbVersion.Text = dr["Version"] == null ? "" : dr["Version"].ToString();
                            lbMaterialNumber.Text = dr["MaterialNumber"] == null ? "" : dr["MaterialNumber"].ToString();
                            lbLength.Text = dr["Length"] == null ? "" : dr["Length"].ToString();
                            txtQty.Text = dr["Qty"] == null ? "" : dr["Qty"].ToString();
                            txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                            this.btnSubmit.Text = "修改";
                        }
                    }

                    this.lbProductNumber.Text = ProductNumber;
                    this.lbVersion.Text = Version;
                    this.lbMaterialNumber.Text = MaterialNumber;
                }
                else
                {
                    Response.Write("请选择BOM信息表的产成品编号、版本、原材料编号！");
                    Response.End();
                    return;
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(lbProductNumber.Text) || string.IsNullOrEmpty(lbVersion.Text) || string.IsNullOrEmpty(lbMaterialNumber.Text))
            {
                Response.Write("请选择BOM信息表的产成品编号、版本、原材料编号！");
                Response.End();
                return;
            }
            string ProductNumber = this.lbProductNumber.Text.Trim();
            string Version = this.lbVersion.Text.Trim();
            string MaterialNumber = this.lbMaterialNumber.Text.Trim();
            string Length = this.txtLength.Text.Trim();
            string Qty = this.txtQty.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string sql = string.Empty;
            if (this.btnSubmit.Text.Equals("添加"))
            {
                sql = string.Format(@" select * from ProductCuttingLineInfo where ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}' and Length='{3}'", ProductNumber, Version, MaterialNumber, Length);
                if (SqlHelper.GetTable(sql, ref error).Rows.Count > 0)
                {
                    lbSubmit.Text = "该长度在裁线信息维护表中已存在！请重新填写！";
                    return;
                }
                List<string> list = new List<string>();
                //添加裁线信息
                sql = string.Format(@" insert into ProductCuttingLineInfo (ProductNumber,Version,MaterialNumber,Length,Qty,Remark )
 values('{0}','{1}','{2}',{3},{4},'{5}')", ProductNumber, Version, MaterialNumber, Length, Qty, Remark);
                list.Add(sql);
                //改变bom单机用量
                sql = new BLL.ToolChangeProduct().changeBomSingleDose(ProductNumber, Version, MaterialNumber);
                list.Add(sql);
                //改变产品成本价
                sql = new BLL.ToolChangeProduct().changeProductCostPrice(ProductNumber, Version);
                list.Add(sql);
                bool result = SqlHelper.BatchExecuteSql(list, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加裁线信息" + ProductNumber, "增加成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加裁线信息" + ProductNumber, "增加失败！原因：" + error);
                    return;
                }
            }
            else
            {
                List<string> sqls = new List<string>();
                sql = string.Format(@" select * from ProductCuttingLineInfo where ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}' and Length='{3}'",
                    ProductNumber, Version, MaterialNumber, this.lbLength.Text.Trim());
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该BOM信息列表的裁线信息已被删除，请刷新页面后进行添加！";
                    return;
                }
                sql = string.Format(@" select COUNT(*) from BOMInfo where ProductNumber='{0}' and Version='{1}' and MaterialNumber='{2}'", ProductNumber, Version, MaterialNumber);
                if (SqlHelper.GetTable(sql, ref error).Rows[0][0].ToString().Equals("0"))
                {
                    lbSubmit.Text = "该产成品编号、版本或原材料编号不存在，请重新填写！";
                    return;
                }
                //编辑裁线信息维护
                sql = string.Format(@" update ProductCuttingLineInfo set Qty ={0},Remark ='{1}'
                where ProductNumber='{2}' and Version='{3}' and MaterialNumber='{4}' and Length={5}",
                Qty, Remark, ProductNumber, Version, MaterialNumber, ToolManager.GetQueryString("Length"));
                sqls.Add(sql);
                //改变bom单机用量
                sql = new BLL.ToolChangeProduct().changeBomSingleDose(ProductNumber, Version, MaterialNumber);
                sqls.Add(sql);
                //改变产品成本价
                sql = new BLL.ToolChangeProduct().changeProductCostPrice(ProductNumber, Version);
                sqls.Add(sql);

                bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败！原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑裁线信息" + ProductNumber, "编辑成功");
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑裁线信息" + ProductNumber, "编辑失败！原因：" + error);
                    return;
                }
            }
        }
    }
}