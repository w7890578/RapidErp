<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialActualQtyDetail.aspx.cs" Inherits="Rapid.PurchaseManager.MaterialActualQtyDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .border {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }

            .border tr td {
                padding: 4px;
                background-color: White;
            }

        a {
            color: Blue;
        }

            a:hover {
                color: Red;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center; font-size: 20px; font-weight: bold; margin-bottom: 5px;">
            实际需求明细(<%=Request["MaterialNumber"] %>)
        </div>
        <div style="text-align: center; margin-bottom: 5px;">
            <a href="SupplyAndDemandBalanceList.aspx">返回列表</a>
        </div>
        <div>
            <div style="margin: 10px; font-size: 15px;">产成品部分</div>
            <table cellpadding="1" cellspacing="1" class="border">
                <thead>
                    <tr>
                        <%
                            foreach (System.Data.DataColumn item in ProductActualQtyTable.Columns)
                            {%>
                        <td>
                            <%=item.ColumnName %>
                        </td>
                        <%}
                        %>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (System.Data.DataRow row in ProductActualQtyTable.Rows)
                       { %>
                    <tr>
                        <%
                           foreach (System.Data.DataColumn item in ProductActualQtyTable.Columns)
                           {%>
                        <td>
                            <%if (item.ColumnName == "在制数量")
                              { %>
                            <a target="_blank" href="GetInBusinessQty.aspx?ProductNumber=<%=row["产成品编号"] %>&Version=<%=row["版本"] %>"><%=row[item.ColumnName] %></a>
                            <%}
                              else
                              { %>
                            <%=row[item.ColumnName] %>
                            <%} %>
                        </td>
                        <%}
                        %>
                    </tr>
                    <%} %>
                </tbody>
            </table>
        </div>

        <div style="margin-top: 10px;">
            <div style="margin: 20px; font-size: 15px;">换算成原材料</div>
            <%MaterialActualQtyTable = MaterialActualQtyTable ?? new System.Data.DataTable(); %>
            <table cellpadding="1" cellspacing="1" class="border">
                <thead>
                    <tr>
                        <%
                            foreach (System.Data.DataColumn item in MaterialActualQtyTable.Columns)
                            {%>
                        <%if (item.ColumnName != "贸易销售订单未交" && item.ColumnName != "原材料编号")
                          {%>
                        <td>
                            <%=item.ColumnName %>
                        </td>
                        <%} %>
                        <%}
                        %>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (System.Data.DataRow row in MaterialActualQtyTable.Rows)
                       { %>
                    <tr>
                        <%
                           foreach (System.Data.DataColumn item in MaterialActualQtyTable.Columns)
                           {%>
                        <%if (item.ColumnName != "贸易销售订单未交" && item.ColumnName != "原材料编号")
                          {%>
                        <td>
                            <%=row[item.ColumnName] %>
                        </td>
                        <%} %>
                        <%}
                        %>
                    </tr>
                    <%} %>
                </tbody>
                <tfoot>
                    <tr>
                        <td>合计</td>
                        <td colspan="<%=MaterialActualQtyTable.Columns.Count-1 %>">

                            <%
 
                                Double weiquerensonghuodan = BLL.QtyManager.Instance.GetWeiQueRenSongHuoDan(Request["MaterialNumber"]);
                                Double weijiao = BLL.QtyManager.Instance.GetMaoYiWeiJiao(Request["MaterialNumber"]);
                                var sumBom = MaterialActualQtyTable.Compute("sum(BOM换算后)", "");
                                double sum = sumBom == null ? 0 : Convert.ToDouble(sumBom == DBNull.Value ? 0 : sumBom);
                            %>
                             <a href="../PurchaseManager/Report_TradingOrderDetailUnpaid.aspx?Id=<%=Request["MaterialNumber"] %>" target="_blank" title="查看详细">贸易销售订单未交(<%=weijiao %>)</a>+BOM换算后(<%=sum %>)+<a href="../PurchaseManager/DeliveryNoteDetailedUnpaid.aspx?Id=<%=Request["MaterialNumber"] %>" target="_blank">未确认送货单(<%=weiquerensonghuodan %>)</a>=
                            <%=sum+weijiao+weiquerensonghuodan %>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </form>
</body>
</html>
