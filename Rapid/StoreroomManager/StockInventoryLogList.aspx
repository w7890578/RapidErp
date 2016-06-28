<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TableList.Master" AutoEventWireup="true"
    CodeBehind="StockInventoryLogList.aspx.cs" Inherits="Rapid.StoreroomManager.StockInventoryLogList" %>

<%--李敏  3月13号--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function TransferForQuoteInfo(number) {
            number = $.trim(number);
            window.location.href = "StockInventoryLogDetail.aspx?InventoryNumber=" + number;
        }
        $(function () {
            $("#navHead").html("&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;库存管理&nbsp;&nbsp;>&nbsp;&nbsp;库存盘点列表");
        })
    </script>

    <script type="text/javascript">
        //排序字段
        var sortname = "盘点编号";
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

            var InventoryNumber = $("#InventoryNumber").val();
            //var InventoryNumber = $("#InventoryNumber").find("option:selected").text();

            var warehouseName = $("#WarehouseName").val();
            var WarehouseName = $("#WarehouseName").find("option:selected").text();

            var inventoryType = $("#InventoryType").val();
            var InventoryType = $("#InventoryType").find("option:selected").text();


            if (InventoryNumber != "" ) {
                condition += " and 盘点编号 like '%" + InventoryNumber + "%' ";
            }
            if (warehouseName != "" && warehouseName != null) {
                condition += " and 仓库名称='" + WarehouseName + "' ";
            }
            if (inventoryType != "" && inventoryType != null) {
                condition += " and 盘点类型='" + InventoryType + "' ";
            }


            return condition;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = "   select  * from [V_StockInventoryLog]  ";
            querySql = querySql + " " + GetQueryCondition();
            $("#ctl00_ContentPlaceHolder1_saveInfo").val(querySql + "");
            //$("#progressBar").show();
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
            querySql = "   select * from V_StockInventoryLog  ";
            querySql = querySql + " " + GetQueryCondition();
            
            $.ajax({
                type: "Get",
                url: "AjaxGetStockInventoryLog.aspx",
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
                        //                        $(".tablesorter tbody tr:odd").hover(function() {
                        //                            $(this).find("td").css("background-color", "yellow");
                        //                        }, function() {
                        //                            $(this).find("td").css("background-color", "#EAFCD5");
                        //                        });
                        $("#pageing").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='11' align='center'>  查无数据 </td> </tr>";
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
                if (confirm("确定删除选择的数据?")) {
                    // alert(ConvertsContent(checkResult));
                    //通用删除
                    DeleteData("../StoreroomManager/AjaxGetStockInventoryLog.aspx", ConvertsContent(checkResult), "btnSearch");
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
                OpenDialog("../StoreroomManager/AddOrEditStockInventoryLog.aspx", "btnSearch", "180", "600");
            });

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();

            //绑定盘点编号
            //            BindSelect("StockInventoryLogInventoryNumber", "InventoryNumber");
            //绑定仓库名称
            BindSelect("StockInventoryLogWarehouseName", "WarehouseName");
            //绑定盘点类型
            BindSelect("StockInventoryLogInventoryType", "InventoryType");
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input type="hidden" id="saveInfo" runat="server" />
     
    <table class="pg_table">
        <tr>
            <td class="pg_talbe_head">仓库名称：
            </td>
            <td class="pg_talbe_content">
                <select id="WarehouseName">
                    <option value="">- - - - - 请 选 择 - - - - -</option>
                </select>
            </td>
            <td class="pg_talbe_head">盘点类型：
            </td>
            <td class="pg_talbe_content">
                <select id="InventoryType">
                    <option value="">- - - - - 请 选 择 - - - - -</option>
                </select>
            </td>
            <td class="pg_talbe_head">盘点编号：
            </td>
            <td class="pg_talbe_content">
                <input type="text" id="InventoryNumber" />
            </td>
            <td class="pg_talbe_head"></td>
            <td class="pg_talbe_content"></td>
        </tr>
        <tr>
            <td colspan="8">&nbsp;
            </td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
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
                    <div style="float: left; width: 65px;" id="divAdd" runat="server">
                        <input type="button" value="盘点" id="btnAdd" class="button" />
                    </div>
                    <div style="float: left; width: 65px;" id="divDelete" runat="server">
                        <input type="button" value="删除" id="btnDelete" class="button" />
                    </div>
                    <div style="float: left; height: 24px; display: <%=Rapid.ToolCode.Tool.GetUserMenuFunc("L0405", "Exp")%>;" id="divExp"  >
                        <asp:Button ID="Button1" runat="server" Text="导出Excel" OnClientClick="return ImpExecl()"
                            CssClass="button" OnClick="Button1_Click1" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <div>
                    <table class="tablesorter" cellpadding="1" cellspacing="1" width="1220px;">
                        <thead>
                            <tr>
                                <td nowrap>
                                    <label style="width: 100%; display: block; cursor: pointer;">
                                        <input type="checkbox" />全选/反选</label>
                                </td>
                                <th nowrap sortname='序号' style="display: none;">序号<span style="text-align: center; float: right; margin-top: 7px;"><img src="../Img/bg.gif"
                                    id="Img10" /></span>
                                </th>
                                <th nowrap sortname='仓库名称'>仓库名称<span><img src="../Img/bg.gif" id="Img1" /></span>
                                </th>
                                <th nowrap sortname='操作人'>操作人<span><img src="../Img/bg.gif" id="Img2" /></span>
                                </th>
                                <th nowrap sortname='盘点类型'>盘点类型<span><img src="../Img/bg.gif" id="Img5" /></span>
                                </th>
                                <th nowrap sortname='盘点时间'>盘点时间<span><img src="../Img/bg.gif" id="Img8" /></span>
                                </th>
                                <td nowrap>审核人
                                </td>
                                <th nowrap sortname='审核时间'>审核时间<span><img src="../Img/bg.gif" id="Img3" /></span>
                                </th>
                                <td nowrap>备注
                                </td>
                                <th nowrap sortname='盘点编号'>盘点编号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                </th>
                                <td nowrap>编辑
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
</asp:Content>
