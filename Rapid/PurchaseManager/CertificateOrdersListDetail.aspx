<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificateOrdersListDetail.aspx.cs"
    Inherits="Rapid.PurchaseManager.CertificateOrdersListDetail" %>

<%--lk--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购订单明细</title>
    <%-- <link href="../Css/Main.css" rel="stylesheet" type="text/css" />--%>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function edit(number, guid) {
            OpenDialog("../PurchaseManager/AddOrEditCertificateOrdersDetail.aspx?OdersNumber=" + number + "&Guid=" + guid + "&date=" + new Date(), "btnSearch", "550", "700");
        }
        function Delete(guid, ordersNumber) {
            if (confirm("确定删除？")) {
                $.ajax({
                    type: "Get",
                    url: "CertificateOrdersListDetail.aspx?time=" + new Date(),
                    data: { guid: guid, OrdersNumber: ordersNumber },
                    success: function(result) {
                        if (result == "1") {
                            alert("删除成功");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("删除失败！原因：" + result);
                            return;
                        }
                    }

                })

            }
        }

        function EditQty(guid) {
            var qty = prompt("填写数量", "");
            if (qty == null || qty == "") {
                return false;
            }
            $.ajax({
                type: "Post",
                url: "CertificateOrdersListDetail.aspx",
                data: { time: new Date(), guid: guid, qty: qty, isEditQty: "true" },
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
                OpenDialog("../PurchaseManager/AddOrEditCertificateOrdersDetail.aspx?OdersNumber=" + odersNumber, "btnSearch", "550", "600");
            });
            $("#btnPrint").click(function() {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function() {
                $("#choosePrintClounm").hide();
            });
            $("#btnBack").click(function() {
                window.location.href = "CertificateOrdersList.aspx";
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
                window.location.href = "CertificateOrdersList.aspx";
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
        采购订单<%=number%>
    </div>
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
                合同号：<asp:Label ID="lblOrdersNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="style1">
                卖方：<asp:Label ID="lblSupplierName" runat="server" Text=""></asp:Label>
            </td>
            <td>
            </td>
            <td>
                买方 ：北京瑞普迪电子设备有限公司
            </td>
        </tr>
    </table>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            &nbsp;&nbsp;
            <div style="float: left;">
                <%--&nbsp;&nbsp; 采购订单编号：<label id="lbNumber" runat="server"></label>--%>
                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="查询" CssClass="button" />
                &nbsp;&nbsp; <span id="spAdd" runat="server">
                    <input type="button" value="增加" id="btnAdd" class="button" style='display: <%=checkStatus%>;' /></span>
            </div>
            <div style="position: relative; float: left">
                &nbsp;&nbsp; <span id="spPrint" runat="server">
                    <input type="button" value="打印" id="btnPrint" class="button" /></span>
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <%-- <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_OrdersNumber" checked="checked" />
                                采购订单编号</label>
                        </li>--%>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_num" checked="checked" />
                                序号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_MaterialNumber" checked="checked" />
                                原材料编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_MaterialName" checked="checked" />
                                型号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_SupplierMaterialNumber" checked="checked" />
                                供应商物料编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_OrderQty" checked="checked" />
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
                                单价（未税）</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_UP" checked="checked" />
                                单价（含税）</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_SumPrice" checked="checked" />
                                总价（未税）</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_SP" checked="checked" />
                                总价（含税）</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_MinOrderQty" checked="checked" />
                                最小起订量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_LeadTime" checked="checked" />
                                交期</label>
                        </li>
                        <li style="display: <%=showPay%>;">
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_PayOne" checked="checked" />
                                付款一</label>
                        </li>
                        <li style="display: <%=showPay%>;">
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_PayTwo" checked="checked" />
                                付款二</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_Status" checked="checked" />
                                状态</label>
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
                    <%--  <td class="tdOperar_OrdersNumber">
                        采购订单编号
                    </td>--%>
                    <td class="tdOperar_num">
                        序号
                    </td>
                    <td class="tdOperar_MaterialNumber">
                        原材料编号
                    </td>
                    <td class="tdOperar_MaterialName">
                        型号
                    </td>
                    <td class="tdOperar_SupplierMaterialNumber">
                        供应商物料编号
                    </td>
                    <td class="tdOperar_OrderQty">
                        数量
                    </td>
                    <td class="tdOperar_NonDeliveryQty">
                        未交货数量
                    </td>
                    <td class="tdOperar_DeliveryQty">
                        已交货数量
                    </td>
                    <td class="tdOperar_UnitPrice">
                        单价（未税）
                    </td>
                    <td class="tdOperar_UP">
                        单价（含税）
                    </td>
                    <td class="tdOperar_SumPrice">
                        总价（未税）
                    </td>
                    <td class="tdOperar_SP">
                        总价（含税）
                    </td>
                    <td class="tdOperar_MinOrderQty">
                        最小起订量
                    </td>
                    <td class="tdOperar_LeadTime">
                        交期
                    </td>
                    <td class="tdOperar_PayOne" style="display: <%=showPay%>;">
                        付款一
                    </td>
                    <td class="tdOperar_PayTwo" style="display: <%=showPay%>;">
                        付款二
                    </td>
                    <td class="tdOperar_Status">
                        状态
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
                            <%--  <td class="tdOperar_OrdersNumber">
                                <%#Eval("OrdersNumber")%>
                            </td>--%>
                            <td class="tdOperar_num">
                                <%#Eval("num")%>
                            </td>
                            <td class="tdOperar_MaterialNumber">
                                <%#Eval("MaterialNumber")%>
                            </td>
                            <td class="tdOperar_MaterialName">
                                <%#Eval("MaterialName")%>
                            </td>
                            <td class="tdOperar_SupplierMaterialNumber">
                                <%#Eval("SupplierMaterialNumber")%>
                            </td>
                            <td class="tdOperar_OrderQty">
                                <%#Eval("OrderQty")%>
                            </td>
                            <td class="tdOperar_NonDeliveryQty">
                                <%#Eval("NonDeliveryQty")%>
                            </td>
                            <td class="tdOperar_DeliveryQty">
                                <%#Eval("DeliveryQty")%>
                            </td>
                            <td class="tdOperar_UnitPrice">
                                <%#Eval("UnitPrice")%>
                            </td>
                            <td class="tdOperar_UP">
                                <%#Eval("UnitPrice_C")%>
                            </td>
                            <td class="tdOperar_SumPrice">
                                <%#Eval("SumPrice")%>
                            </td>
                            <td class="tdOperar_SP">
                                <%#Eval("SumPrice_C")%>
                            </td>
                            <td class="tdOperar_MinOrderQty">
                                <%#Eval("MinOrderQty")%>
                            </td>
                            <td class="tdOperar_LeadTime">
                                <%#Eval("LeadTime")%>
                            </td>
                            <td class="tdOperar_PayOne" style="display: <%=showPay%>;">
                                <%#Eval("PayOne") %>
                            </td>
                            <td class="tdOperar_PayTwo" style="display: <%=showPay%>;">
                                <%#Eval("PayTwo")%>
                            </td>
                            <td class="tdOperar_Status">
                                <%#Eval("Status_New")%>
                            </td>
                            <td class="tdOperar_Remark">
                                <%#Eval("Remark")%>
                            </td>
                            <td class="tdOperar">
                                <span style='display: <%=hasEdit%>;'><span style='display: <%=checkStatus%>;'><span
                                    style='display: <%#Eval("Status_New").ToString()=="未完成"?"block":"none"%>;'><a href="###"
                                        onclick="edit('<%#Eval("OrdersNumber") %>','<%#Eval("Guid")%>')">编辑</a> </span>
                                </span></span><span style='display: <%=hasDelete%>;'><span style='display: <%=checkStatus%>;'>
                                    <span style='display: <%#Eval("Status_New").ToString()=="未完成"?"block":"none"%>;'><a
                                        href="###" onclick="Delete('<%#Eval("Guid") %>','<%#Eval("OrdersNumber") %>')">删除</a>
                                    </span></span></span>&nbsp; &nbsp;<a href="###" style="display: <%=userId=="sysAdmin"?"inline":"none" %>;"
                                        onclick="EditQty('<%#Eval("Guid") %>')">编辑数量</a>
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
                1.此价格为人民币包含增值税价格
            </td>
        </tr>
        <tr>
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
                6.付款方式：<asp:Label ID="lbPayMode" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                7.合同传真件有效
            </td>
        </tr>
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
                单位名称：<asp:Label ID="lblSupplieId" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                公司名称：北京瑞普迪电子设备有限公司
            </td>
        </tr>
        <tr>
            <td>
                单位地址：<asp:Label ID="lblFactoryAddress" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                单位地址：北京市顺义区顺强路1号嘉德工厂3号楼3层
            </td>
        </tr>
        <tr>
            <td>
                联系电话：<asp:Label ID="lblContactTelephone" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                联系电话：010-000010101
            </td>
        </tr>
        <tr>
            <td>
                邮箱：<asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                传真：010-8898888
            </td>
        </tr>
        <tr>
            <td>
                联系人：<asp:Label ID="lblContacts" runat="server" Text=""></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                联系人：<asp:Label ID="lblContactName" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
