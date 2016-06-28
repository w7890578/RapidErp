<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddOrEditBOMInfo.aspx.cs"
    Inherits="Rapid.ProduceManager.AddOrEditBOMInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BOM信息维护</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function() {
            $("#btnSearch").click(function() {
                var productnumber = getQueryString("ProductNumber");
                var version = getQueryString("Version");
                var contion = $.trim($("#txtMarerial").val());
                if (contion == "支持名称与编号同时匹配") {
                    contion = "";
                }

                $.get("AddOrEditBOMInfo.aspx?sq="+new Date (), { ProductNumber: productnumber, Version: version, contion: contion, m: "m", time: new Date() }, function(result) {
                    index = -1;
                    $("#mText").html(result).attr("style", "width: 100%;text-align :center ").find("tr").click(function() {
                        var number = $.trim($(this).find("td:eq(1)").html());
                        //var OdersNumber = $.trim($(this).find("td:eq(2)").html());
                        if (number != "" && number != undefined) {
                            $("#txtMaterialNumber").val(number);
                            $("#tempDiv").hide();
                        }
                    }).hover(function() {
                        //$(this).find("td").addClass("bgGray");
                        var number = $.trim($(this).find("td:eq(1)").html());
                        if (number != "" && number != undefined) {
                            $("#txtMaterialNumber").val(number);
                        }
                    }, function() {
                        //$(this).find("td").removeClass("bgGray");
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
                        //                        $("#mText tr:eq(" + index + ")").siblings().find("td").removeClass("bgGray");
                        //                        $("#mText tr:eq(" + index + ")").find("td").addClass("bgGray");
                        tempNumber = $.trim($("#mText tr:eq(" + index + ") td:eq(1)").html());
                        $("#txtMaterialNumber").val(tempNumber);

                        break;
                    case 40: //下 
                        index = index + 1;
                        if (index > itemIndex) {
                            index = 0;
                        }
                        //                        $("#mText tr:eq(" + index + ")").siblings().find("td").removeClass("bgGray");
                        //                        $("#mText tr:eq(" + index + ")").find("td").addClass("bgGray");
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
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                产成品编号：
            </td>
            <td>
                <asp:Label ID="lbProductNumber" runat="server" Text="" CssClass="required"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:Label ID="lbVersion" runat="server" Text="" CssClass="required"></asp:Label>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                原材料编号：
            </td>
            <td>
                <div style="position: relative;">
                    <asp:TextBox ID="txtMaterialNumber" runat="server" CssClass="input required" size="25"
                        ForeColor="Gray" Text="支持名称与编号同时匹配"></asp:TextBox>
                    <div id="tempDiv" style="position: absolute; display: none; left: 0px; top: 21px;
                        border: 1px solid green; background-color: White; width: 323px;">
                        <div style="margin-bottom: 5px; display: none;">
                            原材料名称：<input type="text" id="txtMareialCondition" style="width: 200px; margin-right: 5px;" /><input
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
                                                <table style="width: 100%; text-align: center;" id="mText">
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
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="*"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户物料号：
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtCustomerMaterialNumber"></asp:TextBox>
                <asp:Label ID="lbtCustomerMaterialNumber" runat="server" Text=""></asp:Label>
                <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="trSingleDose">
            <td align="right">
                单机用量：
            </td>
            <td>
                <asp:TextBox ID="txtSingleDose" runat="server" CssClass="input required number" Text="0"></asp:TextBox>
                <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                单位：
            </td>
            <td>
                <asp:TextBox ID="txtUnit" runat="server" CssClass="input required"></asp:TextBox>
                <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="*"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" Height="31px" Width="300px" MaxLength="200"
                    CssClass="input"></asp:TextBox>
                <asp:Label ID="lblMemo" runat="server" Text="（限制输入200字）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="添加" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;
                <%--  <asp:Label ID="lbMsg" runat="server" Style="color: Red;" Text=""></asp:Label>--%>
                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
