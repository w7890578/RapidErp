﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountsPayApplication.aspx.cs"
    Inherits="Rapid.PurchaseManager.AccountsPayApplication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购应付申请</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        function Detail(ordersnumber, createtime) {
            window.location.href = "../PurchaseManager/AccountsPayApplicationDetail.aspx?OrdersNumber=" + ordersnumber + "&CreateTime=" + createtime + "&SQ=1";
        }

        $(function () {


            //申请
            $("#btnSQ").click(function () {
                var warehouseNumber = $("#hdnumber").val();
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要申请的行！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定选中数据?")) {
                    $.get("AccountsPayApplication.aspx?time=" + new Date(), { sq: ConvertsContent(checkResult) }, function (result) {
                        if (result == "1") {
                            alert("申请成功");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("申请失败!原因：" + result);
                        }

                    });

                }
            });
            $("#lbQx").click(function () {
                $("input[name='subBox']").each(function () {
                    this.checked = !this.checked; //整个反选
                });
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
        </style>
        <input type="hidden" runat="server" id="hdBackUrl" />
        <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px; margin-bottom: 20px;">
            采购应付申请
        </div>
        <div>
            <input type="hidden" id="hdnumber" runat="server" />
            <input type="hidden" id="hdType" runat="server" />
            <div id="divHeader" style="margin-bottom: 10px;">
                <div style="width: 100%">
                    &nbsp&nbsp;采购订单号：<asp:TextBox ID="txtOdersNumber" runat="server"></asp:TextBox>
                    &nbsp&nbsp; 采购合同号：<asp:TextBox ID="txtHDnumber" runat="server"></asp:TextBox>
                    &nbsp&nbsp; 供应商名称：<asp:TextBox ID="txtSupplierName" runat="server"></asp:TextBox>
                    &nbsp; 付款类型：<asp:DropDownList ID="drpPayType" runat="server">
                        <asp:ListItem Value="" Text="- - 请选择 - -"></asp:ListItem>
                        <asp:ListItem Value="现金" Text="现金"></asp:ListItem>
                        <asp:ListItem Value="转账" Text="转账"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp&nbsp; 到货日期：<asp:TextBox ID="txtDeliveryDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                </div>
                <div style="margin-top: 5px;">
                    &nbsp&nbsp; 款项到期日：<asp:TextBox ID="txtPaymentDueDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                    <asp:Button runat="server" ID="btnSearch" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 10px; margin-left: 10px;" />
                    <input type="button" value="申请" id="btnSQ" style="margin-right: 10px;" />
                    &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
                </div>
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
                        <td>采购订单号
                        </td>
                        <td>采购合同号
                        </td>
                        <td>到货数量
                        </td>
                        <td>订单总价
                        </td>
                        <td>到货总价
                        </td>
                        <td>供应商名称
                        </td>
                        <td>付款类型
                        </td>
                        <td>付款方式
                        </td>
                        <td>款项到期日
                        </td>
                        <td>到货日期
                        </td>
                        <td>实际付款金额
                        </td>
                        <td>实际付款日期
                        </td>
                        <td>发票号码
                        </td>
                        <td>开票日期
                        </td>
                        <td>是否结清
                        </td>
                        <td>创建时间
                        </td>
                        <td>备注
                        </td>
                        <td>操作
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpList">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <span style="display: <%#Eval("采购订单号").ToString().Equals("合计") ? "none" : "inline"%>;">
                                        <input type="checkbox" value="<%#Eval("guid")%>" name='subBox' /></span>
                                </td>
                                <td>
                                    <%#Eval("采购订单号")%>
                                </td>
                                <td>
                                    <%#Eval("采购合同号")%>
                                </td>
                                <td>
                                    <%#Eval("到货数量")%>
                                </td>
                                <td>
                                    <%#Eval("订单总价未税")%>
                                </td>
                                <td>
                                    <%#Eval("到货总价未税")%>
                                </td>
                                <td>
                                    <%#Eval("供应商名称")%>
                                </td>
                                <td>
                                    <%#Eval("付款类型")%>
                                </td>
                                <td>
                                    <%#Eval("付款方式")%>
                                </td>
                                <td>
                                    <%#Eval("款项到期日")%>
                                </td>
                                <td>
                                    <%#Eval("到货日期")%>
                                </td>
                                <td>
                                    <%#Eval("实际付款金额")%>
                                </td>
                                <td>
                                    <%#Eval("实际付款日期")%>
                                </td>
                                <td>
                                    <%#Eval("发票号码")%>
                                </td>
                                <td>
                                    <%#Eval("开票日期")%>
                                </td>
                                <td>
                                    <%#Eval("是否结清")%>
                                </td>
                                <td>
                                    <%#Eval("创建时间")%>
                                </td>
                                <td>
                                    <%#Eval("备注")%>
                                </td>
                                <td>
                                    <span style="display: <%#Eval("采购订单号").ToString().Equals("合计") ? "none" : "inline"%>;">
                                        <a href="###" onclick="Detail('<%#Eval("采购订单号")%>','<%#Eval("创建时间")%>')">详细</a>
                                        

                                    </span>
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

