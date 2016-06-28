<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEditDeliveryBillDetail.aspx.cs"
    EnableEventValidation="false" Inherits="Rapid.SellManager.AddEditDeliveryBillDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>送货单明细</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

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
            var deliveryNumber = $("#hdDeliveryNumber").val();
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
                $.get("AddEditDeliveryBillDetail.aspx", { contion: contion, m: "m", DeliveryNumber: deliveryNumber }, function(result) {
                    index = -1;
                    $("#mText").html(result).find("tr").click(function() {
                        var number = $.trim($(this).find("td:eq(1)").html());
                        var version = $.trim($(this).find("td:eq(2)").html());
                        if (number != "" && number != undefined && version != "" && version != undefined) {
                            $("#txtProductNumber").val(number);
                            $("#txtVersion").val(version);
                            $.get("AddEditDeliveryBillDetail.aspx", { time: new Date(), productNumber: number, version: version, DeliveryNumber: deliveryNumber }, function(customerNumber) {
                                var arry = customerNumber.split('^');
                                $("#drpCustomerProductNumber").html(arry[0]);
                                $("#txtMaterialDescription").val(arry[1]);
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
    <input type="hidden" id="hdDeliveryNumber" value="" runat="server" />
    <div id="progressBar" style="position: absolute; top: 40%; left: 50%; display: none;">
        <img src="../Img/loading.gif" alt="loading" />
    </div>
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
                <asp:TextBox ID="txtSN" runat="server" CssClass="input digits required" size="25"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                采购单(销售订单号)：
            </td>
            <td>
                
                <asp:Label ID="lbOrdersNumber" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                产成品编号：
            </td>
            <td>
                <%--<div style="position: relative;">
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
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </div>--%> 
                 <asp:Label ID="lblProductNumber" runat="server"  Text=""></asp:Label>
            </td>
       </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:Label ID="lblVersion" runat="server"  Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                物料编码(客户产成品编号)：
            </td>
            <td>
                <asp:Label runat ="server" ID ="lbCustomerProductNumber"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                物料描述:
            </td>
            <td>

                <asp:Label ID="lblMaterialDescription" runat="server"  Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                采购单行(行号):
            </td>
            <td>
                <asp:Label ID="lblRowNumber" runat="server"  Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                发货数量：
            </td>
            <td>
                    <asp:Label ID="lblDeliveryQty" runat="server"  Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                实到数量(收货)：
            </td>
            <td>
                <asp:TextBox ID="txtArriveQty" runat="server" CssClass="input digits required" size="25"
                    Text="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                实收数量(收货)：
            </td>
            <td>
                <asp:TextBox ID="txtConformenceQty" runat="server" CssClass="input digits required"
                    size="25" Text="100"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                拒收原因(收货):
            </td>
            <td class="noRequired">
                <asp:TextBox ID="txtNGReason" runat="server" CssClass="input " size="25" Text=""></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                合格品数量(质检):
            </td>
            <td>
                <asp:TextBox ID="txtPassQty" runat="server" CssClass="input digits " size="25" Text="0"> </asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                拒收数量(质检):
            </td>
            <td>
                <asp:TextBox ID="txtNgQty" runat="server" CssClass="input digits " size="25" Text="0"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                拒收原因(质检):
            </td>
            <td class="noRequired">
                <asp:TextBox ID="txtInspectorNGReason" runat="server" CssClass="input " size="25"
                    Text=""></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                铸件毛坯编码(仅适用铸件):
            </td>
            <td class="noRequired">
                <asp:TextBox ID="txtRoughCastingCode" runat="server" CssClass="input " size="25"
                    Text=""></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                进口件编码(仅适用进口件):
            </td>
            <td class="noRequired">
                <asp:TextBox ID="txtImportPartsCode" runat="server" CssClass="input " size="25" Text=""></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="编辑" CssClass="submit" OnClick="btnSubmit_Click" />
                <span style="color: Red;">&nbsp;&nbsp;（*为必填项）</span>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
