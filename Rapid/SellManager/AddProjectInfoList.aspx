<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProjectInfoList.aspx.cs"
    Inherits="Rapid.SellManager.AddProjectInfoList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>项目信息添加</title>
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
                项目名称：
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtProjectName" CssClass="required"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr style="display:none;">
            <td align="right">
                产成品编号（瑞普迪编号）：
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtProductNumber" ></asp:TextBox>
                <label style="color: Red;">
                    *</label>&nbsp; 如果是包的话就填包的号。
            </td>
        </tr>
          <tr>
            <td align="right">
                客户产成品编号：
            </td>
            <td>
                <asp:TextBox runat ="server" ID ="txtCustomerProductNumber" CssClass="required"></asp:TextBox>
                <label style="color: Red;">
                    *</label> </td>
        </tr>
        <tr  >
            <td align="right">
                版本：
            </td>
            <td>
                <asp:TextBox ID="txtVersion" runat="server" Text="WU" size="25" CssClass="required"></asp:TextBox>
                <label style="color: Red;">
                    *</label>&nbsp; 如果是包的话就填“WU”， 产品没有版本的话也填“WU”。
            </td>
        </tr>
      
        <tr>
            <td align="right">
                单机：
            </td>
            <td>
                <asp:TextBox ID="txtSingle" runat="server" CssClass="required" size="25"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" size="25" Height="31px"
                    Width="300px"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
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
