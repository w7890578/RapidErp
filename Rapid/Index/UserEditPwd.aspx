<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEditPwd.aspx.cs" Inherits="Rapid.Index.UserEditPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>个人密码修改</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center;margin :10px;" >
        <table>
            <tr>
                <td align="right">
                    当前密码：
                </td>
                <td>
                    <asp:TextBox ID="txtPwdOld" runat="server" TextMode ="Password" ></asp:TextBox><label style="color: Red;">*</label> 
                </td>
            </tr>
            <tr>
                <td align="right">
                    新密码：
                </td>
                <td>
                    <asp:TextBox ID="txtPwdNew" runat="server" TextMode ="Password"></asp:TextBox><label style="color: Red;">*</label> 
                </td>
            </tr>
            <tr>
                <td align="right">
                    确认密码：
                </td>
                <td>
                    <asp:TextBox ID="txtPwdNewAgain" runat="server" TextMode ="Password"></asp:TextBox><label style="color: Red;">*</label> 
                </td>
            </tr>
            <tr>
                <td colspan="2"><asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;font-size :12px;"></asp:Label>
                    <asp:Button ID="btnSubmit" runat="server" Text=" 确 定 " OnClick="btnSubmit_Click" />
                    
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
