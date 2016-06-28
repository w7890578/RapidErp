<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MachineQuoteDetailReport.aspx.cs"
    Inherits="Rapid.SellManager.MachineQuoteDetailReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#btnBack").click(function() {
                window.location.href = "T_MachineQuoteDetailReport.aspx";
            });
        })
    </script>

    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            height: 100%;
            font-size: 14px;
        }
        .border tr td
        {
            background-color: White;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center; font-size: 24px; font-weight: bold; margin-bottom: 20px;">
        加工报价单明细报表</div>
    图纸号：<asp:TextBox runat="server" ID="txtCustomerProductNumber"></asp:TextBox>
    &nbsp;&nbsp;客户名称:<asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>&nbsp;&nbsp;
    BAC物料号:<asp:TextBox ID="txtBACMaterial" runat="server"></asp:TextBox>&nbsp;&nbsp;
    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 10px;" />
    <asp:Button ID="btnEmp" runat="server" Text="导出" OnClick="btnEmp_Click" Style="margin-right: 10px;" />
    <input type="button" value="返回" id="btnBack" />
    <div style="width: 2500px; margin-top: 10px;">
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td class="tdOperar_S#">
                        S#
                    </td>
                    <td class="tdOperar_阶层">
                        阶层
                    </td>
                    <td class="tdOperar_产品编号">
                        产品编号
                    </td>
                    <td class="tdOperar_图纸号">
                        图纸号
                    </td>
                    <td class="tdOperar_BAC物料号">
                        BAC物料号
                    </td>
                    <td class="tdOperar_客户物料号">
                        客户物料号
                    </td>
                    <td class="tdOperar_版本">
                        版本
                    </td>
                    <td class="tdOperar_物料描述">
                        物料描述
                    </td>
                    <td class="tdOperar_BOM用量">
                        BOM用量
                    </td>
                    <td class="tdOperar_原材料单价（未税）">
                        原材料单价（未税）
                    </td>
                    <td class="tdOperar_工时费">
                        工时费
                    </td>
                    <td class="tdOperar_利润（未税）">
                        利润（未税）
                    </td>
                    <td class="tdOperar_管销研费用（未税）">
                        管销研费用（未税）
                    </td>
                    <td class="tdOperar_损耗（未税）">
                        损耗（未税）
                    </td>
                    <td class="tdOperar_单价（未税）">
                        单价（未税）
                    </td>
                    <td class="tdOperar_固定提前期">
                        固定提前期
                    </td>
                    <td>
                        报价单号
                    </td>
                    <td>
                        报价时间
                    </td>
                    <td>
                        客户名称
                    </td>
                    <td>
                        客户联系人
                    </td>
                    <td>
                        创建时间
                    </td>
                    <td class="tdOperar_备注">
                        备注
                    </td>
                    <%--  <td>
                        产品类型
                    </td>--%><%--
                    <td class="tdOperar"  style ="display :none ;>
                        操作
                    </td>--%>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td class="tdOperar_S#" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("S#")%>
                            </td>
                            <td class="tdOperar_阶层" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("阶层")%>
                            </td>
                            <td class="tdOperar_产品编号" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("产成品编号") %>
                            </td>
                            <td class="tdOperar_图纸号" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("客户产成品编号")%>
                            </td>
                            <td class="tdOperar_BAC物料号" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("BAC物料号")%>
                            </td>
                            <td class="tdOperar_客户物料号" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td class="tdOperar_版本" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("IsMaril").ToString ().Equals ("是")?"":Eval("版本")%>
                            </td>
                            <td class="tdOperar_物料描述" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("物料描述")%>
                            </td>
                            <td class="tdOperar_BOM用量" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("BOM用量").ToString ().Replace (".00","")%>
                            </td>
                            <td class="tdOperar_原材料单价（未税）" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("原材料单价未税").ToString().Equals("0.00") ? "" : Eval("原材料单价未税")%>
                            </td>
                            <td class="tdOperar_工时费" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("工时费").ToString().Equals("0.00") ? "" : Eval("工时费")%>
                            </td>
                            <td class="tdOperar_利润（未税）" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("利润").ToString().Equals("0.00") ? "" : Eval("利润")%>
                            </td>
                            <td class="tdOperar_管销研费用（未税）" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <span style="display: <%#Eval("Isone").ToString ().Equals ("是")?"inline":"none" %>;">
                                    <%#Eval("管销研费用未税").ToString().Equals("0.00") ? "" : Eval("管销研费用未税")%></span>
                            </td>
                            <td class="tdOperar_损耗（未税）" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <span style="display: <%#Eval("Isone").ToString ().Equals ("是")?"inline":"none" %>;">
                                    <%#Eval("损耗未税").ToString().Equals("0.00") ? "" : Eval("损耗未税")%></span>
                            </td>
                            <td class="tdOperar_单价（未税）" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("单价未税").ToString().Equals("0.00") ? "" : Eval("单价未税")%>
                            </td>
                            <td class="tdOperar_固定提前期" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("固定提前期")%>
                            </td>
                            <td style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("报价单号") %>
                            </td>
                            <td style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("报价时间") %>
                            </td>
                            <td style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("客户名称") %>
                            </td>
                            <td style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("客户联系人") %>
                            </td>
                            <td style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("创建时间") %>
                            </td>
                            <td class="tdOperar_备注" style="background-color: <%#Eval("Isone").ToString ().Equals ("是")?"Yellow":"White" %>;">
                                <%#Eval("备注")%>
                            </td>
                            <%--
                            <td class="tdOperar" style ="display :none ;">
                                <span style="display: <%=hasEdit %>;"><a href="###" onclick="edit('<%#Eval("Guid")%>')">
                                    编辑</a> </span><span style="display: <%=hasDelete %>;"><a href="###" onclick="Delete('<%#Eval("Guid")%>')">
                                        删除</a></span>
                            </td>--%>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
