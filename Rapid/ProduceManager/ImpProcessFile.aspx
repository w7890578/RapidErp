<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImpProcessFile.aspx.cs" Inherits="Rapid.ProduceManager.ImpProcessFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>过程检验文件上传</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
            <tr>
                <th colspan="2" align="left">文件上传 &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
                </th>
            </tr>
            
            <tr>
                <td align="right">开工单号：
                </td>
                <td>
                    <span style="display:none;"><asp:Label runat="server" ID="lbId" ></asp:Label></span>
                    <asp:Label runat="server" ID="lbProductionOrderNumber"></asp:Label>
                 
                </td>
            </tr>
            <tr>
                <td align="right">产成品编号：
                </td>
                <td> 
                      <asp:Label runat="server" ID="lbProductNumber"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">版本：
                </td>
                <td> 
                       <asp:Label runat="server" ID="lbVersion"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">客户产成品编号：
                </td>
                <td> 
                     <asp:Label runat="server" ID="lbCustomerProductNumber"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">选择文件：
                </td>
                <td>
                    <asp:FileUpload ID="fuFileUrl" runat="server" CssClass="input" />
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">备注：
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtRemark"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right"></td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="修改" OnClick="btnSubmit_Click" CssClass="submit" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="取消" id="btnBack" class="submit" onclick="javascript: window.close(this);" />
                    <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="（*为必填项）"></asp:Label>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
