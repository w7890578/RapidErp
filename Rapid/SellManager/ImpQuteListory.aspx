<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpQuteListory.aspx.cs"
    Inherits="Rapid.SellManager.ImpQuteListory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function Imp() {
            $("#lbMsg").html("正在导入！请稍等......");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        报价单类型：<asp:DropDownList ID="drpType" runat="server">
            <asp:ListItem Value="0" Text="加工报价单"></asp:ListItem>
            <asp:ListItem Value="1" Text="贸易报价单"></asp:ListItem>
        </asp:DropDownList>
        Excel文件：
        <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpload"
            runat="server" Text=" 导 入 " OnClick="btnUpload_Click" OnClientClick="return Imp()" />&nbsp;&nbsp;<a
                href="QuoteInfoList.aspx">返回列表</a>
        <br />
        <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
    </div>
    </form>
</body>
</html>
