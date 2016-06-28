<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TradingQuoteDetailReport.aspx.cs"
    Inherits="Rapid.SellManager.TradingQuoteDetailReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>贸易报价单报表</title>
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
    <div style="margin-bottom: 10px;">
        <div style="font-size: 28px; font-weight: bold; text-align: center; margin-top: 30px;">
            贸易报价单报表</div>
        <br />
        <br />
        &nbsp&nbsp; &nbsp&nbsp; 客户名称：<asp:TextBox ID="txtCustomerName" runat="server" Style="margin-right: 10px;"></asp:TextBox>
        客户物料编号：<asp:TextBox ID="txtCustomerMaterialNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
        供应商物料编号：<asp:TextBox ID="txtSupplierMaterialNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
        报价人：<asp:TextBox ID="txtQuoteUser" runat="server" Style="margin-right: 10px;"></asp:TextBox>
        <asp:Button runat="server" Text="查询" ID="btnSearch" OnClick="btnSearch_Click" Style="margin-left: 10px;
            margin-right: 10px;" />
        <asp:Button ID="btnEmp" runat="server" Text="导出" OnClick="btnEmp_Click" />
    </div>
    <div>
        <table cellpadding="1" cellspacing="1" class="border">
            <thead>
                <tr>
                    <td>
                        报价单号
                    </td>
                    <td>
                        客户名称
                    </td>
                    <td>
                        客户联系人
                    </td>
                    <td>
                        原材料编号
                    </td>
                    <td>
                        客户物料编号
                    </td>
                    <td>
                        供应商物料编号
                    </td>
                    <td>
                        固定提前期
                    </td>
                    <td>
                        单价
                    </td>
                    <td>
                        最小包装
                    </td>
                    <td>
                        最小起订量
                    </td>
                    <td>
                        物料描述
                    </td>
                    <td>
                        品牌
                    </td>
                    <td>
                        创建日期
                    </td>
                     <td>
                        报价人
                    </td>
                    <td>
                        备注
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("报价单号")%>
                            </td>
                            <td>
                                <%#Eval("客户名称")%>
                            </td>
                            <td>
                                <%#Eval("客户联系人")%>
                            </td>
                            <td>
                                <%#Eval("产成品编号")%>
                            </td>
                            <td>
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td>
                                <%#Eval("供应商物料编号")%>
                            </td>
                            <td>
                                <%#Eval("固定提前期")%>
                            </td>
                            <td>
                                <%#Eval("单价")%>
                            </td>
                            <td>
                                <%#Eval("最小包装")%>
                            </td>
                            <td>
                                <%#Eval("最小起订量")%>
                            </td>
                            <td>
                                <%#Eval("物料描述")%>
                            </td>
                            <td>
                                <%#Eval("品牌")%>
                            </td>
                            <td>
                                <%#Eval("创建日期")%>
                            </td>
                            <td>
                                <%#Eval("报价人")%>
                            </td>
                            <td>
                                <%#Eval("备注")%>
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
