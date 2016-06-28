<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScarpWarehouseLogDetail.aspx.cs"
    Inherits="Rapid.StoreroomManager.ScarpWarehouseLogDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>废品出入库明细</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        function edit(warehousenumber, materialnumber) {
            OpenDialog("../StoreroomManager/AddOrEditScarpWarehouseLogDetail.aspx?WarehouseNumber=" + warehousenumber + "&MaterialNumber=" + materialnumber, "btnSearch", "260", "600");
        }
        function Delete(warehousenumber, materialnumber) {
            if (confirm("确定删除？")) {
                $.ajax({
                    type: "Get",
                    url: "ScarpWarehouseLogDetail.aspx",
                    data: { Time: new Date(), WarehouseNumber: warehousenumber, MaterialNumber: materialnumber },
                    success: function(result) {
                        if (result == "1") {
                            alert("删除成功");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("删除失败！原因：" + result);
                            return;
                        }
                    }

                })

            }
        }
        $(function() {
            $("#btnBack").click(function() {
                window.location.href = "ScarpWarehouseLogList.aspx";
            });
            //查询sql语句
            $("#btnAdd").click(function() {
                OpenDialog("../StoreroomManager/AddOrEditScarpWarehouseLogDetail.aspx?WarehouseNumber=<%=warehousenumber%>", "btnSearch", "260", "600");
            });

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
        });
        var querySql = "";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            return condition;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = "   select * from V_ScarpWarehouseLogDetail";
            querySql = querySql + " " + GetQueryCondition();
            $("#saveInfo").val(querySql + "");
            return true;
        }
        $("#btnSearch").click();
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
            top: 23px;
            left: 220px;
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
        废品<%=changeDirection%>明细<%=warehousenumber%></div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader" style="padding: 10px;">
            <div style="position: relative; float: left; margin-bottom: 10px">
                &nbsp;&nbsp;
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />
                <asp:Button ID="btnCheck" runat="server" Text="审核" OnClick="btnCheck_Click" />
                <input type="button" value="增加" id="btnAdd" style="margin-left: 10px" />
                <input type="button" value="打印" id="btnPrint" style="margin-left: 10px" />
                <input type="button" value="返回" id="btnBack" />
                <input type="hidden" id="saveInfo" runat="server" />
                <asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
                <%--  <span style="display: none;">
                    <asp:Button ID="btnExcel" runat="server" Text="导出Excel" Style="margin-left: 10px;"
                        OnClick="btnExcel_Click" OnClientClick="return ImpExecl()" /></span>--%>
                <div id="choosePrintClounm">
                    <div>
                        请选择要打印的列</div>
                    <ul>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_出入库编号" checked="checked" />
                                出入库编号
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_原材料编号" checked="checked" />
                                原材料编号
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_客户物料编号" checked="checked" />
                                客户物料编号
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_数量" checked="checked" />
                                数量
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_库存数量" checked="checked" />
                                库存数量
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_备注" checked="checked" />
                                备注
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
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td class="tdOperar_出入库编号">
                        出入库编号
                    </td>
                    <td class="tdOperar_原材料编号">
                        原材料编号
                    </td>
                    <td class="tdOperar_客户物料编号">
                        客户物料编号
                    </td>
                    <td class="tdOperar_数量">
                        数量
                    </td>
                    <td class="tdOperar_库存数量">
                        库存数量
                    </td>
                    <td class="tdOperar_备注">
                        备注
                    </td>
                    <td class="tdOperar">
                        操作
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td class="tdOperar_出入库编号">
                                <%#Eval("出入库编号")%>
                            </td>
                            <td class="tdOperar_原材料编号">
                                <%#Eval("原材料编号")%>
                            </td>
                            <td class="tdOperar_客户物料编号">
                                <%#Eval("客户物料编号")%>
                            </td>
                            <td class="tdOperar_数量">
                                <%#Eval("数量")%>
                            </td>
                            <td class="tdOperar_库存数量">
                                <%#Eval("库存数量")%>
                            </td>
                            <td class="tdOperar_备注">
                                <%#Eval("备注")%>
                            </td>
                            <td class="tdOperar">
                                <span style="display: <%=checkStatus %>;"><a href="###" onclick="edit('<%#Eval("出入库编号") %>','<%#Eval("原材料编号")%>')">
                                    编辑</a> <a href="###" onclick="Delete('<%#Eval("出入库编号") %>','<%#Eval("原材料编号")%>')">删除</a>
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
            <tfoot>
                <tr>
                    <td class="tdOperar_出入库编号">
                    </td>
                    <td class="tdOperar_原材料编号">
                        合计
                    </td>
                    <td class="tdOperar_客户物料编号">
                    </td>
                    <td class="tdOperar_数量">
                        <%=sumQty  %>
                    </td>
                    <td class="tdOperar_库存数量">
                    </td>
                    <td class="tdOperar_备注">
                    </td>
                    <td class="tdOperar">
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
    </form>
</body>
</html>
