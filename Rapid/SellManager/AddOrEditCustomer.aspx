<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditCustomer.aspx.cs"
    Inherits="Rapid.SellManager.AddOrEditCustomer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>客户信息维护</title>
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
                window.close();
            });
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table style="text-align: left;" width="100%" border="0" cellspacing="0" cellpadding="0"
        class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr id="trCustomerNuber" runat="server">
            <td align="right">
                客户编号：
            </td>
            <td>
                <asp:TextBox ID="txtCustomerNumber" runat="server" CssClass="input required"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户名称：
            </td>
            <td>
                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="input required"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                注册地址：
            </td>
            <td>
                <asp:TextBox ID="txtRegisteredAddress" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                法人代表：
            </td>
            <td>
                <asp:TextBox ID="txtLegalPerson" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                联系人：
            </td>
            <td>
                <asp:TextBox ID="txtContacts" runat="server" CssClass="input required"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                注册电话：
            </td>
            <td>
                <asp:TextBox ID="txtRegisteredPhone" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                联系电话：
            </td>
            <td>
                <asp:TextBox ID="txtContactTelephone" runat="server" CssClass="input required"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                传真：
            </td>
            <td>
                <asp:TextBox ID="txtFax" runat="server" CssClass="input required" size="20"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                手机：
            </td>
            <td>
                <asp:TextBox ID="txtMobilePhone" runat="server" CssClass="input" size="20" minLength="11"
                    MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                邮编：
            </td>
            <td>
                <asp:TextBox ID="txtZipCode" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备用电话：
            </td>
            <td>
                <asp:TextBox ID="txtSparePhone" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Email:
            </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                QQ:
            </td>
            <td>
                <asp:TextBox ID="txtQQ" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                开户银行：
            </td>
            <td>
                <asp:TextBox ID="txtAccountBank" runat="server" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                银行行号：
            </td>
            <td>
                <asp:TextBox ID="txtSortCode" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                银行账号：
            </td>
            <td>
                <asp:TextBox ID="txtBankAccount" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                纳税号：
            </td>
            <td>
                <asp:TextBox ID="txtTaxNo" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                网址：
            </td>
            <td>
                <asp:TextBox ID="txtWebsiteAddress" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                送货地点：
            </td>
            <td>
                <asp:TextBox ID="txtDeliveryAddress" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                工厂地址：
            </td>
            <td>
                <asp:TextBox ID="txtFactoryAddress" runat="server" CssClass="input" size="20"></asp:TextBox>
            </td>
        </tr>
        <%--<tr>
            <td align="right">
                账期：
            </td>
            <td>
                <asp:TextBox ID="txtPaymentdays" runat="server" CssClass="input required  number digits"
                    size="20"></asp:TextBox>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
        <tr>
            <td align="right">
                预收百分比：
            </td>
            <td>
                <asp:DropDownList ID="drpPercentageInAdvance" runat="server" CssClass="required">
                    <asp:ListItem Text="- - - -请 选 择- - - -" Value=""></asp:ListItem>
                    <asp:ListItem Text="0%" Value="0.0"></asp:ListItem>
                    <asp:ListItem Text="30%" Value="0.3"></asp:ListItem>
                    <asp:ListItem Text="50%" Value="0.5"></asp:ListItem>
                    <asp:ListItem Text="70%" Value="0.7"></asp:ListItem>
                    <asp:ListItem Text="100%" Value="1.0"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>--%>
         <tr>
            <td align="right">
               收款方式：
            </td>
            <td>
                <asp:DropDownList ID="drpMakeCollectionsModeId" runat="server" CssClass="required">
                </asp:DropDownList>
                <label style="color: Red;">
                    *</label>
            </td>
        </tr>
         <tr>
            <td align="right">
               收款类型：
            </td>
            <td>
                <asp:DropDownList ID="drpReceiveType" runat="server" CssClass="required">
                <asp:ListItem Value="现金" Text="现金"></asp:ListItem> 
                <asp:ListItem Value="转账" Text="转账"></asp:ListItem> 
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" size="20" Height="31px"
                    Width="300px" class="input"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="关闭" id="btnBack" class="submit" />
                &nbsp;&nbsp;&nbsp;
                <label style="color: Red;">
                    (*号为必填项)</label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
