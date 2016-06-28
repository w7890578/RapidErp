<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductPlanInfoReport.aspx.cs"
    Inherits="Rapid.ProduceManager.ProductPlanInfoReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>开工单进度报表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .red
        {
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


        //排序字段
        var sortname = "交期时间";
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
            var customerproductnumber = $("#txtCustomerProductNumber").val();
            var productplannumber = $("#txrProductPlanNumber").val();
            var type = $("#txtType").val();
            var year = $("#drpYear").val();
            var month = $("#drpMonth").val();
            var ordernumber = $("#txtOrderNumber").val();
            var status = $("#txtStatus").val();
            var customerordersnumber = $("#txtCustomerOrdersNumber").val();
            if (customerproductnumber != "") {
                condition += " and 客户产成品编号 like '%" + customerproductnumber + "%'";
            }
            if (productplannumber != "") {
                condition += " and 开工单号 like '%" + productplannumber + "%'";
            }
            if (type != "") {
                condition += " and 开工单类型 like '%" + type + "%'";
            }
            if (year != "") {
                condition += " and YEAR(创建时间)='" + year + "'";
            }
            if (month != "") {
                condition += " and MONTH(创建时间)='" + month + "'";
            }
            if (ordernumber != "") {
                condition += " and 销售订单号 like '%" + ordernumber + "%'";
            }
            if (status != "") {
                condition += " and 完成情况 like '%" + status + "%'";
            }
            if (customerordersnumber != "") {
                condition += " and 客户采购订单号 like '%" + customerordersnumber + "%'";
            }
            return condition;

        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_ProductPlanInfoReport  ";
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
            querySql = " select * from V_ProductPlanInfoReport  ";
            querySql = querySql + " " + GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "ProductPlanInfoReport.aspx?time=" + new Date(),
                data: { pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql },
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
                        });
                        $(".tablesorter tbody tr:even").hover(function() {
                            $(this).find("td").css("background-color", "yellow");
                        }, function() {
                            $(this).find("td").css("background-color", "white");
                        });
                        $(".tablesorter tbody tr:odd").hover(function() {
                            $(this).find("td").css("background-color", "yellow");
                        }, function() {
                            $(this).find("td").css("background-color", "#EAFCD5");
                        });
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='28' align='center'>  查无数据 </td> </tr>";
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

        $(function() {
            //查询
            $("#btnSearch").click(function() {

                GetData(1, sortname, sortdirection);
            });



            //绑定排序事件和样式
            function tablesorter(className) {
                var obj = $("." + className + " thead tr th");
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
            }

            //全选/反选
            $(".tablesorter thead tr td input").click(function() {
                $("input[name='subBox']").each(function() {
                    this.checked = !this.checked; //整个反选
                });
            });
            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
            //            BindSelect("DeliveryNumber", "slDeliveryNumber");
            //            BindSelect("DeliveryPerson", "slDeliveryPerson");


        })

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="outerDiv" style="width: 2600px; margin-right: 20px;">
        <input type="hidden" id="saveInfo" runat="server" />
        <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-top: 1px;
            padding-top: 4px;">
            &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
            <span>&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;开工单进度报表</span>
        </div>
        <div>
            <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                <img src="../Img/loading.gif" alt="loading" />
            </div>
            <table class="pg_table" style="margin-top: 10px">
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
                    <td style="width: 500px;">
                    </td>
                    <td style="width: 500px;">
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp; 客户产成品编号：
                        <input type="text" id="txtCustomerProductNumber" />
                    </td>
                    <td>
                        开工单号：
                        <input type="text" id="txrProductPlanNumber" />
                    </td>
                    <td>
                        开工单类型：
                        <input type="text" id="txtType" />
                    </td>
                    <td>
                        年度：
                        <asp:DropDownList ID="drpYear" runat="server">
                            <asp:ListItem Value="" Text="- - 请 选 择 - -"></asp:ListItem>
                            <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
                            <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
                            <asp:ListItem Value="2016" Text="2016"></asp:ListItem>
                            <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
                            <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
                            <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
                            <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        月份：
                        <asp:DropDownList ID="drpMonth" runat="server">
                            <asp:ListItem Value="" Text="- - 请 选 择 - -"></asp:ListItem>
                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                            <asp:ListItem Value="5" Text="5"></asp:ListItem>
                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                            <asp:ListItem Value="7" Text="7"></asp:ListItem>
                            <asp:ListItem Value="8" Text="8"></asp:ListItem>
                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                            <asp:ListItem Value="11" Text="11"></asp:ListItem>
                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp; 客户采购订单号：
                        <input type="text" id="txtCustomerOrdersNumber" />
                    </td>
                    <td>
                        完成情况：
                        <input type="text" id="txtStatus" />
                    </td>
                    <td>
                        销售订单号：
                        <input type="text" id="txtOrderNumber" />
                    </td>
                    <td colspan="8" style="text-align: left">
                        <div style="vertical-align: middle">
                            <div style="float: left; width: 150;">
                                每页显示条数：
                                <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                    onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                    maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="15" />
                                &nbsp;&nbsp;
                            </div>
                        </div>
                        <div>
                            <div style="float: left; width: 65px;">
                                <input type="button" value="查询" id="btnSearch" class="button" />
                            </div>
                            <div style="float: left; width: 65px;" id="div2" runat="server">
                                <asp:Button ID="Button3" runat="server" Text="导出Excel" OnClick="Button1_Click" OnClientClick="return ImpExecl()"
                                    CssClass="button" />
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <div>
                            <table class="tablesorter" cellpadding="1" cellspacing="1">
                                <thead>
                                    <tr>
                                        <th sortname='序号' style="display: none;">
                                            序号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                                id="Img24" /></span>
                                        </th>
                                        <th sortname='客户产成品编号'>
                                            客户产成品编号<span><img src="../Img/bg.gif" id="Img25" /></span>
                                        </th>
                                        <th sortname='产成品编号'>
                                            产成品编号<span><img src="../Img/bg.gif" id="Img26" /></span>
                                        </th>
                                        <th sortname='版本'>
                                            版本<span><img src="../Img/bg.gif" id="Img27" /></span>
                                        </th>
                                        <th sortname='套数'>
                                            套数<span><img src="../Img/bg.gif" id="Img28" /></span>
                                        </th>
                                        <th sortname='开工单号'>
                                            开工单号<span><img src="../Img/bg.gif" id="Img29" /></span>
                                        </th>
                                        <th sortname='开工单类型'>
                                            开工单类型<span><img src="../Img/bg.gif" id="Img30" /></span>
                                        </th>
                                        <th sortname='开工单实际开始时间'>
                                            开工单实际开始时间<span><img src="../Img/bg.gif" id="Img31" /></span>
                                        </th>
                                        <th sortname='开工单实际结束时间'>
                                            开工单实际结束时间<span><img src="../Img/bg.gif" id="Img32" /></span>
                                        </th>
                                        <th sortname='班组'>
                                            班组<span><img src="../Img/bg.gif" id="Img33" /></span>
                                        </th>
                                        <th sortname='小组实际开始时间'>
                                            小组实际开始时间<span><img src="../Img/bg.gif" id="Img34" /></span>
                                        </th>
                                        <th>
                                            小组结束时间
                                        </th>
                                        <th sortname='销售订单号'>
                                            销售订单号<span><img src="../Img/bg.gif" id="Img36" /></span>
                                        </th>
                                        <th sortname='行号'>
                                            行号<span><img src="../Img/bg.gif" id="Img37" /></span>
                                        </th>
                                        <th sortname='单套工时'>
                                            单套工时<span><img src="../Img/bg.gif" id="Img38" /></span>
                                        </th>
                                        <th sortname='合计工时'>
                                            合计工时<span><img src="../Img/bg.gif" id="Img39" /></span>
                                        </th>
                                        <th>
                                            工序1(工时)
                                        </th>
                                        <th>
                                            工序2(工时)
                                        </th>
                                        <th>
                                            工序3(工时)
                                        </th>
                                        <th>
                                            工序4(工时)
                                        </th>
                                        <th sortname='交接班组'>
                                            交接班组<span><img src="../Img/bg.gif" id="Img44" /></span>
                                        </th>
                                        <th sortname='完成数量'>
                                            完成数量<span><img src="../Img/bg.gif" id="Img45" /></span>
                                        </th>
                                        <%--   <th sortname='交线情况'>
                                            交线情况<span><img src="../Img/bg.gif" id="Img46" /></span>
                                        </th>--%>
                                        <th sortname='交期时间'>
                                            交期时间<span><img src="../Img/bg.gif" id="Img47" /></span>
                                        </th>
                                        <th sortname='创建时间'>
                                            创建时间<span><img src="../Img/bg.gif" id="Img1" /></span>
                                        </th>
                                        <th sortname='客户采购订单号'>
                                            客户采购订单号<span><img src="../Img/bg.gif" id="Img2" /></span>
                                        </th>
                                        <th sortname='完成情况'>
                                            完成情况<span><img src="../Img/bg.gif" id="Img3" /></span>
                                        </th>
                                        <td>
                                            备注
                                        </td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="28" align="center">
                                            暂无数据
                                        </td>
                                    </tr>
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <td colspan="28" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
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
