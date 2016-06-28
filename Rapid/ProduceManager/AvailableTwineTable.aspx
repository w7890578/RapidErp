<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AvailableTwineTable.aspx.cs"
    Inherits="Rapid.ProduceManager.AvailableTwineTable" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>可利用废线信息表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function Edit(guid) {
            guid = encodeURIComponent(guid);
            OpenDialog("../ProduceManager/AddOrEditAvailableTwineTable.aspx?Guid=" + guid, "btnSearch", "500", "600");
        }
        function Delete(guid) {
            if (confirm("确定删除？")) {
                $.ajax({
                    type: "Get",
                    url: "AvailableTwineTable.aspx?time=" + new Date(),
                    data: { Guid: guid, IsDelete: "true" },
                    success: function(result) {
                        if (result == "1") {
                            alert("删除成功！");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("删除失败！原因：" + result);
                            return;
                        }
                    }
                });
            }
        }
        $(function() {
            
            $("#btnAdd").click(function() {
                OpenDialog("../ProduceManager/AddOrEditAvailableTwineTable.aspx", "btnSearch", "500", "600");
            });
        })
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%;">
        <div style="text-align: center; font-size: 25px; margin: 10px;">
            可利用废线表</div>
        <div style="margin-bottom: 10px;">
            &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;原材料编号：<asp:TextBox ID="txtMaterialNumber"
                runat="server"></asp:TextBox>&nbsp&nbsp;&nbsp&nbsp; 客户物料编号：<asp:TextBox ID="txtCustomerMaterialNumber"
                    runat="server"></asp:TextBox>
            <asp:Button runat="server" ID="btnSearch" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 10px;" />
            <%--  <asp:Button ID="btnImp" runat="server" Text="导入" />--%>
            <input id="btnAdd" type="button" value="增加" />
        </div>
        <table cellpadding="1" cellspacing="1" class="border">
            <tr>
                <td>
                    原材料编号
                </td>
                <td>
                    客户物料编号
                </td>
                <td>
                    长度
                </td>
                <td>
                    数量
                </td>
                <td>
                    描述
                </td>
                <td>
                    备注
                </td>
                <td>
                    操作
                </td>
            </tr>
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("原材料编号")%>
                        </td>
                        <td>
                            <%#Eval("客户物料编号")%>
                        </td>
                        <td>
                            <%#Eval("长度")%>
                        </td>
                        <td>
                            <%#Eval("数量")%>
                        </td>
                        <td>
                            <%#Eval("描述")%>
                        </td>
                        <td>
                            <%#Eval("备注")%>
                        </td>
                        <td>
                            <a href="###" onclick="Edit('<%#Eval("Guid") %>')">编辑</a> <a href="###" onclick="Delete('<%#Eval("Guid") %>')">
                                删除</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td align="center" colspan="7" style="padding: 2px;">
                    <asp:LinkButton ID="lbtnFirstPage" runat="server" OnClick="lbtnFirstPage_Click">页首</asp:LinkButton>
                    <asp:LinkButton ID="lbtnpritPage" runat="server" OnClick="lbtnpritPage_Click">上一页</asp:LinkButton>
                    <asp:LinkButton ID="lbtnNextPage" runat="server" OnClick="lbtnNextPage_Click">下一页</asp:LinkButton>
                    <asp:LinkButton ID="lbtnDownPage" runat="server" OnClick="lbtnDownPage_Click">页尾</asp:LinkButton>&nbsp;&nbsp;
                    第<asp:Label ID="labPage" runat="server" Text="Label"></asp:Label>页/共<asp:Label ID="LabCountPage"
                        runat="server" Text="Label"></asp:Label>页
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
