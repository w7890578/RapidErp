<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialPrice.aspx.cs"
    Inherits="Rapid.SellManager.MaterialPrice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>导入原材料价格</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function Write() {
            $("#lbMsg").html("正在导入！请稍等......");
            return true;
        }

        $(function() {
            $("#btnBack").click(function() {
                window.location.href = "../ProduceManager/MarerialInfoTableList_New.aspx";
            });
        })
       
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpload"
            runat="server" Text=" 导 入 " OnClick="btnUpload_Click" OnClientClick="return Write()"
            Style="margin-right: 10px;" />
        <input type="button" value="返回" id="btnBack" />
        <br />
        <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
    </div>
    </form>
</body>
</html>
