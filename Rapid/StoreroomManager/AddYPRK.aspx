<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Edit.Master" AutoEventWireup="true"
    CodeBehind="AddYPRK.aspx.cs" Inherits="Rapid.StoreroomManager.AddYPRK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <tr>
        <td align="right">
            供应商物料编号：
        </td>
        <td>
            <asp:TextBox ID="txtSuppilMateriNumber" runat="server" size="25" class="input required"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            数量：
        </td>
        <td>
            <asp:TextBox ID="txtQty" runat="server" size="25" class="input required digits"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td align="right">
            备注：
        </td>
        <td>
            <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Width="300px" size="25"
                Height="31px" class="input"></asp:TextBox>
            <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td>
            <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
            <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
        </td>
    </tr>
</asp:Content>
