<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditRejectsProduct.aspx.cs"
    EnableEventValidation="false" Inherits="Rapid.ProduceManager.AddOrEditRejectsProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>不合格品信息维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />
    <!--日期插件-->

    <script src="../Js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        #tbMarerial
        {
            width: 100%;
            text-align: center;
        }
        #mText
        {
            cursor: pointer;
        }
        .bgGray
        {
            background-color: #EBEBEB;
        }
    </style>

    <script type="text/javascript">
        $(function() {
            var ReportTime = $("#hdReportTime").val();
            var index = -1;
            $("#form1").keypress(function(e) {
                if (e.which == 13) {
                    return false;
                }
            });
            $("#btnClose").click(function() {
                $("#tempDiv").hide();
            });
            $("#btnSearch").click(function() {
                var contion = $.trim($("#txtProductNumber").val());

                if (contion == "支持名称与编号同时匹配") {
                    contion = "";
                }
                $.get("AddOrEditRejectsProduct.aspx?sq="+new Date (), { contion: contion, m: "m" }, function(result) {
                    index = -1;

                    $("#mText").html(result).find("tr").click(function() {
                        var number = $.trim($(this).find("td:eq(1)").html());
                        var version = $.trim($(this).find("td:eq(2)").html());
                        if (number != "" && number != undefined && version != "" && version != undefined) {
                            $("#txtProductNumber").val(number);
                            $("#txtVersion").val(version);
                            var customerid = $("#drpCustomerId").val();
                            $.get("AddOrEditRejectsProduct.aspx", { ProductNumber: number, Version: version, CustomerId: customerid }, function(customerNumber) {
                                $("#txtCustomerProductNumber").val(customerNumber);

                            });
                            $("#tempDiv").hide();
                        }
                    }).hover(function() {
                        $(this).find("td").addClass("bgGray");
                        var number = $.trim($(this).find("td:eq(1)").html());
                        if (number != "" && number != undefined) {
                            $("#txtProductNumber").val(number);
                        }
                    }, function() {
                        $(this).find("td").removeClass("bgGray");
                    });
                });
            });

            $("#txtProductNumber").focus(function() {

                var Dropcustomer = document.getElementById("drpCustomerId"); //获取DropDownList控件的引用

                var drocustomertext = Dropcustomer.options[Dropcustomer.selectedIndex].innerText;
                if (drocustomertext == "- - - - - 请 选 择 - - - - -") {
                    document.getElementById("lbSubmit").innerHTML = "请选择客户！！！";
                    return;
                }
                if (drocustomertext != "- - - - - 请 选 择 - - - - -") {
                    document.getElementById("lbSubmit").innerHTML = "";
                
                }   
                if ($(this).val() == "支持名称与编号同时匹配") {
                    $(this).val('');
                    $(this).css('color', 'black');
                }
                $("#tempDiv").show();
            }).keyup(function(e) {
                $("#tempDiv").show();
                var itemIndex = $("#mText tr").length - 1;

                var tempNumber = "";
                switch (e.keyCode) {
                    case 38: //上 
                        index = index - 1;
                        if (index < 0) {
                            index = itemIndex;
                        }
                        $("#mText tr:eq(" + index + ")").siblings().find("td").removeClass("bgGray");
                        $("#mText tr:eq(" + index + ")").find("td").addClass("bgGray");
                        tempNumber = $.trim($("#mText tr:eq(" + index + ") td:eq(1)").html());
                        $("#txtProductNumber").val(tempNumber);

                        break;
                    case 40: //下 
                        index = index + 1;
                        if (index > itemIndex) {
                            index = 0;
                        }
                        $("#mText tr:eq(" + index + ")").siblings().find("td").removeClass("bgGray");
                        $("#mText tr:eq(" + index + ")").find("td").addClass("bgGray");
                        tempNumber = $.trim($("#mText tr:eq(" + index + ") td:eq(1)").html());
                        $("#txtProductNumber").val(tempNumber);
                        break;
                    case 13:  //Enter 
                        $("#mText tr:eq(" + index + ")").click();
                        break;
                    default: $("#btnSearch").click();
                        break;
                }

            });
            $("#btnSearch").click();
        })
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hdReportTime" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr id="trReportTime" runat="server">
            <td align="right">
                上报时间：
            </td>
            <td>
                <asp:TextBox ID="txtReportTime" runat="server" CssClass="input required" size="25"></asp:TextBox>
                <asp:Label ID="lbReportTime" runat="server" Text="" CssClass="input required"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户：
            </td>
            <td>
                <asp:DropDownList ID="drpCustomerId" runat="server">
                    <asp:ListItem Text="- - - - - 请 选 择 - - - - -" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="lblCustomerId" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                产成品编号：
            </td>
            <td>
                <div style="position: relative;">
                    <asp:TextBox ID="txtProductNumber" runat="server" CssClass="input required" size="25"
                        ForeColor="Gray" Text="支持名称与编号同时匹配"></asp:TextBox>
                    <div id="tempDiv" style="position: absolute; display: none; left: 0px; top: 21px;
                        border: 1px solid green; background-color: White; width: 233px;">
                        <%--                        <span style="position: absolute; left: 233px; top: -1px; border-top: 1px solid green;
                            border-right: 1px solid green; border-bottom: 1px solid green; background-color: White;
                            padding: 1px; cursor: pointer; height: 25px; line-height: 20px;" id="btnClose">&nbsp;x&nbsp;关&nbsp;闭&nbsp;
                        </span>--%>
                        <div style="margin-bottom: 5px; display: none;">
                            <input type="button" id="btnSearch" value="查询" /></div>
                        <div>
                            <table id="tbMarerial">
                                <thead>
                                    <tr>
                                        <th>
                                            产品
                                        </th>
                                        <th>
                                            编号
                                        </th>
                                        <th>
                                            版本
                                        </th>
                                        <th style="width: 4px;">
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="3" style="width: 100%; padding: 0px;">
                                            <div style="width: 100%; height: 250px; overflow-y: auto;">
                                                <table style="width: 100%;" id="mText">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <asp:Label ID="lblProductNumber" runat="server" Text=""></asp:Label>
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:TextBox ID="txtVersion" runat="server" CssClass="input required" size="25" ReadOnly="true"></asp:TextBox>
                <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户产成品编号：
            </td>
            <td>
                <asp:TextBox ID="txtCustomerProductNumber" runat="server" CssClass="input required" size="25"
                    ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                数量：
            </td>
            <td>
                <asp:TextBox ID="txtQty" runat="server" CssClass="input required digits" size="25"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                返修原因：
            </td>
            <td>
                <asp:TextBox ID="txtRepairReason" runat="server" CssClass="input required" size="25"></asp:TextBox>
                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                返修日期：
            </td>
            <td>
                <asp:TextBox ID="txtRepairDate" runat="server" CssClass="input required" size="25"
                    onfocus="WdatePicker({skin:'green',minDate:'%y-%M-{%d}'})"></asp:TextBox>
                <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                修回检验日期：
            </td>
            <td>
                <asp:TextBox ID="txtRepairInspectionDate" runat="server" CssClass="input" size="25"
                    onfocus="WdatePicker({skin:'green',minDate:'%y-%M-{%d}'})"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                姓名：
            </td>
            <td>
                <asp:DropDownList ID="drpName" runat="server">
                    <asp:ListItem Text="- - - - - 请 选 择 - - - - -" Value=""></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td class="noRequired">
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" size="25" Height="31px"
                    Width="300px" CssClass="input"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" OnClick="btnSubmit_Click" />
                &nbsp;&nbsp;
                <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
