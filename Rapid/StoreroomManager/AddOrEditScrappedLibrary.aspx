<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditScrappedLibrary.aspx.cs"
    Inherits="Rapid.StoreroomManager.AddOrEditScrappedLibrary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>产品报废出库明细</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
            <tr>
                <th colspan="2" align="left">基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
                </th>
            </tr>
            <tr>
                <td align="right">出库编号：
                </td>
                <td>
                    <asp:Label ID="lbWarehouseNumber" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr style="display: none;">
                <td align="right">产品：
                </td>
                <td>
                    <asp:DropDownList ID="drpProduct" runat="server"
                        OnSelectedIndexChanged="drpProduct_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <asp:Label ID="lbProduct" runat="server"></asp:Label>

                </td>
            </tr>

            <tr>
                <td align="right">产成品编号：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtProductNumber" CssClass="required"></asp:TextBox>
                    <label style="color: Red;">
                        *</label>
                </td>
            </tr>
            <tr>
                <td align="right">版本：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtVersion" CssClass="required"></asp:TextBox>
                    <label style="color: Red;">
                        *</label>
                </td>
            </tr>
            <tr>
                <td align="right">数量：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtQty" CssClass="required digits"></asp:TextBox>
                    <asp:Label runat="server" ID="lbOldQty" Visible="false"></asp:Label>
                    <label style="color: Red;">
                        *</label>
                </td>
            </tr>
            <tr style="display: none;">
                <td align="right">库存数量：
                </td>
                <td>
                    <asp:Label runat="server" ID="lbInventoryQty"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">报废原因：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtReason" TextMode="MultiLine" Width="200px" Height="50px"
                        CssClass="required"></asp:TextBox>
                    <label style="color: Red;">
                        *</label>
                </td>
            </tr>
            <tr>
                <td align="right">备注：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtRemark" MaxLength="200" Width="300px" Height="31px" CssClass="input"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" class="submit" />
                    <label style="color: Red;">
                        (*为必填项)</label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
