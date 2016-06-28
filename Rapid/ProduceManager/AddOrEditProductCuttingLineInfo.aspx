<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditProductCuttingLineInfo.aspx.cs"
    Inherits="Rapid.ProduceManager.AddOrEditProductCuttingLineInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>裁线信息维护</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            height: 60px;
        }
    </style>
</head>
<body >
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                产成品编号：
            </td>
            <td>
                <asp:Label ID="lbProductNumber" runat="server" Text="" ></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:Label ID="lbVersion" runat="server" Text="" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                原材料编号：
            </td>
            <td>
                <asp:Label ID="lbMaterialNumber" runat="server" Text="" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                长度：
            </td>
            <td>
                <asp:TextBox ID="txtLength" runat="server" CssClass="input required number"></asp:TextBox>
                <asp:Label ID="lbLength" runat="server" Text="" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                数量：
            </td>
            <td>
                <asp:TextBox ID="txtQty" runat="server" CssClass="input required number"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" class="style1">
                备注：
            </td>
            <td class="style1">
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Width="300px" Height="31px" CssClass="input"></asp:TextBox>
                <asp:Label ID="lblMemo" runat="server" Text="（限制输入200字）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
