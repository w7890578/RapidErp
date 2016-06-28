<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TableList.Master" AutoEventWireup="true"
    CodeBehind="CertificateOrdersList.aspx.cs" Inherits="Rapid.PurchaseManager.CertificateOrdersList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>采购订单列表</title>

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#divOutMain").width(1620);
            $("#navHead").html("&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;采购管理&nbsp;&nbsp;>&nbsp;&nbsp;采购单列表");
        });
        //排序字段
        var sortname = "CreateTime";
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
            var ordersnumber = $("#txtOrdersNumber").val();
            var paymentmode = $("#slPaymentMode").val();
            var suppliername = $("#txtSupplierId").val();
            var username = $("#txtContactId").val();
            var orderDate = $("#txtOrderDate").val();
            if (ordersnumber != "") {
                condition += " and OrdersNumber like '%" + ordersnumber + "%'";
            }
            if (paymentmode != "") {
                condition += " and PaymentMode='" + paymentmode + "'";
            }
            if (suppliername != "") {
                condition += " and SupplierName like '%" + suppliername + "%'";
            }
            if (username != "") {
                condition += " and USER_NAME like '%" + username + "%'";
            }
            if (orderDate != "") {
                condition += " and OrdersDate ='" + orderDate + "' ";
            }
            return condition;
        }

        function GetCondition() {
            var condidtion = "where 1=1";
            var materiNumber = $("#txtMateriNumber").val();
            var supplierMaterialNumber = $("#txtSupplierMaterialNumber").val();
            if (materiNumber != "") {
                condidtion += " and MaterialNumber like '%" + materiNumber + "%'";
            }
            if (supplierMaterialNumber != "") {
                condidtion += " and SupplierMaterialNumber like '%" + supplierMaterialNumber + "%'";
            }
            return condidtion;
        }

        //导出Execl前将查询条件内容写入隐藏标签
        function ImpExecl() {
            querySql = " select * from V_CertificateOrders  ";
            querySql = querySql + " " + GetQueryCondition();
            $("#saveInfo").val(querySql + "");
            return true;
        }
        //获取数据
        function GetData(pageIndex, sortName, sortDirection) {

            //获取一页显示行数
            pageSize = $("#ctl00_ContentPlaceHolder1_txtPageSize").val();
            if (pageSize == "" || isNaN(pageSize)) {
                alert("请正确输入每页显示条数");
                return;
            }
            querySql = " select * from V_CertificateOrders ";
            querySql = querySql + " " + GetQueryCondition();

            $.ajax({
                type: "Get",
                url: "CertificateOrdersList.aspx",
                data: { time: new Date(), pageIndex: pageIndex, pageSize: pageSize, sortName: sortName, sortDirection: sortDirection, querySql: querySql, conditionTwo: GetCondition() },
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
                        //总行数
                        totalRecords = tempArray[3];
                        $(".tablesorter tbody tr").click(function() {
                            $(this).find("input[type='checkbox']").each(function() {
                                this.checked = !this.checked; //整个反选
                            });
                        });
                        $(".tablesorter tbody tr").hover(function() {
                            $(this).find("td").css("background-color", "#EAFCD5");
                        }, function() {
                            $(this).find("td").css("background-color", "white");
                        });

                        if (tempArray[1] == "") {
                            //如果没有数据
                            var tempStr = " <tr> <td colspan='12' align='center'>  查无数据 </td> </tr>";
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
            pageSize = $("#ctl00_ContentPlaceHolder1_txtPageSize").val();
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
                    DeleteData("../PurchaseManager/CertificateOrdersList.aspx", ConvertsContent(checkResult), "btnSearch");
                }
            });
            //审核
            $("#btnAuditor").click(function() {
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
                    Check("../PurchaseManager/CertificateOrdersList.aspx", ConvertsContent(checkResult), "btnSearch");
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
                $("input[name='subBox']").each(function() {
                    // $(this).attr("checked", !$(this).attr("checked")); //全选、全不选
                    this.checked = !this.checked; //整个反选
                });
            });

            $("#btnAdd").click(function() {
                //           
                OpenDialogWithscroll("AddOrEditCertificateOrdersList.aspx", "btnSearch", "320", "700");
            });

            //绑定
            tablesorter("tablesorter");
            //进入页面加载数据
            $("#btnSearch").click();
            //            BindSelect("OrdersNm", "slOrdersNumber");
            BindSelect("PaymentMode", "slPaymentMode");
            //            BindSelect("SupplierNm", "slSupplierId");
            //            BindSelect("USER_NAME", "slContactId");
            $("#btnImp").click(function() {
                window.location.href = "ImpCertificateOrdersNew.aspx";

            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server" >
    <input type="hidden" id="hdSortName" runat="server" />
    <input type="hidden" id="hdSortDirection" runat="server" />
    <div style="width :97%;"></div>
    <table class="pg_table" >
        <tr style="margin-top: 5px">
            <td class="pg_talbe_head">
                采购订单号：
            </td>
            <td class="pg_talbe_content">
                <%--<select id="slOrdersNumber" style="width: 160px;">
                    <option value="">- - - - - 请 选 择 - - - - -</option>
                </select>--%>
                <input type="text" id="txtOrdersNumber" />
            </td>
            <td class="pg_talbe_head">
                付款方式：
            </td>
            <td class="pg_talbe_content">
                <select id="slPaymentMode" style="width: 160px;">
                    <option value="">- - - - - 请 选 择 - - - - -</option>
                </select>
            </td>
            <td class="pg_talbe_head">
                供应商：
            </td>
            <td class="pg_talbe_content">
                <%-- <select id="slSupplierId" style="width: 160px;">
                    <option value="">- - - - - 请 选 择 - - - - -</option>
                </select>--%>
                <input type="text" id="txtSupplierId" />
            </td>
            <td class="pg_talbe_head">
                业务员：
            </td>
            <td class="pg_talbe_content">
                <%--<select id="slContactId" style="width: 160px;">
                    <option value="">- - - - - 请 选 择 - - - - -</option>
                </select>--%>
                <input type="text" id="txtContactId" />
            </td>
            <td>
            </td>
        </tr>
        <tr style="margin-top: 5px">
            <td class="pg_talbe_head">
                订单日期：
            </td>
            <td class="pg_talbe_content">
                <input type="text" onfocus="WdatePicker({skin:'green'})" id="txtOrderDate" />
            </td>
            <td class="pg_talbe_head">
                原材料编号：
            </td>
            <td class="pg_talbe_content">
                <input type="text" id="txtMateriNumber" />
            </td>
            <td class="pg_talbe_head">
                供应商物料编号:
            </td>
            <td class="pg_talbe_content">
                <input type="text" id="txtSupplierMaterialNumber" />
            </td>
            <td class="pg_talbe_head">
            </td>
            <td class="pg_talbe_content">
            </td>
            <td>
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
            <td>
            </td>
            <td colspan="5" style="text-align: left">
                <div style="vertical-align: middle">
                    <div style="float: left; width: 150;">
                        每页显示条数：
                        <input onkeyup="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                            onafterpaste="if(this.value.length==1){this.value=this.value.replace(/[^1-9]/g,'')}else{this.value=this.value.replace(/\D/g,'')}"
                            maxlength="3" type="text" style="width: 60px;" id="txtPageSize" value="30" runat="server" />
                        &nbsp;&nbsp;
                    </div>
                </div>
                <div>
                    <div style="float: left; width: 65px;">
                        <input type="button" value="查询" id="btnSearch" class="button" />
                    </div>
                    <div style="float: left; width: 65px;" id="divAdd" runat="server">
                        <input type="button" value="增加" id="btnAdd" class="button" />
                    </div>
                    <div style="float: left; width: 65px;" id="divAuditor" runat="server">
                        <input type="button" value="审核" id="btnAuditor" class="button" />
                    </div>
                    <div style="float: left; width: 85px;" id="div1" runat="server">
                        <input type="button" value="导入订单" id="btnImp" class="button" />
                    </div>
                    <div style="float: left; width: 65px;" id="divDelete" runat="server">
                        <input type="button" value="删除" id="btnDelete" class="button" />
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="10">
                <div>
                    <table class="tablesorter" cellpadding="1" cellspacing="1" style="width:100%;">
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
                                <th sortname='OrdersNumber'>
                                    采购订单编号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                                </th>
                                <th sortname='OrdersNumber'>
                                    采购合同号<span><img src="../Img/bg.gif" id="Img9" /></span>
                                </th>
                                <th sortname='OrdersDate'>
                                    采购订单日期<span><img src="../Img/bg.gif" id="Img8" /></span>
                                </th>
                                <th sortname='PaymentMode'>
                                    付款方式<span><img src="../Img/bg.gif" id="Img1" /></span>
                                </th>
                                <th sortname='SupplierName'>
                                    供应商<span><img src="../Img/bg.gif" id="Img2" /></span>
                                </th>
                                <th sortname='USER_NAME'>
                                    业务员<span><img src="../Img/bg.gif" id="Img6" /></span>
                                </th>
                                <th sortname='OrderStatus'>
                                    订单状态<span><img src="../Img/bg.gif" id="Img3" /></span>
                                </th>
                                <th sortname='Auditor'>
                                    审核人<span><img src="../Img/bg.gif" id="Img5" /></span>
                                </th>
                                <th sortname='CheckTime'>
                                    审核时间<span><img src="../Img/bg.gif" id="Img7" /></span>
                                </th>
                                <th sortname='CreateTime'>
                                    创建时间<span><img src="../Img/bg.gif" id="Img4" /></span>
                                </th>
                                <td style="width:200px;">
                                    备注
                                </td>
                                <td>
                                    操作
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="13" align="center" class="style2">
                                    暂无数据
                                </td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <td colspan="13" style="background-color: #F3FFE3; padding-top: 10px; padding-left: 10px;
                                    padding-right: 10px;">
                                    <div id="pageing" class="pages clearfix" >
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
