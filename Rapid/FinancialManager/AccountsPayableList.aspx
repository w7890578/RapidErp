<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountsPayableList.aspx.cs"
    Inherits="Rapid.FinancialManager.AccountsPayableList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>应付账款</title>
    <!--通用基本样式#C1DEA6-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .pg_table
        {
            width: 100%;
            text-align: left;
            word-spacing: 0px;
        }
        .pg_talbe_head
        {
            white-space: nowrap;
            text-align: left;
            width: 300px;
            height: 22px;
        }
        .pg_talbe_content
        {
            width: 100px;
            white-space: nowrap;
            text-align: left;
            height: 22px;
        }
    </style>
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        //销售订单详细跳转

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
        //查询sql语句
        var querySql = "";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            var OrdersNumber = $("#txtOrdersNumber").val();
            var SupplierId = $("#txtSupplierId").val();
            if (OrdersNumber != "" ) {
                condition += " and 销售订单号 like '%" + OrdersNumber + "%' ";
            }
            if (SupplierId != "") {
                condition += " and 供应商名称 like '%" + SupplierId + "%' ";
            }
            return condition;
        }
        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_AccountsPayable  ";
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
            querySql = " select * from V_AccountsPayable  ";
            querySql = querySql + " " + GetQueryCondition();
            $.ajax({
                type: "Get",
                url: "AccountsPayableList.aspx",
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

                        $(".tablesorter tbody tr").click(function() {
                            $(this).find("input[type='checkbox']").each(function() {
                                this.checked = !this.checked; //整个反选
                            });
                        });
                        //                        $(".tablesorter tbody tr").hover(function() {
                        //                            $(this).find("td").css("background-color", "#EAFCD5");
                        //                        }, function() {
                        //                            $(this).find("td").css("background-color", "white");
                        //                        });

                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='17' align='center'>  查无数据 </td> </tr>";
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

        $(document).ready(function() {
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

            $("#btnAdd").click(function() {
                OpenDialog("../SellManager/AddOrEditSaleOder.aspx", "btnSearch", "620", "500");
            });

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
//            BindSelect("PayOrdersNumber", "slOrdersNumber");
//            BindSelect("PaySupplierName", "slSupplierName");
        });

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border thead tr td
        {
            padding: 4px;
            background-color: white;
        }
        .border tbody tr td
        {
            padding: 4px;
            background-color: white;
        }
        a
        {
            color: Blue;
        }
        a:hover
        {
            color: Red;
        }
        #choosePrintClounm
        {
            position: absolute;
            top: 20px;
            left: 50px;
            background-color: White;
            width: 170px;
            border: 1px solid green;
            padding: 10px;
            font-size: 14px;
            display: none;
        }
        #choosePrintClounm ul
        {
            margin-bottom: 10px;
        }
        #choosePrintClounm div
        {
            text-align: center;
            color: Green;
        }
        #choosePrintClounm ul li
        {
            list-style: none;
            float: left;
            width: 100%;
            cursor: pointer;
        }
        .style2
        {
            width: 274px;
        }
    </style>
    <div class="outerDiv" style="width: 1600px; margin-right: 20px;">
        <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-top: 1px;
            padding-top: 4px;">
            &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
            <span>&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;账务管理&nbsp;&nbsp;>&nbsp;&nbsp;应付账款</span>
        </div>
        <div>
            <input type="hidden" id="Hidden1" runat="server" />
            <div>
                <input type="hidden" id="saveInfo" runat="server" />
                <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                    <img src="../Img/loading.gif" alt="loading" />
                </div>
                <table class="pg_table" style="margin-top:10px">
                    <tr>
                        <td class="pg_talbe_head">
                           &nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp;&nbsp&nbsp; 订单号：
                           <input type="text" id="txtOrdersNumber" />
                        </td>
                        <td class="pg_talbe_content">
                            供应商名称：
                            <input type="text" id="txtSupplierId" />
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
                        <td class="style2">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td colspan="5" style="text-align: left">
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
                                <div style="float: left; width: 65px;" id="divExp" runat="server">
                                    <asp:Button ID="Button1" runat="server" Text="导出Excel" OnClick="Button1_Click" OnClientClick="return ImpExecl()"
                                        CssClass="button" />
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="12">
                            <div>
                                <table class="tablesorter" cellpadding="1" cellspacing="1">
                                    <thead>
                                        <tr>
                                            <th sortname='序号' style="display: none;">
                                                序号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                                    id="Img10" /></span>
                                            </th>
                                            <th sortname='销售订单号'>
                                                订单号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                            </th>
                                            <th sortname='原材料编号'>
                                                原材料编号<span><img src="../Img/bg.gif" id="Img8" /></span>
                                            </th>
                                            <th sortname='创建时间'>
                                                创建时间<span><img src="../Img/bg.gif" id="Img1" /></span>
                                            </th>
                                            <th sortname='供应商物料编号'>
                                                供应商物料编号<span><img src="../Img/bg.gif" id="Img2" /></span>
                                            </th>
                                            <th sortname='供应商名称' style="width:150px;">
                                                供应商名称<span><img src="../Img/bg.gif" id="Img4" /></span>
                                            </th>
                                            <th sortname='数量'>
                                                数量<span><img src="../Img/bg.gif" id="Img6" /></span>
                                            </th>
                                            <th sortname='单价'>
                                                单价<span><img src="../Img/bg.gif" id="Img3" /></span>
                                            </th>
                                            <th sortname='总价'>
                                                总价<span><img src="../Img/bg.gif" id="Img7" /></span>
                                            </th>
                                            <th sortname='发货日期'>
                                                发货日期<span><img src="../Img/bg.gif" id="Img9" /></span>
                                            </th>
                                            <th sortname='发票号码'>
                                                发票号码<span><img src="../Img/bg.gif" id="Img11" /></span>
                                            </th>
                                            <th sortname='开票日期'>
                                                开票日期<span><img src="../Img/bg.gif" id="Img12" /></span>
                                            </th>
                                            <th sortname='账期'>
                                                账期<span><img src="../Img/bg.gif" id="Img13" /></span>
                                            </th>
                                            <th sortname='款项到期日'>
                                                款项到期日<span><img src="../Img/bg.gif" id="Img14" /></span>
                                            </th>
                                            <th sortname='实际付款金额'>
                                                实际付款金额<span><img src="../Img/bg.gif" id="Img15" /></span>
                                            </th>
                                            <th sortname='实际付款日期'>
                                                实际付款日期<span><img src="../Img/bg.gif" id="Img16" /></span>
                                            </th>
                                            <th sortname='待付款金额'>
                                                待付款金额<span><img src="../Img/bg.gif" id="Img17" /></span>
                                            </th>
                                            <td>
                                                是否结清
                                            </td>
                                            <td>
                                                操作
                                            </td>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td colspan="17" align="center" class="style3">
                                                暂无数据
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
