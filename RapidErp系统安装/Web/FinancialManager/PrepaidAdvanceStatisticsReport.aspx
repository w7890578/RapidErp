











<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrepaidAdvanceStatisticsReport.aspx.cs"
    Inherits="Rapid.FinancialManager.PrepaidAdvanceStatisticsReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>预收账款统计报表</title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

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
        预收账款统计报表
    </div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <input type="hidden" id="hdType" runat="server" />
        <div id="divHeader" style="margin-bottom: 10px;">
            <div style="width: 100%">
                &nbsp&nbsp; 销售订单号：<asp:TextBox ID="txtOdersNumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 客户采购合同号：<asp:TextBox ID="txtCustomerOdersNumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 客户名称：<asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                &nbsp&nbsp; 收款类型：<asp:DropDownList ID="drpType" runat="server">
                    <asp:ListItem Value="" Text="- - 请选择 - -"></asp:ListItem>
                    <asp:ListItem Value="现金" Text="现金"></asp:ListItem>
                    <asp:ListItem Value="转账" Text="转账"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div style="margin-top: 5px;">
                &nbsp&nbsp; 收款方式：<asp:TextBox ID="txtMathod" runat="server"></asp:TextBox>
                &nbsp&nbsp; 是否结清：<asp:DropDownList ID="drpisSettle" runat="server">
                    <asp:ListItem Value="" Text="- - 请选择 - -"></asp:ListItem>
                    <asp:ListItem Value="是" Text="是"></asp:ListItem>
                    <asp:ListItem Value="否" Text="否"></asp:ListItem>
                </asp:DropDownList>
                <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click"
                    Style="margin-right: 10px; margin-left: 10px;" />
                <asp:Button ID="btnEmp" runat="server" Text="导出Excel" OnClick="btnEmp_Click" />
                &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td>
                        序号
                    </td>
                    <td>
                        销售订单号
                    </td>
                    <td>
                        客户采购订单号
                    </td>
                    <td>
                        是否结清
                    </td>
                    <td>
                        送货日期
                    </td>
                    <td>
                        预收一
                    </td>
                    <td>
                        预收二
                    </td>
                    <td>
                        瑞普迪编号
                    </td>
                    <td>
                        客户物料编号
                    </td>
                    <td>
                        描述
                    </td>
                    <td>
                        客户名称
                    </td>
                    <td>
                        交货数量
                    </td>
                    <td>
                        单价
                    </td>
                    <td>
                        总价
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
                                <%#Eval("序号")%>
                            </td>
                            <td>
                                <%#Eval("销售订单号")%>
                            </td>
                            <td>
                                <%#Eval("客户采购订单号")%>
                            </td>
                            <td>
                                <%#Eval("是否结清")%>
                            </td>
                            <td>
                                <%#Eval("送货日期")%>
                            </td>
                            <td>
                                <%#Eval("预收一")%>
                            </td>
                            <td>
                                <%#Eval("预收二")%>
                            </td>
                            <td>
                                <%#Eval("瑞普迪编号")%>
                            </td>
                            <td>
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td>
                                <%#Eval("描述")%>
                            </td>
                            <td>
                                <%#Eval("客户名称")%>
                            </td>
                            <td>
                                <%#Eval("交货数量")%>
                            </td>
                            <td>
                                <%#Eval("单价")%>
                            </td>
                            <td>
                                <%#Eval("总价")%>
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
