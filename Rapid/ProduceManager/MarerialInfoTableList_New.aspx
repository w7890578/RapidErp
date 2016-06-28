<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarerialInfoTableList_New.aspx.cs"
    Inherits="Rapid.ProduceManager.MarerialInfoTableList_New" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>原材料信息列表</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        * {
            margin: 0;
            padding: 0;
        }

        body {
            font: 14px Verdana, Arial, Helvetica, sans-serif;
        }

        ul {
            list-style: none;
        }

        #tab {
            margin: 15px auto;
            width: 100%;
        }

            #tab ul {
                overflow: hidden;
                zoom: 1;
            }

            #tab li {
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

                #tab li.on {
                    background: green;
                    color: White;
                    font-size: 16px;
                    font-weight: bold;
                }

        #content {
            border-top: 4px solid green;
            background-color: #f3ffe3;
        }

            #content div {
            }

                #content div.show {
                    display: block;
                }
    </style>

    <script type="text/javascript">
        $(function () {
            $("#menus-tab li").click(function () {
                var result = $("#hdMarerial").val(); //选中的原材料
                if (result == "" || result == null) {
                    alert("请选择原材料！");
                    return;
                }
                var index = $(this).index('ul li'); //当前选中的li序号
                $(this).addClass("on").siblings().removeClass("on");
                // $("#content div:eq(" + index + ")").addClass("show").siblings().removeClass("show");

                var title = $(this).html();
                // var obj = $("#content div:eq(" + index + ")");
                $("#gongyingshang").hide();
                $("#kehu").hide();
                $("#tuzhi").hide();
                $("#yuancailiao").hide();
                if (title == "供应商属性") {

                    $("#gongyingshang").show().html("<iframe style='position: relative; background-color: transparent;' width='100%' height='500' frameborder='0' src='MaterialSupplierPropertyList.aspx?Id=" + result + "' </iframe>");
                }
                else if (title == "客户属性") {
                    $("#kehu").show().html("<iframe style='position: relative; background-color: transparent;' width='100%' height='500' frameborder='0' src='MaterialCustomerPropertyList.aspx?Id=" + result + "' </iframe>");
                }
                else if (title == "图纸属性") {
                    $("#tuzhi").show().html("<iframe style='position: relative; background-color: transparent;' width='100%' height='500' frameborder='0' src='MaterialBlueprintPropertyList.aspx?Id=" + result + "' </iframe>");

                }
                else {
                    $("#yuancailiao").show();
                }

            });
        })
    </script>

    <script type="text/javascript">

        //排序字段
        var sortname = "原材料编号";
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
            var materialnumber = $("#MaterialNumber").val();

            var materialname = $("#MaterialName").val();

            var brand = $("#Brand").val();

            var description = $("#Description").val();
            var kind = $("#txtKind").val();
            var type = $("#txtType").val();
            var numberproperties = $("#txtNumberProperties").val();
            var unit = $("#txtUnit").val();
            var HWtype = $("#txtHWtype").val();
            //            var materialposition = $("#MaterialPosition").val();
            //            var MaterialPosition = $("#MaterialPosition").find("option:selected").text();
            //            var scrapposition = $("#ScrapPosition").val();
            //            var ScrapPosition = $("#ScrapPosition").find("option:selected").text();

            if (materialnumber != "" && materialnumber != null) {
                condition += " and (原材料编号 like '%" + materialnumber + "%' or 原材料编号 like '%" + materialnumber + "' or 原材料编号 like '" + materialnumber + "%')  ";
            }
            if (materialname != "" && materialname != null) {
                condition += " and (名称 like '%" + materialname + "%' or 名称 like '%" + materialname + "' or 名称 like '" + materialname + "%')  ";
            }
            if (kind != "" && kind != null) {
                condition += " and (种类 like '%" + kind + "%' or 种类 like '%" + kind + "' or 种类 like '" + kind + "%')  ";
            }
            if (type != "" && type != null) {
                condition += " and (类别 like '%" + kind + "%' or 类别 like '%" + kind + "' or 类别 like '" + kind + "%')  ";
            }
            if (brand != "" && brand != null) {
                condition += " and (品牌 like '%" + brand + "%' or 品牌 like '%" + brand + "' or 品牌 like '" + brand + "%') ";
            }
            //            if (materialposition != "" && materialposition != null) {
            //                condition += " and 原材料仓位='" + MaterialPosition + "' ";
            //            }
            //            if (scrapposition != "" && scrapposition != null) {
            //                condition += " and 废品仓位='" + ScrapPosition + "' ";
            //            }

            if (description != "" && description != null) {
                condition += " and (描述 like '%" + description + "%' or 描述 like '%" + description + "' or 描述 like '" + description + "%') ";

            }
            if (numberproperties != "") {
                condition += " and 编号属性 like '%" + numberproperties + "%'";
            }
            if (unit != "") {
                condition += " and 单位 like '%" + unit + "%'";
            }
            if (HWtype != "") {
                condition += " and 货物类型 like '%" + HWtype + "%'";
            }
            return condition;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select 原材料编号,采购价格 from V_MarerialInfoTable  ";
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
            querySql = "select * from V_MarerialInfoTable  ";
            querySql = querySql + " " + GetQueryCondition();
            $.ajax({
                type: "Get",
                url: "MarerialInfoTableList_New.aspx",
                data: { time: new Date(), pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql },
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
                            //选项卡用到的  
                            $("#hdMarerial").val($(this).find("td:eq(2)").html());
                            $("#lbMarerial").html($(this).find("td:eq(2)").html());
                        });
                        $(".tablesorter tbody tr:even").hover(function () {
                            $(this).find("td").css("background-color", "#EAFCD5");
                        }, function () {
                            $(this).find("td").css("background-color", "white");
                        });
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='19' align='center'>  查无数据 </td> </tr>";
                            $(".tablesorter tbody").append(tempStr);
                            //分页清空
                            $("#pageing").html("");
                        }

                        $("#pages").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='19' align='center'>  查无数据 </td> </tr>";
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


        $(document).ready(function () {


            //查询
            $("#btnSearch").click(function () {
                GetData(1, sortname, sortdirection);
            });

            //删除
            $("#btnDelete").click(function () {
                var i = 0;
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = encodeURIComponent(this.value) + "," + checkResult;
                    i = i + 1;
                });
                if (checkResult == "") {
                    alert("请选择要删除的行！");
                    return;
                }

                //                if (i > 50) {
                //                    alert("一次至多允许删除50条记录");
                //                    return;
                //                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定删除选择的数据?")) {
                    //通用删除
                    DeleteData("../ProduceManager/MarerialInfoTableList_New.aspx", ConvertsContent(checkResult), "btnSearch");
                }
            });

            //绑定排序事件和样式
            var obj = $(".tablesorter thead tr th");
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

            //全选/反选
            $(".tablesorter thead tr td input").click(function () {
                $("input[name='subBox']").each(function () {
                    // $(this).attr("checked", !$(this).attr("checked")); //全选、全不选
                    this.checked = !this.checked; //整个反选
                });
            });

            $("#btnAdd").click(function () {
                OpenDialog("../ProduceManager/AddOrEditMarerialInfoTable.aspx", "btnSearch", "550", "600");
            });

            //批量导入
            $("#btnImp").click(function () {
                //OpenDialog("ImpMarerialInfoTable.aspx", "btnSearch", "320", "500");
                window.location.href = "ImpMarerialInfoTable.aspx";
            });

            //批量导入价格
            $("#btnPrice").click(function () {
                window.location.href = "../SellManager/MaterialPrice.aspx";
            });

            //进入页面加载数据
            $("#btnSearch").click();
            //绑定原材料编号
            BindSelect("MaterialId", "MaterialNumber");
            //绑定原材料名称
            BindSelect("MaterialName", "MaterialName");
            //绑定原材料品牌
            BindSelect("Brand", "Brand");
            //            //绑定原材料种类
            //            BindSelect("MaterialKind", "Kind");
            //            //绑定原材料类别
            //            BindSelect("MaterialType", "Type");
            //            $(function() {
            //                $("#Kind").change(function() {
            //                    var valueKind = $("#Kind").val();
            //                    //这个时候ajax请求本页面得到 级联对象内容 例如：得到内容为result
            //                    $.ajax({
            //                        type: "Get",
            //                        url: "../ProduceManager/MarerialInfoTableList_New.aspx?time=" + new Date(),
            //                        data: { ChoosedVlaue: valueKind },
            //                        success: function(result) {
            //                            $("#Type").html(result);
            //                        }
            //                    });

            //                });
            //            })
            //绑定原材料描述
            BindSelect("Description", "Description");
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 1900px">
            <input type="hidden" id="saveInfo" runat="server" />
            <input type="hidden" id="hdMarerial" runat="server" />
            <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
                <img src="../Img/loading.gif" alt="loading" />
            </div>
            <div style="margin-top: 5px; text-align: center;">
                原材料:<label id="lbMarerial" style="color: Green;"></label>
            </div>
            <div style="padding-right: 10px;">
                <div id="tab">
                    <ul id="menus-tab">
                        <li class="on">原材料列表</li>
                        <li>供应商属性</li>
                        <li>客户属性</li>
                        <li>图纸属性</li>
                    </ul>
                    <div id="content">
                        <div class="show" id="yuancailiao">
                            <table class="pg_table">
                                <tr>
                                    <td colspan="8" style="line-height: 10px; height: 10px;">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>原材料编号：<input type="text" id="MaterialNumber" />
                                    </td>
                                    <td>型&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号：<input type="text" id="MaterialName" />
                                    </td>
                                    <td>品&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;牌：
                                    <input type="text" id="Brand" />
                                    </td>
                                    <td>编号属性：<input type="text" id="txtNumberProperties" />
                                    </td>
                                    <td>货物类型：<input type="text" id="txtHWtype" />
                                    </td>
                                    <td style="width: 300px;"></td>
                                </tr>
                                <tr>
                                    <td colspan="8" style="line-height: 4px; height: 4px;">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>种&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;类：<input type="text" id="txtKind" />
                                    </td>
                                    <td>类&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;别：<input type="text" id="txtType" />
                                    </td>
                                    <td>描&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;述：
                                    <input type="text" id="Description" />
                                    </td>
                                    <td>单&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;位：<input type="text" id="txtUnit" />
                                    </td>
                                    <td style="width: 300px;"></td>
                                </tr>
                                <tr>
                                    <td colspan="8">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td colspan="2">
                                        <div style="vertical-align: middle">
                                            <div style="float: left;">
                                                每页显示条数：
                                            <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                                maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="100" />
                                                &nbsp;&nbsp; &nbsp;&nbsp;
                                            </div>
                                        </div>
                                        <div>
                                            <div style="float: left; width: 65px;">
                                                <input type="button" value="查询" id="btnSearch" class="button" />
                                            </div>
                                            <% if (Rapid.ToolCode.Tool.GetUserMenuFunc("L0112", "Add"))
                                               {  %>
                                            <div style="float: left; width: 65px;" id="divAdd" runat="server">
                                                <input type="button" value="增加" id="btnAdd" class="button" />
                                            </div>
                                            <% } %>
                                            <% if (Rapid.ToolCode.Tool.GetUserMenuFunc("L0112", "Delete"))
                                               {  %>
                                            <div style="float: left; width: 65px;" id="divDelete" runat="server">
                                                <input type="button" value="删除" id="btnDelete" class="button" />
                                            </div>
                                            <%} %>
                                            <div style="float: left; width: 65px; display: none;" id="divPrint" runat="server">
                                                <input type="button" value="打印" id="btnPrint" class="button" onclick="doPrint('form1', 'btnPrint', 'btnAdd', 'btnDelete')" />
                                            </div>
                                            <% if (Rapid.ToolCode.Tool.GetUserMenuFunc("L0112", "Imp"))
                                               {  %>
                                            <div style="float: left; width: 65px;" id="divImp" runat="server">
                                                <input type="button" value="导入信息" id="btnImp" class="button" />
                                            </div>
                                            <%} %>
                                            <div style="float: left; width: 105px; display: none;" id="divPrice" runat="server">
                                                <input type="button" value="导入价格" id="btnPrice" class="button" />
                                            </div>
                                            <div style="float: left; width: 65px; display: none;" id="divExp" runat="server">
                                                <asp:Button ID="Button1" runat="server" Text="导出价格Excel" OnClick="Button1_Click"
                                                    OnClientClick="return ImpExecl()" CssClass="button" />
                                            </div>
                                        </div>
                                    </td>
                                    <td colspan="5" style="text-align: left"></td>
                                </tr>
                                <tr>
                                    <td colspan="19" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 5px; padding-right: 5px;">
                                        <div id="pages" class="pages clearfix">
                                        </div>
                                    </td>
                                </tr>
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
                                                        <th sortname='原材料编号'>原材料编号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                                        </th>
                                                        <th sortname='名称' style="width: 150px;">型号<span><img src="../Img/bg.gif" id="Img8" /></span>
                                                        </th>
                                                        <th sortname='描述' style="width: 150px;">描述<span><img src="../Img/bg.gif" id="Img5" /></span>
                                                        </th>
                                                        <th sortname='种类'>种类<span><img src="../Img/bg.gif" id="Img2" /></span>
                                                        </th>
                                                        <th sortname='类别'>类别<span><img src="../Img/bg.gif" id="Img6" /></span>
                                                        </th>
                                                        <td>品牌
                                                        </td>
                                                        <th sortname='三个月库存安全值'>库存安全值<span><img src="../Img/bg.gif" id="Img7" /></span>
                                                        </th>
                                                        <%--<th sortname='6个月库存安全值'>
                                                        6个月库存安全值<span><img src="../Img/bg.gif" id="Img13" /></span>
                                                    </th>--%>
                                                        <%--  <th sortname='采购价格'>
                                                        采购价格<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                    </th>--%>
                                                        <%--
                                                    <th sortname='原材料仓位'>
                                                        原材料仓库<span><img src="../Img/bg.gif" id="Img9" /></span>
                                                    </th>--%>
                                                        <th sortname='最小包装'>最小包装<span><img src="../Img/bg.gif" id="Img1" /></span>
                                                        </th>
                                                        <th sortname='最小起订量'>最小起订量<span><img src="../Img/bg.gif" id="Img3" /></span>
                                                        </th>
                                                        <th sortname='货位'>货位<span><img src="../Img/bg.gif" id="Img12" /></span>
                                                        </th>
                                                        <th sortname='货物类型'>货物类型<span><img src="../Img/bg.gif" id="Img4" /></span>
                                                        </th>
                                                        <th sortname='单位'>单位<span><img src="../Img/bg.gif" id="Img11" /></span>
                                                        </th>
                                                        <th sortname='编号属性'>编号属性<span><img src="../Img/bg.gif" id="Img9" /></span>
                                                        </th>
                                                        <td>备注
                                                        </td>
                                                        <td>操作
                                                        </td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td colspan="19" align="center">暂无数据
                                                        </td>
                                                    </tr>
                                                </tbody>
                                                <tfoot>
                                                    <tr>
                                                        <td colspan="19" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 5px; padding-right: 5px;">
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
                        <div id="gongyingshang" style="display: none;">
                            供应商属性
                        </div>
                        <div id="kehu" style="display: none;">
                            客户属性
                        </div>
                        <div id="tuzhi" style="display: none;">
                            图纸属性
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
