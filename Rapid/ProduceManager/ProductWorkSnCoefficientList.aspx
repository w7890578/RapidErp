﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductWorkSnCoefficientList.aspx.cs"
    Inherits="Rapid.ProduceManager.ProductWorkSnCoefficientList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品工序系数列表</title>
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
        var sortname = "产成品编号";
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
        //获取产成品编号
        var ProductNumber = getQueryString("ProductNumber");
        //获取产成品版本
        var Version = getQueryString("Version");
        //获取产成品工序编码
        var WorkSnNumber = getQueryString("WorkSnNumber");
        //获取查询条件
        function GetQueryCondition() {
            var condition = " where  产成品编号='" + ProductNumber + "' and 版本='" + Version + "' and 工序编码='" + WorkSnNumber + "'";
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
            querySql = " select * from V_ProductWorkSnCoefficientList ";
            querySql = querySql + " " + GetQueryCondition();
           
            $.ajax({
                type: "Get",
                url: "ProductWorkSnCoefficientList.aspx",
                data: { time: new Date(), ProductNumber: ProductNumber, Version: Version, WorkSnNumber: WorkSnNumber, pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql },
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
                        $(".tablesorter tbody tr:even").hover(function() {
                            $(this).find("td").css("background-color", "#EAFCD5");
                        }, function() {
                            $(this).find("td").css("background-color", "white");
                        });

                        if (tempArray[1] == "") {

                            //如果没有数据
                            var tempStr = " <tr> <td colspan='11' align='center'>  查无数据 </td> </tr>";
                            $(".tablesorter tbody").append(tempStr);
                            //alert(tempStr );
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
                    DeleteProductWorkSnCoefficient("../ProduceManager/ProductWorkSnCoefficientList.aspx", ProductNumber, Version, WorkSnNumber, ConvertsContent(checkResult), "btnSearch");
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

                OpenDialog("AddOrEditProductWorkSnCoefficient.aspx?ProductNumber=" + ProductNumber + "&Version=" + Version + "&WorkSnNumber=" + encodeURIComponent(WorkSnNumber), "btnSearch", "400", "520");

            });

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
        });
    </script>

</head>
<body style="background-color: #f3ffe3;">
    <form id="form1" runat="server">
    <div>
        <div>
            <input type="hidden" id="saveInfo" runat="server" />
            <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                <img src="../Img/loading.gif" alt="loading" />
            </div>
            <table class="pg_table">
                <tr>
                    <td colspan="10">
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
                    <td colspan="5" style="text-align: left">
                        <div style="float: left; width: 150; margin-left: 55%">
                            每页显示条数：
                            <input maxlength="3" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                type="text" style="width: 60px;" id="txtPageSize" value="10" />
                            &nbsp;&nbsp;&nbsp;
                        </div>
                        <div>
                            <div style="float: left; width: 65px;">
                                <input type="button" value="查询" id="btnSearch" class="button" />
                            </div>
                            <div style="float: left; width: 65px;" id="divAdd" runat="server">
                                <input type="button" value="增加" id="btnAdd" class="button" />
                            </div>
                            <div style="float: left; width: 65px;" id="divDelete" runat="server">
                                <input type="button" value="删除" id="btnDelete" class="button" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                        <div>
                            <table class="tablesorter" cellpadding="1" cellspacing="1" width="1220px">
                                <thead>
                                    <tr>
                                        <td>
                                            <label style="width: 100%; display: block; cursor: pointer;">
                                                <input type="checkbox" />全选/反选</label>
                                        </td>
                                        <th sortname='产成品编号'>
                                            产成品编号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                        </th>
                                        <th sortname='版本'>
                                            版本<span><img src="../Img/bg.gif" id="Img8" /></span>
                                        </th>
                                        <th sortname='工序编码' style="display: none;">
                                            工序编码<span><img src="../Img/bg.gif" id="Img1" /></span>
                                        </th>
                                        <td>
                                            工序名称
                                        </td>
                                        <th sortname='序号'>
                                            序号<span><img src="../Img/bg.gif" id="Img3" /></span>
                                        </th>
                                        <th sortname='最小值'>
                                            最小值<span><img src="../Img/bg.gif" id="Img2" /></span>
                                        </th>
                                        <th sortname='最大值'>
                                            最大值<span><img src="../Img/bg.gif" id="Img4" /></span>
                                        </th>
                                        <th sortname='系数'>
                                            系数<span><img src="../Img/bg.gif" id="Img5" /></span>
                                        </th>
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
                                        <td colspan="11" align="center">
                                            暂无数据
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colspan="11" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
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
    </div>
    </form>
</body>
</html>
