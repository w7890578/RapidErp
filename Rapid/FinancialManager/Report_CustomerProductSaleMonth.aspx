<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_CustomerProductSaleMonth.aspx.cs" Inherits="Rapid.FinancialManager.Report_CustomerProductSaleMonth" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客户产成品销售月报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center; font-size: 25px; margin: 10px; margin-top: 50px;">
            客户产成品销售月报表
        </div>
        <div style="width: 100%; height: 20px;"></div>

        &nbsp;&nbsp;&nbsp;&nbsp;年份：
        <asp:DropDownList runat="server" ID="drpYear">
            <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
            <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
            <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
            <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;
        客户名称：
        <asp:TextBox runat="server" ID="txtCustomerName"></asp:TextBox>
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnSearch" Text="查询" OnClick="btnSearch_Click" />
        &nbsp;&nbsp;<input type="button" value="打印" />
        &nbsp;&nbsp;<asp:Button runat="server" ID="btnExp" Text="导出Excel" OnClick="btnExp_Click" />
        <div style="width: 100%; height: 20px;"></div>

        <table cellpadding="1" cellspacing="1" class="border">
            <tr>
                <% foreach (System.Data.DataColumn column in DtResult.Columns)
                    { %>
                <td><%=  column.ColumnName %></td>
                <%} %>
            </tr>
            <%foreach (System.Data.DataRow dr in DtResult.Rows)
                {%>
            <tr>
                <% foreach (System.Data.DataColumn column in DtResult.Columns)
                    { %>
                <td><%= dr[column.ColumnName]%></td>
                <%} %>
            </tr>
            <%} %>
        </table>
    </form>
</body>
</html>