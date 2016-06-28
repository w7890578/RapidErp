<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductPlanSubList.aspx.cs"
    Inherits="Rapid.ProduceManager.ProductPlanSubList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>开工单分表</title>
    <!--通用基本样式-->
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <!--Jquery.js-->

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        //排序字段
        var sortname = "开工单号";
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
            var team = $("#slTeam").val();
            var plannumber = $("#PlanNumber").val();
            var factEndTime = $("#FactEndTime").val();
            var planNumber = getQueryString("PlanNumber");
            if (planNumber != "") {
                condition += " and  (开工单号='" + planNumber + "')";
            }
            if (team != "") {
                condition += " and (班组='" + team + "')";
            }
            if (plannumber != "") {
                condition += " and (开工单号 like '%" + plannumber + "%' or 开工单号 like '%" + plannumber + "' or 开工单号 like '" + plannumber + "%')";
            }

            if (factEndTime != "") {
                condition += " and (实际结束时间 like '%" + factEndTime + "%' or 实际结束时间 like '%" + factEndTime + "' or 实际结束时间 like '" + factEndTime + "%')";
            }


            return condition;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_ProductPlanSub ";
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


            querySql = " select * from V_ProductPlanSub ";
            querySql = querySql + " " + GetQueryCondition() + " ";
           
            $.ajax({
                type: "Get",
                url: "ProductPlanSubList.aspx",
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
                        $(".tablesorter tbody tr:odd").addClass("odd");
                        $(".tablesorter tbody tr").click(function() {
                            $(this).find("input[type='checkbox']").each(function() {
                                this.checked = !this.checked; //整个反选
                            });
                        });

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
                if (confirm("确定删除选中的数据?")) {
                    //通用删除
                    DeleteData("", ConvertsContent(checkResult), "btnSearch");
                }
            });
            //删除
            $("#btnCheck").click(function() {
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function() {
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
                    Check("EditProductPlanSub.aspx", ConvertsContent(checkResult), "btnSearch");
                }
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
                OpenDialog("EditProductPlanSub.aspx", "btnSearch", "320", "600");
            });

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
            $("#btnBack").click(function() {
                window.location.href = "ProductPlanList.aspx";
            });
            BindSelect("Team", "slTeam");

        });
    </script>

    <style type="text/css">
        .table_td
        {
            width: 236px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="1500px" height="100%" border="0" align="center" cellpadding="0" cellspacing="0">
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
                                <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;<a href="ProductPlanList.aspx">开工单列表</a>&nbsp;&nbsp;>&nbsp;&nbsp;<%=type%>开工单分表明细列表</span>
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
                                    <input type="hidden" id="saveInfo" runat="server" />
                                    <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                                        <img src="../Img/loading.gif" alt="loading" />
                                    </div>
                                    <table class="pg_table">
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="4">
                                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 开工单号：<input type="text" id="PlanNumber" />
                                             
                                                实际结束时间：<input type="text" id="FactEndTime" />
                                            
                                                <asp:Label ID="Label1" runat="server" Text="班组："></asp:Label>
                                                <select id="slTeam" style="margin-right: 30px">
                                                    <option value="">- - - - - 请 选 择 - - - - -</option>
                                                </select>
                                                 每页显示条数：
                                                        <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                            onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                            maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="15" />
                                                        &nbsp;&nbsp;
                                                  <input type="button" value="查询" id="btnSearch" class="button" />&nbsp;&nbsp;
                                                 <input type="button" value="返回" id="btnBack" class="button" />&nbsp;&nbsp;
                                                <input type="button" value="增加" id="btnAdd" class="button" style="display: none" />
                                                </td>
                                        
                                        </tr>
                                        <tr>
                                            <td colspan="4">
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
                                                                        id="Img14" /></span>
                                                                </th>
                                                                <th sortname='开工单号'>
                                                                    开工单号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                                                        id="Img10" /></span>
                                                                    <th sortname='班组'>
                                                                        班组 <span>
                                                                            <img src="../Img/bg.gif" id="Img1" /></span>
                                                                    </th>
                                                                    <th sortname='人数'>
                                                                        人数 <span>
                                                                            <img src="../Img/bg.gif" id="Img3" /></span>
                                                                    </th>
                                                                    <th sortname='额定总工时'>
                                                                        额定总工时<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                                    </th>
                                                                    <th sortname='实际总工时'>
                                                                        实际总工时 <span>
                                                                            <img src="../Img/bg.gif" id="Img5" /></span>
                                                                    </th>
                                                                    <th sortname='目标完成工时'>
                                                                        目标完成工时 <span>
                                                                            <img src="../Img/bg.gif" id="Img7" /></span>
                                                                    </th>
                                                                    <th sortname='实际完成工时''>
                                                                        实际完成工时 <span>
                                                                            <img src="../Img/bg.gif" id="Img9" /></span>
                                                                    </th>
                                                                    <th sortname='实际开始时间'>
                                                                        实际开始时间 <span>
                                                                            <img src="../Img/bg.gif" id="Img12" /></span>
                                                                    </th>
                                                                    <th sortname='实际结束时间''>
                                                                        实际结束时间 <span>
                                                                            <img src="../Img/bg.gif" id="Img13" /></span>
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
                                                                <td colspan="13" align="center">
                                                                    暂无数据
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                        <tfoot>
                                                            <tr>
                                                                <td colspan="13" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
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
