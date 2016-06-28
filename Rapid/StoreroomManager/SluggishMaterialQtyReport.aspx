<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SluggishMaterialQtyReport.aspx.cs"
    Inherits="Rapid.StoreroomManager.SluggishMaterialQtyReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>呆滞原材料报表</title>
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
            left: 540px;
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
        margin-top: 20px; margin-bottom: 20px;">
        呆滞原材料报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="position: relative; float: left; margin-bottom: 10px">
                &nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="原材料编号："></asp:Label>
                <asp:TextBox ID="txtMaterialNumber" runat="server" Style="margin-right: 10px;"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="供应商编号："></asp:Label>
                <asp:TextBox ID="txtSupplierNumber" runat="server"></asp:TextBox>
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
                                <input type="checkbox" name="columList" value="tdOperar_原材料编号" checked="checked" />
                                原材料编号
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_供应商编号" checked="checked" />
                                供应商编号
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_四个月" checked="checked" />
                                四个月
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_一年" checked="checked" />
                                一年
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_一年以上" checked="checked" />
                                一年以上
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
        <table class="border" cellpadding="1" cellspacing="1" id="mainTable">
            <thead>
                <tr>
                    <td class="tdOperar_原材料编号">
                        原材料编号
                    </td>
                    <td class="tdOperar_供应商编号">
                        供应商编号
                    </td>
                    <td class="tdOperar_四个月">
                        四个月
                    </td>
                    <td class="tdOperar_一年">
                        一年
                    </td>
                    <td class="tdOperar_一年以上">
                        一年以上
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td class="tdOperar_原材料编号">
                                <%#Eval("原材料编号")%>
                            </td>
                            <td class="tdOperar_供应商编号">
                                <%#Eval("供应商编号")%>
                            </td>
                            <td class="tdOperar_四个月">
                                <%#Eval("四个月")%>
                            </td>
                            <td class="tdOperar_一年">
                                <%#Eval("一年")%>
                            </td>
                            <td class="tdOperar_一年以上">
                                <%#Eval("一年以上")%>
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
