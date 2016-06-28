<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpNonDeliveryCompare.aspx.cs"
    Inherits="Rapid.SellManager.ImpNonDeliveryCompare" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>导入未交清单对比列表</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DropDownList ID="drpCustomerId" runat="server">
        </asp:DropDownList>
        <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpload"
            runat="server" Text=" 导 入 " OnClick="btnUpload_Click" /><br />
        <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
    </div>
    </form>
</body>
</html>
