<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZTDetail.aspx.cs" Inherits="Rapid.PurchaseManager.ZTDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>在途采购明细</title>
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
        margin-top: 20px; margin-bottom: 10px;">
        在途采购明细</div>
    <div style="width: 100%; text-align: center; margin-bottom: 5px; font-size: 16px;
        color: blue;">
        <a href="SupplyAndDemandBalanceList.aspx">返回列表</a></div>
    <div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td>
                        采购订单号
                    </td>
                    <td>
                        原材料编号
                    </td>
                    <td>
                        交期
                    </td>
                    <td>
                        行号
                    </td>
                    <td>
                        供应商物料编号
                    </td>
                    <td>
                        订单数量
                    </td>
                    <td>
                        未交数量
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("采购订单号")%>
                            </td>
                            <td>
                                <%#Eval("原材料编号")%>
                            </td>
                            <td>
                                <%#Eval("交期")%>
                            </td>
                            <td>
                                <%#Eval("行号")%>
                            </td>
                            <td c>
                                <%#Eval("供应商物料编号")%>
                            </td>
                            <td>
                                <%#Eval("订单数量")%>
                            </td>
                            <td>
                                <%#Eval("未交数量")%>
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
