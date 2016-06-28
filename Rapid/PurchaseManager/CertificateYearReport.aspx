<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificateYearReport.aspx.cs"
    Inherits="Rapid.PurchaseManager.CertificateYearReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购年度报表</title>
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
    </style>
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        采购年度报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="position: relative; float: left; margin-bottom: 10px">
                &nbsp;&nbsp;
                <asp:Label ID="txtd" runat="server" CssClass="input required" Text="年份："></asp:Label>
                <asp:DropDownList ID="drpYear" runat="server" CssClass="required " AutoPostBack="True" OnSelectedIndexChanged="drpYear_SelectedIndexChanged">
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
                <asp:Label ID="Label2" runat="server" Text="供应商名称：" Style="margin-left: 20px;"></asp:Label>
                <asp:TextBox ID="txtSupplierName" runat="server" Style="margin-right: 20px;"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
            <thead>
                <tr>
                    <td>
                        供应商名称
                    </td>
                    <td>
                        年份
                    </td>
                    <td>
                        1月
                    </td>
                    <td>
                        2月
                    </td>
                    <td>
                        3月
                    </td>
                    <td>
                        4月
                    </td>
                    <td>
                        5月
                    </td>
                    <td>
                        6月
                    </td>
                    <td>
                        7月
                    </td>
                    <td>
                        8月
                    </td>
                    <td>
                        9月
                    </td>
                    <td>
                        10月
                    </td>
                    <td>
                        11月
                    </td>
                    <td>
                        12月
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("供应商名称")%>
                            </td>
                            <td>
                                <%#Eval("年度")%>
                            </td>
                            <td>
                                <%#Eval("1月")%>
                            </td>
                            <td>
                                <%#Eval("2月")%>
                            </td>
                            <td>
                                <%#Eval("3月")%>
                            </td>
                            <td>
                                <%#Eval("4月")%>
                            </td>
                            <td>
                                <%#Eval("5月")%>
                            </td>
                            <td>
                                <%#Eval("6月")%>
                            </td>
                            <td>
                                <%#Eval("7月")%>
                            </td>
                            <td>
                                <%#Eval("8月")%>
                            </td>
                            <td>
                                <%#Eval("9月")%>
                            </td>
                            <td>
                                <%#Eval("10月")%>
                            </td>
                            <td>
                                <%#Eval("11月")%>
                            </td>
                            <td>
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
