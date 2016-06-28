<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TableList.Master" AutoEventWireup="true"
    CodeBehind="WarehouseMarerialDeatilList.aspx.cs" Inherits="Rapid.StoreroomManager.WarehouseMarerialDeatilList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        //排序字段
        var sortname = "MaterialNumber";
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


        function Edit(marielNumber, warehouseId, tableName) {
            //OpenDialog("../StoreroomManager/EditMaterialStockQty.aspx?marielNumber=" + marielNumber + "&warehouseId=" + warehouseId + "&tableName=" + tableName, "btnSearch", "200", "500");
            //var ulr = "../StoreroomManager/EditMaterialStockQty.aspx?marielNumber=" + marielNumber + "&warehouseId=" + warehouseId + "&tableName=" + tableName;

            window.showModalDialog("" + "../StoreroomManager/EditMaterialStockQty.aspx?marielNumber=" + marielNumber + "&warehouseId=" + warehouseId + "&tableName=" + tableName, "", "dialogWidth:500px;dialogHeight:200px;status:no");
            GetData(pageindex, sortname, sortdirection);
        }
        //导出Execl前将查询条件内容写入隐藏标签
        function ExpExecl() {
            querySql = "  ";
            querySql = querySql + " " + GetQueryCondition();
            $("#saveInfo").val(querySql + "");
            return true;
        }



        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            var description = $("#txtDescription").val();
            var materialnumber = $("#txtMaterialNumber").val();
            var kind = $("#txtKind").val();
            var type = $("#txtType").val();
            var unit = $("#txtUnit").val();
            var numberproperties = $("#txtNumberProperties").val();
            var productType = $.trim($("#txtProductType").val());
            var brand = $.trim($("#txtBrand").val());
            var materialName = $.trim($("#txtmaterialName").val());
            var cargo = $.trim($("#txtCargo").val());

            if (description != '') {
                condition += " and a.Description like '%" + description + "%'";
            }
            if (materialnumber != "") {
                condition += " and a.MaterialNumber like '%" + materialnumber + "%'";
            }
            if (kind != "") {
                condition += " and a.Kind like '%" + kind + "%'";
            }
            if (type != "") {
                condition += " and a.Type like '%" + type + "%'";
            }
            if (unit != "") {
                condition += " and a.Unit like '%" + unit + "%'";
            }
            if (numberproperties != "") {
                condition += " and a.NumberProperties like '%" + numberproperties + "%'";
            }
            if (productType != "") {
                condition += " and a.CargoType like '%" + productType + "%'";
            }
            if (brand != "") {
                condition += " and a.Brand like '%" + brand + "%'";
            }
            if (materialName != "") {
                condition += " and a.materialName like '%" + materialName + "%'";
            }
            if (cargo != "") {
                condition += "  and a.Cargo like '%" + cargo + "%' ";
            }
            return condition;
        }

        //获取数据
        function GetData(pageIndex, sortName, sortDirection) {
            //获取一页显示行数
            pageSize = $("#txtPageSize").val();
            if (pageSize == "" || isNaN(pageSize)) {
                alert("请正确输入每页显示条数");
                return;
            }
            var number = getQueryString("number");
           // var name = getQueryString("warehouseName");
            //$("#divTitle").html(name);

            //querySql = "   select * from V_QuoteInfo_List  ";
            // querySql = querySql + " " + GetQueryCondition();
            $.ajax({
                type: "Get",
                url: "WarehouseMarerialDeatilList.aspx",
                data: { time: new Date(), pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, condition: GetQueryCondition(), number: number },
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

                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='14' align='center'>  查无数据 </td> </tr>";
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
            $("#navHead").html("&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;库房管理&nbsp;&nbsp;>&nbsp;&nbsp;仓库信息&nbsp;&nbsp;>&nbsp;&nbsp;仓库详细信息");
            $("#divOutMain").width(1450);
            //查询
            $("#btnSearch").click(function () {
                GetData(1, sortname, sortdirection);
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

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();

            $("#btnBack").click(function () {
                window.location.href = "WarehouseInfoList.aspx";
            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <input type="hidden" id="saveInfo" runat="server" />
        <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
            <img src="../Img/loading.gif" alt="loading" />
        </div>
        <div style="width: 100%; text-align: center; font-size: 15px;" id="divTitle">
            <%=Request["warehouseName"] %>
        </div>
        <table class="pg_table">
            <tr>
                <td style="display: <%=show %>;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 原材料编号：<input id="txtMaterialNumber" type="text" />
                </td>
                <td>描&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 述：<input id="txtDescription" type="text" />&nbsp;
                </td>
                <td>编号属性：<input id="txtNumberProperties" type="text" />&nbsp;
                </td>
                <td>品&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 牌：<input id="txtBrand" type="text" />&nbsp
                </td>
            </tr>
            <tr>
                <td style="display: <%=show %>;">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 种&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    类：<input id="txtKind" type="text" />
                </td>
                <td>类&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;别：<input id="txtType" type="text" />&nbsp;
                </td>
                <td>货物类型：<input type="text" id="txtProductType" />
                </td>
                <td>&nbsp;单&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;位：<input id="txtUnit" type="text" />&nbsp;
                </td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td colspan="8">&nbsp;
                </td>
            </tr>
            <tr>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;型&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;号：<input type="text" id="txtmaterialName" />
                </td>
                <td> 货&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;位：<input type="text" id="txtCargo" /></td>
                <td colspan="5" style="text-align: left">
                    <div style="vertical-align: middle">
                        <div style="float: left; width: 150;">
                            每页显示条数：
                            <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                                type="text" style="width: 60px;" id="txtPageSize" value="100" maxlength="3" />
                            &nbsp;&nbsp;
                        </div>
                    </div>
                    <div>
                        <div style="float: left; width: 65px;">
                            <input type="button" value="查询" id="btnSearch" class="button" />
                        </div>
                        <div style="float: left; width: 95px; height: 21px;">
                            <asp:Button runat="server" Text="导出Excel" OnClientClick="return ExpExecl()" ID="btnExp" OnClick="btnExp_Click" class="button" />
                        </div>
                        <div style="float: left; width: 65px;">
                            <input type="button" value="返回" id="btnBack" class="button" />
                        </div>
                        <div style="float: left; width: 125px;">
                            <a href="ImpMaterialSafeValue.aspx" target="_blank" style="color: blue;">导入原材料库存安全值</a>
                        </div>
                    </div>
                </td>
                <td></td>
            </tr>
            <tr>
                <td colspan="8">
                    <div>
                        <table class="tablesorter" cellpadding="1" cellspacing="1" width="1220px">
                            <thead>
                                <tr>
                                    <th  sortname='序号' style="display: none;">序号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                        id="Img10" /></span>
                                    </th>
                                    <th sortname='MaterialNumber'>原材料编号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                    </th>
                                    <th sortname='MaterialName' style="width: 150px;">型号<span><img src="../Img/bg.gif" id="Img1" /></span>
                                    </th>

                                    <th sortname='Description' style="width: 150px;">描述<span><img src="../Img/bg.gif" id="Img5" /></span>
                                    </th>
                                    <th sortname='Brand'>品牌<span><img src="../Img/bg.gif" id="Img2" /></span>
                                    </th>
                                    <th sortname='Cargo'>货位<span><img src="../Img/bg.gif" id="Img7" /></span>
                                    </th>
                                    <th sortname='StockQty'>数量<span><img src="../Img/bg.gif" id="Img3" /></span>
                                    </th>
                                    <%if (show == "inline")
                                      { %>
                                    <td style="width: 150px;">库存安全值
                                    </td>
                                    <%} %>

                                    <td>种类
                                    </td>
                                    <td>类别
                                    </td>
                                    <td>单位
                                    </td>
                                    <td>编号属性
                                    </td>
                                    <td>货物类型
                                    </td>
                                    <td>操作
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="8" align="center">暂无数据
                                    </td>
                                </tr>
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="<%=colSpan%>" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px; padding-right: 10px;">
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
</asp:Content>
