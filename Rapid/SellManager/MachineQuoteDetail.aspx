<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MachineQuoteDetail.aspx.cs"
    Inherits="Rapid.SellManager.MachineQuoteDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>加工报价单明细</title>
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
        function Save() {
            if (confirm("确认保存？")) {
                return true;
            }
            return false;
        }
        function edit(guid) {
            OpenDialog("EditMachineQuoteDetailQty.aspx?Guid=" + guid, "btnSearch", "500", "650");
        }
        function Delete(guid) {
            var quotenumber = getQueryString("id");
            if (confirm("确认删除？")) {
                $.ajax({
                    type: "Get",
                    url: "MachineQuoteDetail.aspx",
                    data: { time: new Date(), id: quotenumber, guid: guid },
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
                var quoteNumber = $("#hdnumber").val();
                OpenDialog("../SellManager/AddMachineQuoteDetail.aspx?QuoteNumber=" + quoteNumber, "btnSearch", "330", "650");
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
                newwin = window.open("", "newwin", "height=900,width=1200,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
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
                window.location.href = "QuoteInfoList.aspx";
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
                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="查询" CssClass="button" />
                <span id="spAdd" runat="server">
                    <%--  <input type="button" value="增加" id="btnAdd" class="button" /><--%>
                </span>
            </div>
            <div style="position: relative; float: left">
                &nbsp;&nbsp; <span id="spPrint" runat="server">
                    <input type="button" value="打印" id="btnPrint" class="button" /></span>
                <asp:Button runat="server" ID="btnSave" Text="另存为新的报价单" OnClick="btnSave_Click" OnClientClick="return Save()"
                    Visible="false" />
                <span style="display: none">
                    <asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpload"
                        runat="server" Text=" 导 入 " OnClick="btnUpload_Click" /></span>
                <%-- <asp:Button id="btnDC" runat ="server" Text ="导出" onclick="btnDC_Click"/>--%>
                <br />
                <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_S#" checked="checked" />
                                S#</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_阶层" checked="checked" />
                                阶层</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_产品编号" checked="checked" />
                                产品编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_图纸号" checked="checked" />
                                图纸号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_BAC物料号" checked="checked" />
                                BAC物料号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_客户物料编号" checked="checked" />
                                客户物料编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_版本" checked="checked" />
                                版本</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_物料描述" checked="checked" />
                                物料描述</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_BOM用量" checked="checked" />
                                BOM用量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_库存数量（理论值）" checked="checked" />
                                库存数量（理论值）</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_原材料单价（未税）" checked="checked" />
                                原材料单价(未税)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_原材料采购单价" checked="checked" />
                                原材料采购单价</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_工时费" checked="checked" />
                                工时费</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_利润（未税）" checked="checked" />
                                利润(未税)
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_管销研费用（未税）" checked="checked" />
                                管销研费用(未税)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_损耗（未税）" checked="checked" />
                                损耗(未税)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_单价（未税）" checked="checked" />
                                单价(未税)</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_固定提前期" checked="checked" />
                                固定提前期</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_备注" checked="checked" />
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
        <html>
        <head>
        </head>
        <body>
            <table class="border_Header">
                <tr>
                    <td colspan="2" style="text-align: center; height: 20px">
                        <h1>
                            报 价 单</h1>
                        <h2>
                            <label id="lbQuoteNumber" runat="server">
                            </label>
                        </h2>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        公司名称：
                        <asp:Label ID="lblPlanName" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        报价公司名称: 北京瑞普迪电子设备有限公司
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        联系人：
                        <asp:Label ID="lblContacts" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        报 价 人 :
                        <asp:Label ID="lblUserName" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        联系电话：<asp:Label ID="lblContactTelephone" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        联 系 电 话 : 010-89401325
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        传 真：<asp:Label ID="lblFax" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        传 真 : 010-89401327
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        邮 件：<asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        日 期 :<asp:Label ID="lblDatetime" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </body>
        </html>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td class="tdOperar_S#">
                        S#
                    </td>
                    <td class="tdOperar_阶层">
                        阶层
                    </td>
                    <td class="tdOperar_产品编号">
                        产品编号
                    </td>
                    <td class="tdOperar_图纸号">
                        图纸号
                    </td>
                    <td class="tdOperar_BAC物料号">
                        BAC物料号
                    </td>
                    <td class="tdOperar_客户物料号">
                        客户物料号
                    </td>
                    <td class="tdOperar_版本">
                        版本
                    </td>
                    <td class="tdOperar_物料描述">
                        物料描述
                    </td>
                    <td class="tdOperar_BOM用量">
                        BOM用量
                    </td>
                    <td class="tdOperar_库存数量（理论值）">
                        库存数量（理论值）
                    </td>
                    <td class="tdOperar_原材料单价（未税）">
                        原材料单价（未税）
                    </td>
                    <td class="tdOperar_原材料采购单价">
                        原材料采购单价
                    </td>
                    <td class="tdOperar_工时费">
                        工时费
                    </td>
                    <td class="tdOperar_利润（未税）">
                        利润（未税）
                    </td>
                    <td class="tdOperar_管销研费用（未税）">
                        管销研费用（未税）
                    </td>
                    <td class="tdOperar_损耗（未税）">
                        损耗（未税）
                    </td>
                    <td class="tdOperar_单价（未税）">
                        单价（未税）
                    </td>
                    <td class="tdOperar_固定提前期">
                        固定提前期
                    </td>
                    <td class="tdOperar_备注">
                        备注
                    </td>
                    <%--  <td>
                        产品类型
                    </td>--%><%--
                    <td class="tdOperar"  style ="display :none ;>
                        操作
                    </td>--%>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td class="tdOperar_S#">
                                <%#Eval("SS")%>
                            </td>
                            <td class="tdOperar_阶层">
                                <%#Eval("Hierarchy")%>
                            </td>
                            <td class="tdOperar_产品编号">
                                <%#Eval("ProductNumber") %>
                            </td>
                            <td class="tdOperar_图纸号">
                                <%#Eval("CustomerProductNumber")%>
                            </td>
                            <td class="tdOperar_BAC物料号">
                                <%#Eval("BACNumber")%>
                            </td>
                            <td class="tdOperar_客户物料号">
                                <%#Eval("CustomerMaterialNumber")%>
                            </td>
                            <td class="tdOperar_版本">
                                <%#Eval("IsMaril").ToString ().Equals ("是")?"":Eval("Version")%>
                            </td>
                            <td class="tdOperar_物料描述">
                                <%#Eval("Description")%>
                            </td>
                            <td class="tdOperar_BOM用量">
                                <%#Eval("BOMAmount").ToString ().Replace (".00","")%>
                            </td>
                            <td class="tdOperar_库存数量（理论值）">
                                <%#Eval("库存数量").ToString ().Replace (".00","")%>
                            </td>
                            <td class="tdOperar_原材料单价（未税）">
                                <%#Eval("MaterialPrcie").ToString().Equals("0.00") ? "" : Eval("MaterialPrcie")%>
                            </td>
                            <td class="tdOperar_原材料采购单价">
                                <%#Eval("采购价格").ToString().Equals("0.00") ? "" : Eval("采购价格")%>
                            </td>
                            <td class="tdOperar_工时费">
                                <%#Eval("TimeCharge").ToString().Equals("0.00") ? "" : Eval("TimeCharge")%>
                            </td>
                            <td class="tdOperar_利润（未税）">
                                <%#Eval("Profit").ToString().Equals("0.00") ? "" : Eval("Profit")%>
                            </td>
                            <td class="tdOperar_管销研费用（未税）">
                                <span style="display: <%#Eval("Isone").ToString ().Equals ("是")?"inline":"none" %>;">
                                    <%#Eval("ManagementPrcie").ToString().Equals("0.00") ? "" : Eval("ManagementPrcie")%></span>
                            </td>
                            <td class="tdOperar_损耗（未税）">
                                <span style="display: <%#Eval("Isone").ToString ().Equals ("是")?"inline":"none" %>;">
                                    <%#Eval("LossPrcie").ToString().Equals("0.00") ? "" : Eval("LossPrcie")%></span>
                            </td>
                            <td class="tdOperar_单价（未税）">
                                <%#Eval("UnitPrice").ToString().Equals("0.00") ? "" : Eval("UnitPrice")%>
                            </td>
                            <td class="tdOperar_固定提前期">
                                <%#Eval("FixedLeadTime")%>
                            </td>
                            <td class="tdOperar_备注">
                                <%#Eval("Remark")%>
                            </td>
                            <%--
                            <td class="tdOperar" style ="display :none ;">
                                <span style="display: <%=hasEdit %>;"><a href="###" onclick="edit('<%#Eval("Guid")%>')">
                                    编辑</a> </span><span style="display: <%=hasDelete %>;"><a href="###" onclick="Delete('<%#Eval("Guid")%>')">
                                        删除</a></span>
                            </td>--%>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
