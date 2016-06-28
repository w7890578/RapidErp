using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using System.Data;
using DAL;
using Model;
using BLL;

namespace Rapid.SellManager
{
    public partial class ImpNonDeliveryCompare : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlBindManager.BindCustomer(this.drpCustomerId);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //if (this.drpCustomerId.SelectedValue == "")
            //{
            //    Response.Write("<script>alert('请选择客户');</script>");
            //    return;
            //}
            string sql = string.Empty;
            string error = string.Empty;
            string timeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string userId = ToolCode.Tool.GetUser().UserNumber;
            DataSet ds = ToolManager.ImpExcel(this.FU_Excel, Server);
            if (ds == null)
            {
                lbMsg.Text = "选择的文件为空或不是标准的Excel文件！";
                return;
            }
            List<string> sqls = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                NonDeliveryCompare nondeliverycompare = new NonDeliveryCompare();
                nondeliverycompare.SupplierId = dr["供应商/供应工厂"] == null ? "" : dr["供应商/供应工厂"].ToString();
                nondeliverycompare.OrderNumber = dr["采购凭证"] == null ? "" : dr["采购凭证"].ToString();
                nondeliverycompare.RowNumber = dr["项目"] == null ? 0 : Convert.ToInt32(dr["项目"]);
                nondeliverycompare.CertificateDate = dr["凭证日期"] == null ? "" : dr["凭证日期"].ToString();
                nondeliverycompare.CustomerId = this.drpCustomerId.SelectedValue;
                nondeliverycompare.DeliveryDate = dr["交货日期"] == null ? "" : dr["交货日期"].ToString();
                nondeliverycompare.CustomerProductNumber = dr["物料"] == null ? "" : dr["物料"].ToString();
                nondeliverycompare.ShortText = dr["短文本"] == null ? "" : dr["短文本"].ToString();
                nondeliverycompare.OrderQty = dr["采购订单数量"] == null ? 0 : Convert.ToInt32(dr["采购订单数量"]);
                nondeliverycompare.DeliveryQty = dr["已交货数量"] == null ? 0 : Convert.ToInt32(dr["已交货数量"]);
                nondeliverycompare.StillDeliveryQty = dr["仍要交货(数量)"] == null ? 0 : Convert.ToInt32(dr["仍要交货(数量)"]);
                nondeliverycompare.CollectNumber = dr["汇总号"] == null ? "" : dr["汇总号"].ToString();
                sql = string.Format(@" insert into NonDeliveryCompare (ImportTime,OrderNumber,CustomerProductNumber,RowNumber,userid,SupplierId,
                  CertificateDate,DeliveryDate,ShortText,OrderQty,DeliveryQty,StillDeliveryQty,CollectNumber,customerId)
                   select  '{13}'  as 导入时间,'{0}' as 采购凭证,'{1}' as 物料,'{2}' as 项目,'{3}' as 导入人员,'{4}' as 供应商,
                   '{5}' as 凭证日期,'{6}' as 交货日期,'{7}' as 短文本,'{8}' as 采购订单数量,'{9}' as 已交货数量,'{10}' as 仍要交货数量,
                   '{11}' as 汇总号 , '{12}' as 客户编号",
                      nondeliverycompare.OrderNumber, nondeliverycompare.CustomerProductNumber, nondeliverycompare.RowNumber, userId,
                       nondeliverycompare.SupplierId, nondeliverycompare.CertificateDate, nondeliverycompare.DeliveryDate,
                       nondeliverycompare.ShortText, nondeliverycompare.OrderQty, nondeliverycompare.DeliveryQty,
                       nondeliverycompare.StillDeliveryQty, nondeliverycompare.CollectNumber, nondeliverycompare.CustomerId, timeNow);
                sqls.Add(sql);
            }
            if (SqlHelper.BatchExecuteSql(sqls, ref error))
            {
                string timeNowAdd1 = DateTime.Now.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");//后面插入时sql语句的导入时间
                sql = " select * from NonDeliveryCompare where UserId='" + userId + "'";
                DataTable dt = SqlHelper.GetTable(sql);
                if (dt.Rows.Count > 0)
                {
                    sql = string.Format(@" insert into NonDeliveryCompare (ImportTime,OrderNumber,customerid,CustomerProductNumber,RowNumber,
                            guid,userid,SupplierId,CertificateDate,DeliveryDate,ShortText,OrderQty,DeliveryQty,StillDeliveryQty,CollectNumber,
                            IsOrderNumber,IsRowNumber)
                            select  '{0}' as ImportTime,a.OrderNumber,a.customerid,a.customerproductnumber,a.rownumber,a.guid,a.userid,a.supplierid,
                            a.certificatedate,a.deliverydate,a.shorttext,a.orderqty,a.deliveryqty,a.stilldeliveryqty,a.collectnumber,
                            a.采购凭证对比结果,
                            case when a.RowNumber=b.贸易行号 then '正常' when a.RowNumber =b.加工行号 
                           then '正常' else '不正常' end 项目对比结果
                            from (
                                  select * from (  
                                  select ndc.* ,case when ISNULL ( so.OdersNumber,'')='' then '采购凭证异常' else '正常' end 采购凭证对比结果
                                  from NonDeliveryCompare ndc left join SaleOder so on so.OdersNumber =ndc.OrderNumber ) a          
                            ) a
                            left  join 
                           (
                            select so.OdersNumber,tod.RowNumber as 贸易行号,modt.RowNumber 加工行号 from SaleOder so 
                            inner join  TradingOrderDetail tod on so.OdersNumber =tod.OdersNumber
                            inner join MachineOderDetail modt on modt.OdersNumber =so.OdersNumber ) b on a.OrderNumber =b.OdersNumber ", timeNowAdd1);
                    if (SqlHelper.ExecuteSql(sql, ref error))
                    {
                        List<string> list = new List<string>();
                        sql = string.Format(@" delete NonDeliveryCompare where userid='{0}' and importtime='{1}'", userId, timeNow);
                        SqlHelper.ExecuteSql(sql, ref error);
                        sql = " select * from NonDeliveryCompare where UserId='" + userId + "' and IsOrderNumber='采购凭证正常' and IsRowNumber='正常'";
                        DataTable datatable = SqlHelper.GetTable(sql);
                        if (datatable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in datatable.Rows)
                            {
                                //加工
                                sql = string.Format(@" select ndc.StillDeliveryQty-mod.NonDeliveryQty as 仍要交货数量 from NonDeliveryCompare ndc 
                                left join MachineOderDetail mod on ndc.OrderNumber=mod.OdersNumber WHERE ndc.OrderNumber='{0}'", dr["OrderNumber"]);
                                DataTable Da = SqlHelper.GetTable(sql);
                                foreach (DataRow datarow in Da.Rows)
                                {
                                    if (Convert.ToInt32(datarow["仍要交货数量"]) == 0)
                                    {
                                        sql = string.Format(@" update NonDeliveryCompare set IsStillDeliveryQty='正常' where OrderNumber='{0}'", dr["OrderNumber"]);
                                        list.Add(sql);

                                    }
                                    else if (datarow["仍要交货数量"].ToString() == null)
                                    {
                                        //贸易
                                        sql = string.Format(@" select ndc.StillDeliveryQty-tod.NonDeliveryQty as 仍要交货数量 from NonDeliveryCompare ndc 
                                        left join TradingOrderDetail tod on ndc.OrderNumber=tod.OdersNumber
                                        where ndc.OrderNumber='{0}'", dr["OrderNumber"]);
                                        DataTable DT = SqlHelper.GetTable(sql);
                                        foreach (DataRow Dr in DT.Rows)
                                        {
                                            if (Convert.ToInt32(Dr["仍要交货数量"]) == 0)
                                            {
                                                sql = string.Format(@" update NonDeliveryCompare set IsStillDeliveryQty='正常' where OrderNumber='{0}'", dr["OrderNumber"]);
                                                list.Add(sql);


                                            }

                                        }

                                    }


                                }

                            }
                            SqlHelper.BatchExecuteSql(list, ref error);

                        }
                        else
                        {
                            List<string> li = new List<string>();
                            sql = " update NonDeliveryCompare set IsStillDeliveryQty='不正常' where UserId='" + userId + "' and IsOrderNumber='采购凭证异常' and IsRowNumber='不正常'";
                            li.Add(sql);
                            sql = " update NonDeliveryCompare set IsStillDeliveryQty='不正常' where UserId='" + userId + "' and IsOrderNumber='正常' and IsRowNumber='不正常'";
                            li.Add(sql);
                            SqlHelper.BatchExecuteSql(li, ref error);
                        }


                    }


                }


                lbMsg.Text = "导入成功！";

            }
            else
            {
                lbMsg.Text = error;
                return;
            }
        }
    }
}