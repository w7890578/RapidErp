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
    public partial class HalfProductWarehouseLogDetailList1 : System.Web.UI.Page
    {
        public static string titleName = string.Empty;
        public static string showEdit = "inline";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
                string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
                string sql = string.Format(@"
select ISNULL (CheckTime ,'') from HalfProductWarehouseLog where WarehouseNumber='{0}'", warehouseNumber);
                if (!string.IsNullOrEmpty(SqlHelper.GetScalar(sql)))
                {
                    showEdit = "none";
                }
                else
                {
                    showEdit = "inline";
                }
                titleName = string.Format("半成品{0}明细{1}", Server.UrlDecode(ToolManager.GetQueryString("Type")), ToolManager.GetQueryString("WarehouseNumber"));
            }
        }
        private void Bind()
        {
            if (ToolManager.CheckQueryString("WarehouseNumber"))
            {
                string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");



                string sql = string.Format(@" select hpwld.*,isnull( vhq.StockQty,0) StockQty,pcp.CustomerProductNumber from HalfProductWarehouseLogDetail hpwld left join V_HalfProductStock_Qty vhq on hpwld.ProductNumber =vhq.ProductNumber
   and hpwld.Version =vhq.Version and  hpwld.MaterialNumber =vhq.MaterialNumber
inner join ProductCustomerProperty pcp on hpwld .ProductNumber =pcp.ProductNumber 
and pcp.Version =hpwld .Version 
   where hpwld.WarehouseNumber ='{0}' and hpwld.qty!=0", warehouseNumber);
                if (!txtPlanNumber.Text.Trim().Equals(""))
                {
                    sql += string.Format(" and hpwld.DocumentNumber='{0}' ", txtPlanNumber.Text.Trim());
                }
                if (!txtCustomerProductNumber.Text.Trim().Equals(""))
                {
                    sql += string.Format(" and pcp.CustomerProductNumber='{0}' ", txtCustomerProductNumber.Text.Trim());
                }
                if (!txtQLNumber.Text.Trim().Equals(""))
                {
                    sql += string.Format(" and hpwld.MaterialNumber='{0}' ", txtQLNumber.Text.Trim());
                }

                rpList.DataSource = SqlHelper.GetTable(sql);
                rpList.DataBind();
                return;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            List<string> sqls = new List<string>();
            string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            string sql = string.Format(@"
   update HalfProductWarehouseLog set CheckTime ='{0}' ,Auditor ='{1}' where WarehouseNumber='{2}'", timeNow, userId, warehouseNumber);
            sqls.Add(sql);
            sql = string.Format(" select * from  HalfProductWarehouseLogDetail where WarehouseNumber='{0}'", warehouseNumber);
            DataTable dt = SqlHelper.GetTable(sql);
            if (type.Equals("入库"))
            {
                foreach (DataRow dr in dt.Rows)
                {
                    sqls.Add(GetUpdateKCQtySql(dr["ProductNumber"].ToString(), dr["Version"].ToString(), dr["MaterialNumber"].ToString(), dr["Qty"].ToString()));
                }
            }
            else
            {
                sql = string.Format(@"
   select COUNT(*) from HalfProductWarehouseLogDetail hpwld inner join V_HalfProductStock_Qty vhq on hpwld.ProductNumber =vhq.ProductNumber
   and hpwld.Version =vhq.Version and  hpwld.MaterialNumber =vhq.MaterialNumber
   where vhq.StockQty-hpwld .Qty <0 and hpwld .WarehouseNumber ='{0}'", warehouseNumber);
                if (!SqlHelper.GetScalar(sql).Equals("0"))
                {
                    LbMsg.Text = string.Format("审核失败！原因：库存数量低，无法出库");
                    return;
                }
                foreach (DataRow dr in dt.Rows)
                {
                    sqls.Add(GetUpdateKCQtySql(dr["ProductNumber"].ToString(), dr["Version"].ToString(), dr["MaterialNumber"].ToString(), "-" + dr["Qty"].ToString()));
                }
            }
          //  sqls.Add(WrterLSZSql(warehouseNumber));
            bool result = SqlHelper.BatchExecuteSql(sqls, ref error);
            if (result)
            {
                Response.Redirect("HalfProductWarehouseLogList.aspx");
            }
            else
            {
                LbMsg.Text = "审核失败！原因：" + error;
            }

            return;
        }

        /// <summary>
        /// 获取更新半成品库存数量的sql语句
        /// </summary>
        private string GetUpdateKCQtySql(string productNumber, string version, string materialNumber, string qty)
        {
            string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = string.Format(@" select COUNT (*) from HalfProductStock where 
ProductNumber ='{0}' and Version ='{1}' and MaterialNumber='{2}'", productNumber, version, materialNumber);
            if (SqlHelper.GetScalar(sql).Equals("0"))
            {
                sql = string.Format(@" insert into HalfProductStock(ProductNumber ,Version ,MaterialNumber,WarehouseName,StockQty ,UpdateTime )
   values('{0}','{1}','{2}','{3}',{4},'{5}')", productNumber, version, materialNumber, "bcpk", qty, timeNow);
            }
            else
            {
                sql = string.Format(@" update HalfProductStock set StockQty =StockQty+({0}),
UpdateTime ='{1}' where ProductNumber ='{2}' and Version ='{3}' and MaterialNumber='{4}'", qty, timeNow, productNumber, version, materialNumber);
            }
            return sql;
        }

        private string WrterLSZSql(string warehouseNumber)
        {
            string type = Server.UrlDecode(ToolManager.GetQueryString("Type"));
            string sql = string.Empty;
            if (type.Equals("入库"))
            {
                sql = string.Format(@"insert into ProductWarehouseCurrentAccount(ProductNumber ,CustomerProductNumber ,Version ,MoveTimer ,
WarehouseNumber ,Income ,Balances ,HandledPerson ,MoveReasons ,OrdersNumber )
select hwld.ProductNumber  ,modt.CustomerProductNumber,hwld.Version, CONVERT(varchar(100), GETDATE(), 20)
 ,hwld.WarehouseNumber ,hwld.Qty ,vhpsq.StockQty+(hwld.Qty ),'{0}','{1}',DocumentNumber  from  HalfProductWarehouseLogDetail hwld 
  left  join MachineOderDetail modt on hwld.ProductNumber =modt.ProductNumber 
  left join V_HalfProductStock_Qty vhpsq on hwld.ProductNumber =vhpsq.ProductNumber and vhpsq.Version =hwld.Version 
  and vhpsq.MaterialNumber=hwld.MaterialNumber 
 and hwld.Version =modt.Version 
 where hwld.WarehouseNumber ='{2}'
", ToolCode.Tool.GetUser().UserName, "半成品入库", warehouseNumber);
            }
            else
            {
                sql = string.Format(@"insert into ProductWarehouseCurrentAccount(ProductNumber ,CustomerProductNumber ,Version ,MoveTimer ,
WarehouseNumber ,Issue  ,Balances ,HandledPerson ,MoveReasons ,OrdersNumber )select hwld.ProductNumber  ,modt.CustomerProductNumber,hwld.Version, CONVERT(varchar(100), GETDATE(), 20)
 ,hwld.WarehouseNumber ,hwld.Qty ,vhpsq.StockQty-(hwld.Qty ),'{0}','{1}',DocumentNumber  from  HalfProductWarehouseLogDetail hwld 
  left  join MachineOderDetail modt on hwld.ProductNumber =modt.ProductNumber 
  left join V_HalfProductStock_Qty vhpsq on hwld.ProductNumber =vhpsq.ProductNumber and vhpsq.Version =hwld.Version 
  and vhpsq.MaterialNumber=hwld.MaterialNumber 
 and hwld.Version =modt.Version 
 where hwld.WarehouseNumber ='{2}'
", ToolCode.Tool.GetUser().UserName, "半成品出库", warehouseNumber);
            }
            return sql;
        }
    }
}
