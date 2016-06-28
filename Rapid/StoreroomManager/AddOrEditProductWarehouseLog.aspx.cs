using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Rapid.ToolCode;

namespace Rapid.StoreroomManager
{
    public partial class AddOrEditProductWarehouseLog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.trCheckTime.Visible = false;
                this.trCreateTime.Visible = false;
                this.trAuditor.Visible = false;
                string WarehouseNumber = ToolManager.GetQueryString("WarehouseNumber");



                // ScarpWarehouseLogManager.BindWarehouseName(this.drpWarehouseName);
                ControlBindManager.BindDrp("select WarehouseNumber,WarehouseName from WarehouseInfo  where Type='产成品库'", this.drpWarehouseName, "WarehouseNumber", "WarehouseName");
                drpWarehouseName.SelectedValue = "cpk";
                string error = string.Empty;
                string sql = string.Format(@" select * from ProductWarehouseLog where WarehouseNumber='{0}' ", WarehouseNumber);
                if (!ToolManager.CheckQueryString("WarehouseNumber"))
                {
                    this.btnSubmit.Text = "添加";
                    this.lbWarehouseNumber.Visible = false;
                    trWarehouseNumber.Visible = false;

                    if(ToolManager .CheckQueryString ("IsXS"))
                    {
                        drpChangeDirection.Items.Clear ();
                        drpChangeDirection.Items .Add (new ListItem ("---请选择---",""));
                        drpChangeDirection.Items .Add (new ListItem ("出库","出库"));
                        drpChangeDirection .SelectedIndex =0;
                    }
                }
                else
                {
                    this.btnSubmit.Text = "修改";
                    DataTable dt = SqlHelper.GetTable(sql, ref error);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        this.txtWarehouseNumber.Text = dr["WarehouseNumber"] == null ? "" : dr["WarehouseNumber"].ToString();
                        this.drpWarehouseName.SelectedValue = dr["WarehouseName"] == null ? "" : dr["WarehouseName"].ToString();
                        this.drpChangeDirection.SelectedValue = dr["ChangeDirection"] == null ? "" : dr["ChangeDirection"].ToString();

                        this.drpType.Items.Clear();
                        this.drpType.DataSource = bind();
                        this.drpType.DataValueField = "InOutType";
                        this.drpType.DataTextField = "InOutType";
                        this.drpType.DataBind();
                        this.drpType.SelectedValue = dr["Type"] == null ? "" : dr["Type"].ToString();



                        this.txtCreator.Text = dr["Creator"] == null ? "" : dr["Creator"].ToString();
                        this.txtCreateTime.Text = dr["CreateTime"] == null ? "" : dr["CreateTime"].ToString();
                        this.txtCheckTime.Text = dr["CheckTime"] == null ? "" : dr["CheckTime"].ToString();
                        this.txtAuditor.Text = dr["Auditor"] == null ? "" : dr["Auditor"].ToString();
                        txtRemark.Text = dr["Remark"] == null ? "" : dr["Remark"].ToString();
                        this.btnSubmit.Text = "修改";
                        this.txtWarehouseNumber.Visible = false;
                        this.lbWarehouseNumber.Text = ToolManager.GetQueryString("WarehouseNumber");
                        trWarehouseNumber.Visible = true;
                    }

                }

            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string WarehouseNumber = string.Empty;
            string WarehouseName = this.drpWarehouseName.SelectedValue;
            string ChangeDirection = this.drpChangeDirection.SelectedValue;
            string Type = this.drpType.SelectedValue;
            string Creator = ToolCode.Tool.GetUser().UserNumber;
            string CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string CheckTime = this.txtCheckTime.Text.Trim();
            string Auditor = this.txtAuditor.Text.Trim();
            string Remark = txtRemark.Text.Trim();
            string sql = string.Empty;
            bool result = false;
            if (ChangeDirection.Equals("出库"))
            {
                WarehouseNumber = "CCPCK" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            else
            {
                WarehouseNumber = "CCPRK" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            if (this.btnSubmit.Text.Equals("添加"))
            {
                if(string .IsNullOrEmpty (ChangeDirection )||string.IsNullOrEmpty (Type ))
                {
                    lbSubmit.Text = "信息填写不完整";
                    return;
                }


                string isConfirm = ToolManager.CheckQueryString("IsXS") ? "否" : "是"; //如果是销售模块跳转过来的话,默认确认为否

                sql = string.Format(@" insert into ProductWarehouseLog (WarehouseNumber,WarehouseName,ChangeDirection,Type,Creator,CreateTime,CheckTime,Auditor,Remark,IsConfirm )
 values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", WarehouseNumber, WarehouseName, ChangeDirection, Type, Creator, CreateTime, CheckTime, Auditor, Remark, isConfirm);
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "添加成功" : "添加失败，原因：" + error;
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加产成品出入库信息" + WarehouseNumber, "增加成功");
                    //ToolCode.Tool.ResetControl(this.Controls);
                    Response.Write(ToolManager.GetClosePageJS());
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "增加产成品出入库信息" + WarehouseNumber, "增加失败！原因" + error);
                    return;
                }
            }
            else
            {
                sql = string.Format(@" select * from ProductWarehouseLog where WarehouseNumber='{0}' ", ToolManager.GetQueryString("WarehouseNumber"));
                if (SqlHelper.GetTable(sql, ref error).Rows.Count <= 0)
                {
                    lbSubmit.Text = "该产成品出入库信息已被删除";
                    return;
                }
                sql = string.Format(@" update ProductWarehouseLog set WarehouseName='{0}',ChangeDirection='{1}',Type='{2}',
                Creator='{3}',CreateTime='{4}',CheckTime='{5}',Auditor='{6}',Remark ='{7}'
                where WarehouseNumber='{8}' ", WarehouseName, ChangeDirection, Type, Creator, CreateTime, CheckTime, Auditor, Remark, ToolManager.GetQueryString("WarehouseNumber"));
                result = SqlHelper.ExecuteSql(sql, ref error);
                lbSubmit.Text = result == true ? "修改成功" : "修改失败，原因：" + error;
                if (result)
                {
                    //Response.Write("<script>window.close();</script>");
                    Tool.WriteLog(Tool.LogType.Operating, "编辑产成品出入库信息" + ToolManager.GetQueryString("WarehouseNumber"), "编辑成功");
                    //ToolCode.Tool.ResetControl(this.Controls);

                    Response.Write(ToolManager.GetClosePageJS());
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "编辑产成品出入库信息" + ToolManager.GetQueryString("WarehouseNumber"), "编辑失败！原因" + error);
                    return;
                }
            }
        }
        private DataTable bind()
        {
            string sql = string.Format(@" select InOutType from WarehouseInOutType where InOutType!='生产入库'
and InOutType!='盘盈入库' and InOutType!='盘亏出库'  and InOutType!='包装入库' and InOutType!='销售出库'
and WarehouseInOutType='产成品' and ChangeDirection='{0}'",
            this.drpChangeDirection.SelectedValue);

            if (ToolManager.CheckQueryString("IsXS"))
            {
                sql = " select '销售出库' as  InOutType";
            }

            DataTable dt = SqlHelper.GetTable(sql);

            return dt;

        }

        protected void drpChangeDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.drpChangeDirection.SelectedValue == "出库")
            {
                this.drpType.Items.Clear();
                this.drpType.DataSource = bind();
                this.drpType.DataValueField = "InOutType";
                this.drpType.DataTextField = "InOutType";
                this.drpType.DataBind();
            }
            else if (this.drpChangeDirection.SelectedValue == "入库")
            {
                this.drpType.Items.Clear();
                this.drpType.DataSource = bind();
                this.drpType.DataValueField = "InOutType";
                this.drpType.DataTextField = "InOutType";
                this.drpType.DataBind();

            }
            else
            {
                this.drpType.Items.Clear();
                //this.drpType.Items.Add("---请选择---");
            }
        }
    }
}
