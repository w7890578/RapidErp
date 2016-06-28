<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NonDeliveryCompareList.aspx.cs"
    Inherits="Rapid.SellManager.NonDeliveryCompareList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>未交清单对比表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            height: 100%;
        }
        .border tr td
        {
            background-color: White;
        }
    </style>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#btnImp").click(function() {
                OpenDialog("ImpNonDeliveryCompare.aspx", "btnSearch", "320", "500");
            });
        });
    </script>

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
        .border_Header
        {
            width: 100%;
            font-size: 14px;
            text-align: left;
        }
        .border tr td
        {
            padding: 4px;
            background-color: White;
        }
        .border_Header tr td
        {
            padding: 4px;
            background-color: White;
            width: 50%;
        }
        a
        {
            color: Blue;
        }
        a:hover
        {
            color: Red;
        }
        #choosePrintClounm
        {
            position: absolute;
            top: 20px;
            left: 50px;
            background-color: White;
            width: 170px;
            border: 1px solid green;
            padding: 10px;
            font-size: 14px;
            display: none;
        }
        #choosePrintClounm ul
        {
            margin-bottom: 10px;
        }
        #choosePrintClounm div
        {
            text-align: center;
            color: Green;
        }
        #choosePrintClounm ul li
        {
            list-style: none;
            float: left;
            width: 100%;
            cursor: pointer;
        }
    </style>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="float: left;">
               <span id="spImp" runat="server"> <input type="button" value="导入" id="btnImp" class="button" /></span>&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="查询" CssClass="button" />&nbsp;&nbsp;
                <%-- <input type="button" value="返回" id="btnBack" class="button" />--%>
            </div>
            <table class="border_Header">
                <tr>
                    <td colspan="2" style="text-align: center; height: 20px">
                        <h1>
                            未交清单对比表</h1>
                    </td>
                </tr>
            </table>
            <table class="border" cellpadding="1" cellspacing="1">
                <thead>
                    <tr>
                        <td class="tdOperar_SupplierId">
                            供应商/供应工厂
                        </td>
                        <td class="tdOperar_OrderNumber">
                            采购凭证
                        </td>
                        <td class="tdOperar_RowNumber">
                            项目
                        </td>
                        <td class="tdOperar_CertificateDate">
                            凭证日期
                        </td>
                        <td class="tdOperar_CustomerProductNumber">
                            物料
                        </td>
                        <td class="tdOperar_ShortText">
                            短文本
                        </td>
                        <td class="tdOperar_OrderQty">
                            采购订单数量
                        </td>
                        <td class="tdOperar_DeliveryQty">
                            已交货数量
                        </td>
                        <td class="tdOperar_StillDeliveryQty">
                            仍要交货(数量)
                        </td>
                        <td class="tdOperar_CollectNumber">
                            汇总号
                        </td>
                        <td class="tdOperar_IsexitOrderNumber">
                            该采购凭证是否异常
                        </td>
                        <td class="tdOperar_RowNumber">
                            项目对比结果
                        </td>
                        <td class="tdOperar_Istrue">
                            仍要交货数量对比
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpList">
                        <ItemTemplate>
                            <tr>
                                <td class="tdOperar_SupplierId">
                                    <%#Eval("供应商")%>
                                </td>
                                <td class="tdOperar_OrderNumber">
                                    <%#Eval("采购凭证")%>
                                </td>
                                <td class="tdOperar_RowNumber">
                                    <%#Eval("项目")%>
                                </td>
                                <td class="tdOperar_CertificateDate">
                                    <%#Eval("凭证日期")%>
                                </td>
                                <td class="tdOperar_CustomerProductNumber">
                                    <%#Eval("物料")%>
                                </td>
                                <td class="tdOperar_ShortText">
                                    <%#Eval("短文本")%>
                                </td>
                                <td class="tdOperar_OrderQty">
                                    <%#Eval("采购订单数量")%>
                                </td>
                                <td class="tdOperar_DeliveryQty">
                                    <%#Eval("已交货数量")%>
                                </td>
                                <td class="tdOperar_StillDeliveryQty">
                                    <%#Eval("仍要交货数量")%>
                                </td>
                                <td class="tdOperar_CollectNumber">
                                    <%#Eval("汇总号")%>
                                </td>
                                <td class="tdOperar_Is">
                                    <%#Eval("该采购凭证是否异常")%>
                                </td>
                                <td class="tdOperar_Remark">
                                    <%#Eval("项目对比结果")%>
                                </td>
                                <td class="tdOperar_IsMeetDelivery">
                                    <%#Eval("仍要交货数量对比")%>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
