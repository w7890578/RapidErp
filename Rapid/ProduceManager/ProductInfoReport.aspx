<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductInfoReport.aspx.cs"
    Inherits="Rapid.ProduceManager.ProductInfoReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>产品信息报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border tr td
        {
            padding: 4px;
            background-color: White;
        }
        a
        {
            color: Blue;
        }
        a:hover
        {
            color: Red;
        }
    </style>
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        产品信息报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="margin-bottom:10px;width :1500px;">
                &nbsp;&nbsp; 产成品编号：
                <asp:TextBox ID="txtProductNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                &nbsp;&nbsp; 版&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 本：
                <asp:TextBox ID="txtVerison" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                客户产成品编号：
                <asp:TextBox ID="txtCustomerProductNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                客户名称：
                <asp:TextBox ID="txtCustomerName" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                &nbsp;&nbsp; 描 &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp; 述：
                <asp:TextBox ID="txtDescription" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                </div>
                 
                <div>
                &nbsp;&nbsp; 种 &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp; 类：
                <asp:TextBox ID="txtKind" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                成品类别：
                <asp:TextBox ID="txtType" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click"  style="margin-right:10px;"/>
                
                     <%
                         bool exp = Rapid.ToolCode.Tool.GetUserMenuFunc("L0116", "exp");
                 %>
                    <span style="display: <%=exp?"":"none"%>;">
                    <asp:Button ID="btnEmp" runat="server" Text="导出Excel" onclick="btnEmp_Click" />
                        </span>
                &nbsp;
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
            <thead>
                <tr>
                    <td>
                        产成品编号
                    </td>
                    <td>
                        版本
                    </td>
                    <td>
                        客户产成品编号
                    </td>
                    <td>
                        客户名称
                    </td>
                    <td>
                        描述
                    </td>
                    <td>
                        种类
                    </td>
                    <td>
                        成品类别
                    </td>
                    <td>
                        额定工时
                    </td>
                    <td>
                        货位
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("产成品编号")%>
                            </td>
                            <td>
                                <%#Eval("版本")%>
                            </td>
                            <td>
                                <%#Eval("客户产成品编号")%>
                            </td>
                            <td>
                                <%#Eval("客户名称")%>
                            </td>
                            <td>
                                <%#Eval("描述")%>
                            </td>
                            <td>
                                <%#Eval("种类")%>
                            </td>
                            <td>
                                <%#Eval("成品类别")%>
                            </td>
                            <td>
                                <%#Eval("额定工时")%>
                            </td>
                            <td>
                                <%#Eval("货位")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
