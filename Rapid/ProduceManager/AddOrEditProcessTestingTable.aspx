<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditProcessTestingTable.aspx.cs"
    Inherits="Rapid.ProduceManager.AddOrEditProcessTestingTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>过程检验信息维护</title>
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

                window.close(this);
            });
        })
    </script>

</head>
<body >
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写 &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr runat="server">
            <td align="right">
                开工单号：
            </td>
            <td>
                <asp:TextBox ID="txtProductionOrderNumber" runat="server" CssClass="input required"
                    size="25"></asp:TextBox>
                <asp:Label ID="lblProductionOrderNumber" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr style="display:none;">
            <td align="right">
                产成品编号：
            </td>
            <td>
                <asp:TextBox ID="txtProductNumber" runat="server" CssClass="input" size="25"></asp:TextBox>
                <asp:Label ID="lblProductNumber" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
           <tr >
            <td align="right">
                版本：
            </td>
            <td>
                <asp:TextBox ID="txtVersion" runat="server" CssClass="input" size="25"></asp:TextBox>
                <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户产成品编号：
            </td>
            <td>
                <asp:TextBox ID="txtCustomerProductNumber" runat="server" CssClass="input required"
                    size="25"></asp:TextBox>
                <asp:Label ID="lblCustomerProductNumber" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr style="display: <%=show%>">
            <td align="right">
                上传文件：
            </td>
            <td>
                <asp:FileUpload ID="fuFileUrl" runat="server" CssClass="input" />
                 <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Height="31px" Width="300px"
                    CssClass="input"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="返回" id="btnBack" class="submit" />
                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="（*为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
