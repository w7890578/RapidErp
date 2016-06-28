<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Edit.Master" AutoEventWireup="true"
    EnableEventValidation="false" CodeBehind="AddorEditRetutnOfGoodsInStoom.aspx.cs"
    Inherits="Rapid.StoreroomManager.AddorEditRetutnOfGoodsInStoom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>
        <%=titleName %>明细维护</title>

    <script type="text/javascript">

        $(function() {
            //表单验证JS
            $("#aspnetForm").validate({
                //出错时添加的标签
                errorElement: "span",
                success: function(label) {
                    //正确时的样式
                    label.text(" ").addClass("success");
                }
            });
            // $("#ctl00_bText").html("您当前的位置：<%=titleName %> 明细维护");
        });
    </script>

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
            height: 33px;
        }
    </style>

    <script type="text/javascript">
        $(function() {

            $("#aspnetForm").keypress(function(e) {
                if (e.which == 13) {
                    return false;
                }
            });
            var index = -1;
            //获取数据
            $("#btnSearch").click(function() {
                var changeDirection = " <%=titleName %>";



                var odersNumber = $.trim($("#ctl00_ContentPlaceHolder1_txtOdersNumber").val()); //销售订单号 
                if (odersNumber == "编号同步匹配") {
                    odersNumber = "";
                }

                $.get("../AjaxRequest/GetOdersNumberForProduct.ashx", { time: new Date(), OdersNumber: odersNumber, IsQueryOdersNumber: "true", ChangeDirection: changeDirection }, function(result) {
                    index = -1;
                    $("#mText").html(result).find("tr").click(function() {
                        var number = $.trim($(this).find("td:eq(0)").html()); //销售订单号 
                        if (number != "" && number != undefined) {
                            $("#ctl00_ContentPlaceHolder1_txtOdersNumber").val(number);
                            $.get("../AjaxRequest/GetOdersNumberForProduct.ashx", { time: new Date(), OdersNumber: number, IsQueryOdersNumber: "false" },
                             function(txt) {
                                 $("#ctl00_ContentPlaceHolder1_drpProduct").html(txt).click(function() {
                                     var product = $(this).val(); //产品
                                     var warehouseNumber = $("#ctl00_ContentPlaceHolder1_lbWarehouseNumber").html(); //出入库编号
                                     var orderNumber = number;
                                     $.get("../AjaxRequest/GetOdersNumberForProduct.ashx",
                                     { time: new Date(), OdersNumber: orderNumber, Prouduct: product, WarehouseNumber: warehouseNumber }, function(temp) {
                                         var arry = temp.split('^');
                                         $("#ctl00_ContentPlaceHolder1_txtCustomerProductNumber").val(arry[0]);
                                         $("#ctl00_ContentPlaceHolder1_txtInventoryQty").val(arry[1]);
                                     });
                                 });
                             });
                            $("#tempDiv").hide();
                        }
                    }).hover(function() {
                        $(this).find("td").addClass("bgGray");
                        var number = $.trim($(this).find("td:eq(0)").html());
                        if (number != "" && number != undefined) {
                            $("#ctl00_ContentPlaceHolder1_txtOdersNumber").val(number);
                        }
                    }, function() {
                        $(this).find("td").removeClass("bgGray");
                    });
                });
            })

            $("#ctl00_ContentPlaceHolder1_txtOdersNumber").focus(function() {
                if ($(this).val() == "编号同步匹配") {
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
                        tempNumber = $.trim($("#mText tr:eq(" + index + ") td:eq(0)").html());
                        $("#ctl00_ContentPlaceHolder1_txtOdersNumber").val(tempNumber);

                        break;
                    case 40: //下 
                        index = index + 1;
                        if (index > itemIndex) {
                            index = 0;
                        }
                        $("#mText tr:eq(" + index + ")").siblings().find("td").removeClass("bgGray");
                        $("#mText tr:eq(" + index + ")").find("td").addClass("bgGray");
                        tempNumber = $.trim($("#mText tr:eq(" + index + ") td:eq(0)").html());
                        $("#ctl00_ContentPlaceHolder1_txtOdersNumber").val(tempNumber);
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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <tr>
        <td align="right">
            <input type="hidden" id="hdChangeDirection" runat="server" />
            出入库编号：
        </td>
        <td>
            <asp:Label ID="lbWarehouseNumber" runat="server" Text=""></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">
            <%=orderName%>：
        </td>
        <td>
            <div style="position: relative;">
                <asp:TextBox ID="txtOdersNumber" runat="server" CssClass="input required" size="25"
                    ForeColor="Gray" Text="编号同步匹配"></asp:TextBox>
                <div id="tempDiv" style="position: absolute; display: none; left: 0px; top: 20px;
                    border: 1px solid black; background-color: White; width: 166px;">
                    <div style="margin-bottom: 5px; display: none;">
                        <input type="button" id="btnSearch" value="查询" /></div>
                    <div>
                        <table id="tbMarerial">
                            <tbody id="mText" style="text-align: left;">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <asp:Label ID="lbOdersNumber" runat="server"></asp:Label>
        </td>
    </tr>
    <tr id="trPlanNumberOrderNumber" runat ="server">
        <td align="right">
            销售订单号：
        </td>
        <td>
            <asp:Label ID="lbOrdersNumber" runat="server"></asp:Label>
        </td>
    </tr>
    <tr id="trRowNumber" runat="server">
        <td align="right">
            行号：
        </td>
        <td>
            <asp:Label ID="lbRowNumber" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">
            产品：
        </td>
        <td>
            <asp:DropDownList ID="drpProduct" runat="server" CssClass="required">
            </asp:DropDownList>
            <asp:Label ID="lbProduct" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td align="right">
            客户产品编号：
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtCustomerProductNumber" CssClass="input required"
                ReadOnly="true"></asp:TextBox><label style="color: Red;">
                    *</label>
        </td>
    </tr>
    <tr>
        <td align="right">
            数量：
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtQty" CssClass="input required digits"></asp:TextBox>
            <asp:Label ID="lbOldQty" runat="server" Visible="false"></asp:Label><!--用于编辑时存储原来的数量-->
            <label style="color: Red;">
                *</label>
        </td>
    </tr>
    <tr>
        <td align="right">
            库存数量：
        </td>
        <td>
            <asp:TextBox runat="server" ID="txtInventoryQty" CssClass="input" ReadOnly="true"></asp:TextBox>
        </td>
    </tr>
    <tr style="display: <%=showReason %>;">
        <td align="right">
            退货原因：
        </td>
        <td>
            <asp:TextBox ID="txtReason" Text="无" runat="server" TextMode="MultiLine" Style="height: 50px;
                width: 200px;" CssClass="input required"></asp:TextBox>
            <label style="color: Red;">
                *</label>
        </td>
    </tr>
    <tr>
        <td align="right">
            备注：
        </td>
        <td>
            <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" Height="30px" Width="300px"
                CssClass="input"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:Button ID="btnSubmit" runat="server" Text="修改" OnClick="btnSubmit_Click" class="submit" />
            <label style="color: Red;">
                (*为必填项)</label>
        </td>
    </tr>
</asp:Content>
