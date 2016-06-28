<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="Rapid.ProduceManager.ProductList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>产品基本信息列表</title>
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
    <style type="text/css">
        *
        {
            margin: 0;
            padding: 0;
        }
        body
        {
            font: 14px Verdana, Arial, Helvetica, sans-serif;
        }
        ul
        {
            list-style: none;
        }
        #tab
        {
            margin: 15px auto;
            width: 100%;
        }
        #tab ul
        {
            overflow: hidden;
            zoom: 1;
        }
        #tab li
        {
            float: left;
            margin-right: 8px;
            width: 200px;
            height: 30px;
            line-height: 30px;
            border: 1px solid green;
            border-bottom: 0;
            cursor: pointer;
            text-align: center;
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
        }
        #tab li.on
        {
            background: green;
            color: White;
            font-size: 16px;
            font-weight: bold;
        }
        #content
        {
            border-top: 4px solid green;
            background-color: #f3ffe3;
        }
        #content div
        {
        }
        #content div.show
        {
            display: block;
        }
    </style>

    <script type="text/javascript">
        $(function() {
            $("#menus-tab li").click(function() {
                var result = $("#hdProduct").val(); //选中的产成品
                var version = $("#hdVersion").val();
                var type = $("#hdType").val();
                if (result == "" || result == null || version == "" || version == null) {
                    alert("请选择产成品、版本！");
                    return;
                }
                var index = $(this).index('ul li'); //当前选中的li序号
                $(this).addClass("on").siblings().removeClass("on");
                // $("#content div:eq(" + index + ")").addClass("show").siblings().removeClass("show");

                var title = $(this).html();
                // var obj = $("#content div:eq(" + index + ")");

                $("#bominfo").hide();
                $("#kehu").hide();
                $("#tuzhi").hide();
                $("#productwork").hide();
                $("#chanchengpin").hide();

                result = escape(result);
                version = escape(version);
                if (title == "BOM信息表") {
                    if (type == "包") {
                        $("#bominfo").show().html("<iframe id='tempbominfo' style='position: relative; background-color: transparent;' width='100%' height='800' frameborder='0' src='PackageBom.aspx?Id=" + result + "' </iframe>");
                    }
                    else {

                        $("#bominfo").show().html("<iframe id='tempbominfo' style='position: relative; background-color: transparent;' width='100%' height='500' frameborder='0' src='BOMInfoList.aspx?Id=" + result + "&version=" + version + "' </iframe>");

                    }
                }
                else if (title == "客户属性") {
                    $("#kehu").show().html("<iframe style='position: relative; background-color: transparent;' width='100%' height='500' frameborder='0' src='ProductCustomerPropertyList.aspx?Id=" + result + "&version=" + version + "' </iframe>");
                }
                else if (title == "图纸属性") {
                    $("#tuzhi").show().html("<iframe style='position: relative; background-color: transparent;' width='100%' height='500' frameborder='0' src='ProductBlueprintPropertyList.aspx?Id=" + result + "&version=" + version + "' </iframe>");

                } else if (title == "产品工序") {
                    $("#productwork").show().html("<iframe style='position: relative; background-color: transparent;' width='100%' height='500' frameborder='0' src='ProductWorkSnPropertyList.aspx?Id=" + result + "&version=" + version + "' </iframe>");

                }
                else {
                    $("#chanchengpin").show();
                }

            });
        })
    </script>

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

        //获取查询条件
        function GetQueryCondition() {
            var condition = "where 1=1";
            var productnumber = $("#ProductNumber").val();
            var productname = $("#ProductName").val();
            var productDescription = $("#ProductDescription").val();
            var unit = $("#txtUnit").val();
            var NumberProperties = $("#txtNumberProperties").val();
            var type = $("#Type").val();
            var Type = $("#Type").find("option:selected").text();
            if (productnumber != "" && productnumber != null) {
                condition += " and ( 产成品编号 like '%" + productnumber + "%' or 产成品编号 like '%" + productnumber + "' or 产成品编号 like '" + productnumber + "%')";
            }

            if (productname != "" && productname != null) {
                condition += "and(  名称 like '%" + productname + "%' or 名称 like '%" + productname + "' or 名称 like '" + productname + "%')";
            }

            if (productDescription != "" && productDescription != null) {
                condition += "and(  描述 like '%" + productDescription + "%' or 描述 like '%" + productDescription + "' or 描述 like '" + productDescription + "%')";

            }
            if (type != "" && type != null) {
                condition += " and (类别='" + Type + "') ";
            }
            if (unit != "") {
                condition += " and 单位 like '%" + unit + "%'";
            }
            if (NumberProperties != "") {
                condition += " and 编号属性 like '%" + NumberProperties + "%'";
            }
            return condition;
        }
        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_Product  ";
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
            querySql = " select * from V_Product ";
            querySql = querySql + " " + GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "ProductList.aspx",
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
                        $("#pages").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        $(".tablesorter tbody tr").click(function() {
                            $(this).find("input[type='checkbox']").each(function() {
                                this.checked = !this.checked; //整个反选
                            });
                            //选项卡用到的  
                            $("#hdProduct").val($(this).find("td:eq(2)").html());
                            $("#lbProduct").html($(this).find("td:eq(2)").html());

                            $("#hdVersion").val($(this).find("td:eq(3)").html());
                            $("#lbVersion").html($(this).find("td:eq(3)").html());
                            $("#hdType").val($(this).find("td:eq(5)").html());

                        });
                        //                        $(".tablesorter tbody tr:even").hover(function() {
                        //                            $(this).find("td").css("background-color", "#EAFCD5");
                        //                        }, function() {
                        //                            $(this).find("td").css("background-color", "white");
                        //                        });

                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='17' align='center'>  查无数据 </td> </tr>";
                            $(".tablesorter tbody").append(tempStr);
                            //分页清空
                            $("#pageing").html("");
                        }
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='17' align='center'>  查无数据 </td> </tr>";
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
                var checkResultProductNumber = "";
                var checkResultVersion = "";
                var arrChkProductNumber = $("input[name='subBoxProductNumber']:checked");
                var arrChkVersion = $("input[name='subBoxVersion']:checked");
                $(arrChkProductNumber).each(function() {
                    checkResultProductNumber = this.value + "," + checkResultProductNumber;
                });
                $(arrChkVersion).each(function() {
                    checkResultVersion = this.value + "," + checkResultVersion;
                });
                if (checkResultProductNumber == "" || checkResultVersion == "") {
                    alert("请选择要删除的行！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResultProductNumber = checkResultProductNumber.replace(reg, "");
                checkResultVersion = checkResultVersion.replace(reg, "");
                //这是获取的值  
                if (confirm("确定删除选择的数据?")) {
                    //通用删除
                    DeleteDataProperty("../ProduceManager/ProductList.aspx", ConvertsContent(checkResultVersion), ConvertsContent(checkResultProductNumber), "btnSearch");
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
                $("input[name='subBoxProductNumber']").each(function() {
                    // $(this).attr("checked", !$(this).attr("checked")); //全选、全不选
                    this.checked = !this.checked; //整个反选
                });
                $("input[name='subBoxVersion']").each(function() {
                    // $(this).attr("checked", !$(this).attr("checked")); //全选、全不选
                    this.checked = !this.checked; //整个反选
                });
            });

            $("#btnAdd").click(function() {
                //window.location.href = "AddOrEditProduct.aspx";
                OpenDialog("../ProduceManager/AddOrEditProduct.aspx", "btnSearch", "460", "600");
            });

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();

            $("#btnImp").click(function() {
                //OpenDialog("ImpProduct.aspx", "btnSearch", "320", "500");
                window.location.href = "ImpProductBom.aspx";
            });
            BindSelect("ProductType", "Type");
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 1550px;">
        <input type="hidden" id="saveInfo" runat="server" />
        <input type="hidden" id="hdProduct" runat="server" />
        <input type="hidden" id="hdVersion" runat="server" />
        <input type="hidden" id="hdType" runat="server" />
        <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            <img src="../Img/loading.gif" alt="loading" />
        </div>
        <div style="margin-top: 5px; text-align: center;">
            产成品:<label id="lbProduct" style="color: Green;"></label>&nbsp;&nbsp;&nbsp;&nbsp;
            版本:<label id="lbVersion" style="color: Green;"></label>
        </div>
        <div style="padding-right: 10px;">
            <div id="tab">
                <ul id="menus-tab">
                    <li class="on">产成品列表</li>
                    <li>客户属性</li><li>BOM信息表</li>
                    <li>图纸属性</li>
                    <li>产品工序</li>
                </ul>
                <div id="content" style="margin-bottom: 10px;">
                    <div class="show" id="chanchengpin">
                        <table class="pg_table">
                            <tr>
                                <td colspan="5">
                                </td>
                            </tr>
                            <tr>
                                <td class="pg_talbe_head">
                                    型&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号：
                                </td>
                                <td class="pg_talbe_content">
                                    <input type="text" id="ProductName" style="width: 160px;" />
                                </td>
                                <td class="pg_talbe_head">
                                    产成品编号：
                                </td>
                                <td class="pg_talbe_content">
                                    <input type="text" id="ProductNumber" style="width: 160px;" />
                                </td>
                                <td class="pg_talbe_head">
                                    描&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;述：
                                </td>
                                <td class="pg_talbe_content">
                                    <input type="text" id="ProductDescription" style="width: 160px;" />
                                </td>
                                <td class="pg_talbe_head">
                                    类别：
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
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;单&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 位：
                                </td>
                                <td class="pg_talbe_content">
                                    <input type="text" id="txtUnit" style="width: 160px;" />
                                </td>
                                <td class="pg_talbe_head">
                                    &nbsp;&nbsp;&nbsp; 编号属性：
                                </td>
                                <td class="pg_talbe_content">
                                    <input type="text" id="txtNumberProperties" style="width: 160px;" />
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <label style="color: #FF8B2F; font-weight: bold;">
                                        橘黄色显示的行为旧版本</label>
                                    &nbsp;&nbsp;&nbsp;&nbsp; 名词解释：产成品编号（瑞普迪自己的产成品编号）
                                </td>
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
                                         
                                        <div style="float: left; width: 65px;display :<%=(Session["User_Func"] as System.Collections.Generic.List<string>).Contains("L0113|Add")?"inline": "none"%>;" id="divAdd" runat="server">
                                            <input type="button" value="增加" id="btnAdd" class="button" />
                                        </div>
                                        <div style="float: left; width: 65px;display :<%=(Session["User_Func"] as System.Collections.Generic.List<string>).Contains("L0113|Delete")?"inline": "none"%>;" id="divDelete" runat="server">
                                            <input type="button" value="删除" id="btnDelete" class="button" />
                                        </div>
                                        <div style="float: left; width: 65px;" id="divPrint" runat="server">
                                            <input style="display: none;" type="button" value="打印" id="btnPrint" class="button"
                                                onclick="doPrint('form1','btnPrint','btnAdd','btnDelete')" />
                                        </div>
                                        <div style="float: left; width: 65px;display :<%=(Session["User_Func"] as System.Collections.Generic.List<string>).Contains("L0113|Imp")?"inline": "none"%>;" id="divImp" runat="server">
                                            <input type="button" value="导入" id="btnImp" class="button" />
                                        </div>
                                        <div style="float: left; width: 65px;" id="div1" runat="server">
                                          <%--  <asp:Button ID="btnExpWrokSn" runat="server" Text="导出产品工序工时" CssClass="button" 
                                                onclick="btnExpWrokSn_Click" />--%>
                                        </div>
                                        <div style="float: left; width: 65px;display :<%=(Session["User_Func"] as System.Collections.Generic.List<string>).Contains("L0113|Exp")?"inline": "none"%>;" id="divExp" runat="server">
                                            <asp:Button ID="Button1" Visible="false" runat="server" Text="导出Excel" OnClick="Button1_Click"
                                                OnClientClick="return ImpExecl()" CssClass="button" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="17" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
                                    padding-right: 10px;">
                                    <div id="pages" class="pages clearfix">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10">
                                    <div>
                                        <table class="tablesorter" cellpadding="1" cellspacing="1">
                                            <thead>
                                                <tr>
                                                    <td>
                                                        <label style="width: 101%; display: block; cursor: pointer; height: 24px;">
                                                            <input type="checkbox" />全选/反选</label>
                                                    </td>
                                                    <th sortname='产成品编号'>
                                                        产成品编号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                                    </th>
                                                    <th sortname='版本'>
                                                        版本<span><img src="../Img/bg.gif" id="Img8" /></span>
                                                    </th>
                                                    <th sortname='名称'>
                                                        型号<img src="../Img/bg.gif" id="Img10" /></span>
                                                    </th>
                                                    <th sortname='类别'>
                                                        成品类别<img src="../Img/bg.gif" id="Img11" /></span>
                                                    </th>
                                                    <th sortname='额定工时'>
                                                        额定工时(分钟)<span><img src="../Img/bg.gif" id="Img1" /></span>
                                                    </th>
                                                    <th sortname='报价工时'>
                                                        报价工时(分钟)<span><img src="../Img/bg.gif" id="Img2" /></span>
                                                    </th>
                                                    <th sortname='成本价'>
                                                        标准成本价<span><img src="../Img/bg.gif" id="Img3" /></span>
                                                    </th>
                                                    <th sortname='描述' style="width: 100px;">
                                                        描述<span><img src="../Img/bg.gif" id="Img5" /></span>
                                                    </th>
                                                    <th sortname='半成品仓位'>
                                                        半成品仓位<span><img src="../Img/bg.gif" id="Img6" /></span>
                                                    </th>
                                                    <th sortname='产成品仓位'>
                                                        产成品仓位<span><img src="../Img/bg.gif" id="Img7" /></span>
                                                    </th>
                                                    <th sortname='货位'>
                                                        货位<span><img src="../Img/bg.gif" id="Img12" /></span>
                                                    </th>
                                                    <td>
                                                        单位
                                                    </td>
                                                    <td>
                                                        编号属性
                                                    </td>
                                                    <td style="width: 100px;">
                                                        备注
                                                    </td>
                                                    <td>
                                                        操作
                                                    </td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td colspan="17" align="center">
                                                        暂无数据
                                                    </td>
                                                </tr>
                                            </tbody>
                                            <tfoot>
                                                <tr>
                                                    <td colspan="17" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
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
                    <div id="bominfo" style="display: none;">
                        BOM信息表
                    </div>
                    <div id="kehu" style="display: none;">
                        客户属性
                    </div>
                    <div id="tuzhi" style="display: none;">
                        图纸属性
                    </div>
                    <div id="productwork" style="display: none;">
                        产品工序
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
