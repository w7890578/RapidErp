<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditMaterialSupplierProperty.aspx.cs"
    Inherits="Rapid.ProduceManager.MaterialSupplierProperty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>原材料供应商信息维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

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
        <tr>
            <td align="right">
                原材料编号：
            </td>
            <td>
                <asp:Label ID="lbMaterialNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                供应商：
            </td>
            <td>
                <asp:DropDownList ID="drpSupplierId" runat="server">
                </asp:DropDownList>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                供应商物料编号：
            </td>
            <td>
                <asp:TextBox ID="txtSupplierMaterialNumber" runat="server" size="25"></asp:TextBox>
                <asp:Label ID="lbSupplierMaterialNumber" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                最小起订量：
            </td>
            <td>
                <asp:TextBox ID="txtMinOrderQty" runat="server" CssClass="input required digits"
                    size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                单价：
            </td>
            <td>
                <asp:TextBox ID="txtPrice" runat="server" CssClass="input required number" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                供应商交货周期：
            </td>
            <td>
                <asp:TextBox ID="txtDeliveryCycle" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" Width="300px" Height="31px" CssClass="input"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;&nbsp;<%--<asp:Button ID="Button1" runat="server" Text="返回" 
                        CssClass="submit" onclick="back_Click" />--%>
                <input type="button" value="返回" id="btnBack" class="submit" />
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
