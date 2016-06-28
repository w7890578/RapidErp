using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DAL;
using System.Web.UI.WebControls;

namespace BLL 
{
    /// <summary>
    /// 控件内容绑定通用类
    /// </summary>
    public static class ControlBindManager
    {
        #region 下拉框获取内容通用函数
        /// <summary>
        /// 下拉框通用获取内容函数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="keyCloumName">key字段名</param>
        /// <param name="valueCloumName">value字段名</param>
        /// <returns></returns>
        public static string GetOption(string sql, string keyCloumName, string valueCloumName)
        {
            string error = string.Empty;
            string result = "<option value =\"\">- - - - - 请 选 择 - - - - -</option>";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            foreach (DataRow dr in dt.Rows)
            {
                result += string.Format(" <option value ='{0}'>{1}</option> ", dr[keyCloumName], dr[valueCloumName]);
            }
            return result;
        }

        public static void BindDrp(string sql, DropDownList drp, string valueName, string textName)
        {
            drp.Items.Clear();
            string error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                drp.DataSource = dt;
                drp.DataValueField = valueName;
                drp.DataTextField = textName;
                drp.DataBind();
            }
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));


        }
        /// <summary>
        /// 绑定ListBox并设置为可多选
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="lb">控件</param>
        /// <param name="valueName">值字段</param>
        /// <param name="textName">文本字段</param>
        public static void BindListBox(string sql, ListBox lb, string valueName, string textName)
        {
            lb.Items.Clear();
            string error = string.Empty;
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            if (dt.Rows.Count > 0)
            {
                lb.DataSource = dt;
                lb.DataValueField = valueName;
                lb.DataTextField = textName;
                lb.DataBind();
            }
            lb.SelectionMode = ListSelectionMode.Multiple;
        }
        #endregion

        #region 绑定原材料种类
        public static void MarerialKind(DropDownList drp)
        {
            string error = string.Empty;
            string sql = "select Kind from MarerialKind";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            drp.DataSource = dt;
            drp.DataValueField = "Kind";
            drp.DataTextField = "Kind";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));
        }
        #endregion

        #region 绑定原材料类别
        public static void MareriaType(DropDownList drp)
        {
            string error = string.Empty;
            string sql = "select Type from MareriaType";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            drp.DataSource = dt;
            drp.DataValueField = "Type";
            drp.DataTextField = "Type";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));

        }
        #endregion

        #region 绑定客户
        public static void BindCustomer(DropDownList drp)
        {
            string error = string.Empty;
            string sql = " select CustomerId ,CustomerName  from Customer";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            drp.DataSource = dt;
            drp.DataValueField = "CustomerId";
            drp.DataTextField = "CustomerName";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));
        }
        #endregion

        #region 客户编号
        public static string GetCustomerNumber()
        {
            return GetOption(" select  distinct (CustomerId) from Customer ", "CustomerId", "CustomerId");
        }
        #endregion

        #region 客户名称
        public static string GetCustomerName()
        {
            return GetOption(" select  distinct (CustomerName) from Customer ", "CustomerName", "CustomerName");
        }
        #endregion

        #region 客户开户银行
        public static string GetCustomerAccountBank()
        {
            return GetOption(" select distinct (AccountBank)   from Customer", "AccountBank", "AccountBank");
        }
        #endregion

        #region 获取收款方式
        public static string GetReceivablesMode()
        {
            return GetOption("select Id,MakeCollectionsMode from MakeCollectionsMode", "Id", "MakeCollectionsMode");
        }
        #endregion

        #region 送货单号
        public static string GetDeliveryNumber()
        {
            return GetOption("select DeliveryNumber from DeliveryBill", "DeliveryNumber", "DeliveryNumber");
        }
        #endregion

        #region 送货人
        public static string GetDeliveryPerson()
        {
            return GetOption("select distinct (DeliveryPerson) from DeliveryBill", "DeliveryPerson", "DeliveryPerson");
        }
        #endregion

        #region 获取用户姓名
        /// <summary>
        /// 获取用户姓名
        /// </summary>
        /// <param name="HasDisable">是否包含禁用的用户</param>
        /// <returns></returns>
        public static string GetUserName(bool HasDisable)
        {
            string sql = "select UserNumber,UserName from UserInfo ";
            if (!HasDisable)
            {
                sql += " where Isvisible=1";
            }
            return GetOption(sql, "UserNumber", "UserName");
        }
        #endregion

        #region 获取角色
        public static string GetRoleInfo()
        {
            return GetOption("select RoleNumber,RoleName from RoleInfo", "RoleNumber", "RoleName");
        }
        #endregion

        #region 获取职位
        public static string GetPost()
        {
            return GetOption("select  distinct( Post) from userInfo", "Post", "Post");
        }
        #endregion

        #region 绑定角色
        /// <summary>
        /// 绑定角色
        /// </summary>
        /// <param name="drp"></param>
        public static void BindRoleInfo(DropDownList drp)
        {
            string error = string.Empty;
            string sql = "select RoleNumber,RoleName from RoleInfo";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            drp.DataSource = dt;
            drp.DataValueField = "RoleNumber";
            drp.DataTextField = "RoleName";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));
        }

        #endregion

        #region 绑定供应商
        /// <summary>
        /// 绑定供应商
        /// </summary>
        /// <param name="drp"></param>
        public static void BindSupplier(DropDownList drp)
        {
            string error = string.Empty;
            string sql = " select SupplierId ,SupplierName  from SupplierInfo";
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            drp.DataSource = dt;
            drp.DataValueField = "SupplierId";
            drp.DataTextField = "SupplierName";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));
        }
        #endregion

        #region 产成品编号（原材料报废上报）
        public static string GetProductNumber()
        {
            return GetOption(" select  distinct (productnumber) from MarerialScrapLog ", "productnumber", "productnumber");
        }
        #endregion

        #region 原材料编号
        public static string GetMaterialNumber()
        {
            return GetOption(" select  distinct (MaterialNumber) from MarerialScrapLog ", "MaterialNumber", "MaterialNumber");
        }
        #endregion

        #region 开工单号
        public static string GetProductionOrderNumber()
        {
            return GetOption(" select  distinct (ProductionOrderNumber) from ProcessTestingTable ", "ProductionOrderNumber", "ProductionOrderNumber");
        }
        #endregion

        #region 产成品编号（过程检验）
        public static string GetProductId()
        {
            return GetOption(" select  distinct (ProductNumber) from ProcessTestingTable ", "ProductNumber", "ProductNumber");
        }
        #endregion

        #region 产成品类型（产成品）
        public static string GetProductType()
        {
            return GetOption(" select  distinct (类别) from V_Product  ", "类别", "类别");
        }
        #endregion

        #region 客户产成品编号
        public static string GetCustomerProductNumber()
        {
            return GetOption(" select  distinct (CustomerProductNumber) from ProcessTestingTable ", "CustomerProductNumber", "CustomerProductNumber");
        }
        #endregion



        #region  类别（产品基本信息表）
        public static string GetType()
        {
            return GetOption(" select  distinct (Type) from Product ", "Type", "Type");
        }
        #endregion

        #region  年度（员工考试成绩上报表）
        public static string GetYear()
        {
            return GetOption(" select  distinct (Year) from ExaminationLog ", "Year", "Year");
        }
        #endregion

        #region  月份（员工考试成绩上报表）
        public static string GetMonth()
        {
            return GetOption(" select  distinct (Month) from ExaminationLog ", "Month", "Month");
        }
        #endregion

        #region  姓名（员工考试成绩上报表）
        public static string GetName()
        {
            return GetOption(" select distinct(姓名) from V_ExaminationLog ", "姓名", "姓名");
        }
        #endregion

        #region  原材料种类(原材料信息表)
        public static string GetMaterialKind()
        {
            return GetOption(" select distinct(种类) from V_MarerialInfoTable ", "种类", "种类");
        }
        #endregion

        #region  原材料种类(原材料信息表)
        public static string GetMarerialType()
        {
            return GetOption(" select distinct(类别) from V_MarerialInfoTable ", "类别", "类别");
        }
        #endregion


        #region  原材料类别(原材料信息表)
        public static string GetMaterialType()
        {
            return GetOption(" select distinct( mt.Type) from MareriaType mt inner join  MarerialKind mk on mt.Pid=mk.Id ", "Type", "Type");
        }
        #endregion

        #region  原材料编号(供需平衡表)
        public static string GetMaterialNumberGong()
        {
            return GetOption(" select  distinct (MaterialNumber) from SupplyAndDemandBalance ", "MaterialNumber", "MaterialNumber");
        }
        #endregion

        #region 采购订单编号
        public static string GetOrdersNumber()
        {
            return GetOption("select OrdersNumber from CertificateOrders", "OrdersNumber", "OrdersNumber");
        }
        #endregion

        #region 绑定产品
        /// <summary>
        /// 绑定产品
        /// </summary>
        /// <param name="drp"></param>
        public static void BindProduct(DropDownList drp)
        {
            string error = string.Empty;
            string sql = string.Format(" select (ProductName+' 版本:'+[Version]) as text ,(ProductNumber+'|'+[Version])as value from Product");
            DataTable dt = SqlHelper.GetTable(sql, ref error);
            drp.DataSource = dt;
            drp.DataValueField = "value";
            drp.DataTextField = "text";
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("- - - - - 请 选 择 - - - - -", ""));


        }
        #endregion

        #region  包编码(包信息列表)
        public static string GetPackageNumber()
        {
            return GetOption(" select  distinct (PackageNumber) from PackageInfo ", "PackageNumber", "PackageNumber");
        }
        #endregion

        #region  包名称(包信息列表)
        public static string GetPackageName()
        {
            return GetOption(" select  distinct (PackageName) from PackageInfo ", "PackageName", "PackageName");
        }
        #endregion

        #region  仓库类型（出入库类型列表）
        public static string Gethousetype()
        {
            return GetOption(" select  distinct (WarehouseInOutType) from WarehouseInOutType ", "WarehouseInOutType", "WarehouseInOutType");
        }
        #endregion

        #region  变动方向（出入库类型列表）
        public static string GetChangeDirection()
        {
            return GetOption(" select  distinct (ChangeDirection) from WarehouseInOutType ", "ChangeDirection", "ChangeDirection");
        }
        #endregion

        #region  类型（出入库类型列表）
        public static string GetInOutType()
        {
            return GetOption(" select  distinct (InOutType) from WarehouseInOutType ", "InOutType", "InOutType");
        }
        #endregion

        #region  盘点编号（库存盘点）
        public static string GetInventoryNumber()
        {
            return GetOption(" select  distinct (InventoryNumber) from StockInventoryLog ", "InventoryNumber", "InventoryNumber");
        }
        #endregion

        #region  仓库名称（库存盘点）
        public static string GetWarehouseName()
        {
            return GetOption(" select distinct (WarehouseName) from WarehouseInfo", "WarehouseName", "WarehouseName");
        }
        #endregion

        #region  盘点类型（库存盘点）
        public static string GetInventoryType()
        {
            return GetOption(" select  distinct (InventoryType) from StockInventoryLog ", "InventoryType", "InventoryType");
        }
        #endregion

        #region 出入库编号（产成品出入库列表）
        public static string GetProductWarehouseLogWarehouseNumber()
        {
            return GetOption(" select  distinct (WarehouseNumber) from ProductWarehouseLog ", "WarehouseNumber", "WarehouseNumber");
        }
        #endregion

        #region 仓库名称（产成品出入库列表）
        public static string GetProductWarehouseLogWarehouseName()
        {
            return GetOption(" select distinct wi.WarehouseName as WarehouseName from ProductWarehouseLog pw left join WarehouseInfo wi on pw.WarehouseName=wi.WarehouseNumber ", "WarehouseName", "WarehouseName");
        }
        #endregion

        #region 变动方向（产成品出入库列表）
        public static string GetProductWarehouseLogChangeDirection()
        {
            return GetOption(" select  distinct (ChangeDirection) from ProductWarehouseLog ", "ChangeDirection", "ChangeDirection");
        }
        #endregion

        #region 出入库类型（产成品出入库列表）
        public static string GetProductWarehouseLogType()
        {
            return GetOption(" select  distinct (Type) from ProductWarehouseLog ", "Type", "Type");
        }
        #endregion

        #region 订单号（应付账款表）
        public static string GetPayOrdersNumber()
        {
            return GetOption("select distinct (OrdersNumber) from AccountsPayable", "OrdersNumber", "OrdersNumber");
        }
        #endregion

        #region 供应商名称（应付账款表）
        public static string GetPaySupplierName()
        {
            return GetOption("select distinct 供应商名称 from V_AccountsPayable", "供应商名称", "供应商名称");
        }
        #endregion



        #region 订单号（应收账款表）
        public static string GetReceivOrdersNumber()
        {
            return GetOption("select distinct (OrdersNumber) from AccountsReceivable", "OrdersNumber", "OrdersNumber");
        }
        #endregion

        #region 产成品编号（应收账款表）
        public static string GetReceivProductNumber()
        {
            return GetOption("select distinct (ProductNumber) from AccountsReceivable", "ProductNumber", "ProductNumber");
        }

        #endregion

        #region 客户编号（应收账款）
        public static string GetReceivaCustomerId()
        {
            return GetOption("select distinct 客户名称 from V_AccountsReceivable", "客户名称", "客户名称");
        }
        #endregion

        #region 客户名称（销售订单信息表）
        public static string GetSaleOderCustomer()
        {
            return GetOption(" select distinct 客户名称 from V_SaleOder ", "客户名称", "客户名称");
        }
        #endregion

        #region 开工单号（开工单总表）
        public static string GetPlanNumber()
        {
            return GetOption("select distinct 开工单号 from V_ProductPlan", "开工单号", "开工单号");
        }
        #endregion

        #region 制单人（开工单总表）
        public static string GetCreator()
        {
            return GetOption("select distinct 制单人 from V_ProductPlan", "制单人", "制单人");
        }
        #endregion

        #region 班组（开工单分表）
        public static string GetTeam()
        {
            return GetOption(" select distinct 班组 from V_ProductPlanSub", "班组", "班组");
        }
        #endregion

        #region 采购订单编号（采购订单主表）
        public static string GetOrdersNm()
        {
            return GetOption("select distinct OrdersNumber from V_CertificateOrders", "OrdersNumber", "OrdersNumber");
        }
        #endregion

        #region 付款方式（采购订单主表）
        public static string GetPaymentMode()
        {
            return GetOption("select distinct PaymentMode from V_CertificateOrders", "PaymentMode", "PaymentMode");
        }
        #endregion

        #region 供应商（采购订单主表）
        public static string GetSupplierN()
        {
            return GetOption("select distinct SupplierName from V_CertificateOrders", "SupplierName", "SupplierName");
        }
        #endregion

        #region 业务员（采购订单主表）
        public static string GetuserName()
        {
            return GetOption("select distinct USER_NAME from V_CertificateOrders", "USER_NAME", "USER_NAME");
        }
        #endregion


    }
}
