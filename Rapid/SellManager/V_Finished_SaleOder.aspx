<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="V_Finished_SaleOder.aspx.cs"
    Inherits="Rapid.SellManager.V_Finished_SaleOder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>已交销售订单</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 1500px;">
            <div style="text-align: center; font-size: 25px; margin: 10px;">
                已交销售订单
            </div>
            <div style="margin-bottom: 10px; margin-top: 10px;">
                &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;
            <asp:Label ID="Label1" runat="server" Text="销售订单号："></asp:Label>
                <asp:TextBox ID="txtOrdersNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="客户采购订单号："></asp:Label>
                <asp:TextBox ID="txtCGNomber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Label ID="Label3" runat="server" Text="订单日期："></asp:Label>
                <asp:TextBox ID="txtOrdersDate" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" Text="产成品编号："></asp:Label>
                <asp:TextBox ID="txtProductNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Label ID="Label5" runat="server" Text="客户产成品编号："></asp:Label>
                <asp:TextBox ID="txtCustomerProductNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
            </div>
            <div style="margin-bottom: 20px; margin-top: 10px;">
                &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;
            <asp:Label ID="Label6" runat="server" Text="项目："></asp:Label>
                <asp:TextBox ID="txtProjectName" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Label ID="Label7" runat="server" Text="客户名称："></asp:Label>
                <asp:TextBox ID="txtCustomerName" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Label ID="Label8" runat="server" Text="业务员："></asp:Label>
                <asp:TextBox ID="txtYW" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                &nbsp;每页显示行数：<asp:TextBox runat="server" ID="txtPageSize" Text="20" Width ="50"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询"
                    OnClick="btnSearch_Click1" />&nbsp;&nbsp;
                  <%
                      bool exp = Rapid.ToolCode.Tool.GetUserMenuFunc("L0119", "exp");
                 %>
                    <span style="display: <%=exp?"":"none"%>;">
                 <asp:Button ID="btnExp" runat="server" Text="导出Excel" OnClick="btnExp_Click"
                      />
                        </span>
            </div>
            <table cellpadding="1" cellspacing="1" class="border">
                <tr>
                    <td>销售订单号
                    </td>
                    <td>客户采购订单编号
                    </td>
                    <td>订单日期
                    </td>
                    <td>产成品编号
                    </td>
                    <td>客户产成品编号
                    </td>
                    <td>版本
                    </td>
                    <td>项目
                    </td>
                    <td>单价
                    </td>
                    <td>客户
                    </td>
                    <td>业务员
                    </td>
                    <td>数量
                    </td>
                    <td>总价
                    </td>
                </tr>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("销售订单号")%>
                            </td>
                            <td>
                                <%#Eval("客户采购订单号")%>
                            </td>
                            <td>
                                <%#Eval("订单日期")%>
                            </td>
                            <td>
                                <%#Eval("产成品编号")%>
                            </td>
                            <td>
                                <%#Eval("客户产成品编号")%>
                            </td>
                            <td>
                                <%#Eval("版本")%>
                            </td>
                            <td>
                                <%#Eval("项目")%>
                            </td>
                            <td>
                                <%#Eval("单价")%>
                            </td>
                            <td>
                                <%#Eval("客户")%>
                            </td>
                            <td>
                                <%#Eval("业务员")%>
                            </td>
                            <td>
                                <%#Eval("数量")%>
                            </td>
                            <td>
                                <%#Eval("总价")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr>
                    <td align="center" colspan="13" style="padding: 2px;">
                        <asp:LinkButton ID="lbtnFirstPage" runat="server" OnClick="lbtnFirstPage_Click">页首</asp:LinkButton>
                        <asp:LinkButton ID="lbtnpritPage" runat="server" OnClick="lbtnpritPage_Click">上一页</asp:LinkButton>
                        <asp:LinkButton ID="lbtnNextPage" runat="server" OnClick="lbtnNextPage_Click">下一页</asp:LinkButton>
                        <asp:LinkButton ID="lbtnDownPage" runat="server" OnClick="lbtnDownPage_Click">页尾</asp:LinkButton>&nbsp;&nbsp;
                    第<asp:Label ID="labPage" runat="server" Text="Label"></asp:Label>页/共<asp:Label ID="LabCountPage"
                        runat="server" Text="Label"></asp:Label>页
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
