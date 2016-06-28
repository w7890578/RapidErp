<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpMarerialScrapLogList.aspx.cs"
    Inherits="Rapid.StoreroomManager.ImpMarerialScrapLogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报废上报导入</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
        <p style="color: red;">提示：请正确填写客户产成品编号(图纸号),没有则请填写为空。</p>
        <div>
            Excel文件：<asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button
                ID="btnUpload" runat="server" Text=" 导 入 " OnClick="btnUpload_Click" /><br />
            <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
        </div>
    </form>
</body>
</html>
