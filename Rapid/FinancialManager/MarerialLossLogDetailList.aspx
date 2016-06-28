<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarerialLossLogDetailList.aspx.cs"
    Inherits="Rapid.FinancialManager.MarerialLossLogDetailList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>原材料损耗明细</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        .border {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }

            .border thead tr td {
                padding: 4px;
                background-color: white;
            }

            .border tbody tr td {
                padding: 4px;
                background-color: white;
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

            #choosePrintClounm ul {
                margin-bottom: 10px;
            }

            #choosePrintClounm div {
                text-align: center;
                color: Green;
            }

            #choosePrintClounm ul li {
                list-style: none;
                float: left;
                width: 100%;
                cursor: pointer;
            }

        .style1 {
            width: 46px;
        }


        table.tablesorter thead tr td {
            color: black;
            font-size: 10pt;
            padding: 4px;
            font-weight: 300;
            background: url() repeat-x left top;
            border: 1px solid green;
            padding: 0px 10px;
            height: 24px;
            line-height: 24px;
        }
    </style>
</head>
<body style="background-color:#F3FFE3">
    <form id="form1" runat="server">
    <div>
        <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            <img src="../Img/loading.gif" alt="loading" />
        </div>
    <%--    <table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="9" background="../Img/tab_12.gif">
                                &nbsp;
                            </td>
                            <td bgcolor="#f3ffe3" style="padding-top: 5px;">
                                
                            </td>
                            <td width="9" background="../Img/tab_16.gif">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            
        </table>--%>
        <div  >
                                    <input type="hidden" id="Hidden1" runat="server" />
                                    <div id="Div1" style="position: absolute; top: 40%; left: 50%; display: none;">
                                        <img src="../Img/loading.gif" alt="loading" />
                                    </div>
             <div style="width: 100%; text-align: right; padding-right: 50px; margin: 5px;">
                    <asp:Button ID="Button1" runat="server" Text="导出Excel" OnClick="Button1_Click"
                        CssClass="button" />

                </div>
                                    <table class="pg_table">
                                        <tr>
                                            <td colspan="14">
                                             <%--   <div id="outsideDiv">--%>
                                                    <table class="tablesorter" cellpadding="1" cellspacing="1"  ">
                                                        <thead>
                                                             <tr  >
                                                                <td   class="tdOperar_产成品编号" style="width: 150px; ">
                                                                    产成品编号
                                                                </td>
                                                                <td class="tdOperar_原材料编号" style="width: 100px">
                                                                    原材料编号
                                                                </td>
                                                                
                                                                <%-- <td class="tdOperar_供应商物料编号" style="width: 100px">
                                                                    供应商物料编号
                                                                </td>
                                                                 <td class="tdOperar_客户物料编号" style="width: 100px">
                                                                    客户物料编号
                                                                </td>--%>
                                                                <td class="tdOperar_日期" style="width: 100px">
                                                                    日期
                                                                </td>
                                                                <td class="tdOperar_班组" style="width: 100px">
                                                                    班组
                                                                </td>
                                                                <td class="tdOperar_数量" style="width: 100px">
                                                                    数量
                                                                </td>
                                                                <td class="tdOperar_补领人" style="width: 100px">
                                                                    补领人
                                                                </td>
                                                                <td class="tdOperar_损耗原因" style="width: 100px">
                                                                    损耗原因
                                                                </td>
                                                                <td class="tdOperar_原材料描述"  style="width: 100px">
                                                                    原材料描述
                                                                </td>
                                                                <td class="tdOperar_原材料名称" style="width: 100px">
                                                                    原材料名称
                                                                </td>
                                                                 <td class="tdOperar_单价" style="width: 100px">
                                                                    单价
                                                                </td>
                                                                <td class="tdOperar_总价" style="width: 100px">
                                                                    总价
                                                                </td>
                                                                <td class="tdOperar_备注" >
                                                                    备注
                                                                </td>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                           
                                                            <asp:Repeater runat="server" ID="rpList">
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td class="tdOperar_产成品编号">
                                                                            <%#Eval("产成品编号")%>
                                                                        </td>
                                                                        <td class="tdOperar_原材料编号">
                                                                            <%#Eval("原材料编号")%>
                                                                        </td>
                                                                       <%--  <td class="tdOperar_供应商物料编号">
                                                                            <%#Eval("供应商物料编号")%>
                                                                        </td>
                                                                         <td class="tdOperar_客户物料编号">
                                                                            <%#Eval("客户物料编号")%>
                                                                        </td>--%>
                                                                        <td class="tdOperar_日期">
                                                                            <%#Eval("日期")%>
                                                                        </td>
                                                                        <td class="tdOperar_班组">
                                                                            <%#Eval("班组")%>
                                                                        </td>
                                                                        <td class="tdOperar_数量">
                                                                            <%#Eval("数量")%>
                                                                        </td>
                                                                        <td class="tdOperar_补领人">
                                                                            <%#Eval("补领人")%>
                                                                        </td>
                                                                        <td class="tdOperar_损耗原因">
                                                                            <%#Eval("损耗原因")%>
                                                                        </td>
                                                                        <td class="tdOperar_原材料描述">
                                                                            <%#Eval("原材料描述")%>
                                                                        </td>
                                                                        <td class="tdOperar_原材料名称">
                                                                            <%#Eval("原材料名称")%>
                                                                        </td>
                                                                        <td class="tdOperar_单价">
                                                                            <%#Eval("单价")%>
                                                                        </td>
                                                                        <td class="tdOperar_总价">
                                                                            <%#Eval("总价")%>
                                                                        </td>
                                                                        <td class="tdOperar_备注">
                                                                            <%#Eval("备注")%>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </tbody>
                                                        <tfoot>
                                                            <tr>
                                                                <td colspan="14" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 5px;
                                                                    padding-right: 5px;">
                                                                    <div id="pageing" class="pages clearfix">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tfoot>
                                                    </table>
                                         <%--       </div>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
    </div>
    </form>
</body>
</html>
