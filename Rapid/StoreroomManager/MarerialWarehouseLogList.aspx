﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarerialWarehouseLogList.aspx.cs"
    Inherits="Rapid.StoreroomManager.MarerialWarehouseLogList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>原材料出入库列表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        a:link, a:visited {
            text-decoration: none; /*超链接无下划线*/
        }
    </style>

    <script type="text/javascript">
        function Transfer(warehousenumber, ChangeDirection, Type) {
            if (Type == "损耗出库") {
                window.location.href = "MarerialLossLogDetailList.aspx?WarehouseNumber=" + warehousenumber;
            }
            else if (Type == "换号出库" || Type == "换号入库") {
                window.location.href = "Detail_ChangeNumberInStorage.aspx?warehouseNumber=" + warehousenumber;
            }
            else {
                window.open("ToolMaterialWarehouseLogDetail.aspx?WarehouseNumber=" + warehousenumber + "&Type=" + encodeURI(Type));
                //window.location.href = "ToolMaterialWarehouseLogDetail.aspx?WarehouseNumber=" + warehousenumber + "&Type=" + encodeURI(Type);
            }
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            var txt_Numbers = $.trim($("#txt_Numbers").val());
            if (txt_Numbers == "") {
                alert("请输入出入库编号！");
                return false;
            }
            querySql = "exec Report_MaterialWarehouseQty '" + txt_Numbers + "'";

            $("#txtSql").val(querySql + "");
            return true;
        }

        //排序字段
        var sortname = "制单时间";
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
            var condition = " where 1=1 and 出入库类型!='样品入库' and 出入库类型!='样品出库'";
            //            var warehouseNumber = $("#WarehouseNumber").val();
            //            var WarehouseNumber = $("#WarehouseNumber").find("option:selected").text();

            var changeDirection = $("#ChangeDirection").val();
            var ChangeDirection = $("#ChangeDirection").find("option:selected").text();

            var type = $("#Type").val();
            var Type = $("#Type").find("option:selected").text();

            var warehouseName = $("#WarehouseName").val();
            var WarehouseName = $("#WarehouseName").find("option:selected").text();

            var warehouseNumber = $("#txtWarehouseNumber").val();
            if (warehouseName != "" && warehouseName != null) {
                condition += " and 仓库名称='" + WarehouseName + "' ";
            }
            if (changeDirection != "" && changeDirection != null) {
                condition += " and 变动方向='" + ChangeDirection + "' ";
            }
            if (type != "" && type != null) {
                condition += " and 出入库类型='" + Type + "' ";
            }
            if (warehouseNumber != "") {
                condition += " and 出入库编号 like '%" + warehouseNumber + "%' ";
            }
            return condition;
        }

        function GetCondtionTwo() {
            var temp = "where 1=1 ";
            var materiNumber = $("#txtMateriNumber").val();
            var suppleMateriNumber = $("#txtSuppleMateriNumber").val();
            var description = $("#txtDescription").val();
            var customerMateriNumber = $("#txtCustomerMateriNumber").val();
            var kgnumber = $("#txtKGNumber").val();
            var xsNumber = $.trim($("#txtXSNumber").val());
            if (materiNumber != "") {
                temp += " and mwld.MaterialNumber like '%" + materiNumber + "%' ";
            }
            if (suppleMateriNumber != "") {
                temp += "and mwld.SupplierMaterialNumber like '%" + suppleMateriNumber + "%'";
            }
            if (description != "") {
                temp += "and mit.Description like '%" + description + "%'";
            }
            if (customerMateriNumber != "") {
                temp += "and mwld.CustomerMaterialNumber like '%" + customerMateriNumber + "%'";
            }
            if (kgnumber != "") {
                temp += "and mwld.DocumentNumber like '%" + kgnumber + "%'";
            }
            if (xsNumber != "") {
                temp += "and mwld.DocumentNumber like '%" + xsNumber + "%'";
            }
            return temp;
        }

        //获取数据
        function GetData(pageIndex, sortName, sortDirection) {

            //获取一页显示行数
            pageSize = $("#txtPageSize").val();
            if (pageSize == "" || isNaN(pageSize)) {
                alert("请正确输入每页显示条数");
                return;
            }
            querySql = " select * from  V_MarerialWarehouseLogList ";
            querySql = GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "MarerialWarehouseLogList.aspx?sq=" + new Date(),
                data: { time: new Date(), pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql, tempCondtion: GetCondtionTwo(), KGNumber: $("#txtKGNumber").val() },
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
                        //$(".tablesorter tbody tr:odd").addClass("odd");
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        $(".tablesorter tbody tr").click(function () {
                            $(this).find("input[type='checkbox']").each(function () {
                                this.checked = !this.checked; //整个反选
                            });
                        });
                        $(".tablesorter tbody tr").hover(function () {
                            $(this).find("td").css("background-color", "#EAFCD5");
                        }, function () {
                            $(this).find("td").css("background-color", "white");
                        });

                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='11' align='center'>  查无数据 </td> </tr>";
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
                if (confirm("确定删除选择的数据?")) {
                    //通用删除
                    DeleteData("../StoreroomManager/MarerialWarehouseLogList.aspx", ConvertsContent(checkResult), "btnSearch");
                }
            });
            //绑定排序事件和样式
            function tablesorter(className) {
                // $("." + className + " tbody tr:odd").addClass("odd");
                var obj = $("." + className + " thead tr th");
                obj.find("img").hide();
                // obj.addClass("header");
                //排序事件
                obj.click(function () {
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
            $(".tablesorter thead tr td input").click(function () {
                $("input[name='subBox']").each(function () {
                    // $(this).attr("checked", !$(this).attr("checked")); //全选、全不选
                    this.checked = !this.checked; //整个反选
                });
            });

            $("#btnAdd").click(function () {
                OpenDialog("AddOrEditMarerialWarehouseLog.aspx", "btnSearch", "240", "600");
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
                if (confirm("确定审核选择的数据?")) {
                    Check("../StoreroomManager/MarerialWarehouseLogList.aspx", ConvertsContent(checkResult), "btnSearch");
                }
            });
            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
            //出入库编号
            //            BindSelect("MarerialWarehouseLogWarehouseNumber", "WarehouseNumber");
            //变动方向
            BindSelect("MarerialWarehouseLogChangeDirection", "ChangeDirection");
            //仓库名称
            BindSelect("MarerialWarehouseLogWarehouseName", "WarehouseName");

            //出入库类型
            BindSelect("MarerialWarehouseLogType", "Type");

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 1400px;">
          <asp:TextBox ID="txtSql" runat="server" Style="display: none;"></asp:TextBox>
        <input type="hidden" id="saveInfo" runat="server" />
        <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            <img src="../Img/loading.gif" alt="loading" />
        </div>
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
                                <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;库存管理&nbsp;&nbsp;>&nbsp;&nbsp;原材料出入库列表</span>
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
                                    <input type="hidden" id="Hidden1" runat="server" />
                                    <div id="Div1" style="position: absolute; top: 40%; left: 50%; display: none;">
                                        <img src="../Img/loading.gif" alt="loading" />
                                    </div>
                                    <table class="pg_table">
                                        <tr>
                                            <td colspan="10">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pg_talbe_head">
                                                出入库编号：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <%--<select id="WarehouseNumber">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                                <input type="text" id="txtWarehouseNumber" />
                                            </td>
                                            <td class="pg_talbe_head">
                                                仓库名称：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <select id="WarehouseName">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>
                                            </td>
                                            <td class="pg_talbe_head">
                                                变动方向：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <select id="ChangeDirection">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>
                                            </td>
                                            <td class="pg_talbe_head">
                                                出入库类型：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <select id="Type">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>
                                            </td>
                                            <td class="pg_talbe_head">
                                            </td>
                                            <td class="pg_talbe_content">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="pg_talbe_head">
                                                原材料编号：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <input type="text" id="txtMateriNumber" />
                                            </td>
                                            <td class="pg_talbe_head">
                                                供应商物料编号：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <input type="text" id="txtSuppleMateriNumber" />
                                            </td>
                                            <td class="pg_talbe_head">
                                                客户物料编号：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <input type="text" id="txtCustomerMateriNumber" />
                                            </td>
                                            <td class="pg_talbe_head">
                                                描述：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <input type="text" id="txtDescription" />
                                            </td>
                                        </tr>
                                        <tr>
                                         <td class="pg_talbe_head">
                                                开工单号：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <input type="text" id="txtKGNumber" />
                                            </td>
                                            <td class="pg_talbe_head">
                                                销售订单号：
                                            </td>
                                            <td class="pg_talbe_content">
                                                <input type="text" id="txtXSNumber" />
                                            </td>

                                            <td colspan="10" style="text-align: left">
                                                <div style="float: left; width: 150; margin-left: 20%">
                                                    每页显示条数：
                                                    <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                        onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                        maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="100" />
                                                    &nbsp;&nbsp;&nbsp;
                                                </div>
                                                <div>
                                                    <div style="float: left; width: 65px;">
                                                        <input type="button" value="查询" id="btnSearch" class="button" />
                                                    </div>
                                                    <div style="float: left; width: 65px;" id="divAdd" runat="server">
                                                        <input type="button" value="增加" id="btnAdd" class="button" />
                                                    </div>
                                                    <%-- <div style="float: left; width: 65px;" id="divCheck" runat="server">
                                                        <input type="button" value="审核" id="btnCheck" class="button" runat="server" />
                                                    </div>--%>
                                                    <div style="float: left; width: 65px;" id="divDelete" runat="server">
                                                        <input type="button" value="删除" id="btnDelete" class="button" />
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td class="pg_talbe_head">
                                               <label title="多个出入库编号之间用“逗号(,)”分开，注意：逗号是英文输入法状态下的符号"> 出入库编号（多）：</label>
                                            </td>
                                            <td colspan="9">
                                                <input type="text" id="txt_Numbers" style="width:600px;" />   &nbsp;&nbsp;   &nbsp;&nbsp;
                                               <asp:Button ID="Button1" runat="server" Text="导出统计结果" OnClick="Button1_Click" OnClientClick="return ImpExecl()"
                                                            CssClass="button" />
                                            </td>
                                            </tr>
                                        <tr>
                                            <td colspan="10">
                                                <div>
                                                    <table class="tablesorter" cellpadding="1" cellspacing="1" width="1220px;">
                                                        <thead>
                                                            <tr>
                                                                <td>
                                                                    <label style="width: 100%; display: block; cursor: pointer;">
                                                                        <input type="checkbox" />全选/反选</label>
                                                                </td>
                                                                <th sortname='出入库编号'"'>
                                                                    出入库编号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                                                </th>
                                                                <th sortname='仓库名称'>
                                                                    仓库名称<span><img src="../Img/bg.gif" id="Img8" /></span>
                                                                </th>
                                                                <th sortname='变动方向'>
                                                                    变动方向<span><img src="../Img/bg.gif" id="Img3" /></span>
                                                                </th>
                                                                <th sortname='出入库类型'>
                                                                    出入库类型<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                                </th>
                                                                <th sortname='制单人'>
                                                                    制单人<span><img src="../Img/bg.gif" id="Img1" /></span>
                                                                </th>
                                                                <th sortname='制单时间'>
                                                                    制单时间<span><img src="../Img/bg.gif" id="Img2" /></span>
                                                                </th>
                                                                <th sortname='审核时间'>
                                                                    审核时间<span><img src="../Img/bg.gif" id="Img6" /></span>
                                                                </th>
                                                                <th sortname='审核人'>
                                                                    审核人<span><img src="../Img/bg.gif" id="Img5" /></span>
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