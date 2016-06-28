<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="Rapid.SellManager.CustomerList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客户列表</title>
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

    <script type="text/javascript">
        //排序字段
        var sortname = "客户编号";
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
            var customerId = $("#CustomerId").val();

            var customerName = $("#CustomerName").val();

            if (customerId != "" && customerId != null) {
                condition += " and (客户编号 like '%" + customerId + "%' or 客户编号 like '%" + customerId + "' or 客户编号 like '" + customerId + "%') ";
            }
            if (customerName != "" && customerName != null) {
                condition += " and (客户名称 like '%" + customerName + "%' or 客户名称 like '%" + customerName + "' or 客户名称 like '" + customerName + "%') ";
            }
            return condition;
        }
        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_Customer ";
            querySql = querySql + " " + GetQueryCondition();
            $("#saveInfo").val(querySql + "");
            return true;
        }
        //获取数据
        function GetData(pageIndex, sortName, sortDirection) {

            //获取一页显示行数
            pageSize = $("#txtPageSize").val();
            if (pageSize == "" || isNaN(pageSize)) {
                alert("请正确输入每页显示条数");
                return;
            }
            querySql = " select * from V_CustomerList ";
            querySql = querySql + " " + GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "CustomerList.aspx",
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
                            var tempStr = " <tr> <td colspan='26' align='center'>  查无数据 </td> </tr>";
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
                    DeleteData("../SellManager/CustomerList.aspx", ConvertsContent(checkResult), "btnSearch");
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
                //            window.location.href = "AddOrEditCustomer.aspx";
                OpenDialogWithscroll("AddOrEditCustomer.aspx", "btnSearch", "600", "600");
            });
            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();

            $("#btnImp").click(function() {
                OpenDialog("ImpCustomer.aspx", "btnSearch", "320", "500");
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 3360px; background-color: #F3FFE3; border-radius: 5px; border: 1px solid #B3D08F;
        margin-top: 5px;">
        <div>
            <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-bottom: 10px;
                margin-top: 1px;">
                &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
                <span>&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;销售管理&nbsp;&nbsp;>&nbsp;&nbsp;客户资料</span>
            </div>
            <input type="hidden" id="saveInfo" runat="server" />
            <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                <img src="../Img/loading.gif" alt="loading" />
            </div>
            <table class="pg_table">
                <tr>
                    <td>
                        &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;客户编号：
                    </td>
                    <td>
                        <input type="text" id="CustomerId" />
                    </td>
                    <td>
                        客户名称：
                    </td>
                    <td>
                        <input type="text" id="CustomerName" />
                    </td>
                    <td style="width: 600px;">
                    </td>
                    <td style="width: 600px;">
                    </td>
                    <td>
                    </td>
                    <td style="width: 1000px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="10">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="9" style="text-align: left">
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
                            <div style="float: left; width: 65px;" id="divDelete" runat="server">
                                <input type="button" value="删除" id="btnDelete" class="button" />
                            </div>
                            <div style="float: left; width: 65px; display: none;">
                                <input type="button" value="打印" id="btnPrint" class="button" onclick="doPrint('form1','btnPrint','btnAdd','btnDelete')" />
                            </div>
                            <div>
                                <div style="float: left; width: 65px;" id="divImp" runat="server">
                                    <input type="button" value="导入" id="btnImp" class="button" />
                                </div>
                            </div>
                            <div style="float: left; width: 65px;" id="divExp" runat="server">
                                <span style="display: none;">
                                    <asp:Button ID="Button1" runat="server" Text="导出Excel" OnClick="Button1_Click" OnClientClick="return ImpExecl()"
                                        CssClass="button" /></span>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="26" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
                        padding-right: 10px;">
                        <div id="pages" class="pages clearfix">
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
                                        <th sortname='客户编号'>
                                            客户编号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                        </th>
                                        <th sortname='客户名称'>
                                            客户名称<span><img src="../Img/bg.gif" id="Img8" /></span>
                                        </th>
                                        <td>
                                            法人代表
                                        </td>
                                        <td>
                                            联系人
                                        </td>
                                        <th sortname='联系电话'>
                                            联系电话<span><img src="../Img/bg.gif" id="Img1" /></span>
                                        </th>
                                        <th sortname='备用电话'>
                                            备用电话<span><img src="../Img/bg.gif" id="Img2" /></span>
                                        </th>
                                        <th sortname='注册电话'>
                                            注册电话<span><img src="../Img/bg.gif" id="Img3" /></span>
                                        </th>
                                        <td>
                                            传真
                                        </td>
                                        <th sortname='手机'>
                                            手机<span><img src="../Img/bg.gif" id="Img4" /></span>
                                        </th>
                                        <th sortname='邮编'>
                                            邮编<span><img src="../Img/bg.gif" id="Img5" /></span>
                                        </th>
                                        <th sortname='Email'>
                                            Email<span><img src="../Img/bg.gif" id="Img6" /></span>
                                        </th>
                                        <th sortname='QQ'>
                                            QQ<span><img src="../Img/bg.gif" id="Img7" /></span>
                                        </th>
                                        <td>
                                            开户银行
                                        </td>
                                        <th sortname='银行账号'>
                                            银行账号<span><img src="../Img/bg.gif" id="Img9" /></span>
                                        </th>
                                        <th sortname='银行行号'>
                                            银行行号<span><img src="../Img/bg.gif" id="Img10" /></span>
                                        </th>
                                        <th sortname='纳税号'>
                                            纳税号<span><img src="../Img/bg.gif" id="Img11" /></span>
                                        </th>
                                        <td>
                                            工厂地址
                                        </td>
                                        <td style="width: 200px;">
                                            注册地址
                                        </td>
                                        <td>
                                            网址
                                        </td>
                                        <td style="width: 200px;">
                                            送货地点
                                        </td>
                                        <%--   <th sortname='账期'>
                                            账期<span><img src="../Img/bg.gif" id="Img12" /></span>
                                        </th>
                                        <th sortname='预收百分比'>
                                            预收百分比<span><img src="../Img/bg.gif" id="Img13" /></span>
                                        </th>--%>
                                        <td>
                                            收款方式
                                        </td>
                                        <td>
                                            收款类型
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
                                        <td colspan="26" align="center">
                                            暂无数据
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colspan="26" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
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
