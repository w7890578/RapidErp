<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountPayStatisticsReport.aspx.cs" Inherits="Rapid.FinancialManager.AccountPayStatisticsReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>应付账款统计表</title>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        应付账款统计表</div>
    <div>
     <input type="hidden" id="Hidden1" runat="server" />
        <div id="divHeader" style="margin-top: 20px;">
            <div style="position: relative; float: left; margin-bottom: 10px">
            <asp:Label ID="Label1" runat="server" Text="采购订单号：" Style="margin-left: 20px;"></asp:Label>
            <asp:TextBox ID="txtNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="采购合同号：" Style="margin-left: 20px;"></asp:Label>
            <asp:TextBox ID="txtContractNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
            <asp:Label ID="Label3" runat="server" Text="原材料编号：" Style="margin-left: 20px;"></asp:Label>
            <asp:TextBox ID="txtMaterialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
            <asp:Label ID="Label4" runat="server" Text="供应商物料编号：" Style="margin-left: 20px;"></asp:Label>
            <asp:TextBox ID="txtSupplierNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <br />
                <br />
            <asp:Label ID="Label5" runat="server" Text="供应商名称：" Style="margin-left: 20px;"></asp:Label>
            <asp:TextBox ID="txtSupplierName" runat="server" Style="margin-right: 20px;"></asp:TextBox>
            <asp:Label ID="Label6" runat="server" Text="发票号码：" Style="margin-left: 20px;"></asp:Label>
            <asp:TextBox ID="txtInvoiceNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
            &nbsp;
            <asp:Label ID="Label7" runat="server" Text="是否结清：" Style="margin-left: 20px;"></asp:Label>
            <asp:DropDownList ID ="ddlIsSettlement" runat ="server">
             <asp:ListItem Value="" Text=""> - - - 请选择 - - - </asp:ListItem>
             <asp:ListItem Value="是" Text="是"></asp:ListItem>
             <asp:ListItem Value="否" Text="否"></asp:ListItem>
            </asp:DropDownList>
             &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
             <asp:Button ID="Button1" runat="server" Text="查询"  OnClick="btnSearch_Click" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="Button2" runat="server" Text="导出Excel"   OnClick ="btnEmp_Click"/>
            </div>
    </div>
    <br />
    <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
                <thead>
                <tr>
                <td class="tdOperar_序号" >
                        序号
                    </td>
                    <td class="tdOperar_采购订单号" >
                        采购订单号
                    </td>
                    <td class="tdOperar_采购合同号">
                        采购合同号
                    </td>
                    <td class="tdOperar_采购订单日期" >
                        采购订单日期
                    </td>
                      <td class="tdOperar_原材料编号" >
                         原材料编号
                    </td>
                       <td class="tdOperar_供应商物料编号" >
                        供应商物料编号
                    </td>
                       <td class="tdOperar_描述" >
                        描述
                    </td>
                       <td class="tdOperar_供应商名称" >
                        供应商名称
                    </td>
                       <td class="tdOperar_采购数量" >
                        采购数量
                    </td>
                     <td class="tdOperar_到货数量" >
                        到货数量
                    </td>
                     <td class="tdOperar_单价" >
                        单价
                    </td>
                     <td class="tdOperar_总价" >
                        总价
                    </td>
                     <td class="tdOperar_发票号码" >
                        发票号码
                    </td>
                     <td class="tdOperar_开票日期" >
                        开票日期
                    </td>
                     <td class="tdOperar_运输号" >
                        运输号
                    </td>
                     <td class="tdOperar_实际付款金额" >
                        实际付款金额
                    </td>
                     <td class="tdOperar_实际付款日期" >
                        实际付款日期
                    </td>
                     <td class="tdOperar_是否结清" >
                        是否结清
                    </td>
                     <td class="tdOperar_备注" >
                        备注
                    </td>
                </tr>
           
            </thead>
            <tbody >
                <asp:Repeater runat="server" ID="rptList">
                    <ItemTemplate>
                  <tr>
                     <td class="tdOperar_序号" >
                        <%#Eval("序号")%>
                    </td>
                    <td class="tdOperar_采购订单号" >
                        <%#Eval("采购订单号")%>
                    </td>
                    <td class="tdOperar_采购合同号">
                         <%#Eval("采购合同号")%>
                    </td>
                    <td class="tdOperar_采购订单日期" >
                        <%#Eval("采购订单日期")%>
                    </td>
                      <td class="tdOperar_原材料编号" >
                        <%#Eval("原材料编号")%>
                    </td>
                       <td class="tdOperar_供应商物料编号" >
                        <%#Eval("供应商物料编号")%>
                    </td>
                       <td class="tdOperar_描述" >
                         <%#Eval("描述")%>
                    </td>
                       <td class="tdOperar_供应商名称" >
                         <%#Eval("供应商名称")%>
                    </td>
                       <td class="tdOperar_采购数量" >
                         <%#Eval("采购数量")%>
                    </td>
                     <td class="tdOperar_到货数量" >
                         <%#Eval("到货数量")%>
                    </td>
                     <td class="tdOperar_单价" >
                        <%#Eval("单价")%>
                    </td>
                     <td class="tdOperar_总价" >
                        <%#Eval("总价")%>
                    </td>
                     <td class="tdOperar_发票号码" >
                        <%#Eval("发票号码")%>
                    </td>
                     <td class="tdOperar_开票日期" >
                        <%#Eval("开票日期")%>
                    </td>
                     <td class="tdOperar_运输号" >
                        <%#Eval("运输号")%>
                    </td>
                     <td class="tdOperar_实际付款金额" >
                         <%#Eval("实际付款金额")%>
                    </td>
                     <td class="tdOperar_实际付款日期" >
                         <%#Eval("实际付款日期")%>
                    </td>
                     <td class="tdOperar_是否结清" >
                         <%#Eval("是否结清")%>
                    </td>
                     <td class="tdOperar_备注" >
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

