<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditWarehouseAndOtherPerformance.aspx.cs" Inherits="Rapid.StoreroomManager.AddOrEditWarehouseAndOtherPerformance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
      <title>考核标准信息维护</title>
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
<body>
    <form id="form1" runat="server">
 <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写 &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
             <td align="right">
                 考核标准名称：
            </td>
            <td>
             <asp:TextBox ID="txtStandardName" runat="server" CssClass="input required" size="25" ></asp:TextBox>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                考核项目：
            </td>
            <td>
                <asp:TextBox ID="txtProject" runat="server" CssClass="input required" size="25" ></asp:TextBox>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                序号：
            </td>
            <td>
                <asp:TextBox ID="txtRowNumber" runat="server" CssClass="input required number" size="25" ></asp:TextBox>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                满分：
            </td>
            <td> 
                <asp:TextBox ID="txtFullScore" runat="server" CssClass="input required number" size="25" ></asp:TextBox>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                描述：
            </td>
            <td>
                <asp:TextBox ID="txtDescription"  runat="server" CssClass="input required " size="25" ></asp:TextBox>
                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                统计方式：
            </td>
            <td> 
                <asp:DropDownList ID="drpStatMode"  runat="server"  >
                    <asp:ListItem Selected="True" Value="0" >按个人统计</asp:ListItem>
                    <asp:ListItem Value="1" >按组统计</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：             </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" 
                    Height="30px" Width="300px" CssClass="input"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" 
                    onclick="btnSubmit_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="返回" id="btnBack" class="submit" />&nbsp;
                <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
