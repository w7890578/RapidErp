﻿using BLL;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Rapid.SellManager
{
    public partial class PrepaidAdvanceApplication : System.Web.UI.Page
    {
        public static string isYs = "";

        protected void btnExpExcel_Click(object sender, EventArgs e)
        {
            ExcelHelper.Instance.ExpExcel(SqlHelper.GetTable(GetSql()), ToolManager.GetQueryString("ISYS").Equals("1") ? "预收账款" : "应收账款");
            //ToolCode.Tool.ExpExcel(GetSql(), ToolManager.GetQueryString("ISYS").Equals("1") ? "预收账款" : "应收账款");
        }

        protected void btnExpForSearch_Click(object sender, EventArgs e)
        {
            string sql = GetSql();
            DataTable dt = SqlHelper.GetTable(sql);

            ExpDetailForSearch(dt);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (ToolManager.GetQueryString("ISYS") == "1")
            {
                PrepaidAdvanceApplicationManager.Instance.ExpSalePreIncomeNoInvoice();
            }
            else
            {
                PrepaidAdvanceApplicationManager.Instance.ExpSaleShouldIncomeNoInvoice();
            }
            //            if (ToolManager.GetQueryString("ISYS") == "1")
            //            {
            //                string sql = @"
            //WITH B AS
            //  ( SELECT *
            //   FROM
            //     (SELECT va.*,
            //             ta.InvoiceNumber AS 发票号码,ta.InvoiceDate AS 开票日期
            //      FROM V_AccountsReceivableDetail va
            //      INNER JOIN T_AccountsReceivable_Detail ta ON va.guid=ta.guid)t) ,
            //      A AS
            //  ( SELECT t.销售订单号
            //   FROM
            //     (SELECT *
            //      FROM
            //        ( SELECT vt.*,
            //                 a.InvoiceNumber AS 发票号码,a.InvoiceDate AS 开票日期
            //         FROM V_T_AccountsReceivableMain_IsAdvance vt
            //         INNER JOIN AccountsReceivable a ON vt.guid=a.guid)t
            //      WHERE 是否为预收='是'
            //        AND 是否已审批='否'
            //        AND 是否已被申请='否'
            //        AND 是否结清='否')t
            //   GROUP BY t.销售订单号)
            //SELECT DISTINCT B.*
            //FROM A
            //INNER JOIN B ON A.销售订单号=B.销售订单号 WHERE ISNULL(B.发票号码,'')=''
            //";
            //                ExcelHelper.Instance.ExpExcel(SqlHelper.GetTable(sql), "未开票详细");
            //            }
            //            else
            //            {
            //                string customerName = txtCustomerName.Text.Trim();
            //                string sql = string.Format(@"select CustomerId from Customer where CustomerName like '%{0}%'", customerName);
            //                DataTable dt = SqlHelper.GetTable(sql);
            //                if (dt.Rows.Count == 0)
            //                {
            //                    lbMsg.InnerText = "客户名称不存在，请确认是否填写正确的客户名称";
            //                    return;
            //                }
            //                sql = string.Format(@"
            //select * from (
            //select distinct t.* from (
            //select va.*,ta.InvoiceNumber as 发票号码,ta.InvoiceDate as 开票日期
            //from V_AccountsReceivableDetail va
            //inner join T_AccountsReceivable_Detail
            //ta on va.guid=ta.guid)t
            //inner join (select CreateTime,DeliveryNumber from AccountsReceivable where  CustomerId in (select CustomerId from Customer where CustomerName like '%{0}%') and
            //  isnull(InvoiceDate,'')='')
            // a on t.创建时间=a.CreateTime and t.送货单号=a.DeliveryNumber
            //union all
            //select '','合计','','','','','','',SUM(交货数量),'',SUM(总价),'','','','','','' from V_AccountsReceivableDetail
            //h inner join (select CreateTime,DeliveryNumber from AccountsReceivable where   CustomerId in (select CustomerId from Customer where CustomerName like '%{0}%') and
            //  isnull(InvoiceDate,'')='')
            // a on h.创建时间=a.CreateTime and h.送货单号=a.DeliveryNumber) H  where ISNULL( h.开票日期,'')=''
            //", customerName);
            //                ExcelHelper.Instance.ExpExcel(SqlHelper.GetTable(sql), "未开票详细");
            //            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            isYs = ToolManager.GetQueryString("ISYS");
            if (isYs == "1")
            {
                PrepaidAdvanceApplicationManager.Instance.ExpSalePreIncome();
            }
            else
            {
                PrepaidAdvanceApplicationManager.Instance.ExpSaleShouldIncome();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (ToolManager.GetQueryString("ISYS").Equals("0"))
            //{
            //    Response.Redirect("PrepaidAdvanceApplicationForYS.aspx?ISYS=0");
            //}
            if (Request["istrace"] != null)
            {
                Page.Trace.IsEnabled = true;
            }
            if (!IsPostBack)
            {
                if (ToolManager.CheckQueryString("sq"))//选选择
                {
                    string error = string.Empty;
                    string guids = ToolManager.GetQueryString("sq");
                    string sql = string.Format(@"
update AccountsReceivable set IsApplicationed='是' where guid in ({0}) ", guids);
                    string result = SqlHelper.ExecuteSql(sql, ref error) ? "1" : error;
                    Response.Write(result);
                    Response.End();
                    return;
                }
                Bind();
            }
        }

        private void Bind()
        {
            //Button1.Visible = ToolManager.GetQueryString("ISYS") == "1" ? false : true;
            string sql = GetSql();
            // this.labPage.Text = "1";
            // contrlRepeater();
            rpList.DataSource = SqlHelper.GetTable(sql);
            rpList.DataBind();

            //Response.Write(sql);
        }

        private void ExpDetailForSearch(DataTable dt)
        {
            if (dt == null)
                return;
            HashSet<string> hashSetOrdersNumber = new HashSet<string>();
            HashSet<string> hashSetCreateTime = new HashSet<string>();
            HashSet<string> hashSetDeliveryNumber = new HashSet<string>();

            foreach (DataRow dr in dt.Rows)
            {
                hashSetOrdersNumber.Add(dr["销售订单号"].ToString());
                hashSetCreateTime.Add(dr["创建时间"].ToString());
                hashSetDeliveryNumber.Add(dr["送货单号"].ToString());

                //                if (isYs != "1")//应收
                //                {
                //                    sb.AppendFormat(" and 创建时间='{0}' and 送货单号='{1}' ", dr["创建时间"].ToString(), dr["送货单号"].ToString());
                //                }
                //                sb.AppendLine(" union all ");
            }
            //            sb.Append(@"select * from (
            //select va.*, ta.InvoiceNumber as 发票号码, ta.InvoiceDate as 开票日期 from V_AccountsReceivableDetail va
            //  inner join T_AccountsReceivable_Detail ta on va.guid = ta.guid
            //)t where 1!=1");
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(@"
                select * from (
                select va.*,ta.InvoiceNumber as 发票号码,ta.InvoiceDate as 开票日期 from V_AccountsReceivableDetail va
                inner join T_AccountsReceivable_Detail ta on va.guid=ta.guid
                )t where t.销售订单号 in ({0})
                 ", GetString(hashSetOrdersNumber));
            if (isYs != "1")//应收
            {
                sb.AppendFormat(" and 创建时间 in ({0}) and 送货单号 in ({1}) ", GetString(hashSetCreateTime), GetString(hashSetDeliveryNumber));
            }
            DataTable dtResult = SqlHelper.GetTable(sb.ToString());
            ExcelHelper.Instance.ExpExcel(dtResult, isYs.Equals("1") ? "预收账款明细" : "应收账款明细");
        }

        private string GetSql()
        {
            string sql = string.Empty;
            isYs = ToolManager.GetQueryString("ISYS");

            string condinton = " where 是否为预收='" + (isYs.Equals("1") ? "是" : "否") + "' and 是否已审批='否' and 是否已被申请='否'";
            if (txtOdersNumber.Text != "")
            {
                condinton += " and 销售订单号 like '%" + txtOdersNumber.Text.Trim() + "%'";
            }
            if (txtCustomerOdersNumber.Text != "")
            {
                condinton += " and 客户采购订单号 like '%" + txtCustomerOdersNumber.Text.Trim() + "%'";
            }
            if (drpType.SelectedValue != "")
            {
                condinton += " and 收款类型='" + drpType.SelectedValue + "'";
            }
            if (drpisSettle.SelectedValue != "")
            {
                condinton += " and 是否结清='" + drpisSettle.SelectedValue + "'";
            }
            if (txtCustomerName.Text != "")
            {
                condinton += " and 客户名称 like '%" + txtCustomerName.Text.Trim() + "%'";
            }
            if (txtInvoiceNumber.Text.Trim() != "")
            {
                condinton += " and 发票号码 like '%" + txtInvoiceNumber.Text.Trim() + "%' ";
            }

            if (txtDateStart.Text.Trim() != "" && txtDateEnd.Text.Trim() != "")
            {
                DateTime dateStart = Convert.ToDateTime(txtDateStart.Text.Trim());
                DateTime dateEnd = Convert.ToDateTime(txtDateEnd.Text.Trim());

                if (dateEnd > dateStart)
                {
                    condinton += string.Format(" and 创建时间 between '{0}' and '{1}' ", dateStart.ToString("yyyy-MM-dd HH:mm:ss"), dateEnd.ToString("yyyy-MM-dd HH:mm:ss"));
                }
                else if (dateEnd == dateStart)
                {
                    condinton += string.Format(" and 创建时间 between '{0}' and '{1}' ", dateStart.ToString("yyyy-MM-dd 00:00:00"), dateEnd.ToString("yyyy-MM-dd 23:59:59"));
                }
            }

            string viewName = ToolManager.GetQueryString("ISYS").Equals("1") ? "V_T_AccountsReceivableMain_IsAdvance" : "V_T_AccountsReceivableMain_NoAdvance";

            if (txtSHorderNumber.Text.Trim() != "")
            {
                if (ToolManager.GetQueryString("ISYS").Equals("1"))
                {
                    condinton += string.Format(@" and 销售订单号 in (select distinct OrdersNumber  from T_AccountsReceivable_Detail where OrdersNumber in (
select OrdersNumber from AccountsReceivable where
IsAdvance='是') and DeliveryNumber ='{0}') ", txtSHorderNumber.Text.Trim());

                    sql = string.Format(@"
select * from (
select vt.*,a.InvoiceNumber as 发票号码,a.InvoiceDate as 开票日期  from {1} vt
inner join AccountsReceivable a  on vt.guid=a.guid
)t {0}
union all
   select '合计','','',SUM(订单总价),'','','','',SUM(交货总价),'','','','','','','','','','','','','','','','' from (select * from (select vt.*,a.InvoiceNumber as 发票号码,a.InvoiceDate as 开票日期  from {1} vt
inner join AccountsReceivable a  on vt.guid=a.guid )t {0}) t {0}
order by 创建时间 desc
", condinton, viewName);
                }
                else
                {
                    sql = string.Format(@"
select * from (
select vt.*,a.InvoiceNumber as 发票号码,a.InvoiceDate as 开票日期  from {1} vt
inner join AccountsReceivable a  on vt.guid=a.guid
inner join (select a.OrdersNumber,a.CreateTime from T_AccountsReceivable_Detail a
inner join (select OrdersNumber,CreateTime from AccountsReceivable where
IsAdvance='否') b on a.OrdersNumber=b.OrdersNumber and a.CreateTime=b.CreateTime
and DeliveryNumber='{2}'
group by a.OrdersNumber,a.CreateTime) b
 on vt.销售订单号=b.OrdersNumber and vt.创建时间=b.CreateTime

)t {0}
union all
   select '合计','','',SUM(订单总价),'','','','',SUM(交货总价),'','','','','','','','','','','','','','','','' from (select * from (
select vt.*,a.InvoiceNumber as 发票号码,a.InvoiceDate as 开票日期  from {1}
vt
inner join AccountsReceivable a  on vt.guid=a.guid
inner join (select a.OrdersNumber,a.CreateTime from T_AccountsReceivable_Detail a
inner join (select OrdersNumber,CreateTime from AccountsReceivable where
IsAdvance='否') b on a.OrdersNumber=b.OrdersNumber and a.CreateTime=b.CreateTime
and DeliveryNumber='{2}'
group by a.OrdersNumber,a.CreateTime) b
 on vt.销售订单号=b.OrdersNumber and vt.创建时间=b.CreateTime
)t {0}) t {0}
order by 创建时间 desc
", condinton, viewName, txtSHorderNumber.Text.Trim());
                }
            }
            else
            {
                sql = string.Format(@"
select * from (
select vt.*,a.InvoiceNumber as 发票号码,a.InvoiceDate as 开票日期  from {1} vt
inner join AccountsReceivable a  on vt.guid=a.guid
)t {0}
union all
   select '合计','','',SUM(订单总价),'','','','',SUM(交货总价),'','','','','','','','','','','','','','','','' from (select * from (select vt.*,a.InvoiceNumber as 发票号码,a.InvoiceDate as 开票日期  from {1} vt
inner join AccountsReceivable a  on vt.guid=a.guid )t {0}) t {0}
order by 创建时间 desc
", condinton, viewName);
            }
            return sql;
        }

        private string GetString(HashSet<String> hashSet)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in hashSet)
            {
                sb.AppendFormat("'{0}',", item);
            }
            return sb.ToString().TrimEnd(',');
        }

        //public void contrlRepeater()
        //{
        //    Page.Trace.Warn(GetSql());
        //    DataTable dt = SqlHelper.GetTable(GetSql());
        //    PagedDataSource pds = new PagedDataSource();
        //    pds.DataSource = dt.DefaultView;
        //    pds.AllowPaging = true;
        //    pds.PageSize = 100;
        //    pds.CurrentPageIndex = Convert.ToInt32(this.labPage.Text) - 1;
        //    rpList.DataSource = pds;
        //    LabCountPage.Text = pds.PageCount.ToString();
        //    labPage.Text = (pds.CurrentPageIndex + 1).ToString();
        //    this.lbtnpritPage.Enabled = true;
        //    this.lbtnFirstPage.Enabled = true;
        //    this.lbtnNextPage.Enabled = true;
        //    this.lbtnDownPage.Enabled = true;
        //    if (pds.CurrentPageIndex < 1)
        //    {
        //        this.lbtnpritPage.Enabled = false;
        //        this.lbtnFirstPage.Enabled = false;
        //    }
        //    if (pds.CurrentPageIndex == pds.PageCount - 1)
        //    {
        //        this.lbtnNextPage.Enabled = false;
        //        this.lbtnDownPage.Enabled = false;
        //    }
        //    rpList.DataBind();
        //    //ToolCode.Tool.MergeCells(Repeater1, "tdOrderNumber");
        //    //ToolCode.Tool.MergeCells(Repeater1, "tdCustomerOrderNumber");
        //}
        //protected void lbtnpritPage_Click(object sender, EventArgs e)
        //{
        //    this.labPage.Text = Convert.ToString(Convert.ToInt32(labPage.Text) - 1);
        //    this.contrlRepeater();
        //}
        //protected void lbtnFirstPage_Click(object sender, EventArgs e)
        //{
        //    this.labPage.Text = "1";
        //    this.contrlRepeater();
        //}

        //protected void lbtnDownPage_Click(object sender, EventArgs e)
        //{
        //    this.labPage.Text = this.LabCountPage.Text;
        //    this.contrlRepeater();
        //}

        //protected void lbtnNextPage_Click(object sender, EventArgs e)
        //{
        //    this.labPage.Text = Convert.ToString(Convert.ToInt32(labPage.Text) + 1);
        //    this.contrlRepeater();
        //}
    }
}