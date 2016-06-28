<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthCertificateOrderReport.aspx.cs"
    Inherits="Rapid.PurchaseManager.MonthCertificateOrderReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购已入库统计报表</title>
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
        采购已入库统计报表
    </div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <input type="hidden" id="hdType" runat="server" />
        <div id="divHeader" style="margin-bottom: 10px;">
            <div style="width: 100%">
                <asp:Label ID="Label1" runat="server" Text="年度：" Style="margin-left: 10px;"></asp:Label><asp:DropDownList
                    ID="drpYear" runat="server" Style="margin-right: 10px;">
                    <asp:ListItem Value="" Text=" - - 请选择 - -"></asp:ListItem>
                    <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
                    <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
                    <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
                    <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
                    <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
                    <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
                    <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label3" runat="server" Text="原材料编号"></asp:Label>
                <asp:TextBox ID="txtMaterialNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="供应商物料编号"></asp:Label>
                <asp:TextBox ID="txtSupplierMaterialNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 10px;" />
                <asp:Button ID="btnEmp" runat="server" Text="导出Excel" OnClick="btnEmp_Click" />
                &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td>
                        原材料编号
                    </td>
                    <td>
                        供应商物料编号
                    </td>
                    <td>
                        1月
                    </td>
                    <td>
                        2月
                    </td>
                    <td>
                        3月
                    </td>
                    <td>
                        4月
                    </td>
                    <td>
                        5月
                    </td>
                    <td>
                        6月
                    </td>
                    <td>
                        1-6月小计
                    </td>
                    <td>
                        7月
                    </td>
                    <td>
                        8月
                    </td>
                    <td>
                        9月
                    </td>
                    <td>
                        10月
                    </td>
                    <td>
                        11月
                    </td>
                    <td>
                        12月
                    </td>
                    <td>
                        1-12月小计
                    </td>
                     <td>
                       单价
                    </td>
                    <td>
                        总价
                    </td>
                    <td>
                        原材料编号合计
                    </td>
                    <td>
                        瑞普迪编号合计
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("原材料编号")%>
                            </td>
                            <td>
                                <%#Eval("供应商物料编号")%>
                            </td>
                            <td>
                                <%#Eval("1月")%>
                            </td>
                            <td>
                                <%#Eval("2月")%>
                            </td>
                            <td>
                                <%#Eval("3月")%>
                            </td>
                            <td>
                                <%#Eval("4月")%>
                            </td>
                            <td>
                                <%#Eval("5月")%>
                            </td>
                            <td>
                                <%#Eval("6月")%>
                            </td>
                            <td>
                                <%#Eval("1-6月小计")%>
                            </td>
                            <td>
                                <%#Eval("7月")%>
                            </td>
                            <td>
                                <%#Eval("8月")%>
                            </td>
                            <td>
                                <%#Eval("9月")%>
                            </td>
                            <td>
                                <%#Eval("10月")%>
                            </td>
                            <td>
                                <%#Eval("11月")%>
                            </td>
                            <td>
                                <%#Eval("12月")%>
                            </td>
                            <td>
                                <%#Eval("7-12月小计")%>
                            </td>
                                <td>
                                <%#Eval("单价")%>
                            </td>
                            <td>
                                <%#Eval("总价")%>
                            </td>
                            <td>
                                <%#Eval("原材料编号合计")%>
                            </td>
                            <td>
                                <%#Eval("瑞普迪编号合计")%>
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
