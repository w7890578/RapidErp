<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MaterialInfoReport.aspx.cs"
    Inherits="Rapid.ProduceManager.MaterialInfoReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>原材料信息报表</title>
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
            width: 2000px;
            font-size: 14px;
            text-align: center;
        }
        .border tr td
        {
            padding: 1px;
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
    <div style="text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        原材料信息报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px; width: 1500px;">
            <div style="margin-bottom: 10px;">
                &nbsp;&nbsp; 原材料编号：
                <asp:TextBox ID="txtMaterialNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                客户物料编号：
                <asp:TextBox ID="txtCustomerMateialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                客户名称：
                <asp:TextBox ID="txtCustomerName" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                供应商物料编号：
                <asp:TextBox ID="txtSupplierMaterialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <span>&nbsp;&nbsp; 供应商名称：
                    <asp:TextBox ID="txtSupplierName" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                </span>
            </div>
            <div>
                &nbsp&nbsp;描 &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp;述：
                <asp:TextBox ID="txtDescription" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                种 &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;类：
                <asp:TextBox ID="txtKind" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                类&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;别：
                <asp:TextBox ID="txtType" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                 货物类型：
                <asp:TextBox ID="txtHWtype" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 10px;" />
                    <%
                        bool exp = Rapid.ToolCode.Tool.GetUserMenuFunc("L0115", "exp");
                 %>
                    <span style="display: <%=exp?"":"none"%>;">
                 <asp:Button ID="btnEmp" runat="server" Text="导出Excel" OnClick="btnEmp_Click" />
                        </span>
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td style="width: 150px">
                        原材料编号
                    </td>
                    <td style="width: 250px">
                        客户物料编号
                    </td>
                    <td style="width: 250px">
                        客户名称
                    </td>
                    <td style="width: 150px">
                        供应商物料编号
                    </td>
                    <td>
                        供应商单价
                    </td>
                    <td style="width: 250px">
                        供应商名称
                    </td>
                    <td style="width: 250px">
                        描述
                    </td>
                    <td>
                        种类
                    </td>
                    <td>
                        品牌
                    </td>
                    <td>
                        库存安全值
                    </td>
                    <td>
                        最小包装
                    </td>
                    <td>
                        最小起订量
                    </td>
                    <td>
                        货位
                    </td>
                    <td>
                        货物类型
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("原材料编号")%>
                            </td>
                            <td>
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td>
                                <%#Eval("客户名称")%>
                            </td>
                            <td>
                                <%#Eval("供应商物料编号").ToString ().Trim ()%>
                            </td>
                            <td>
                                <%#Eval("供应商单价")%>
                            </td>
                            <td>
                                <%#Eval("供应商名称")%>
                            </td>
                            <td>
                                <%#Eval("描述").ToString ().Trim ()%>
                            </td>
                            <td>
                                <%#Eval("种类")%>
                            </td>
                            <td>
                                <%#Eval("品牌")%>
                            </td>
                            <td>
                                <%#Eval("库存安全值")%>
                            </td>
                            <td>
                                <%#Eval("最小包装")%>
                            </td>
                            <td>
                                <%#Eval("最小起订量")%>
                            </td>
                            <td>
                                <%#Eval("货位")%>
                            </td>
                            <td>
                                <%#Eval("货物类型")%>
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
