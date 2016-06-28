<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditToolMaterialWarehouseLogDetail.aspx.cs" Inherits="Rapid.StoreroomManager.AddOrEditToolMaterialWarehouseLogDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>样品入库信息维护</title>
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
        <tr id="trWarehouseNumber" runat ="server">
            <td align="right">
                出入库编号：
            </td>
            <td>
               
                <asp:Label ID="lbWarehouseNumber" runat="server" Text="" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            原材料编号：
            </td>
            <td>
                 <asp:TextBox ID="txtMaterialNumber" runat="server" CssClass="input" size="25" ></asp:TextBox>

            </td>
        </tr>
        <tr>
            <td align="right">
            供应商编号：
            </td>
            <td>
                <asp:DropDownList ID="drpGYSID" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
            供应商物料编号：
            </td>
            <td>
                <asp:TextBox ID="txtSupplierMaterialNumber" runat="server" CssClass="input" size="25"></asp:TextBox>
            </td>
        </tr>
        
         <tr>
            <td align="right">
            数量：
            </td>
            <td>
                 <asp:TextBox ID="txtQty" runat="server" CssClass="input required number digits"  size="25" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" height="30px"
                    width="300px" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" class="submit" />
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
