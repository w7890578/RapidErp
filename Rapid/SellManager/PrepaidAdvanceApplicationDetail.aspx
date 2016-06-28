<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrepaidAdvanceApplicationDetail.aspx.cs"
    Inherits="Rapid.SellManager.PrepaidAdvanceApplicationDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售收明细</title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        function Edit(guid) {
            OpenDialog("../PurchaseManager/EditPrepaidAccountsApplicationDetail.aspx?Guid=" + guid + "&time=" + new Date() + "&IsSK=true&fatherGuid=<%=fatherGuid%>", "btnSearch", "210", "570");

        }
        $(function() {
            //查询sql语句
            var sq = getQueryString("SQ");
            var sp = getQueryString("SP");
            var ck = getQueryString("ck");
            var isYS = getQueryString("isYS");
            if (isYS == 1) {
                $("#divTitle").html("销售预收明细");
            }
            else {
                $("#divTitle").html("销售应收明细");
            }
            if (sq == "1") {
                $("#btnBack").click(function() {
                    window.location.href = "../SellManager/PrepaidAdvanceApplication.aspx?isYS=" + isYS;
                });
            }
            if (sp == "2") {
                $("#btnBack").click(function() {
                    window.location.href = "../FinancialManager/PrepaidAdvanceCheck.aspx";

                });
            }
            if (ck == "3") {
                $("#btnBack").click(function() {
                    window.location.href = "../FinancialManager/PrepaidAdvanceLookOver.aspx";
                });
            }
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
    </style>
    <input type="hidden" runat="server" id="hdBackUrl" />
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px" id="divTitle">
    </div>
    <div style="margin-bottom: 10px;">
        <input type="hidden" id="hdnumber" runat="server" />
        <input type="hidden" id="hdType" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="width: 100%">
                &nbsp&nbsp; 客户采购合同号：<asp:TextBox ID="txtCustomerOdersNumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 瑞普迪编号：<asp:TextBox ID="txtProductNumber" runat="server"></asp:TextBox>
                &nbsp&nbsp; 客户物料编号：<asp:TextBox ID="txtCustomerMaterialNumber" runat="server"></asp:TextBox>
                <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click"
                    Style="margin-right: 10px; margin-left: 10px;" />
                <input type="button" value="返回" id="btnBack" style="margin-right: 10px;" />
                &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td>
                        送货单号
                    </td>
                    <td>
                        销售订单号
                    </td>
                    <td>
                        客户采购订单号
                    </td>
                    <td>
                        瑞普迪编号
                    </td>
                    <td>
                        客户物料编号
                    </td>
                    <td>
                        版本
                    </td>
                    <td>
                        描述
                    </td>
                    <td>
                        交货数量
                    </td>
                    <td>
                        单价
                    </td>
                    <td>
                        总价
                    </td>
                    <td>
                        行号
                    </td>
                    <td>
                        发票号码
                    </td>
                    <td>
                        开票日期
                    </td>
                    <td>
                        备注
                    </td>
                     <td  >
                        操作
                    </td> 
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("送货单号")%>
                            </td>
                            <td>
                                <%#Eval("销售订单号")%>
                            </td>
                            <td>
                                <%#Eval("客户采购订单号")%>
                            </td>
                            <td>
                                <%#Eval("瑞普迪编号")%>
                            </td>
                            <td>
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td>
                                <%#Eval("版本")%>
                            </td>
                            <td>
                                <%#Eval("描述")%>
                            </td>
                            <td>
                                <%#Eval("交货数量")%>
                            </td>
                            <td>
                                <%#Eval("单价")%>
                            </td>
                            <td>
                                <%#Eval("总价")%>
                            </td>
                            <td>
                                <%#Eval("行号")%>
                            </td>
                            <td>
                                <%#Eval("发票号码")%>
                            </td>
                            <td>
                                <%#Eval("开票日期")%>
                            </td>
                            <td>
                                <%#Eval("备注")%>
                            </td>
                             <td   >
                                <span ><a href="###" onclick="Edit('<%#Eval("Guid") %>')">
                                    编辑</a></span>
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
