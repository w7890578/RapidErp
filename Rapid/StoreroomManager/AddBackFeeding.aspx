<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBackFeeding.aspx.cs" Inherits="Rapid.StoreroomManager.AddBackFeeding" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=titleName%>信息维护</title>
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
            <tr>
                <td align="right">出入库编号：
                </td>
                <td>
                    <asp:Label runat="server" ID="lbWarehouseNumber"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">开工单号：
                </td>
                <td>
                    <asp:TextBox ID="txtKGNumber" runat="server" CssClass="input"></asp:TextBox>
                    <asp:Label ID="lbKGNumber" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">物料编号：
                </td>
                <td>
                    <asp:TextBox ID="txtNumber" runat="server" CssClass="input required"></asp:TextBox>
                    <asp:Label ID="lbNumber" runat="server" Text="">(客户物料编号或供应商物料编号，不支持模糊查询)</asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">数量：
                </td>
                <td>
                    <asp:TextBox ID="txtQty" runat="server" CssClass="input required number"></asp:TextBox>
                    <asp:Label ID="lbQty" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" OnClick="btnSubmit_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                <label style="color: Red;">
                    (*号为必填项)</label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

