﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditWarehouseInOutType.aspx.cs" Inherits="Rapid.StoreroomManager.AddOrEditWarehouseInOutType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>出入库类型</title>
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
               仓库类型：
            </td>
            <td>
               <asp:DropDownList ID="drpWarehouseInOutType" runat="server">
                   <asp:ListItem Selected="True">原材料</asp:ListItem>
                   <asp:ListItem>产成品</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                变动方向：
            </td>
            <td>
                <asp:DropDownList ID="drpChangeDirection" runat="server">
                    <asp:ListItem Selected="True">出库</asp:ListItem>
                    <asp:ListItem>入库</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
      
       
        <tr>
            <td align="right">
              类型：
            </td>
            <td>
                <asp:TextBox ID="txtInOutType" runat="server" CssClass="input required"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
       
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" class="submit" />
                <%--  <asp:Label ID="lbMsg" runat="server" Text="" ></asp:Label>--%>
                <asp:Label ID="Label2" runat="server" Text="（*号为必填项）" ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
   
    </form>
</body>
</html>

