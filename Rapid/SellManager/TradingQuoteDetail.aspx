<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TradingQuoteDetail.aspx.cs"
    Inherits="Rapid.SellManager.TradingQuoteDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>贸易报价单明细</title>
    <%--
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />--%>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function edit(guid, quotenumber, sn) {
            guid = encodeURIComponent(guid);
            OpenDialog("EditTradingQuote.aspx?Guid=" +guid+"&QuoteNumber="+quotenumber+"&SN="+sn, "btnSearch", "400", "600");
        }
        function Delete(guid, quotenumber, sn) {
            if (confirm("确定删除？")) {
                var quoteNumber = $("#hdnumber").val();
                $.ajax({
                    type: "Get",
                    url: "TradingQuoteDetail.aspx",
                    data: { time: new Date(),Guid:guid,Id:quotenumber,SN:sn },
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
                OpenDialog("../SellManager/AddTradingQuote.aspx?QuoteNumber=" + quoteNumber, "btnSearch", "290", "600");
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
                var className = "";
                var arrChk = $("input[name='columList']:checkbox");
                $(arrChk).each(function() {
                    if ($(this).is(':checked')) {
                        chooseResult += $(this).val() + ",";
                    }
                    else {
                        unChooseResult += $(this).val() + ",";
                    }
                });
                var reg = /,$/gi; //替换最后一个 ','
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
        .border1
        {
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border tr td, .border1 tr td
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
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="float: left;">
                <asp:Button runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="查询" CssClass="button" />
                <span id="spAdd" runat="server">
                    <%--<input type="button" value="增加" id="btnAdd" class="button" />--%></span></div>
            <div style="position: relative; float: left">
                &nbsp;&nbsp; <span id="spPrint" runat="server">
                    <input type="button" value="打印" id="btnPrint" class="button" /></span>
               <span style ="display :none ;"><asp:FileUpload ID="FU_Excel" runat="server" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnUpload"
                    runat="server" Text=" 导 入 " OnClick="btnUpload_Click" /></span> <br />
                <asp:Label ID="lbMsg" runat="server" Text="" Style="color: Red;"></asp:Label>
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
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
                                <input type="checkbox" name="columList" value="tdOperar_MaterialDescription" checked="checked" />
                                物料描述</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_Brand" checked="checked" />
                                品牌</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_CustomerMaterialNumber" checked="checked" />
                                客户物料编号</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_UnitPrice" checked="checked" />
                                单价</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_MinPackage" checked="checked" />
                                最小包装</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_MinMOQ" checked="checked" />
                                最小起订量</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_FixedLeadTime" checked="checked" />
                                固定提前期</label>
                        </li>
                        >
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_DW" checked="checked" />
                                单位</label>
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
        <table border='0' cellpadding='0' cellspacing='0' class="border1">
            <tr>
                <td align="left" colspan="2" style="width: 100%;">
                    <img src="../Img/my/tading.png" />&nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" style="font-size: 24px" colspan="2">
                    <b>北京普迪电子设备有限公司</b>
                </td>
            </tr>
            <tr>
                <td align="center" style="font-size: 20px" colspan="2">
                    <b>RAPID ELECTRONICS CO.,LTD</b>
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 20px">
                    <b>公司地址</b>：北京市顺义区强路1号嘉德工场3号楼3层<br />
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 20px">
                    <b>工厂地址</b>：北京市顺义区强路1号嘉德工场3号楼3层<br />
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 20px">
                    <b>TEL</b>：010-89401326&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>FAX</b>：010-89401327
                </td>
            </tr>
            <tr>
                <td style="font-size: 24px" colspan="2" align="center">
                    <b>QUOTATION</b>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>To:</b><b><asp:Label id="lbCustomerName" runat ="server" ></asp:Label></b>
                </td>
                <td align="left">
                    <b>OUT REF</b>：<b><label id="lbNumber" runat="server"></label></b>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>ATTN</b>：
                </td>
                <td align="left">
                    <b>DATE</b>：<b><asp:Label ID="lbDate" runat="server" Text="Label"></asp:Label></b>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <b>C.C</b>.：
                </td>
                <td align="left">
                    <b>PACE：1</b>
                </td>
            </tr>
        </table>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td class="tdOperar_SN">
                        序号
                    </td>
                    <td class="tdOperar_ProductNumber">
                        原材料编号
                    </td>
                    <td class="tdOperar_CustomerMaterialNumber">
                        客户物料编号
                    </td>
                    <td class="tdOperar_MaterialDescription">
                        物料描述
                    </td>
                    <td class="tdOperar_Brand">
                        品牌
                    </td>
                    <td class="tdOperar_UnitPrice">
                        单价
                    </td>
                    <td class="tdOperar_MinPackage">
                        最小包装
                    </td>
                    <td class="tdOperar_MinMOQ">
                        最小起订量
                    </td>
                    <td class="tdOperar_FixedLeadTime">
                        固定提前期
                    </td>
                    <td class="tdOperar_DW">
                        单位
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
                                <%#Eval("num")%>
                            </td>
                            <td class="tdOperar_ProductNumber">
                                <%#Eval("ProductNumber")%>
                            </td>
                            <td class="tdOperar_CustomerMaterialNumber">
                                <%#Eval("CustomerMaterialNumber")%>
                            </td>
                            <td class="tdOperar_MaterialDescription">
                                <%#Eval("MaterialDescription")%>
                            </td>
                            <td class="tdOperar_Brand">
                                <%#Eval("BrandNew")%>
                            </td>
                            <td class="tdOperar_UnitPrice">
                                <%#Eval("UnitPrice")%>
                            </td>
                            <td class="tdOperar_MinPackage">
                                <%#Eval("MinPackage")%>
                            </td>
                            <td class="tdOperar_MinMOQ">
                                <%#Eval("MinMOQ")%>
                            </td>
                            <td class="tdOperar_FixedLeadTime">
                                <%#Eval("FixedLeadTime")%>
                            </td>
                            <td class="tdOperar_DW">
                                <%#Eval("DW") %>
                            </td>
                            <td class="tdOperar_Remark">
                                <%#Eval("Remark")%>
                            </td>
                            <td class="tdOperar">
                                <span style="display: <%=hasEdit %>;"><a href="###" onclick="edit('<%#Eval("Guid")%>','<%#Eval("QuoteNumber") %>','<%#Eval("SN") %>')">
                                    编辑</a></span> <span style="display: <%=hasDelete %>;"><a href="###" onclick="Delete('<%#Eval("Guid")%>','<%#Eval("QuoteNumber") %>','<%#Eval("SN") %>')">
                                        删除</a></span>
                                      <%--  ("QuoteNumber") %>','<%#Eval("SN")%>--%>
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
