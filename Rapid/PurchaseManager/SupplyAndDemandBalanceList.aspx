<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupplyAndDemandBalanceList.aspx.cs"
    Inherits="Rapid.PurchaseManager.SupplyAndDemandBalanceList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>供需平衡表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <!--主要js-->

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            //查询sql语句 
            $("#btnPrint").click(function () {
                $("#choosePrintClounm").toggle();
            });
            $("#btnExit").click(function () {
                $("#choosePrintClounm").hide();
            });
            $("#btnChoosePrintColum").click(function () {
                var chooseResult = "";
                var unChooseResult = "";
                var arrChk = $("input[name='columList']:checkbox");
                $(arrChk).each(function () {
                    if ($(this).is(':checked')) {
                        chooseResult += $(this).val() + ",";

                    }
                    else {
                        unChooseResult += $(this).val() + ",";

                    }
                });
                var reg = /,$/gi;
                chooseResult = chooseResult.replace(reg, "");
                unChooseResult = unChooseResult.replace(reg, "");

                var unChoosedArray = unChooseResult.split(',');
                if (chooseResult == "") {
                    alert("请选择要打印的列!");
                    return;
                }
                if (!confirm("确定打印所选列？")) {
                    return;
                }
                //遍历border样式的table下的td
                $("#printTalbe tr td").each(function () {
                    className = $(this).attr("class");
                    if (className == "tdOperar") {
                        $(this).hide();
                    }
                    for (var j = 0; j < unChoosedArray.length; j++) {
                        if (className == unChoosedArray[j] + "") {
                            $(this).hide();
                        }
                    }
                });
                $("#printTalbe").removeClass("tablesorter").addClass("border");
                $("#upDiv").removeClass("printDiv");
                $("#divs").hide();
                newwin = window.open("", "newwin", "height=900,width=750,toolbar=no,scrollbars=auto,menubar=no,resizable=no,location=no");
                newwin.document.body.innerHTML = document.getElementById("form1").innerHTML;
                newwin.document.getElementById("divHeader").style.display = 'none';
                newwin.document.getElementById("choosePrintClounm").style.display = 'none';


                newwin.window.print();
                newwin.window.close();
                $("#choosePrintClounm").hide();
                $("#printTalbe tr td").each(function () {
                    $(this).show();
                })
                $("#divs").show();
                $("#printTalbe").removeClass("border").addClass("tablesorter");
                $("#upDiv").addClass("printDiv");
            });
        });
        var querySql = "";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            return condition;
        }
    </script>

    <style type="text/css">
        .printDiv {
            border-radius: 5px;
            border: 1px solid #B3D08F;
            margin-top: 5px;
            margin-right: 10px;
            background-color: #F3FFE3;
            width: 100%;
        }
    </style>

    <script type="text/javascript">
        //排序字段
        var sortname = "计算结果";
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
            var materialNumber = $.trim($("#txtMaterialdescription").val());
            var number = $.trim($("#txtMaterialNumber").val());
            var txtxh = $.trim($("#txtxh").val())
            if (materialNumber != "" && materialNumber != null) {
                condition += " and 原材料描述 like '%" + materialNumber + "%' ";
            }
            if (number != "" && number != null) {
                condition += " and  原材料编号  like '%" + number + "%' ";
            }
            if (txtxh != "" && txtxh!=null)
            {
                condition += " and 型号 like '%" + txtxh + "%' ";
            }
            return condition;
        }
        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = "select * from V_SupplyAndDemandBalanceList_New   ";
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
            querySql = " select * from V_SupplyAndDemandBalanceList_New ";
            querySql = querySql + " " + GetQueryCondition();
            $.ajax({
                type: "Get",
                url: "SupplyAndDemandBalanceList.aspx",
                data: { time: new Date(), pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql },
                beforeSend: function () { $("#progressBar").show(); },
                success: function (result) {
                    //alert(result);
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
                            $(this).find("td").css("background-color", "yellow");
                        }, function () {
                            $(this).find("td").css("background-color", "white");
                        });

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


            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();

            $("#btnAdd").click(function () {
                OpenDialog("../PurchaseManager/AddMrp.aspx", "btnSearch", "380", "400");
            });

        });
    </script>

