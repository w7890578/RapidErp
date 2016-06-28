<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductModelReport.aspx.cs" Inherits="Rapid.ProduceManager.ProductModelReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品型号统计报表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>
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
    </style>
</head>
<body>
    <form id="form2" runat="server">
        <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold; margin-top: 20px">
            产品型号统计报表
        </div>
        <div>
            <input type="hidden" id="Hidden1" runat="server" />
            <div id="divHeader" style="margin-top: 20px;">
                <div style="position: relative; float: left; margin-bottom: 10px">
                    <asp:Label ID="Label3" runat="server" Text="产成品编号:" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtProductNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    <asp:Label ID="Label1" runat="server" Text="客户物料号:" Style="margin-left: 20px;"></asp:Label>
                    <asp:TextBox ID="txtNumber" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                    <asp:Label ID="Label2" runat="server" Text="年度：" Style="margin-left: 20px;"></asp:Label>
                    <asp:DropDownList ID="ddlYear" runat="server">
                        <asp:ListItem Value="" Text="">----请选择----</asp:ListItem>
                        <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
                        <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
                        <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
                        <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
                        <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
                        <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
                        <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;
             <asp:Button ID="Button1" runat="server" Text="查询" OnClick="btnSearch_Click" />
                    &nbsp;&nbsp;&nbsp;
                 <%
                     bool exp = Rapid.ToolCode.Tool.GetUserMenuFunc("L0122", "exp");
                 %>
                    <span style="display: <%=exp?"":"none"%>;">
                        <asp:Button ID="Button2" runat="server" Text="导出Excel" OnClick="btnEmp_Click" />
                    </span>

                </div>
            </div>
            <br />
            <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
                <thead>
                    <tr>
                        <td class="tdOperar_产成品编号">产成品编号
                        </td>
                        <td class="tdOperar_客户物料号">客户物料号
                        </td>
                        <td class="tdOperar_版本">版本
                        </td>
                        <td class="tdOperar_年度">年度
                        </td>
                        <td class="tdOperar_1月">1月
                        </td>
                        <td class="tdOperar_2月">2月
                        </td>
                        <td class="tdOperar_3月">3月
                        </td>
                        <td class="tdOperar_4月">4月
                        </td>
                        <td class="tdOperar_5月">5月
                        </td>
                        <td class="tdOperar_6月">6月
                        </td>
                        <td class="tdOperar_1-6月小计">1-6月小计
                        </td>
                        <td class="tdOperar_7月">7月
                        </td>
                        <td class="tdOperar_8月">8月
                        </td>
                        <td class="tdOperar_9月">9月
                        </td>
                        <td class="tdOperar_10月">10月
                        </td>
                        <td class="tdOperar_11月">11月
                        </td>
                        <td class="tdOperar_12月">12月
                        </td>
                        <td class="tdOperar_7-12月小计">7-12月小计
                        </td>
                        <td class="tdOperar_客户编号合计">客户编号合计
                        </td>
                        <td class="tdOperar_瑞普迪编号合计">瑞普迪编号合计
                        </td>
                    </tr>

                </thead>
                <tbody>
                    <asp:Repeater runat="server" ID="rptList">
                        <ItemTemplate>
                            <tr>
                                <td class="tdOperar_产成品编号">
                                    <%#Eval("产成品编号")%>
                                </td>
                                <td class="tdOperar_客户物料号">
                                    <%#Eval("客户物料号")%>
                                </td>
                                <td class="tdOperar_版本">
                                    <%#Eval("版本")%>
                                </td>
                                <td class="tdOperar_年度">
                                    <%#Eval("年度")%>
                                </td>
                                <td class="tdOperar_1月">
                                    <%#Eval("1月")%>
                                </td>
                                <td class="tdOperar_2月">
                                    <%#Eval("2月")%>
                                </td>
                                <td class="tdOperar_3月">
                                    <%#Eval("3月")%>
                                </td>
                                <td class="tdOperar_4月">
                                    <%#Eval("4月")%>
                                </td>
                                <td class="tdOperar_5月">
                                    <%#Eval("5月")%>
                                </td>
                                <td class="tdOperar_6月">
                                    <%#Eval("6月")%>
                                </td>
                                <td class="tdOperar_1-6月小计">
                                    <%#Eval("1-6月小计")%>
                                </td>
                                <td class="tdOperar_7月">
                                    <%#Eval("7月")%>
                                </td>
                                <td class="tdOperar_8月">
                                    <%#Eval("8月")%>
                                </td>
                                <td class="tdOperar_9月">
                                    <%#Eval("9月")%>
                                </td>
                                <td class="tdOperar_10月">
                                    <%#Eval("10月")%>
                                </td>
                                <td class="tdOperar_11月">
                                    <%#Eval("11月")%>
                                </td>
                                <td class="tdOperar_12月">
                                    <%#Eval("12月")%>
                                </td>
                                <td class="tdOperar_7-12月小计">
                                    <%#Eval("7-12月小计")%>
                                </td>
                                <td class="tdOperar_客户编号合计">
                                    <%#Eval("客户编号合计")%>
                                </td>
                                <td class="tdOperar_瑞普迪编号合计">
                                    <%#Eval("瑞普迪编号合计")%>
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


