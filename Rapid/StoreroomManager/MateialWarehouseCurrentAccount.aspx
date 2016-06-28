<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MateialWarehouseCurrentAccount.aspx.cs"
    Inherits="Rapid.StoreroomManager.MateialWarehouseCurrentAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>原材料库房流水帐</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .red
        {
            background-color: Red;
        }
    </style>
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        function sysEdit(guid) {
            OpenDialog("SysEditMateialWarehouseCurrentAccount.aspx?guid=" + guid + "&date=" + new Date(), "btnSearch", "270", "520");
        }

        //排序字段
        var sortname = "移动时间";
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
        var sql = "";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            var customermaterialnumber = $("#txtCustomerMaterialNumber").val();
            var movetime = $("#txtMoveTime").val();
            var ordersnumber = $("#txtOrdersNumber").val();
            var movereason = $("#txtMoveReason").val();
            var warehousenumber = $("#txtWarehouseNumber").val();
            var material = $("#txtMaterialNumber").val();
            if (customermaterialnumber != '') {
                condition += " and 客户物料编号 like '%" + customermaterialnumber + "%'";
            }
            if (movetime != '') {
                condition += " and 移动时间 like '%" + movetime + "%'";
            }
            if (ordersnumber != '') {
                condition += " and 相关单号 like '%" + ordersnumber + "%'";
            }
            if (movereason != '') {
                condition += " and 移动原因 like '%" + movereason + "%'";
            }
            if (warehousenumber != '') {
                condition += " and 出入库编号 like '%" + warehousenumber + "%'";
            }
            if (material != '') {
                condition += " and 原材料编号 like '%" + material + "%'";
            }
            return condition;

        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_MaterialWarehouseCurrentAccount  ";
            querySql = querySql + " " + GetQueryCondition();
            $("#txtSql").val(querySql + "");
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
            querySql = " select * from V_MaterialWarehouseCurrentAccount  ";
            querySql = querySql + " " + GetQueryCondition();
            sql = "union all select '合计','','','','','','', SUM(收入),SUM(发出),SUM(结存),'','','' from V_MaterialWarehouseCurrentAccount"
            sql = sql + " " + GetQueryCondition();
            querySql = querySql + sql;
            $.ajax({
                type: "Get",
                url: "MateialWarehouseCurrentAccount.aspx?time=" + new Date(),
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
                            var tempStr = " <tr> <td colspan='16' align='center'>  查无数据 </td> </tr>";
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



            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();

        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 1550px;">
        <%-- <input type="hidden" id="saveInfo" runat="server" />--%>
        <asp:TextBox ID="txtSql" runat="server" Style="display: none;"></asp:TextBox>
        <table width="100%" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
            <!--背景top-->
            <tr style="margin-bottom: 10px;">
                <td height="30">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="15" height="30">
                                <img src="../Img/tab_03.gif" width="15" height="30" />
                            </td>
                            <td width="1101" background="../Img/tab_05.gif">
                                <img src="../Img/311.gif" width="16" height="16" />
                                <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;库房管理&nbsp;&nbsp;>&nbsp;&nbsp;原材料库房流水帐</span>
                            </td>
                            <td width="281" background="../Img/tab_05.gif">
                                <table border="0" align="right" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="60">
                                        </td>
                                        <td width="52">
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
                                    <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                                        <img src="../Img/loading.gif" alt="loading" />
                                    </div>
                                    <table class="pg_table">
                                        <tr>
                                            <td>
                                                &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp; 客户物料编号：<input type="text" id="txtCustomerMaterialNumber" />
                                            </td>
                                            <td class="style1">
                                                移动时间:<input type="text" id="txtMoveTime" />
                                            </td>
                                            <td>
                                                出入库编号：<input type="text" id="txtWarehouseNumber" />
                                            </td>
                                            <td>
                                                相关单号：<input type="text" id="txtOrdersNumber" />
                                            </td>
                                            <td style="width: 100px;">
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp; &nbsp&nbsp; 移动原因：<input type="text" id="txtMoveReason" />
                                            </td>
                                            <td>
                                                原材料编号：<input type="text" id="txtMaterialNumber" />
                                            </td>
                                            <td colspan="8">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr style="margin-top: 5px;">
                                            <td colspan="6" style="text-align: center">
                                                <div style="vertical-align: middle">
                                                    <div style="width: 150;">
                                                        每页显示条数：
                                                        <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                            onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                            maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="100" />
                                                        &nbsp;&nbsp;
                                                        <input type="button" value="查询" id="btnSearch" class="button" />
                                                        <asp:Button ID="Button1" runat="server" Text="导出Excel" OnClick="Button1_Click" OnClientClick="return ImpExecl()"
                                                            CssClass="button" />
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
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
                                                                <th sortname='序号' style="display: none;">
                                                                    序号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                                                        id="Img10" /></span>
                                                                </th>
                                                                <th sortname='原材料编号'>
                                                                    原材料编号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                                                </th>
                                                                <th sortname='供应商物料编号'>
                                                                    供应商物料编号<span><img src="../Img/bg.gif" id="Img1" /></span>
                                                                </th>
                                                                <th sortname='客户物料编号'>
                                                                    客户物料编号<span><img src="../Img/bg.gif" id="Img8" /></span>
                                                                </th>
                                                                <th sortname='移动时间'>
                                                                    移动时间<span><img src="../Img/bg.gif" id="Img3" /></span>
                                                                </th>
                                                                <th sortname='出入库编号'>
                                                                    出入库编号<span><img src="../Img/bg.gif" id="Img2" /></span>
                                                                </th>
                                                                <th sortname='相关单号'>
                                                                    相关单号<span><img src="../Img/bg.gif" id="Img6" /></span>
                                                                </th>
                                                                <th sortname='收入'>
                                                                    收入<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                                </th>
                                                                <th sortname='发出'>
                                                                    发出<span><img src="../Img/bg.gif" id="Img5" /></span>
                                                                </th>
                                                                <th sortname='结存'>
                                                                    结存<span><img src="../Img/bg.gif" id="Img7" /></span>
                                                                </th>
                                                                <th sortname='经手人'>
                                                                    经手人<span><img src="../Img/bg.gif" id="Img9" /></span>
                                                                </th>
                                                                <th sortname='移动原因'>
                                                                    移动原因<span><img src="../Img/bg.gif" id="Img11" /></span>
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
