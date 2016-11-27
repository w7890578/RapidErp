using BLL;
using DAL;
using Rapid.ToolCode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.StoreroomManager
{
    public partial class MarerialLossLogDetailList : System.Web.UI.Page
    {
        public static string hasDelete = "inline";
        public static string hasEdit = "inline";
        public static string show = "inline";

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string error = string.Empty;
            string autior = ToolCode.Tool.GetUser().UserNumber;
            string warehouseNumber = ToolManager.GetQueryString("WarehouseNumber");
            bool result = Check(warehouseNumber, ref error);
            if (result)
            {
                Response.Redirect("MarerialWarehouseLogList.aspx");
            }
            else
            {
                lbMsg.Text = "审核失败，原因：" + error;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                spAdd.Visible = ToolCode.Tool.GetUserMenuFunc("L0404", "Add");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0404", "Delete");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0404", "Edit");
                if (!ToolManager.CheckQueryString("WarehouseNumber"))
                {
                    Response.Write("未知出入库编号");
                    Response.End();
                    return;
                }
                Bind();
                isExit(ToolManager.GetQueryString("WarehouseNumber"));
            }
        }

        private void Bind()
        {
            string sql = string.Empty;
            string error = string.Empty;
            string warehousenumber = ToolManager.GetQueryString("WarehouseNumber");
            string productnumber = ToolManager.GetQueryString("ProductNumber");
            string version = ToolManager.GetQueryString("Version");
            string materialnumber = ToolManager.GetQueryString("MaterialNumber");

            if (ToolManager.CheckQueryString("WarehouseNumber") && ToolManager.CheckQueryString("ProductNumber") && ToolManager.CheckQueryString("Version") && ToolManager.CheckQueryString("MaterialNumber"))
            {
                sql = string.Format(@" delete MarerialLossLog where WarehouseNumber ='{0}' and ProductNumber='{1}' and Version='{2}' and
                MaterialNumber='{3}'", warehousenumber, productnumber, version, materialnumber);
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除原材料损耗出库明细" + warehousenumber, "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;
                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除原材料损耗出库明细" + warehousenumber, "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }

            sql = string.Format(@"
                  select mll.WarehouseNumber as Warehousenumber, mll.ProductNumber as ProductNumber,mll.Version as Version,mll.MaterialNumber as MaterialNumber,
                  mt.MaterialName as MaterialName,mt.Description as Description,mll.LogDate as Date,
                  pmu.USER_NAME as TakeMaterialPerson,mll.Team as Team,mll.Qty as Qty,
                  wi.WarehouseName as MaterialPosition,mll.LossReason as LossReason,mll.Remark as Remark
                  ,ms.StockQty
                  from  MarerialLossLog mll left join MarerialInfoTable mt on mll.MaterialNumber=mt.MaterialNumber
                  left join PM_USER pmu on mll.TakeMaterialPerson=pmu.USER_ID left join WarehouseInfo wi on

                  mt.MaterialPosition=wi.WarehouseNumber
                  inner join MaterialStock ms on ms.MaterialNumber=mll.MaterialNumber
                  where mll.Warehousenumber='{0}'", warehousenumber);
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();
            hdnumber.Value = ToolManager.GetQueryString("WarehouseNumber");
        }

        private bool Check(string warehouseNumber, ref string error)
        {
            string userId = ToolCode.Tool.GetUser().UserNumber;
            string userName = ToolCode.Tool.GetUser().UserName;
            List<string> sqls = new List<string>();
            //            string sql = string.Format(@"select COUNT(*) from  MarerialLossLog mll left join V_MaterialStock_Qty vmsq on mll.MaterialNumber =
            //vmsq.MaterialNumber  where WarehouseNumber='{0}' and mll.Qty -vmsq.qty >0", warehouseNumber);
            //            if (!SqlHelper.GetScalar(sql).Equals("0"))
            //            {
            //                error = "当前库存数量低，无法出库！";
            //                return false;
            //            }
            string sql = string.Empty;
            sql = string.Format(@"select * from MarerialLossLog where WarehouseNumber='{0}'", warehouseNumber);
            DataTable dt = SqlHelper.GetTable(sql);
            foreach (DataRow dr in dt.Rows)
            {
                //                sql = string.Format(@"select COUNT(*) from MaterialStock where MaterialNumber='{0}' and WarehouseName='ycl'", warehouseNumber);
                //                if (SqlHelper.GetScalar(sql).Equals("0"))
                //                {
                //                    sql = string.Format(@"insert into MaterialStock (MaterialNumber ,StockQty ,UpdateTime ,WarehouseName )
                //values('{0}',{1},CONVERT(varchar(100),GETDATE (),20),'ycl')", dr["MaterialNumber"], dr["Qty"]);
                //                    sqls.Add(sql);
                //                }
                //                else
                //                {
                sql = string.Format(@"update MaterialStock set StockQty =StockQty-abs({1}),updatetime='{2}'
where MaterialNumber ='{0}' and WarehouseName='ycl'", dr["MaterialNumber"], dr["Qty"], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sqls.Add(sql);
                //}
            }
            sql = string.Format(@"
 update MarerialWarehouseLog set CheckTime =CONVERT (varchar(100),GETDATE (),20) ,Auditor ='{0}'
 where WarehouseNumber='{1}'", userId, warehouseNumber);
            sqls.Add(sql);
            //更新瞬时库存
            sql = string.Format(@" update MarerialLossLog set InventoryQty =vmsq.qty  from MarerialLossLog mll left join V_MaterialStock_Qty vmsq
 on mll.MaterialNumber =vmsq.MaterialNumber
 where mll.WarehouseNumber ='{0}' ", warehouseNumber);
            sqls.Add(sql);
            //写入流水账
            //            sql = string.Format(@"
            //insert into MateialWarehouseCurrentAccount(MaterialNumber ,CustomerMaterialNumber ,SupplierMaterialNumber,
            //MoveTime ,WarehouseNumber ,OrdersNumber
            //,Issue ,Balances ,HandledPerson ,MoveReasons)
            //select ml.MaterialNumber,bom.CustomerMaterialNumber ,'',CONVERT (varchar(100),GETDATE (),20),WarehouseNumber,'',Qty ,InventoryQty ,'{0}'
            //,'{1}' from MarerialLossLog ml left join BOMInfo bom on ml.ProductNumber =bom.ProductNumber and ml.Version =bom.Version
            //and ml.MaterialNumber =bom.MaterialNumber where ml.WarehouseNumber ='{2}'", userName, "损耗出库", warehouseNumber);

            sql = string.Format(@"
 insert into MateialWarehouseCurrentAccount(MaterialNumber ,CustomerMaterialNumber ,SupplierMaterialNumber,
MoveTime ,WarehouseNumber ,OrdersNumber,Issue ,Balances ,HandledPerson ,MoveReasons)
 select ml.MaterialNumber,'' ,'',CONVERT (varchar(100),GETDATE (),20),WarehouseNumber,'',ml.Qty ,ISNULL( vm.qty,0) ,'{0}'
,'{1}' from MarerialLossLog ml
left join V_MaterialStock_Qty vm on vm.MaterialNumber=ml.MaterialNumber
where ml.WarehouseNumber ='{2}'
", userName, "损耗出库", warehouseNumber);
            sqls.Add(sql);
            return SqlHelper.BatchExecuteSql(sqls, ref error);
        }

        /// <summary>
        /// 检测是否有审核时间
        /// </summary>
        /// <param name="warehousenumber"></param>
        private void isExit(string warehousenumber)
        {
            string sql = string.Empty;
            sql = string.Format(" select CheckTime from MarerialWarehouseLog where WarehouseNumber='{0}'", warehousenumber);
            string checktime = SqlHelper.GetScalar(sql);
            if (checktime == "" || checktime == null)
            {
                show = "inline";
            }
            else
            {
                show = "none";
            }
        }
    }
}