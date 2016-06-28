using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using DAL;
using Rapid.ToolCode;
using System.Data;

namespace Rapid.SellManager
{
    public partial class DeliveryBillListDetail : System.Web.UI.Page
    {
        public static string hasEdit = "inline";
        public static string hasDelete = "inline";
        public static string isConfirm = "none";
        public static string number = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                spPrint.Visible = ToolCode.Tool.GetUserMenuFunc("L0104", "Print");
                hasEdit = ToolCode.Tool.GetUserMenuFuncStr("L0104", "Edit");
                hasDelete = ToolCode.Tool.GetUserMenuFuncStr("L0104", "Delete");
                if (!ToolManager.CheckQueryString("id"))
                {
                    Response.Write("未知送货单");
                    Response.End();
                    return;
                }

                Bind();
            }
        }

        private void Bind()
        {
            string sql = string.Empty;
            string error = string.Empty;
            number = ToolManager.GetQueryString("id");
            if (ToolManager.CheckQueryString("DeliveryNumber"))
            {
                string deliveryNumber = ToolManager.GetQueryString("DeliveryNumber");
                string ordersnumber = ToolManager.GetQueryString("OrdersNumber");
                string prodeuctnumber = ToolManager.GetQueryString("ProductNumber");
                string customerpn = ToolManager.GetQueryString("CustomerProductNumber");
                string version = ToolManager.GetQueryString("Version");
                string rownumber = ToolManager.GetQueryString("RowNumber");

                sql = string.Format(@"delete DeliveryNoteDetailed  where DeliveryNumber='{0}' and OrdersNumber='{1}' and ProductNumber='{2}'
and CustomerProductNumber='{3}' and Version='{4}' and RowNumber='{5}'", deliveryNumber, ordersnumber, prodeuctnumber,
                                                              customerpn, version, rownumber);
              
                bool result = SqlHelper.ExecuteSql(sql, ref error);
                if (result)
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除送货单" + deliveryNumber, "删除成功");
                    Response.Write("1");
                    Response.End();
                    return;

                }
                else
                {
                    Tool.WriteLog(Tool.LogType.Operating, "删除送货单" + deliveryNumber, "删除失败！原因" + error);
                    Response.Write(error);
                    Response.End();
                    return;
                }
            }
            sql = string.Format(@"select DeliveryDate,pu.USER_NAME from DeliveryBill db left join PM_USER pu on db.DeliveryPerson=pu.USER_ID where DeliveryNumber='{0}' ", ToolManager.GetQueryString("id"));
            DataTable dt = SqlHelper.GetTable(sql);
            lblDeliveryDate.Text = dt.Rows[0]["DeliveryDate"].ToString();
            lblDeliveryPerson.Text = dt.Rows[0]["USER_NAME"].ToString();
            sql = string.Format(" select IsConfirm  from DeliveryBill where DeliveryNumber='{0}' ", ToolManager.GetQueryString("id"));
            isConfirm=SqlHelper .GetScalar (sql).Equals ("已确认")?"none":"inline";
            sql = string.Format(@" 
select cast(ROW_NUMBER () over(order by tpi.ProjectName asc) as varchar(10)) as num,so.CustomerOrderNumber,
dnd.DeliveryNumber,dnd.OrdersNumber,dnd.ProductNumber,dnd.CustomerProductNumber,
dnd.Version,dnd.RowNumber,dnd.LeadTime,dnd.SN,dnd.MaterialDescription,
cast(dnd.DeliveryQty as varchar(10)) as DeliveryQty,
cast(dnd.ArriveQty as varchar(10)) as ArriveQty,cast(dnd.ConformenceQty as varchar(10)) as ConformenceQty,
dnd.NGReason,cast(dnd.PassQty as varchar(10)) as PassQty,cast(dnd.NgQty as varchar(10)) as NgQty,
dnd.InspectorNGReason,dnd.InspectorNGReason,dnd.RoughCastingCode,dnd.ImportPartsCode,
cast(dnd.IsGeneratingCope as varchar(10)) as IsGeneratingCope,
dnd.ProjectName,dnd.Remark,ISNULL ( tpi.ProjectName,'') as projectName_New
 from DeliveryNoteDetailed dnd left join T_ProjectInfo tpi on dnd.ProductNumber=tpi.ProductNumber 
 and dnd.Version =tpi.Version 
inner join SaleOder so on dnd.OrdersNumber =so.OdersNumber where dnd.DeliveryNumber='{0}'
union all
select '合计','',
'','','','',
'','','','','',
sum(dnd.DeliveryQty) as DeliveryQty,
sum(dnd.ArriveQty ) as ArriveQty,sum(dnd.ConformenceQty) as ConformenceQty,
'',sum(dnd.PassQty ) as PassQty,sum(dnd.NgQty ) as NgQty,
'','','','','',
'','',''
 from DeliveryNoteDetailed dnd left join T_ProjectInfo tpi on dnd.ProductNumber=tpi.ProductNumber 
 and dnd.Version =tpi.Version 
inner join SaleOder so on dnd.OrdersNumber =so.OdersNumber
where dnd.DeliveryNumber='{0}' ", ToolManager.GetQueryString("id"));
           
            this.rpList.DataSource = SqlHelper.GetTable(sql, ref error);
            this.rpList.DataBind();
            hdnumber.Value = ToolManager.GetQueryString("id");
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void btnE_Click(object sender, EventArgs e)
        {
            if (ToolManager.CheckQueryString("id"))
            {
                string error = string.Empty;
                string deliveryNumber = ToolManager.GetQueryString("id");
                string sql = string.Format(@"update DeliveryNoteDetailed set ArriveQty=DeliveryQty,ConformenceQty=DeliveryQty where  DeliveryNumber='{0}'",deliveryNumber);
                if (SqlHelper.ExecuteSql(sql, ref error))
                {
                    Bind();
                }
                else
                {
                    lblResult.Text = "数量填充失败！" + error;
                    return;
                }

            }
        }

        protected void btnEmp_Click(object sender, EventArgs e)
        {
            string sql = string.Format(@"
 select 
 ROW_NUMBER () over(order by dnd.sn asc) as 行号 ,
dnd.CustomerProductNumber as 物料编码,
 dnd.Version as 版本号,
 dnd.MaterialDescription as 物料描述,
so.CustomerOrderNumber as 采购单,
 dnd.RowNumber as 采购单行,
  ISNULL ( tpi.ProjectName,'') as 项目名称,
  dnd.DeliveryQty as 发货数量,
 dnd.DeliveryNumber as 送货单号,
 dnd.ArriveQty as 实到数量,
 dnd.ConformenceQty as 实收数量,
 dnd.NGReason as 拒收原因,
 dnd.PassQty as 合格品数量,
 dnd.NgQty as 拒收数量,
 dnd.InspectorNGReason as 拒收原因,
 dnd.RoughCastingCode as 铸件毛坯编码仅适用铸件,
 dnd.ImportPartsCode as 进口件编码仅适用进口件
 from DeliveryNoteDetailed dnd left join T_ProjectInfo tpi on dnd.ProductNumber=tpi.ProductNumber 
 and dnd.Version =tpi.Version 
inner join SaleOder so on dnd.OrdersNumber =so.OdersNumber  where dnd.DeliveryNumber='{0}' ", ToolManager.GetQueryString("id"));
            ToolCode.Tool.ExpExcel(sql, "收货单明细报表");
        }
    }
}
