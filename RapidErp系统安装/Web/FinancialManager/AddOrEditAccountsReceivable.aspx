<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditAccountsReceivable.aspx.cs"
    Inherits="Rapid.FinancialManager.AddOrEditAccountsReceivable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>应收账款</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        #tbMarerial
        {
            width: 100%;
            text-align: center;
        }
        #mText
        {
            cursor: pointer;
        }
        .bgGray
        {
            background-color: #EBEBEB;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                订单编号：
            </td>
            <td>
                <asp:Label ID="lblOrdersNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                产成品编号：
            </td>
            <td>
                <asp:Label ID="lblProductNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                创建时间：
            </td>
            <td>
                <asp:Label ID="lblCreateTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户产成品编号：
            </td>
            <td>
                <asp:Label ID="lblCustomerProductNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户编号：
            </td>
            <td>
                <asp:Label ID="lblCustomerId" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                数量：
            </td>
            <td>
                <asp:Label ID="lblQty" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                单价：
            </td>
            <td>
                <asp:Label ID="lblUnitPrice" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                总价：
            </td>
            <td>
                <%--<asp:TextBox ID="txtSumPrice" runat="server" CssClass="input required number" size="25"></asp:TextBox>--%>
                <asp:Label ID="lblSumPrice" runat="server" Text=""></asp:Label>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                发货日期：
            </td>
            <td>
                <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="input required" size="25"
                     onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                发票号码：
            </td>
            <td>
                <asp:TextBox ID="txtInvoiceNumber" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" class="style1">
                开票日期：
            </td>
            <td class="style1">
                <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="input required" size="25"
                    onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                账期：
            </td>
            <td class="style1">
                <asp:DropDownList ID="drpAccountPeriod" runat="server">
                    <asp:ListItem Text="- - - 请 选 择 - - -"></asp:ListItem>
                    <asp:ListItem Value="0" Text="0天"></asp:ListItem>
                    <asp:ListItem Value="30" Text="30天"></asp:ListItem>
                    <asp:ListItem Value="60" Text="60天"></asp:ListItem>
                    <asp:ListItem Value="90" Text="90天"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                实收收款金额：
            </td>
            <td>
                <asp:TextBox ID="txtActualMakeCollectionsAmount" runat="server" CssClass="input required number"
                    size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                实收收款日期：
            </td>
            <td>
                <asp:TextBox ID="txtActuaMakeCollectionsDate" runat="server" CssClass="input required"
                    size="25" onfocus="WdatePicker({skin:'green',minDate:'%y-%M-{%d}'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="编辑" CssClass="submit" OnClick="btnSubmit_Click" />
                &nbsp;
                <asp:Label ID="Label11" runat="server" ForeColor="Red" Text="（*为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
