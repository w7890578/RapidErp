﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManufactureProdudctStockMoney.aspx.cs"
    Inherits="Rapid.FinancialManager.ManufactureProdudctStockMoney" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>在制品库存金额报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1 {
            width: 200px;
        }
    </style>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            //查询sql语句

            $("#btnPrint").click(function () {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function () {
                $("#choosePrintClounm").hide();
            });
            $("#btnChoosePrintColum").click(function () {
                var chooseResult = "";
                var unChooseResult = "";
                var arrChk = $("input[name='columList']:checkbox");
                $(arrChk).each(function () {
                    if ($(this).is(':checked')) {
                        chooseResult += $(this).val() + ",";

                    }
                    else {
                        unChooseResult += $(this).val() + ",";

                    }
                });
                var reg = /,$/gi;
                chooseResult = chooseResult.replace(reg, "");
                unChooseResult = unChooseResult.replace(reg, "");

                var unChoosedArray = unChooseResult.split(',');
                if (chooseResult == "") {
                    alert("请选择要打印的列!");
                    return;
                }
                if (!confirm("确定打印所选列？")) {
                    return;
                }
                //遍历border样式的table下的td
                $("#printTalbe tr td").each(function () {
                    className = $(this).attr("class");
                    if (className == "tdOperar") {
                        $(this).hide();
                    }
                    for (var j = 0; j < unChoosedArray.length; j++) {
                        if (className == unChoosedArray[j] + "") {
                            $(this).hide();
                        }
                    }
                });
                $("#printTalbe").removeClass("tablesorter").addClass("border");
                newwin = window.open("", "newwin", "height=900,width=750,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
                newwin.document.body.innerHTML = document.getElementById("form1").innerHTML;
                newwin.document.getElementById("divHeader").style.display = 'none';
                newwin.document.getElementById("choosePrintClounm").style.display = 'none';

                newwin.window.print();
                newwin.window.close();
                $("#choosePrintClounm").hide();
                $("#printTalbe tr td").each(function () {
                    $(this).show();
                })

                $("#printTalbe").removeClass("border").addClass("tablesorter");
            });
        });
        var querySql = "";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            return condition;
        }
    </script>

    <style type="text/css">
        .printDiv {
            border-radius: 5px;
            border: 1px solid #B3D08F;
            margin-top: 5px;
            margin-right: 10px;
            background-color: #F3FFE3;
            width: 100%;
        }
    </style>
</head>
<body style="padding: 5px 10px 0px 0px;">
    <form id="form1" runat="server">
        <style type="text/css">
            .border {
                background-color: Black;
                width: 740px;
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
                top: 25px;
                left: 300px;
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
        </style>
        <div class="printDiv" id="upDiv">
            <table class="pg_table">
                <tr>
                    <td td colspan="13">
                        <div>
                            <input type="hidden" id="hdnumber" runat="server" />
                            <div id="divHeader">
                                <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-bottom: 10px">
                                    &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
                                    <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;账务管理&nbsp;&nbsp;>&nbsp;&nbsp;在制品库存金额报表</span>
                                </div>
                                <div style="position: relative; float: left; margin-bottom: 10px; top: 0px; left: 0px;">
                                    &nbsp;&nbsp;
                                <asp:Label ID="Label1" runat="server" Text="产成品编号："></asp:Label>
                                    <asp:TextBox ID="txtProductnumber" runat="server"></asp:TextBox>
                                    <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" class="button" />
                                    <input type="button" value="打印" id="btnPrint" style="margin-left: 10px" class="button" />
                                    <asp:Button ID="btnEmp" runat="server" Text="导出Excel" class="button"
                                        OnClick="btnEmp_Click" />
                                    <input type="hidden" id="Hidden1" runat="server" />
                                    <div id="choosePrintClounm">
                                        <div>
                                            请选择要打印的列
                                        </div>
                                        <ul>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_产成品编号" checked="checked" />
                                                    产成品编号
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_版本" checked="checked" />
                                                    版本
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_客户产成品编号" checked="checked" />
                                                    客户产成品编号
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_在制数量" checked="checked" />
                                                    在制数量
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_成本价" checked="checked" />
                                                    成本价
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_金额" checked="checked" />
                                                    金额
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_描述" checked="checked" />
                                                    描述
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_客户名称" checked="checked" />
                                                    客户名称
                                                </label>
                                            </li>
                                        </ul>
                                        <div>
                                            &nbsp;<br />
                                            <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                                                type="button" value=" 取 消 " id="btnExit" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="13">
                        <table class="tablesorter" cellpadding="1" cellspacing="1" id="printTalbe">
                            <thead>
                                <tr>
                                    <td class="tdOperar_产成品编号">产成品编号
                                    </td>
                                    <td class="tdOperar_版本">版本
                                    </td>
                                    <td class="tdOperar_描述" style="width: 100px">描述
                                    </td>
                                    <td class="tdOperar_客户名称" style="width: 100px">客户名称
                                    </td>
                                    <td class="tdOperar_在制数量">在制数量
                                    </td>
                                    <td class="tdOperar_成本价">成本价
                                    </td>
                                    <td class="tdOperar_金额">金额
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
                                            <td class="tdOperar_版本">
                                                <%#Eval("版本")%>
                                            </td>
                                            <td class="tdOperar_描述">
                                                <%#Eval("描述")%>
                                            </td>
                                            <td class="tdOperar_客户名称">
                                                <%#Eval("客户名称")%>
                                            </td>
                                            <td class="tdOperar_在制数量">
                                                <%#Eval("在制数量")%>
                                            </td>
                                            <td class="tdOperar_成本价">
                                                <%#Eval("成本价")%>
                                            </td>
                                            <td class="tdOperar_金额">
                                                <%#Eval("金额")%>
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