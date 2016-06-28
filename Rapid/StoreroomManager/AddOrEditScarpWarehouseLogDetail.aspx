<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditScarpWarehouseLogDetail.aspx.cs" Inherits="Rapid.StoreroomManager.AddOrEditScarpWarehouseLogDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>废品出入库明细</title>
     <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>
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
                出入库编号：
            </td>
            <td>
                <asp:Label ID="lbWarehouseNumber" runat="server" Text="" ></asp:Label>
            </td>
        </tr>
       <tr>
            <td align="right">
                客户物料编号：
            </td>
            <td>
                <asp:TextBox ID="txtCustomerMarterilNumber" runat="server" CssClass="input required"></asp:TextBox>
                <asp:Label ID="lblCustomerMarterilNumber" runat="server" Text="" ></asp:Label>
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
      
        <tr>
            <td align="right">
               数量：
            </td>
            <td>
                <asp:TextBox ID="txtQty" runat="server" CssClass="input required"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Width="300px"  Height="31px" CssClass="input"></asp:TextBox>
                <asp:Label ID="lblMemo" runat="server" Text="（限制输入200字）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" class="submit" />
                <%--  <asp:Label ID="lbMsg" runat="server" Text="" ></asp:Label>--%>
                <asp:Label ID="Label4" runat="server" Text="（*号为必填项）" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
   
    </form>
</body>
</html>
