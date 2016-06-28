<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpMarerialInfoTable.aspx.cs"
    Inherits="Rapid.ProduceManager.ImpMarerialInfoTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>导入原材料信息</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Excel文件:<asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button
            ID="btnUpload" runat="server" Text=" 导 入 " OnClick="btnUpload_Click" />
        <a href="MarerialInfoTableList_New.aspx">返回列表</a>
        <br />
        <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
    </div>
    </form>
</body>
</html>
