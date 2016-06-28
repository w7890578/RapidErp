<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SampleCRKDetail.aspx.cs"
    Inherits="Rapid.StoreroomManager.SampleCRKDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        </title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        function IsConfimQR() {
            if (confirm("确定确认？")) {
                return true;
            }
            return false;
        }

        function Edit(warehouseNumber, documentNumber, materialNumber) {
            var type = encodeURI($("#hdType").val());
            OpenDialog("EditPurchaseStorageOrdersDetail.aspx?WarehouseNumber=" + warehouseNumber + "&DocumentNumber=" + encodeURI(documentNumber) + "&MaterialNumber=" + materialNumber + "&Type=" + type, "btnSearch", "300", "550");
        }
        function Delete(guid) {
            if (confirm("确定删除？")) {
                $.ajax({
                    type: "Get",
                    url: "SampleCRKDetail.aspx",
                    data: { time: new Date(), key: guid },
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
        $(function() {
            $("#btnAdd").click(function() {
                var type = $.trim("<%=type %>");
                var warehouseNumber = "<%=warehouseNumber%>";

                if (type == "样品入库") {
                    OpenDialog("AddYPRK.aspx?WarehouseNumber=" + warehouseNumber, "btnSearch", "300", "600");
                }
                else {
                    OpenDialog("AddYPCK.aspx?WarehouseNumber=" + warehouseNumber + "&Type=" + encodeURI(type), "btnSearch", "250", "400");
                }
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
                window.location.href = "SampleCRK.aspx";
            });

            //审核
            $("#btnAutior").click(function() {
                if (confirm("确定审核？")) {
                    var warehouseNumber = $("#hdnumber").val();

                    $.get("ToolMaterialWarehouseLogDetail.aspx?sq="+new Date (), { IsAutior: "true", WarehouseNumber: warehouseNumber, time: new Date() }, function(result) {
                        if (result == "1") {
                            window.location.reload(); //页面重新加载
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
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        <%=type %>明细<%=warehouseNumber%></div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <input type="hidden" id="hdType" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="float: left;">
                <span>&nbsp&nbsp; &nbsp&nbsp; 原材料编号：<asp:TextBox ID="txtMaterialNumber" runat="server"></asp:TextBox>
                    <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click"
                        Style="margin-right: 10px;" />
                    <span style="display: <%=checkStatus %>;">
                        <asp:Button ID="btnCheck" runat="server" Text="审核" OnClick="btnCheck_Click" />
                    </span></span>
                <input type="button" value="增加" id="btnAdd" style="display: <%=checkStatus %>;" />
            </div>
            <div style="position: relative; float: left">
                &nbsp;&nbsp; <span id="spPrint" runat="server">
                    <input type="button" value="打印" id="btnPrint" class="button" /></span>
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <li style="display: <%=showDocumentNumber%>;">
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_单据编号" checked="checked" />
                                单据编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_原材料编号" />
                                原材料编号</label>
                        </li>
                        <li style="display: <%=showRowNumber%>;">
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_行号" />
                                行号</label>
                        </li>
                        <li style="display: <%=showLeadTime%>;">
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_交期" />
                                交期</label>
                        </li>
                        <li style="display: <%=showSupplNumber%>;">
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_供应商物料编号" />
                                供应商物料编号</label>
                        </li>
                        <li style="display: <%=showCustomerNumber%>;">
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_客户物料编号" />
                                客户物料编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_数量" checked="checked" />
                                数量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_库存数量" />
                                库存数量</label>
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
                            type="button" value=" 取 消 " id="btnExit" /></div>
                </div>
            </div>
            &nbsp;
            <input type="button" value="返回" id="btnBack" class="button" />
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbMsg" runat="server"></asp:Label>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td class="tdOperar_单据编号" style="display: <%=showDocumentNumber%>;">
                        单据编号
                    </td>
                    <td class="tdOperar_原材料编号">
                        原材料编号
                    </td>
                    <td class="tdOperar_行号" style="display: <%=showRowNumber%>;">
                        行号
                    </td>
                    <td class="tdOperar_交期" style="display: <%=showLeadTime%>;">
                        交期
                    </td>
                    <td class="tdOperar_供应商物料编号" style="display: <%=showSupplNumber%>;">
                        供应商物料编号
                    </td>
                    <td class="tdOperar_客户物料编号" style="display: <%=showCustomerNumber%>;">
                        客户物料编号
                    </td>
                    <td class="tdOperar_数量">
                        数量
                    </td>
                    <td class="tdOperar_库存数量">
                        库存数量
                    </td>
                    <td class="tdOperar_备注">
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
                            <td class="tdOperar_单据编号" style="display: <%=showDocumentNumber%>;">
                                <%#Eval("单据编号")%>
                            </td>
                            <td class="tdOperar_原材料编号">
                                <%#Eval("原材料编号")%>
                            </td>
                            <td class="tdOperar_行号" style="display: <%=showRowNumber%>;">
                                <%#Eval("行号")%>
                            </td>
                            <td class="tdOperar_交期" style="display: <%=showLeadTime%>;">
                                <%#Eval("交期")%>
                            </td>
                            <td class="tdOperar_供应商物料编号" style="display: <%=showSupplNumber%>;">
                                <%#Eval("供应商物料编号")%>
                            </td>
                            <td class="tdOperar_客户物料编号" style="display: <%=showCustomerNumber%>;">
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td class="tdOperar_数量">
                                <%#Eval("数量")%>
                            </td>
                            <td>
                                <%#Eval("库存数量").ToString () %>
                            </td>
                            <td class="tdOperar_备注">
                                <%#Eval("备注")%>
                            </td>
                            <td class="tdOperar">
                                <span style="display: <%=checkStatus %>;"><a href="###" style="display: <%#Eval("Guid").ToString ().Equals ("")?"none":"inline"%>;"
                                    onclick="Delete('<%#Eval("Guid")%>')">删除</a></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <tr>
                    <td class="tdOperar_单据编号" style="display: <%=showDocumentNumber%>;">
                    </td>
                    <td class="tdOperar_原材料编号">
                        合计
                    </td>
                    <td class="tdOperar_行号" style="display: <%=showRowNumber%>;">
                    </td>
                    <td class="tdOperar_交期" style="display: <%=showLeadTime%>;">
                    </td>
                    <td class="tdOperar_供应商物料编号" style="display: <%=showSupplNumber%>;">
                    </td>
                    <td class="tdOperar_客户物料编号" style="display: <%=showCustomerNumber%>;">
                    </td>
                    <td class="tdOperar_数量">
                        <%=sumQty%>
                    </td>
                    <td>
                    </td>
                    <td class="tdOperar_备注">
                    </td>
                    <td class="tdOperar">
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
    </form>
</body>
</html>
