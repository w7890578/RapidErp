<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpQty.aspx.cs" Inherits="Rapid.StoreroomManager.ImpQty" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>库存数量导入</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        导入类型：<asp:DropDownList runat="server" ID="drpType">
            <asp:ListItem Text="原材料库存数量导入" Value="0"></asp:ListItem>
            <asp:ListItem Text="产成品库存数量导入" Value="1"></asp:ListItem> 
        </asp:DropDownList>
        <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpload"
            runat="server" Text=" 导 入 " OnClick="btnUpload_Click" />&nbsp;&nbsp;<a href="WarehouseInfoList.aspx">返回仓库信息</a><br />
        <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;">注意：导入前请核对Excel内的相关内容，在系统内是否存在！</asp:Label>
        
    </div>
    </form>
</body>
</html>
