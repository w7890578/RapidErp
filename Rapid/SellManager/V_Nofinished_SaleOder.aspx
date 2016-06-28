<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="V_Nofinished_SaleOder.aspx.cs" Inherits="Rapid.SellManager.V_Nofinished_SaleOder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> 未交销售订单</title> 
    
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
    <div style="width: 1500px;">
        <div style="text-align: center; font-size: 25px; margin: 10px;">
           未交销售订单
        </div>
           <div style="margin-bottom:10px;margin-top:10px;">
            &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;
            <asp:Label ID="Label1" runat="server" Text="销售订单号："></asp:Label>
            <asp:TextBox ID="txtOrdersNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="客户采购订单号："></asp:Label>
            <asp:TextBox ID="txtCGNomber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
            <asp:Label ID="Label3" runat="server" Text="订单日期："></asp:Label>
            <asp:TextBox ID="txtOrdersDate" runat="server" Style="margin-right: 10px;" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
            <asp:Label ID="Label4" runat="server" Text="产成品编号："></asp:Label>
            <asp:TextBox ID="txtProductNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
            <asp:Label ID="Label5" runat="server" Text="客户产成品编号："></asp:Label>
            <asp:TextBox ID="txtCustomerProductNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
               <asp:Label ID="Label11" runat="server" Text="项目："></asp:Label>
            <asp:TextBox ID="txtProjectName" runat="server" Style="margin-right: 10px;"></asp:TextBox>
          </div>
          <div style="margin-bottom:10px;margin-top:10px;">
 &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;
           
           <asp:Label ID="Label12" runat="server" Text="订单交期：" ></asp:Label>
            <asp:TextBox ID="txtLeadTime" runat="server" Style="margin-right: 10px;" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
            <asp:Label ID="Label7" runat="server" Text="客户："></asp:Label>
            <asp:TextBox ID="txtCustomerName" runat="server" Style="margin-right: 10px;"></asp:TextBox>
            <asp:Label ID="Label8" runat="server" Text="业务员："></asp:Label>
            <asp:TextBox ID="txtYW" runat="server" Style="margin-right: 10px;"></asp:TextBox>
             <asp:Label ID="Label6" runat="server" Text="订单状态："></asp:Label>
            <asp:TextBox ID="txtStatus" runat="server" Style="margin-right: 10px;"></asp:TextBox>
             <asp:Label ID="Label9" runat="server" Text="订单类型："></asp:Label>
            <asp:TextBox ID="txtType" runat="server" Style="margin-right: 10px;"></asp:TextBox>
             <asp:Label ID="Label10" runat="server" Text="生产类型："></asp:Label>
            <asp:TextBox ID="txtProductType" runat="server" Style="margin-right: 10px;"></asp:TextBox>
           
        </div>
        <div style ="text-align :center; margin-bottom :10px;">
         <asp:Button ID="btnSearch" runat="server" Text="查询" 
                onclick="btnSearch_Click1" style="margin-right:10px;"/>
                <%
                    bool exp = Rapid.ToolCode.Tool.GetUserMenuFunc("L0118", "exp");
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
                    客户采购订单号
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
                    订单日期
                </td>
                <td>
                    订单交期
                </td>
                <td>
                    行号
                </td>
                <td>
                    未交数量
                </td>
                <td>
                    已交数量
                </td>
                <td>
                    数量
                </td>
                 <td>
                    单价
                </td>
                <td>
                    总价
                </td>
                <td>
                    订单状态
                </td>
                <td>
                    订单类型
                </td>
                <td>
                    生产类型
                </td>
                <td>
                    客户
                </td>
                <td>
                    业务员
                </td>
                <td>项目</td>
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
                    <%#Eval("产成品编号")%>
                </td>
                <td>
                   <%#Eval("客户产成品编号")%> 
                </td>
                <td>
                    <%#Eval("版本")%>
                </td>
                <td>
                    <%#Eval("订单日期")%>
                </td>
                <td style="color:<%#Eval("color") %>;">
              
                    <%#Eval("订单交期")%>
                </td>
                <td>
                   <%#Eval("行号")%> 
                </td>
                <td>
                    <%#Eval("未交数量")%>
                </td>
                <td>
                   <%#Eval("已交数量")%> 
                </td>
                <td>
                    <%#Eval("数量")%>
                </td>
                 <td>
                    <%#Eval("单价")%>
                </td>
                <td>
                    <%#Eval("总价")%>
                </td>
                <td>
                    <%#Eval("订单状态")%>
                </td>
                <td>
                   <%#Eval("订单类型")%> 
                </td>
                <td>
                    <%#Eval("生产类型")%>
                </td>
                <td>
                   <%#Eval("客户")%> 
                </td>
                <td>
                    <%#Eval("业务员")%>
                </td>
                <td><%#Eval("项目")%></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>

