<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditCertificateOrdersDetail.aspx.cs"
    Inherits="Rapid.PurchaseManager.AddOrEditCertificateOrdersDetail" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购订单明细</title>
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
        .style1
        {
            height: 29px;
        }
    </style>

<%--    <script type="text/javascript">
        $(function() {
            var index = -1;
            //价格计算
            function CalculatePrice() {
                var count = $("#txtOrderQty").val();
                var unitPrice = $("#txtUnitPrice").val();
                var sum = 0.00;
                try {
                    sum = count * unitPrice;
                }
                catch (e) {
                    sum = 0.00;
                }
                $("#txtSumPrice").val(isNaN(sum) == true ? 0 : sum.toFixed(2));
            }
            //数量计算
            function CalculateQuantity() {
                var count = $("#txtOrderQty").val();
                var noDeliveryQty = $("#txtNonDeliveryQty").val();
                var deliveryQty = 0;
                try {
                    deliveryQty = count - noDeliveryQty;
                }
                catch (e) {
                    deliveryQty = 0;
                }
                if (isNaN(deliveryQty) || deliveryQty < 0) {
                    deliveryQty = 0;
                }
                $("#txtDeliveryQty").val(deliveryQty);
            }
            $("#txtOrderQty").blur(function() {
                CalculatePrice();
                CalculateQuantity();
            });
            $("#txtUnitPrice").blur(function() {
                CalculatePrice();
            });
            $("#txtSumPrice").focus(function() {
                CalculatePrice();
            });

            $("#txtDeliveryQty").focus(function() {
                CalculateQuantity();
            });
            $("#txtNonDeliveryQty").blur(function() {
                CalculateQuantity();
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
                var oderNumber = $("#hdOderNumber").val();
                var contion = $.trim($("#txtMarerial").val());
                if (contion == "支持名称与编号同时匹配") {
                    contion = "";
                }
                $.get("AddOrEditCertificateOrdersDetail.aspx?sq="+new Date(), { OdersNumber: oderNumber, contion: contion, m: "m", time: new Date() }, function(result) {
                    index = -1;
                    $("#mText").html(result).find("tr").click(function() {
                        var number = $.trim($(this).find("td:eq(1)").html());
                        //var OdersNumber = $.trim($(this).find("td:eq(2)").html());
                        if (number != "" && number != undefined) {
                            $("#txtMaterialNumber").val(number);

                            $.get("AddOrEditCertificateOrdersDetail.aspx?sq="+new Date (), { OdersNumber: oderNumber, MaterialNumber: number, time: new Date() }, function(SupplierMaterialNumber) {
                                $("#txtSupplierMaterialNumber").val(SupplierMaterialNumber);
                            });
                            $("#tempDiv").hide();
                        }
                    }).hover(function() {
                        $(this).find("td").addClass("bgGray");
                        var number = $.trim($(this).find("td:eq(1)").html());
                        if (number != "" && number != undefined) {
                            $("#txtMaterialNumber").val(number);
                        }
                    }, function() {
                        $(this).find("td").removeClass("bgGray");
                    });
                }); 
            });

            $("#txtMaterialNumber").focus(function() {
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
                        $("#txtMaterialNumber").val(tempNumber);

                        break;
                    case 40: //下 
                        index = index + 1;
                        if (index > itemIndex) {
                            index = 0;
                        }
                        $("#mText tr:eq(" + index + ")").siblings().find("td").removeClass("bgGray");
                        $("#mText tr:eq(" + index + ")").find("td").addClass("bgGray");
                        tempNumber = $.trim($("#mText tr:eq(" + index + ") td:eq(1)").html());
                        $("#txtMaterialNumber").val(tempNumber);
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
    </script>--%>

</head>
<body >
    <form id="form1" runat="server">
    <!--存储采购订单编号-->
    <input type="hidden" id="hdOderNumber" runat="server" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                采购订单编号：
            </td>
            <td>
                <asp:TextBox ID="txtOrdersNumber" runat="server" CssClass="input required" size="25"
                    ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
      
    <%--    <tr style="display:none;">
            <td align="right">
                原材料编号：
            </td>
            <td>
                <div style="position: relative;">
                    <asp:TextBox ID="txtMaterialNumber" runat="server" CssClass="input " size="25"
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
                     <asp:Label ID="lbMaterialNumber" runat="server"  Text=""></asp:Label>
                    <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </div>
            </td>
        </tr>--%>
        <tr>
            <td align="right">
                编号类型:
            </td>
            <td>
                <asp:DropDownList ID="drpType" runat="server">
                <asp:ListItem value="供应商物料编号" Text="供应商物料编号"></asp:ListItem>
                <asp:ListItem value="瑞普迪编号" Text="瑞普迪编号"></asp:ListItem>
                </asp:DropDownList>
                        
            </td>
        </tr>
        <tr>
            <td align="right">
                物料编号：
            </td>
            <td>
                <asp:TextBox ID="txtSupplierMaterialNumber" runat="server" CssClass="input required"
                    size="25"></asp:TextBox>
               <asp:Label ID="lbSupplierMaterialNumber" runat="server" Text="" ></asp:Label>
                  <asp:Label ID="Label12" runat="server" Text="根据编号类型输入相应的物料编号" style="color:Red;margin-left:10px;"></asp:Label>
            </td>
        </tr>
           <tr>
            <td align="right">
                交期：
            </td>
            <td>
                <asp:TextBox ID="txtLeadTime" runat="server" CssClass="input required" size="25"
                    onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                <asp:Label ID="Label10" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                数量：
            </td>
            <td>
                <asp:TextBox ID="txtOrderQty" runat="server" CssClass="input required digits" size="25"></asp:TextBox>
                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
      <tr>
            <td align="right">
                最小起订量：
            </td>
            <td>
                <asp:TextBox ID="txtMinOrderQty" runat="server" CssClass="input required digits" size="25"></asp:TextBox>
                  <asp:Label ID="Label6" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr style="display:<%=show %>;">
            <td align="right">
                单价(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="input number" size="25"></asp:TextBox>
                <asp:Label ID="Label8" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
           <tr style="display:<%=show %>;">
            <td align="right">
                单价(含税)：
            </td>
            <td>
                <asp:TextBox ID="txtUnitPrice_C" runat="server" CssClass="input number" size="25"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
         <tr style="display:<%=show %>;">
            <td align="right">
                总价(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtSumPrice" runat="server" CssClass="input number" size="25"></asp:TextBox>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
         <tr style="display:<%=show %>;">
            <td align="right">
                总价(含税)：
            </td>
            <td>
                <asp:TextBox ID="txtSumPrice_C" runat="server" CssClass="input number" size="25"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr style="display:<%=showPay %>;">
            <td align="right">
                付款一：
            </td>
            <td>
                <asp:TextBox ID="txtPayOne" runat="server" CssClass="input number" size="25" Text ="0"></asp:TextBox>
                <asp:Label ID="Label7" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
         <tr style="display:<%=showPay %>;">
            <td align="right">
                付款二：
            </td>
            <td>
                <asp:TextBox ID="txtPayTwo" runat="server" CssClass="input number" size="25" Text ="0"></asp:TextBox>
                <asp:Label ID="Label9" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
  <%--      <tr>showPay
            <td align="right" class="style1">
                总价(元)：
            </td>
            <td class="style1">
                <asp:TextBox ID="txtSumPrice" runat="server" CssClass="input required number" size="25"  ></asp:TextBox>
 
                <asp:Label ID="Label9" runat="server" ForeColor="Red" Text=""></asp:Label>
            </td>
        </tr>--%>
     
       
          <tr style ="display :none ">
            <td align="right" class="style1">
                序号：
            </td>
            <td class="style1">
                <asp:TextBox ID="txtRowNumber" runat="server" CssClass="input required digits" size="25" Text ="1"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td  >
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Width="300px"
                    size="25" Height="31px"  class="input"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" CssClass="submit" OnClick="btnSubmit_Click" />
                &nbsp;
                <asp:Label ID="Label11" runat="server" ForeColor="Red" Text="（*为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
