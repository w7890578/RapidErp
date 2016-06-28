<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditDeliveryBillList.aspx.cs"
    Inherits="Rapid.SellManager.AddOrEditDeliveryBillList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>送货单信息维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-ui-1.10.4.custom/js/jquery-1.10.2.js" type="text/javascript"></script>

    <script src="../Js/jquery-validation-1.11.1/dist/jquery.validate.js" type="text/javascript"></script>

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
            <td align="right">
                送货人：
            </td>
            <td>
                <%-- <asp:TextBox ID="txtDeliveryPerson" runat="server" CssClass="input required" size="25"></asp:TextBox>--%>
                <asp:DropDownList ID="drpDeliveryPerson" runat="server">
                </asp:DropDownList>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                送货日期：
            </td>
            <td>
                <asp:TextBox ID="txtDeliveryDate" runat="server" CssClass="input required" size="25"
                    onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户名称：
            </td>
            <td>
                <asp:DropDownList ID="drpCustomerName" runat="server">
                </asp:DropDownList>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                确认状态：
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpIsConfirm">
                    <asp:ListItem Text="未确认" Value="未确认"></asp:ListItem>
                    <asp:ListItem Text="已确认" Value="已确认"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                联系人：
            </td>
            <td>
                <asp:TextBox ID="txtContactsName" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                联系电话：
            </td>
            <td>
                <asp:TextBox ID="txtTel" runat="server" CssClass="input required" size="25"></asp:TextBox>
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
                &nbsp;&nbsp;&nbsp;<asp:Label ID="Label3" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
                &nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
