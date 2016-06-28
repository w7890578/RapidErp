<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="T_SamplesFigureList.aspx.cs"
    Inherits="Rapid.ProduceManager.T_SamplesFigureList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>样品图</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<fieldset>
            <legend>增加图纸：</legend>
            <form>--%>
        图纸号：
        <asp:TextBox runat="server" ID="txtNumber"></asp:TextBox>
        <asp:Button ID="btnAdd" runat="server" Text="增加" OnClick="btnAdd_Click" />
        <asp:Label runat="server" ID="lbMsg" ForeColor="Red"></asp:Label>
        <%--   </form>
        </fieldset>--%>
        <fieldset>
            <legend>导入图片：</legend>图纸号：
            <asp:TextBox runat="server" ID="txtNumberImp"></asp:TextBox>上传文件：
            <asp:FileUpload ID="fuFileUrl" runat="server" CssClass="input" />
            <asp:Button ID="btnImp" runat="server" Text="导入" OnClick="btnImp_Click" /><asp:Label
                ID="lbMsgImp" runat="server" ForeColor="Red"> </asp:Label>
        </fieldset>
        <div style="margin-top: 20px;">
            <div style="width: 150px; float: left; margin-right: 20px;">
                <asp:ListBox runat="server" ID="lbNumber" Width="150px" OnSelectedIndexChanged="lbNumber_SelectedIndexChanged"
                    AutoPostBack="true"></asp:ListBox>
            </div>
            <div style="float: left;">
                <asp:Repeater ID="rpList" runat="server">
                    <ItemTemplate>
                        <div>
                            <img src="<%#Eval("Url") %>" title ="<%#Eval("FileName") %>"/>
                            <br /><%#Eval("FileName") %></div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
