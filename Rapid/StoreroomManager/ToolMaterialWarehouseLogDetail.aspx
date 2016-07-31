<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ToolMaterialWarehouseLogDetail.aspx.cs"
    Inherits="Rapid.StoreroomManager.ToolMaterialWarehouseLogDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=type %>明细</title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js?v=2" type="text/javascript"></script>

    <script type="text/javascript">

        function IsConfimQR() {
            if (confirm("确定确认？")) {
                return true;
            }
            return false;
        }

        function Edit(guid) {
            var type = encodeURI($("#hdType").val());
            OpenDialog("EditPurchaseStorageOrdersDetail.aspx?Guid=" + guid + "&Type=" + type + "&time=" + new Date(), "btnSearch", "300", "550");
        }
        function Delete(guid) {
            if (confirm("确定删除？")) {
                $.ajax({
                    type: "Get",
                    url: "ToolMaterialWarehouseLogDetail.aspx?time=" + new Date(),
                    data: { guid: guid },
                    success: function (result) {
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

        var hkey_key;
        var hkey_root = "HKEY_CURRENT_USER";
        var hkey_path = "\\Software\\Microsoft\\Internet Explorer\\PageSetup\\";
        //设置网页打印的页眉页脚为空
        function pagesetup_null() {
            var RegWsh = new ActiveXObject("WScript.Shell");
            hkey_key = "header";
            RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "");
            hkey_key = "footer";
            RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "");
        }
        //设置网页打印的页眉页脚为默认值
        function pagesetup_default() {
            try {
                var RegWsh = new ActiveXObject("WScript.Shell");
                hkey_key = "header";
                RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "&w&b页码，&p/&P");
                hkey_key = "footer";
                RegWsh.RegWrite(hkey_root + hkey_path + hkey_key, "&u&b&d");
            } catch (e) { }
        }

        function ImpPurchasingStorage() {
            $("#impPurchasingStorage").show();
        }

        function HideImpPurchasingStorage() {
            $("#impPurchasingStorage").hide();

        }
        function ImpWite() {
            $("#ImpMsg").html("正在导入！请稍等......");
            return true;
        }

        $(function () {
            $("#btnAdd").click(function () {
                var warehouseNumber = $("#hdnumber").val();
                var type = $("#hdType").val();

                if (type == "采购退料出库") {
                    OpenDialog("AddCCTLCKDetail.aspx?WarehouseNumber=" + warehouseNumber, "btnSearch", "500", "600");
                }
                else if (type == "辅料出库") {
                    OpenDialog("AddFLCKDetail.aspx?WarehouseNumber=" + warehouseNumber, "btnSearch", "500", "600");
                }
                else if (type == "生产出库") {
                    OpenDialog("AddSCCK.aspx?WarehouseNumber=" + warehouseNumber, "btnSearch", "500", "600");
                }
                else if (type == "生产退料入库") {
                    OpenDialog("AddBackFeeding.aspx?WarehouseNumber=" + warehouseNumber, "btnSearch", "200", "600");
                }
                else {
                    OpenDialog("AddPurchaseStorageOrdersDetail.aspx?WarehouseNumber=" + warehouseNumber + "&Type=" + encodeURI(type), "btnSearch", "600", "600");
                }
            });

            $("#btnPrint").click(function () {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function () {
                $("#choosePrintClounm").hide();
            });
            $("#btnChoosePrintColum").click(function () {
                var chooseResult = "";
                var unChooseResult = "";
                var arrChk = $("input[name='columList']:checkbox");
                $(arrChk).each(function () {
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
                $(".border tr td").each(function () {
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
                $("#divprintmsg").hide();
                pagesetup_null();
                newwin = window.open("", "newwin", "height=900,width=750,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
                newwin.document.body.innerHTML = document.getElementById("form1").innerHTML;
                newwin.document.getElementById("divHeader").style.display = 'none';
                newwin.document.getElementById("choosePrintClounm").style.display = 'none';
                newwin.window.print();
                newwin.window.close();
                pagesetup_default();
                $("#choosePrintClounm").hide();
                $(".border tr td").each(function () {
                    $(this).show();
                })
                $("#divprintmsg").show();
                $("#btnSearch").click();
            });

            $("#btnBack").click(function () {
                var backUrl = $("#hdBackUrl").val();
                window.location.href = backUrl;
                //var isxs = getQueryString("IsXS");
                //if (isxs) {
                //    window.location.href = " ../SellManager/MaterialSaleOderWarehouseOut.aspx";
                //}
                //else {

                //}
            });

            $("#btnXuan").click(function () {
                var warehouseNumber = $("#hdnumber").val();
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择行！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定选中数据?")) {
                    $.get("ToolMaterialWarehouseLogDetail.aspx?time=" + new Date(), { xuan: ConvertsContent(checkResult), warehouseNumber: warehouseNumber }, function (result) {
                        if (result == "1") {
                            alert("选择成功");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("选择失败!原因：" + result);
                        }

                    });

                }
            });
            $("#lbQx").click(function () {
                $("input[name='subBox']").each(function () {
                    this.checked = !this.checked; //整个反选
                    SetTrColor(jQuery(this));
                });
            });
            //审核
            $("#btnAutior").click(function () {
                if (confirm("确定审核？")) {
                    var warehouseNumber = $("#hdnumber").val();

                    $.get("ToolMaterialWarehouseLogDetail.aspx?sq=" + new Date(), { IsAutior: "true", WarehouseNumber: warehouseNumber, time: new Date() }, function (result) {
                        if (result == "1") {
                            //window.location.reload(); //页面重新加载
                            window.location.href = "MarerialWarehouseLogList.aspx";
                        }
                        else {
                            $("#lbMsg").html(result);
                        }
                    });
                }
            });

        })
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <style type="text/css">
            .border {
                background-color: Black;
                width: 100%;
                font-size: 14px;
                text-align: center;
            }

                .border tr td {
                    padding: 4px;
                    background-color: White;
                }

            a {
                color: Blue;
            }

                a:hover {
                    color: Red;
                }

            #choosePrintClounm {
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

                #choosePrintClounm ul {
                    margin-bottom: 10px;
                }

                #choosePrintClounm div {
                    text-align: center;
                    color: Green;
                }

                #choosePrintClounm ul li {
                    list-style: none;
                    float: left;
                    width: 100%;
                    cursor: pointer;
                }
        </style>

        <div id="impPurchasingStorage" style="position: absolute; top: 40%; left: 30%; display: none; padding: 30px; border: 2px solid gray; background-color: white;">
            Excel导入模板：<a href="../Text/采购入库明细导入模板.xls">采购入库明细导入模板.xls</a><br />
            <br />
            Excel文件：
            <asp:FileUpload ID="FU_Excel" runat="server" /><br />
            <br />
            &nbsp;&nbsp;&nbsp; &nbsp;
            <asp:Button ID="btnUpload"
                runat="server" Text=" 导 入 " OnClick="btnUpload_Click" OnClientClick="return ImpWite();" />&nbsp;&nbsp;<input type="button" value="取消" onclick="HideImpPurchasingStorage();" /><br />
            <br />
            <label id="ImpMsg" style="color: red;"></label>
        </div>

        <input type="hidden" runat="server" id="hdBackUrl" />
        <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px">
            <%=type %>明细<%=number%>
        </div>
        <div>
            <input type="hidden" id="hdnumber" runat="server" />
            <input type="hidden" id="hdType" runat="server" />
            <div id="divHeader" style="padding: 10px;">
                <div style="float: left; width: 100%;">
                    &nbsp&nbsp; &nbsp&nbsp; 原材料编号：<asp:TextBox ID="txtMaterialNumber" runat="server"></asp:TextBox>
                    &nbsp&nbsp; 客户物料编号：<asp:TextBox ID="txtCustomerMaterialNumber" runat="server"></asp:TextBox>
                    &nbsp&nbsp; 供应商物料编号：<asp:TextBox ID="txtSupplierMaterialNumber" runat="server"></asp:TextBox>
                </div>
                <div style="margin-top: 3px;">
                    &nbsp&nbsp;&nbsp&nbsp;
                <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 运输号：<asp:TextBox ID="txtYSnumber" runat="server"></asp:TextBox>
                    &nbsp&nbsp; 原材料描述：<asp:TextBox ID="txtMaterialDescription" runat="server"></asp:TextBox>
                    &nbsp&nbsp; 货物类型：<asp:TextBox ID="txtCargoType" runat="server"></asp:TextBox>
                    <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click"
                        Style="margin-right: 10px;" /><span style="display: <%=showAdd %>;">
                            <input type="button" value="增加" id="btnAdd" class="button" style="margin-right: 10px;" /></span>
                    <input type="button" value="选择" id="btnXuan" style="display: <%=showOperar %>;" style="margin-right: 10px;" />
                    <span style="display: <%=showCheck %>;">
                        <input type="button" value="审核所有" id="btnAutior" class="button" style="margin-right: 10px;" /></span>

                    &nbsp;&nbsp;<input type="button" value="导入采购入库明细" onclick="ImpPurchasingStorage();" />
                    &nbsp;&nbsp; <span id="spPrint" runat="server">
                        <input type="button" value="打印" id="btnPrint" class="button" style="margin-right: 10px;" /></span>
                    <input type="button" value="返回" id="btnBack" class="button" />
                    &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
                    <asp:Label runat="server" ID="lbSubmit" ForeColor="Red"></asp:Label>
                </div>
                <div style="position: relative; float: left">
                    <div id="choosePrintClounm">
                        <div>
                            请选择要打印的列
                        </div>
                        <ul>
                            <li>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_出入库编号" checked="checked" />
                                    出入库编号</label>
                            </li>
                            <li style='display: <%=showDocumentNumber%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_单据编号" checked="checked" />
                                    <%=documentName%></label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_原材料编号" />
                                    原材料编号</label>
                            </li>
                            <li style='display: <%=showProductNumber%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_产成品编号" />
                                    产成品编号</label>
                            </li>
                            <li style='display: <%=showProductNumber%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_客户产成品编号" />
                                    客户产成品编号</label>
                            </li>
                            <li style='display: <%=showProductNumber%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_版本" />
                                    版本</label>
                            </li>
                            <li style='display: <%=showSupplierMaterialNumber%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_供应商物料编号" />
                                    供应商物料编号</label>
                            </li>
                            <li style='display: <%=showCustomerMaterialNumber%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_客户物料编号" />
                                    客户物料编号</label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_货位" checked="checked" />
                                    货位</label>
                            </li>
                            <li style='display: <%=showSupplierName%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_供应商名称" />
                                    供应商名称</label>
                            </li>
                            <li style='display: <%=showCustomerName%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_客户名称" />
                                    客户名称</label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_原材料名称" checked="checked" />
                                    原材料名称</label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_原材料描述" checked="checked" />
                                    原材料描述</label>
                            </li>
                            <li style='display: <%=showSingleDose%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_单机用量" />
                                    单机用量</label>
                            </li>
                            <li style='display: <%=showSingleDose%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_单位" />
                                    单位</label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_数量" checked="checked" />
                                    数量</label>
                            </li>
                            <li style="display: <%=showRoadTransport%>;">
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_运输号" checked="checked" />
                                    运输号</label>
                            </li>
                            <li style='display: <%=showRowNumber%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_行号" />
                                    行号
                                </label>
                            </li>
                            <li style='display: <%=showLeadTime%>;'>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_交期" />
                                    交期</label>
                            </li>
                            <li style="display: <%=showCompleteQty  %>;">
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_已完成数量" />
                                    已完成数量</label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_货物类型" />
                                    货物类型</label>
                            </li>
                            <li>
                                <label>
                                    <input type="checkbox" name="columList" value="tdOperar_备注" checked="checked" />
                                    备注
                                </label>
                            </li>
                        </ul>
                        <div>
                            &nbsp;<br />
                            <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                                type="button" value=" 取 消 " id="btnExit" />
                        </div>
                    </div>
                </div>
            </div>
            <div style="margin: 10px; text-align: left; color: red;" id="divprintmsg">
                PS:使用打印功能前请确保您的IE浏览器进行了如下操作,否则将打印失败：打开你的ie浏览器internet选项—— 安全—— 自定义级别—— 把对没有标记为安全的activex控件进行初始化和脚本运行 设置为启用。 打印功能仅支持IE浏览器。
            </div>
            <table class="border" cellpadding="1" cellspacing="1">
                <thead>
                    <tr>
                        <%--<td class="tdOperar_出入库编号">
                        出入库编号
                    </td>--%>
                        <td>
                            <label id="lbQx">
                                <input type="checkbox" /></label>全选/反选
                        </td>
                        <%if (ShowCustomerOrderNumber)
                            {%>
                        <td class="tdOperar_客户采购订单号 ">客户采购订单号
                        </td>
                        <%} %>
                        <%if (showDocumentNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_单据编号"><%=documentName%>
                        </td>
                        <%} %>

                        <%-- <td class="tdOperar_单据编号" style='display: <%=showDocumentNumber%>;'>
                            <%=documentName%>
                        </td>--%>
                        <%--  <td class="tdOperar_产成品编号" style='display: <%=showProductNumber%>;'>产成品编号
                        </td>--%>
                        <%if (showProductNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_产成品编号">产成品编号
                        </td>
                        <%} %>

                        <%if (showProductNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_客户产成品编号">客户产成品编号
                        </td>
                        <%} %>
                        <%--                        <td class="tdOperar_客户产成品编号" style='display: <%=showProductNumber%>;'>客户产成品编号
                        </td>--%>
                        <%if (showProductNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_版本">版本
                        </td>
                        <%} %>
                        <%--  <td class="tdOperar_版本" style='display: <%=showProductNumber%>;'>版本
                        </td>--%>
                        <td class="tdOperar_原材料编号">原材料编号
                        </td>

                        <%if (showSupplierMaterialNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_供应商物料编号">供应商物料编号
                        </td>
                        <%} %>

                        <%if (showCustomerMaterialNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_客户物料编号">客户物料编号
                        </td>
                        <%} %>

                        <td class="tdOperar_货位">货位
                        </td>
                        <td class="tdOperar_供应商名称" style='display: <%=showSupplierName%>;'>供应商名称
                        </td>
                        <td class="tdOperar_客户名称" style='display: <%=showCustomerName%>;'>客户名称
                        </td>
                        <%--  <td class="tdOperar_原材料名称">
                        原材料名称
                    </td>--%>
                        <td class="tdOperar_原材料描述">原材料描述
                        </td>
                        <td class="tdOperar_单机用量" style='display: <%=showSingleDose%>;'>单机用量
                        </td>
                        <td class="tdOperar_单位" style='display: <%=showSingleDose%>;'>单位
                        </td>

                        <td class="tdOperar_运输号" style="display: <%=showRoadTransport%>;">运输号
                        </td>
                        <td class="tdOperar_行号" style="display: <%=showRowNumber%>;">行号
                        </td>
                        <td class="tdOperar_交期" style="display: <%=showLeadTime%>;">交期
                        </td>
                        <td class="tdOperar_已完成数量" style="display: <%=showCompleteQty  %>;">已完成数量
                        </td>
                        <td>实时库存数量
                        </td>
                        <td class="tdOperar_货物类型">货物类型
                        </td>
                        <td class="tdOperar_数量">数量
                        </td>
                        <td class="tdOperar_备注">备注
                        </td>
                        <td class="tdOperar">操作
                        </td>
                    </tr>
                </thead>
                <tbody>

                    <asp:Repeater runat="server" ID="rpList">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <input type="checkbox" value="<%#Eval("Guid")%>" name='subBox' />
                                </td>
                                <%--  <td class="tdOperar_出入库编号">
                                <%#Eval("出入库编号")%>
                            </td>--%>
                                <%if (ShowCustomerOrderNumber)
                                    {%>
                                <td class="tdOperar_客户采购订单号 "><%#Eval("客户采购订单号") %>
                                </td>
                                <%} %>
                                <%if (showDocumentNumber.Equals("inline"))
                                    {
                                %>
                                <td class="tdOperar_单据编号"><%#Eval("单据编号") %>
                                </td>
                                <%} %>

                                <%-- <td class="tdOperar_单据编号" style='display: <%=showDocumentNumber%>;'>
                                    <%#Eval("单据编号")%>
                                </td>--%>
                                <%--<td class="tdOperar_产成品编号" style='display: <%=showProductNumber%>;'>
                                    <%#Eval("产成品编号")%>
                                </td>--%>

                                <%if (showProductNumber.Equals("inline"))
                                    {
                                %>
                                <td class="tdOperar_产成品编号"><%#Eval("产成品编号") %>
                                </td>
                                <%} %>

                                <%if (showProductNumber.Equals("inline"))
                                    {
                                %>
                                <td class="tdOperar_客户产成品编号"><%#Eval("客户产成品编号") %>
                                </td>
                                <%} %>
                                <%-- <td class="tdOperar_客户产成品编号" style='display: <%=showProductNumber%>;'>
                                    <%#Eval("客户产成品编号")%>
                                </td>--%>
                                <%--<td class="tdOperar_版本" style='display: <%=showProductNumber%>;'>
                                    <%#Eval("版本")%>
                                </td>--%>
                                <%if (showProductNumber.Equals("inline"))
                                    {
                                %>
                                <td class="tdOperar_版本"><%#Eval("版本") %>
                                </td>
                                <%} %>

                                <td class="tdOperar_原材料编号">
                                    <%#Eval("原材料编号")%>
                                </td>
                                <%if (showSupplierMaterialNumber.Equals("inline"))
                                    {
                                %>
                                <td class="tdOperar_供应商物料编号"><%#Eval("供应商物料编号") %>
                                </td>
                                <%} %>
                                <%if (showCustomerMaterialNumber.Equals("inline"))
                                    { %>
                                <td class="tdOperar_客户物料编号"><%#Eval("客户物料编号") %> </td>
                                <%} %>
                                <td class="tdOperar_货位">
                                    <%#Eval("货位")%>
                                </td>
                                <%if (showSupplierName.Equals("inline"))
                                    { %>
                                <td class="tdOperar_供应商名称" style='display: <%=showSupplierName%>;'>
                                    <%#Eval("供应商名称")%>
                                </td>
                                <%} %>
                                <%if (showCustomerName.Equals("inline"))
                                    { %>
                                <td class="tdOperar_客户名称" style='display: <%=showCustomerName%>;'>
                                    <%#Eval("客户名称")%>
                                </td>
                                <%} %>

                                <td class="tdOperar_原材料名称" style="display: none;">
                                    <%#Eval("原材料名称")%>
                                </td>
                                <td class="tdOperar_原材料描述">
                                    <%#Eval("原材料描述")%>
                                </td>
                                <td class="tdOperar_单机用量" style='display: <%=showSingleDose%>;'>
                                    <%#Eval("单机用量")%>
                                </td>
                                <td class="tdOperar_单位" style='display: <%=showSingleDose%>;'>
                                    <%#Eval("单位")%>
                                </td>

                                <td class="tdOperar_运输号" style="display: <%=showRoadTransport%>;">
                                    <%#Eval("运输号")%>
                                </td>
                                <td class="tdOperar_行号" style="display: <%=showRowNumber%>;">
                                    <%#Eval("行号")%>
                                </td>
                                <td class="tdOperar_交期" style="display: <%=showLeadTime%>;">
                                    <%#Eval("交期")%>
                                </td>
                                <td class="tdOperar_已完成数量" style="display: <%=showCompleteQty  %>;">
                                    <%#Eval ("已完成数量") %>
                                </td>
                                <td>
                                    <%#Eval("库存数量") %>
                                </td>
                                <td class="tdOperar_货物类型">
                                    <%#Eval("货物类型") %>
                                </td>
                                <td class="tdOperar_数量">
                                    <%-- <%if (type.Equals("生产出库"))
                                      {%>
                                    <a href="/PurchaseManager/DDDetail.aspx?MateriNumber=<%#Eval("原材料编号")%>" target="_blank"><%# Eval("数量")%></a>
                                    <%}
                                      else
                                      { %>
                                    <%# Eval("数量")%>
                                    <%} %>--%> <%# Eval("数量")%></td>
                                <td class="tdOperar_备注">
                                    <%#Eval("备注")%>
                                </td>
                                <td class="tdOperar">
                                    <span style="display: <%=showOperar%>;"><a href="###" onclick="Edit('<%#Eval("Guid") %>')">编辑</a> <a href="###" onclick="Delete('<%#Eval("Guid") %>')">删除</a> </span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
                <tfoot>
                    <tr>

                        <td>合计
                        </td>
                        <%if (ShowCustomerOrderNumber)
                            {%>
                        <td></td>
                        <%} %>
                        <%if (showDocumentNumber.Equals("inline"))
                            {
                        %>
                        <td></td>
                        <%} %>

                        <%-- <td class="tdOperar_单据编号" style='display: <%=showDocumentNumber%>;'>
                            <%=documentName%>
                        </td>--%>
                        <%--  <td class="tdOperar_产成品编号" style='display: <%=showProductNumber%>;'>产成品编号
                        </td>--%>
                        <%if (showProductNumber.Equals("inline"))
                            {
                        %>
                        <td></td>
                        <%} %>

                        <%if (showProductNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_客户产成品编号"></td>
                        <%} %>
                        <%--                        <td class="tdOperar_客户产成品编号" style='display: <%=showProductNumber%>;'>客户产成品编号
                        </td>--%>
                        <%if (showProductNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_版本"></td>
                        <%} %>
                        <%--  <td class="tdOperar_版本" style='display: <%=showProductNumber%>;'>版本
                        </td>--%>
                        <td class="tdOperar_原材料编号"></td>

                        <%if (showSupplierMaterialNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_供应商物料编号"></td>
                        <%} %>

                        <%if (showCustomerMaterialNumber.Equals("inline"))
                            {
                        %>
                        <td class="tdOperar_客户物料编号"></td>
                        <%} %>

                        <td class="tdOperar_货位"></td>
                        <td class="tdOperar_供应商名称" style='display: <%=showSupplierName%>;'></td>
                        <td class="tdOperar_客户名称" style='display: <%=showCustomerName%>;'></td>
                        <%--  <td class="tdOperar_原材料名称">
                        原材料名称
                    </td>--%>
                        <td class="tdOperar_原材料描述"></td>
                        <td class="tdOperar_单机用量" style='display: <%=showSingleDose%>;'></td>
                        <td class="tdOperar_单位" style='display: <%=showSingleDose%>;'></td>

                        <td class="tdOperar_运输号" style="display: <%=showRoadTransport%>;"></td>
                        <td class="tdOperar_行号" style="display: <%=showRowNumber%>;"></td>
                        <td class="tdOperar_交期" style="display: <%=showLeadTime%>;"></td>
                        <td class="tdOperar_已完成数量" style="display: <%=showCompleteQty  %>;"></td>
                        <td></td>
                        <td class="tdOperar_货物类型"></td>
                        <td class="tdOperar_数量">
                            <%=CountQty %>
                        </td>
                        <td class="tdOperar_备注"></td>
                        <td class="tdOperar"></td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <%if (type == "采购入库")
            { %>
        <div>
            <iframe frameborder="0" style="min-width: 200px; min-height: 450px; width: 100%;" src="OrderCheckPage.aspx?WarehouseNumber=<%=Request["WarehouseNumber"] %>"></iframe>
        </div>
        <%} %>
    </form>
</body>
</html>
<script type="text/javascript">

    jQuery(function () {
        jQuery("input[name='subBox']").change(function () {
            SetTrColor(jQuery(this));
        });
    })
</script>