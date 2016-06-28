using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using System.Data;

namespace Rapid.StoreroomManager
{
    public partial class MaterialSafetyStockCalculationReport : System.Web.UI.Page
    {
        #region 页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                Bind();
            }
        }
        #endregion

        #region 数据显示
        private void Bind()
        {
        
            rpList.DataSource = SqlHelper.GetTable(selectTJ());
            rpList.DataBind();
        }
        #endregion

        #region 查询按钮
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }
        #endregion

        #region 查询条件
        private string selectTJ()
        {
            string sql=string.Empty;
            string condition = " where 1=1";
            if (txtMaterialNumber.Text != "")
            {
                condition += " and t.原材料编号 like '%" + txtMaterialNumber.Text.Trim() + "%'";
            }
            if (txtCustomerMateialNumber.Text != "")
            {
                condition += " and t.客户物料编号 like '%" + txtCustomerMateialNumber.Text.Trim() + "%'";
            }
            if(txtxinghao.Text.Trim()!="")
            {
                condition += " and v.名称 like '%" + txtxinghao.Text.Trim() + "%'";
            }

            sql = GetSql(txtDate.Text.Trim()) + condition;
            return sql;
        #endregion
        }

        #region 查询结果
        private string  GetSql(string date)
        {
            string sql=string.Format (@"--计算原材料安全库存
select mi.MaterialNumber as 原材料编号,mcp.CustomerMaterialNumber as 客户物料编号,mi.StockSafeQty as 现有安全库存数,ISNULL ( a.一个月出库数,0) as 一个月出库数 ,
isnull(b.三个月出库数,0) as 三个月出库数,ISNULL ( c.六个月出库数,0) as 六个月出库数  from MarerialInfoTable mi left join (
select mwld.MaterialNumber ,SUM (Qty ) as 一个月出库数 from  MaterialWarehouseLogDetail mwld inner join MarerialWarehouseLog mwl on mwld.WarehouseNumber =
mwl.WarehouseNumber where ISNULL( mwl.CheckTime ,'')!='' and mwl.ChangeDirection='出库' and ( cast (mwld.CreateTime as datetime ) between
DATEADD ( MONTH ,-1,CAST ( '{0}' as datetime ))
and CAST ( '{0}' as datetime ))
group by  mwld.MaterialNumber
 )a  on mi.MaterialNumber =a.MaterialNumber 
left join (
select mwld.MaterialNumber ,SUM (Qty ) as 三个月出库数 from  MaterialWarehouseLogDetail mwld inner join MarerialWarehouseLog mwl on mwld.WarehouseNumber =
mwl.WarehouseNumber where ISNULL( mwl.CheckTime ,'')!='' and mwl.ChangeDirection='出库' and ( cast (mwld.CreateTime as datetime ) between
DATEADD ( MONTH ,-3,CAST ( '{0}' as datetime ))
and CAST ( '{0}' as datetime ))
group by  mwld.MaterialNumber
)b on mi.MaterialNumber =b.MaterialNumber 
  left join (
select mwld.MaterialNumber ,SUM (Qty ) as 六个月出库数 from  MaterialWarehouseLogDetail mwld inner join MarerialWarehouseLog mwl on mwld.WarehouseNumber =
mwl.WarehouseNumber where ISNULL( mwl.CheckTime ,'')!='' and mwl.ChangeDirection='出库' and ( cast (mwld.CreateTime as datetime ) between
DATEADD ( MONTH ,-6,CAST ( '{0}' as datetime ))
and CAST ( '{0}' as datetime ))
group by  mwld.MaterialNumber
)c on mi.MaterialNumber =c.MaterialNumber 
inner join MaterialCustomerProperty mcp on mi.MaterialNumber=mcp.MaterialNumber ", date );
            sql = string.Format(" select t.*,v.名称 from ({0})t inner join V_MarerialInfoTable v on t.原材料编号=v.原材料编号", sql);
            return sql;
        }
        #endregion

        #region 导出
        protected void btnEmp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectTJ()))
            {
                return;
            }
            ToolCode.Tool.ExpExcel(selectTJ(), "原材料安全库存计算报表");
        }
        #endregion
    }
}
