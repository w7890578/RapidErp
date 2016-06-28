<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditProductBlueprintProperty.aspx.cs"
    Inherits="Rapid.ProduceManager.AddOrEditProductBlueprintProperty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品图纸信息维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

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
                产成品编号：
            </td>
            <td>
                <asp:Label ID="lbProductNumber" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:Label ID="lbVersion" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr id="trFileName" runat="server">
            <td align="right">
                文件名称：
            </td>
            <td>
                <asp:TextBox ID="txtFileName" runat="server" CssClass="input required"></asp:TextBox>
                <asp:Label ID="lbFileName" runat="server" Text="Label"></asp:Label>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                类型：
            </td>
            <td>
                <asp:DropDownList ID="drpType" runat="server">
                    <asp:ListItem Selected="True">客户图纸</asp:ListItem>
                    <asp:ListItem>瑞普迪图纸</asp:ListItem>
                </asp:DropDownList>
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
        <tr id="trImportUser" runat="server">
            <td align="right">
                导入人员：
            </td>
            <td>
                <%--  <asp:DropDownList ID="drpImportPerson" runat="server">
        </asp:DropDownList>--%>
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
        <tr id="trCCFileUrl" runat="server">
            <td align="right">
                操作指导书：
            </td>
            <td>
                <asp:FileUpload ID="fuCCFileUrl" runat="server" CssClass="input" />
            </td>
        </tr>
        <tr id="trCCFileName" runat="server">
            <td>
                指导书名称：
            </td>
            <td>
                <asp:TextBox ID="txtCCFileName" runat="server" CssClass="input"></asp:TextBox>
                <asp:Label ID="lblCCFileName" runat="server" Text="Label"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                发放人员：
            </td>
            <td>
                <%-- <asp:DropDownList ID="drpProvidePersion" runat="server">
                </asp:DropDownList>--%>
                <asp:TextBox ID="txtProvidePersion" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                发放日期：
            </td>
            <td>
                <asp:TextBox ID="txtProvideDate" runat="server" CssClass="input" size="25" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                接收人员：
            </td>
            <td>
                <%--<asp:DropDownList ID="drpReceivePersion" runat="server">
                </asp:DropDownList>--%>
                <asp:TextBox ID="txtReceivePersion" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr id="trClaimModificationPerson" runat="server">
            <td align="right">
                要求修改人：
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtClaimModificationPerson" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Width="300px" Height="31px"
                    CssClass="input"></asp:TextBox>
                <asp:Label ID="lblMemo" runat="server" Text="（限制输入200字）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" class="submit" OnClick="btnSubmit_Click"
                    OnClientClick="return ChkFile();" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="返回" id="btnBack" class="submit" />
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script type="text/javascript">
    function ChkFile() {
        //        if (document.getElementById("fuFileUrl").value.length == 0) {
        //            alert("请选择上传文件");
        //            return false;
        //        }
        return true;
    }


</script>

