<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpCertificateOrdersList.aspx.cs"
    Inherits="Rapid.PurchaseManager.ImpCertificateOrdersList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {

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
    <div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
            <tr>
                <th colspan="2" align="left">
                    基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
                </th>
            </tr>
            <tr>
                <td align="right">
                    采购订单号：
                </td>
                <td>
                    <asp:TextBox ID="txtOrderNumber" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    付款方式：
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="drpPaymentMode" CssClass="required">
                    </asp:DropDownList>
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    采购订单日期：
                </td>
                <td>
                    <asp:TextBox ID="txtOrdersDate" runat="server" CssClass="input required" size="25"
                        onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    供应商：
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="drpSupplierId" CssClass="required">
                    </asp:DropDownList>
                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    业务员：
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="drpContactId" CssClass="required">
                    </asp:DropDownList>
                    <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    备注：
                </td>
                <td>
                    <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" size="25" Height="31px"
                        Width="300px" class="input"></asp:TextBox>
                    <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Excel文件：
                </td>
                <td>
                    <div>
                        <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnSubmit" runat="server" Text="导入" CssClass="submit" OnClick="btnSubmit_Click" />&nbsp;&nbsp;&nbsp;
                        <a href="CertificateOrdersList.aspx">返回列表</a><br />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
