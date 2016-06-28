<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpExaminationLog.aspx.cs"
    Inherits="Rapid.ProduceManager.ImpExaminationLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>导入员工考试成绩上报</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 30px;">
        友情提示：导入前请检查Excel中，姓名是否存在于用户表！
    </div>
    <div>
        &nbsp;&nbsp; 年份：<asp:DropDownList ID="drpYear" runat="server">
            <asp:ListItem>2012</asp:ListItem>
            <asp:ListItem>2013</asp:ListItem>
            <asp:ListItem>2014</asp:ListItem>
            <asp:ListItem>2015</asp:ListItem>
            <asp:ListItem>2016</asp:ListItem>
            <asp:ListItem>2017</asp:ListItem>
            <asp:ListItem>2018</asp:ListItem>
            <asp:ListItem>2019</asp:ListItem>
            <asp:ListItem>2020</asp:ListItem>
        </asp:DropDownList>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 月份：
        <asp:DropDownList ID="drpMonth" runat="server">
            <asp:ListItem>1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>11</asp:ListItem>
            <asp:ListItem>12</asp:ListItem>
        </asp:DropDownList>
    </div>
    <div style="height: 8px;">
    </div>
    <div>
        <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpload"
            runat="server" Text=" 导 入 " OnClick="btnUpload_Click" /><br />
        <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
    </div>
    </form>
</body>
</html>
