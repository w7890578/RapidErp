<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Edit.Master" AutoEventWireup="true"
    CodeBehind="AddSampleCRK.aspx.cs" Inherits="Rapid.StoreroomManager.AddSampleCRK" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <tr>
        <td align="right">
            变动方向：
        </td>
        <td>
            <asp:DropDownList ID="drpType" runat="server"> 
                <asp:ListItem Text="入库" Value="入库"></asp:ListItem>
                <asp:ListItem Text="出库" Value="出库"></asp:ListItem>
            </asp:DropDownList>
            <label style="color: Red;">
                *</label>
        </td>
    </tr>
    <tr>
        <td align="right">
        </td>
        <td>
            <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" OnClick="btnSubmit_Click" />
            &nbsp;<asp:Label ID="lbMsg" runat="server" Text="（*号为必填项）" ForeColor="Red"></asp:Label>
            &nbsp;&nbsp;&nbsp;
        </td>
    </tr>
</asp:Content>
