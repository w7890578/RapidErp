﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="top.aspx.cs" Inherits="Rapid.Index.top" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        body {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
        }

        .STYLE1 {
            color: #43860c;
            font-size: 12px;
        }
    </style>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#btnHomePage").click(function () {
                window.parent['mainFrame'].frames["I1"].location.href = "Welcome.aspx";
            });

            $("#btnExit").click(function () {
                // window.parent.close();
                if (confirm("确定退出系统？")) {
                    $.get("AjaxSystemExit.aspx?sq=" + new Date(), { time: new Date() }, function (result) {
                        window.parent.location.href = "../Login/login.html";
                        //                        window.parent.opener = null;
                        //                        window.parent.open('', '_self');
                        //                        window.parent.close();
                    });
                }

            });
            $("#btnRefresh").click(function () {
                var parrentFrame = window.parent['mainFrame'];
                parrentFrame.frames["I1"].location.reload();
            });
            $("#btnEdit").click(function () {
                window.showModalDialog("UserEditPwd.aspx", "", "dialogWidth:350px;dialogHeight:150px;scroll:no;status:no");
            });
            $("#btnBack").click(function () {
                var parrentFrame = window.parent['mainFrame'];
                var url = parrentFrame.frames["I1"].location.href;
                var arry = url.split('/');
                url = arry[arry.length - 1];
                if (url == "Welcome.aspx") {
                    return false;
                }
                parrentFrame.frames["I1"].history.back();

                //javascript: history.go(-1);
            });
            //function ReportTask() {
            //    $.get("ReportTask.aspx", { "type": "MaterialDull" }, function (data) { })
            //}

            //window.setTimeout(ReportTask, 3000);
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" style="table-layout: fixed;">
                <tr>
                    <td height="9" style="line-height: 9px; background-image: url(images/main_04.gif)">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="97" height="9" background="images/main_01.gif">&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="47" background="images/main_09.gif">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="38" height="47" background="images/main_06.gif">&nbsp;
                                </td>
                                <td width="59">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td height="29" background="images/main_07.gif">&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="18" background="images/main_14.gif">
                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" style="table-layout: fixed;">
                                                    <tr>
                                                        <td style="width: 1px;">&nbsp;
                                                        </td>
                                                        <td>
                                                            <span class="STYLE1" runat="server" id="loginUser"></span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="155" background="images/main_08.gif">&nbsp;
                                </td>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td height="23" valign="bottom">
                                                <img src="images/main_12.gif" width="367" height="23" border="0" usemap="#Map" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="200" background="images/main_11.gif">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="11%" height="23">&nbsp;
                                            </td>
                                            <td width="89%" valign="bottom">
                                                <%--当前在线人数：<span><%=Application["online"] %> </span>&nbsp;&nbsp;--%><span class="STYLE1" id="showDate">日期：2008年7月22日 星期二</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="5" style="line-height: 5px; background-image: url(images/main_18.gif)">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="180" background="images/main_16.gif" style="line-height: 5px;">&nbsp;
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <map name="Map" id="Map">
                <area shape="rect" coords="3,1,49,22" href="###" id="btnHomePage" />
                <area shape="rect" coords="52,2,95,21" href="###" id="btnBack" />
                <area shape="rect" coords="102,2,144,21" href="javascript:history.go(1);" />
                <area shape="rect" coords="150,1,197,22" href="###" id="btnRefresh" />
                <area shape="rect" coords="210,2,304,20" href="###" id="btnEdit" title="个人密码修改" />
                <area shape="rect" coords="314,1,361,23" href="###" id="btnExit" />
            </map>

            <script type="text/javascript">
                function Refresh() {
                    var temp = new Date().toLocaleString() + ' 星期' + '日一二三四五六'.charAt(new Date().getDay());
                    document.getElementById("showDate").innerHTML = temp;
                    setTimeout("Refresh()", 1000);
                }
                var timer = setTimeout("Refresh()", 1000);
            </script>

        </div>
    </form>
</body>
</html>
