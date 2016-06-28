<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductStockPic.aspx.cs"
    Inherits="Rapid.StoreroomManager.ProductStockPic" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>成品库存报表</title>
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
            width: 500px;
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
        成品库存报表</div>
    <input type="hidden" id="hdnumber" runat="server" />
    <div id="divHeader" style="padding: 10px;">
        <div style="position: relative; float: left; margin-bottom: 10px">
            &nbsp;&nbsp;
            <asp:Label ID="Label1" runat="server" Text="年份："></asp:Label>
            <asp:DropDownList ID="drpYear" runat="server" Style="margin-right: 20px">
            </asp:DropDownList>
            <asp:Label ID="Label2" runat="server" Text="月份："></asp:Label>
            <asp:DropDownList ID="drpMonth" runat="server" Style="margin-right: 20px">
            </asp:DropDownList>
            <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="margin-right: 20px" />
            <asp:Button ID="btnEmp" runat="server" Text="导出Excel" OnClick="btnEmp_Click" />
        </div>
    </div>
    <br />
         <br />    
        <table class="border" cellpadding="1" cellspacing="1"  style="margin-left :30px;">
            <thead>
                <tr>
                    <td>
                        时段
                    </td>
                    <td>
                        库存数（条）
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#Eval("时段")%>
                            </td>
                            <td>
                                <%#Eval("库存总数")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    
    </form>
</body>
</html>
