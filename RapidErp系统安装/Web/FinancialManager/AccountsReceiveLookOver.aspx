<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountsReceiveLookOver.aspx.cs" Inherits="Rapid.FinancialManager.AccountsReceiveLookOver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>应收账款查看</title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>
 <script type="text/javascript">

     function Detail(ordersnumber, createtime) {
         window.location.href = "../FinancialManager/AccountsReceiveDetail.aspx?OrdersNumber=" + ordersnumber + "&CreateTime=" + createtime + "&ck=3&time=" + new Date();
     }
     function Edit(guid) {
         OpenDialog("../FinancialManager/EditAccountsReceive.aspx?Guid=" + guid, "btnSearch", "400", "600");

     }
    </script>
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
        应收账款查看
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
                &nbsp&nbsp; 是否已开发票：<asp:DropDownList ID="drpFP" runat="server">
                    <asp:ListItem Value="" Text="- - 请选择 - -"></asp:ListItem>
                    <asp:ListItem Value="是" Text="是"></asp:ListItem>
                    <asp:ListItem Value="否" Text="否"></asp:ListItem>
                </asp:DropDownList> 
                 &nbsp&nbsp; 款项到期日：<asp:TextBox ID="txtPaymentDueDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                 </div>
                 <div style="margin-top: 5px;">
                &nbsp&nbsp; 送货日期：<asp:TextBox ID="txtDeliveryDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                &nbsp&nbsp; 送货单号：<asp:TextBox ID="txtDeliveryNumber" runat="server"></asp:TextBox>
                <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click"
                    Style="margin-right: 10px; margin-left: 10px;" />
                &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td>
                        销售订单号
                    </td>
                    <td>
                        客户采购订单号
                    </td>
                    <td>
                        送货单号
                    </td>
                    <td>
                        交货数量
                    </td>
                    <td>
                        交货总价
                    </td>
                      <td>
                        客户名称
                    </td>
                    <td>
                        收款类型
                    </td>
                    <td>
                        收款方式
                    </td>
                    <td>
                        款项到期日
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
                        是否已开发票
                    </td>
                    <td>
                        创建时间
                    </td>
                    <td>
                        备注
                    </td>
                    <td>
                        操作
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("销售订单号")%>
                            </td>
                            <td>
                                <%#Eval("客户采购订单号")%>
                            </td>
                            <td>
                                <%#Eval("送货单号")%>
                            </td>
                            <td>
                                <%#Eval("交货数量")%>
                            </td>
                            <td>
                                <%#Eval("交货总价")%>
                            </td>
                            <td>
                                <%#Eval("客户名称")%>
                            </td>
                            <td>
                                <%#Eval("收款类型")%>
                            </td>
                            <td>
                                <%#Eval("收款方式")%>
                            </td>
                            <td>
                                <%#Eval("款项到期日")%>
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
                                <%#Eval("是否已开发票")%>
                            </td>
                             <td>
                                <%#Eval("创建时间")%>
                            </td>
                            <td>
                                <%#Eval("备注")%>
                            </td>
                            <td>
                                <span style="display: <%#Eval("销售订单号").ToString().Equals("合计") ? "none" : "inline"%>;">
                                    <a href="###" onclick="Edit('<%#Eval("Guid")%>')">编辑</a> <a href="###" onclick="Detail('<%#Eval("销售订单号")%>',' <%#Eval("创建时间")%>')">
                                        详细</a></span>
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
