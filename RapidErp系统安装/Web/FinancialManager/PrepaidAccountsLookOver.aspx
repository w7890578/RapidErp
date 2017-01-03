<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrepaidAccountsLookOver.aspx.cs"
    Inherits="Rapid.FinancialManager.PrepaidAccountsLookOver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>预付账款查看</title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        function Detail(ordersnumber, fatherguid) {
            window.location.href = "../PurchaseManager/PrepaidAccountsApplicationDetail.aspx?OrdersNumber=" + ordersnumber + "&ck=3&time=" + new Date() + "&fatherguid=" + fatherguid;
        }
        function Edit(guid) {
            OpenDialog("../PurchaseManager/EditPrepaidAccountsApplication.aspx?Guid=" + guid, "btnSearch", "400", "600");

        }
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
            采购预付查看
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
                    &nbsp;&nbsp;是否结清:<asp:DropDownList runat="server" ID="drpIsJQ">
                        <asp:ListItem Text="---请选择---" Value=""></asp:ListItem>
                        <asp:ListItem Text="是" Value="是"></asp:ListItem>
                        <asp:ListItem Text="否" Value="否"></asp:ListItem>
                    </asp:DropDownList>
                     &nbsp;&nbsp;
                发票号码：<asp:TextBox runat="server" ID="txtfpnumber"></asp:TextBox>
                    <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click"
                        Style="margin-right: 10px; margin-left: 10px;" />
                    <asp:Button runat="server" ID="btnExpExcel" Text="导出Excel"
                        CssClass="button" OnClick="btnExpExcel_Click" />
                    &nbsp;&nbsp;
                    
                &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
                </div>
            </div>
            <table class="border" cellpadding="1" cellspacing="1">
                <thead>
                    <tr>
                        <td>采购订单号
                        </td>
                        <td>采购合同号
                        </td>
                        <td>订单总价
                        </td>
                        <td>到货总价
                        </td>
                        <td>供应商名称
                        </td>
                        <td>付款类型
                        </td>
                        <td>预付一
                        </td>
                        <td>预付二
                        </td>
                        <td>预付一实际付款日期
                        </td>
                        <td>预付二实际付款日期
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
                                    <%#Eval("采购订单号")%>
                                </td>
                                <td>
                                    <%#Eval("采购合同号")%>
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
                                    <%#Eval("预付一")%>
                                </td>
                                <td>
                                    <%#Eval("预付二")%>
                                </td>
                                <td>
                                    <%#Eval("预付一实际付款日期")%>
                                </td>
                                <td>
                                    <%#Eval("预付二实际付款日期")%>
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
                                        <a href="###" onclick="Edit('<%#Eval("Guid")%>')">编辑</a> <a href="###" onclick="Detail('<%#Eval("采购订单号")%>','<%#Eval("guid") %>')">详细</a></span>
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
