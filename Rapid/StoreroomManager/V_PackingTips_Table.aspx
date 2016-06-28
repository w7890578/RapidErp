<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="V_PackingTips_Table.aspx.cs"
    Inherits="Rapid.StoreroomManager.V_PackingTips_Table" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>未完销售订单（贸易）包装提示表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        function Edit(num, guid, qty) {
            //alert(guid);
            if (!confirm("确认修改？")) {
                return false;
            }

            else {
                var num = prompt("请输入修改的完成数量！", "0"); //将输入的内容赋给变量 name ，

                //这里需要注意的是，prompt有两个参数，前面是提示的话，后面是当对话框出来后，在对话框里的默认值

                if (num)//如果返回的有内容
                {

                    $.get("V_PackingTips_Table.aspx?sq=" + new Date(), { Qty: qty, Guid: guid, Num: num, time: new Date() }, function(result) {
                        if (result == "0") {
                            alert("已完成数量不能大于订单数量！");
                            return false;
                        }
                        if (result == "1") {
                            alert("修改成功！");
                            $("#btnSearch").click();
                        }
                        else { alert("修改失败！原因：" + result); }

                    });

                }
            }

        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 1500px;">
        <div style="text-align: center; font-size: 25px; margin: 10px;">
            未完销售订单（贸易）包装提示表
        </div>
        <div style="margin-bottom: 10px; margin-top: 10px;">
            <asp:Label ID="Label1" runat="server" Text="客户名称：" Style="margin-left: 10px;"></asp:Label>
            <asp:TextBox ID="txtCustomerName" runat="server" Style="margin-right: 10px;"></asp:TextBox>
            <asp:Label ID="Label2" runat="server" Text="交期："></asp:Label>
            <asp:TextBox ID="txtDate" runat="server" onfocus="WdatePicker({skin:'green'})" Style="margin-right: 10px;"></asp:TextBox>
            <asp:Label ID="Label3" runat="server" Text="原材料编号："></asp:Label>
            <asp:TextBox ID="txtMateriNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
            <asp:Label ID="Label4" runat="server" Text="客户物料编号："></asp:Label>
            <asp:TextBox ID="txtCustomerMateriNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="查询" Style="margin-right: 10px;" OnClick="btnSearch_Click" />
            <asp:Button ID="btnEmp" runat="server" Text="导出Excel" OnClick="btnEmp_Click" />
        </div>
        <table cellpadding="1" cellspacing="1" class="border">
            <tr>
                <td>
                    销售订单号
                </td>
                <td>
                    客户名称
                </td>
                <td>
                    原材料编号
                </td>
                <td>
                    客户物料编号
                </td>
                <td>
                    行号
                </td>
                <td>
                    物料描述
                </td>
                <td>
                    订单总数量
                </td>
                <td>
                    未交数量
                </td>
                 <td>
                    已交数量
                </td>
                <td>
                    已完成数量
                </td>
                <td>
                    交期
                </td>
                <td>
                    操作
                </td>
            </tr>
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td>
                            <%#Eval("销售订单号")%>
                        </td>
                        <td>
                            <%#Eval("客户名称")%>
                        </td>
                        <td>
                            <%#Eval("原材料编号")%>
                        </td>
                        <td>
                            <%#Eval("客户物料编号")%>
                        </td>
                        <td>
                            <%#Eval("行号")%>
                        </td>
                        <td>
                            <%#Eval("原材料描述")%>
                        </td>
                        <td>
                            <%#Eval("数量")%>
                        </td>
                        <td>
                            <%#Eval("未交数量")%>
                        </td>
                         <td>
                            <%#Eval("已交数量")%>
                        </td>
                        <td>
                            <%#Eval("已完成数量")%>
                        </td>
                        <td>
                            <%#Eval("销售订单号").ToString().Equals("合计") ? "" : Eval("交期")%>
                        </td>
                        <td>
                            <a href="###" onclick="Edit('<%#Eval("已完成数量")%>','<%#Eval("Guid") %>','<%#Eval("数量")%>')">
                                <%#Eval("销售订单号").ToString().Equals("合计") ? "" : "编辑"%>
                            </a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    </form>
</body>
</html>
