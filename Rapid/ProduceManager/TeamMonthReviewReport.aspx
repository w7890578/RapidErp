<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TeamMonthReviewReport.aspx.cs"
    Inherits="Rapid.ProduceManager.TeamMonthReviewReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>小组月度绩效报表</title>
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
            padding: 2px;
            background-color: White;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-bottom: 10px;" id="divHeader">
        <div style="font-size: 28px; font-weight: bold; text-align: center; margin-top: 30px;">
            小组月度绩效报表</div>
        <br />
        <br />
        &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp; 年度：
        <asp:DropDownList ID="drpYear" runat="server" Style="margin-right: 10px;">
            <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
            <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
            <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
            <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
            <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
            <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
            <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
        </asp:DropDownList>
        月份：
        <asp:DropDownList ID="drpMonth" runat="server" Style="margin-right: 10px;">
            <asp:ListItem Value="1" Text="1"></asp:ListItem>
            <asp:ListItem Value="2" Text="2"></asp:ListItem>
            <asp:ListItem Value="3" Text="3"></asp:ListItem>
            <asp:ListItem Value="4" Text="4"></asp:ListItem>
            <asp:ListItem Value="5" Text="5"></asp:ListItem>
            <asp:ListItem Value="6" Text="6"></asp:ListItem>
            <asp:ListItem Value="7" Text="7"></asp:ListItem>
            <asp:ListItem Value="8" Text="8"></asp:ListItem>
            <asp:ListItem Value="9" Text="9"></asp:ListItem>
            <asp:ListItem Value="10" Text="10"></asp:ListItem>
            <asp:ListItem Value="11" Text="11"></asp:ListItem>
            <asp:ListItem Value="12" Text="12"></asp:ListItem>
        </asp:DropDownList>
        班组：<asp:DropDownList ID="drpTeam" runat="server" Style="margin-right: 10px;" 
            AutoPostBack="false">
        </asp:DropDownList>
        <asp:Button runat="server" Text="查询" ID="btnSearch" OnClick="btnSearch_Click" Style="margin-left: 10px;margin-right:10px;" />
        <asp:Button ID="btnEmp" runat="server" Text="导出Excel" onclick="btnEmp_Click" />
    </div>
    <div>
        <table cellpadding="1" cellspacing="1" class="border">
            <thead>
                <tr>
                  <td>
                        班组
                    </td>
                    <td>
                        开工单号
                    </td>
                  
                    <td>
                        完成数量
                    </td>
                    <td>
                        目标完成工时
                    </td>
                    <td>
                        实际完成工时
                    </td>
                    <td>
                        差额
                    </td>
                    <td>
                        创建时间
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("班组") %>
                            </td>
                            <td>
                                <%#Eval("开工单号") %>
                            </td>
                            <td>
                                <%#Eval("完成数量") %>
                            </td>
                            <td>
                                <%#Eval("目标完成工时") %>
                            </td>
                            <td>
                                <%#Eval("实际完成工时") %>
                            </td>
                            <td>
                                <%#Eval("差额") %>
                            </td>
                            <td>
                                <%#Eval("创建时间") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <asp:Repeater runat="server" ID="rpListTotal">
                    <ItemTemplate>
                        <tr>
                            <td>
                               合计
                            </td>
                            <td>
                            </td>
                            <td>
                                <%#Eval("完成数量") %>
                            </td>
                            <td>
                                <%#Eval("目标完成工时") %>
                            </td>
                            <td>
                                <%#Eval("实际完成工时") %>
                            </td>
                            <td>
                                <%#Eval("差额") %>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tfoot>
        </table>
    </div>
    </form>
</body>
</html>
