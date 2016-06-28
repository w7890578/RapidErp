<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductProcessList.aspx.cs"
    Inherits="Rapid.SellManager.ProductProcessList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>生产进度查询</title>
    <!--通用基本样式-->
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

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        //排序字段
        var sortname = "销售订单号";
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
            var condition = " where 1=1 ";
            //            var odersNumber = $("#OdersNumber").val();
            //            var OdersNumber = $("#OdersNumber").find("option:selected").text();

            //            var productNumber = $("#ProductNumber").val();
            //            var ProductNumber = $("#ProductNumber").find("option:selected").text();

            //            var version = $("#Version").val();
            //            var Version = $("#Version").find("option:selected").text();

            //            var customerProductNumber = $("#CustomerProductNumber").val();
            //            var CustomerProductNumber = $("#CustomerProductNumber").find("option:selected").text();

            //            var customerName = $("#CustomerName").val();
            //            var CustomerName = $("#CustomerName").find("option:selected").text();

            //            var productDescription = $("#ProductDescription").val();
            //            var ProductDescription = $("#ProductDescription").find("option:selected").text();

            var odersNumber = $("#txtOdersNumber").val();
            var productNumber = $("#txtProductNumber").val();
            var customerProductNumber = $("#txtCustomerProductNumber").val();
            var customerName = $("#txtCustomerName").val();
            var productDescription = $("#txtProductDescription").val();

            var customerOrderNumber = $("#txtCustomerOrderNumber").val();

            if (odersNumber != "") {
                condition += " and t.销售订单号 like '%" + odersNumber + "%' ";
            }
            if (productNumber != "") {
                condition += " and t.产品编号 like '%" + productNumber + "%' ";
            }
            //            if (version != "") {
            //                condition += " and t.版本='" + Version + "' ";
            //            }
            if (customerProductNumber != "") {
                condition += " and pcp.CustomerProductNumber like '%" + customerProductNumber + "%' ";
            }
            if (customerName != "") {
                condition += " and c.CustomerName like '%" + customerName + "%' ";
            }
            if (productDescription != "") {
                condition += " and t.Description like '%" + productDescription + "%' ";
            }
            if (customerOrderNumber != "") {
                condition += " and t.CustomerOrderNumber like '%" + customerOrderNumber + "%'";
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
            querySql = GetQueryCondition();
            $.ajax({
                type: "Get",
                url: "ProductProcessList.aspx",
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
                            $(this).find("td").css("background-color", "yellow");
                        }, function() {
                            $(this).find("td").css("background-color", "white");
                        });

                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='15' align='center'>  查无数据 </td> </tr>";
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

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();

            //绑定销售订单号
            //            BindSelect("ProductProcessOdersNumber", "OdersNumber");
            //            //绑定产成品编号
            //            BindSelect("ProductProcessProductNumber", "ProductNumber");
            //            //绑定版本
            //            BindSelect("ProductProcessVersion", "Version");
            //            //绑定客户产品编号
            //            BindSelect("ProductProcessCustomerProductNumber", "CustomerProductNumber");
            //            //绑定客户名称
            //            BindSelect("ProductProcessCustomerName", "CustomerName");
            //            //绑定产品描述
            //            BindSelect("ProductProcessProductDescription", "ProductDescription");
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 1750px;">
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
                                <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;销售管理&nbsp;&nbsp;>&nbsp;&nbsp;生产进度查询列表</span>
                            </td>
                            <td width="281" background="../Img/tab_05.gif">
                                <table border="0" align="right" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="60">
                                            <%--  <table width="90%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="STYLE1">
                                                    <div align="center">
                                                        <img src="../Img/001.gif" width="14" height="14" /></div>
                                                </td>
                                                <td class="STYLE1">
                                                    <div align="center">
                                                        新增</div>
                                                </td>
                                            </tr>
                                        </table>--%>
                                        </td>
                                        <td width="52">
                                            <%--  <table width="88%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="STYLE1">
                                                    <div align="center">
                                                        <img src="../Img/083.gif" width="14" height="14" /></div>
                                                </td>
                                                <td class="STYLE1">
                                                    <div align="center">
                                                        删除</div>
                                                </td>
                                            </tr>
                                        </table>--%>
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
                                            <td class="pg_talbe_text">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;销售订单号：
                                                <%--<select id="OdersNumber">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                                <input type="text" id="txtOdersNumber" />
                                            </td>
                                            <td class="pg_talbe_text">
                                                产成品编号：
                                                <%--<select id="ProductNumber">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                                <input type="text" id="txtProductNumber" />
                                            </td>
                                            <td class="pg_talbe_text">
                                                客户产品编号：
                                                <%-- <select id="CustomerProductNumber">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                                <input type="text" id="txtCustomerProductNumber" />
                                            </td>
                                            <td class="pg_talbe_text">
                                                客&nbsp;户&nbsp;&nbsp;名&nbsp;称：
                                                <%--<select id="CustomerName">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                                <input type="text" id="txtCustomerName" />
                                            </td>
                                            <td class="pg_talbe_text" style="width: 100px;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pg_talbe_text">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 产&nbsp;品&nbsp;描&nbsp;述：
                                                <%-- <select id="ProductDescription">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                                <input type="text" id="txtProductDescription" />
                                            </td>
                                            <td class="pg_talbe_text">
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 客户采购订单号：
                                                <%-- <select id="ProductDescription">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                                <input type="text" id="txtCustomerOrderNumber" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="10">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="15" style="text-align: left;">
                                                <div style="padding-left: 600px;">
                                                    <div style="vertical-align: middle">
                                                        <div style="float: left; width: 150;">
                                                            每页显示条数：
                                                            <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                                onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                                maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="10" />
                                                            &nbsp;&nbsp;
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div style="float: left; width: 65px;">
                                                            <input type="button" value="查询" id="btnSearch" class="button" />
                                                        </div>
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
                                                                <th sortname='销售订单号'>
                                                                    销售订单号<span><img src="../Img/bg.gif" id="Img1" /></span>
                                                                </th>
                                                                <th sortname='客户采购订单号'>
                                                                    客户采购订单号<span><img src="../Img/bg.gif" id="Img11" /></span>
                                                                </th>
                                                                <th sortname='订单类型'>
                                                                    订单类型<span><img src="../Img/bg.gif" id="Img8" /></span>
                                                                </th>
                                                                <th sortname='客户产成品编号'>
                                                                    客户产成品编号<span><img src="../Img/bg.gif" id="Img12" /></span>
                                                                </th>
                                                                <th sortname='产品编号'>
                                                                    产品编号<span><img src="../Img/bg.gif" id="Img2" /></span>
                                                                </th>
                                                                <th sortname='版本'>
                                                                    版本<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                                                </th>
                                                                <th sortname='订单数量'>
                                                                    订单数量<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                                </th>
                                                                <th sortname='已交货数量'>
                                                                    已交货数量<span><img src="../Img/bg.gif" id="Img9" /></span>
                                                                </th>
                                                                <th sortname='未交货数量'>
                                                                    未交货数量<span><img src="../Img/bg.gif" id="Img10" /></span>
                                                                </th>
                                                                <th sortname='库存数量'>
                                                                    库存数量<span><img src="../Img/bg.gif" id="Img5" /></span>
                                                                </th>
                                                                <th sortname='在制品数量'>
                                                                    在制品数量<span><img src="../Img/bg.gif" id="Img6" /></span>
                                                                </th>
                                                                <th sortname='需要生产数量'>
                                                                    需要生产数量<span><img src="../Img/bg.gif" id="Img7" /></span>
                                                                </th>
                                                                <th sortname='交期'>
                                                                    交期<span><img src="../Img/bg.gif" id="Img3" /></span>
                                                                </th>
                                                                <td>
                                                                    行号
                                                                </td>
                                                                 <th sortname='产品描述' style ="width :100px;">
                                                                    产品描述<span><img src="../Img/bg.gif" id="Img13" /></span>
                                                                </th>
                                                                <td style="width: 100px;">
                                                                    客户名称
                                                                </td>
                                                                
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr>
                                                                <td colspan="15" align="center">
                                                                    暂无数据
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                        <tfoot>
                                                            <tr>
                                                                <td colspan="16" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
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
