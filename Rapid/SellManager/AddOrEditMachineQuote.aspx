<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditMachineQuote.aspx.cs"
    EnableEventValidation="false" Inherits="Rapid.SellManager.AddOrEditMachineQuote" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>加工报价单信息维护</title>
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
            var quoteNumber = $("#hdQuoteNumber").val();
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
            $("#btnClose").click(function() {
                $("#tempDiv").hide();
            });
            $("#btnSearch").click(function() {
                var contion = $.trim($("#txtProductNumber").val());
                if (contion == "支持名称与编号同时匹配") {
                    contion = "";
                }
                $.get("AddOrEditMachineQuote.aspx", { contion: contion, m: "m", QuoteNumber: quoteNumber }, function(result) {
                    index = -1;
                    $("#mText").html(result).find("tr").click(function() {
                        var number = $.trim($(this).find("td:eq(1)").html());
                        var version = $.trim($(this).find("td:eq(2)").html());
                        if (number != "" && number != undefined && version != "" && version != undefined) {
                            $("#txtProductNumber").val(number);
                            $("#txtVersion").val(version);
                            $.get("AddOrEditMachineQuote.aspx", { time: new Date(), productNumber: number, version: version, QuoteNumber: quoteNumber }, function(customerNumber) {
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
    <input type="hidden" id="hdQuoteNumber" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                序号：
            </td>
            <td>
                <asp:TextBox ID="txtSN" runat="server" CssClass="input required" size="25"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
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
                        <span style="position: absolute; left: 233px; top: -1px; border-top: 1px solid green;
                            border-right: 1px solid green; border-bottom: 1px solid green; background-color: White;
                            padding: 1px; cursor: pointer; height: 25px; line-height: 20px;" id="btnClose">&nbsp;x&nbsp;关&nbsp;闭&nbsp;
                        </span>
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
                </div>
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:TextBox ID="txtVersion" runat="server" CssClass="input required" size="25" ReadOnly="true"></asp:TextBox>
                <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                物料编码(客户产成品编号)：
            </td>
            <td>
                <%--<asp:DropDownList runat="server" ID="txtCustomerProductNumber">
                    <asp:ListItem Text="- - - - - 请 选 择 - - - - -" Value=""></asp:ListItem>
                </asp:DropDownList>--%><asp:TextBox ID="txtCustomerProductNumber" runat="server"
                    CssClass="input required" size="25" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                阶层：
            </td>
            <td>
                <asp:TextBox ID="txtHierarchy" runat="server" CssClass="input required" size="25"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                BOM用量：
            </td>
            <td>
                <asp:TextBox ID="txtBOMAmount" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1"></asp:TextBox>
                <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                原材料单价(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtMaterialPrcie" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1"></asp:TextBox>
                <asp:Label ID="Label6" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                工时费(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtTimeCharge" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1"></asp:TextBox>
                <asp:Label ID="Label7" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                利润(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtProfit" runat="server" CssClass="input required" size="25" MaxLength="50"
                    minlength="1"></asp:TextBox>
                <asp:Label ID="Label8" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                管销研费用(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtManagementPrcie" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1"></asp:TextBox>
                <asp:Label ID="Label9" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                损耗(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtLossPrcie" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1"></asp:TextBox>
                <asp:Label ID="Label10" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                单价(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1"></asp:TextBox>
                <asp:Label ID="Label11" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                固定提前期：
            </td>
            <td>
                <asp:TextBox ID="txtFixedLeadTime" runat="server" CssClass="input required" size="25"
                    MaxLength="50" minlength="1"></asp:TextBox>
                <asp:Label ID="Label12" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200"  CssClass="input"
                    size="25" Height="31px" Width="300px" ></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;<asp:Label ID="Label13" runat="server" Text="（*号为必填项）" ForeColor="Red"></asp:Label>
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
