<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditTradingQuote.aspx.cs"
    Inherits="Rapid.SellManager.EditTradingQuote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>贸易报价单维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

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
<body  >
    <form id="form1" runat="server">
   
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                序号：
            </td>
            <td>
                <asp:TextBox ID="txtSN" runat="server" CssClass="input required digits" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                原材料编号：
            </td>
            <td>
                <asp:Label runat="server" ID="lbMarerial"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户物料编号：
            </td>
            <td>
                <asp:Label runat="server" ID="lbCustomerMarerial"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                物料描述：
            </td>
            <td>
                <asp:Label runat="server" ID="lbDescription"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                品牌：
            </td>
            <td>
                <asp:Label runat="server" ID="lbBrand"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                单价：
            </td>
            <td>
                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                最小包装：
            </td>
            <td>
                <asp:TextBox ID="txtMinPackage" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                最小起订量：
            </td>
            <td>
                <asp:TextBox ID="txtMinMOQ" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                固定提前期：
            </td>
            <td>
                <asp:TextBox ID="txtFixedLeadTime" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" TextMode="MultiLine" CssClass="input"
                    size="25" Height="31px"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
