<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificateDemandReport.aspx.cs"
    Inherits="Rapid.PurchaseManager.CertificateDemandReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购需求报表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
             $(function() {
             var querySql = "";

             //获取查询条件
             function GetQueryCondition() {
                 var condition = " where 1=1 ";
                 return condition;
             }

             //导出Execl前将查询条件内容写入隐藏标签
             function ImpExecl() {
                 querySql = "   select * from V_SupplyMaterialInfoReport";
                 querySql = querySql + " " + GetQueryCondition();
                 $("#saveInfo").val(querySql + "");
                 return true;
             }
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
            top: 25px;
            left: 540px;
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
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        采购需求报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="margin-top: 20px;">
            <div style="margin-bottom: 5px">
                <asp:Label ID="txtd" runat="server" Text="采购日期：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtOrderDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                <asp:Label ID="Label6" runat="server" Text="采购订单号：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtOrdersNumber" runat="server"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="原材料编号：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtMaterialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" Text="合同号："></asp:Label>
                <asp:TextBox ID="txtHDnumber" runat="server"></asp:TextBox>
            </div>
            <div>
                <asp:Label ID="Label3" runat="server" Text="供应商物料编号：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtSupplierMateialNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" Text="供应商名称：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtSupplierName" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <asp:Label ID="Label5" runat="server" Text="备注：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtRemark" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
            </div>
        </div>
        <br />
        <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
            <thead>
                <tr>
                    <td>
                        序号
                    </td>
                    <td>
                        采购日期
                    </td>
                    <td>
                        采购订单号
                    </td>
                    <td>
                        合同号
                    </td>
                    <td>
                        原材料编号
                    </td>
                    <td>
                        描述
                    </td>
                    <td>
                        供应商物料编号
                    </td>
                    <td>
                        库存数量
                    </td>
                    <td>
                        在途数量
                    </td>
                    <td>
                        安全用量
                    </td>
                    <td>
                        实际订单数量
                    </td>
                    <td>
                        计算结果
                    </td>
                    <td>
                        预计交期
                    </td>
                    <td>
                        供应商名称
                    </td>
                    <td>
                        单价
                    </td>
                    <td>
                        总价
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
                                <%#Eval("采购日期")%>
                            </td>
                            <td>
                                <%#Eval("采购订单号")%>
                            </td>
                            <td>
                                <%#Eval("合同号")%>
                            </td>
                            <td>
                                <%#Eval("原材料编号")%>
                            </td>
                            <td>
                                <%#Eval("描述")%>
                            </td>
                            <td>
                                <%#Eval("供应商物料编号")%>
                            </td>
                            <td>
                                <%#Eval("库存数量")%>
                            </td>
                            <td>
                                <%#Eval("在途数量")%>
                            </td>
                            <td>
                                <%#Eval("安全用量")%>
                            </td>
                            <td>
                                <%#Eval("实际订单数量")%>
                            </td>
                            <td>
                                <%#Eval("计算结果")%>
                            </td>
                            <td>
                                <%#Eval("预计交期")%>
                            </td>
                            <td>
                                <%#Eval("供应商名称")%>
                            </td>
                            <td>
                                <%#Eval("单价")%>
                            </td>
                            <td>
                                <%#Eval("总价")%>
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
