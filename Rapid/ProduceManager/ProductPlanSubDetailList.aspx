<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductPlanSubDetailList.aspx.cs"
    Inherits="Rapid.ProduceManager.ProductPlanSubDetailList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=type%>开工单分表明细</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        var team = encodeURI(getQueryString("Team"));
        //        var type=<%=type  %>;
        function edit(plannumber, team, ordersnumber, productnumber, version, rownumber, type) {
            var type = "<%=type  %>";
            type = encodeURI(type);
            OpenDialog("../ProduceManager/EditProductPlanSubDetail.aspx?PlanNumber=" + plannumber + "&Team=" + encodeURI(team) + "&OrdersNumber=" + ordersnumber + "&ProductNumber=" + productnumber + "&Version=" + version + "&RowNumber=" + rownumber + "&type=" + type + "&time=" + new Date(), "btnSearch", "550", "600");
        }

        function Delete(plannumber, team, ordersnumber, productnumber, version, rownumber) {
            if (confirm("确认删除？")) {
                $.ajax({
                    type: "Get",
                    url: "ProductPlanSubDetailList.aspx",
                    data: { time: new Date, PlanNumber: plannumber, Team: team, OrdersNumber: ordersnumber, ProductNumber: productnumber, Version: version, RowNumber: rownumber },
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
        $("#btnSearch").click();
        $(function() {
            //查询sql语句

            $("#btnBack").click(function() {
                window.location.href = "ProductPlanSubList.aspx?PlanNumber=<%=plannumber%>";

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
                $("#printTalbe tr td").each(function() {
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
                $("#divHiddeMGX").hide();
                $("#printTalbe").removeClass("tablesorter").addClass("border");
                newwin = window.open("", "newwin", "height=900,width=750,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
                newwin.document.body.innerHTML = document.getElementById("form1").innerHTML;
                newwin.document.getElementById("divHeader").style.display = 'none';
                newwin.document.getElementById("choosePrintClounm").style.display = 'none';


                newwin.window.print();
                newwin.window.close();
                $("#choosePrintClounm").hide();
                $("#printTalbe tr td").each(function() {
                    $(this).show();
                })
                $("#divHiddeMGX").show();
                //                $("#printTalbe").removeClass("border").addClass("tablesorter");
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
        .printDiv
        {
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
        .et4
        {
            color: #000000;
            font-size: 11.0pt;
            font-family: 宋体;
            font-weight: 400;
            font-style: normal;
            text-decoration: none;
            text-align: general;
            vertical-align: middle;
        }
    </style>
    <div id="divHiddeMGX">
        首页 > 生产管理 ><a href="ProductPlanList.aspx?PlanNumber=<%=plannumber%>">开工单列表</a> ><a
            href="ProductPlanSubList.aspx?PlanNumber=<%=plannumber%>"><%=type%>
            开工单分表明细列表 </a>><%=type%>开工单分表明细详细列表</div>
    <input type="hidden" id="hdnumber" runat="server" />
    <div id="divHeader">
        <div style="position: relative; float: left; margin-bottom: 10px; top: 10px; left: 0px;">
            &nbsp;&nbsp;&nbsp;&nbsp; 班组：
            <asp:DropDownList ID="drpTeam" runat="server" Style="margin-right: 10px" OnSelectedIndexChanged="drpTeam_SelectedIndexChanged"
                AutoPostBack="True">
                <asp:ListItem Value="">- - - - - 请 选 择 - - - - -</asp:ListItem>
            </asp:DropDownList>
            <input type="button" value="返回" id="btnBack" />
            <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" Style="display: none;" />
            <input type="button" value="打印" id="btnPrint" style="margin-left: 10px" />
            <div id="choosePrintClounm">
                <div>
                    请选择要打印的列</div>
                <ul>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_开工单号" checked="checked" />
                            开工单号
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_销售订单号" checked="checked" />
                            销售订单号
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="ttdOperar_产成品编号" checked="checked" />
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
                            <input type="checkbox" name="columList" value="tdOperar_套数" checked="checked" />
                            套数
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_单套工时" checked="checked" />
                            单套工时
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_合计工时" checked="checked" />
                            合计工时
                        </label>
                    </li>
                    <li style="display: <%=worksn1%>;">
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_工序1" style="display: <%=worksn1%>;" />
                            工序1
                        </label>
                    </li>
                    <li style="display: <%=worksn2%>;">
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_工序2" style="display: <%=worksn2%>;" />
                            工序2
                        </label>
                    </li>
                    <li style="display: <%=worksn3%>;">
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_工序3" style="display: <%=worksn3%>;" />
                            工序3
                        </label>
                    </li>
                    <li style="display: <%=worksn4%>;">
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_工序4" style="display: <%=worksn1%>;" />
                            工序4
                        </label>
                    </li>
                    <li style="display: <%=nextteam%>;">
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_交接班组" style="display: <%=nextteam%>;" />
                            交接班组
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_完成数量" checked="checked" />
                            完成数量
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_交线情况" " " checked="checked" />
                            交线情况
                        </label>
                    </li>
                    <li>
                        <label>
                            <input type="checkbox" name="columList" value="tdOperar_交期时间" checked="checked" />
                            交期时间
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
    <br />
    <br />
    <table width="100%">
        <tr>
            <td>
                <table style="font-size: 14px; line-height: 26px; height: 200px">
                    <tr>
                        <td style="width: 100px;">
                            <img src="../Img/my/tading.png" />
                        </td>
                        <td colspan="7" style="font-size: 28px; font-weight: bold; width: 700px;">
                            &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;
                            <%=type  %>
                            开 工 单（分表）明细
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td style="width: 100px">
                            班组：
                        </td>
                        <td style="width: 100px">
                            <asp:Label ID="lblTeam" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 100px">
                            额定总工时：
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblRatedTotalManhour" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 100px">
                            目标完成工时：
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblTargetFinishManhour" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            小组人数：
                        </td>
                        <td>
                            <asp:Label ID="lblPersonQty" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            实际总工时：
                        </td>
                        <td>
                            <asp:Label ID="lblFactTotalManhour" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            实际完成工时：
                        </td>
                        <td>
                            <asp:Label ID="lblFactFinishManhour" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            单位：
                        </td>
                        <td>
                            小时
                        </td>
                        <td>
                            开始时间：
                        </td>
                        <td>
                            <asp:Label ID="lblFactStartTime" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            结束时间：
                        </td>
                        <td>
                            <asp:Label ID="lblFactEndTime" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
                <table class="border" cellpadding="1" cellspacing="1" id="printTalbe">
                    <thead>
                        <tr>
                            <td class="tdOperar_开工单号" style="width: 150px;">
                                开工单号
                            </td>
                            <%-- <td class="tdOperar_班组">
                                班组
                            </td>--%>
                            <td class="tdOperar_销售订单号" style="width: 150px;">
                                销售订单号
                            </td>
                            <td class="tdOperar_产成品编号" style="width: 269px;">
                                产成品编号
                            </td>
                            <td class="tdOperar_版本" style="width: 80px;">
                                版本
                            </td>
                            <td class="tdOperar_行号" style="width: 80px;">
                                行号
                            </td>
                            <td class="tdOperar_客户产成品编号" style="width: 200px;">
                                客户产成品编号
                            </td>
                            <td class="tdOperar_套数" style="width: 80px;">
                                套数
                            </td>
                            <td class="tdOperar_单套工时" style="width: 80px;">
                                单套工时
                            </td>
                            <td class="tdOperar_合计工时" style="width: 80px;">
                                合计工时
                            </td>
                            <%if (worksn1=="inline"){ %>
                            <td class="tdOperar_工序1" style=" width: 233px;">
                                工序1(工时)
                            </td>  
                            <%} %>
                            <%if (worksn2=="inline")
                              { %>
                            <td class="tdOperar_工序2" style=" width: 233px;">
                                工序2(工时)
                            </td>
                            <%} %>
                            <%if (worksn3=="inline")
                              { %>
                            <td class="tdOperar_工序3" style="  width: 233px;">
                                工序3(工时)
                            </td>
                            <%} %>
                            <%if (worksn4=="inline")
                              { %>
                             <td class="tdOperar_工序4" style=" width: 233px;">
                                工序4(工时)
                            </td>
                            <%} %>
                           <%if (nextteam=="inline"){ %>
                             <td class="tdOperar_交接班组" style="  width: 80px;">
                                交接班组
                            </td>
                            <%} %>
                           <%if(finishqty=="inline"){ %>
                            <td class="tdOperar_完成数量" style=" width: 80px;">
                                完成数量
                            </td>
                            <%} %>
                            <%if (takeline=="inline"){ %>
                            <td class="tdOperar_交线情况" style="  width: 80px;">
                                交线情况
                            </td>
                            <%} %>
                            
                            <td class="tdOperar_交期时间" style="width: 413px;">
                                交期时间
                            </td>
                            <td class="tdOperar_备注" style="width: 200px;">
                                备注
                            </td>
                            <td class="tdOperar" style="width: 100px;">
                                操作
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpList">
                            <ItemTemplate>
                                <tr>
                                    <td class="tdOperar_开工单号">
                                        <%#Eval("开工单号")%>
                                    </td>
                                    <%--  <td class="tdOperar_班组">
                                        <%#Eval("班组")%>
                                    </td>--%>
                                    <td class="tdOperar_销售订单号">
                                        <%#Eval("销售订单号")%>
                                    </td>
                                    <td class="tdOperar_产成品编号">
                                        <%#Eval("产成品编号")%>
                                    </td>
                                    <td class="tdOperar_版本">
                                        <%#Eval("版本")%>
                                    </td>
                                    <td class="tdOperar_行号">
                                        <%#Eval("行号")%>
                                    </td>
                                    <td class="tdOperar_客户产成品编号">
                                        <%#Eval("客户产成品编号")%>
                                    </td>
                                    <td class="tdOperar_套数">
                                        <%#Eval("套数")%>
                                    </td>
                                    <td class="tdOperar_单套工时">
                                        <%#Eval("单套工时新")%>
                                    </td>
                                    <td class="tdOperar_合计工时">
                                        <%#Eval("合计工时")%>
                                    </td>
                                    
                                    <%if (worksn1 == "inline")
                              { %>
                                     <td class="tdOperar_工序1"  >
                                        <%#Eval("工序1")%><%#Eval("工时1")%>
                                    </td>
                                    <%} %>
                                      <%if (worksn2 == "inline")
                              { %>
                                     <td class="tdOperar_工序2"  >
                                        <%#Eval("工序2")%><%#Eval("工时2")%>
                                    </td>
                                    <%} %>

                                   <%if (worksn3=="inline")
                                     { %>
                                    <td class="tdOperar_工序3"  >
                                        <%#Eval("工序3")%>
                                        <%#Eval("工时3")%>
                                    </td>
                                    <%} %>
                                    <%if (worksn4=="inline")
                                      { %>
                                    <td class="tdOperar_工序4"  >
                                        <%#Eval("工序4")%>
                                        <%#Eval("工时4")%>
                                    </td>
                                    <%} %>
                                    
                                    <%if (nextteam=="inline")
                                      { %>
                                    <td class="tdOperar_交接班组"  >
                                        <%#Eval("交接班组")%>
                                    </td>
                                    <%} %>

                                     <%if (finishqty=="inline")
                                      { %>
                                     <td class="tdOperar_完成数量"  >
                                        <%#Eval("完成数量")%>
                                    </td>
                                    <%} %>
                                   
                                    <%if (takeline=="inline")
                                      { %>
                                    <td class="tdOperar_交线情况"  >
                                        <%#Eval("交线情况")%>
                                    </td>
                                    <%} %>
                                    
                                    <td class="tdOperar_交期时间">
                                        <%#Eval("交期时间")%>
                                    </td>
                                    <td class="tdOperar_备注">
                                        <%#Eval("备注")%>
                                    </td>
                                    <td class="tdOperar">
                                        <span style="display: <%#Eval("开工单号").ToString ().Equals ("合计")?"none":"inline"%>;">
                                            <a href="###" onclick="edit('<%#Eval("开工单号") %>','<%#Eval("班组")%>', '<%#Eval("销售订单号")%>','<%#Eval("产成品编号")%>','<%#Eval("版本")%>','<%#Eval("行号")%>')">
                                                编辑</a> </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                <%--   <div style="font-size: 14px; text-align: right; margin-right: 10px; margin-top: 10px;">
                    审核人：<asp:Label ID="lblAuditor" runat="server" Text="Label"></asp:Label></div>
                <div style="font-size: 14px; text-align: left; margin-left: 10px">
                    开始时间取第一道工序开始时间，结束时间取最后一道工序结束时间</div>--%>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
