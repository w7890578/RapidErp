<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnfinishSaleTradingBZReport.aspx.cs" Inherits="Rapid.StoreroomManager.UnfinishSaleTradingBZReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>未完销售订单（贸易）包装提示表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

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
            padding: 2px;
            background-color: White;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-bottom: 10px;">
        <div style="font-size: 28px; font-weight: bold; text-align: center; margin-top: 30px;">
           未完销售订单（贸易）包装提示表</div>
        <br />
        <br />
       
        <asp:Button runat="server" Text="查询" ID="btnSearch" OnClick="btnSearch_Click" Style="margin-left: 10px;
            margin-right: 10px;" />
        <asp:Button ID="btnEmp" runat="server" Text="导出" OnClick="btnEmp_Click" />
    </div>
    <div>
        <table cellpadding="1" cellspacing="1" class="border">
            <thead>
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
                        行号
                    </td>
                    <td>
                        客户物料编号
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
                        完成数量
                    </td>
                    <td>
                        备注
                    </td>
                    <td>
                        交期
                    </td>
                    <td>
                        本次包装数量
                    </td>
                   
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
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
                                <%#Eval("行号")%>
                            </td>
                            <td>
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td>
                                <%#Eval("物料描述")%>
                            </td>
                            <td>
                                <%#Eval("订单总数量")%>
                            </td>
                            <td>
                                <%#Eval("未交数量")%>
                            </td>
                            <td>
                                <%#Eval("完成数量")%>
                            </td>
                            <td>
                                <%#Eval("备注")%>
                            </td>
                            <td>
                                <%#Eval("交期")%>
                            </td>
                            <td>
                                <%#Eval("本次包装数量")%>
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
