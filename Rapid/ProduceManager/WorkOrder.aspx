<%@ Page Title="" Language="C#" MasterPageFile="~/Master/TableList.Master" AutoEventWireup="true"
    CodeBehind="WorkOrder.aspx.cs" Inherits="Rapid.ProduceManager.WorkOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <style type="text/css">
        .tilte {
            font-size: 17px;
            font-weight: bold;
            color: Blue;
        }

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

        .bgGreen {
            background-color: #EAFCD5;
        }

        .tdYellow {
            background-color: Yellow;
        }
    </style>

    <script type="text/javascript">
        //查看具体缺料情况
        function Check(productNumber, version, qty, customerProductNumber) {
            //OpenDialog("../ProduceManager/QueLiaoDetail.aspx?productNumber=" + productNumber + "&version=" + version + "&qty=" + qty, "btnSearch", "750", "600");

            window.location.href = "../ProduceManager/QueLiaoDetail.aspx?productNumber=" + productNumber + "&version=" + version + "&qty=" + qty + "&customerProductNumber=" + customerProductNumber;
        }
        //编辑实际生产数量
        function EditQty(id, qty) {
            OpenDialog("../ProduceManager/EditWorkOrderQty.aspx?Id=" + id + "&qty=" + qty, "btnSearch", "350", "400");
        }

        window.onscroll = function () {
            if (document.documentElement.scrollTop + document.body.scrollTop > 20) {
                //document.getElementById("scrollto").style.display = "block";
                $("#divGXTitile").addClass("bgGreen");
            }
            else {
                // document.getElementById("scrollto").style.display = "none";
                $("#divGXTitile").removeClass("bgGreen");
            }
            // 
        }


        $(function () {
            $("#divOutMain").width(1500);
            var setType = "zz";
            $("#navHead").html("&nbsp;&nbsp;首页&nbsp;&nbsp;>&nbsp;&nbsp;生产管理&nbsp;&nbsp;>&nbsp;&nbsp;生成开工单");
            $("#btnNextOne").click(function () {

                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要开工的产品！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                $("#hdProductId").val(ConvertsContent(checkResult));
                //alert($("#hdProductId").val());
                $("#divOne").hide();
                $("#divTwo").show();
                $("#showStep").html("开工单生成进度：已选需要开工的产品。");
            });

            //算料
            $("#btnSL").click(function () {
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要开工的产品！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //window.location.href = "?";
                //  $.cookie("SLText", checkResult,{  path: 'SL.aspx' }); //设置cookie 名字，值

                // window.location.href = "SL.aspx";

                $.ajax({
                    type: "Post",
                    url: "AjaxWirteSession.aspx",
                    data: { keyName: "SLText", value: checkResult },
                    beforeSend: function () { },
                    success: function (result) {
                        //window.location.href = "";
                        window.open("SL.aspx");
                    }
                })
            });


            //第二步
            $("#btnNextTwo").click(function () {
                var productIds = $("#hdProductId").val();
                var format = $("#ctl00_ContentPlaceHolder1_drpFormat").find("option:selected").text();
                $("#showStep").html("开工单生成进度：已选需要开工的产品》选择了开工单格式为\"" + format + "\"。");
                var check = $("#ctl00_ContentPlaceHolder1_drpFormat").val();
                if (check == "xz") { //小组开工单
                    $("#divTwo").hide();
                    $("#divThree").show();
                }
                else {//工序开工单
                    //                    $.get("WorkOrder.aspx", { IsLoadGX: "true", Ids: productIds, time: new Date() }, function(result) {
                    //                        if (result != "") {
                    //                            $("#divGX").show();
                    //                            $("#divTwo").hide();
                    //                            $("#tbGX tbody").html("");
                    //                            $("#tbGX tbody").append(result);
                    //                            $("#tbGX tbody tr").click(function() {
                    //                                $(this).find("input[type='checkbox']").each(function() {
                    //                                    this.checked = !this.checked; //整个反选
                    //                                });
                    //                                var id = $(this).attr("Id");
                    //                                var obj = $("." + id + "");
                    //                                if (obj.length > 0)
                    //                                { $("." + id + "").toggle(); }

                    //                            }).hover(function() {
                    //                                var tempCss = $(this).find("td").css("background-color");

                    //                                if (tempCss != "rgb(234, 252, 213)") {
                    //                                    $(this).find("td").css("background-color", "yellow");
                    //                                }

                    //                            }, function() {
                    //                                var tempCss = $(this).find("td").css("background-color");
                    //                                if (tempCss == "rgb(255, 255, 0)") {
                    //                                    $(this).find("td").css("background-color", "White");
                    //                                }
                    //                            });
                    //                        }
                    //                        else {
                    //                            alert("数据更新错误！请与技术人员联系");
                    //                            return;
                    //                        }

                    //                    });


                    $.ajax({
                        type: "Post",
                        url: "WorkOrder.aspx",
                        data: { IsLoadGX: "true", Ids: productIds, time: new Date() },
                        beforeSend: function () { },
                        success: function (result) {
                            if (result != "") {
                                $("#divGX").show();
                                $("#divTwo").hide();
                                $("#tbGX tbody").html("");
                                $("#tbGX tbody").append(result);
                                $("#tbGX tbody tr").click(function () {
                                    $(this).find("input[type='checkbox']").each(function () {
                                        this.checked = !this.checked; //整个反选
                                    });
                                    var id = $(this).attr("Id");
                                    var obj = $("." + id + "");
                                    if (obj.length > 0)
                                    { $("." + id + "").toggle(); }

                                }).hover(function () {
                                    var tempCss = $(this).find("td").css("background-color");

                                    //                                    if (tempCss != "rgb(234, 252, 213)") {
                                    //                                        $(this).find("td").css("background-color", "green");
                                    //                                    }

                                }, function () {
                                    var tempCss = $(this).find("td").css("background-color");
                                    //                                    if (tempCss == "rgb(255, 255, 0)") {
                                    //                                        $(this).find("td").css("background-color", "White");
                                    //                                    }
                                });
                            }
                            else {
                                alert("数据更新错误！请与技术人员联系");
                                return;
                            }
                        }
                    })

                }
            });
            //工序上一步
            $("#btnGXUp").click(function () {
                $("#divGX").hide();
                $("#divTwo").show();
            });
            $("#btnUpTwo").click(function () {
                $("#divOne").show();
                $("#divTwo").hide();

            });

            $("#btnUpThree").click(function () {
                $("#divTwo").show();
                $("#divThree").hide();
                var format = $("#ctl00_ContentPlaceHolder1_drpFormat").find("option:selected").text();


            });
            //第三步
            $("#btnNextThree").click(function () {
                var format = $("#ctl00_ContentPlaceHolder1_drpFormat").val();
                if (format == "xz") //小组开工单
                {
                    if ($("#ctl00_ContentPlaceHolder1_drpIsSplit").val() == "是") //拆分
                    {
                        $("#divXZ").show();
                        $("#divThree").hide();
                        $("#zzTeamDiv").show();

                        var productId = $("#hdProductId").val();
                        //                        $.get("WorkOrder.aspx", { ProductId: productId, time: new Date() }, function(html) {
                        //                            $("#tbZZXz tbody").html("");
                        //                            $("#tbZZXz tbody").append(html);
                        //                             
                        //                            $("#tbZZXz tbody tr").click(function() {
                        //                                $(this).find("input[type='checkbox']").each(function() {
                        //                                    this.checked = !this.checked; //整个反选
                        //                                });
                        //                            });
                        //                        }); 
                        $.ajax({
                            type: "Post",
                            url: "WorkOrder.aspx",
                            data: { ProductId: productId, time: new Date() },
                            beforeSend: function () { },
                            success: function (html) {
                                $("#tbZZXz tbody").html("");
                                $("#tbZZXz tbody").append(html);

                                $("#tbZZXz tbody tr").click(function () {
                                    $(this).find("input[type='checkbox']").each(function () {
                                        this.checked = !this.checked; //整个反选
                                    });
                                });
                            }
                        })



                        //                        $.get("WorkOrder.aspx", { IsJY: "true", ProductId: productId, time: new Date() }, function(html) {
                        //                            $("#tbJYXz tbody").html("");
                        //                            $("#tbJYXz tbody").append(html);
                        //                            $("#tbJYXz tbody tr").click(function() {
                        //                                $(this).find("input[type='checkbox']").each(function() {
                        //                                    this.checked = !this.checked; //整个反选
                        //                                });
                        //                            });
                        //                        });


                        $.ajax({
                            type: "Post",
                            url: "WorkOrder.aspx",
                            data: { IsJY: "true", ProductId: productId, time: new Date() },
                            beforeSend: function () { },
                            success: function (html) {
                                $("#tbJYXz tbody").html("");
                                $("#tbJYXz tbody").append(html);
                                $("#tbJYXz tbody tr").click(function () {
                                    $(this).find("input[type='checkbox']").each(function () {
                                        this.checked = !this.checked; //整个反选
                                    });
                                });
                            }
                        })

                    }
                    else {
                        //不拆分
                        $("#divThree").hide();
                        $("#divFour").show();
                    }

                }
                else {
                    //工序开工单
                }
                var text = $("#ctl00_ContentPlaceHolder1_drpFormat").find("option:selected").text();
                var isSplit = $("#ctl00_ContentPlaceHolder1_drpIsSplit").val();
                if (isSplit == "是") {
                    isSplit = "需要拆分";
                }
                else {
                    isSplit = "不需要拆分";
                }
                $("#showStep").html("开工单生成进度：已选需要开工的产品》选择了开工单格式为\"" + text + "\"》" + isSplit + "。");
            });
            //设置
            $("#btnXZSet").click(function () {
                var team = ""; //班组
                var noProductId = ""; //选中的产品
                var checkResult = ""; //临时变量
                var arrChk;
                //制造
                if (setType == "zz") {
                    team = $("#ctl00_ContentPlaceHolder1_lbZZXZ").val();
                    arrChk = $("input[name='subBoxXZ']:checked");
                }
                else {
                    team = $("#ctl00_ContentPlaceHolder1_lbJYXZ").val();
                    arrChk = $("input[name='subBoxJYXZ']:checked");
                }

                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要开工的产品！");
                    return;
                }
                if (team == "" || team == null) {
                    alert("请选择班组");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                noProductId = ConvertsContent(checkResult);

                //                $.get("WorkOrder.aspx", { NoProductId: noProductId, Team: team, SetType: setType, time: new Date() }, function(html) {

                //                    if (setType == "zz") {
                //                        $("#tbZZXz tbody").html("");
                //                        $("#tbZZXz tbody").append(html);
                //                        $("#tbZZXz tbody tr").click(function() {
                //                            $(this).find("input[type='checkbox']").each(function() {
                //                                this.checked = !this.checked; //整个反选
                //                            });
                //                        });
                //                    }
                //                    else {
                //                        $("#tbJYXz tbody").html("");
                //                        $("#tbJYXz tbody").append(html);
                //                        $("#tbJYXz tbody tr").click(function() {
                //                            $(this).find("input[type='checkbox']").each(function() {
                //                                this.checked = !this.checked; //整个反选
                //                            });
                //                        });
                //                    }

                //                });
                $.ajax({
                    type: "Post",
                    url: "WorkOrder.aspx",
                    data: { NoProductId: noProductId, Team: team, SetType: setType, time: new Date() },
                    beforeSend: function () { },
                    success: function (html) {
                        if (setType == "zz") {
                            $("#tbZZXz tbody").html("");
                            $("#tbZZXz tbody").append(html);
                            $("#tbZZXz tbody tr").click(function () {
                                $(this).find("input[type='checkbox']").each(function () {
                                    this.checked = !this.checked; //整个反选
                                });
                            });
                        }
                        else {
                            $("#tbJYXz tbody").html("");
                            $("#tbJYXz tbody").append(html);
                            $("#tbJYXz tbody tr").click(function () {
                                $(this).find("input[type='checkbox']").each(function () {
                                    this.checked = !this.checked; //整个反选
                                });
                            });
                        }
                    }
                })

            });
            //第四步上一步
            $("#btnUpFour").click(function () {
                $("#divThree").show();
                $("#divFour").hide();

            });
            //第四步下一步
            $("#btnNextFour").click(function () {
                var team = $("#ctl00_ContentPlaceHolder1_drpTeam").val(); //选择的班组
                var productId = $("#hdProductId").val(); //第一步选择的产品
                //                $.get("WorkOrder.aspx", { Four: "true", Team: team, Numbers: productId, time: new Date() }, function(result) {
                //                    if (result != "") {
                //                        var arry = result.split("~");

                //                        $("#divXZ").show();
                //                        $("#divFour").hide();
                //                        $("#zzTeamDiv").hide();

                //                        $("#tbZZXz tbody").html("");
                //                        $("#tbZZXz tbody").append(arry[0]);
                //                        $("#tbZZXz tbody tr").click(function() {
                //                            $(this).find("input[type='checkbox']").each(function() {
                //                                this.checked = !this.checked; //整个反选
                //                            });
                //                        });

                //                        $("#tbJYXz tbody").html("");
                //                        $("#tbJYXz tbody").append(arry[1]);
                //                        $("#tbJYXz tbody tr").click(function() {
                //                            $(this).find("input[type='checkbox']").each(function() {
                //                                this.checked = !this.checked; //整个反选
                //                            });
                //                        });

                //                        $("#showStep").html("开工单生成进度：已选需要开工的产品》选择了开工单格式为\"小组开工单\"》不需要拆分》" + team + "组开工。");

                //                    }
                //                    else {
                //                        alert("更新数据错误！请联系技术人员");
                //                        return;
                //                    }
                //                })


                $.ajax({
                    type: "Post",
                    url: "WorkOrder.aspx",
                    data: { Four: "true", Team: team, Numbers: productId, time: new Date() },
                    beforeSend: function () { },
                    success: function (result) {
                        if (result != "") {
                            var arry = result.split("~");

                            $("#divXZ").show();
                            $("#divFour").hide();
                            $("#zzTeamDiv").hide();

                            $("#tbZZXz tbody").html("");
                            $("#tbZZXz tbody").append(arry[0]);
                            $("#tbZZXz tbody tr").click(function () {
                                $(this).find("input[type='checkbox']").each(function () {
                                    this.checked = !this.checked; //整个反选
                                });
                            });

                            $("#tbJYXz tbody").html("");
                            $("#tbJYXz tbody").append(arry[1]);
                            $("#tbJYXz tbody tr").click(function () {
                                $(this).find("input[type='checkbox']").each(function () {
                                    this.checked = !this.checked; //整个反选
                                });
                            });

                            $("#showStep").html("开工单生成进度：已选需要开工的产品》选择了开工单格式为\"小组开工单\"》不需要拆分》" + team + "组开工。");

                        }
                        else {
                            alert("更新数据错误！请联系技术人员");
                            return;
                        }
                    }
                })

            });
            //小组上一步
            $("#btnXZUp").click(function () {
                $("#divXZ").hide();
                $("#divThree").show();
            });


            //            $("#tbOne tbody tr").click(function() {
            //                $(this).find("input[type='checkbox']").each(function() {
            //                    this.checked = !this.checked; //整个反选
            //                });
            //            });



            //全选/反选
            $("#tbZZXz thead tr td input").click(function () {
                $("input[name='subBoxXZ']").each(function () {
                    this.checked = !this.checked; //整个反选
                });
            });
            //全选/反选
            $("#tbJYXz thead tr td input").click(function () {
                $("input[name='subBoxJYXZ']").each(function () {
                    this.checked = !this.checked; //整个反选
                });
            });

            //全选/反选
            $("#tbGX thead tr td input").click(function () {
                $("input[name='subBoxGX']").each(function () {
                    this.checked = !this.checked; //整个反选
                    //$(this).attr("checked", !$(this).attr("checked"))
                });
            });
            //生成小组开工单
            $("#btnXZNext").click(function () {
                if (confirm("确定生成开工单？生成成功后无法返回上一步进行修改！")) {
                    $.get("WorkOrder.aspx?sq=" + new Date(), { time: new Date(), IsGenerateXZ: "true" }, function (result) {
                        if (result == "1") {
                            var i = 2;
                            $("#divShowResult").show();
                            setInterval(function () {
                                $("#divShowResult").html("开工单生成成功！" + i + "秒后跳转至开工单列表");
                                i--;
                            }, 1000);

                            setTimeout(function () { window.location.href = "ProductPlanList.aspx"; }, 3000);

                            return;
                        }
                        else {
                            alert(result);
                            return;
                        }
                    })
                }
            });
            //生成工序开工单
            $("#btnGenerateGX").click(function () {
                if (confirm("确定生成开工单？生成成功后无法返回上一步进行修改！")) {
                    $.get("WorkOrder.aspx?sq=" + new Date(), { time: new Date(), IsGenerateGX: "true" }, function (result) {
                        if (result == "1") {
                            var i = 2;
                            $("#divShowResult").show();
                            setInterval(function () {
                                $("#divShowResult").html("开工单生成成功！" + i + "秒后跳转至开工单列表");
                                i--;
                            }, 1000);

                            setTimeout(function () { window.location.href = "ProductPlanList.aspx"; }, 3000);
                            return;
                        }
                        else {
                            alert(result);
                            return;
                        }
                    })
                }
            });

            $("#menus-tab li").click(function () {
                var index = $(this).index('ul li'); //当前选中的li序号
                $(this).addClass("on").siblings().removeClass("on");
                var title = $(this).html();
                $("#zz").hide();
                $("#jy").hide();
                if (title == "制造组") {
                    $("#zz").show();
                    setType = "zz";
                }
                else if (title == "检验组") {
                    $("#jy").show();
                    setType = "jy";
                }


            });
            //设置工序班组
            $("#btnSetGX").click(function () {
                var team = $("#ctl00_ContentPlaceHolder1_liGXTeam").val(); //选择的班组
                var checkResult = "";
                var noProductId = "";
                var arrChk = $("input[name='subBoxGX']:checked");
                $(arrChk).each(function () {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要开工的产品！");
                    return;
                }
                if (team == null) {
                    alert("请选择班组");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                noProductId = ConvertsContent(checkResult);
                //                $.get("WorkOrder.aspx", { GXPRdouctId: noProductId, Team: team, time: new Date() }, function(result) {

                //                    if (result == "1") {
                //                        $(arrChk).each(function() {
                //                            // alert("#" + this.value + "");
                //                            $("#" + this.value + "").find("td:eq(1)").html(team);
                //                        });
                //                        $("input[name='subBoxGX']").each(function() {
                //                            $(this).attr("checked", false);
                //                        });
                //                    }
                //                    else {
                //                        alert("一个班组不能设置同一个产品超过四道工序");
                //                        return;
                //                    }

                //                    //                    if (result == "0") {
                //                    //                        alert("一个班组不能设置同一个产品超过四道工序");
                //                    //                        return;
                //                    //                    }
                //                    //                    else {
                //                    //                        $("#tbGX tbody").html("");
                //                    //                        $("#tbGX tbody").append(result);
                //                    //                        $("#tbGX tbody tr").click(function() {
                //                    //                            $(this).find("input[type='checkbox']").each(function() {
                //                    //                                this.checked = !this.checked; //整个反选
                //                    //                            });
                //                    //                            var id = $(this).attr("Id");
                //                    //                            $("." + id + "").toggle();
                //                    //                        });
                //                    //                    }
                //                });
                $.ajax({
                    type: "Post",
                    url: "WorkOrder.aspx",
                    data: { GXPRdouctId: noProductId, Team: team, time: new Date() },
                    beforeSend: function () { },
                    success: function (result) {
                        if (result == "1") {
                            $(arrChk).each(function () {
                                // alert("#" + this.value + "");
                                $("#" + this.value + "").find("td:eq(1)").html(team);
                            });
                            $("input[name='subBoxGX']").each(function () {
                                $(this).attr("checked", false);
                            });
                        }
                        else {
                            alert("一个班组不能设置同一个产品超过四道工序");
                            return;
                        }
                    }
                })

            });
        })
    </script>

    <script type="text/javascript">
        //开工单第一步列表

        //排序字段
        var sortname = "交期";
        //排序规则
        var sortdirection = "asc";

        //获取查询条件
        function GetQueryCondition() {
            var condition = " where 1=1 ";
            var customerProductNumber = $.trim($("#txtCustomerProductNumber").val());
            var orderNumber = $.trim($("#txtOrderNumber").val());
            var beginDate = $.trim($("#txtBeginDate").val());
            var endDate = $.trim($("#txtEndDate").val());
            var type = $.trim($("#txtType").val());
            if (customerProductNumber != "") {
                condition += "and t.客户产品编号 like '%" + customerProductNumber + "%'";
            }
            if (orderNumber != "") {
                condition += "and t.销售订单号 like '%" + orderNumber + "%'";
            }
            if (beginDate != "" && endDate != "") {
                condition += "and (t.交期>='" + beginDate + "' and t.交期<='" + endDate + "')";
            }
            if (type != "") {
                condition += "and so.OdersType like '%" + type + "%'";
            }
            return condition;
        }
        //获取数据
        function GetData(pageIndex, sortName, sortDirection) {
            $.ajax({
                type: "Get",
                url: "AjaxGetWorkOrder.aspx",
                data: { time: new Date(), sortName: sortName, sortDirection: sortDirection, condition: GetQueryCondition() },
                beforeSend: function () { $("#progressBar").show(); },
                success: function (result) {
                    //loading隐藏
                    $("#progressBar").hide();
                    //清空内容
                    $("#tbOne tbody").html("");
                    //如果有数据就追加
                    if (result != "") {
                        // alert(result);
                        $("#tbOne tbody").html(result);
                        $("#tbOne tbody tr").click(function () {
                            $(this).find("input[type='checkbox']").each(function () {
                                this.checked = !this.checked; //整个反选
                            });
                        });


                        $("#tbOne tbody tr").hover(function () {
                            $(this).find("td").css("background-color", "yellow");
                        }, function () {
                            $(this).find("td").css("background-color", "white");
                        });
                        //                        //全选/反选
                        //                        $("#tbOne thead tr td input").click(function() {
                        //                            $("input[name='subBox']").each(function() {
                        //                                this.checked = !this.checked; //整个反选
                        //                            });

                        //                        });
                    }
                    //                    $("#tbOne thead tr td input[type='checkbox']").attr("checked", false);

                }
            });
        }


        $(document).ready(function () {


            //绑定排序事件和样式
            function tablesorter(className) {
                var obj = $("#" + className + " thead tr th");
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

            //绑定
            tablesorter("tbOne");

            //查询
            $("#btnSearch").click(function () {
                GetData(1, sortname, sortdirection);
            });

            //进入页面加载数据
            GetData(1, sortname, sortdirection);
            //全选/反选
            $("#inputOne").click(function () {
                $("input[name='subBox']").each(function () {
                    this.checked = !this.checked; //整个反选
                });

            });

        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
        <img src="../Img/loading.gif" alt="loading" />
    </div>
    <div style="position: absolute; display: none; color: Green; top: 40%; left: 40%; width: 270px; background-color: White; border: 1px solid green; padding: 10px;"
        id="divShowResult">
        开工单生成成功！3秒后跳转至开工单列表
    </div>
    <input type="hidden" id="hdProductId" />
    <!--选择开工的产品-->
    <div id="divOne" style="text-align: center;">
        <span class="tilte ">请选择准备开工的产品</span> &nbsp;&nbsp;&nbsp;&nbsp;<input type="button"
            id="btnNextOne" value=" 下一步 " class="button" />
        <div style="margin-bottom: 10px; margin-top: 10px;">
            &nbsp;&nbsp;&nbsp;&nbsp;客户产成品编号：<input type="text" id="txtCustomerProductNumber" />
            &nbsp;&nbsp;销售订单号：<input type="text" id="txtOrderNumber" />&nbsp;&nbsp;开始日期：<input
                type="text" onfocus="WdatePicker({skin:'green'})" id="txtBeginDate" />
            &nbsp;&nbsp;结束日期：<input type="text" onfocus="WdatePicker({skin:'green'})" id="txtEndDate" />
            &nbsp;&nbsp;订单类型：<input type="text" id="txtType" />
        </div>
        <div style="text-align: center;">
            &nbsp;&nbsp;
            <input type="button" id="btnSearch" value=" 查 询 " class="button" />
            <input type="button" id="btnSL" value=" 算 料 " class="button" />
        </div>
        <table class="tablesorter" cellpadding="1" cellspacing="1" id="tbOne">
            <thead>
                <tr>
                    <td nowrap>
                        <input type="checkbox" id="inputOne" /><label style="cursor: pointer;">
                            全选/反选</label>
                    </td>
                    <th nowrap sortname='销售订单号'>销售订单号<span><img src="../Img/bg.gif" id="sortImg" /></span>
                    </th>
                    <th nowrap sortname='客户采购订单号'>客户采购订单号<span><img src="../Img/bg.gif" id="Img10" /></span>
                    </th>
                    <th nowrap sortname='订单类型'>订单类型<span><img src="../Img/bg.gif" id="Img11" /></span>
                    </th>
                    <th nowrap sortname='客户产品编号'>客户产成品编号<span><img src="../Img/bg.gif" id="Img1" /></span>
                    </th>
                    <th nowrap sortname='产品编号'>产品编号<span><img src="../Img/bg.gif" id="Img2" /></span>
                    </th>
                    <th nowrap sortname='版本'>版本<span><img src="../Img/bg.gif" id="Img3" /></span>
                    </th>
                    <th nowrap sortname='库存数量'>库存数量<span><img src="../Img/bg.gif" id="Img5" /></span>
                    </th>
                    <th nowrap sortname='未交货数量'>未交货数量<span><img src="../Img/bg.gif" id="Img4" /></span>
                    </th>

                    <td nowrap>实时库存数量</td>
                    <th nowrap sortname='在制品数量'>在制品数量<span><img src="../Img/bg.gif" id="Img6" /></span>
                    </th>
                    <th nowrap sortname='未入库数量'>未入库数量<span><img src="../Img/bg.gif" id="Img7" /></span>
                    </th>
                    <th nowrap sortname='需要生产数量'>需要生产数量<span><img src="../Img/bg.gif" id="Img8" /></span>
                    </th>
                    <th nowrap sortname='实际生产数量'>实际生产数量<span><img src="../Img/bg.gif" id="Img12" /></span>
                    </th>
                    <th nowrap sortname='交期'>交期<span><img src="../Img/bg.gif" id="Img9" /></span>
                    </th>
                    <th nowrap sortname='行号'>行号<span><img src="../Img/bg.gif" id="Img13" /></span>
                    </th>
                    <td nowrap>操作
                    </td>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
    <!--选择开工单格式-->
    <div id="divTwo" style="display: none; text-align: center;">
        <p class="tilte " style="margin-bottom: 20px;">
            请选择开工单格式
        </p>
        <span style="margin-left: 40px;">开工单格式：
            <asp:DropDownList runat="server" ID="drpFormat" Width="150px">
                <asp:ListItem Text="小组开工单" Value="xz"></asp:ListItem>
                <asp:ListItem Text="工序开工单" Value="gx"></asp:ListItem>
            </asp:DropDownList>
            <p style="margin-top: 20px;">
                <input type="button" id="btnUpTwo" value=" 上一步 " class="button" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" id="btnNextTwo" value=" 下一步 " class="button" />
            </p>
        </span>
    </div>
    <!--选择是否拆分-->
    <div id="divThree" style="display: none; text-align: center;">
        <p class="tilte" style="margin-bottom: 20px;">
            请选择是否需要拆分
        </p>
        <span style="margin-left: 40px;">是否拆分：
            <asp:DropDownList runat="server" ID="drpIsSplit" Width="150px">
                <asp:ListItem Text="是" Value="是"></asp:ListItem>
                <asp:ListItem Text="否" Value="否"></asp:ListItem>
            </asp:DropDownList>
        </span>
        <p style="margin-top: 20px;">
            <input type="button" id="btnUpThree" value=" 上一步 " class="button" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" id="btnNextThree" value=" 下一步 " class="button" />
        </p>
    </div>
    <!--选择不拆分则选择班组-->
    <div id="divFour" style="display: none; text-align: center;">
        <p class="tilte" style="margin-bottom: 20px;">
            请选择班组
        </p>
        <span style="margin-left: 40px;">班组：
            <asp:DropDownList runat="server" ID="drpTeam" Width="150px">
            </asp:DropDownList>
        </span>
        <p style="margin-top: 20px;">
            <input type="button" id="btnUpFour" value=" 上一步 " class="button" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" id="btnNextFour" value=" 下一步 " class="button" />
        </p>
    </div>
    <!---小组开工单-->
    <div id="divXZ" style="display: none; text-align: center; width: 1150px;">
        <p class="tilte ">
            设置小组开工单明细&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" id="btnXZSet" value=" 设 置 "
                class="button" />
            &nbsp;&nbsp;&nbsp;&nbsp;<input type="button" id="btnXZUp" value=" 上一步 " class="button" />
            &nbsp;&nbsp;&nbsp;&nbsp;<input type="button" id="btnXZNext" value=" 生成小组开工单 " class="button" />
        </p>
        <div style="padding-right: 10px;">
            <div id="tab">
                <ul id="menus-tab">
                    <li class="on">制造组</li>
                    <li>检验组</li>
                </ul>
                <div id="content">
                    <div class="show" id="zz">
                        <div style="float: left; margin: 10px 20px 0px 20px;" id="zzTeamDiv">
                            班组<br />
                            <asp:ListBox runat="server" ID="lbZZXZ" Width="100px" Height="150px"></asp:ListBox>
                        </div>
                        <table class="tablesorter" cellpadding="1" cellspacing="1" style="width: 1000px; float: left;"
                            id="tbZZXz">
                            <thead>
                                <tr>
                                    <td>
                                        <label style="cursor: pointer;">
                                            <input type="checkbox" />全选/反选</label>
                                    </td>
                                    <td>班组
                                    </td>
                                    <td>销售订单号
                                    </td>
                                    <td>客户产品编号
                                    </td>
                                    <td>产品编号
                                    </td>
                                    <td>版本
                                    </td>
                                    <td>未交数量
                                    </td>
                                    <td>库存数量
                                    </td>
                                    <td>在制品数量
                                    </td>
                                    <td>需要生产数量
                                    </td>
                                    <td>交期
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                    <div id="jy" style="display: none;">
                        <div style="float: left; margin: 10px 20px 0px 20px;">
                            班组<br />
                            <asp:ListBox runat="server" ID="lbJYXZ" Width="100px" Height="150px"></asp:ListBox>
                        </div>
                        <table class="tablesorter" cellpadding="1" cellspacing="1" style="width: 1000px; float: left;"
                            id="tbJYXz">
                            <thead>
                                <tr>
                                    <td>
                                        <label style="cursor: pointer;">
                                            <input type="checkbox" />全选/反选</label>
                                    </td>
                                    <td>班组
                                    </td>
                                    <td>销售订单号
                                    </td>
                                    <td>客户产品编号
                                    </td>
                                    <td>产品编号
                                    </td>
                                    <td>版本
                                    </td>
                                    <td>未交数量
                                    </td>
                                    <td>库存数量
                                    </td>
                                    <td>在制品数量
                                    </td>
                                    <td>需要生产数量
                                    </td>
                                    <td>交期
                                    </td>
                                </tr>
                            </thead>
                            <tbody>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!---工序开工单-->
    <div id="divGX" style="display: none; text-align: center; width: 1050px;">
        <div class="tilte" style="position: fixed; margin-left: 240px; margin-top: -25px;"
            id="divGXTitile">
            <span style="width: 165px;">设置工序开工单明细</span> &nbsp;&nbsp;&nbsp;&nbsp;<input type="button"
                id="btnGXUp" value=" 上一步 " class="button" />&nbsp;&nbsp;&nbsp;&nbsp;<input type="button"
                    id="btnSetGX" value=" 设 置 " class="button" />
            &nbsp;&nbsp;&nbsp;&nbsp;<input type="button" id="btnGenerateGX" value=" 生成工序开工单 "
                class="button" />
        </div>
        <div style="padding-right: 10px; margin-top: 20px;">
            <div id="Div1">
                <div style="float: left; margin: 10px 20px 0px 20px; position: fixed;" id="Div4">
                    班组<br />
                    <asp:ListBox runat="server" ID="liGXTeam" Width="50px" Height="150px"></asp:ListBox>
                </div>
                <table class="tablesorter" cellpadding="1" cellspacing="1" style="width: 1130px; float: left; margin-left: 80px;"
                    id="tbGX">
                    <thead>
                        <tr>
                            <td>
                                <label style="cursor: pointer;">
                                    <input type="checkbox" />全选/反选</label>
                            </td>
                            <td>班组
                            </td>
                            <td>销售订单号
                            </td>
                            <td>客户产品编号
                            </td>
                            <td>产品编号
                            </td>
                            <td>版本
                            </td>
                            <td>未交数
                            </td>
                            <td>库存数
                            </td>
                            <td>在制品数
                            </td>
                            <td>需要生产数
                            </td>
                            <td>交期
                            </td>
                            <td>工序
                            </td>
                            <td>行号
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
    <!--显示步骤-->
    <div id="showStep" style="color: #7700BB; float: left; margin-left: 70px; margin-top: 20px;">
        开工单生成进度：
    </div>
</asp:Content>
