<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SL.aspx.cs" Inherits="Rapid.ProduceManager.SL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border1
        {
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border tr td, .border1 tr td
        {
            padding: 4px;
            background-color: White;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center; margin-bottom: 5px;">
        <a href="WorkOrder.aspx">返回生成开工单列表</a>
    </div>
    <div style="margin-bottom: 10px; margin-top: 10px;">
        &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;是否缺料：<asp:DropDownList ID="drpQL" runat="server"
            Style="margin-right: 20px;">
            <asp:ListItem Value="" Text="- - - 请选择 - - -"></asp:ListItem>
            <asp:ListItem Value="是" Text="是"></asp:ListItem>
            <asp:ListItem Value="否" Text="否"></asp:ListItem>
        </asp:DropDownList>
        原材料编号：<asp:TextBox ID="txtOrderNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
        客户采购订单号：<asp:TextBox ID="txtCustomerOrderNumer" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="margin-left: 20px;" /></div>
    <div>
        <table cellpadding="1" cellspacing="1" class="border">
            <thead>
                <tr>
                    <td>
                        销售订单号
                    </td>
                       <td>
                        客户采购订单号
                    </td>
                    <td>
                        包号
                    </td>
                    <td>
                        产成品编号
                    </td>
                    <td>
                        客户产成品编号
                    </td>
                    <td>
                        版本
                    </td>
                    <td>
                        原材料编号
                    </td>
                    <td>
                        客户物料编号
                    </td>
                    <td>
                        单机用量
                    </td>
                    <td>
                        单位
                    </td>
                    <td>
                        实际生产数量
                    </td>
                    <td>
                        种类
                    </td>
                    <td>
                        描述
                    </td>
                    <td>
                        交期
                    </td>
                    <td>
                        行号
                    </td>
                    <td>
                        库存数量
                    </td>
                    <td>
                        计算结果
                    </td>
                    <td>
                        是否缺料
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rpList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("销售订单号") %>
                            </td>
                            <td>
                                <%#Eval("客户采购订单号") %>
                            </td>
                            <td>
                                <%#Eval("包号") %>
                            </td>
                            <td>
                                <%#Eval("产成品编号") %>
                            </td>
                            <td>
                                <%#Eval("客户产成品编号") %>
                            </td>
                            <td>
                                <%#Eval("版本") %>
                            </td>
                            <td>
                                <%#Eval("原材料编号") %>
                            </td>
                            <td>
                                <%#Eval("客户物料编号") %>
                            </td>
                            <td>
                                <%#Eval("单机用量新")%>
                            </td>
                            <td>
                                <%#Eval("单位新")%>
                            </td>
                            <td>
                                <%#Eval("实际生产数量") %>
                            </td>
                            <td>
                                <%#Eval("种类") %>
                            </td>
                            <td>
                                <%#Eval("描述") %>
                            </td>
                            <td>
                                <%#Eval("交期") %>
                            </td>
                            <td>
                                <%#Eval("行号") %>
                            </td>
                            <td>
                                <%#Eval("库存数量新")%>
                            </td>
                            <td style="color: <%#Eval("计算结果新").ToString ().Contains ("-")?"red":"black"%>;">
                                <%#Eval("计算结果新")%>
                            </td>
                            <td>
                                <%#Eval("是否缺料")%>
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
