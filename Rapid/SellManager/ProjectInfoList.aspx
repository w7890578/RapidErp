<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TableList.Master" AutoEventWireup="true"
    CodeBehind="ProjectInfoList.aspx.cs" Inherits="Rapid.SellManager.ProjectInfoList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#navHead").html("&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;销售管理&nbsp;&nbsp;>&nbsp;&nbsp;<a href='ProjectMain.aspx'>项目信息</a>>&nbsp;&nbsp;项目信息详细列表");

            $("#btnBack").click(function() {
                window.location.href = "ProjectMain.aspx";
            });
        })

        //排序字段
        var sortname = "项目名称";
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
        var name = getQueryString("Name");
        function GetQueryCondition() {
            var condition = " where 项目名称='" + name + "' ";
            var productNumber = $.trim($("#txtProductNumber").val());
            var customerProductNumber = $.trim($("#txtCustomerProductNumber").val());
            var description = $.trim($("#txtDescription").val());
            if (productNumber != "") {
                condition += " and (产成品编号 like '%" + productNumber + "' or 产成品编号 like '" + productNumber + "%' or 产成品编号 like '%" + productNumber + "%')";
            }
            if (customerProductNumber != "") {
                condition += " and (客户产成品编号 like '%" + customerProductNumber + "' or 客户产成品编号 like '" + customerProductNumber + "%' or 客户产成品编号 like '%" + customerProductNumber + "%')";
            }
            if (description != "") {
                condition += " and (描述 like '%" + description + "' or 描述 like '" + description + "%' or 描述 like '%" + description + "%')";

            }
            return condition;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = "   select * from V_T_ProjectInfo  ";
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
            querySql = "   select * from V_T_ProjectInfo  ";
            querySql = querySql + " " + GetQueryCondition();
            $.ajax({
                type: "Get",
                url: "ProjectInfoList.aspx",
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
                        $(".tablesorter tbody tr").hover(function() {
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
                            var tempStr = " <tr> <td colspan='10' align='center'>  查无数据 </td> </tr>";
                            $(".tablesorter tbody").append(tempStr);
                            //分页清空
                            $("#pageing").html("");
                        }

                        $("#pages").html(tempArray[2]);
                        //总行数
                        totalRecords = tempArray[3];
                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='10' align='center'>  查无数据 </td> </tr>";
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
                    DeleteData("../SellManager/ProjectInfoList.aspx", checkResult, "btnSearch");
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
                OpenDialog("../SellManager/AddProjectInfoList.aspx", "btnSearch", "290", "750");
            });
            //导入
            $("#btnImp").click(function() {
                //OpenDialog("../ProduceManager/ImpT_ProjectInfo.aspx", "btnSearch", "410", "700");
                window.location.href = "../ProduceManager/ImpT_ProjectInfo.aspx";
            });
            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();

            // BindSelect("CustomerName", "slCustomer");
        });
      
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
        <img src="../Img/loading.gif" alt="loading" />
    </div>
    <table class="pg_table">
        <tr>
            <td>
                &nbsp;&nbsp;&nbsp;&nbsp;产成品编号（瑞普迪编号）：<input type="text" id="txtProductNumber" />
            </td>
            <td>
                &nbsp;&nbsp; &nbsp;&nbsp; 客户产成品编号：<input type="text" id="txtCustomerProductNumber" />
            </td>
            <td>
                &nbsp;&nbsp; &nbsp;&nbsp; 描述：<input type="text" id="txtDescription" />
            </td>
            <td colspan="5">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="8">
                &nbsp;
            </td>
        </tr>
        <tr>
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
                            type="text" style="width: 60px;" id="txtPageSize" value="500" maxlength="3" />
                        &nbsp;&nbsp;</div>
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
                    <div style="float: left; width: 65px;" id="divImp" runat="server">
                        <input type="button" value="导入" id="btnImp" class="button" />
                    </div>
                    <div style="float: left; width: 65px;" id="divBack" runat="server">
                        <input type="button" value="返回" id="btnBack" class="button" />
                    </div>
                </div>
            </td>
            <td>
            </td>
        </tr>
          <tr>
                                <td colspan="10" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
                                    padding-right: 10px;">
                                    <div id="pages" class="pages clearfix">
                                    </div>
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
                                <th sortname='项目名称'>
                                    项目名称<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                </th>
                                <th sortname='阶层'>
                                    阶层<span><img src="../Img/bg.gif" id="Img1" /></span>
                                </th>
                                <th sortname='产成品编号'>
                                    产成品编号<span><img src="../Img/bg.gif" id="Img8" /></span>
                                </th>
                                <th sortname='客户产成品编号'>
                                    客户产成品编号<span><img src="../Img/bg.gif" id="Img4" /></span>
                                </th>
                                <td>
                                    版本
                                </td>
                                <th sortname='描述'>
                                    描述<span><img src="../Img/bg.gif" id="Img5" /></span>
                                </th>
                                <td>
                                    单机
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
                                <td colspan="10" align="center">
                                    暂无数据
                                </td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="10" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
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
</asp:Content>
