<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDeliveryDetailList.aspx.cs"
    Inherits="Rapid.SellManager.CustomerDeliveryDetailList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客户提前要货明细</title>
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
        function edit(guid) {
            OpenDialog("EditCustomerDeliveryDetail.aspx?Guid=" + guid, "btnSearch", "550", "550");
        }
        $(document).ready(function() {
            $("#btnImp").click(function() {
                OpenDialog("ImpCustomerDeliveryDetail.aspx", "btnSearch", "320", "500");
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
                <input type="button" value="导入" id="btnImp" class="button" />&nbsp;&nbsp;
                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="查询" CssClass="button" />&nbsp;&nbsp;
                <%-- <input type="button" value="返回" id="btnBack" class="button" />--%>
            </div>
            <table class="border_Header">
                <tr>
                    <td colspan="2" style="text-align: center; height: 20px">
                        <h1>
                            客户提前要货明细</h1>
                    </td>
                </tr>
            </table>
            <table class="border" cellpadding="1" cellspacing="1">
                <thead>
                    <tr>
                        <td class="tdOperar_Guid" style="display: none;">
                            Guid
                        </td>
                        <td class="tdOperar_ImportTime">
                            导入时间
                        </td>
                        <td class="tdOperar_CustomerId">
                            客户编号
                        </td>
                        <td class="tdOperar_Number">
                            物料
                        </td>
                        <td class="tdOperar_Description">
                            描述
                        </td>
                        <td class="tdOperar_AdvanceQty">
                            需提前交货数量
                        </td>
                        <td class="tdOperar_ReplyDate">
                            供应商回复交货日期
                        </td>
                        <td class="tdOperar_WipQty">
                            在制品数量
                        </td>
                        <td class="tdOperar_NonDeliveryQty">
                            未交订单数量
                        </td>
                        <td class="tdOperar_StockQty">
                            库存数量
                        </td>
                        <td class="tdOperar_IsMeetDelivery">
                            是否满足交货
                        </td>
                        <td class="tdOperar_OrderContrast">
                            订单对比结果
                        </td>
                        <td class="tdOperar_Remark">
                            备注
                        </td>
                        <td class="tdOperar">
                            操作
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpList">
                        <ItemTemplate>
                            <tr>
                                <td class="tdOperar_Guid" style="display: none;">
                                    <%#Eval("Guid")%>
                                </td>
                                <td class="tdOperar_ImportTime">
                                    <%#Eval("导入时间")%>
                                </td>
                                <td class="tdOperar_CustomerId">
                                    <%#Eval("客户编号")%>
                                </td>
                                <td class="tdOperar_Number">
                                    <%#Eval("物料")%>
                                </td>
                                <td class="tdOperar_Description">
                                    <%#Eval("描述")%>
                                </td>
                                <td class="tdOperar_AdvanceQty">
                                    <%#Eval("需提前交货数量")%>
                                </td>
                                <td class="tdOperar_ReplyDate">
                                    <%#Eval("供应商回复交货日期")%>
                                </td>
                                <td class="tdOperar_WipQty">
                                    <%#Eval("在制品数量")%>
                                </td>
                                <td class="tdOperar_NonDeliveryQty">
                                    <%#Eval(" 未交订单数量")%>
                                </td>
                                <td class="tdOperar_StockQty">
                                    <%#Eval("库存数量")%>
                                </td>
                                <td class="tdOperar_IsMeetDelivery">
                                    <%#Eval("是否满足交货")%>
                                </td>
                                <td class="tdOperar_OrderContrast">
                                    <%#Eval(" 订单对比结果")%>
                                </td>
                                <td class="tdOperar_Remark">
                                    <%#Eval("备注")%>
                                </td>
                                <td class="tdOperar">
                                    <span style="display: <%=hasEdit%>;"><a href="###" onclick="edit('<%#Eval("Guid")%>')">
                                        编辑</a></span>
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
