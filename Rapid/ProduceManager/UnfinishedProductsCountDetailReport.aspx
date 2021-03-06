﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnfinishedProductsCountDetailReport.aspx.cs" Inherits="Rapid.ProduceManager.UnfinishedProductsCountDetailReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>在制末完成产品详细报表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            //查询sql语句

            $("#btnBack").click(function() {
            window.location.href = "UnfinishedProductsAttheEndCountReport.aspx";
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
                //$("#printTalbe").removeClass("border").addClass("tablesorter");
            });
        });
        function Delete(plannumber) {
            if (confirm("确认删除？")) {
                $.ajax({
                    type: "Get",
                    url: "ProductPlanDetailList.aspx",
                    data: { time: new Date(), PlanNumber: plannumber },
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
    </style>
    <div id="divHiddeMGX">
      首页  >  生产管理  > <a href="UnfinishedProductsAttheEndCountReport.aspx";>月末在制末完成产品统计表</a>  >  在制末完成产品详细报表
    </div>
    <div style="font-size: 28px; font-weight: bold; text-align: center; margin-top: 30px;">
            在制末完成产品详细报表</div>
    <div>
        <input type="hidden" id="hdnumber" runat="server" />
        <div id="divHeader">
            <div style="position: relative; float: left; margin-bottom: 30px; top: 10px; left: 0px;">
                &nbsp;&nbsp;
              <%--  <asp:Label ID="Label1" runat="server" Text="开工单号："></asp:Label>
                <asp:TextBox ID="txtPlanNumber" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" />--%>
                <input type="button" value="返回" id="btnBack" />
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
                                <input type="checkbox" name="columList" value="tdOperar_产品类型" checked="checked" />
                                产品类型
                            </label>
                        </li>
                        <li>
                            <label>
                                <input type="checkbox" name="columList" value="tdOperar_产成品编号" checked="checked" />
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
                                <input type="checkbox" name="columList" value="tdOperar_在制数量" checked="checked" />
                                在制数量
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
    </div>

    <table class="border" cellpadding="1" cellspacing="1" id ="printTalbe" style="margin-top:20px;">
        <thead>
            <tr>
                <td class="tdOperar_开工单号">
                    开工单号
                </td>
                <td class="tdOperar_销售订单号" >
                    销售订单号
                </td>
                <td class="tdOperar_产品类型" >
                    产品类型
                </td>
                <td class="tdOperar_产成品编号">
                    产成品编号
                </td>
                <td class="tdOperar_版本" >
                    版本
                </td>
                <td class="tdOperar_客户产成品编号" >
                    客户产成品编号
                </td>
                <td class="tdOperar_在制数量">
                    在制数量
                </td>
                <td class="tdOperar_单套工时" >
                    单套工时
                </td>
                <td class="tdOperar_合计工时" >
                    合计工时
                </td>
                <td class="tdOperar_交期时间">
                    交期时间
                </td>
                <td class="tdOperar_备注" style="width: 200px">
                    备注
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
                        <td class="tdOperar_销售订单号">
                            <%#Eval("销售订单号")%>
                        </td>
                        <td class="tdOperar_产品类型">
                            <%#Eval("产品类型")%>
                        </td>
                        <td class="tdOperar_产成品编号">
                            <%#Eval("产成品编号")%>
                        </td>
                        <td class="tdOperar_版本">
                            <%#Eval("版本")%>
                        </td>
                        <td class="tdOperar_客户产成品编号">
                            <%#Eval("客户产成品编号")%>
                        </td>
                        <td class="tdOperar_在制数量">
                            <%#Eval("在制数量")%>
                        </td>
                        <td class="tdOperar_单套工时">
                            <%# Eval("单套工时").ToString ().Equals ("0.00")?"":Eval("单套工时")%>
                        </td>
                        <td class="tdOperar_合计工时">
                            <%#Eval("合计工时").ToString ().Equals ("0.00")?"":Eval("合计工时")%>
                        </td>
                        <td class="tdOperar_交期时间">
                            <%#Eval("交期时间")%>
                        </td>
                        <td class="tdOperar_备注">
                            <%#Eval("备注")%>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    
    </form>
</body>
</html>