<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BOMInfoList.aspx.cs" Inherits="Rapid.ProduceManager.BOMInfoList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BOM信息列表</title>
    <style type="text/css">
        body, div, ul, li
        {
            margin: 0 auto;
            padding: 0;
        }
        body
        {
            font: 12px "宋体";
            text-align: center;
        }
        a:link
        {
            color: #00F;
            text-decoration: none;
        }
        a:visited
        {
            color: #00F;
            text-decoration: none;
        }
        a:hover
        {
            color: #c00;
            text-decoration: underline;
        }
        ul
        {
            list-style: none;
        }
        .main
        {
            clear: both;
            padding: 8px;
            text-align: center;
        }
        #tabs1
        {
            text-align: left;
            width: 98%;
            padding-right: 20px;
            margin-right: 20px;
        }
        .menu1box
        {
            position: relative;
            overflow: hidden;
            height: 28px;
            width: 100%;
            text-align: left;
            margin-left: 20px;
        }
        #menu1
        {
            position: absolute;
            top: 0;
            left: 0;
            z-index: 1;
        }
        #menu1 li
        {
            float: left;
            display: block;
            cursor: pointer;
            width: 72px;
            text-align: center;
            line-height: 25px;
            height: 25px;
            margin-right: 8px;
        }
        #menu1 li.hover
        {
            background: #fff; /* border-left: 1px solid #333;
            border-top: 1px solid #333;
            border-right: 1px solid #333;*/
            border: 1px solid #333;
            border-radius: 12px;
        }
        .main1box
        {
            clear: both;
            margin-top: 1px;
            height: 100%;
            width: 100%;
        }
        #main1 ul
        {
            display: none;
        }
        #main1 ul.block
        {
            display: block;
        }
    </style>
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
        var ProductNumber = getQueryString("Id");
        //获取产成品版本
        var Version = getQueryString("version");
        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 产成品编号='" + ProductNumber + "' and 版本='" + Version + "'";
            var materialNumber = $("#txtMaterialNumber").val(); //原材料编号
            var singleDose = $("#txtSingleDose").val(); //单机用量
            var description = $("#txtDescription").val(); //描述
            var customermaterialnumber = $("#txtCustomerMaterialNumber").val();//客户物料编号
            if (materialNumber != "") {
                condition += " and  (原材料编号 like '%" + materialNumber + "' or 原材料编号 like '" + materialNumber + "%' or 原材料编号 like '%" + materialNumber + "%' )";
            }
            if (singleDose != "") {
                condition += "and 单机用量='" + singleDose + "'";
            }
            if (description != "") {
                condition += "and 描述 like '%" + description + "%'";
            }
            if (customermaterialnumber != "") {
                condition += " and 客户物料编号 like '%" + customermaterialnumber + "%'";
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

            querySql = " select * from V_BOMInfoList_New_List ";
            querySql = querySql + " " + GetQueryCondition();
            $.ajax({
                type: "Get",
                url: "BOMInfoList.aspx",
                data: { time: new Date(), Id: ProductNumber, Version: Version, pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql },
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
                        $(".tablesorter tbody tr:odd").addClass("odd");
                        $(".tablesorter tbody tr").click(function() {
                            $(this).find("input[type='checkbox']").each(function() {
                                this.checked = !this.checked; //整个反选
                            });
                            //选项卡用到的  
                            $("#hdProduct").val($(this).find("td:eq(2)").html());
                            $("#lbProduct").html($(this).find("td:eq(2)").html());
                            $("#hdVersion").val($(this).find("td:eq(3)").html());
                            $("#lbVersion").html($(this).find("td:eq(3)").html());
                            $("#hdMaterial").val($(this).find("td:eq(4)").html());
                            $("#lbMaterial").html($(this).find("td:eq(4)").html());
                        });
                        $(".tablesorter tbody tr:even").hover(function() {
                            $(this).find("td").css("background-color", "#EAFCD5");
                        }, function() {
                            $(this).find("td").css("background-color", "white");
                        });
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='12' align='center'>  查无数据 </td> </tr>";
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
            $("#btnBOM").click(function() {
                $("#showOne").show();
                $("#showTwo").hide();
            });
            $("#btnCai").click(function() {
                var template1 = "<li><iframe id='temp' style='position: relative; background-color: transparent;' width='100%' height='500' frameborder='0' src='ProductCuttingLineInfoList.aspx?ProductNumber={0}&Version={1}&MaterialNumber={2}&date=" + new Date() + "'> </iframe></li>";
                var ProductNumber = $("#hdProduct").val();
                var Version = $("#hdVersion").val();
                var MaterialNumber = $("#hdMaterial").val();
                if (ProductNumber == "" || ProductNumber == null || Version == "" || Version == null || MaterialNumber == "" || MaterialNumber == null) {
                    alert("请选择bom！");
                    return;
                }
                $("#showOne").hide();
                $("#showTwo").show().html(template1.format(ProductNumber, Version, MaterialNumber));
            });


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
                    DeleteProductProperty("../ProduceManager/BOMInfoList.aspx", ProductNumber, Version, checkResult, "btnSearch");
                }
            });

            //绑定排序事件和样式
            var obj = $(".tablesorter thead tr th");
            obj.find("img").hide();
            //排序事件
            obj.click(function() {
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

            //全选/反选
            $(".tablesorter thead tr td input").click(function() {
                $("input[name='subBox']").each(function() {
                    // $(this).attr("checked", !$(this).attr("checked")); //全选、全不选
                    this.checked = !this.checked; //整个反选
                });
            });

            $("#btnAdd").click(function() {

                OpenDialog("AddOrEditBOMInfo.aspx?ProductNumber=" + ProductNumber + "&Version=" + Version, "btnSearch", "310", "700");
            });

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
            <input type="hidden" id="hdProduct" runat="server" />
            <input type="hidden" id="hdVersion" runat="server" />
            <input type="hidden" id="hdMaterial" runat="server" />
            <div style="margin-top: 15px; text-align: center; font-size: 14px;">
                产成品:<label id="lbProduct" style="color: Green;"></label>
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; 版本:<label id="lbVersion" style="color: Green;"></label>
                &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; 原材料:<label id="lbMaterial" style="color: Green;"></label>
            </div>
            <div style="float: left; margin-left: 20px; display: none;">
                <input type="button" id="btnBOM" value=" BOM信息 " />
                <input type="button" id="btnCai" value=" 裁线信息维护 " /></div>
            <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            </div>
            <div class="main1box">
                <div class="main" id="main1">
                    <div id="showOne">
                        <table class="pg_table">
                            <tr>
                                <td colspan="10">
                                    &nbsp;&nbsp;原材料编号：<input type="text" id="txtMaterialNumber" />
                                    &nbsp;&nbsp; 单机用量：<input type="text" id="txtSingleDose" />&nbsp;&nbsp; &nbsp;&nbsp;
                                    描述：<input type="text" id="txtDescription" />
                                    &nbsp;&nbsp; 客户物料编号：<input type="text" id="txtCustomerMaterialNumber" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10" style="text-align: left">
                                    <div style="float: left; width: 150; margin-left: 55%">
                                        每页显示条数：
                                        <input maxlength="3" type="text" onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                            onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                            style="width: 60px;" id="txtPageSize" value="10" />
                                        &nbsp;&nbsp;&nbsp;
                                    </div>
                                    <div>
                                        <div style="float: left; width: 65px;">
                                            <input type="button" value="查询" id="btnSearch" class="button" />
                                        </div>
                                        <div style="float: left; width: 65px; <%=(Session["User_Func"] as System.Collections.Generic.List<string>).Contains("L0113|Bom_Add")?"inline": "none"%>;"
                                            id="divAdd" runat="server">
                                            <input type="button" value="增加" id="btnAdd" class="button" />
                                        </div>
                                        <div style="float: left; width: 65px; <%=(Session["User_Func"] as System.Collections.Generic.List<string>).Contains("L0113|Bom_Delete")?"inline": "none"%>;"
                                            id="divDelete" runat="server">
                                            <input type="button" value="删除" id="btnDelete" class="button" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <div>
                                        <table class="tablesorter" cellpadding="1" cellspacing="1" width="1234px">
                                            <thead>
                                                <tr>
                                                    <td>
                                                        <label style="width: 100%; display: block; cursor: pointer;">
                                                            <input type="checkbox" />全选/反选</label>
                                                    </td>
                                                    <th sortname='序号' style="display: none;">
                                                        序号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                                    </th>
                                                    <td>
                                                        客户产成品编号(图纸号)
                                                    </td>
                                                    <th sortname='产成品编号'>
                                                        产成品编号<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                    </th>
                                                    <th sortname='版本'>
                                                        版本<span><img src="../Img/bg.gif" id="Img3" /></span>
                                                    </th>
                                                    <th sortname='原材料编号'>
                                                        原材料编号<span><img src="../Img/bg.gif" id="Img5" /></span>
                                                    </th>
                                                    <td>
                                                        客户物料编号
                                                    </td>
                                                    <td>
                                                        种类
                                                    </td>
                                                    <td>
                                                        描述
                                                    </td>
                                                    <th sortname='单机用量'>
                                                        单机用量<span><img src="../Img/bg.gif" id="Img1" /></span>
                                                    </th>
                                                    <td>
                                                        单位
                                                    </td>
                                                    <td>
                                                        备注
                                                    </td>
                                                    <td style="display: none;">
                                                        操作
                                                    </td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td colspan="12" align="center">
                                                        暂无数据
                                                    </td>
                                                </tr>
                                            </tbody>
                                            <tfoot>
                                                <tr>
                                                    <td colspan="12" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
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
                    <div id="showTwo">
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
