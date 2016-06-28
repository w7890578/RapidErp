<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditDeliveryNoteDetailed_two.aspx.cs" Inherits="Rapid.SellManager.EditDeliveryNoteDetailed_two" %>

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
                
                <asp:Label ID="lblSn" runat="server"  Text=""></asp:Label>
            </td>
        </tr>

        <tr>
            <td align="right">
                产成品编号：
            </td>
            <td>
              
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
                描述:
            </td>
            <td>

                <asp:Label ID="lblMaterialDescription" runat="server"  Text=""></asp:Label>
            </td>
        </tr>
            <tr>
            <td align="right">
                行号             </td>
            <td>
                <asp:Label ID="lblRowNumber" runat="server"  Text=""></asp:Label>
            </td>
        </tr>
         </tr>
             <tr>
            <td align="right">
              销售订单号：
            </td>
            <td>
                
                <asp:Label ID="lbOrdersNumber" runat="server"></asp:Label>
            </td>
        </tr>
             <tr>
            <td align="right">
                客户订单号（客户采购订单号）：
            </td>
            <td>
                <asp:Label runat ="server" ID ="lbCustomerOrdersNumber"></asp:Label>
            </td>
        </tr>
       
    
        <tr>
            <td align="right">
               客户号（库房用）：
            </td>
            <td>
                    <asp:Label ID="lblKhhN" runat="server"  Text=""></asp:Label>
            </td>
       </tr>
          <tr>
            <td align="right">
              订单总量：
            </td>
            <td>
                    <asp:Label ID="lblQty" runat="server"  Text=""></asp:Label>
            </td>
       </tr>
          <tr>
            <td align="right">
                交货数量：
            </td>
            <td>
                <asp:TextBox ID="txtConformenceQty" runat="server" CssClass="input digits required"
                    size="25" Text="100"></asp:TextBox>
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
