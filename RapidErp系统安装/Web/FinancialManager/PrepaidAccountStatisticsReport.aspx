<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrepaidAccountStatisticsReport.aspx.cs"
    Inherits="Rapid.FinancialManager.PrepaidAccountStatisticsReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>预付账款统计报表</title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

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
    </style>
    <input type="hidden" runat="server" id="hdBackUrl" />
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px;margin-bottom:20px;">
        预付账款统计报表
    </div>
    <div style="margin-bottom: 10px;">
        <input type="hidden" id="hdnumber" runat="server" />
        <input type="hidden" id="hdType" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="margin-bottom: 10px">
                &nbsp&nbsp; &nbsp&nbsp; 采购订单号：<asp:TextBox ID="txtOdersNumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 采购合同号：<asp:TextBox ID="txtHDnumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 供应商名称：<asp:TextBox ID="txtSupplierName" runat="server"></asp:TextBox>
                &nbsp&nbsp; 原材料编号：<asp:TextBox ID="txtMaterialNumber" runat="server"></asp:TextBox>
            </div>
            <div>
                &nbsp&nbsp; &nbsp&nbsp; 是否结清：<asp:DropDownList ID="drpJQ" runat="server">
                    <asp:ListItem Value="" Text=" - - 请选择 - - "></asp:ListItem>
                    <asp:ListItem Value="是" Text="是"></asp:ListItem>
                    <asp:ListItem Value="否" Text="否"></asp:ListItem>
                </asp:DropDownList>
                &nbsp&nbsp; 供应商物料编号：<asp:TextBox ID="txtSupplierMaterialNumber" runat="server"></asp:TextBox>
                <asp:Button runat="server" ID="btnSearch" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 10px;
                    margin-left: 10px;" />
                <asp:Button ID="btnEmp" runat="server" Text="导出Excel" OnClick="btnEmp_Click" />
                &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td>
                        序号
                    </td>
                    <td>
                        采购订单号
                    </td>
                    <td>
                        采购合同号
                    </td>
                    <td>
                        采购订单日期
                    </td>
                    <td>
                        预付一
                    </td>
                    <td>
                        预付二
                    </td>
                    <td>
                        预付一实际付款日期
                    </td>
                    <td>
                        预付二实际付款日期
                    </td>
                    <td>
                        是否结清
                    </td>
                    <td>
                        原材料编号
                    </td>
                    <td>
                        供应商物料编号
                    </td>
                    <td>
                        描述
                    </td>
                    <td>
                        供应商名称
                    </td>
                    <td>
                        采购数量
                    </td>
                    <td>
                        到货数量
                    </td>
                    <td>
                        单价
                    </td>
                    <td>
                        总价
                    </td>
                    <td>
                        发票号码
                    </td>
                    <td>
                        开票日期
                    </td>
                    <td>
                        运输号
                    </td>
                    <td>
                        备注
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("序号")%>
                            </td>
                            <td>
                                <%#Eval("采购订单号")%>
                            </td>
                            <td>
                                <%#Eval("采购合同号")%>
                            </td>
                            <td>
                                <%#Eval("采购订单日期")%>
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
                                <%#Eval("是否结清")%>
                            </td>
                            <td>
                                <%#Eval("原材料编号")%>
                            </td>
                            <td>
                                <%#Eval("供应商物料编号")%>
                            </td>
                            <td>
                                <%#Eval("描述")%>
                            </td>
                            <td>
                                <%#Eval("供应商名称")%>
                            </td>
                            <td>
                                <%#Eval("采购数量")%>
                            </td>
                            <td>
                                <%#Eval("到货数量")%>
                            </td>
                            <td>
                                <%#Eval("单价")%>
                            </td>
                            <td>
                                <%#Eval("总价")%>
                            </td>
                            <td>
                                <%#Eval("发票号码")%>
                            </td>
                            <td>
                                <%#Eval("开票日期")%>
                            </td>
                            <td>
                                <%#Eval("运输号")%>
                            </td>
                            <td>
                                <%#Eval("备注")%>
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
