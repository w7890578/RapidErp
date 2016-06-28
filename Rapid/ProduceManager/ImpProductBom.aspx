<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpProductBom.aspx.cs"
    Inherits="Rapid.ProduceManager.ImpProductBom" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产成品信息导入</title>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script type ="text/javascript" >
        function Start() {
            $("#lbMsg").html("正在导入！请稍等......");
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        导入类型：<asp:DropDownList runat="server" ID="drpType">
            <asp:ListItem Text="产成品基本信息导入" Value="0"></asp:ListItem>
            <asp:ListItem Text="产成品2级BOM导入（不含包号）" Value="1"></asp:ListItem>
            <asp:ListItem Text="产成品3级BOM导入（包）" Value="2"></asp:ListItem>
            <asp:ListItem Text="工序工时导入" Value="3"></asp:ListItem>
        </asp:DropDownList>
        <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpload"
            runat="server" Text=" 导 入 " OnClick="btnUpload_Click" OnClientClick ="return Start()"/>&nbsp;&nbsp;<a href="ProductList.aspx">返回产品列表</a><br />
        <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;">注意：导入前请核对Excel内的相关内容，在系统内是否存在！</asp:Label>
        
    </div>
    </form>
</body>
</html>
