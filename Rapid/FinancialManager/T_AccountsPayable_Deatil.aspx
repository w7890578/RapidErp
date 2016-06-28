<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T_AccountsPayable_Deatil.aspx.cs"
    Inherits="Rapid.FinancialManager.T_AccountsPayable_Deatil" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#btnBack").click(function() {
                window.location.href = "T_AccountsPayable_MainList_New.aspx";
            });
        })
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
        <div class="outerDiv" style="width: 1000px; margin-right: 20px;">
            <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-top: 1px;
                padding-top: 4px;">
                &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
                <span>&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;账务管理&nbsp;&nbsp;>&nbsp;&nbsp;应付账款明细</span>
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
                            <td class="pg_talbe_head">
                            </td>
                            <td class="pg_talbe_content">
                            </td>
                            </td>
                            <td class="pg_talbe_content">
                            </td>
                            <td class="pg_talbe_head">
                            </td>
                            <td class="pg_talbe_content">
                            </td>
                            <td class="pg_talbe_head">
                            </td>
                            <td class="pg_talbe_content">
                            </td>
                            <td class="pg_talbe_head">
                            </td>
                            <td class="pg_talbe_content">
                            </td>
                            <td class="pg_talbe_head">
                            </td>
                            <td class="pg_talbe_content">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="13">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
                            <td colspan="5" style="text-align: left">
                                <div style="vertical-align: middle">
                                </div>
                                <div>
                                    <div style="float: left; width: 65px;">
                                        <asp:Button Text="查询" ID="btnSearch" runat="server" OnClick="btnSearch_Click" class="button" />
                                    </div>
                                    <div style="float: left; width: 65px;">
                                        <input type="button" value="返回" class="button" id="btnBack" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="13">
                                <div>
                                    <table class="tablesorter" cellpadding="1" cellspacing="1">
                                        <thead>
                                            <tr>
                                                <td>
                                                    采购订单号
                                                </td>
                                                <td>
                                                    创建时间
                                                </td>
                                                <td>
                                                    原材料编号
                                                </td>
                                                <td>
                                                    供应商物料编号
                                                </td>
                                                <td>
                                                    单价
                                                </td>
                                                <td>
                                                    数量
                                                </td>
                                                <td>
                                                    交期
                                                </td>
                                                <td>
                                                    总价
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="rpList" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <%#Eval("采购订单号")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("创建时间")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("原材料编号")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("供应商物料编号")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("单价")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("数量")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("交期")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("总价")%>
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
