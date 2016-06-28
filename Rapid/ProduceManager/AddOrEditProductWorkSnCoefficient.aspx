<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditProductWorkSnCoefficient.aspx.cs" Inherits="Rapid.ProduceManager.AddOrEditProductWorkSnCoefficient" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>产品工序系数信息维护</title>
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
               工序名称：
            </td>
            <td>
                <asp:Label ID="lbWorkSnNumber" runat="server" Text="" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                序号：
            </td>
            <td>
                <asp:TextBox ID="txtRowNumber" runat="server" CssClass="input required number digits"></asp:TextBox>
                <asp:Label ID="lbRowNumber" runat="server" Text="" ></asp:Label>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
               最小值：
            </td>
            <td>
                <asp:TextBox ID="txtMinValue" runat="server" CssClass="input required number "></asp:TextBox>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
          <tr>
            <td align="right">
               最大值：
            </td>
            <td>
                <asp:TextBox ID="txtMaxValue" runat="server" CssClass="input required number "></asp:TextBox>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
          <tr>
            <td align="right">
             系数：
            </td>
            <td>
                <asp:TextBox ID="txtCoefficient" runat="server" CssClass="input required number"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        
        <tr>
            <td align="right" class="style1">
                备注：
            </td>
            <td class="style1">
                <asp:TextBox ID="txtRemark" runat="server"  height="31px"
                    width="300px" CssClass="input"></asp:TextBox>
                    <asp:Label ID="Label6" runat="server" Text="（限制输入200字）"></asp:Label>
            </td>
            
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
