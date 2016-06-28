<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpT_ProjectInfo.aspx.cs" Inherits="Rapid.ProduceManager.ImpT_ProjectInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>导入项目信息</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin:10px;"><a target="_blank" href="../Text/项目信息导入模板.xls">项目信息导入模板.xls</a>
   </div>
    <div>
    <asp:FileUpload ID="FU_Excel" runat="server"  />&nbsp;&nbsp;&nbsp;<asp:Button
                    ID="btnUpload" runat="server" Text=" 导 入 " 
            onclick="btnUpload_Click"  />
            &nbsp;&nbsp;<a href="../SellManager/ProjectMain.aspx">返回列表</a>
            <br /><asp:Label ID="lbMsg" runat="server" Text=""  style="color:Red ;"></asp:Label>
    </div>
    </form>
</body> 
</html>

