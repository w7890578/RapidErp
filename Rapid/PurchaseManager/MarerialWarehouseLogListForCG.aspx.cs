using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using Rapid.ToolCode;
using System.Data;
using DAL;

namespace Rapid.PurchaseManager
{
    public partial class MarerialWarehouseLogListForCG : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //确认
                if (ToolManager.CheckQueryString("IsDetermine"))
                {
                    string warehousenumber = ToolManager.GetQueryString("Warehousenumber");
                    Determine(warehousenumber);
                }
                //删除
                if (ToolManager.CheckQueryString("IsDelete"))
                {
                    string warehousenumber = ToolManager.GetQueryString("Warehousenumber");
                    Delete(warehousenumber);
                }
                this.labPage.Text = "1";
                this.contrlRepeater();
            }
        }
        private DataTable  Bind()
        {
            string sql = string.Format(@"
select * from V_MarerialWarehouseLogList_For_CG vm  
   where 1=1 ");
            

            if (!txtMateriNumber.Text.Trim().Equals(""))
            {
                sql += string.Format(@" and
 出入库编号 in (select 出入库编号 from V_Tool_MaterialWarehouseLogDetail
where  原材料编号 like '%{0}%')  ", txtMateriNumber.Text.Trim());
            }
            if (!txtSuppileNumber.Text.Trim().Equals(""))
            {
                sql += string.Format(@" and
 出入库编号 in (select 出入库编号 from V_Tool_MaterialWarehouseLogDetail
where  供应商物料编号 like '%{0}%')  ", txtSuppileNumber.Text.Trim());
                //sql += string.Format(" and mwld.供应商物料编号 like '%{0}%' ", txtSuppileNumber.Text.Trim());
            }
            if (!txtSuppName.Text.Trim().Equals(""))
            {
                //sql += string.Format(" and mwld.供应商名称 like '%{0}%' ", txtSuppName.Text.Trim());
                sql += string.Format(@" and
 出入库编号 in (select 出入库编号 from V_Tool_MaterialWarehouseLogDetail
where  供应商名称 like '%{0}%')  ", txtSuppName.Text.Trim());
            }

            // order by 制单时间 desc
           
//            if (!conditionTwo.Equals("mwl.Type in ('采购入库','采购退料出库')"))
//            {
//                sql += string.Format(@"where 出入库编号 in (
//select mwl.WarehouseNumber from V_Tool_MaterialWarehouseLogDetail mwld inner join MarerialWarehouseLog mwl on mwld.出入库编号 =mwl.WarehouseNumber
//where {0} )", conditionTwo);
//            }
//            else {
//                sql += " where 1=1";
//            }
            if (!txtDate.Text.Trim().Equals(""))
            {
                sql += string.Format(" and vm.制单时间 like '%{0}%' ", txtDate.Text.Trim());
            }
            if (!txtUser.Text.Trim().Equals(""))
            {
                sql += string.Format(" and vm.制单人 like '%{0}%' ", txtUser.Text.Trim());
            }
            sql += string.Format(" order by vm.制单时间 desc  ");
            return SqlHelper.GetTable(sql);
            //rpList.DataSource = SqlHelper.GetTable(sql);
            //rpList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.labPage.Text = "1";
            this.contrlRepeater();
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void Delete(string warehousenumber)
        {
            List<string> sqls = new List<string>();
            string error = string.Empty;
            string sql = string.Format(@"
delete MaterialWarehouseLogDetail where WarehouseNumber ='{0}'", warehousenumber);
            sqls.Add(sql);
            sql = string.Format(@" 
delete MarerialWarehouseLog   where WarehouseNumber='{0}' ", warehousenumber);
            sqls.Add(sql);
            Response.Write(SqlHelper.BatchExecuteSql(sqls, ref error) ? "1" : error);
            Response.End();
            return;
        }

        /// <summary>
        /// 确认
        /// </summary>
        private void Determine(string warehousenumber)
        {
            string error = string.Empty;
            string sql = string.Format(@" 
update MarerialWarehouseLog set IsConfirm ='是' where WarehouseNumber='{0}' ", warehousenumber);
            Response.Write(SqlHelper.ExecuteSql(sql, ref error) ? "1" : error);
            Response.End();
            return;
        }

        public void contrlRepeater()
        {
            DataTable dt = Bind();
            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dt.DefaultView;
            pds.AllowPaging = true;
            pds.PageSize = 35;
            pds.CurrentPageIndex = Convert.ToInt32(this.labPage.Text) - 1;
            rpList.DataSource = pds;
            LabCountPage.Text = pds.PageCount.ToString();
            labPage.Text = (pds.CurrentPageIndex + 1).ToString();
            this.lbtnpritPage.Enabled = true;
            this.lbtnFirstPage.Enabled = true;
            this.lbtnNextPage.Enabled = true;
            this.lbtnDownPage.Enabled = true;
            if (pds.CurrentPageIndex < 1)
            {
                this.lbtnpritPage.Enabled = false;
                this.lbtnFirstPage.Enabled = false;
            }
            if (pds.CurrentPageIndex == pds.PageCount - 1)
            {
                this.lbtnNextPage.Enabled = false;
                this.lbtnDownPage.Enabled = false;
            }
            rpList.DataBind();
            //ToolCode.Tool.MergeCells(Repeater1, "tdOrderNumber");
            //ToolCode.Tool.MergeCells(Repeater1, "tdCustomerOrderNumber");
        }
        protected void lbtnpritPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = Convert.ToString(Convert.ToInt32(labPage.Text) - 1);
            this.contrlRepeater();
        }
        protected void lbtnFirstPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = "1";
            this.contrlRepeater();
        }

        protected void lbtnDownPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = this.LabCountPage.Text;
            this.contrlRepeater();
        }

        protected void lbtnNextPage_Click(object sender, EventArgs e)
        {
            this.labPage.Text = Convert.ToString(Convert.ToInt32(labPage.Text) + 1);
            this.contrlRepeater();
        }
 

    }
}
