<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TradingOrderDetail.aspx.cs"
    Inherits="Rapid.SellManager.TradingOrderDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>贸易销售订单明细</title>
    <%--<link href="../Css/Main.css" rel="stylesheet" type="text/css" />--%>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function edit(number, rownumber, ProductNumber) {
            OpenDialog("AddOrEditTradingOrderDetail.aspx?OdersNumber=" + number + "&RowNumber=" + rownumber + "&ProductNumber=" + ProductNumber, "btnSearch", "500", "600");
        }
        function Delete(number, rownumber, productNumber) {
            if (confirm("确定删除？")) {
                var number = $("#hdnumber").val();
                $.ajax({
                    type: "Get",
                    url: "TradingOrderDetail.aspx",
                    data: { time: new Date(), OdersNumber: number, RowNumber: rownumber, ProductNumber: productNumber, id: number, IsDelete: "true" },
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


        function EditQty(ordersNumber, rownumber,productnumber ) {
            var qty = prompt("填写数量", "");
            if (qty == null || qty == "") {
                return false;
            }
            $.ajax({
                type: "Post",
                url: "TradingOrderDetail.aspx",
                data: { time: new Date(), ordersNumber: ordersNumber, ProductNumber: productnumber, RowNumber: rownumber, qty: qty, isEditQty: "true" },
                success: function(result) {
                    if (result == "1") {
                        alert("编辑成功！");
                        $("#btnSearch").click();
                    }
                    else {
                        alert("编辑失败！原因：" + result);
                        return;
                    }
                }
            });
        }
        //EditYJQty编辑已交数量

        function EditYJQty(ordersNumber, rownumber, productnumber) {
            var qty = prompt("填写数量", "");
            if (qty == null || qty == "") {
                return false;
            }
            $.ajax({
                type: "Post",
                url: "TradingOrderDetail.aspx",
                data: { time: new Date(), ordersNumber: ordersNumber, ProductNumber: productnumber, RowNumber: rownumber, qty: qty, isEditYJQty: "true" },
                success: function(result) {
                    if (result == "1") {
                        alert("编辑成功！");
                        $("#btnSearch").click();
                    }
                    else {
                        alert("编辑失败！原因：" + result);
                        return;
                    }
                }
            });
        }
        $(function() {
            $("#btnAdd").click(function() {
                var odersNumber = $("#hdnumber").val();
                OpenDialog("../SellManager/AddOrEditTradingOrderDetail.aspx?OdersNumber=" + odersNumber, "btnSearch", "500", "600");
            });
            $("#btnPrint").click(function() {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function() {
                $("#choosePrintClounm").hide();
            });
            $("#btnBack").click(function() {
                window.location.href = "SaleOderList.aspx";
            });
            $("#btnChoosePrintColum").click(function() {
                var chooseResult = "";
                var unChooseResult = "";
                var arrChk = $("input[name='columList']:checkbox");
                $(arrChk).each(function() {
                    if ($(this).is(':checked')) {
                        chooseResult += $(this).val() + ",";
                    }
                    else {
                        unChooseResult += $(this).val() + ",";
                    }
                });
                var reg = /,$/gi;
                chooseResult = chooseResult.replace(reg, "");
                unChooseResult = unChooseResult.replace(reg, "");
                var unChoosedArray = unChooseResult.split(',');

                if (chooseResult == "") {
                    alert("请选择要打印的列!");
                    return;
                }
                if (!confirm("确定打印所选列？")) {
                    return;
                }
                //遍历border样式的table下的td 
                $(".border tr td").each(function() {
                    className = $(this).attr("class");
                    if (className == "tdOperar") {
                        $(this).hide();
                    }
                    for (var j = 0; j < unChoosedArray.length; j++) {
                        if (className == unChoosedArray[j] + "") {
                            $(this).hide();
                        }
                    }
                });
                newwin = window.open("", "newwin", "height=900,width=750,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
                newwin.document.body.innerHTML = document.getElementById("form1").innerHTML;
                newwin.document.getElementById("divHeader").style.display = 'none';
                newwin.document.getElementById("choosePrintClounm").style.display = 'none';
                newwin.window.print();
                newwin.window.close();
                $("#choosePrintClounm").hide();
                $(".border tr td").each(function() {
                    $(this).show();
                })
            });

            $("#btnBack").click(function() {
                window.location.href = "SaleOderList.aspx";
            });
        })
        
        
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
        .border tr td
        {
            padding: 2px;
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
        .choose
        {
            background-color: Green;
        }
    </style>
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        贸易销售订单明细列表</div>
    <table class="border_Header">
        <tr>
            <td align="left">
                <img src="../Img/my/tading.png" />
            </td>
        </tr>
        <tr>
            <td class="style1">
                日期：<asp:Label ID="lblOrdersDate" runat="server" Text=""></asp:Label>
            </td>
            <td>
            </td>
            <td>
                合同号：<asp:Label ID="lblOdersNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">
                卖方：北京瑞普迪电子设备有限公司
            </td>
            <td>
            </td>
            <td>
                买方：
                <asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            &nbsp;&nbsp;
            <div style="float: left;">
                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="查询" CssClass="button" />
                &nbsp;&nbsp; <span id="spAdd" runat="server">
                    <input type="button" value="增加" id="btnAdd" class="button" style="display: <%=show%>;" /></span>
            </div>
            <div style="position: relative; float: left">
                &nbsp;&nbsp; <span id="spPrint" runat="server"></span>
                <input type="button" value="打印" id="btnPrint" class="button" /></span>
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_OdersNumber" checked="checked" />
                                销售订单号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_SN" checked="checked" />
                                序号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_ProductNumber" checked="checked" />
                                产成品编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_ProductModel" checked="checked" />
                                产品型号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_RowNumber" checked="checked" />
                                行号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_CustomerMaterialNumber" checked="checked" />
                                客户物料编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_MaterialName" checked="checked" />
                                物料描述</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_Brand" checked="checked" />
                                品牌</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_Quantity" checked="checked" />
                                数量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_NonDeliveryQty" checked="checked" />
                                未交货数量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_DeliveryQty" checked="checked" />
                                已交货数量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_UnitPrice" checked="checked" />
                                单价</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_TotalPrice" checked="checked" />
                                总价</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_Delivery" checked="checked" />
                                交期</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_Remark" checked="checked" />
                                备注</label>
                        </li>
                    </ul>
                    <div>
                        &nbsp;<br />
                        <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                            type="button" value=" 取 消 " id="btnExit" /></div>
                </div>
            </div>
            &nbsp;
            <input type="button" value="返回" id="btnBack" class="button" />
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td class="tdOperar_SN">
                        序号
                    </td>
                    <td class="tdOperar_OdersNumber">
                        销售订单号
                    </td>
                    <td class="tdOperar_ProductNumber">
                        原材料编号
                    </td>
                    <%-- <td class="tdOperar_ProductModel">
                        产品型号
                    </td>--%>
                    <td class="tdOperar_RowNumber">
                        行号
                    </td>
                    <td class="tdOperar_CustomerMaterialNumber">
                        客户物料编号
                    </td>
                    <td class="tdOperar_MaterialName">
                        物料描述
                    </td>
                    <td class="tdOperar_Brand">
                        品牌
                    </td>
                    <td class="tdOperar_Quantity">
                        数量
                    </td>
                    <td class="tdOperar_NonDeliveryQty">
                        未交货数量
                    </td>
                    <td class="tdOperar_DeliveryQty">
                        已交货数量
                    </td>
                    <td class="tdOperar_UnitPrice">
                        单价
                    </td>
                    <td class="tdOperar_TotalPrice">
                        总价
                    </td>
                    <td class="tdOperar_Delivery">
                        交期
                    </td>
                    <td class="tdOperar_Status">
                        订单状态
                    </td>
                    <td class="tdOperar_ReceiveOne" style="display: <%=rp%>;">
                        收款一
                    </td>
                    <td class="tdOperar_ReceiveTwo" style="display: <%=rp%>;">
                        收款二
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
                            <td class="tdOperar_SN">
                                <%#Eval("OdersNumber").ToString().Equals("合计") ? "" : Eval("numNew")%>
                            </td>
                            <td class="tdOperar_OdersNumber">
                                <%#Eval("OdersNumber")%>
                            </td>
                            <td class="tdOperar_ProductNumber">
                                <%#Eval("ProductNumber")%>
                            </td>
                            <%--   <td class="tdOperar_ProductModel">
                                <%#Eval("ProductModel")%>
                            </td>--%>
                            <td class="tdOperar_RowNumber">
                                <%#Eval("OdersNumber").ToString ().Equals ("合计")?"": Eval("RowNumber") %>
                            </td>
                            <td class="tdOperar_CustomerMaterialNumber">
                                <%#Eval("CustomerMaterialNumber")%>
                            </td>
                            <td class="tdOperar_MaterialName">
                                <%#Eval("MaterialName")%>
                            </td>
                            <td class="tdOperar_Brand">
                                <%#Eval("Brand")%>
                            </td>
                            <td class="tdOperar_Quantity">
                                <%#Eval("Quantity")%>
                            </td>
                            <td class="tdOperar_NonDeliveryQty">
                                <%#Eval("NonDeliveryQty")%>
                            </td>
                            <td class="tdOperar_DeliveryQty">
                                <%#Eval("DeliveryQty")%>
                            </td>
                            <td class="tdOperar_UnitPrice">
                                <%#Eval("OdersNumber").ToString().Equals("合计") ? "" : Eval("UnitPrice")%>
                            </td>
                            <td class="tdOperar_TotalPrice">
                                <%#Eval("TotalPrice")%>
                            </td>
                            <td class="tdOperar_Delivery">
                                <span style="color: <%#Eval("color")%>;" title="交期异常：小于等于订单创建时间">
                                    <%#Eval("Delivery")%></span>
                            </td>
                            <td class="tdOperar_Status">
                                <%#Eval("Status")%>
                            </td>
                            <td class="tdOperar_ReceiveOne" style="display: <%=rp%>;">
                                <%#Eval("ReceiveOne")%>
                            </td>
                            <td class="tdOperar_ReceiveTwo" style="display: <%=rp%>;">
                                <%#Eval("ReceiveTwo")%>
                            </td>
                            <td class="tdOperar_Remark">
                                <%#Eval("Remark")%>
                            </td>
                            <td class="tdOperar">
                                <span style="display: <%#Eval("OdersNumber").ToString().Equals("合计") ? "none" : "inline"%>;">
                                    <span style="display: <%=hasEdit %>;"><span style="display: <%=show %>;"><a href="###"
                                        onclick="edit('<%#Eval("OdersNumber") %>','<%#Eval("RowNumber")%>','<%#Eval("ProductNumber")%>')">
                                        编辑</a></span></span> <span style="display: <%=hasDelete %>;"><span style="display: <%=show %>;">
                                            <a href="###" onclick="Delete('<%#Eval("OdersNumber") %>','<%#Eval("RowNumber")%>','<%#Eval("ProductNumber")%>')">
                                                删除</a></span></span></span>
                                                
                                                &nbsp;&nbsp;  &nbsp; &nbsp;<a href ="###" style="display:<%=userId=="sysAdmin"?"inline":"none" %>;" onclick="EditQty('<%#Eval("OdersNumber") %>','<%#Eval("RowNumber")%>','<%#Eval("ProductNumber")%>')">编辑数量</a>
                                                 &nbsp;&nbsp;  &nbsp; &nbsp;<a href ="###" style="display:<%=userId=="sysAdmin"?"inline":"none" %>;" onclick="EditYJQty('<%#Eval("OdersNumber") %>','<%#Eval("RowNumber")%>','<%#Eval("ProductNumber")%>')">编辑已交数量</a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <table class="border_Header">
        <tr>
            <td>
                1.ERP中的销售合同价格均为不含税价格。
            </td>
        </tr>
       <%-- <tr>
            <td>
                2.技术标准：按制造商产品的技术执行，如有质量问题，供方负责换货
            </td>
        </tr>
        <tr>
            <td>
                3.含运费：运至客户指定地点
            </td>
        </tr>
        <tr>
            <td>
                4.违约责任：按中华人民共和国合同法相关条款执行
            </td>
        </tr>
        <tr>
            <td>
                5.解决合同纠纷的方式：协商解决
            </td>
        </tr>
        <tr>
            <td>
                6.付款方式:预付30%，货到通知买方付尾款，完成发货.
            </td>
        </tr>
        <tr>
            <td>
                7.合同传真件有效
            </td>
        </tr>--%>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                THE SELLER，卖方 &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;THE
                BUYER,买方
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;
            </td>
        </tr>
    </table>
    ------------------------------------------------------------------------------------------------------------------------------------------
    <table class="border_Header">
        <tr>
            <td>
                公司名称：北京瑞普迪电子设备有限公司
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                公司名称：
                <asp:Label ID="lblCutomerId" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                联系电话：010-89401325
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                联系电话：
                <asp:Label ID="lblContactTelephone" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                传真：010-89401327
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                传真：
                <asp:Label ID="lblFax" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                联系人：
                <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                联系人：
                <asp:Label ID="lblContacts" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
