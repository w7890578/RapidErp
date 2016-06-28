<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpireOrderViewDetail.aspx.cs"
    Inherits="Rapid.ProduceManager.ExpireOrderViewDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>未交加工销售订单明细</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

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
            width: 1200px;
        }
    </style>
</head>
<body style="padding: 5px 10px 0px 0px;">
    <form id="form1" runat="server">
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 740px;
            font-size: 14px;
            text-align: center;
        }
        .border thead tr td
        {
            padding: 4px;
            background-color: white;
        }
        .border tbody tr td
        {
            padding: 4px;
            background-color: white;
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
            top: 25px;
            left: 330px;
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
    <div class="printDiv" id="upDiv">
        <table class="pg_table">
            <tr>
                <td colspan="13">
                    <div>
                        <input type="hidden" id="hdnumber" runat="server" />
                        <div id="divHeader">
                            <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-bottom: 10px">
                                &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
                                <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;过期未交订单明细</span>
                            </div>
                            <div style="position: relative; float: left; margin-bottom: 10px">
                                &nbsp;&nbsp;
                                <asp:Label ID="Label1" runat="server" Text="销售订单号："></asp:Label>
                                <asp:TextBox ID="txtOdersNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" Text="产成品编号："></asp:Label>
                                <asp:TextBox ID="txtProductNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                                <asp:Label ID="Label3" runat="server" Text="交期："></asp:Label>
                                <asp:TextBox ID="txtLeadTime" runat="server" CssClass="input required" size="25"
                                    onfocus="WdatePicker({skin:'green'})" Style="margin-right: 10px;"></asp:TextBox>
                                     <asp:Label ID="Label4" runat="server" Text="客户采购订单号："></asp:Label>
                                <asp:TextBox ID="txtCustomerOrderNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" class="button" />
                                <%
                                    bool exp = Rapid.ToolCode.Tool.GetUserMenuFunc("L0120", "exp");
                 %>
                    <span style="display: <%=exp?"":"none"%>;">
                                <asp:Button ID="btnExcel" runat="server" Text="导出Excel" Style="margin-left: 10px;"
                                    OnClick="btnExcel_Click" CssClass="button" Width="80px" />
                            </span></div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="13">
                    <table class="tablesorter" cellpadding="1" cellspacing="1" id="printTalbe" style="width: 1150px">
                        <thead>
                            <tr>
                                <td class="tdOperar_销售订单号">
                                    销售订单号
                                </td>
                                 <td class="tdOperar_客户采购订单号">
                                    客户采购订单号
                                </td>
                                <td class="tdOperar_行号">
                                    行号
                                </td>
                                <td class="tdOperar_产成品编号">
                                    产成品编号
                                </td>
                                <td class="tdOperar_版本">
                                    版本
                                </td>
                                 
                                <td class="tdOperar_客户产成品编号">
                                    客户产成品编号
                                </td>
                                <td class="tdOperar_订单日期">
                                    订单日期
                                </td>
                                <td class="tdOperar_交期">
                                    交期
                                </td>
                                <td class="tdOperar_数量">
                                    数量
                                </td>
                                <td class="tdOperar_未交数量">
                                    未交数量
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpList">
                                <ItemTemplate>
                                    <tr>
                                        <td class="tdOperar_销售订单号">
                                            <%#Eval("销售订单号")%>
                                        </td>
                                         <td class="tdOperar_客户采购订单号">
                                            <%#Eval("客户采购订单号")%>
                                        </td>
                                        <td class="tdOperar_行号">
                                            <%#Eval("行号")%>
                                        </td>
                                        <td class="tdOperar_产成品编号">
                                            <%#Eval("产成品编号")%>
                                        </td>
                                        <td class="tdOperar_版本">
                                            <%#Eval("版本")%>
                                        </td>
                                        
                                        <td class="tdOperar_客户产成品编号">
                                            <%#Eval("客户产成品编号")%>
                                        </td>
                                        <td class="tdOperar_订单日期">
                                            <%#Eval("订单日期")%>
                                        </td>
                                        <td class="tdOperar_交期">
                                          <%#Eval("销售订单号").ToString ().Equals ("合计")?"":Eval ("交期")%>    
                                        </td>
                                        <td class="tdOperar_数量">
                                            <%#Eval("数量")%>
                                        </td>
                                        <td class="tdOperar_未交数量">
                                            <%#Eval("未交数量")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
