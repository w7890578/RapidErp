<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductPlanList.aspx.cs"
    Inherits="Rapid.ProduceManager.ProductPlanList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>开工单列表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
        }

        body {
            font: 14px Verdana, Arial, Helvetica, sans-serif;
        }

        ul {
            list-style: none;
        }

        #tab {
            margin: 15px auto;
            width: 100%;
        }

            #tab ul {
                overflow: hidden;
                zoom: 1;
            }

            #tab li {
                float: left;
                margin-right: 8px;
                width: 200px;
                height: 30px;
                line-height: 30px;
                border: 1px solid green;
                border-bottom: 0;
                cursor: pointer;
                text-align: center;
                border-top-left-radius: 5px;
                border-top-right-radius: 5px;
            }

                #tab li.on {
                    background: green;
                    color: White;
                    font-size: 16px;
                    font-weight: bold;
                }

        #content {
            border-top: 4px solid green;
            background-color: #f3ffe3;
        }

            #content div {
            }

                #content div.show {
                    display: block;
                }
    </style>

    <script type="text/javascript">

        //        function window.confirm(str) {
        //            execScript("n = (msgbox('" + str + "',vbYesNo, '提示')=vbYes)", "vbscript");
        //            return (n);
        //        }

        function DeletePlan(planNumber) {
            if (confirm("确认删除开工单？开工单删除后将不能被恢复")) {
                $.ajax({
                    type: "Post",
                    url: "ProductPlanList.aspx",
                    data: { "PlanNumber": planNumber, "IsDelete": "true" },
                    success: function (res) {
                        if (res == "ok") {
                            alert("删除成功");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("删除失败！<br/>原因：" + res);
                        }
                    }
                });
            }
        }
        $(function () {
            $("#btnQD").click(function () {
                $("#divChooseType").hide();
            });

            $("#menus-tab li").click(function () {
                var title = $(this).html();
                var plannumber = $("#hPlannumber").val();
                if (plannumber == "" || plannumber == null) {

                    alert("请选择开单号！");
                    return;
                }
                $(this).addClass("on").siblings().removeClass("on");
                if (title == "裁线信息表") {
                    $("#kgd").hide();
                    $("#cxxx").show().html("<iframe style='position: relative; background-color: transparent;' width='100%' height='2000' frameborder='0' src='CuttingLineInfoList.aspx?PlanNumber=" + plannumber + "&date=" + new Date() + "'></iframe>");
                }
                else {
                    $("#kgd").show();
                    $("#cxxx").hide();
                }

            });
        })
    </script>

    <script type="text/javascript">
        //排序字段
        var sortname = "创建时间";
        //排序规则
        var sortdirection = "desc";
        //当前页
        var pageindex = 1;
        //总页数
        var pageCount = 0;
        //总行数
        var totalRecords = 0;
        //一页显示行数
        var pageSize = 0;
        //查询sql语句
        var querySql = "";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            var type = $("#slType").val();
            var plannumber = $("#txtPlanNumber").val();
            var creator = $("#slCreator").val();
            if (type != "") {
                condition += " and 开工单类型='" + type + "'";
            }
            if (plannumber != "") {

                condition += " and 开工单号 like '%" + plannumber + "%'";
            }
            if (creator != "") {
                condition += " and 制单人='" + creator + "'";
            }

            return condition;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_ProductPlan  ";
            querySql = querySql + " " + GetQueryCondition();
            $("#saveInfo").val(querySql + "");
            return true;
        }

        //获取数据
        function GetData(pageIndex, sortName, sortDirection) {
            //获取一页显示行数
            pageSize = $("#txtPageSize").val();
            if (pageSize == "0") {
                pageSize = 10;
                $("#txtPageSize").val("1");
            }
            if (pageSize == "" || isNaN(pageSize)) {
                alert("请正确输入每页显示条数");
                return;
            }
            querySql = " select * from V_ProductPlan  ";
            querySql = querySql + " " + GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "ProductPlanList.aspx?time=" + new Date(),
                data: { pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql },
                beforeSend: function () { $("#progressBar").show(); },
                success: function (result) {
                    //清空内容
                    $(".tablesorter tbody").html("");
                    //如果有数据就追加
                    if (result != "") {
                        var tempArray = result.split("^");
                        //总页数
                        pageCount = tempArray[0];
                        //追加html
                        $(".tablesorter tbody").append(tempArray[1]);
                        $(".tablesorter tbody tr:odd").addClass("odd");
                        $(".tablesorter tbody tr").click(function () {
                            //                            $(this).find("input[type='checkbox']").each(function() {
                            //                                this.checked = !this.checked; //整个反选
                            //                            });
                            $("#hPlannumber").val($(this).find("td:eq(2)").html());
                            $("#lblPlannumber").html($(this).find("td:eq(2)").html());

                            $(this).find("td").css("background-color", "yellow");
                            $(this).siblings().find("td").css("background-color", "white");
                        });
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='20' align='center'>  查无数据 </td> </tr>";
                            $(".tablesorter tbody").append(tempStr);
                            //分页清空
                            $("#pageing").html("");
                        }
                    }

                    //loading隐藏
                    $("#progressBar").hide();
                    $(".tablesorter thead tr td input[type='checkbox']").attr("checked", false);

                }
            });
        }
        //分页点击
        function aClick(index) {
            if (index == "第一页") {
                pageindex = 1;
            }
            else if (index == "上一页") {
                if (pageindex != 1) {
                    pageindex = parseInt(pageindex) - 1;
                }
            }
            else if (index == "下一页") {
                if (pageindex != pageCount) {
                    pageindex = parseInt(pageindex) + 1;
                }
            }
            else if (index == "最后一页") {
                pageindex = pageCount;
            }
            else {
                pageindex = index;
            }
            pageSize = $("#txtPageSize").val();
            if (pageSize == "0") {
                pageSize = 10;
                $("#txtPageSize").val("1");
            }
            if (pageSize == "" || isNaN(pageSize)) {
                alert("请正确输入每页显示条数");
                return;
            }

            //如果当前请求页大于总页数
            var tempPageCount = parseInt(totalRecords) % parseInt(pageSize);
            if (tempPageCount > 0) {
                tempPageCount = (parseInt(totalRecords) / parseInt(pageSize)) + 1;
            }
            else {
                tempPageCount = (parseInt(totalRecords) / parseInt(pageSize));
            }
            if (pageindex > tempPageCount) {
                pageindex = 1;
            }
            GetData(pageindex, sortname, sortdirection);
        }

        $(document).ready(function () {
            //查询
            $("#btnSearch").click(function () {
                GetData(1, sortname, sortdirection);
            });

            //删除
            $("#btnDelete").click(function () {
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要删除的行！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定删除选中的数据?")) {
                    //通用删除
                    DeleteData("", ConvertsContent(checkResult), "btnSearch");
                }
            });
            //审核
            $("#btnCheck").click(function () {
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要审核的行！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定审核选中的数据?")) {
                    //通用删除
                    Check("", ConvertsContent(checkResult), "btnSearch");
                }
            });

            //确认领料
            $("#btnConfirmCollar").click(function () {

                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要领料的行！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                var type = "bz";
                if (confirm("是否为生产出库领料？如果是包装出库领料请选择否【取消】")) {
                    type = "sc";
                }
                //这是获取的值
                if (confirm("确定选中的数据领料?")) {

                    $.get("ProductPlanList.aspx?sq=" + new Date(), { IsConfirmCollar: type, PlanNumbers: checkResult, time: new Date() }, function (result) {
                        if (result == "1") {
                            alert("生成原材料出库单成功");
                        }
                        else {
                            alert(result);
                        }
                        return;
                    });
                }


            });

            //绑定排序事件和样式
            function tablesorter(className) {
                var obj = $("." + className + " thead tr th");
                obj.find("img").hide();
                //排序事件
                obj.click(function () {
                    obj.find("img").hide();

                    sortname = $(this).attr("sortname");
                    if (sortdirection == "asc") {
                        $(this).find("img").attr("src", "../Img/asc.gif").show();
                        sortdirection = "desc";
                    }
                    else {
                        $(this).find("img").attr("src", "../Img/desc.gif").show();
                        sortdirection = "asc";
                    }
                    var index = $(".current").html();
                    if (index == null) {
                        index = 1;
                    }
                    GetData(1, sortname, sortdirection);
                });
            }

            //全选/反选
            $(".tablesorter thead tr td input").click(function () {
                $("input[name='subBox']").each(function () {
                    this.checked = !this.checked; //整个反选
                });
            });

            $("#btnAdd").click(function () {
                OpenDialog("", "btnSearch", "320", "550");
            });

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
            BindSelect("PlanNumber", "slPlanNumber");
            BindSelect("Creator", "slCreator");

        });
    </script>

    <style type="text/css">
        .table_td {
            width: 236px;
        }

        .style1 {
            width: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="position: absolute; top: 40%; left: 40%; width: 300px; height: 80px;
        border: 1px solid green; background-color: White; padding: 10px; display: none;"
        id="divChooseType">
        请选择领料类型：<select id="slLLType">
            <option value="生产领料">生产领料</option>
            <option value="包装领料">包装领料</option>
        </select>
        <input type="button" value="确定" id="btnQD" />
    </div>
    <div class="outerDiv" style="width: 2100px; margin-right: 20px;">
        <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-top: 1px;
            padding-top: 4px;">
            &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
            <span>&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;开工单列表</span>
        </div>
        <div>
            <input type="hidden" id="Hidden1" runat="server" />
            <div>
                <input type="hidden" id="saveInfo" runat="server" />
                <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                    <img src="../Img/loading.gif" alt="loading" />
                </div>
                <table class="pg_table" style="background-color: #F3FFE3">
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label3" runat="server" Text="开工单号："></asp:Label>
                            <label id="lblPlannumber" />
                        </td>
                        <td colspan="6" style="text-align: left">
                            <div style="vertical-align: middle">
                                <div style="float: left; width: 150;">
                                </div>
                            </div>
                            <div>
                                <div style="float: left; width: 65px;">
                                </div>
                                <%--<div style="float: left; width: 65px;">
                                                        <input type="button" value="增加" id="btnAdd" class="button" />
                                                    </div>
                                                    <div style="float: left; width: 65px;">
                                                        <input type="button" value="删除" id="btnDelete" class="button" />
                                                    </div>--%>
                                <%--<div style="float: left; width: 65px;">
                                                        <asp:Button ID="Button1" runat="server" Text="导出Excel" OnClick="Button1_Click" OnClientClick="return ImpExecl()"
                                                            CssClass="button" />
                                                    </div>--%>
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <div id="tab">
                                <input type="hidden" id="hPlannumber" style="display: none" />
                                <ul id="menus-tab">
                                    <li class="on">开工单列表</li>
                                    <li>裁线信息表</li>
                                </ul>
                                <div id="content">
                                    <div class="show" id="kgd">
                                        <div style="width: 100%; height: 30px;">
                                        </div>
                                        <div style="width: 100%; height: 30px; margin-bottom: 10px">
                                            &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;
                                            <asp:Label ID="Label1" runat="server" Text="开工单类型："></asp:Label>
                                            <select id="slType" style="margin-right: 40px">
                                                <option value="">- - - - - 请 选 择 - - - - -</option>
                                                <option value="小组">小组</option>
                                                <option value="工序">工序</option>
                                            </select>
                                            <asp:Label ID="ii" runat="server" Text="开工单号："></asp:Label>
                                            <%--<select id="slPlanNumber" style="margin-right: 40px">
                                                <option value="">- - - - - 请 选 择 - - - - -</option>
                                            </select>--%>
                                            <input type="text" id="txtPlanNumber" />
                                            <asp:Label ID="Label2" runat="server" Text="制单人："></asp:Label>
                                            <select id="slCreator" style="margin-right: 40px">
                                                <option value="">- - - - - 请 选 择 - - - - -</option>
                                            </select>
                                            每页显示条数：
                                            <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="15" />
                                            &nbsp;&nbsp;
                                            <input type="button" value="查询" id="btnSearch" class="button" />
                                            <input type="button" value="审核" id="btnCheck" class="button" runat="server" style="display: none;" />
                                            <input type="button" value="确认领料" id="btnConfirmCollar" class="button" />
                                        </div>
                                        <div>
                                            <div id="main1">
                                                <table class="tablesorter" cellpadding="1" cellspacing="1">
                                                    <thead>
                                                        <tr>
                                                            <td>
                                                                <label style="width: 100%; display: block; cursor: pointer;">
                                                                    <input type="checkbox" />全选/反选</label>
                                                            </td>
                                                            <th sortname='序号' style="display: none;">
                                                                序号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                                                    id="Img14" /></span>
                                                            </th>
                                                            <th sortname='开工单号'>
                                                                开工单号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                                                    id="Img10" /></span>
                                                            </th>
                                                            <th sortname='开工单类型'>
                                                                开工单类型 <span>
                                                                    <img src="../Img/bg.gif" id="sortImg" /></span>
                                                            </th>
                                                            <th sortname='创建时间'>
                                                                创建时间 <span>
                                                                    <img src="../Img/bg.gif" id="Img8" /></span>
                                                            </th>
                                                            <th sortname='制单人'>
                                                                制单人 <span>
                                                                    <img src="../Img/bg.gif" id="Img1" /></span>
                                                            </th>
                                                            <%--  <th sortname='审核人'>
                                                                审核人<span><img src="../Img/bg.gif" id="Img2" /></span>
                                                            </th>
                                                            <th sortname='审核时间'>
                                                                审核时间<span><img src="../Img/bg.gif" id="Img15" /></span>
                                                            </th>--%>
                                                            <th sortname='人数'>
                                                                人数 <span>
                                                                    <img src="../Img/bg.gif" id="Img3" /></span>
                                                            </th>
                                                            <th sortname='套数'>
                                                                套数<span><img src="../Img/bg.gif" id="Img16" /></span>
                                                            </th>
                                                            <th sortname='额定总工时'>
                                                                额定总工时<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                            </th>
                                                            <th sortname='实际总工时'>
                                                                实际总工时 <span>
                                                                    <img src="../Img/bg.gif" id="Img5" /></span>
                                                            </th>
                                                            <th sortname='目标完成工时'>
                                                                目标完成工时 <span>
                                                                    <img src="../Img/bg.gif" id="Img7" /></span>
                                                            </th>
                                                            <th sortname='实际完成工时''>
                                                                实际完成工时 <span>
                                                                    <img src="../Img/bg.gif" id="Img9" /></span>
                                                            </th>
                                                            <th sortname='计划开始时间'>
                                                                计划开始时间 <span>
                                                                    <img src="../Img/bg.gif" id="Img6" /></span>
                                                            </th>
                                                            <th sortname='计划结束时间''>
                                                                计划结束时间 <span>
                                                                    <img src="../Img/bg.gif" id="Img11" /></span>
                                                            </th>
                                                            <th sortname='实际开始时间'>
                                                                实际开始时间 <span>
                                                                    <img src="../Img/bg.gif" id="Img12" /></span>
                                                            </th>
                                                            <th sortname='实际结束时间''>
                                                                实际结束时间 <span>
                                                                    <img src="../Img/bg.gif" id="Img13" /></span>
                                                            </th>
                                                            <td>
                                                                是否确认领料
                                                            </td>
                                                            <td>
                                                                备注
                                                            </td>
                                                            <td>
                                                                操作
                                                            </td>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td colspan="21" align="center">
                                                                暂无数据
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                      <tfoot>
                                            <tr>
                                                <td colspan="18" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px; padding-right: 10px;">
                                                    <div id="pageing" class="pages clearfix">
                                                    </div>
                                                </td>
                                            </tr>
                                        </tfoot>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="cxxx" style="display: none;">
                                        裁线信息表
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
