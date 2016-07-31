<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrepaidAdvanceApplication.aspx.cs"
    Inherits="Rapid.SellManager.PrepaidAdvanceApplication" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售收款申请</title>

    <script src="../Js/jquery-easyui-1.4/jquery.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        function Detail(ordersnumber, deliveryNumber, createtime, isYS, guid) {
            window.location.href = "../SellManager/PrepaidAdvanceApplicationDetail.aspx?OrdersNumber=" + ordersnumber + "&SQ=1&isYS=" + isYS + "&CreateTime=" + createtime + "&deliveryNumber=" + deliveryNumber + "&fatherGuid=" + guid;
        }

        function Edit(guid) {
            OpenDialog("../SellManager/EditPrepaidAdvanceApplication.aspx?Guid=" + guid, "btnSearch", "250", "600");
        }

        function search() {
            $("#progressBar").show();
            return true;
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
                    $.get("PrepaidAdvanceApplication.aspx?time=" + new Date(), { sq: ConvertsContent(checkResult) }, function (result) {
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
        <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            <img src="../Img/loading.gif" alt="loading" />
        </div>
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
            销售<%=isYs.Equals ("1")?"预收":"应收" %>账款
        </div>
        <div>
            <input type="hidden" id="hdnumber" runat="server" />
            <input type="hidden" id="hdType" runat="server" />
            <div id="divHeader" style="margin-bottom: 10px;">
                <div style="width: 100%">
                    &nbsp&nbsp; 销售订单号：<asp:TextBox ID="txtOdersNumber" runat="server"></asp:TextBox>
                    &nbsp&nbsp; 客户采购合同号：<asp:TextBox ID="txtCustomerOdersNumber" runat="server"></asp:TextBox>
                    &nbsp&nbsp; 客户名称：<asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                    &nbsp&nbsp; 收款类型：<asp:DropDownList ID="drpType" runat="server">
                        <asp:ListItem Value="" Text="- - 请选择 - -"></asp:ListItem>
                        <asp:ListItem Value="现金" Text="现金"></asp:ListItem>
                        <asp:ListItem Value="转账" Text="转账"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="margin-top: 5px;">
                    &nbsp&nbsp; 收款方式：<asp:TextBox ID="txtMathod" runat="server"></asp:TextBox>&nbsp&nbsp;
                是否结清：<asp:DropDownList ID="drpisSettle" runat="server">
                    <asp:ListItem Value="" Text="- - 请选择 - -"></asp:ListItem>
                    <asp:ListItem Value="是" Text="是"></asp:ListItem>
                    <asp:ListItem Value="否" Text="否" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                    &nbsp&nbsp; 发票号码：
                    <asp:TextBox runat="server" ID="txtInvoiceNumber"></asp:TextBox>
                </div>
                <div>
                    &nbsp;&nbsp;  送货单号：<asp:TextBox runat="server" ID="txtSHorderNumber"> </asp:TextBox><label style="color: red;">(*按送货单号查询请输全,不支持模糊查询)</label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click" OnClientClick="return search();"
                        Style="margin-right: 10px; margin-left: 10px;" />
                    <input type="button" value="申请" id="btnSQ" style="margin-right: 10px; display: none;" />
                    &nbsp;&nbsp;<asp:Button runat="server" ID="btnExpExcel" Text="导出Excel" CssClass="button"
                        OnClick="btnExpExcel_Click" />

                    &nbsp;&nbsp;<asp:Button runat="server" ID="Button1" Text="导出未开票详细" CssClass="button" OnClick="Button1_Click" />

                    &nbsp;&nbsp;<asp:Button runat="server" ID="Button2" Text="导出所有明细" CssClass="button" OnClick="Button2_Click" />
                    &nbsp;&nbsp;<label style="color: Red;" id="lbMsg" runat="server"></label>
                </div>
            </div>
            <table class="border" cellpadding="1" cellspacing="1" style="table-layout: fixed; word-break: break-all">
                <thead>
                    <tr>
                        <%-- <td>
                        <label id="lbQx">
                            <input type="checkbox" /></label>全选/反选
                    </td>--%>
                        <td nowrap>销售订单号
                        </td>
                        <td>客户采购订单号
                        </td>
                        <td nowrap>订单总价
                        </td>
                        <td nowrap>交货总价
                        </td>
                        <td nowrap>客户名称
                        </td>
                        <td nowrap>收款类型
                        </td>
                        <td nowrap>收款方式
                        </td>
                        <td nowrap style="display: <%=isYs.Equals ("1")?"inline":"none"%>;">预收一
                        </td>
                        <td nowrap style="display: <%=isYs.Equals ("1")?"inline":"none"%>;">预收二
                        </td>
                        <td style="width: 100px; white-space: nowrap">发票号码
                        </td>
                        <td nowrap>开票日期
                        </td>
                        <td nowrap>是否结清
                        </td>
                        <td nowrap>创建时间
                        </td>
                        <td nowrap>备注
                        </td>
                        <td>操作
                        </td>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rpList">
                        <ItemTemplate>
                            <tr>
                                <%--  <td>
                                <span style="display: <%#Eval("销售订单号").ToString().Equals("合计") ? "none" : "inline"%>;">
                                    <input type="checkbox" value="<%#Eval("Guid")%>" name='subBox' /></span>
                            </td>--%>
                                <td>
                                    <%#Eval("销售订单号")%>
                                </td>
                                <td>
                                    <%#Eval("客户采购订单号")%>
                                </td>
                                <td>
                                    <%#Eval("订单总价")%>
                                </td>
                                <td>
                                    <%#Eval("交货总价")%>
                                </td>
                                <td>
                                    <%#Eval("客户名称")%>
                                </td>
                                <td>
                                    <%#Eval("收款类型")%>
                                </td>
                                <td>
                                    <%#Eval("收款方式")%>
                                </td>
                                <td style="display: <%=isYs.Equals ("1")?"inline":"none"%>;">
                                    <%#Eval("预收一")%>
                                </td>
                                <td style="display: <%=isYs.Equals ("1")?"inline":"none"%>;">
                                    <%#Eval("预收二")%>
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
                                    <span style="display: <%#Eval("销售订单号").ToString().Equals("合计") ? "none" : "inline"%>;">
                                        <a href="###" onclick="Detail('<%#Eval("销售订单号")%>','<%#Eval("送货单号") %>',' <%#Eval("创建时间")%>','<%=isYs%>','<%#Eval("guid") %>')">详细</a></span> &nbsp;&nbsp; <span style="display: <%#Eval("销售订单号").ToString().Equals("合计") ? "none" : "inline"%>;">
                                            <a href="###" onclick="Edit('<%#Eval("guid") %>')">编辑</a></span>
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