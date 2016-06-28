<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTradingQuote.aspx.cs"
    EnableEventValidation="false" Inherits="Rapid.SellManager.AddTradingQuote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>贸易报价单维护</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

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
            var index = -1;
            //表单验证JS
            $("#form1").validate({
                //出错时添加的标签
                errorElement: "span",
                success: function(label) {
                    //正确时的样式
                    label.text(" ").addClass("success");
                }
            });
            $("#form1").keypress(function(e) {
                if (e.which == 13) {
                    return false;
                }
            });


            $("#btnSearch").click(function() {
                var contion = $.trim($("#txtMarerial").val());
                var QuoteNumber = $.trim($("#lbQuoteNumber").html())
                if (contion == "支持名称与编号同时匹配") {
                    contion = "";
                }
                $.get("AddTradingQuote.aspx", { time: new Date(), QuoteNumber: QuoteNumber, contion: contion, m: "m" }, function(result) {
                    index = -1;
                    $("#mText").html(result).find("tr").click(function() {
                        var number = $.trim($(this).find("td:eq(1)").html());
                        if (number != "" && number != undefined) {
                            $("#txtMarerial").val(number);
                            $.get("AddTradingQuote.aspx", { time: new Date(), QuoteNumber: QuoteNumber, mareialNumber: number }, function(customerNumber) {
                                $("#txtCustomerMarerial").val(customerNumber);
                            });
                            $("#tempDiv").hide();
                        }
                    }).hover(function() {
                        $(this).find("td").addClass("bgGray");
                        var number = $.trim($(this).find("td:eq(1)").html());
                        if (number != "" && number != undefined) {
                            $("#txtMarerial").val(number);
                        }
                    }, function() {
                        $(this).find("td").removeClass("bgGray");
                    });
                });
            });

            $("#txtMarerial").focus(function() {
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
                        $("#txtMarerial").val(tempNumber);

                        break;
                    case 40: //下 
                        index = index + 1;
                        if (index > itemIndex) {
                            index = 0;
                        }
                        $("#mText tr:eq(" + index + ")").siblings().find("td").removeClass("bgGray");
                        $("#mText tr:eq(" + index + ")").find("td").addClass("bgGray");
                        tempNumber = $.trim($("#mText tr:eq(" + index + ") td:eq(1)").html());
                        $("#txtMarerial").val(tempNumber);
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
    <div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
            <tr>
                <th colspan="2" align="left">
                    基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbMsg" runat="server" ForeColor="Red"></asp:Label>
                </th>
            </tr>
            <tr>
                <td align="right">
                    报价单号：
                </td>
                <td>
                    <asp:Label runat="server" ID="lbQuoteNumber"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    序号：
                </td>
                <td>
                    <asp:TextBox ID="txtSN" runat="server" CssClass="input required digits" size="25"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    原材料编号：
                </td>
                <td>
                    <div style="position: relative;">
                        <asp:TextBox ID="txtMarerial" runat="server" CssClass="input required" size="25"
                            ForeColor="Gray" Text="支持名称与编号同时匹配"></asp:TextBox>
                        <div id="tempDiv" style="position: absolute; display: none; left: 0px; top: 21px;
                            border: 1px solid green; background-color: White; width: 233px;">
                            <div style="margin-bottom: 5px; display: none;">
                                原材料名称：<input type="text" id="txtMareialCondition" style="width: 100px; margin-right: 5px;" /><input
                                    type="button" id="btnSearch" value="查询" /></div>
                            <div>
                                <table id="tbMarerial">
                                    <thead>
                                        <tr>
                                            <th>
                                                原材料
                                            </th>
                                            <th>
                                                编号
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
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="right">
                    客户物料编号：
                </td>
                <td>
                    <asp:TextBox ID="txtCustomerMarerial" runat="server" CssClass="input required" size="25"
                        ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    固定提前期：
                </td>
                <td>
                    <asp:TextBox ID="txtFixedLeadTime" runat="server" CssClass="input required" size="25"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    备注：
                </td>
                <td>
                    <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Width="300px" size="25"
                        Height="31px" class="input"></asp:TextBox>
                    <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
