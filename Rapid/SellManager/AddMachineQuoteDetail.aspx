<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddMachineQuoteDetail.aspx.cs"
    Inherits="Rapid.SellManager.AddMachineQuoteDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>加工报价单明细</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

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
            width: 135px;
        }
        .style2
        {
            width: 86px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hdQuoteNumber" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="4" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                报价单号：
            </td>
            <td class="style1">
                <asp:Label ID="lbQuoteNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr id="trPackageNumber" runat="server">
            <td align="right">
                包：<br />
                <label style="color: blue;">
                    按Ctrl键多选</label>
            </td>
            <td class="style1">
                <asp:ListBox runat="server" ID="liPackageNumber" SelectionMode="Multiple" Width="100px"
                    Height="200px"></asp:ListBox>
            </td>
            <td align="right" class="style2">
                产成品|版本：
            </td>
            <td>
                <asp:ListBox runat="server" ID="liProductNumberAndVersion" SelectionMode="Multiple"
                    Width="100px" Height="200px"></asp:ListBox>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td class="style1">
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
