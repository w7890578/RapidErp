<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditMarerialLossLogDetail.aspx.cs"
    Inherits="Rapid.StoreroomManager.AddOrEditMarerialLossLogDetail" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>原材料损耗出库信息维护</title>
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
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                出入库编号：
            </td>
            <td>
                <asp:Label ID="lbWarehouseNumber" runat="server" Text="" CssClass="required"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户产成品编号：
            </td>
            <td>
                <asp:TextBox ID="txtCustomerProductNumber" runat="server" CssClass="input required"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:TextBox ID="txtVersion" runat="server" CssClass="input required"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                编号类型:
            </td>
            <td>
                <asp:DropDownList ID="drpType" runat="server">
                <asp:ListItem value="供应商物料编号" Text="供应商物料编号"></asp:ListItem>
                <asp:ListItem value="客户物料编号" Text="客户物料编号"></asp:ListItem>
                <asp:ListItem value="瑞普迪编号" Text="瑞普迪编号"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                原材料编号：
            </td>
            <td>
                <asp:TextBox ID="txtUniversalNumber" runat="server" CssClass="input required"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                日期：
            </td>
            <td>
                <asp:TextBox ID="txtDate" runat="server" CssClass="input required" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                数量：
            </td>
            <td>
                <asp:TextBox ID="txtQty" runat="server" CssClass="input required number"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                补领人：
            </td>
            <td>
                <asp:DropDownList ID="drpTakeMaterialPerson" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpTakeMaterialPerson_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                班组：
            </td>
            <td>
                <asp:Label ID="lbTeam" runat="server" Text="" CssClass="required"></asp:Label>
                <%--  <asp:DropDownList ID="drpTeam" runat="server" CssClass="required">
                </asp:DropDownList>--%>
            </td>
        </tr>
        <tr id="trAuditor" runat="server">
            <td align="right">
                损耗原因：
            </td>
            <td>
                <asp:TextBox ID="txtLossReason" runat="server" TextMode="MultiLine" Style="height: 50px;
                    width: 200px;" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Style="height: 50px;
                    width: 200px;" CssClass="input"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" class="submit" />
                <%--  <asp:Label ID="lbMsg" runat="server" Text="" ></asp:Label>--%>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
