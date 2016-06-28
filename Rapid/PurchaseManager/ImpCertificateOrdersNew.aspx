<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpCertificateOrdersNew.aspx.cs"
    Inherits="Rapid.PurchaseManager.ImpCertificateOrdersNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type ="text/javascript">
        function Write() {
            $("#lbMsg").html("正在导入！请稍等......");
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Excel文件：
        <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnSubmit" runat="server" Text="导入" CssClass="submit" OnClick="btnSubmit_Click" OnClientClick ="return Write()"/>&nbsp;&nbsp;&nbsp;
        <a href="CertificateOrdersList.aspx">返回列表</a><br />
        <asp:Label id="lbMsg" runat ="server" ForeColor ="Red"></asp:Label>
    </div>
    </form>
</body>
</html>
