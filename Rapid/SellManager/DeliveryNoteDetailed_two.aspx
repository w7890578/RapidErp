<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryNoteDetailed_two.aspx.cs"
    Inherits="Rapid.SellManager.DeliveryNoteDetailed_two" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>送货单明细</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        function edit(deliverynumber, ordersnumber, productnumber, version, rownumber, customerproductnumber) {
            customerproductnumber = encodeURIComponent(customerproductnumber)

            OpenDialog("../SellManager/EditDeliveryNoteDetailed_two.aspx?DeliveryNumber=" + deliverynumber + "&OrdersNumber=" + ordersnumber + "&ProductNumber=" + productnumber + "&Version=" + version + "&RowNumber=" + rownumber + "&CustomerProductNumber=" + customerproductnumber + "&date=" + new Date(), "btnSearch", "550", "600");
        }

        function Delete(plannumber, team, ordersnumber, productnumber, version, rownumber) {
            if (confirm("确认删除？")) {
                $.ajax({
                    type: "Get",
                    url: "ProductPlanSubDetailList.aspx",
                    data: { time: new Date, PlanNumber: plannumber, Team: team, OrdersNumber: ordersnumber, ProductNumber: productnumber, Version: version, RowNumber: rownumber },
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
        $("#btnSearch").click();
        $(function() {
            //查询sql语句

            $("#btnBack").click(function() {
                window.location.href = "DeliveryBillList.aspx";

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
                $("#divHiddeMGX").hide();
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
                $("#divHiddeMGX").show();

                //                $("#printTalbe").removeClass("border").addClass("tablesorter");
            });
        });
        var querySql = "";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            return condition;
        }
    </script>

    <style type="text/css">
        .printDiv
        {
            border-radius: 5px;
            border: 1px solid #B3D08F;
            margin-top: 5px;
            margin-right: 10px;
            background-color: #F3FFE3;
            width: 100%;
        }
    </style>
</head>
<body style="padding: 5px 10px 0px 0px;">
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
        .et4
        {
            color: #000000;
            font-size: 11.0pt;
            font-family: 宋体;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            text-align: general;
            vertical-align: middle;
        }
        p.p0
        {
            margin: 0pt;
            margin-bottom: 0.0001pt;
            margin-bottom: 0pt;
            margin-top: 0pt;
            text-align: justify;
            font-size: 10.5000pt;
            font-family: 'Calibri';
        }
        p.p15
        {
            margin-bottom: 0pt;
            margin-top: 0pt;
            border-bottom: 0.7500pt solid rgb(0,0,0);
            padding: 0pt 0pt 1pt 0pt;
            text-align: center;
            font-size: 9.0000pt;
            font-family: 'Calibri';
        }
    </style>
    <div id="divHiddeMGX">
        首页 > 销售管理 ><a href="DeliveryBillList.aspx">送货单列表</a>> 送货单明细</div>
    <input type="hidden" id="hdnumber" runat="server" />
    <div id="divHeader">
        <div style="position: relative; float: left; margin-bottom: 10px; top: 10px; left: 0px;">
            &nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="返回" id="btnBack" />
            <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="display: none;" />
            <input type="button" value="打印" id="btnPrint" style="margin-left: 10px; margin-right: 10px;" />
            <asp:Button ID="btnEmp" runat="server" Text="导出Excel" 
                style="margin-right: 10px;" onclick="btnEmp_Click"/>
            <span style="display: <%=show%>;">
                <asp:Button ID="btnE" runat="server" Text="填充数量" OnClick="btnE_Click" OnClientClick="return TC()"
                    Style="margin-right: 20px;" />
            </span>
            <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
            <div id="choosePrintClounm">
                <div>
                    请选择要打印的列</div>
                <ul>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_序号" checked="checked" />
                            序号
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_产成品编号" checked="checked" />
                            产成品编号
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_客户产成品编号" checked="checked" />
                            客户产成品编号
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_版本" checked="checked" />
                            版本
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_描述" checked="checked" />
                            描述
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_行号" checked="checked" />
                            行号
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_发货数量" checked="checked" />
                            发货数量
                        </label>
                    </li>
                     <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_单位" checked="checked" />
                            单位
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_实收数量" checked="checked" />
                            实收数量
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_销售订单号" checked="checked" />
                            销售订单号
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_客户订单号" checked="checked" />
                            客户订单号
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_订单总量" checked="checked" />
                            订单总量
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_客户号" checked="checked" />
                            客户号
                        </label>
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
    </div>
    &nbsp;<br />
    <br />
    <table width="100%">
        <tr>
            <td>
                <table style="font-size: 14px; line-height: 26px; height: 200px; width: 100%">
                    <tr>
                        <td colspan="4">
                            <img src="../Img/my/tading.png" />
                        </td>
                        <td colspan="4" style="font-size: 28px; font-weight: bold;" align="left">
                            <span style="mso-spacerun: 'yes'; font-weight: bold; font-size: 18.0000pt; font-family: '宋体';">
                                北京瑞普迪电子设备有限公司</span> <span style="mso-spacerun: 'yes'; font-weight: bold; font-size: 14.0000pt;
                                    font-family: '宋体';">
                                    <br />
                                    &nbsp;&nbsp;&nbsp;RAPID&nbsp;ELECTRONICS&nbsp;CO.,&nbsp;LTD</span><span style="mso-spacerun: 'yes';
                                        font-weight: bold; font-size: 14.0000pt; font-family: 'Calibri';"></span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            公司名称：北京瑞普迪电子设备有限公司
                        </td>
                        <td colspan="4">
                            公司名称：<asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                           联系人：<asp:Label ID="lblContactsName" runat="server" Text=""></asp:Label>
                            
                        </td>
                        <td colspan="4">
                            联系人：<asp:Label ID="lblContacts" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            电话：<asp:Label ID="lblTel" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="4">
                            电话：<asp:Label ID="lblContactTelephone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            传真：010-89401327
                        </td>
                        <td colspan="4">
                            传真：<asp:Label ID="lblFax" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            邮箱：snb1119@126.com
                        </td>
                        <td colspan="4">
                            邮箱：<asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            地址：北京市顺义区顺强路1号嘉德工场3号楼3层
                        </td>
                        <td colspan="4">
                            地址：<asp:Label ID="lblFactoryAddress" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <div style="font-size: 28px; font-weight: bold; text-align: center; margin-top: 30px;
                    margin-bottom: 30px;">
                    送货单</div>
                <table class="border" cellpadding="1" cellspacing="1">
                    <thead>
                        <tr>
                            <td class="tdOperar_序号">
                                序号
                            </td>
                            <td class="tdOperar_产成品编号">
                                产成品编号
                            </td>
                            <td class="tdOperar_客户产成品编号">
                                客户产成品编号
                            </td>
                            <td class="tdOperar_版本">
                                版本
                            </td>
                            <td class="tdOperar_描述">
                                描述
                            </td>
                            <td class="tdOperar_行号">
                                行号
                            </td>
                            <td class="tdOperar_发货数量">
                                发货数量
                            </td>
                             <td class="tdOperar_单位">
                                单位
                            </td>
                            <td class="tdOperar_实收数量">
                                实收数量
                            </td>
                            <td class="tdOperar_销售订单号">
                                销售订单号
                            </td>
                            <td class="tdOperar_客户订单号">
                                客户订单号
                            </td>
                            <td class="tdOperar_订单总量">
                                订单总量
                            </td>
                            <td class="tdOperar_客户号">
                                客户号
                            </td>
                           
                            <td class="tdOperar_备注">
                                备注
                            </td>
                            <td class="tdOperar" style="width: 100px;">
                                操作
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpList">
                            <ItemTemplate>
                                <tr>
                                    <td class="tdOperar_序号">
                                        <%#Eval("序号")%>
                                    </td>
                                    <td class="tdOperar_产成品编号">
                                        <%#Eval("产成品编号")%>
                                    </td>
                                    <td class="tdOperar_客户产成品编号">
                                        <%#Eval("客户产成品编号")%>
                                    </td>
                                    <td class="tdOperar_版本">
                                        <%#Eval("版本")%>
                                    </td>
                                    <td class="tdOperar_描述">
                                        <%#Eval("描述")%>
                                    </td>
                                    <td class="tdOperar_行号">
                                        <%#Eval("行号")%>
                                    </td>
                                    <td class="tdOperar_发货数量">
                                        <%#Eval("发货数量")%>
                                    </td>
                                     <td class="tdOperar_单位">
                                        <%#Eval("单位")%>
                                    </td>
                                    <td class="tdOperar_实收数量">
                                        <%#Eval("实收数量")%>
                                    </td>
                                    <td class="tdOperar_销售订单号">
                                        <%#Eval("销售订单号")%>
                                    </td>
                                    <td class="tdOperar_客户订单号">
                                        <%#Eval("客户订单号")%>
                                    </td>
                                    <td class="tdOperar_订单总量">
                                        <%#Eval("订单总量")%>
                                    </td>
                                    <td class="tdOperar_客户号">
                                        <%#Eval("客户号")%>
                                    </td>
                                   
                                    <td class="tdOperar_备注">
                                        <%#Eval("备注")%>
                                    </td>
                                    <td class="tdOperar">
                                        <span style="display: <%=show%>;"><a href="###" onclick="edit('<%#Eval("送货单号") %>','<%#Eval("销售订单号")%>','<%#Eval("产成品编号")%>','<%#Eval("版本")%>','<%#Eval("行号")%>','<%#Eval("物料编号")%>')">
                                           <%#Eval("序号").ToString().Equals("合计") ? "" : "编辑"%>  </a></span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
    <div style="font-size: 16px; text-align: left; margin-left: 50px; margin-top: 10px;
        width: 100%">
        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="line-height: 30px;">
            <tr>
                <td>
                    送货人（签字）:<asp:Label ID="lblDeliveryPerson" runat="server" Text=""></asp:Label>
                </td>
                <td align="right" >
                    收货人（签字）：<asp:Label ID="Label2" runat="server" Text="________________"></asp:Label>
                </td>
                <td style="width:100px;"></td>
            </tr>
            <tr>
                <td>
                    送货日期:<asp:Label ID="lblDeliveryDate" runat="server" Text="" ></asp:Label>
                </td>
                <td align="right">
                    收货单位（章）：<asp:Label ID="Label4" runat="server" Text="________________"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right">
                    收货日期：<asp:Label ID="Label5" runat="server" Text="________________"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
    </div>
    </form>
</body>
</html>
