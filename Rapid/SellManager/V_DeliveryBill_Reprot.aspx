<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="V_DeliveryBill_Reprot.aspx.cs"
    Inherits="Rapid.SellManager.V_DeliveryBill_Reprot" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>送货单明细报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 1500px;">
        <div style="text-align: center; font-size: 25px; margin: 10px;margin-top:50px;">
            送货单明细报表</div>
        <div style="margin-top:20px;"s>
            &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;销售单号:<asp:TextBox ID="txtOrderNumber" runat="server"></asp:TextBox>&nbsp&nbsp;&nbsp&nbsp;
            客户采购订单编号:<asp:TextBox ID="txtCustomerOrderNumber" runat="server"></asp:TextBox>&nbsp&nbsp;&nbsp&nbsp;
            产成品编号:<asp:TextBox ID="txtProductNumber" runat="server"></asp:TextBox>&nbsp&nbsp;&nbsp&nbsp;
            客户产成品编号:<asp:TextBox ID="txtCustomerProductNumber" runat="server"></asp:TextBox>
             项目:<asp:TextBox ID="txtProjectName" runat="server"></asp:TextBox>
        </div>
        <div style="margin-bottom:10px;margin-top:10px;">
            &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;客户名称:<asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>&nbsp&nbsp;&nbsp&nbsp;
            订单类型:<asp:TextBox ID="txtOrderType" runat="server"></asp:TextBox>&nbsp;&nbsp; 送货单号:
            <asp:TextBox ID="txtDeliyNumber" runat="server"></asp:TextBox>
            &nbsp&nbsp;&nbsp&nbsp; 送货人:
            <asp:TextBox ID="txtDeliveryPerson" runat="server"></asp:TextBox>
            &nbsp&nbsp;&nbsp&nbsp;
            <asp:Button runat="server" ID="btnSearch" Text="查询" OnClick="btnSearch_Click" style="margin-right:10px;"/>
             <%
                 bool exp = Rapid.ToolCode.Tool.GetUserMenuFunc("L0117", "exp");
                 %>
                    <span style="display: <%=exp?"":"none"%>;">
            
             <asp:Button ID="btnEmp" runat="server" Text="导出Excel" onclick="btnEmp_Click" />
                        </span>
        </div>
        <table cellpadding="1" cellspacing="1" class="border">
            <tr>
                <td>
                    销售订单号
                </td>
                <td>
                    客户采购订单编号
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
                    送货日期
                </td>
                <td>
                    行号
                </td>
                <td>
                    数量
                </td>
                <td>
                    项目
                </td>
                <td>
                    客户名称
                </td>
                <td>
                    订单类型
                </td>
                <td>
                    送货单号
                </td>
                 <td>
                    送货人
                </td>
            </tr>
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td >
                            <%#Eval("销售订单号") %>
                        </td>
                        <td >
                            <%#Eval("客户采购订单号") %>
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
                            <%#Eval("送货日期") %>
                        </td>
                        <td>
                            <%#Eval("行号") %>
                        </td>
                        <td>
                            <%#Eval("数量") %>
                        </td>
                        <td>
                            <%#Eval("项目") %>
                        </td>
                        <td>
                            <%#Eval("客户名称") %>
                        </td>
                        <td>
                            <%#Eval("订单类型") %>
                        </td>
                        <td>
                            <%#Eval("送货单号") %>
                        </td>
                        <td>
                            <%#Eval("送货人") %>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td align="center" colspan="13" style ="padding :2px;">
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
