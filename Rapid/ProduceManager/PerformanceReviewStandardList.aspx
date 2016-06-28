<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerformanceReviewStandardList.aspx.cs"
    Inherits="Rapid.ProduceManager.PerformanceReviewStandardList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>考核标准维护列表</title>
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
        var sortname = "考核标准名称";
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
            var condition = " where 考核类型!='仓库及其他' ";
            var standardname = $("#txtStandardName").val();
            var performancereviewitem = $("#txtPerformanceReviewItem").val();
            if (standardname != "") {
                condition += " and 考核标准名称='" + standardname + "'";
            }
            if (performancereviewitem != "") {
                condition += " and 考核项目='" + performancereviewitem + "'";
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
            querySql = " select * from V_PerformanceReviewStandardList ";
            querySql = querySql + " " + GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "PerformanceReviewStandardList.aspx",
                data: { time: new Date(), pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql },
                beforeSend: function() { $("#progressBar").show(); },
                success: function(result) {
                    //清空内容
                    $(".border tbody").html("");
                    //如果有数据就追加
                    if (result != "") {
                        var tempArray = result.split("^");
                        //总页数
                        pageCount = tempArray[0];
                        //追加html
                        $(".border tbody").append(tempArray[1]);
                        //$(".border tbody tr:odd").addClass("odd");
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        $(".border tbody tr").click(function() {
                            $(this).find("input[type='checkbox']").each(function() {
                                this.checked = !this.checked; //整个反选
                            });
                        });
                        //                        $(".border tbody tr:even").hover(function() {
                        //                            $(this).find("td").css("background-color", "#EAFCD5");
                        //                        }, function() {
                        //                            $(this).find("td").css("background-color", "white");
                        //                        });

                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='9' align='center'>  查无数据 </td> </tr>";
                            $(".border tbody").append(tempStr);
                            //分页清空
                            $("#pageing").html("");
                        }
                    }
                    //loading隐藏
                    $("#progressBar").hide();
                    $(".border thead tr td input[type='checkbox']").attr("checked", false);
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
                    DeleteData("../ProduceManager/PerformanceReviewStandardList.aspx", ConvertsContent(checkResult), "btnSearch");
                }
            });
            //绑定排序事件和样式
            function border(className) {
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
            $(".border thead tr td input").click(function() {
                $("input[name='subBox']").each(function() {
                    // $(this).attr("checked", !$(this).attr("checked")); //全选、全不选
                    this.checked = !this.checked; //整个反选
                });
            });

            $("#btnAdd").click(function() {
                //window.location.href = "AddOrEditProduct.aspx";
                OpenDialog("AddOrEditPerformanceReviewStandard.aspx", "btnSearch", "320", "600");
            });

            //绑定
            border("border");
            //进入页面加载数据
            $("#btnSearch").click();

        });
    </script>

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
            padding: 2px;
            background-color: White;
        }
        .border tr th
        {
            padding: 2px;
            background-color: White;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top:10px;">
        <input type="hidden" id="saveInfo" runat="server" />
        <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            <img src="../Img/loading.gif" alt="loading" />
        </div>
         <span class="STYLE4" style="font-size:14px">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;员工考核标准</span>
        <table class="pg_table">
            <tr>
                <td colspan="14">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="pg_talbe_head">
                    考核标准名称：
                </td>
                <td class="pg_talbe_content">
                    <%--<select id="StandardName">
                        <option value="">- - - - - 请 选 择 - - - - -</option>
                    </select>--%>
                    <input id="txtStandardName" />
                </td>
                <td class="pg_talbe_head">
                    考核项目：
                </td>
                <td class="pg_talbe_content">
                    <%-- <select id="PerformanceReviewItem">
                        <option value="">- - - - - 请 选 择 - - - - -</option>
                    </select>--%>
                    <input id="txtPerformanceReviewItem" />
                </td>
                <td class="pg_talbe_head">
                </td>
                <td class="pg_talbe_content">
                </td>
                <td class="pg_talbe_head">
                </td>
                <td class="pg_talbe_content">
                </td>
                <td class="pg_talbe_head">
                </td>
                <td class="pg_talbe_content">
                </td>
                <td class="pg_talbe_head">
                </td>
                <td class="pg_talbe_content">
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
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td>
                </td>
                <td colspan="8" style="text-align: left">
                    <div style="vertical-align: middle">
                        <div style="float: left; width: 150;">
                            每页显示条数：
                            <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="100" />
                            &nbsp;&nbsp;
                        </div>
                    </div>
                    <div>
                        <div style="float: left; width: 65px;">
                            <input type="button" value="查询" id="btnSearch" />
                        </div>
                        <div style="float: left; width: 65px;" id="divAdd" runat="server">
                            <input type="button" value="增加" id="btnAdd"  />
                        </div>
                        <div style="float: left; width: 65px;" id="divDelete" runat="server">
                            <input type="button" value="删除" id="btnDelete"  />
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="14">
                    <div>
                        <table class="border" cellpadding="1" cellspacing="1" width="1220px">
                            <thead>
                                <tr>
                                    <td style="width: 130px;">
                                        <label style="width: 100%; display: block; cursor: pointer;">
                                            <input type="checkbox" />全选/反选</label>
                                    </td>
                                    <td style="width: 130px;">
                                        考核标准名称
                                    </td>
                                    <td style="width: 130px;">
                                        考核项目
                                    </td>
                                    <th sortname='序号' style="width: 40px;">
                                        序号<span><img src="../Img/bg.gif" id="Img1" /></span>
                                    </th>
                                    <th sortname='满分' style="width: 40px;">
                                        满分<span><img src="../Img/bg.gif" id="Img2" /></span>
                                    </th>
                                    <td>
                                        描述
                                    </td>
                                    <td style="width: 130px;">
                                        统计方式
                                    </td>
                                    <td>
                                        备注
                                    </td>
                                    <td>考核类型</td>
                                    <td style="width: 100px;">
                                        操作
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="9" align="center">
                                        暂无数据
                                    </td>
                                </tr>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="9" style="padding-top: 10px; padding-left: 10px; padding-right: 10px;">
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
    </form>
</body>
</html>
