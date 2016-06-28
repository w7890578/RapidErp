<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarerialScrapLogList.aspx.cs"
    Inherits="Rapid.FinancialManager.MarerialScrapLogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>原材料报废信息表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
        }

        body {
            font: 14px Verdana, Arial, Helvetica, sans-serif;
        }

        ul {
            list-style: none;
        }

        #tab {
            margin: 15px auto;
            width: 100%;
        }

            #tab ul {
                overflow: hidden;
                zoom: 1;
            }

            #tab li {
                float: left;
                margin-right: 8px;
                width: 200px;
                height: 30px;
                line-height: 30px;
                border: 1px solid green;
                border-bottom: 0;
                cursor: pointer;
                text-align: center;
                border-top-left-radius: 5px;
                border-top-right-radius: 5px;
            }

                #tab li.on {
                    background: green;
                    color: White;
                    font-size: 16px;
                    font-weight: bold;
                }

        #content {
            border-top: 4px solid green;
            background-color: #f3ffe3;
        }

            #content div {
            }

                #content div.show {
                    display: block;
                }
    </style>

    <script type="text/javascript">
        $(function () {
            $("#menus-tab li").click(function () {
                $(this).addClass("on").siblings().removeClass("on");
                var title = $(this).html();
                var year = $("#drpYear").val();
                var month = $("#drpMonth").val();
                var startDateTime = $("#startDateTime").val();
                var endDateTime = $("#endDateTime").val();

                if (title == "原材料报废明细列表") {
                    $("#yclbaofei").hide();
                    $("#yclbaofeimingxi").show().html("<iframe style='position: relative; background-color: transparent;' width='100%' height='400' frameborder='0' src='MarerialScrapLogListDetailList.aspx?year=" + year + "&month=" + month + "&startDateTime=" + startDateTime + "&endDateTime=" + endDateTime + "&date=" + new Date() + "'></iframe>");
                }
                else {
                    $("#yclbaofei").show();
                    $("#yclbaofeimingxi").hide();
                }

            });
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <!--背景top-->
            <tr>
                <td height="30">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="15" height="30">
                                <img src="../Img/tab_03.gif" width="15" height="30" />
                            </td>
                            <td width="1101" background="../Img/tab_05.gif" style="padding: 5px;">
                                <img src="../Img/311.gif" width="16" height="16" />
                                <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;账务管理&nbsp;&nbsp;>&nbsp;&nbsp;原材料报废列表</span>
                            </td>
                            <td width="281" background="../Img/tab_05.gif">
                                <table border="0" align="right" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="60"></td>
                                        <td width="52"></td>
                                        <td width="60"></td>
                                        <td width="60"></td>
                                    </tr>
                                </table>
                            </td>
                            <td width="14">
                                <img src="../Img/tab_07.gif" width="15" height="30" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--主内容-->
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="10" background="../Img/tab_12.gif">&nbsp;
                            </td>
                            <td bgcolor="#f3ffe3" style="padding-top: 5px;">
                                <div>
                                    <input type="hidden" id="saveInfo" runat="server" />
                                    <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                                        <img src="../Img/loading.gif" alt="loading" />
                                    </div>
                                    <table class="pg_table">
                                        <tr>
                                            <td class="pg_talbe_head">年份：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <asp:DropDownList ID="drpYear" runat="server" OnSelectedIndexChanged="drpYear_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="pg_talbe_head">月份：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <asp:DropDownList ID="drpMonth" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="pg_talbe_content">开始日期时间：
                                            </td>
                                            <td class="pg_talbe_head">
                                                <asp:TextBox runat="server" ID="startDateTime" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>

                                            </td>
                                            <td class="pg_talbe_content">结束日期时间：
                                            </td>
                                            <td class="pg_talbe_head">
                                                <asp:TextBox runat="server" ID="endDateTime" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                                                
                                            </td>

                                            <td class="pg_talbe_head">
                                                <asp:Button ID="btnSearch" runat="server" Text="查询" class="button" OnClick="btnSearch_Click" />
                                            </td>
                                            <td class="pg_talbe_content"></td>
                                            <td class="pg_talbe_head"></td>
                                            <td class="pg_talbe_content"></td>
                                            <td class="pg_talbe_head"></td>
                                            <td class="pg_talbe_content"></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="tab">
                                    <ul id="menus-tab">
                                        <li class="on">原材料报废列表</li>
                                        <li>原材料报废明细列表</li>
                                    </ul>
                                    <div id="content">
                                        <div class="show" id="yclbaofei">


                                            <div id="showOne">
                                                <table class="tablesorter" cellpadding="1" cellspacing="1" id="printTalbe">
                                                    <thead>
                                                        <tr>
                                                            <td class="tdOperar_班组">班组
                                                            </td>
                                                            <td class="tdOperar_数量">数量
                                                            </td>
                                                            <td class="tdOperar_金额">金额
                                                            </td>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rpList">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="tdOperar_班组">
                                                                        <%#Eval("班组")%>
                                                                    </td>
                                                                    <td class="tdOperar_数量">
                                                                        <%#Eval("数量")%>
                                                                    </td>
                                                                    <td class="tdOperar_金额">
                                                                        <%#Eval("金额")%>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <tr>
                                                            <td>总计</td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lbSumQty"></asp:Label></td>
                                                            <td>
                                                                <asp:Label runat="server" ID="lbSumPrice"></asp:Label></td>
                                                        </tr>
                                                    </tbody>
                                                    <tfoot>
                                                        <tr>
                                                            <td colspan="3" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 5px; padding-right: 5px;">
                                                                <div id="pageing" class="pages clearfix">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </tfoot>
                                                </table>
                                            </div>

                                        </div>
                                        <div id="yclbaofeimingxi" style="display: none;">
                                            原材料报废明细列表
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
                <td width="9" background="../Img/tab_16.gif">&nbsp;
                </td>
            </tr>
            <!--背景down-->
            <tr>
                <td height="29">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="15" height="29">
                                <img src="../Img/tab_20.gif" width="15" height="29" />
                            </td>
                            <td background="../Img/tab_21.gif"></td>
                            <td width="14">
                                <img src="../Img/tab_22.gif" width="14" height="29" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