</head>
<body style="padding: 5px 10px 0px 0px;">
     <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            <img src="../Img/loading.gif" alt="loading" />
        </div>
    <form id="form1" runat="server">
        <input type="hidden" id="saveInfo" runat="server" />
        <style type="text/css">
            .border {
                background-color: Black;
                width: 740px;
                font-size: 14px;
                text-align: center;
            }

                .border thead tr td {
                    padding: 4px;
                    background-color: white;
                }

                .border thead tr th {
                    padding: 4px;
                    background-color: white;
                }

                .border tbody tr td {
                    padding: 4px;
                    background-color: white;
                }

            a {
                color: Blue;
            }

                a:hover {
                    color: Red;
                }

            #choosePrintClounm {
                position: absolute;
                top: 70px;
                left: 550px;
                background-color: White;
                width: 170px;
                border: 1px solid green;
                padding: 10px;
                font-size: 14px;
                display: none;
            }

                #choosePrintClounm ul {
                    margin-bottom: 10px;
                }

                #choosePrintClounm div {
                    text-align: center;
                    color: Green;
                }

                #choosePrintClounm ul li {
                    list-style: none;
                    float: left;
                    width: 100%;
                    cursor: pointer;
                }

            .style1 {
                width: 9px;
            }
        </style>
        <style type="text/css">
            a:link, a:visited {
                text-decoration: none; /*超链接无下划线*/
                color: blue;
            }

            .tablesorter tbody tr td {
                padding: 6px;
            }
        </style>
        <div class="printDiv" id="upDiv" style="width: 1400px;">
            <table class="pg_table">
                <tr>
                    <td colspan="11">
                        <div id="divs">
                            <div style="background-image: url(../Img/nav_tab1.gif); width: auto; margin-bottom: 10px;">
                                &nbsp&nbsp;&nbsp&nbsp;<img src="../Img/311.gif" width="16" height="16" />
                                <span class="STYLE4">&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;采购管理&nbsp;&nbsp;>&nbsp;&nbsp;供需平衡表</span>
                            </div>
                            <div style="vertical-align: middle">
                                <div style="float: left; width: 150;">
                                    &nbsp;&nbsp;&nbsp;&nbsp; 原材料描述：
                                <input type="text" id="txtMaterialdescription" style="margin-right: 10px" />
                                    &nbsp;&nbsp;原材料编号:
                                  <input type="text" id="txtMaterialNumber" style="margin-right: 10px" />
                                    &nbsp;&nbsp;型号:
                                    <input type="text" id="txtxh" style="margin-right: 10px"/>
                                    每页显示条数：
                                <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                    onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                    maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="300" />
                                    &nbsp;&nbsp;
                                </div>
                            </div>
                          
                            <div id="divHeader">
                                <div style="float: left; width: 65px;">
                                    <input type="button" value="查询" id="btnSearch" class="button" />
                                </div>
                                <div style="float: left; width: 95px;display:none;" id="divAdd" runat="server">
                                    <input type="button" value=" 运行MRP " id="btnAdd" class="button" />
                                </div>
                                <div style="float: left; width: 65px;" id="divPrint" runat="server">
                                    <input type="button" value="打印" id="btnPrint" style="margin-left: 10px" class="button" />
                                    <div id="choosePrintClounm">
                                        <div>
                                            请选择要打印的列
                                        </div>
                                        <ul>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_原材料编号" checked="checked" />
                                                    原材料编号
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_原材料描述" checked="checked" />
                                                    原材料描述
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_最小起订量" checked="checked" />
                                                    最小起订量
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_库存数量" checked="checked" />
                                                    库存数量
                                                </label>
                                            </li>
                                              <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_实际库存数量" checked="checked" />
                                                    实际库存数量
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_在途数量" checked="checked" />
                                                    在途数量
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_订单需求数" checked="checked" />
                                                    订单需求数
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_实际需求数" checked="checked" />
                                                    实际需求数
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_安全库存数量" checked="checked" />
                                                    安全库存数量
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_计算结果" checked="checked" />
                                                    计算结果
                                                </label>
                                            </li>
                                            <li>
                                                <label>
                                                    <input type="checkbox" name="columList" value="tdOperar_平衡结果" checked="checked" />
                                                    平衡结果
                                                </label>
                                            </li>
                                        </ul>
                                        <div>
                                            &nbsp;<br />
                                            <input type="button" value=" 确 定 " id="btnChoosePrintColum" />&nbsp;&nbsp;&nbsp;&nbsp;<input
                                                type="button" value=" 取 消 " id="btnExit" />
                                        </div>
                                    </div>
                                </div>
                                <div style="float: left; width: 65px;" id="div1" runat="server">
                                    <asp:Button ID="btnExport" runat="server" Text="导出Excel" OnClick="btnExport_Click"
                                        class="button" OnClientClick="return ImpExecl()" />
                                </div>
                            </div>
                            <br />   <br />   <br />
                                <div style="color:green;padding-left:100px; font-family:'Microsoft YaHei',微软雅黑,'MicrosoftJhengHei',华文细黑,STHeiti,MingLiu ;"> 系统提示：请您在物料库存相对<label style="color:red;">稳定</label>的情况下进行MRP运算，推荐在<label style="color:red;">上班之前或下班之后</label> 这段时间内进行MRP运算。</div>
                          
                        </div>
                
                             
                    </td>
                
                </tr>
                <tr>
                    <td colspan="10">
                        <div>
                            <table class="tablesorter" cellpadding="1" cellspacing="1" id="printTalbe">
                                <thead>
                                    <tr>
                                        <th sortname='原材料编号' style="width: 150px;" class="tdOperar_原材料编号">原材料编号<span><img src="../Img/bg.gif" id="Img1" /></span>
                                        </th>
                                        <th sortname='原材料描述' style="width: 150px;" class="tdOperar_原材料描述">原材料描述<span><img src="../Img/bg.gif" id="Img16" /></span>
                                        </th>
                                         <th sortname='型号' style="width: 150px;" class="tdOperar_型号">型号<span><img src="../Img/bg.gif" id="Img3" /></span>
                                        </th>
                                        <th sortname='最小起订量' class="tdOperar_最小起订量">最小起订量<span><img src="../Img/bg.gif" id="Img17" /></span>
                                        </th>
                                        <th sortname='库存数量' class="tdOperar_库存数量">库存数量<span><img src="../Img/bg.gif" id="Img18" /></span>
                                        </th>
                                          <th sortname='实际库存数量' class="tdOperar_实际库存数量">实际库存数量<span><img src="../Img/bg.gif" id="Img18" /></span>
                                        </th>

                                        <th sortname='在途数量' class="tdOperar_在途数量">在途数量<span><img src="../Img/bg.gif" id="Img19" /></span>
                                        </th>
                                        <th sortname='实际需求数' class="tdOperar_实际需求数">实际需求数<span><img src="../Img/bg.gif" id="Img20" /></span>
                                        </th>
                                        <th sortname='安全库存数量' class="tdOperar_安全库存数量">安全库存数量<span><img src="../Img/bg.gif" id="Img21" /></span>
                                        </th>
                                        <th sortname='订单需求数' class="tdOperar_订单需求数">订单需求数<span><img src="../Img/bg.gif" id="Img2" /></span>
                                        </th>
                                         <th sortname='未审核出库单数量' style="width: 100px;" class="tdOperar_未审核出库单数量">未审核出库单数量<span><img src="../Img/bg.gif" id="Img4" /></span>
                                        </th>
                                        <th sortname='计算结果' class="tdOperar_计算结果">计算结果<span><img src="../Img/bg.gif" id="Img22" /></span>
                                        </th>
                                        
                                        <th sortname='平衡结果' style="width: 100px;" class="tdOperar_平衡结果">平衡结果<span><img src="../Img/bg.gif" id="Img23" /></span>
                                        </th>
                                       
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
                                        <td colspan="10" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px; padding-right: 10px;">
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
    </form>
</body>
</html>
