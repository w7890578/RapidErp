<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="finishedProductsAttheEndCountReport.aspx.cs" Inherits="Rapid.ProduceManager.finishedProductsAttheEndCountReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>月末在制已完成产品统计表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

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
            left: 240px;
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
        月末在制已完成产品统计表</div>
    <div style="width: 100%; text-align: center; font: 96px; font-size: small; font-weight: bold;
        margin-top: 20px">
        电缆制造合计：
        <asp:Label ID="lblDL" runat="server" Text=""></asp:Label></div>
    <div style="width: 100%; text-align: center; font: 96px; font-size: small; font-weight: bold;">
        包装生产合计：<asp:Label ID="lblBZ" runat="server" Text=""></asp:Label></div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="margin-top: 20px;">
            <div style="position: relative; float: left; margin-bottom: 10px">
                &nbsp;&nbsp;
                <asp:Label ID="txtd" runat="server" CssClass="input required" Text="日期："></asp:Label>
                <asp:TextBox ID="txtDate" runat="server" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                <input type="button" value="打印" id="btnPrint" style="margin-left: 10px" />
                <input type="hidden" id="saveInfo" runat="server" />
                <asp:Button ID="btnExcel" runat="server" Text="导出Excel" Style="margin-left: 10px;
                    margin-right: 500px;" OnClick="btnExcel_Click" />
                <a href="###" style="color: Blue">原材料清单</a>
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_产成品编号" checked="checked" />
                                产成品编号
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
                                <input type="checkbox" name="columList" value="tdOperar_版本" checked="checked" />
                                版本
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_在制品数量" checked="checked" />
                                在制品数量
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_类型" checked="checked" />
                                类型
                            </label>
                        </li>
                    </ul>
                    <div>
                        &nbsp;<br />
                        <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                            type="button" value=" 取 消 " id="btnExit" /></div>
                </div>
            </div>
        </div>
        <br />
        <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
            <thead>
                <tr>
                    <td class="tdOperar_产成品编号" style="width: 100px;">
                        产成品编号
                    </td>
                    <td class="tdOperar_客户产成品编号" style="width: 200px;">
                        客户产成品编号
                    </td>
                    <td class="tdOperar_版本" style="width: 200px;">
                        版本
                    </td>
                    <td class="tdOperar_在制品数量" style="width: 200px;">
                        在制品数量
                    </td>
                    <td class="tdOperar_ 类型" style="width: 200px;">
                        类型
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
                            <td class="tdOperar_客户产成品编号">
                                <%#Eval("客户产成品编号")%>
                            </td>
                            <td class="tdOperar_版本">
                                <%#Eval("版本")%>
                            </td>
                            <td class="tdOperar_在制品数量">
                                <a href="UnfinishedProductsCountDetailReport.aspx?ProductNumber=<%#Eval("产成品编号")%>&Version= <%#Eval("版本")%> &Type=<%#Eval("类型")%>">
                                    <%#Eval("在制品数量")%></a>
                            </td>
                            <td class="tdOperar_ 类型">
                                <%#Eval("类型")%>
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