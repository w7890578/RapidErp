<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditHalfOutProductWarehouseLogDetail.aspx.cs" Inherits="Rapid.StoreroomManager.AddOrEditHalfOutProductWarehouseLogDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>半成品出库明细</title>
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
                出库编号：
            </td>
            <td>
                <asp:Label ID="lbWarehouseNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                开工单号：
            </td>
            <td>
                <asp:TextBox ID="txtDocumentNumber" runat="server" CssClass="input required"></asp:TextBox>
                <asp:Label ID="lbDocumentNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                缺料原材料编号：
            </td>
            <td>
                 <asp:TextBox ID="txtMaterialNumner" runat="server" TextMode="MultiLine" Style="height: 50px;
                    width: 200px;" CssClass="input required"></asp:TextBox>
            </td>
        </tr>
       
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Style="height: 50px;
                    width: 200px;" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" class="submit" />
                <%--  <asp:Label ID="lbMsg" runat="server" Text="" ></asp:Label>--%>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>


