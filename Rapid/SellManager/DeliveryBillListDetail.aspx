<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryBillListDetail.aspx.cs"
    Inherits="Rapid.SellManager.DeliveryBillListDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>送货单明细</title>
    <%-- <link href="../Css/Main.css" rel="stylesheet" type="text/css" />--%>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function edit(number, ordersnumber, prodeuctnumber, customerpn, version, rownumber) {
            customerpn = encodeURIComponent(customerpn);
            OpenDialog("AddEditDeliveryBillDetail.aspx?DeliveryNumber=" + number + "&OrdersNumber=" + ordersnumber + "&ProductNumber=" + prodeuctnumber + "&CustomerProductNumber=" + customerpn + "&Version=" + encodeURI(version) + "&RowNumber=" + rownumber, "btnSearch", "600", "630");
        }
        function Delete(number, ordersnumber, prodeuctnumber, customerpn, version, rownumber) {
            if (confirm("确定删除？")) {
                $.ajax({
                    type: "Get",
                    url: "DeliveryBillListDetail.aspx" + "?time=" + new Date() + "&DeliveryNumber=" + number + "&OrdersNumber=" + ordersnumber + "&ProductNumber=" + prodeuctnumber + "&CustomerProductNumber=" + customerpn + "&Version=" + encodeURI(version) + "&RowNumber=" + rownumber,
                    data: { ids: number },
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

        function TC() {
            if (confirm("确定填充数量？")) {
                return true;
            }
            return false;
        }

        $(function() {
            $("#btnAdd").click(function() {
                var deliveryNumber = $("#hdnumber").val();
                OpenDialog("../SellManager/AddDeliveryBillDetail.aspx?DeliveryNumber=" + deliveryNumber, "btnSearch", "200", "500");
            });

            $("#btnPrint").click(function() {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function() {
                $("#choosePrintClounm").hide();
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
                newwin = window.open("", "newwin", "height=900,width=1000,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
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
                window.location.href = "DeliveryBillList.aspx";
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
            font-size: 13px;
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
        .style1
        {
            height: 11px;
        }
    </style>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="float: left;">
                <span style="display: none;">
                    <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="查询" CssClass="button" /></span>
                <input type="button" value="增加" id="btnAdd" class="button" /></div>
            <div style="position: relative; float: left">
                &nbsp;&nbsp; <span id="spPrint" runat="server">
                    <input type="button" value="打印" id="btnPrint" class="button" /></span>
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_num" checked="checked" />
                                序号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_OrdersNumber" checked="checked" />
                                采购单</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_ProductNumber" checked="checked" />
                                产成品编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_CustomerProductNumber" checked="checked" />
                                物料编码</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_Version" checked="checked" />
                                版本</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_MaterialDescription" checked="checked" />
                                物料描述</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_RowNumber" checked="checked" />
                                采购单行</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_DeliveryQty" checked="checked" />
                                发货数量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_ArriveQty" checked="checked" />
                                实到数量
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_ConformenceQty" checked="checked" />
                                实收数量(收货)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_NGReason" checked="checked" />
                                拒收原因(收货)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_PassQty" checked="checked" />
                                合格品数量(质检)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_NgQty" checked="checked" />
                                拒收数量(质检)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_InspectorNGReason" checked="checked" />
                                拒收原因(质检)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_RoughCastingCode" checked="checked" />
                                铸件毛坯编码(仅适用铸件)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_ImportPartsCode" checked="checked" />
                                进口件编码(仅适用进口件)</label>
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
            <asp:Button ID="btnEmp" runat="server" Text="导出Excel" 
                style="margin-right:10px;" onclick="btnEmp_Click" />
            <span style="display: <%=isConfirm%>;">
            <asp:Button ID="btnE" runat="server" Text="填充数量" OnClick="btnE_Click" OnClientClick="return TC()"
                Style="margin-right: 20px;" /></span>
            <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
        </div>
        <table width="100%">
            <tr>
                <td colspan="3">
                    <img src="../Img/GF.png" />
                </td>
                <td colspan="10">
                </td>
                <td colspan="2">
                    QEOHS/BAC-Q-424-D
                </td>
                <td>
                    <%=number %>
                </td>
            </tr>
            <tr>
                <td colspan="17" style="font-size: x-large; font-weight: bold;" align="center">
                    收货明细表<br />
                    Goods Receipt List
                </td>
            </tr>
        </table>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td colspan="2">
                        GRN号<br />
                        GRN Code
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        供方编码<br />
                        VD Code
                    </td>
                    <td>
                        VD00583
                    </td>
                    <td>
                    </td>
                    <td>
                        供方名称<br />
                        Supplier
                    </td>
                    <td colspan="6" align="center">
                        北京瑞普迪电子设备有限公司
                    </td>
                    <td>
                        送货人签字<br />
                        delivery
                    </td>
                    <td>
                        <asp:Label ID="lblDeliveryPerson" runat="server" Text=""></asp:Label><br />
                        <asp:Label ID="lblDeliveryDate" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="tdOperar" rowspan="3">
                        操作
                    </td>
                </tr>
                <tr>
                    <td class="tdOperar_num" rowspan="2">
                        序号 No
                    </td>
                    <td class="tdOperar_CustomerProductNumber" rowspan="2">
                        物料编码
                        <br />
                        Material Code
                    </td>
                    <td rowspan="2">
                        版本号 version number
                    </td>
                    <td class="tdOperar_MaterialDescription" rowspan="2">
                        物料描述<br />
                        Material Description
                    </td>
                    <td class="tdOperar_OrdersNumber" rowspan="2">
                        采购单
                        <br />
                        PO/NCR
                    </td>
                    <td class="tdOperar_RowNumber" rowspan="2">
                        采购单行
                        <br />
                        PO-Line
                    </td>
                    <td rowspan="2">
                        项目名称Project Name
                    </td>
                    <td class="tdOperar_DeliveryQty" rowspan="2">
                        发货数量<br />
                        Delivery QTY
                    </td>
                    <td colspan="3">
                        收货员/Receiver
                    </td>
                    <td colspan="3">
                        质检员/Inspector
                    </td>
                    <td class="tdOperar_RoughCastingCode" rowspan="2">
                        铸件毛坯编码(仅适用铸件)<br />
                        Rough casting Code(Only for casting)
                    </td>
                    <td class="tdOperar_ImportPartsCode" rowspan="2">
                        进口件编码(仅适用进口件)<br />
                        Import parts Code(Only for import parts)
                    </td>
                </tr>
                <tr>
                    <td class="tdOperar_ArriveQty">
                        实到数量
                        <br />
                        Arrive QTY
                    </td>
                    <td class="tdOperar_ConformenceQty">
                        实收数量<br />
                        Conformence<br />
                        QTY
                    </td>
                    <td class="tdOperar_NGReason">
                        拒收原因<br />
                        NG Reason
                    </td>
                    <td class="tdOperar_PassQty">
                        合格品数量<br />
                        PassQty
                    </td>
                    <td class="tdOperar_NgQty">
                        拒收数量<br />
                        NG QTY
                    </td>
                    <td class="tdOperar_InspectorNGReason">
                        拒收原因<br />
                        Inspector<br />
                        NGReason
                    </td>
                </tr>
            </thead>
            <tbody style="width: 1000px">
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td class="tdOperar_num">
                                <%#Eval("num")%>
                            </td>
                            <td class="tdOperar_CustomerProductNumber">
                                <%#Eval("CustomerProductNumber")%>
                            </td>
                            <td>
                                <%#Eval("Version").ToString().Equals("WU") ? "" : Eval("Version")%>
                            </td>
                            <td class="tdOperar_MaterialDescription">
                                <%#Eval("MaterialDescription")%>
                            </td>
                            <td class="tdOperar_OrdersNumber">
                                <%#Eval("CustomerOrderNumber")%>
                            </td>
                            <td class="tdOperar_RowNumber">
                                <%#Eval("RowNumber")%>
                            </td>
                            <td>
                                <%#Eval("projectName_New")%>
                            </td>
                            <td class="tdOperar_DeliveryQty">
                                <%#Eval("DeliveryQty")%>
                            </td>
                            <td class="tdOperar_ArriveQty">
                                <%#Eval("ArriveQty").ToString().Equals("0") ? "" : Eval("ArriveQty").ToString()%>
                            </td>
                            <td class="tdOperar_ConformenceQty">
                                <%#Eval("ConformenceQty").ToString().Equals("0") ? "" : Eval("ConformenceQty").ToString()%>
                            </td>
                            <td class="tdOperar_NGReason">
                                <%#Eval("NGReason")%>
                            </td>
                            <td class="tdOperar_PassQty">
                                <%#Eval("PassQty").ToString().Equals("0") ? "" : Eval("PassQty").ToString()%>
                            </td>
                            <td class="tdOperar_NgQty">
                                <%#Eval("NgQty").ToString().Equals("0") ? "" : Eval("NgQty").ToString()%>
                            </td>
                            <td class="tdOperar_InspectorNGReason">
                                <%#Eval("InspectorNGReason")%>
                            </td>
                            <td class="tdOperar_RoughCastingCode">
                                <%#Eval("RoughCastingCode")%>
                            </td>
                            <td class="tdOperar_ImportPartsCode">
                                <%#Eval("ImportPartsCode")%>
                            </td>
                            <td class="tdOperar">
                                <span style="display: <%=hasEdit%>;"><span style="display: <%=isConfirm%>;"><a href="###"
                                    onclick="edit('<%#Eval("DeliveryNumber") %>','<%#Eval("OrdersNumber")%>',
                                '<%#Eval("ProductNumber")%>','<%#Eval("CustomerProductNumber")%>','<%#Eval("Version")%>','<%#Eval("RowNumber")%>')">
                                    <%#Eval("num").ToString().Equals("合计") ? "" : "编辑"%> </a></span></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin-top: 10px;
            font-size: 12px">
            <tr>
                <td colspan="17" align="left">
                    到岸分配信息如下：
                </td>
            </tr>
            <tr>
                <td colspan="17" align="left">
                    1 Freight Cost:
                </td>
            </tr>
            <tr>
                <td colspan="17" align="left">
                    2 Forwarder Name:
                </td>
            </tr>
            <tr>
                <td colspan="6" align="left">
                    3 收货员/Receiver
                </td>
                <td colspan="6" align="left">
                    质检员/Inspector
                </td>
                <td colspan="5" align="left">
                    库管员/Stockman
                </td>
            </tr>
            <tr>
                <td colspan="6" align="left">
                    4 日期/date：
                </td>
                <td colspan="6" align="left">
                    日期/date：
                </td>
                <td colspan="5" align="left">
                    日期/date：
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
