<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeAttendance.aspx.cs"
    Inherits="Rapid.ProduceManager.EmployeeAttendance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title></title>
    <%--<link href="../Css/Main.css" rel="stylesheet" type="text/css" />--%>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function edit(number, rownumber, ProductNumber) {
            OpenDialog("AddOrEditTradingOrderDetail.aspx?OdersNumber=" + number + "&RowNumber=" + rownumber + "&ProductNumber=" + ProductNumber, "btnSearch", "500", "600");
        }
        function Delete(number, rownumber, productNumber) {
            if (confirm("确定删除？")) {
                var number = $("#hdnumber").val();
                $.ajax({
                    type: "Get",
                    url: "TradingOrderDetail.aspx",
                    data: { time: new Date(), OdersNumber: number, RowNumber: rownumber, ProductNumber: productNumber, id: number, IsDelete: "true" },
                    success: function(result) {
                        if (result == "1") {
                            alert("删除成功！");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("删除失败！原因：" + result);
                            return;
                        }
                    }
                });
            }
        }
        $(function() {

            $("#btnImp").click(function() {
                OpenDialog("ImpEmployeeAttendance.aspx", "btnSearch", "320", "500");
            });

            $("#btnAdd").click(function() {
                var odersNumber = $("#hdnumber").val();
                OpenDialog("../SellManager/AddOrEditTradingOrderDetail.aspx?OdersNumber=" + odersNumber, "btnSearch", "500", "600");
            });
            $("#btnPrint").click(function() {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function() {
                $("#choosePrintClounm").hide();
            });
            $("#btnBack").click(function() {
                window.location.href = "SaleOderList.aspx";
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
                $(".border tr td").each(function() {
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
                $(".border tr td").each(function() {
                    $(this).show();
                })
            });

            $("#btnBack").click(function() {
                window.location.href = "";
            });

        })

    </script>

</head>
<body>
    <form id="form2" runat="server">
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border1
        {
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border tr td, .border1 tr td
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
            top: 20px;
            left: 50px;
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
    &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
    <span>&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;考勤记录</span>
    <div class="printDiv" id="Div1">
        <table class="pg_table">
            <tr>
                <td>
                    <div>
                        <input type="hidden" id="Hidden1" runat="server" />
                        <div id="div2" style="margin-top: 10px;">
                            <div style="position: relative; float: left; margin-bottom: 10px; margin-left: 20px;">
                                <asp:Label ID="Label1" runat="server" Text="年度："></asp:Label>
                                <asp:DropDownList ID="drpYear" runat="server">
                                    <asp:ListItem Value="" Text="- - - - - 请 选 择 - - - - -"></asp:ListItem>
                                    <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
                                    <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
                                    <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
                                    <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
                                    <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
                                    <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
                                    <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div style="position: relative; float: left; margin-bottom: 10px; margin-left: 20px;">
                                <asp:Label ID="Label2" runat="server" Text="月份："></asp:Label>
                                <asp:DropDownList ID="drpMonth" runat="server">
                                    <asp:ListItem Value="" Text="- - - - - 请 选 择 - - - - -"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                    <asp:ListItem Value="8" Text="8"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                    <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                    <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                             <div style="position: relative; float: left; margin-bottom: 10px; margin-left: 20px;">
                                <asp:Label ID="Label3" runat="server" Text="姓名："></asp:Label>
                                 <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                            </div>
                            <div style="position: relative; float: left; margin-bottom: 10px; margin-left: 20px;
                                width: 40px;">
                                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" class="button" />
                            </div>
                            <div style="position: relative; float: left; margin-bottom: 10px; margin-left: 20px;
                                width: 40px;">
                                <asp:Button ID="btnDelete" runat="server" Text="删除" OnClick="btnDelete_Click" />
                            </div>
                            <div style="position: relative; float: left; margin-bottom: 10px; margin-left: 20px;">
                                <asp:Label ID="lbDelete" runat="server" Text="" Style="color: Red;"></asp:Label>
                            </div>
                            <div style="position: relative; float: left; margin-bottom: 10px; margin-left: 20px;
                                width: 40px;">
                                <input type="button" value="导入" id="btnImp" class="button" onclick="return btnImp_onclick()" />
                            </div>
                            <div style="position: relative; float: left; margin-bottom: 10px; margin-left: 20px;
                                width: 40px;">
                                <asp:Button ID="Button1" runat="server" Text="导出Excel" 
                                    onclick="Button1_Click" />
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="13">
                    <table class="border" cellpadding="1" cellspacing="1">
                        <thead>
                            <tr>
                                <td class="tdOperar_年度">
                                    年度
                                </td>
                                <td class="tdOperar_月份">
                                    月份
                                </td>
                                <td class="tdOperar_姓名" style="width: 100px;">
                                    姓名
                                </td>
                                <td class="tdOperar_入职日期">
                                    入职日期
                                </td>
                                <td class="tdOperar_工号">
                                    工号
                                </td>
                                <td class="tdOperar_应出勤天">
                                    应出勤天
                                </td>
                                <td class="tdOperar_累计已休年假天">
                                    累计已休年假天
                                </td>
                                <td class="tdOperar_当月休年假">
                                    当月休年假
                                </td>
                                <td class="tdOperar_当月休产假">
                                    当月休产假
                                </td>
                                <td class="tdOperar_存休天">
                                    存休天
                                </td>
                                <td class="tdOperar_工伤天">
                                    工伤天
                                </td>
                                <td class="tdOperar_病假天">
                                    病假天
                                </td>
                                <td class="tdOperar_丧假">
                                    丧假
                                </td>
                                <td class="tdOperar_平日事假天数">
                                    平日事假天数
                                </td>
                                <td class="tdOperar_迟到早退(分)">
                                    迟到早退(分)
                                </td>
                                <td class="tdOperar_平时加班(天)">
                                    平时加班(天)
                                </td>
                                <td class="tdOperar_请假与加班/年假冲抵(天)">
                                    请假与加班/年假冲抵(天)
                                </td>
                                <td class="tdOperar_实际出勤总计（天）">
                                    实际出勤总计（天）
                                </td>
                                <td class="tdOperar_周六日加班(天)">
                                    周六日加班(天)
                                </td>
                                <td class="tdOperar_六日加班总(天)">
                                    六日加班总(天)
                                </td>
                                <td class="tdOperar_截至2014年底年假剩余天数">
                                    截至上一年年底年假剩余天数
                                </td>
                                <td class="tdOperar_截至上月底年假剩余天数">
                                    截至上月底年假剩余天数
                                </td>
                                <td class="tdOperar_截至本月底年假剩余天数">
                                    截至本月底年假剩余天数
                                </td>
                                <td class="tdOperar_截至本月底存休剩余天数">
                                    截至本月底存休剩余天数
                                </td>
                                <td class="tdOperar_截至上月底存休剩余天数">
                                    截至上月底存休剩余天数
                                </td>
                                <td class="tdOperar_备注">
                                    备注
                                </td>
                                <td class="tdOperar_迟到次数">
                                    迟到次数
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater runat="server" ID="rpList">
                                <ItemTemplate>
                                    <tr>
                                        <td class="tdOperar_年度">
                                            <%#Eval("年度")%>
                                        </td>
                                        <td class="tdOperar_月份">
                                            <%#Eval("月份")%>
                                        </td>
                                        <td class="tdOperar_姓名">
                                            <%#Eval("姓名")%>
                                        </td>
                                        <td class="tdOperar_入职日期">
                                            <%#Eval("入职日期")%>
                                        </td>
                                        <td class="tdOperar_工号">
                                            <%#Eval("工号")%>
                                        </td>
                                        <td class="tdOperar_应出勤天">
                                            <%#Eval("应出勤天")%>
                                        </td>
                                        <td class="tdOperar_累计已休年假天">
                                            <%#Eval("累计已休年假天")%>
                                        </td>
                                        <td class="tdOperar_当月休年假">
                                            <%#Eval("当月休年假")%>
                                        </td>
                                        <td class="tdOperar_当月假产假">
                                            <%#Eval("当月休产假")%>
                                        </td>
                                        <td class="tdOperar_存休天">
                                            <%#Eval("存休天")%>
                                        </td>
                                        <td class="tdOperar_工伤天">
                                            <%#Eval("工伤天")%>
                                        </td>
                                        <td class="tdOperar_病假天">
                                            <%#Eval("病假天")%>
                                        </td>
                                        <td class="tdOperar_丧假">
                                            <%#Eval("丧假")%>
                                        </td>
                                        <td class="tdOperar_平日事假天数">
                                            <%#Eval("平日事假天数").ToString().IndexOf('.') == 0 ? "0" + Eval("平日事假天数").ToString() : Eval("平日事假天数").ToString()%>
                                        </td>
                                        <td class="tdOperar_迟到早退(分)">
                                            <%#Eval("迟到早退")%>
                                        </td>
                                        <td class="tdOperar_平时加班(天)">
                                            <%#Eval("平时加班").ToString().IndexOf('.') == 0 ? "0" + Eval("平时加班").ToString() : Eval("平时加班").ToString()%>
                                        </td>
                                        <td class="tdOperar_请假与加班/年假冲抵(天)">
                                            <%#Eval("请假与加班年假冲抵")%>
                                        </td>
                                        <td class="tdOperar_实际出勤总计（天）">
                                            <%#Eval("实际出勤总计")%>
                                        </td>
                                        <td class="tdOperar_周六日加班(天)">
                                            <%#Eval("周六日加班")%>
                                        </td>
                                        <td class="tdOperar_六日加班总(天)">
                                            <%#Eval("六日加班总")%>
                                        </td>
                                        <td class="tdOperar_截至2014年底年假剩余天数">
                                            <%#Eval("截至2014年底年假剩余天数")%>
                                        </td>
                                        <td class="tdOperar_截至上月底年假剩余天数">
                                            <%#Eval("截至上月底年假剩余天数")%>
                                        </td>
                                        <td class="tdOperar_截至本月底年假剩余天数">
                                            <%#Eval("截至本月底年假剩余天数")%>
                                        </td>
                                        <td class="tdOperar_截至本月底存休剩余天数">
                                            <%#Eval("截至本月底存休剩余天数")%>
                                        </td>
                                        <td class="tdOperar_截至上月底存休剩余天数">
                                            <%#Eval("截至上月底存休剩余天数")%>
                                        </td>
                                        <td class="tdOperar_备注">
                                            <%#Eval("备注")%>
                                        </td>
                                        <td class="tdOperar_迟到次数">
                                            <%#Eval("迟到次数")%>
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
