﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditPrepaidAccountsApplicationNew.aspx.cs" Inherits="Rapid.PurchaseManager.EditPrepaidAccountsApplicationNew" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>预付主表维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            //表单验证JS
            $("#form1").validate({
                //出错时添加的标签
                errorElement: "span",
                success: function (label) {
                    //正确时的样式
                    label.text(" ").addClass("success");
                }
            });
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
            <tr>
                <th colspan="2" align="left">基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
                </th>
            </tr>
            <tr  >
                <td style="text-align: right;">预付一
                </td>
                <td>
                    <asp:TextBox ID="txtYFOne" runat="server" CssClass="input number required" ></asp:TextBox>
                </td>
            </tr>
            <tr  >
                <td style="text-align: right;">预付二
                </td>
                <td>
                    <asp:TextBox ID="txtYFTwo" runat="server" CssClass="input number required"></asp:TextBox>
                </td>
            </tr> 
            <tr>
                <td align="right"></td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="修改" CssClass="submit" OnClick="btnSubmit_Click" />
                    &nbsp;<asp:Label ID="Label5" runat="server" Text="（*号为必填项）" ForeColor="Red"></asp:Label>
                    &nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
