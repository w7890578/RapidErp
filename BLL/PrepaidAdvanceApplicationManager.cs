using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using DAL;

namespace BLL
{
    public class PrepaidAdvanceApplicationManager
    {
        public static PrepaidAdvanceApplicationManager Instance
        {
            get
            {
                return new PrepaidAdvanceApplicationManager();
            }
        }
        private PrepaidAdvanceApplicationManager()
        {

        }

        /// <summary>
        /// 获取所有销售预收明细
        /// </summary>
        /// <returns></returns>
        public DataTable GetSalePreIncome()
        {
            string sql = @"
WITH B AS
  (SELECT *
   FROM
     (SELECT va.*,
             ta.InvoiceNumber AS 发票号码,ta.InvoiceDate AS 开票日期
      FROM V_AccountsReceivableDetail va
      INNER JOIN T_AccountsReceivable_Detail ta ON va.guid=ta.guid)t) ,
     A AS
  (SELECT t.销售订单号
   FROM
     (SELECT *
      FROM
        (SELECT vt.*,
                a.InvoiceNumber AS 发票号码,a.InvoiceDate AS 开票日期
         FROM V_T_AccountsReceivableMain_IsAdvance vt
         INNER JOIN AccountsReceivable a ON vt.guid=a.guid)t
      WHERE 是否为预收='是'
        AND 是否已审批='否'
        AND 是否已被申请='否'
        AND 是否结清='否')t
   GROUP BY t.销售订单号),
     C AS
  ( SELECT DISTINCT B.*
   FROM A
   INNER JOIN B ON A.销售订单号=B.销售订单号) ,
     D AS
  ( SELECT C.*,
           e.CustomerName AS 客户名称
   FROM C
   INNER JOIN SaleOder d
   INNER JOIN Customer e ON d.CustomerId=e.CustomerId ON c.销售订单号=d.OdersNumber)
SELECT D.送货单号,D.销售订单号,D.客户采购订单号,D.客户名称,D.瑞普迪编号,D.客户物料编号, D.版本,D.描述,D.交货数量,D.单价,D.总价,D.行号,D.交期,D.备注,D.创建时间,D.发票号码,D.开票日期
FROM D  ";
            return SqlHelper.GetTable(sql);
        }

        /// <summary>
        /// 导出所有销售预收明细
        /// </summary>
        /// <returns></returns>
        public void ExpSalePreIncome()
        {
            DataTable dt = GetSalePreIncome();
            DataRow dr = dt.NewRow();
            dr["交货数量"] = dt.Compute("Sum(交货数量)", "");
            dr["总价"] = dt.Compute("Sum(总价)", "");
            dr["送货单号"] = "合计";
            dt.Rows.Add(dr);
            dt.Columns["发票号码"].DataType = typeof(string);
            ExcelHelper.Instance.ExpExcel(dt, "所有销售预收明细(" + DateTime.Now.ToString("yyyy-MM-dd") + ")");
        }

        /// <summary>
        /// 获取所有销售应收明细
        /// </summary>
        /// <returns></returns>
        public DataTable GetSaleShouldIncome()
        {
            string sql = @"
WITH A AS
  (SELECT *
   FROM
     (SELECT va.*,
             ta.InvoiceNumber AS 发票号码,ta.InvoiceDate AS 开票日期
      FROM V_AccountsReceivableDetail va
      INNER JOIN T_AccountsReceivable_Detail ta ON va.guid=ta.guid)t) ,
     B AS
  (SELECT t.销售订单号,t.创建时间,t.送货单号
   FROM
     (SELECT va.*,
             ta.InvoiceNumber AS 发票号码,ta.InvoiceDate AS 开票日期
      FROM V_AccountsReceivableDetail va
      INNER JOIN T_AccountsReceivable_Detail ta ON va.guid=ta.guid)t
   GROUP BY t.销售订单号,t.创建时间,t.送货单号),
     C AS
  (SELECT A.*
   FROM A
   INNER JOIN B ON A.销售订单号=B.销售订单号
   AND A.创建时间=B.创建时间
   AND A.送货单号=B.送货单号),
     D AS
  (SELECT C.*,
          e.CustomerName AS 客户名称
   FROM C
   INNER JOIN SaleOder d
   INNER JOIN Customer e ON d.CustomerId=e.CustomerId ON c.销售订单号=d.OdersNumber)
SELECT D.送货单号,d.销售订单号,d.客户采购订单号,d.客户名称,d.瑞普迪编号,d.客户物料编号,d.版本 ,d.描述,d.交货数量,d.单价,d.总价,d.行号,d.交期,d.备注,d.创建时间,d.发票号码,d.开票日期
FROM D
";
            return SqlHelper.GetTable(sql);
        }

        /// <summary>
        /// 导出所有销售应收明细
        /// </summary>
        /// <returns></returns>
        public void ExpSaleShouldIncome()
        {
            DataTable dt = GetSaleShouldIncome();
            DataRow dr = dt.NewRow();
            dr["交货数量"] = dt.Compute("Sum(交货数量)", "");
            dr["总价"] = dt.Compute("Sum(总价)", "");
            dr["送货单号"] = "合计";
            dt.Rows.Add(dr);
            dt.Columns["发票号码"].DataType = typeof(string);
            ExcelHelper.Instance.ExpExcel(dt, "所有销售应收明细(" + DateTime.Now.ToString("yyyy-MM-dd") + ")");
        }

        /// <summary>
        /// 导出销售预收未开票明细
        /// </summary>
        public void ExpSalePreIncomeNoInvoice()
        {
            DataTable dt = GetSalePreIncome();
            dt = DataTableHelper.Instance.Select(dt, "发票号码 is null");
            DataRow dr = dt.NewRow();
            dr["交货数量"] = dt.Compute("Sum(交货数量)", "");
            dr["总价"] = dt.Compute("Sum(总价)", "");
            dr["送货单号"] = "合计";
            dt.Rows.Add(dr);
            ExcelHelper.Instance.ExpExcel(dt, "所有未开票销售预收明细(" + DateTime.Now.ToString("yyyy-MM-dd") + ")");
        }

        /// <summary>
        /// 导出销售应收未开票明细
        /// </summary>
        public void ExpSaleShouldIncomeNoInvoice()
        {
            DataTable dt = GetSaleShouldIncome();
            dt = DataTableHelper.Instance.Select(dt, "发票号码 is null");
            DataRow dr = dt.NewRow();
            dr["交货数量"] = dt.Compute("Sum(交货数量)", "");
            dr["总价"] = dt.Compute("Sum(总价)", "");
            dr["送货单号"] = "合计";
            dt.Rows.Add(dr);
            ExcelHelper.Instance.ExpExcel(dt, "所有未开票销售应收明细(" + DateTime.Now.ToString("yyyy-MM-dd") + ")");
        }

    }
}
