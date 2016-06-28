<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditMarerialScrapLog.aspx.cs"
    Inherits="Rapid.StoreroomManager.MarerialScrapLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>原材料报废信息维护</title>
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
                window.location.href = "MarerialScrapLogList.aspx";
            });
        })
    </script>

</head>
<body style="padding: 10px;">
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr id="trId" runat="server">
            <td align="right">
                编号：
            </td>
            <td>
                <asp:TextBox ID="txtId" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr id="trCreateTime" runat="server">
            <td align="right">
                创建时间：
            </td>
            <td>
                <asp:TextBox ID="txtCreateTime" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr id="trProductNumber" runat="server">
            <td align="right">
                产成品编号：
            </td>
            <td>
                <asp:TextBox ID="txtProductNumber" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr id="trMaterialNumber" runat="server">
            <td align="right">
                原材料编号：
            </td>
            <td>
                <asp:TextBox ID="txtMaterialNumber" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                报废日期：
            </td>
            <td>
                <asp:TextBox ID="txtScrapDate" runat="server" CssClass="input" size="25" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                班组：
            </td>
            <td>
                <asp:TextBox ID="txtTeam" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                数量：
            </td>
            <td>
                <asp:TextBox ID="txtCount" runat="server" CssClass="input required" size="25" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                责任人：
            </td>
            <td>
                <asp:TextBox ID="txtResponsiblePerson" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                报废原因：
            </td>
            <td>
                <asp:TextBox ID="txtScrapReason" runat="server" CssClass="input required" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200"  CssClass="input"
                    size="25" Height="31px"  width="300px"></asp:TextBox>
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
                &nbsp;&nbsp;&nbsp;
                <%--  <label style="color: Red;">
                    (*号为必填项)</label>--%>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
