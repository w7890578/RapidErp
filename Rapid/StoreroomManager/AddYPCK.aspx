<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Add.Master" AutoEventWireup="true"
    CodeBehind="AddYPCK.aspx.cs" Inherits="Rapid.StoreroomManager.AddYPCK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                样品订单：
            </td>
            <td>
                <asp:DropDownList ID="drpOrderNumber" runat="server" class="required">
                </asp:DropDownList>
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
    </table>
</asp:Content>
