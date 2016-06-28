<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductWarehouseLogDetail.aspx.cs"
    Inherits="Rapid.StoreroomManager.ProductWarehouseLogDetail" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <% 
        string isPrint = BLL.ToolManager.GetParamsString("isPrint");
  
    %>
    <title>
        <%=titleName%></title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js?v=2" type="text/javascript"></script>

    <script type="text/javascript">
        //审核
        function ConfirmCheck() {
            if (confirm("确定审核？")) {
                return true;
            }
            return false;
        }
        //确认
        function ConfirmQR() {
            if (confirm("确定确认？")) {
                return true;
            }
            return false;
        }

        function Edit(warehouseNumber, documentNumber, productNumber, version, planNumberOrderNumber, rowNumber) {
            var changeDirection = $("#hdChangeDirection").val();
            changeDirection = encodeURI(decodeURI(changeDirection));
            var height = "420";
            var width = "600";
            var type = "<%=titleName%>";
            var url = "";
            if (type == "盘盈入库" || type == "盘亏出库") {
                // url = "../StoreroomManager/AddOrEditOverageOutOfStorage.aspx";
            }
            else if (type == "报废出库") {
                url = "../StoreroomManager/AddOrEditScrappedLibrary.aspx";
                height = "450";
                width = "600";
            }
            else {
                url = "../StoreroomManager/AddorEditRetutnOfGoodsInStoom.aspx";
            }
            OpenDialog(url + "?WarehouseNumber=" + warehouseNumber + "&DocumentNumber=" + documentNumber + "&ProductNumber=" + productNumber + "&Version=" + version + "&ChangeDirection=" + changeDirection + "&PlanNumberOrderNumber=" + planNumberOrderNumber + "&RowNumber=" + rowNumber, "btnSearch", height, width);
        }
        function Delete(warehouseNumber, guid) {
            if (confirm("确认删除？")) {
                $.get("ProductWarehouseLogDetail.aspx?sq=" + new Date(),
                {
                    WarehouseNumber: warehouseNumber, guid: guid
                },
                function (result) {
                    if (result == "1") {
                        alert("删除成功");
                        $("#btnSearch").click();
                    }
                    else {
                        alert("删除失败！原因：" + result);
                    }
                });
            }
        }

        function printview() {
            // 打印页面预览 
            pagesetup_null();
            wb.execwb(7, 1);
            wb.execwb(45, 1); //关闭窗体无提示
        }

        $(function () {

            var isPrint = "<%=isPrint%>";
            if (isPrint == "true") {
                document.getElementById("divHeader").style.display = 'none';
                document.getElementById("printdiv").style.display = 'block';
            }
            //var printLink = window.location.href;
            //if (printLink.indexOf("?") != -1) {
            //    printLink += "&";
            //}
            //printLink += "isPrint=true";

            //$("#aprint").attr("href", printLink);

            //$(".border tbody tr").click(function () {
            //    $(this).find("input[type='checkbox']").each(function () {
            //        this.checked = !this.checked; //整个反选
            //    });
            //});


            $("#btnAdd").click(function () {
                var warehouseNumber = getQueryString("WarehouseNumber");
                var changeDirection = $("#hdChangeDirection").val();
                changeDirection = encodeURI(decodeURI(changeDirection));
                var height = "550";
                var width = "600";
                var type = "<%=titleName%>";
                var url = "";
                //                if (type == "盘盈入库" || type == "盘亏出库") {
                //                    url = "../StoreroomManager/AddOrEditOverageOutOfStorage.aspx";
                //                }
                if (type == "报废出库") {
                    url = "../StoreroomManager/AddOrEditScrappedLibrary.aspx";
                    height = "450";
                    width = "600";
                }
                else if (type == "退货入库") {
                    url = "../StoreroomManager/AddProductInReturn.aspx";
                    height = "200";
                    width = "600";
                }
                else {
                    url = "../StoreroomManager/ToolAddProductWarehouseLogDetail.aspx";
                }
                //                else {
                //                    url = "../StoreroomManager/AddorEditRetutnOfGoodsInStoom.aspx";
                //                }
                OpenDialog(url + "?WarehouseNumber=" + warehouseNumber + "&Type=" + encodeURI(type), "btnSearch", height, width);

            });

            $("#btnPrint").click(function () {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function () {
                $("#choosePrintClounm").hide();
            });
            $("#lbQx").click(function () {
                $("input[name='subBox']").each(function () {
                    this.checked = !this.checked; //整个反选
                    SetTrColor(jQuery(this));
                });
            });

            $("#btnXuan").click(function () {
                var warehouseNumber = getQueryString("WarehouseNumber");
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
                    $.get("ProductWarehouseLogDetail.aspx?time=" + new Date(), { xuan: ConvertsContent(checkResult), warehouseNumber: warehouseNumber }, function (result) {
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
                    var className = $(this).attr("class");
                    if (className == "tdOperar") {
                        $(this).hide();
                    }
                    for (var j = 0; j < unChoosedArray.length; j++) {
                        if (className == unChoosedArray[j] + "") {
                            $(this).hide();
                        }
                    }
                });
                $("#printdiv").hide();
                //printpreview();
                //newwin = window.open("", "newwin", "height=900,width=800,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
                //newwin.document.body.innerHTML = document.getElementById("form1").innerHTML;
                ////newwin.document.body.innerHTML = document.getElementsByTagName('html')[0].innerHTML;
                //newwin.document.getElementById("divHeader").style.display = 'none';
                //newwin.document.getElementById("choosePrintClounm").style.display = 'none';
                //newwin.window.print();
                //newwin.window.close();
                printview();
                //window.parent.close();
                //window.location.href=window.location.href;
                // $("#printdiv").show();
                //遍历border样式的table下的td 
                //$(".border tr td").each(function () {
                //    var className = $(this).attr("class");
                //    if (className == "tdOperar") {
                //        $(this).show();
                //    }
                //    for (var j = 0; j < unChoosedArray.length; j++) {
                //       // if (className == unChoosedArray[j] + "") {
                //            $(this).show();
                //       // }
                //    }
                //});
            });

            $("#btnBack").click(function () {
                var backUrl = $("#hdBackUrl").val();
                window.location.href = backUrl;

                // window.location.href = "ProductWarehouseLogList.aspx";
            });
        })
    </script>

</head>
<body>

    <object classid="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2" height="0" id="wb" name="wb" width="0"></object>
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
                top: 95px;
                left: 730px;
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

            .printchoosediv {
                display: none;
            }

                .printchoosediv ul li {
                    list-style: none;
                    float: left;
                    cursor: pointer;
                }
        </style>
        <style type="text/css" media="print">
            .noprint {
                display: none;
            }
        </style>

        <input type="hidden" runat="server" id="hdBackUrl" />
        <input type="hidden" id="hdChangeDirection" runat="server" />


        <div class="printchoosediv noprint" id="printdiv">
            <p>
                请选择要打印的列
            </p>
            <ul>
                <li style="display: <%=showDocumentNumber %>;">
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_单据编号" checked="checked" />
                        <%=orderName%></label>
                </li>
                <li style="display: <%=showPlanNumberOrdersNumber %>;">
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_开工单销售订单号" />
                        单据编号</label>
                </li>
                <li style="display: <%=showCustomerOrderNumber %>;">
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_客户采购订单号" />
                        客户采购订单号</label>
                </li>
                <li style="display: <%=showCustomerOrderNumber %>;">
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_项目名称" />
                        项目名称</label>
                </li>
                <li style="display: <%=showPlanNumberRowNumber %>;">
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_行号" />
                        行号</label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_产品编号" checked="checked" />
                        产成品编号</label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_版本" checked="checked" />
                        版本</label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_型号" checked="checked" />
                        型号</label>
                </li>
                <li style="display: <%=showCustomerProductNumber %>;">
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_客户产成品编号" checked="checked" />
                        客户产品编号</label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_产品描述" checked="checked" />
                        产品描述</label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_数量" checked="checked" />
                        数量
                    </label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_货位" checked="checked" />
                        货位
                    </label>
                </li>
                <li style="display: <%=showLeadTime %>;">
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_交期" checked="checked" />
                        交期
                    </label>
                </li>
                <li style="display: <%=showCustomerName%>;">
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_客户名称" checked="checked" />
                        客户名称
                    </label>
                </li>
                <li style="display: <%=showReason%>;">
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_报废退货原因" checked="checked" />
                        退货原因
                    </label>
                </li>
                <li>
                    <label>
                        <input type="checkbox" name="columList" value="tdOperar_备注" checked="checked" />
                        备注
                    </label>
                </li>
                <li>&nbsp; 
                <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                    type="button" value=" 取 消 " id="btnExit" />

                </li>
            </ul>
            <br />
        </div>

        <div style="width: 100%; text-align: center; font: 96px; font-size: x-large; font-weight: bold; margin-top: 5px; margin-bottom: 10px;">
            <%=titleName%>单<%=number%>
        </div>
        <div>
            <input type="hidden" id="hdnumber" runat="server" />
            <div id="divHeader">
                <div style="margin-bottom: 10px;">
                    产成品编号：<asp:TextBox ID="txtProductNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                    客户产成品编号：<asp:TextBox ID="txtCustomerProductNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                    型号：<asp:TextBox ID="txtProductName" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                    客户名称：<asp:TextBox ID="txtCustomerName" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                </div>
                <div style="margin-bottom: 10px;" align="center">
                    <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click"
                        Style="margin-right: 10px;" />
                    <input type="button" id="btnAdd" value="增加" style="display: <%=show %>;" />
                    &nbsp;&nbsp;<asp:Button runat="server" Text="审核" ID="btnCheck" OnClick="btnCheck_Click"
                        OnClientClick="return ConfirmCheck()" />
                    &nbsp;&nbsp;
                <input type="button" value="选择" id="btnXuan" style="display: <%=showOperar %>; margin-right: 10px;" />
                    &nbsp;&nbsp;
                      <asp:Button ID="btnEmp" runat="server" Text="导出Excel" Style="margin-right: 10px;"
                          OnClick="btnEmp_Click" />
                    &nbsp;&nbsp;
                    <input type="button" value="返回" id="btnBack" class="button" />
                    &nbsp;&nbsp;  <span id="spPrint" runat="server">
                        <%
                            string printLink = Request.Url.AbsoluteUri;
                            if (printLink.IndexOf('?') != -1)
                            {
                                printLink += "&isPrint=true";
                            }
                            else
                            {
                                printLink += "?isPrint=true";
                            }
                        %>
                        <a href="<%=printLink %>" target="_blank" id="aprint">打印单据</a>
                        &nbsp;&nbsp; 
                    </span>



                </div>
                &nbsp;
            </div>
            <asp:Label runat="server" ID="lbSubmit" ForeColor="Red"></asp:Label>
            <table class="border" cellpadding="1" cellspacing="1">
                <thead>
                    <tr>
                        <td class="tdOperar">
                            <label id="lbQx">
                                <input type="checkbox" /></label>全选/反选
                        </td>
                        <td class="tdOperar_单据编号" style="display: <%=showDocumentNumber %>;">
                            <%=orderName%>
                        </td>
                        <td class="tdOperar_开工单销售订单号" style="display: <%=showPlanNumberOrdersNumber %>;">销售订单号
                        </td>
                        <td class="tdOperar_客户采购订单号" style="display: <%=showCustomerOrderNumber %>;">客户采购订单号
                        </td>
                        <td class="tdOperar_项目名称" style="display: <%=showCustomerOrderNumber %>;">项目名称
                        </td>
                        <td class="tdOperar_行号" style="display: <%=showPlanNumberRowNumber %>;">行号
                        </td>
                        <td class="tdOperar_产品编号">产成品编号
                        </td>
                        <td class="tdOperar_版本">版本
                        </td>
                        <td class="tdOperar_型号">型号
                        </td>
                      
                        <td class="tdOperar_客户产成品编号">客户产品编号
                        </td>
                        
                        <td class="tdOperar_产品描述">产品描述
                        </td>
                        <td class="tdOperar_数量">数量
                        </td>
                        <td class="tdOperar_货位">货位
                        </td>
                        <td class="tdOperar_交期" style="display: <%=showLeadTime %>;">交期
                        </td>
                        <td class="tdOperar_客户名称" style="display: <%=showCustomerName %>;">客户名称
                        </td>
                        <td class="tdOperar_报废退货原因" style="display: <%=showReason%>;">退货原因
                        </td>
                        <td>库存数量
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
                                <td class="tdOperar">
                                    <input type="checkbox" value="<%#Eval("Guid")%>" name='subBox' />
                                </td>
                                <td class="tdOperar_单据编号" style="display: <%=showDocumentNumber %>;">
                                    <%#Eval("单据编号")%>
                                </td>
                                <td class="tdOperar_开工单销售订单号" style="display: <%=showPlanNumberOrdersNumber %>;">
                                    <%#Eval("开工单销售订单号")%>
                                </td>
                                <td class="tdOperar_客户采购订单号" style="display: <%=showCustomerOrderNumber %>;">
                                    <%#Eval("客户采购订单号")%>
                                </td>
                                <td class="tdOperar_项目名称" style="display: <%=showCustomerOrderNumber %>;">
                                    <%#Eval("项目名称")%>
                                </td>
                                <td class="tdOperar_行号" style="display: <%=showPlanNumberRowNumber %>;">
                                    <%#Eval("行号")%>
                                </td>
                                <td class="tdOperar_产品编号">
                                    <%#Eval("产品编号")%>
                                </td>
                                <td class="tdOperar_版本">
                                    <%#Eval("版本")%>
                                </td>
                                <td class="tdOperar_型号">
                                    <%#Eval("产品名称")%>
                                </td>
                                
                                <td class="tdOperar_客户产成品编号">
                                    <%#Eval("客户产成品编号")%>
                                </td>
                               
                                <td class="tdOperar_产品描述">
                                    <%#Eval("产品描述")%>
                                </td>
                                <td class="tdOperar_数量">
                                    <%#Eval("数量")%>
                                </td>
                                <td class="tdOperar_货位">
                                    <%#Eval("货位")%>
                                </td>
                                <td class="tdOperar_交期" style="display: <%=showLeadTime %>;">
                                    <%#Eval("交期")%>
                                </td>
                                <td class="tdOperar_客户名称" style="display: <%=showCustomerName %>;">
                                    <%#Eval("客户名称")%>
                                </td>
                                <td class="tdOperar_报废退货原因" style="display: <%=showReason%>;">
                                    <%#Eval("报废退货原因")%>
                                </td>
                                <td>
                                    <%#Eval("库存数量")%>
                                </td>
                                <td class="tdOperar_备注">
                                    <%#Eval("备注")%>
                                </td>
                                <td class="tdOperar">
                                    <span style="display: <%=showOperar %>;"><a href="###" onclick="Edit('<%#Eval("出入库编号")%>','<%#Eval("单据编号")%>','<%#Eval("产品编号")%>','<%#Eval("版本")%>','<%#Eval("开工单销售订单号") %>','<%#Eval("行号") %>')">编辑</a> <a href="###" onclick="Delete('<%#Eval("出入库编号")%>','<%#Eval("Guid") %>')">删除</a></span>
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
<script type="text/javascript">
    jQuery(function () {
        jQuery("input[name='subBox']").change(function () {
            SetTrColor(jQuery(this));
        });
    })
</script>
