<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditMakeCollectionsMode.aspx.cs"
    Inherits="Rapid.SellManager.AddOrEditMakeCollectionsMode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>收款方式</title>
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
            $("#btnBack").click(function() {
            //window.location.href = "MakeCollectionsModeList.aspx";
                window.close(this);
            });
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr id="trNumber" runat="server">
            <td align="right">
                编号：
            </td>
            <td>
                <asp:TextBox ID="txtNumber" runat="server" CssClass="input required" size="25" MaxLength="50"
                    minlength="1"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                收款方式：
            </td>
            <td>
                <asp:TextBox ID="txtMakeCollectionsMode" runat="server" CssClass="input required"
                    size="25" MaxLength="50" minlength="2"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="返回" id="btnBack" class="submit" />
                &nbsp;&nbsp;&nbsp;
                <label style="color: Red;">
                    (*号为必填项)</label>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
