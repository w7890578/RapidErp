<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificateDeliveryDetail.aspx.cs"
    Inherits="Rapid.PurchaseManager.CertificateDeliveryDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>采购已交明细表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

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
        <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px">
            采购已交明细表
        </div>
        <div>
            <div id="divHeader" style="margin-top: 20px;">
                <div style="margin-bottom: 5px">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="txtd" runat="server" CssClass="input required" Text="采购订单编号："></asp:Label>
                    <asp:TextBox ID="txtOrdersNumber" runat="server"></asp:TextBox>
                    &nbsp;<asp:Label ID="Label2" runat="server" Text="原材料编号：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtMaterialNumber" runat="server"></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" Text="描述：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;
                <asp:Label ID="Label3" runat="server" Text="供应商名称：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtSupplierName" runat="server"></asp:TextBox>
                </div>
                <div style="margin-bottom: 5px">
                    <asp:Label ID="Label5" runat="server" Text="采购日期：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtOrdersDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                    &nbsp;
                <asp:Label ID="Label6" runat="server" Text="预计交期：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtLeadTime" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                    <asp:Label ID="Label7" runat="server" Text="到货日期：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                    <asp:Label ID="Label4" runat="server" Text="供应商物料编号：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtSupplierNumber" runat="server"></asp:TextBox>
                </div>
                <div>
                    &nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label8" runat="server" Text="运输号：" Style="margin-left: 10px;"></asp:Label>
                    <asp:TextBox ID="txtNumber" runat="server"></asp:TextBox>
                    <asp:Label ID="Label9" runat="server" Text="付款：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtPay" runat="server"></asp:TextBox>
                    <asp:Label ID="Label10" runat="server" Text="备注：" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtRemark" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                    &nbsp;&nbsp;
                <asp:Button ID="btnExp" runat="server" Text="导出Excel" OnClick="btnExp_Click" />
                </div>
            </div>
            <br />
            <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
                <thead>
                    <tr>
                        <td>采购订单号
                        </td>
                        <td>合同号
                        </td>
                        <td>原材料编号
                        </td>
                        <td>描述
                        </td>
                        <td>供应商名称
                        </td>
                        <td>供应商物料编号
                        </td>
                        <td>采购数量
                        </td>
                        <td>已交数量
                        </td>
                        <td>单价
                        </td>
                        <td>总价
                        </td>
                        <td>采购日期
                        </td>
                        <td>预计交期
                        </td>
                        <td>到货日期
                        </td>
                        <td>入库单号</td>
                        <td>运输号
                        </td>
                        <td>付款
                        </td>
                        <td>备注
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpList">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("采购订单号")%>
                                </td>
                                <td>
                                    <%#Eval("合同号")%>
                                </td>
                                <td>
                                    <%#Eval("原材料编号")%>
                                </td>
                                <td>
                                    <%#Eval("描述")%>
                                </td>
                                <td>
                                    <%#Eval("供应商名称")%>
                                </td>
                                <td>
                                    <%#Eval("供应商物料编号")%>
                                </td>
                                <td>
                                    <%#Eval("采购数量")%>
                                </td>
                                <td>
                                    <%#Eval("已交数量")%>
                                </td>
                                <td>
                                    <%#Eval("单价")%>
                                </td>
                                <td>
                                    <%#Eval("总价")%>
                                </td>
                                <td>
                                    <%#Eval("采购日期")%>
                                </td>
                                <td>
                                    <%#Eval("预计交期")%>
                                </td>
                                <td>
                                    <%#Eval("到货日期")%>
                                </td>
                                <td>
                                    <%#Eval("出入库单号")%>
                                </td>
                                <td>
                                    <%#Eval("运输号")%>
                                </td>
                                <td>
                                    <%#Eval("付款")%>
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