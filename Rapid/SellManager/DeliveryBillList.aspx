﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeliveryBillList.aspx.cs"
    Inherits="Rapid.SellManager.DeliveryBillList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>送货单列表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .red {
            background-color: Red;
        }
    </style>
    <%--
    <!--日期插件-->
    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script> --%>
    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        function Transfer(id, type, customerId) {

            if (customerId == "R001") {
                window.location.href = "DeliveryBillListDetail.aspx?id=" + id;
            }
            else {
                window.location.href = "DeliveryNoteDetailed_two.aspx?id=" + id;
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
            var deliveryperson = $("#txtDeliveryPerson").val();
            var deliverynumber = $("#txtDeliveryNumber").val();
            var customerName = $("#txtCustomerName").val();
            if (deliverynumber != '') {
                condition += " and 送货单号 like '%" + deliverynumber + "%'";
            }
            if (deliveryperson != '') {
                condition += " and 送货人 like '%" + deliveryperson + "%'";
            }
            if (customerName != '') {
                condition += " and 客户名称 like '%" + customerName + "%'";
            }
            return condition;

        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_DeliveryBill  ";
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
            querySql = " select * from V_DeliveryBill  ";
            querySql = querySql + " " + GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "DeliveryBillList.aspx?time=" + new Date(),
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
                            $(this).find("input[type='checkbox']").each(function () {
                                this.checked = !this.checked; //整个反选
                            });
                        });
                        $(".tablesorter tbody tr:even").hover(function () {
                            $(this).find("td").css("background-color", "yellow");
                        }, function () {
                            $(this).find("td").css("background-color", "white");
                        });
                        $(".tablesorter tbody tr:odd").hover(function () {
                            $(this).find("td").css("background-color", "yellow");
                        }, function () {
                            $(this).find("td").css("background-color", "#EAFCD5");
                        });
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='10' align='center'>  查无数据 </td> </tr>";
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
                    DeleteData("../SellManager/DeliveryBillList.aspx", ConvertsContent(checkResult), "btnSearch");
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
                OpenDialog("../SellManager/AddOrEditDeliveryBillList.aspx", "btnSearch", "240", "600");
            });

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
            //            BindSelect("DeliveryNumber", "slDeliveryNumber");
            //            BindSelect("DeliveryPerson", "slDeliveryPerson");
            //确认
            $("#btnConfirm").click(function () {
                var isNext = "1";
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要确认的送货单！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定确认选中的送货单?")) {

                    $.ajax({
                        type: "Post",
                        url: "DeliveryBillList.aspx",
                        data: { "DNumbers": ConvertsContent(checkResult), IsCheck: "true" },
                        success: function (res) {
                            if (res != "ok") {
                                if (confirm("系统检测，没有填写数量！是否继续？")) {
                                    QR(ConvertsContent(checkResult));
                                }
                                else {
                                    return false;
                                }
                            }
                            else {
                                QR(ConvertsContent(checkResult));
                            }
                        }
                    });

                    //ConvertsContent(checkResult)

                }
            });

            function QR(number) {
                $.get("DeliveryBillList.aspx?sq=" + new Date(), { confirmNumbers: number, time: new Date() }, function (result) {
                    if (result == "1") {
                        alert("确认成功！");
                        $("#btnSearch").click();
                    }
                    else {
                        alert(result);
                        return;
                    }
                });
            }


            //汇总
            $("#btnCollect").click(function () {
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要汇总的送货单！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定汇总选中的送货单?")) {
                    //通用删除
                    //ConvertsContent(checkResult)
                    $.get("DeliveryBillList.aspx?sq=" + new Date(), { Numbers: ConvertsContent(checkResult), time: new Date() }, function (result) {
                        if (result == "1") {
                            alert("汇总成功！");
                            $("#btnSearch").click();
                        }
                        else {
                            alert(result);
                            return;
                        }
                    });
                }
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 1500px;">
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
                                    <img src="../Img/311.gif" width="16" height="16" />
                                    <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;销售管理&nbsp;&nbsp;>&nbsp;&nbsp;送货单列表</span>
                                </td>
                                <td width="281" background="../Img/tab_05.gif">
                                    <table border="0" align="right" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td width="60"></td>
                                            <td width="52"></td>
                                            <td width="60"></td>
                                            <td width="60"></td>
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
                                <td width="9" background="../Img/tab_12.gif">&nbsp;
                                </td>
                                <td bgcolor="#f3ffe3" style="padding-top: 5px;">
                                    <div>
                                        <input type="hidden" id="saveInfo" runat="server" />
                                        <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                                            <img src="../Img/loading.gif" alt="loading" />
                                        </div>
                                        <table class="pg_table">
                                            <tr>
                                                <td style="width: 280px">&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp; 送货单号：
                                                <%--<select id="slDeliveryNumber">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                                    <input type="text" id="txtDeliveryNumber" />
                                                </td>
                                                <td>送货人：
                                                <%--<select id="slDeliveryPerson">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>--%>
                                                    <input type="text" id="txtDeliveryPerson" />
                                                </td>
                                                <td class="pg_talbe_head">客户名称:
                                                <input type="text" id="txtCustomerName" />
                                                </td>
                                                <td class="pg_talbe_content"></td>
                                                <td class="pg_talbe_head"></td>
                                                <td class="pg_talbe_content"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td colspan="5" style="text-align: left">
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
                                                            <input type="button" value="查询" id="btnSearch" class="button" />
                                                        </div>
                                                        <span style="display: none">
                                                            <div style="float: left; width: 65px;">
                                                                <input type="button" value="增加" id="btnAdd" class="button" />
                                                            </div>
                                                        </span>
                                                        <div style="float: left; width: 65px;">
                                                            <input type="button" value="汇总" id="btnCollect" class="button" />
                                                        </div>
                                                        <div style="float: left; width: 65px;">
                                                            <input type="button" value="确认" id="btnConfirm" class="button" />
                                                        </div>
                                                        <div style="float: left; width: 65px; display: none;">
                                                            <input type="button" value="删除" id="btnDelete" class="button" />
                                                        </div>
                                                        <div style="float: left; width: 65px;" id="divExp" runat="server">
                                                            <span style="display: none;">
                                                                <asp:Button ID="Button1" runat="server" Text="导出Excel" OnClick="Button1_Click" OnClientClick="return ImpExecl()"
                                                                    CssClass="button" Visible="false" /></span>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td></td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td colspan="8">
                                                    <div>
                                                        <table class="tablesorter" cellpadding="1" cellspacing="1" width="1220px">
                                                            <thead>
                                                                <tr>
                                                                    <td>
                                                                        <label style="width: 100%; display: block; cursor: pointer;">
                                                                            <input type="checkbox" />全选/反选</label>
                                                                    </td>
                                                                    <th sortname='序号' style="display: none;">序号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                                                        id="Img10" /></span>
                                                                    </th>
                                                                    <th sortname='送货单号'>送货单号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                                                    </th>
                                                                    <th sortname='送货人'>送货人<span><img src="../Img/bg.gif" id="Img8" /></span>
                                                                    </th>
                                                                    <th sortname='送货日期'>送货日期<span><img src="../Img/bg.gif" id="Img1" /></span>
                                                                    </th>
                                                                    <th sortname='客户名称'>客户名称<span><img src="../Img/bg.gif" id="Img3" /></span>
                                                                    </th>
                                                                    <th sortname='确认状态'>确认状态<span><img src="../Img/bg.gif" id="Img2" /></span>
                                                                    </th>
                                                                    <th sortname='创建时间'>创建时间<span><img src="../Img/bg.gif" id="Img6" /></span>
                                                                    </th>
                                                                    <td>联系人
                                                                    </td>
                                                                    <th sortname='联系电话'>联系电话<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                                    </th>
                                                                    <td style="width: 100px;">备注
                                                                    </td>
                                                                    <td>操作
                                                                    </td>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <tr>
                                                                    <td colspan="10" align="center">暂无数据
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                            <tfoot>
                                                                <tr>
                                                                    <td colspan="11" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px; padding-right: 10px;">
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
                                <td width="9" background="../Img/tab_16.gif">&nbsp;
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
                                <td background="../Img/tab_21.gif"></td>
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
