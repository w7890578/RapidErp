<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditMaterialBlueprintProperty.aspx.cs"
    Inherits="Rapid.ProduceManager.AddOrEditMaterialBlueprintProperty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>原材料图纸信息维护</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

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
                <asp:Label ID="lbMaterialNumber" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                文件名称：
            </td>
            <td>
                <asp:TextBox ID="txtFileName" runat="server" CssClass="input required"></asp:TextBox>
                <asp:Label ID="lbFileName" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr id="trModifyTime" runat="server">
            <td align="right">
                修改时间：
            </td>
            <td>
                <asp:TextBox ID="txtModifyTime" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr id="trImportTime" runat="server">
            <td align="right">
                导入时间：
            </td>
            <td>
                <asp:TextBox ID="txtImportTime" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                导入人员：
            </td>
            <td>
                <asp:DropDownList ID="drpImportPerson" runat="server">
                </asp:DropDownList>
                <asp:Label ID="lblImportPerson" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr id="trFileUrl" runat="server">
            <td align="right">
                上传文件：
            </td>
            <td>
                <asp:FileUpload ID="fuFileUrl" runat="server" CssClass="input" />
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" Height="31px" MaxLength="200" Width="300px"
                    CssClass="input"></asp:TextBox>
                <asp:Label ID="Label3" runat="server" Text="（限制输入200字）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" OnClientClick="return ChkFile();"
                    CssClass="submit" />
                &nbsp;&nbsp;&nbsp;
                <%--  <asp:Label ID="lbMsg" runat="server" Style="color: Red;" Text=""></asp:Label>--%>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>

<script type="text/javascript">
    function ChkFile() {
        if (document.getElementById("fuFileUrl").value.length == 0) {
            alert("请选择上传文件");
            return false;
        }
</script>

