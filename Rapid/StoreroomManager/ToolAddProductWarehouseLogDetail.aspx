<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ToolAddProductWarehouseLogDetail.aspx.cs"
    Inherits="Rapid.StoreroomManager.ToolAddProductWarehouseLogDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>增加<%=titleName%></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

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
                出入库编号：
            </td>
            <td>
                <asp:Label runat="server" ID="lbWarehouseNumber"></asp:Label>
            </td>
        </tr>
        <tr id="trSaleOrder" runat="server">
            <td align="right">
                查询条件：
            </td>
            <td>
                交&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;期：<asp:TextBox runat="server" ID="txtDate"
                    onfocus="WdatePicker({skin:'green'})"></asp:TextBox><br />
                产成品编号：<asp:TextBox runat="server" ID="txtProductNumber"></asp:TextBox><br />
                客户产成品编号：<asp:TextBox runat="server" ID="txtCustomerProductNumber"></asp:TextBox>
                <br />
                客户编号：<asp:TextBox runat="server" ID="txtCustomerName"></asp:TextBox>&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnSearch" Text="查询" OnClick="btnSearch_Click" CssClass="submit" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <%=documentName %>：<br />
                <label style="color: Blue;">
                    按ctrl键多选</label>
            </td>
            <td>
                <asp:ListBox runat="server" ID="drpOrdersNumber" Width="300px" Height="300px"></asp:ListBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
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
