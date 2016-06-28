<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceAccountReport.aspx.cs"
    Inherits="Rapid.FinancialManager.InvoiceAccountReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>送货单明细报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Delete(guid) {
            if (confirm("确定删除？")) {
                var number = $("#hdnumber").val();
                $.ajax({
                    type: "Get",
                    url: "InvoiceAccountReport.aspx",
                    data: { time: new Date(), Guid: guid, IsDelete: "true" },
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
            $("#btnImp").click(function() {
                window.location.href = "ImpInvoiceAccountInfo.aspx";
            });
        })
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 100%;">
        <div style="text-align: center; font-size: 25px; margin: 10px;">
            发票登记表</div>
        <div style="margin-bottom: 10px;">
            &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;发票号码：<asp:TextBox ID="txtInvoiceNumber"
                runat="server"></asp:TextBox>&nbsp&nbsp;&nbsp&nbsp; 客户名称：<asp:TextBox ID="txtCustomerName"
                    runat="server"></asp:TextBox>&nbsp&nbsp;&nbsp&nbsp; 是否已收款：<asp:DropDownList ID="drpIsPay"
                        runat="server">
                        <asp:ListItem Value="" Text=""> - - - 请选择 - - - </asp:ListItem>
                        <asp:ListItem Value="是" Text="是"></asp:ListItem>
                        <asp:ListItem Value="否" Text="否"></asp:ListItem>
                    </asp:DropDownList>
            &nbsp&nbsp;&nbsp&nbsp; 发票类型：<asp:DropDownList ID="drpInvoiceType" runat="server"
                Style="margin-right: 10px;">
                <asp:ListItem Value="" Text=""> - - - 请选择 - - - </asp:ListItem>
                <asp:ListItem Value="专用" Text="专用"></asp:ListItem>
                <asp:ListItem Value="普通" Text="普通"></asp:ListItem>
            </asp:DropDownList>
            <asp:Button runat="server" ID="btnSearch" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 10px;" />
          <%--  <asp:Button ID="btnImp" runat="server" Text="导入" />--%>
          <input id="btnImp" type="button" value="导入" />
        </div>
        <table cellpadding="1" cellspacing="1" class="border">
            <tr>
            
                <td>
                    序号
                </td>
                <td>
                    发票代码
                </td>
                <td>
                    发票号码
                </td>
                <td>
                    开票日期
                </td>
                <td>
                    客户名称
                </td>
                <td>
                    不含税金额
                </td>
                <td>
                    税额
                </td>
                <td>
                    开票金额
                </td>
                <td>
                    是否已收款
                </td>
                <td>
                    发票类型
                </td>
                <td>
                    操作
                </td>
            </tr>
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("序号")%>
                        </td>
                        <td>
                            <%#Eval("发票代码")%>
                        </td>
                        <td>
                            <%#Eval("发票号码")%>
                        </td>
                        <td>
                            <%#Eval("开票日期")%>
                        </td>
                        <td>
                            <%#Eval("客户名称")%>
                        </td>
                        <td>
                            <%#Eval("不含税金额")%>
                        </td>
                        <td>
                            <%#Eval("税额")%>
                        </td>
                        <td>
                            <%#Eval("发票金额")%>
                        </td>
                        <td>
                            <%#Eval("是否已收款")%>
                        </td>
                        <td>
                            <%#Eval("发票类型")%>
                        </td>
                        <td>
                            <a href="###" onclick="Delete('<%#Eval("Guid") %>')">删除</a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            <tr>
                <td align="center" colspan="13" style="padding: 2px;">
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
