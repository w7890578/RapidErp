<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MonthRejectsProductReport.aspx.cs"
    Inherits="Rapid.ProduceManager.MonthRejectsProductReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>月度不合格品报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            //查询sql语句


            $("#btnPrint").click(function() {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function() {
                $("#choosePrintClounm").hide();
            });
            $("#btnChoosePrintColum").click(function() {
                var chooseResult = "";
                var unChooseResult = "";
                var arrChk = $("input[name='columList']:checkbox");
                $(arrChk).each(function() {
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
                $("#mainTable tr td").each(function() {
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
                newwin = window.open("", "newwin", "height=900,width=750,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
                newwin.document.body.innerHTML = document.getElementById("form1").innerHTML;
                newwin.document.getElementById("divHeader").style.display = 'none';
                newwin.document.getElementById("choosePrintClounm").style.display = 'none';


                newwin.window.print();
                newwin.window.close();
                $("#choosePrintClounm").hide();
                $("#mainTable tr td").each(function() {
                    $(this).show();
                })
            });
        });
        var querySql = "";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            return condition;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = "   select * from V_ExaminationReport";
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
            left: 500px;
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
        margin-top: 20px;margin-bottom:20px;">
        月度不合格品报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="position: relative; float: left; margin-bottom: 10px">
                &nbsp;&nbsp;
                <asp:Label ID="txtd" runat="server" CssClass="input required" Text="年份：" ></asp:Label>
                <asp:DropDownList ID="drpYear" runat="server" CssClass="required " AutoPostBack="True" style="margin-right:10px;">
                    <asp:ListItem Text="- - - - - 请 选 择 - - - - -" Value=""></asp:ListItem>
                    <asp:ListItem Value="2014">2014</asp:ListItem>
                    <asp:ListItem Value="2015">2015</asp:ListItem>
                    <asp:ListItem Value="2016">2016</asp:ListItem>
                    <asp:ListItem Value="2017">2017</asp:ListItem>
                    <asp:ListItem Value="2018">2018</asp:ListItem>
                    <asp:ListItem Value="2019">2019</asp:ListItem>
                    <asp:ListItem Value="2020">2020</asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                     <asp:Label ID="Label1" runat="server" CssClass="input required" Text="班组：" ></asp:Label>
                <asp:TextBox ID="txtTeam" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                <input type="button" value="打印" id="btnPrint" style="margin-left: 10px" />
                <input type="hidden" id="saveInfo" runat="server" />
                <asp:Button ID="btnExcel" runat="server" Text="导出Excel" Style="margin-left: 10px;"
                    OnClick="btnExcel_Click1" />
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_班组" checked="checked" />
                                班组
                            </label>
                        </li>
                               <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_年度" checked="checked" />
                                年度
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_1月" checked="checked" />
                                1月
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_2月" checked="checked" />
                                2月
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_3月" checked="checked" />
                                3月
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_4月" checked="checked" />
                                4月
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_5月" checked="checked" />
                                5月
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_6月" checked="checked" />
                                6月
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_7月" checked="checked" />
                                7月
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_8月" checked="checked" />
                                8月
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_9月" checked="checked" />
                                9月</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_10月" checked="checked" />
                                10月</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_11月" checked="checked" />
                                11月</label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_12月" checked="checked" />
                                12月</label>
                        </li>
                    </ul>
                    <div>
                        &nbsp;<br />
                        <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                            type="button" value=" 取 消 " id="btnExit" /></div>
                </div>
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
            <thead>
                <tr>
                    <td class="tdOperar_班组">
                        班组
                    </td>
                    <td class="tdOperar_年度">
                        年度
                    </td>
                    <td class="tdOperar_1月">
                        1月
                    </td>
                    <td class="tdOperar_2月">
                        2月
                    </td>
                    <td class="tdOperar_3月">
                        3月
                    </td>
                    <td class="tdOperar_4月">
                        4月
                    </td>
                    <td class="tdOperar_5月">
                        5月
                    </td>
                    <td class="tdOperar_6月">
                        6月
                    </td>
                    <td class="tdOperar_7月">
                        7月
                    </td>
                    <td class="tdOperar_8月">
                        8月
                    </td>
                    <td class="tdOperar_9月">
                        9月
                    </td>
                    <td class="tdOperar_10月">
                        10月
                    </td>
                    <td class="tdOperar_11月">
                        11月
                    </td>
                    <td class="tdOperar_12月">
                        12月
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
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
