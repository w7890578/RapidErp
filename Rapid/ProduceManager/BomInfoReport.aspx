<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BomInfoReport.aspx.cs"
    Inherits="Rapid.ProduceManager.BomInfoReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>BOM信息报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <style type="text/css">
            .border {
                background-color: Black;
                width: 1800px;
                font-size: 14px;
                text-align: center;
            }

                .border tr td {
                    padding: 4px;
                    background-color: White;
                }

            a {
                color: Blue;
            }

                a:hover {
                    color: Red;
                }
        </style>
        <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px; margin-bottom: 20px;">
            BOM信息报表
        </div>
        <div>
            <input type="hidden" id="hdnumber" runat="server" />
            <div id="divHeader" style="padding: 10px;">
                <div style="margin-bottom: 10px; width: 1700px;">
                    &nbsp;&nbsp; 包&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp;号：
                <asp:TextBox ID="txtPageNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    产&nbsp;成&nbsp;品&nbsp;编&nbsp;号：
                <asp:TextBox ID="txtProductNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    版&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 本：
                <asp:TextBox ID="txtVerison" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    客户产成品编号：
                <asp:TextBox ID="txtCutomerProductNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    产品描述：
                <asp:TextBox ID="txtDescription" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                </div>
                <div style="width: 1700px;">
                    &nbsp;&nbsp;原材料编号：
                <asp:TextBox ID="txtMaterialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    客户物料编号：
                <asp:TextBox ID="txtCustomerMaterialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    物料描述：
                <asp:TextBox ID="txtMaterialDescription" runat="server" Style="margin-right: 20px;"></asp:TextBox>

                    成品类别：
                <asp:TextBox ID="txtType" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    客户包号：
                <asp:TextBox ID="txtCustomerPackNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />&nbsp;&nbsp;
                 <%
                     bool exp = Rapid.ToolCode.Tool.GetUserMenuFunc("L0114", "exp");
                 %>
                    <span style="display: <%=exp?"":"none"%>;">
                        <asp:Button ID="btnExp" runat="server" Text="导出Excel"
                            OnClick="btnExp_Click" />
                    </span>
                    &nbsp;
                </div>
            </div>
            <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
                <thead>
                    <tr>
                        <td>包号
                        </td>
                        <td>客户包号(图纸号)
                        </td>
                        <td>产成品编号
                        </td>
                        <td>版本
                        </td>
                        <td>客户产成品编号
                        </td>
                        <td>成品类别
                        </td>
                        <td>产品描述
                        </td>
                        <td>原材料编号
                        </td>
                        <td>客户物料编号
                        </td>
                        <td>物料描述
                        </td>
                        <td>单机用量
                        </td>
                        <td>单位
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="Repeater1">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Eval("包号")%>
                                </td>
                                <td>
                                    <%#Eval("客户包号")%>
                                </td>
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
                                    <%#Eval("成品类别")%>
                                </td>
                                <td>
                                    <%#Eval("产品描述")%>
                                </td>
                                <td>
                                    <%#Eval("原材料编号")%>
                                </td>
                                <td>
                                    <%#Eval("客户物料号")%>
                                </td>
                                <td>
                                    <%#Eval("物料描述")%>
                                </td>
                                <td>
                                    <%#Eval("单机用量新")%>
                                </td>
                                <td>
                                    <%#Eval("单位新")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>
                        <td align="center" colspan="12" style="padding: 2px;">
                            <asp:LinkButton ID="lbtnFirstPage" runat="server" OnClick="lbtnFirstPage_Click">页首</asp:LinkButton>
                            <asp:LinkButton ID="lbtnpritPage" runat="server" OnClick="lbtnpritPage_Click">上一页</asp:LinkButton>
                            <asp:LinkButton ID="lbtnNextPage" runat="server" OnClick="lbtnNextPage_Click">下一页</asp:LinkButton>
                            <asp:LinkButton ID="lbtnDownPage" runat="server" OnClick="lbtnDownPage_Click">页尾</asp:LinkButton>&nbsp;&nbsp;
                    第<asp:Label ID="labPage" runat="server" Text="Label"></asp:Label>页/共<asp:Label ID="LabCountPage"
                        runat="server" Text="Label"></asp:Label>页
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </form>
</body>
</html>
