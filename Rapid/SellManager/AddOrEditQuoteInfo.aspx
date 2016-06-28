<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditQuoteInfo.aspx.cs"
    Inherits="Rapid.SellManager.AddOrEditQuoteInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报价单信息维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            //表单验证JS
            $("#form1").validate({
                //出错时添加的标签
                errorElement: "span",
                success: function(label) {
                    //正确时的样式
                    label.text(" ").addClass("success");
                }
            });
        })
    </script>

    <style type="text/css">
        .style1
        {
            height: 33px;
        }
        .style2
        {
            height: 34px;
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
            <td align="right" class="style1">
                报价单类型：
            </td>
            <td class="style1">
                <asp:DropDownList runat="server" ID="drpType">
                    <asp:ListItem Text="加工报价单" Value="加工报价单"></asp:ListItem>
                    <asp:ListItem Text="贸易报价单" Value="贸易报价单"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right" class="style2">
                报价时间：
            </td>
            <td class="style2">
                <%--  <asp:TextBox ID="txtQuoteTime" runat="server" CssClass="input required" size="25"
                    onfocus="WdatePicker({skin:'green',maxDate:'%y-%M-%d'})"></asp:TextBox>--%>
                <asp:TextBox ID="txtQuoteTime" runat="server" CssClass="input required" size="25"
                    onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户：
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpCustomer" class="required">
                </asp:DropDownList>
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户联系人：
            </td>
            <td>
                <asp:TextBox ID="txtCustomerContact" runat="server" class="input required"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" size="25" Height="31px"
                    Width="300px" class="input"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" OnClick="btnSubmit_Click" />
                <asp:Label ID="Label3" runat="server" Text="（*号为心填项）" ForeColor="Red"></asp:Label>
                &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
