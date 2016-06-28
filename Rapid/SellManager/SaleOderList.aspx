<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SaleOderList.aspx.cs" Inherits="Rapid.SellManager.SaleOderList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>销售订单列表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->
    <link href="../Js/jquery-easyui-1.4/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../Js/jquery-easyui-1.4/themes/icon.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-easyui-1.4/jquery.min.js" type="text/javascript"></script>

    <script src="../Js/jquery-easyui-1.4/jquery.easyui.min.js" type="text/javascript"></script>

    <script src="../Js/jquery-easyui-1.4/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        .pg_table thead tr td {
            font-size: 14px;
        }

        .STYLE4 {
            font-size: 14px;
        }

        .printDiv {
            border-radius: 5px;
            border: 1px solid #B3D08F;
            margin-top: 5px;
            margin-right: 10px;
            background-color: #F3FFE3;
            width: 1600px;
        }
    </style>

    <script type="text/javascript">
        //销售订单详细跳转
        function Transfer(id, parameter) {
            if (parameter == "加工") {
                window.location.href = "MachineOderDetail.aspx?id=" + id;
            }
            else {
                window.location.href = "TradingOrderDetail.aspx?id=" + id;
            }
        }
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
            var orderType = $("#slOrderType").val();
            var receivablesMode = $("#slReceivablesMode").val();
            var customerid = $("#txtCustomerName").val();
            var producttype = $("#slProductType").val();
            var orderstatus = $("#slOrderStatus").val();
            var ordersdate = $("#txtOrdersDate").val();
            var noLeadTime = $("#slLeadTime").val();
            var oderNumber = $("#txtOdersNumber").val();
            var customerOrdersNumber = $("#txtCustomerOrdersNumber").val();
            if (orderType != '') {
                condition += " and 订单类型='" + orderType + "' ";
            }
            //            if (receivablesMode != '') {
            //                receivablesMode = $("#slReceivablesMode").find("option:selected").text();
            //                condition += " and 收款方式='" + receivablesMode + "' ";
            //            }
            if (customerid != '') {
                condition += " and 客户名称 like '%" + customerid + "%' ";
            }
            if (producttype != '') {
                condition += " and 生产类型='" + producttype + "'";
            }
            //            if (orderstatus != '') {
            //                condition += " and 订单状态='" + orderstatus + "'";
            //            }
            if (ordersdate != "") {
                condition += " and 订单日期='" + ordersdate + "'";
            }
            if (noLeadTime == "未交订单") {
                condition += " and 交期状态='未交订单' ";
            }
            if (oderNumber != "") {
                condition += " and 销售订单号='" + oderNumber + "' ";
            }
            if (customerOrdersNumber != "") {
                condition += " and 客户采购订单号 like '%" + customerOrdersNumber + "%' ";
            }
            return condition;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_SaleOder  ";
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
            querySql = " select * from V_SaleOder  ";
            querySql = querySql + " " + GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "SaleOderList.aspx",
                data: { time: new Date(), pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql, customerProductNumber: $("#txtCustomerProductNumber").val(), customerMateriNumber: $("#txtCustomerMateriNumber").val() },
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
                            $(this).find("input[type='checkbox']").each(function () {
                                this.checked = !this.checked; //整个反选
                            });
                        });
                        //                        $(".tablesorter tbody tr:even").hover(function() {
                        //                            $(this).find("td").css("background-color", "yellow");
                        //                        }, function() {
                        //                            $(this).find("td").css("background-color", "white");
                        //                        });
                        //                        $(".tablesorter tbody tr:odd").hover(function() {
                        //                            $(this).find("td").css("background-color", "yellow");
                        //                        }, function() {
                        //                            $(this).find("td").css("background-color", "#EAFCD5");
                        //                        });
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='15' align='center'>  查无数据 </td> </tr>";
                            $(".tablesorter tbody").append(tempStr);
                            //分页清空
                            $("#pageing").html("");
                        }

                        $("#pages").html(tempArray[2]); pages
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='15' align='center'>  查无数据 </td> </tr>";
                            $(".tablesorter tbody").append(tempStr);
                            //分页清空
                            $("#pages").html("");
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
        //转成正式订单
        function Change(id) {
            if (!confirm("确定转成正式订单？")) {
                return false;
            }
            var CustomerOrderNumber = prompt("输入客户采购订单号", "");
            if (CustomerOrderNumber == "" || CustomerOrderNumber == null) {
                alert("请输入客户采购订单号");
                return;
            }

            $.ajax({
                url: "../SellManager/SaleOderList.aspx",
                type: "Get",
                data: { "id": id, "IsChangeType": "true", "CustomerOrderNumber": CustomerOrderNumber },
                success: function (res) {
                    if (res == "ok") {
                        alert("转换成功");
                        $("#btnSearch").click();
                    }
                    else {
                        alert(res);
                        return;
                    }
                },
                error: function (error) {
                    alert(JSON.stringify(error));
                }
            });
        }

        $(document).ready(function () {

            //$("#OdersType").combobox({
            //    select: function () {

            //    }
            //});

            //查询
            $("#btnSearch").click(function () {
                GetData(1, sortname, sortdirection);
            });

            //导入
            $("#btnImp").click(function () {
                //OpenDialog("ImpSaleOderList.aspx", "btnSearch", "410", "700");

                window.location.href = "ImpSaleOderList.aspx";
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
                    DeleteData("../SellManager/SaleOderList.aspx", ConvertsContent(checkResult), "btnSearch");
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
                    Check("../SellManager/SaleOderList.aspx", ConvertsContent(checkResult), "btnSearch");
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
                if (confirm("系统检测到新版添加功能，是否切换新版添加？")) {
                    OpenDialog("../SellManager/AddOrEditSaleOder2.aspx", "btnSearch", "350", "900");

                }
                else {
                    OpenDialog("../SellManager/AddOrEditSaleOder.aspx", "btnSearch", "400", "700");

                }
            });

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
            BindSelect("ReceivablesMode", "slReceivablesMode");
            //            BindSelect("SaleCustomerName", "slCustomerName");

        });
    </script>

    <style type="text/css">
        .table_td {
            width: 236px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            <img src="../Img/loading.gif" alt="loading" />
        </div>
        <div class="printDiv" id="upDiv" style="width: 1600px">
            <div>
                <input type="hidden" id="saveInfo" runat="server" />
                <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-bottom: 10px; margin-top: 1px;">
                    &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
                    <span>&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;销售管理&nbsp;&nbsp;>&nbsp;&nbsp;销售订单</span>
                </div>
                <table class="pg_table">
                    <thead>
                        <tr>
                            <td>&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp; 生产类型：
                            <select id="slProductType">
                                <option value="">- - - - - 请 选 择 - - - - -</option>
                                <option value="加工">加工</option>
                                <option value="贸易">贸易</option>
                            </select>
                            </td>
                            <td>订单类型：
                            <select id="slOrderType">
                                <option value="">- - - - - 请 选 择 - - - - -</option>
                                <option value="加急订单">加急订单</option>
                                <option value="正常订单">正常订单</option>
                                <option value="维修订单">维修订单</option>
                                <option value="临时订单">临时订单</option>
                                <option value="样品订单">样品订单</option>
                                <option value="包装生产订单">包装生产订单</option>
                            </select>
                            </td>
                            <%-- <td>
                                                            收款方式：
                                                            <select id="slReceivablesMode">
                                                                <option value="">- - - - - 请 选 择 - - - - -</option>
                                                            </select>
                                                        </td>--%>
                            <%-- <td>
                                                            订单日期：
                                                            <input type="text" id="txtOrdersDate" onfocus="WdatePicker({skin:'green'})" />
                                                        </td>--%>
                            <td>销售订单号：
                            <input type="text" id="txtOdersNumber" />
                            </td>
                        </tr>
                        <tr>
                            <%--<td>
                                                            &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp; 订单状态：
                                                            <select id="slOrderStatus">
                                                                <option value="">- - - - - 请 选 择 - - - - -</option>
                                                                <option value="未完成">未完成</option>
                                                                <option value="已完成">已完成</option>
                                                            </select>
                                                        </td>--%>
                            <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 订单日期：
                            <input type="text" id="txtOrdersDate" onfocus="WdatePicker({skin:'green'})" />
                            </td>
                            <td>客户名称：
                            <%--<select id="slCustomerName">
                                <option value="">- - - - - 请 选 择 - - - - -</option>
                            </select>--%>
                                <input type="text" id="txtCustomerName" />
                            </td>
                            <td>交 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;期：&nbsp;<select
                                id="slLeadTime">
                                <option value="">- - - - - 请 选 择 - - - - -</option>
                                <option value="未交订单">未交订单</option>
                                <option value="全部订单">全部订单</option>
                            </select>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp; 客户物料编号：<input type="text" id="txtCustomerMateriNumber" />
                            </td>
                            <td>客户产成品编号：<input type="text" id="txtCustomerProductNumber" />
                            </td>
                            <td>客户采购订单号：
                            <input type="text" id="txtCustomerOrdersNumber" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="style6"></td>
                            <td class="style7"></td>
                            <td colspan="5" style="text-align: left">
                                <div style="vertical-align: middle">
                                    <div style="float: left; width: 150;">
                                        每页显示条数：
                                    <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                        onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                        maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="30" />
                                        &nbsp;&nbsp;
                                    </div>
                                </div>
                                <div>
                                    <div style="float: left; width: 65px;">
                                        <input type="button" value="查询" id="btnSearch" class="button" />
                                    </div>
                                    <div style="float: left; width: 65px;" id="divAdd" runat="server">
                                        <input type="button" value="增加" id="btnAdd" class="button" />
                                    </div>
                                    <div style="float: left; width: 65px;" id="divCheck" runat="server">
                                        <input type="button" value="审核" id="btnCheck" class="button" />
                                    </div>
                                    <div style="float: left; width: 65px;" id="divDelete" runat="server">
                                        <input type="button" value="删除" id="btnDelete" class="button" />
                                    </div>
                                    <div style="float: left; width: 65px;" id="divExp" runat="server">
                                        <span style="display: none;">
                                            <asp:Button ID="Button1" runat="server" Text="导出Excel" OnClick="Button1_Click" OnClientClick="return ImpExecl()"
                                                CssClass="button" /></span>
                                    </div>
                                    <div style="float: left; width: 65px;" id="divImp" runat="server">
                                        <%-- <asp:Button ID="btnImp" runat="server" Text="导入" CssClass="button" />--%>
                                        <input type="button" id="btnImp" value="导入" class="button" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="15" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px; padding-right: 10px;">
                                <div id="pages" class="pages clearfix">
                                </div>
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="8">
                                <div>
                                    <table class="tablesorter" cellpadding="1" cellspacing="1">
                                        <thead>
                                            <tr>
                                                <td>
                                                    <label style="width: 100%; display: block; cursor: pointer;">
                                                        <input type="checkbox" />全选/反选</label>
                                                </td>
                                                <th sortname='序号' style="display: none;">序号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                                    id="Img10" /></span>
                                                </th>
                                                <th sortname='销售订单号'>销售订单号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                                </th>
                                                <th sortname='客户采购订单号'>客户采购订单号<span><img src="../Img/bg.gif" id="Img7" /></span>
                                                </th>
                                                <%-- <th sortname='客户订单号'>
                                                客户订单号<span><img src="../Img/bg.gif" id="Img19" /></span>
                                            </th>--%>
                                                <th sortname='订单日期'>订单日期<span><img src="../Img/bg.gif" id="Img8" /></span>
                                                </th>
                                                <th sortname='生产类型'>生产类型<span><img src="../Img/bg.gif" id="Img3" /></span>
                                                </th>
                                                <th sortname='订单类型'>订单类型<span><img src="../Img/bg.gif" id="Img2" /></span>
                                                </th>
                                                <th sortname='收款方式'>收款方式<span><img src="../Img/bg.gif" id="Img6" /></span>
                                                </th>
                                                <th sortname='客户名称'>客户名称<span><img src="../Img/bg.gif" id="Img7" /></span>
                                                </th>
                                                <th sortname='业务员'>业务员<span><img src="../Img/bg.gif" id="Img19" /></span>
                                                </th>
                                                <%-- <th sortname='产成品编号'>
                                                产成品编号<span><img src="../Img/bg.gif" id="Img6" /></span>
                                            </th>
                                            <th sortname='版本'>
                                                版本<span><img src="../Img/bg.gif" id="Img11" /></span>
                                            </th>
                                            <th sortname='行号'>
                                                行号<span><img src="../Img/bg.gif" id="Img12" /></span>
                                            </th>
                                            <th sortname='客户物料编号'>
                                                客户物料编号<span><img src="../Img/bg.gif" id="Img13" /></span>
                                            </th>
                                            <th sortname='描述' style="width: 200px">
                                                描述<span><img src="../Img/bg.gif" id="Img14" /></span>
                                            </th>
                                            <th sortname='数量'>
                                                数量<span><img src="../Img/bg.gif" id="Img15" /></span>
                                            </th>
                                            <th sortname='未交数量'>
                                                未交数量<span><img src="../Img/bg.gif" id="Img16" /></span>
                                            </th>
                                            <th sortname='已交数量'>
                                                已交数量<span><img src="../Img/bg.gif" id="Img17" /></span>
                                            </th>
                                            <th sortname='交期'>
                                                交期<span><img src="../Img/bg.gif" id="Img18" /></span>
                                            </th>
                                            <td>
                                                客户
                                            </td>--%>
                                                <th sortname='订单状态'>订单状态<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                </th>
                                                <th sortname='创建时间'>创建时间<span><img src="../Img/bg.gif" id="Img5" /></span>
                                                </th>
                                                <th sortname='审核人'>审核人<span><img src="../Img/bg.gif" id="Img7" /></span>
                                                </th>
                                                <th sortname='审核时间'>审核时间<span><img src="../Img/bg.gif" id="Img9" /></span>
                                                </th>
                                                <td>备注
                                                </td>
                                                <th sortname='交期状态' style="display: none;">交期状态<span><img src="../Img/bg.gif" id="Img1" /></span>
                                                </th>
                                                <td>操作
                                                </td>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td colspan="15" align="center">暂无数据
                                                </td>
                                            </tr>
                                        </tbody>
                                        <tfoot>
                                            <tr>
                                                <td colspan="15" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px; padding-right: 10px;">
                                                    <div id="pageing" class="pages clearfix">
                                                    </div>
                                                </td>
                                            </tr>
                                        </tfoot>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="easyui-dialog" style="width: 600px; height: 600px" data-options="buttons:'#bb',modal:true,closed:true"
            id="Dlg">
            <form id="fmDetail" method="post"></form>
            <table>
                <tr>
                    <td style="text-align: right;">生产类型：</td>
                    <td>
                        <select name="ProductType" class="easyui-combobox" style="width: 150px;">
                            <option value="加工">加工</option>
                            <option value="贸易">贸易</option>
                        </select>

                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">订单类型：</td>
                    <td>
                        <select id="OdersType" name="OdersType" class="easyui-combobox" style="width: 150px;">
                            <option value="正常订单">正常订单</option>
                            <option value="加急订单">加急订单</option>
                            <option value="维修订单">维修订单</option>
                            <option value="临时订单">临时订单</option>
                            <option value="样品订单">样品订单</option>
                            <option value="包装生产订单">包装生产订单</option>
                        </select>

                    </td>
                </tr>
            </table>
        </div>
        <div id="bb">
            <a href="###" class="easyui-linkbutton">保存</a> <a href="javascript:void(0)" class="easyui-linkbutton"
                iconcls="icon-cancel" onclick="javascript:$('#Dlg').dialog('close')">关闭</a>
        </div>
    </form>
</body>
</html>
