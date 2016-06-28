<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpMaterialSafeValue.aspx.cs" Inherits="Rapid.StoreroomManager.ImpMaterialSafeValue" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin: 10px;">
            <a href="../Text/原材料库存安全值导入模版.xls">原材料库存安全值导入模版.xls</a>
        </div>
        <div>
            Excel文件:<asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button
                ID="btnUpload" runat="server" Text=" 导 入 " OnClick="btnUpload_Click" OnClientClick="return upload()" />
            <a href="###" onclick="javascript:window.close();">关闭</a>
            <br />
            <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    function upload() {
        document.getElementById("lbMsg").innerHTML = "玩命导入中......";
        return true;
    }

</script>
