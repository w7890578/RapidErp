<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthSaleOderPriceAccount.aspx.cs"
    Inherits="Rapid.FinancialManager.MonthSaleOderPriceAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>月度销售额统计</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
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
        #choosePrintClounm
        {
            position: absolute;
            top: 20px;
            left: 50px;
            background-color: White;
            width: 170px;
            border: 1px solid green;
            padding: 10px;
            font-size: 14px;
            display: none;
        }
    </style>
    <input type="hidden" runat="server" id="hdBackUrl" />
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px; margin-bottom: 20px;">
        月度销售额统计
    </div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <input type="hidden" id="hdType" runat="server" />
        <div id="divHeader" style="margin-bottom: 10px;">
            <div style="width: 100%">
                <asp:Label ID="Label1" runat="server" Text="年度：" Style="margin-left: 10px;"></asp:Label><asp:DropDownList
                    ID="drpYear" runat="server" Style="margin-right: 10px;">
                    <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
                    <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
                    <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
                    <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
                    <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
                    <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
                    <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label2" runat="server" Text="客户名称"></asp:Label>
                <asp:TextBox ID="txtCustomerName" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 10px;" />
                <asp:Button ID="btnEmp" runat="server" Text="导出Excel" onclick="btnEmp_Click" />
                &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td>
                        客户名称
                    </td>
                    <td>
                        1月销售额
                    </td>
                    <td>
                        1月成本
                    </td>
                    <td>
                        1月利润
                    </td>
                    <td>
                        2月销售额
                    </td>
                    <td>
                        2月成本
                    </td>
                    <td>
                        2月利润
                    </td>
                    <td>
                        3月销售额
                    </td>
                    <td>
                        3月成本
                    </td>
                    <td>
                        3月利润
                    </td>
                    <td>
                        4月销售额
                    </td>
                    <td>
                        4月成本
                    </td>
                    <td>
                        4月利润
                    </td>
                    <td>
                        5月销售额
                    </td>
                    <td>
                        5月成本
                    </td>
                    <td>
                        5月利润
                    </td>
                    <td>
                        6月销售额
                    </td>
                    <td>
                        6月成本
                    </td>
                    <td>
                        6月利润
                    </td>
                    <td>
                        7月销售额
                    </td>
                    <td>
                        7月成本
                    </td>
                    <td>
                        7月利润
                    </td>
                    <td>
                        8月销售额
                    </td>
                    <td>
                        8月成本
                    </td>
                    <td>
                        8月利润
                    </td>
                    <td>
                        9月销售额
                    </td>
                    <td>
                        9月成本
                    </td>
                    <td>
                        9月利润
                    </td>
                    <td>
                        10月销售额
                    </td>
                    <td>
                        10月成本
                    </td>
                    <td>
                        10月利润
                    </td>
                    <td>
                        11月销售额
                    </td>
                    <td>
                        11月成本
                    </td>
                    <td>
                        11月利润
                    </td>
                    <td>
                        12月销售额
                    </td>
                    <td>
                        12月成本
                    </td>
                    <td>
                        12月利润
                    </td>
                    <td>
                        销售额合计
                    </td>
                    <td>
                        成本价合计
                    </td>
                    <td>
                        利润合计
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("客户名称")%>
                            </td>
                            <td>
                                <%#Eval("1月销售额")%>
                            </td>
                            <td>
                                <%#Eval("1月成本")%>
                            </td>
                            <td>
                                <%#Eval("1月利润")%>
                            </td>
                            <td>
                                <%#Eval("2月销售额")%>
                            </td>
                            <td>
                                <%#Eval("2月成本")%>
                            </td>
                            <td>
                                <%#Eval("2月利润")%>
                            </td>
                            <td>
                                <%#Eval("3月销售额")%>
                            </td>
                            <td>
                                <%#Eval("3月成本")%>
                            </td>
                            <td>
                                <%#Eval("3月利润")%>
                            </td>
                            <td>
                                <%#Eval("4月销售额")%>
                            </td>
                            <td>
                                <%#Eval("4月成本")%>
                            </td>
                            <td>
                                <%#Eval("4月利润")%>
                            </td>
                            <td>
                                <%#Eval("5月销售额")%>
                            </td>
                            <td>
                                <%#Eval("5月成本")%>
                            </td>
                            <td>
                                <%#Eval("5月利润")%>
                            </td>
                            <td>
                                <%#Eval("6月销售额")%>
                            </td>
                            <td>
                                <%#Eval("6月成本")%>
                            </td>
                            <td>
                                <%#Eval("6月利润")%>
                            </td>
                            <td>
                                <%#Eval("7月销售额")%>
                            </td>
                            <td>
                                <%#Eval("7月成本")%>
                            </td>
                            <td>
                                <%#Eval("7月利润")%>
                            </td>
                            <td>
                                <%#Eval("8月销售额")%>
                            </td>
                            <td>
                                <%#Eval("8月成本")%>
                            </td>
                            <td>
                                <%#Eval("8月利润")%>
                            </td>
                            <td>
                                <%#Eval("9月销售额")%>
                            </td>
                            <td>
                                <%#Eval("9月成本")%>
                            </td>
                            <td>
                                <%#Eval("9月利润")%>
                            </td>
                            <td>
                                <%#Eval("10月销售额")%>
                            </td>
                            <td>
                                <%#Eval("10月成本")%>
                            </td>
                            <td>
                                <%#Eval("10月利润")%>
                            </td>
                            <td>
                                <%#Eval("11月销售额")%>
                            </td>
                            <td>
                                <%#Eval("11月成本")%>
                            </td>
                            <td>
                                <%#Eval("11月利润")%>
                            </td>
                            <td>
                                <%#Eval("12月销售额")%>
                            </td>
                            <td>
                                <%#Eval("12月成本")%>
                            </td>
                            <td>
                                <%#Eval("12月利润")%>
                            </td>
                            <td>
                                <%#Eval("销售额合计")%>
                            </td>
                            <td>
                                <%#Eval("成本价合计")%>
                            </td>
                            <td>
                                <%#Eval("利润合计")%>
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
