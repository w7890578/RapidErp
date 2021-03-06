﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountsReceivable_Main.aspx.cs"
    Inherits="Rapid.FinancialManager.AccountsReceivable_Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>应收账款</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function Edit(ordersNumber, createTime) {
            OpenDialog("../FinancialManager/EditAccountsReceivable_Main.aspx?ordersNumber=" + ordersNumber + "&createTime=" + createTime, "btnSearch", "620", "500");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <style type="text/css">
            .border
            {
                background-color: Black;
                width: 100%;
                font-size: 14px;
                text-align: center;
            }
            .border thead tr td
            {
                padding: 4px;
                background-color: white;
            }
            .border tbody tr td
            {
                padding: 4px;
                background-color: white;
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
            #choosePrintClounm ul
            {
                margin-bottom: 10px;
            }
            #choosePrintClounm div
            {
                text-align: center;
                color: Green;
            }
            #choosePrintClounm ul li
            {
                list-style: none;
                float: left;
                width: 100%;
                cursor: pointer;
            }
        </style>
        <div class="outerDiv" style="width: 1800px; margin-right: 20px;">
            <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-top: 1px;
                padding-top: 4px;">
                &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
                <span>&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;账务管理&nbsp;&nbsp;>&nbsp;&nbsp;应收账款</span>
            </div>
            <div>
                <input type="hidden" id="Hidden1" runat="server" />
                <div>
                    <input type="hidden" id="saveInfo" runat="server" />
                    <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                        <img src="../Img/loading.gif" alt="loading" />
                    </div>
                    <table class="pg_table" style="margin: 20px 10px 0px 0px">
                        <tr>
                            <td>
                                &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;客户名称：<asp:TextBox ID="txtCustomerName"
                                    runat="server"></asp:TextBox>
                            </td>
                            <td>
                                收款方式：<asp:TextBox ID="txtReceivable" runat="server"></asp:TextBox>
                            </td>
                            <td colspan="5" style="text-align: left">
                                <div style="vertical-align: middle">
                                </div>
                                <div>
                                    <div style="float: left; width: 65px;">
                                        <asp:Button Text="查询" ID="btnSearch" runat="server" OnClick="btnSearch_Click" class="button" />
                                    </div>
                                </div>
                            </td>
                            <td style="width: 1200px;">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="13">
                                <div>
                                    <table class="tablesorter" cellpadding="1" cellspacing="1">
                                        <thead>
                                            <tr>
                                                <td>
                                                    销售订单号
                                                </td>
                                                <td>
                                                    创建时间
                                                </td>
                                                <td>
                                                    订单总价
                                                </td>
                                                <td>
                                                    预收金额
                                                </td>
                                                <td>
                                                    应收总价
                                                </td>
                                                <td>
                                                    收款方式
                                                </td>
                                                <td>
                                                    客户名称
                                                </td>
                                                <td>
                                                    发货日期
                                                </td>
                                                <td>
                                                    发票号码
                                                </td>
                                                <td>
                                                    开票日期
                                                </td>
                                                <td>
                                                    账期
                                                </td>
                                                <td>
                                                    款项到期日
                                                </td>
                                                <td>
                                                    实际收款金额
                                                </td>
                                                <td>
                                                    实际收款日期
                                                </td>
                                                <td>
                                                    待收款金额
                                                </td>
                                                <td>
                                                    是否结清
                                                </td>
                                                <td>
                                                    送货人
                                                </td>
                                                <td>
                                                    操作
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rpList" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%#Eval("销售订单号")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("创建时间")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("订单总价")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("预收金额")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("应收总价")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("收款方式")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("客户名称")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("发货日期")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("发票号码")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("开票日期")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("账期")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("款项到期日")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("实际收款金额")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("实际收款日期")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("待收款金额")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("是否结清")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("送货人")%>
                                                        </td>
                                                        <td>
                                                            <a href="###" onclick="Edit('<%#Eval("销售订单号")%>','<%#Eval("创建时间")%>')">编辑</a> <a
                                                                href="AccountsReceivable_Detail.aspx?OrderNumber=<%#Eval("销售订单号")%>&CreateTime=<%#Eval("创建时间")%>">
                                                                详细</a>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
