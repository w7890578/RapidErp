<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificateMoneyReport.aspx.cs"
    Inherits="Rapid.PurchaseManager.CertificateMoneyReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购年度报表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border tr td
        {
            padding: 4px;
            background-color: White;
        }
        a
        {
            color: Blue;
        }
        a:hover
        {
            color: Red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        采购年度报表</div>
    <div>
        <div id="divHeader" style="margin-top: 20px;">
            <div>
                <asp:Label ID="Label6" runat="server" Text="年度：" Style="margin-left: 20px;"></asp:Label>
                <asp:DropDownList ID="drpYear" runat="server">
                    <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
                    <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
                    <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
                    <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
                    <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
                    <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
                    <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label1" runat="server" Text="月份：" Style="margin-left: 20px;"></asp:Label>
                <asp:DropDownList ID="drpMonth" runat="server" style="margin-right:10px;">
                    <asp:ListItem Value="1" Text="1">
                    </asp:ListItem>
                    <asp:ListItem Value="2" Text="2">
                    </asp:ListItem>
                    <asp:ListItem Value="3" Text="3">
                    </asp:ListItem>
                    <asp:ListItem Value="4" Text="4">
                    </asp:ListItem>
                    <asp:ListItem Value="5" Text="5">
                    </asp:ListItem>
                    <asp:ListItem Value="6" Text="6">
                    </asp:ListItem>
                    <asp:ListItem Value="7" Text="7">
                    </asp:ListItem>
                    <asp:ListItem Value="8" Text="8">
                    </asp:ListItem>
                    <asp:ListItem Value="9" Text="9">
                    </asp:ListItem>
                    <asp:ListItem Value="10" Text="10">
                    </asp:ListItem>
                    <asp:ListItem Value="11" Text="11">
                    </asp:ListItem>
                    <asp:ListItem Value="12" Text="12">
                    </asp:ListItem>
                </asp:DropDownList>
               <%-- <asp:Label ID="Label7" runat="server" Text="供应商名称：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtSupplierName" runat="server" Style="margin-right: 20px;"></asp:TextBox>--%>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
            </div>
        </div>
        <br />
        <table class="border" cellpadding="1" cellspacing="1" id="mainTable" style="width:800px;">
            <thead>
                <tr>
                    <td>
                        供应商名称
                    </td>
                    <td>
                        数量
                    </td>
                    <td>
                        采购额
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("供应商名称")%>
                            </td>
                            <td>
                                <%#Eval("数量")%>
                            </td>
                            <td>
                                <%#Eval("采购额")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
