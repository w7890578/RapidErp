<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerformanceReviewLogList.aspx.cs"
    Inherits="Rapid.ProduceManager.PerformanceReviewLogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>员工绩效上报列表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        a:link, a:visited
        {
            text-decoration: none; /*超链接无下划线*/
        }
    </style>

    <script type="text/javascript">
        //排序字段
        var sortname = "姓名";
        //排序规则
        var sortdirection = "asc";
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
            var condition = " where 考核类型!='库房及其它' ";
            var year = $("#Year").val();
            var Year = $("#Year").find("option:selected").text();

            var month = $("#Month").val();
            var Month = $("#Month").find("option:selected").text();

            var performanceReviewItem = $("#txtPerformanceReviewItem").val();

            //            var name = $("#Name").val();
            //            var Name = $("#Name").find("option:selected").text();

            var rowNumber = $("#RowNumber").val();
            var RowNumber = $("#RowNumber").find("option:selected").text();
            var name = $("#txtName").val();
            if (year != "" && year != null) {
                condition += " and 年度='" + Year + "' ";
            }
            if (month != "" && month != null) {
                condition += " and 月份='" + Month + "' ";
            }
            if (performanceReviewItem != "") {
                condition += " and 考核项目 like '%" + performanceReviewItem + "%' ";
            }
            //            if (name != "" && name != null) {
            //                condition += " and 姓名='" + Name + "' ";
            //            }
            if (rowNumber != "" && rowNumber != null) {
                condition += " and 序号='" + RowNumber + "' ";
            }
            if (name != "") {
                condition += " and 姓名='" + name + "' ";
            }
            return condition;
        }
        //获取数据
        function GetData(pageIndex, sortName, sortDirection) {

            //获取一页显示行数
            pageSize = $("#txtPageSize").val();
            if (pageSize == "" || isNaN(pageSize)) {
                alert("请正确输入每页显示条数");
                return;
            }
            querySql = " select * from V_PerformanceReviewLogList ";
            querySql = querySql + " " + GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "PerformanceReviewLogList.aspx",
                data: { time: new Date(), pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql },
                beforeSend: function() { $("#progressBar").show(); },
                success: function(result) {
                    //清空内容
                    $(".tablesorter tbody").html("");
                    //如果有数据就追加
                    if (result != "") {
                        var tempArray = result.split("^");
                        //总页数
                        pageCount = tempArray[0];
                        //追加html
                        $(".tablesorter tbody").append(tempArray[1]);
                        //$(".tablesorter tbody tr:odd").addClass("odd");
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        $(".tablesorter tbody tr").click(function() {
                            $(this).find("input[type='checkbox']").each(function() {
                                this.checked = !this.checked; //整个反选
                            });
                        });
                        $(".tablesorter tbody tr").hover(function() {
                            $(this).find("td").css("background-color", "#EAFCD5");
                        }, function() {
                            $(this).find("td").css("background-color", "white");
                        });

                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='13' align='center'>  查无数据 </td> </tr>";
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
        $(document).ready(function() {

            //查询
            $("#btnSearch").click(function() {

                GetData(1, sortname, sortdirection);
            });

            //删除
            $("#btnDelete").click(function() {
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function() {
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
                if (confirm("确定删除选择的数据?")) {

                    //通用删除
                    DeleteData("../ProduceManager/PerformanceReviewLogList.aspx", ConvertsContent(checkResult), "btnSearch");
                }
            });
            //绑定排序事件和样式
            function tablesorter(className) {
                // $("." + className + " tbody tr:odd").addClass("odd");
                var obj = $("." + className + " thead tr th");
                obj.find("img").hide();
                // obj.addClass("header"); 
                //排序事件
                obj.click(function() {
                    obj.find("img").hide();

                    sortname = $(this).attr("sortname");
                    //obj.removeClass("headerSortUp");
                    //obj.removeClass("headerSortDown");
                    if (sortdirection == "asc") {
                        // $(this).addClass("headerSortUp");
                        $(this).find("img").attr("src", "../Img/asc.gif").show();
                        sortdirection = "desc";
                    }
                    else {
                        //$(this).addClass("headerSortDown");
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
            $(".tablesorter thead tr td input").click(function() {
                $("input[name='subBox']").each(function() {
                    // $(this).attr("checked", !$(this).attr("checked")); //全选、全不选
                    this.checked = !this.checked; //整个反选
                });
            });

            $("#btnAdd").click(function() {
                OpenDialog("AddPerformanceReviewLog.aspx", "btnSearch", "160", "400");
            });
            $("#btnAddUser").click(function() {
                OpenDialog("AddPerformanceReviewLogForUser.aspx", "btnSearch", "160", "400");
            });
            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
            //绑定年度
            BindSelect("PerformanceReviewLogYear", "Year");
            //绑定月份
            BindSelect("PerformanceReviewLogMonth", "Month");
            //绑定考核项目
            //            BindSelect("PerformanceReviewLogPerformanceReviewItem", "PerformanceReviewItem");
            //姓名
            //            BindSelect("PerformanceReviewLogName", "Name");
            //绑定序号
            BindSelect("PerformanceReviewLogRowNumber", "RowNumber");

        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 1450px;">
        <table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <!--背景top-->
            <tr>
                <td height="30">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="15" height="30">
                                <img src="../Img/tab_03.gif" width="15" height="30" />
                            </td>
                            <td width="1101" background="../Img/tab_05.gif">
                                <img src="../Img/311.gif" width="16" height="16" visible />
                                <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;员工绩效上报列表</span>
                            </td>
                            <td width="281" background="../Img/tab_05.gif">
                                <table border="0" align="right" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="60">
                                        </td>
                                        <td width="52">
                                        </td>
                                        <td width="60">
                                        </td>
                                        <td width="60">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="14">
                                <img src="../Img/tab_07.gif" width="14" height="30" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--主内容-->
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="9" background="../Img/tab_12.gif">
                                &nbsp;
                            </td>
                            <td bgcolor="#f3ffe3" style="padding-top: 5px;">
                                <div>
                                    <input type="hidden" id="saveInfo" runat="server" />
                                    <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                                        <img src="../Img/loading.gif" alt="loading" />
                                    </div>
                                    <table class="pg_table">
                                        <tr>
                                            <td colspan="14">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pg_talbe_head">
                                                年&nbsp;&nbsp;度：
                                                <select id="Year">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>
                                            </td>
                                            <td class="pg_talbe_content">
                                                月&nbsp;&nbsp;份：
                                                <select id="Month">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>
                                            </td>
                                            <td class="pg_talbe_head">
                                                考核项目：
                                                <input type="text" id="txtPerformanceReviewItem" />
                                            </td>
                                            <td class="pg_talbe_content">
                                                姓&nbsp;&nbsp;名：
                                                <input type="text" id="txtName" />
                                            </td>
                                            <td class="pg_talbe_head">
                                            </td>
                                            <td class="pg_talbe_content">
                                                <%--<select id="PerformanceReviewItem">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                            </td>
                                            <td class="pg_talbe_head">
                                            </td>
                                            <td class="pg_talbe_content">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="14">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pg_talbe_head">
                                            </td>
                                            <td class="pg_talbe_content">
                                            </td>
                                            <td class="pg_talbe_head">
                                                &nbsp;
                                            </td>
                                            <td class="pg_talbe_content">
                                                &nbsp;
                                            </td>
                                            <td colspan="9" style="text-align: left">
                                                <div style="vertical-align: middle">
                                                    <div style="float: left; width: 150;">
                                                        每页显示条数：
                                                        <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                            onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                            maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="200" />
                                                        &nbsp;&nbsp;
                                                    </div>
                                                </div>
                                                <div>
                                                    <div style="float: left; width: 65px;">
                                                        <input type="button" value="查询" id="btnSearch" class="button" />
                                                    </div>
                                                    <div style="float: left; width: 65px;" id="divAdd" runat="server">
                                                        <input type="button" value="上报" id="btnAdd" class="button" />
                                                    </div>
                                                    <div style="float: left; width: 65px;" id="div1" runat="server">
                                                        <input type="button" value="追加上报" id="btnAddUser" class="button" />
                                                    </div>
                                                    <div style="float: left; width: 65px;" id="divDelete" runat="server">
                                                        <input type="button" value="删除" id="btnDelete" class="button" />
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="15">
                                                <div>
                                                    <table class="tablesorter" cellpadding="1" cellspacing="1" width="1220px">
                                                        <thead>
                                                            <tr>
                                                                <td>
                                                                    <label style="width: 100%; display: block; cursor: pointer;">
                                                                        <input type="checkbox" />全选/反选</label>
                                                                </td>
                                                                <td>
                                                                    年度
                                                                </td>
                                                                <td>
                                                                    月份
                                                                </td>
                                                                <%-- <th sortname='年度'>
                                                                    年度<span><img src="../Img/bg.gif" id="Img3" /></span>
                                                                </th>
                                                                <th sortname='月份'>
                                                                    月份<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                                </th>--%>
                                                                <td>
                                                                    考核项目
                                                                </td>
                                                                <td>
                                                                    姓名
                                                                </td>
                                                                <td>
                                                                    序号
                                                                </td>
                                                                <td>
                                                                    满分
                                                                </td>
                                                                <td>
                                                                    扣分
                                                                </td>
                                                                <td>
                                                                    得分
                                                                </td>
                                                                <%-- <th sortname='序号'>
                                                                    序号<span><img src="../Img/bg.gif" id="Img1" /></span>
                                                                </th>
                                                                <th sortname='满分'>
                                                                    满分<span><img src="../Img/bg.gif" id="Img2" /></span>
                                                                </th>
                                                                <th sortname='扣分'>
                                                                    扣分<span><img src="../Img/bg.gif" id="Img5" /></span>
                                                                </th>
                                                                <th sortname='得分'>
                                                                    得分<span><img src="../Img/bg.gif" id="Img6" /></span>
                                                                </th>--%>
                                                                <td style="width: 250px;">
                                                                    描述
                                                                </td>
                                                                <td>
                                                                    统计方式
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
                                                                <td colspan="13" align="center">
                                                                    暂无数据
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                        <tfoot>
                                                            <tr>
                                                                <td colspan="13" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
                                                                    padding-right: 10px;">
                                                                    <div id="pageing" class="pages clearfix">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </tfoot>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td width="9" background="../Img/tab_16.gif">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <!--背景down-->
            <tr>
                <td height="29">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="15" height="29">
                                <img src="../Img/tab_20.gif" width="15" height="29" />
                            </td>
                            <td background="../Img/tab_21.gif">
                                <div class="pages clearfix">
                                </div>
                            </td>
                            <td width="14">
                                <img src="../Img/tab_22.gif" width="14" height="29" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
