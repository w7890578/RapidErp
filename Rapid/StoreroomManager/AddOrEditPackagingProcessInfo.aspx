<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditPackagingProcessInfo.aspx.cs" Inherits="Rapid.StoreroomManager.AddOrEditPackagingProcessInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>包装工序维护</title>
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
                成品类别：
            </td>
            <td>
                <asp:DropDownList runat="server" ID="drpProductTYpe" CssClass="required">
                    <asp:ListItem Value="光缆" Text="光缆"></asp:ListItem>
                    <asp:ListItem Value="连接器" Text="连接器"></asp:ListItem>
                </asp:DropDownList>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr> <tr>
            <td align="right">
                工序名称：
            </td>
            <td>
                <asp:TextBox ID="txtWorkSnName" runat="server"  CssClass="required" size="25"></asp:TextBox>
                <label style="color: Red;">
                    *</label>&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                工序编号：
            </td>
            <td>
                <asp:TextBox ID="txtWorkSnNumber" runat="server" CssClass="required" size="25"></asp:TextBox>
                <label style="color: Red;">
                    *</label>&nbsp; 建议填写工序名称拼音首字母，例如：裁线-cx。
            </td>
        </tr>
       
        <tr>
            <td align="right">
                序号：
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtSn"  CssClass="required digits"></asp:TextBox>
                <label style="color: Red;">
                    *</label>&nbsp; 表明工序的先后顺序，哪道工序先做或后做。
            </td>
        </tr>
        
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" OnClick="btnSubmit_Click" />
                &nbsp;<asp:Label ID="Label5" runat="server" Text="（*号为必填项）" ForeColor="Red"></asp:Label>
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>