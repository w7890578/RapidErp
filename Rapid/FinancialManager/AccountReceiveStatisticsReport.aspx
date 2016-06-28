<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountReceiveStatisticsReport.aspx.cs"
    Inherits="Rapid.FinancialManager.AccountReceiveStatisticsReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>应收账款统计报表</title>

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
        应收账款统计报表
    </div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <input type="hidden" id="hdType" runat="server" />
        <div id="divHeader" style="margin-bottom: 10px;">
            <div style="width: 100%">
                &nbsp&nbsp; 销售订单号：<asp:TextBox ID="txtOdersNumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 客户采购合同号：<asp:TextBox ID="txtCustomerOdersNumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 客户名称：<asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                &nbsp&nbsp; 送货日期：<asp:TextBox ID="txtDeliveryDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
            </div>
            <div style="margin-top: 5px;">
                &nbsp&nbsp; 送货单号：<asp:TextBox ID="txtDeliveryNumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 瑞普迪编号：<asp:TextBox ID="txtRapidNumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 客户物料编号：<asp:TextBox ID="txtCustomerMaterialNumber" runat="server"></asp:TextBox>
                <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click"
                    Style="margin-right: 10px; margin-left: 10px;" />
                    <asp:Button ID="btnEmp" runat="server" Text="导出Excel" 
                    onclick="btnEmp_Click" />
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
                        送货单号
                    </td>
                    <td>
                        销售订单号
                    </td>
                    <td>
                        客户采购订单号
                    </td>
                    <td>
                        送货日期
                    </td>
                    <td>
                        实际收款金额
                    </td>
                    <td>
                        实际收款日期
                    </td>
                    <td>
                        是否结清
                    </td>
                    <td>
                        瑞普迪编号
                    </td>
                    <td>
                        客户物料编号
                    </td>
                    <td>
                        版本
                    </td>
                    <td>
                        描述
                    </td>
                    <td>
                        客户名称
                    </td>
                    <td>
                        净化数量
                    </td>
                    <td>
                        单价
                    </td>
                    <td>
                        总价
                    </td>
                    <td>
                        创建时间
                    </td>
                    <td>
                        交期
                    </td>
                    <td>
                        行号
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
                                <%#Eval("送货单号")%>
                            </td>
                            <td>
                                <%#Eval("销售订单号")%>
                            </td>
                            <td>
                                <%#Eval("客户采购订单号")%>
                            </td>
                            <td>
                                <%#Eval("送货日期")%>
                            </td>
                            <td>
                                <%#Eval("实际收款金额")%>
                            </td>
                            <td>
                                <%#Eval("实际收款日期")%>
                            </td>
                            <td>
                                <%#Eval("是否结清")%>
                            </td>
                            <td>
                                <%#Eval("瑞普迪编号")%>
                            </td>
                            <td>
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td>
                                <%#Eval("版本")%>
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
                                <%#Eval("创建时间")%>
                            </td>
                            <td>
                                <%#Eval("交期")%>
                            </td>
                            <td>
                                <%#Eval("行号")%>
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
