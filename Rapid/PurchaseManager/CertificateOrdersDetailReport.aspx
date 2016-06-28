<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificateOrdersDetailReport.aspx.cs"
    Inherits="Rapid.PurchaseManager.CertificateOrdersDetailReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购明细报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

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
        采购明细报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="margin-top: 20px;">
            <div style="position: relative; float: left; margin-bottom: 10px">
                &nbsp;&nbsp;
                <asp:Label ID="txtd" runat="server" CssClass="input required" Text="客户物料编号："></asp:Label>
                <asp:TextBox ID="txtCutomerMaterialNumber" runat="server"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="供应商名称：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtSupplyName" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                <input type="hidden" id="saveInfo" runat="server" />
                <asp:Button ID="btnExcel" runat="server" Text="导出Excel" 
                    style="margin-left:10px;" onclick="btnExcel_Click" />
            </div>
        </div>
        <br />
        <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
            <thead>
                <tr>
                    <td class="tdOperar_原材料编号">
                        原材料编号
                    </td>
                    <td class="tdOperar_客户物料编号">
                        客户物料编号
                    </td>
                    <td class="tdOperar_描述">
                        描述
                    </td>
                    <td class="tdOperar_ 供应商型号">
                        供应商型号
                    </td>
                    <td class="tdOperar_采购数量">
                        采购数量
                    </td>
                    <td class="tdOperar_ 单价">
                        单价
                    </td>
                    <td class="tdOperar_金额">
                        金额
                    </td>
                    <td class="tdOperar_ 采购日期">
                        采购日期
                    </td>
                    <td class="tdOperar_预计交期">
                        预计交期
                    </td>
                    <td class="tdOperar_ 到货日期">
                        到货日期
                    </td>
                    <td class="tdOperar_供应商名称">
                        供应商名称
                    </td>
                    <td class="tdOperar_联系人">
                        联系人
                    </td>
                    <td class="tdOperar_联系电话">
                        联系电话
                    </td>
                    <td class="tdOperar_QQ">
                        QQ
                    </td>
                    <td class="tdOperar_Email">
                        Email
                    </td>
                    <td class="tdOperar_备注">
                        备注
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td class="tdOperar_原材料编号">
                                <%#Eval("原材料编号")%>
                            </td>
                            <td class="tdOperar_客户物料编号">
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td class="tdOperar_描述">
                                <%#Eval("描述")%>
                            </td>
                            <td class="tdOperar_ 供应商型号">
                                <%#Eval("供应商型号")%>
                            </td>
                            <td class="tdOperar_采购数量">
                                <%#Eval("采购数量")%>
                            </td>
                            <td class="tdOperar_ 单价">
                                <%#Eval("单价")%>
                            </td>
                            <td class="tdOperar_金额">
                                <%#Eval("金额")%>
                            </td>
                            <td class="tdOperar_ 采购日期">
                                <%#Eval("采购日期")%>
                            </td>
                            <td class="tdOperar_预计交期">
                                <%#Eval("预计交期")%>
                            </td>
                            <td class="tdOperar_ 到货日期">
                                <%#Eval("到货日期")%>
                            </td>
                            <td class="tdOperar_供应商名称">
                                <%#Eval("供应商名称")%>
                            </td>
                            <td class="tdOperar_联系人">
                                <%#Eval("联系人")%>
                            </td>
                            <td class="tdOperar_联系电话">
                                <%#Eval("联系电话")%>
                            </td>
                            <td class="tdOperar_QQ">
                                <%#Eval("QQ")%>
                            </td>
                            <td class="tdOperar_Email">
                                <%#Eval("Email")%>
                            </td>
                            <td class="tdOperar_备注">
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
