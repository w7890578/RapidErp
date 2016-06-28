<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Add.Master" AutoEventWireup="true" CodeBehind="AddFLCKDetail.aspx.cs" Inherits="Rapid.StoreroomManager.AddFLCKDetail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr> 
        <%-- <tr>
            <td align="right">
                开工单号：
            </td>
            <td>
                <asp:TextBox ID="txtPlanNumber" runat="server" CssClass="input required"></asp:TextBox> 
            </td>
        </tr>--%>
       <tr><td align="right">
                编号类型：
            </td>
            <td>
                <asp:DropDownList ID="drpType" runat="server">
                <asp:ListItem value="供应商物料编号" Text="供应商物料编号"></asp:ListItem>
                <asp:ListItem value="客户物料编号" Text="客户物料编号"></asp:ListItem>
                <asp:ListItem value="瑞普迪编号" Text="瑞普迪编号"></asp:ListItem>
                <asp:ListItem Value="物料描述" Text="物料描述"></asp:ListItem>
                </asp:DropDownList>
            </td></tr>
        <tr>
            <td align="right">
                辅料编号：
            </td>
            <td>
                <asp:TextBox ID="txtCustomerMaterilNumber" runat="server" CssClass="input required"></asp:TextBox> 
            </td>
        </tr>
         <tr>
            <td align="right">
                数量：
            </td>
            <td>
                <asp:TextBox ID="txtQty" runat="server" CssClass="input required digits"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Style="height: 50px;
                    width: 200px;" CssClass="input"></asp:TextBox>&nbsp; 
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
</asp:Content>
