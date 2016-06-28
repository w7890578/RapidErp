<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountsPayApplicationDetail.aspx.cs"
    Inherits="Rapid.PurchaseManager.AccountsPayApplicationDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购应付明细</title>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">

        //            $('#fmDetail').form('load', {
        //                Guid: Guid,
        //                InvoiceNumber: InvoiceNumber,
        //                BillingDate: BillingDate,
        //                Remark: Remark
        //            });
        //            $("#Guid").val(Guid);
        //            $("#InvoiceNumber").val(InvoiceNumber);
        //            $("#BillingDate").datebox('setValue', BillingDate)
        //            $("#Remark").val(Remark);
        //            $('#Dlg').dialog('open').dialog('setTitle', '编辑');

        //        }


        //        function Save() { 
        //            $.ajax({
        //                url: "AccountsPayApplicationDetail.aspx",
        //                type: "POST",
        //                data: {
        //                    "Guid": $("#Guid").val(),
        //                    "InvoiceNumber": $("#InvoiceNumber").val(),
        //                    "BillingDate": $("#BillingDate").datebox('getValue'),
        //                    "Remark": $("#Remark").val()
        //                },
        //                success: function(res) {
        //                    if (res == "ok") {
        //                        alert("保存成功");
        //                        $('#Dlg').dialog('close');
        //                        $('#fmDetail').form('clear');
        //                        $("#btnSearch").click();

        //                    } else {
        //                        $.messager.show({
        //                            title: "错误",
        //                            msg: '保存失败！<br/>' + res
        //                        });
        //                    }

        //                }
        //            });
        //            $('#fmDetail').form('submit', {
        //                url: "AccountsPayApplicationDetail.aspx",
        //                onSubmit: function(param) {                //提交时触发
        //                    var flag = $(this).form('validate');    //是否通过验证
        //                    if (flag) {
        //                        //$('#tbDatagrid').datagrid("loading"); //如果通过验证让datagrid显示载入状态
        //                    }
        //                    return flag;
        //                },
        //                success: function(res) {
        //                    alert(res);
        //                    if (res == "ok") { 
        //                        $.messager.show({
        //                            title: "提示",
        //                            msg: "操作成功"
        //                        });
        //                        $('#Dlg').dialog('close');         //关闭弹出框
        //                        $('#fmDetail').form('clear');              //清除表单数据。 
        //                    }
        //                    else {

        //                        $.messager.show({
        //                            title: "错误",
        //                            msg: '保存失败！<br/>' + res
        //                        });
        //                    }
        //                }
        //            })

        $(function() {
            //查询sql语句
            var sq = getQueryString("SQ");
            var sp = getQueryString("SP");
            var ck = getQueryString("ck");

            if (sq == "1") {
                $("#btnBack").click(function() {
                    window.location.href = "../PurchaseManager/AccountsPayApplication.aspx";
                });
            }
            if (sp == "2") {
                $("#btnBack").click(function() {
                    window.location.href = "../FinancialManager/AccountsPayCheck.aspx";

                });
            }
            if (ck == "3") {
                $("#btnBack").click(function() {
                    window.location.href = "../FinancialManager/AccountsPayLookOver.aspx";
                });
            }
            $("#btnJS").click(function() {
                var ordersnumber = $("#hdOrdersNumber").val();
                var createtime = $("#hdCreateTime").val();
                var checkResult = "";
                var arrChk = $("input[name='subBox']:checked");
                $(arrChk).each(function() {
                    checkResult = this.value + "," + checkResult;
                });
                if (checkResult == "") {
                    alert("请选择要结算的行！");
                    return;
                }
                //去掉最后一个逗号
                var reg = /,$/gi;
                checkResult = checkResult.replace(reg, "");
                //这是获取的值
                if (confirm("确定选中数据?")) {
                    $.get("AccountsPayApplicationDetail.aspx?time=" + new Date(), { js: ConvertsContent(checkResult), OrdersNumber: ordersnumber, CreateTime: createtime }, function(result) {
                        if (result == "1") {
                            alert("操作成功");
                            $("#btnSearch").click();
                        }
                        else {
                            alert("操作失败!原因：" + result);
                        }

                    });

                }
            });
            $("#lbQx").click(function() {
                $("input[name='subBox']").each(function() {
                    this.checked = !this.checked; //整个反选
                });
            });

        })

        function Edit(Guid) {
            OpenDialog("../FinancialManager/EditAccountPayDetail.aspx?Guid=" + Guid + "&fatherGuid=" + getQueryString("fatherGuid") + "&time=" + new Date(), "btnSearch", "400", "600");
            //alert(Guid);

            //window.showModalDialog("../FinancialManager/EditAccountPayDetail.aspx?Guid=" + guid + "&time=" + new Date(), "", "dialogWidth:600px;dialogHeight:400px;scroll:no;status:no");
            //模态窗口关闭，再执行一遍查询
            //$("#btnSearch").click();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <style type="text/css">
        .border
        {
            background-color: Black;
            width: 100%;
            font-size: 14px;
            text-align: center;
        }
        .border tr td
        {
            padding: 4px;
            background-color: White;
        }
        a
        {
            color: Blue;
        }
        a:hover
        {
            color: Red;
        }
        #choosePrintClounm
        {
            position: absolute;
            top: 20px;
            left: 50px;
            background-color: White;
            width: 170px;
            border: 1px solid green;
            padding: 10px;
            font-size: 14px;
            display: none;
        }
    </style>
    <input type="hidden" runat="server" id="hdBackUrl" />
    <div style="width: 100%; text-align: center; font: 96px; font-size: xx-large; font-weight: bold;
        margin-top: 20px">
        采购应付明细
    </div>
    <div style="margin-bottom: 10px;">
        <div id="divHeader" style="padding: 10px;">
            <div style="float: left; margin-bottom: 10px">
                <input type="hidden" id="hdOrdersNumber" runat="server" />
                <input type="hidden" id="hdCreateTime" runat="server" />
                <%-- &nbsp&nbsp; &nbsp&nbsp; 采购订单号：<asp:TextBox ID="txtOdersNumber" runat="server"></asp:TextBox>--%>
                &nbsp&nbsp; 采购合同号：<asp:TextBox ID="txtHDnumber" runat="server"></asp:TextBox>
                <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="button" OnClick="btnSearch_Click"
                    Style="margin-right: 10px; margin-left: 10px;" />
                <input type="button" value="返回" id="btnBack" style="margin-right: 10px;" />
                <input type="button" value="结算" id="btnJS" style="margin-right: 10px;" />
                &nbsp;&nbsp; &nbsp;&nbsp;<label style="color: Red;" id="lbMsg"></label>
            </div>
        </div>
        <table class="border" cellpadding="1" cellspacing="1">
            <thead>
                <tr>
                    <td>
                        <label id="lbQx">
                            <input type="checkbox" /></label>全选/反选
                    </td>
                    <td>
                        入库单号
                    </td>
                    <td>
                        采购订单号
                    </td>
                    <td>
                        采购合同号
                    </td>
                    <td>
                        原材料编号
                    </td>
                    <td>
                        供应商物料编号
                    </td>
                    <td>
                        描述
                    </td>
                    <td>
                        采购数量
                    </td>
                    <td>
                        到货数量
                    </td>
                    <td>
                        单价
                    </td>
                    <td>
                        总价
                    </td>
                    <td>
                        实际付款金额
                    </td>
                    <td>
                        实际付款日期
                    </td>
                    <td>
                        运输号
                    </td>
                    <td>
                        是否结算
                    </td>
                    <td>
                        发票号码
                    </td>
                    <td>
                        开票日期
                    </td>
                    <td>
                        预计交期
                    </td>
                    <td>
                        备注
                    </td>
                    <td style="display: <%=show%>;">
                        操作
                    </td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rpList">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <span style="display: <%#Eval("入库单号").ToString().Equals("合计") ? "none" : "inline"%>;">
                                    <input type="checkbox" value="<%#Eval("Guid")%>" name='subBox' /></span>
                            </td>
                            <td>
                                <%#Eval("入库单号")%>
                            </td>
                            <td>
                                <%#Eval("采购订单号")%>
                            </td>
                            <td>
                                <%#Eval("采购合同号")%>
                            </td>
                            <td>
                                <%#Eval("原材料编号")%>
                            </td>
                            <td>
                                <%#Eval("供应商物料编号")%>
                            </td>
                            <td>
                                <%#Eval("描述")%>
                            </td>
                            <td>
                                <%#Eval("采购数量")%>
                            </td>
                            <td>
                                <%#Eval("到货数量")%>
                            </td>
                            <td>
                                <%#Eval("单价")%>
                            </td>
                            <td>
                                <%#Eval("总价")%>
                            </td>
                            <td>
                                <%#Eval("实际付款金额")%>
                            </td>
                            <td>
                                <%#Eval("实际付款日期")%>
                            </td>
                            <td>
                                <%#Eval("运输号")%>
                            </td>
                            <td>
                                <%#Eval("是否结清")%>
                            </td>
                            <td>
                                <%#Eval("发票号码")%>
                            </td>
                            <td>
                                <%#Eval("开票日期")%>
                            </td>
                            <td>
                                <%#Eval("预计交期")%>
                            </td>
                            <td>
                                <%#Eval("备注")%>
                            </td>
                            <td style="display: <%=show%>;">
                                <span style="display: <%#Eval("入库单号").ToString().Equals("合计") ? "none" : "inline"%>;">
                                    <span><a href="###" onclick="Edit('<%#Eval("Guid")%>')">编辑</a></span></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>
    <!--编辑框-->
    <%--<div id="Dlg" class="easyui-dialog" style="width: 350px; height: 200px; padding: 10px;"
        data-options="iconCls: 'icon-save',modal:true,closed:true,buttons:'#Dlg_Buttons'">
        <form id="fmDetail" method="post" novalidate enctype="multipart/form-data">
        <input type="hidden" name="Guid" value="" id="Guid" />
        <table width="100%">
            <tr>
                <td style="text-align: right;">
                    发票号码：
                </td>
                <td>
                    <input name="InvoiceNumber" id="InvoiceNumber" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    开票日期：
                </td>
                <td>
                    <input name="BillingDate" id="BillingDate" class="easyui-datebox" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    备注：
                </td>
                <td>
                    <input name="Remark" id="Remark" />
                </td>
            </tr>
        </form>
        </table>
    </div>
    <div id="Dlg_Buttons">
        <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-ok" onclick="Save()">
            保存</a> <a href="javascript:void(0)" class="easyui-linkbutton" iconcls="icon-cancel"
                onclick="javascript:$('#Dlg').dialog('close')">取消</a>
    </div>--%>
    </form>
</body>
</html>
