<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpInvoiceAccountInfo.aspx.cs" Inherits="Rapid.FinancialManager.ImpInvoiceAccountInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <div>
         <asp:Label ID="Label1" runat="server" Text="Excel文件："></asp:Label>
       <asp:FileUpload ID="FU_Excel" runat="server"  />&nbsp;&nbsp;&nbsp;<asp:Button
                    ID="btnUpload" runat="server" Text=" 导 入 " 
            onclick="btnUpload_Click"  style="margin-right:10px;"/> <a href="InvoiceAccountReport.aspx">返回列表</a> <br /><asp:Label ID="lbMsg" runat="server" Text=""  style="color:Red ;"></asp:Label>
    </div>
    </div>
    </form>
</body>
</html>
