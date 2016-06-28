<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetInBusinessQty.aspx.cs" Inherits="Rapid.PurchaseManager.GetInBusinessQty" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
            在制品数量(<%=Request["ProductNumber"] %>,<%=Request["Version"] %>)
        </div>
        <div style="text-align: center; margin-bottom: 5px;">
            <a href="SupplyAndDemandBalanceList.aspx">返回列表</a>
        </div>
        <div> 
            <%
                InBusinessQtyTable = InBusinessQtyTable ?? new System.Data.DataTable();
            %>
            <table cellpadding="1" cellspacing="1" class="border">
                <thead>
                    <tr>
                        <%
                            foreach (System.Data.DataColumn item in InBusinessQtyTable.Columns)
                            {%>
                        <td>
                            <%=item.ColumnName %>
                        </td>
                        <%}
                        %>
                    </tr>
                </thead>
                <tbody>
                    <% foreach (System.Data.DataRow row in InBusinessQtyTable.Rows)
                       { %>
                    <tr>
                        <%
                           foreach (System.Data.DataColumn item in InBusinessQtyTable.Columns)
                           {%>
                        <td>
                            <%=row[item.ColumnName] %>
                        </td>
                        <%}
                        %>
                    </tr>
                    <%} %>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

