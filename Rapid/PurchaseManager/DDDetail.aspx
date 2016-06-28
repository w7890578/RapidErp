<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DDDetail.aspx.cs" Inherits="Rapid.PurchaseManager.DDDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 1800px;
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
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center; font-size: 20px; font-weight: bold; margin-bottom: 5px;">
        订单需求明细</div>
    <div style="text-align: center; margin-bottom: 5px;">
        <a href="SupplyAndDemandBalanceList.aspx">返回列表</a></div>
    <div>
        <table cellpadding="1" cellspacing="1" class="border">
            <thead>
                <tr>
                    <td>
                        生产类型
                    </td>
                    <td>
                        订单类型
                    </td>
                    <td>
                           <asp:LinkButton runat="server" ID="lborder" OnClick="lborder_Click"  ></asp:LinkButton>
                    </td>
                    <td>
                        客户采购订单号
                    </td>
                    <td>
                        产成品编号
                    </td>
                    <td>
                        客户产成品编号
                    </td>
                    <td>
                        版本
                    </td>
                    <td>
                        原材料编号
                    </td>
                    <td>
                        客户物料号
                    </td>
                    <td>
                        物料描述
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lbjq" OnClick="lbjq_Click"    ></asp:LinkButton>
                    </td>
                    <td>
                        行号
                    </td>
                    <td>
                        订单未交数量
                    </td>
                    <td>
                        单机用量
                    </td>
                    <td>
                        单位
                    </td>
                    <td>
                         <asp:LinkButton runat="server" ID="lbordernum" OnClick="lbordernum_Click"  ></asp:LinkButton>
                        
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rpList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("生产类型") %>
                            </td>
                            <td>
                                <%#Eval("订单类型") %>
                            </td>
                            <td>
                                <%#Eval("销售订单号") %>
                            </td>
                            <td>
                                <%#Eval("客户采购订单号") %>
                            </td>
                            <td>
                                <%#Eval("产成品编号") %>
                            </td>
                            <td>
                                <%#Eval("客户产成品编号") %>
                            </td>
                            <td>
                                <%#Eval("版本") %>
                            </td>
                            <td>
                                <%#Eval("原材料编号") %>
                            </td>
                            <td>
                                <%#Eval("客户物料号") %>
                            </td>
                            <td>
                                <%#Eval("物料描述") %>
                            </td>
                            <td>
                                <%#Eval("交期") %>
                            </td>
                            <td>
                                <%#Eval("行号") %>
                            </td>
                            <td>
                                <%#Eval("订单未交数量") %>
                            </td>
                            <td>
                                <%#Eval("单机用量") %>
                            </td>
                            <td>
                                <%#Eval("单位") %>
                            </td>
                            <td>
                                <%#Eval("订单需求数") %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <tr>
                    <td>
                        合计
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <%=count %>
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
    </form>
</body>
</html>
