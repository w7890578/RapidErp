<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditMachineQuoteDetail.aspx.cs"
    Inherits="Rapid.SellManager.EditMachineQuoteDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>加工报价单明细</title>
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

    <script>

        function changetTextMaterialPrcie() {
            document.getElementById("txtMarerialOnePrice").value = document.getElementById("txtMaterialPrcie").value;
            document.getElementById("txtOnePrice").value = parseFloat(document.getElementById("txtMarerialOnePrice").value) *
            parseFloat(document.getElementById("txtBOMAmountUnitPrice").value);

        }

        function changetTextProfit() {

            //管销
            document.getElementById("txtManagementPrcie").value = parseFloat(document.getElementById("txtMarerialPriceManagementPrcie").value) *
            parseFloat(document.getElementById("txtCoefficientManagementPrcie0").value) + parseFloat(document.getElementById("txtTimeChargeManagementPrcie").value) *
            parseFloat(document.getElementById("txtCoefficientManagementPrcie1").value);
            //损耗
            document.getElementById("txtManagementPrcieUnitPrice").value = document.getElementById("txtManagementPrcie").value;
            document.getElementById("txtLossPrcie").value = parseFloat(document.getElementById("txtMarerialPriceLossPrcie").value) *
            parseFloat(document.getElementById("txtCoefficientLossPrcie").value);
            document.getElementById("txtLossPrcieUnitPrice").value = document.getElementById("txtLossPrcie").value;
            //单价
            document.getElementById("txtProfitUnitPrice").value = document.getElementById("txtProfit").value;
            document.getElementById("txtUnitPrice").value = parseFloat(document.getElementById("txtMarerialPriceUnitPrice").value) +
            parseFloat(document.getElementById("txtTimeChargeUnitPrice").value) + parseFloat(document.getElementById("txtProfitUnitPrice").value) +
            parseFloat(document.getElementById("txtManagementPrcieUnitPrice").value) +
            parseFloat(document.getElementById("txtLossPrcieUnitPrice").value)

        }
        function changetTextCoefficientManagementPrcie0() {
            document.getElementById("txtManagementPrcie").value = parseFloat(document.getElementById("txtMarerialPriceManagementPrcie").value) *
            parseFloat(document.getElementById("txtCoefficientManagementPrcie0").value) + parseFloat(document.getElementById("txtTimeChargeManagementPrcie").value) *
            parseFloat(document.getElementById("txtCoefficientManagementPrcie1").value);
            document.getElementById("txtManagementPrcieUnitPrice").value = document.getElementById("txtManagementPrcie").value;
            //单价
            document.getElementById("txtUnitPrice").value = parseFloat(document.getElementById("txtMarerialPriceUnitPrice").value) +
            parseFloat(document.getElementById("txtTimeChargeUnitPrice").value) + parseFloat(document.getElementById("txtProfitUnitPrice").value) +
            parseFloat(document.getElementById("txtManagementPrcieUnitPrice").value) +
            parseFloat(document.getElementById("txtLossPrcieUnitPrice").value)
        }
        function changetTextCoefficientManagementPrcie1() {
            document.getElementById("txtManagementPrcie").value = parseFloat(document.getElementById("txtMarerialPriceManagementPrcie").value) *
            parseFloat(document.getElementById("txtCoefficientManagementPrcie0").value) + parseFloat(document.getElementById("txtTimeChargeManagementPrcie").value) *
            parseFloat(document.getElementById("txtCoefficientManagementPrcie1").value);
            document.getElementById("txtManagementPrcieUnitPrice").value = document.getElementById("txtManagementPrcie").value;
            //单价
            document.getElementById("txtUnitPrice").value = parseFloat(document.getElementById("txtMarerialPriceUnitPrice").value) +
            parseFloat(document.getElementById("txtTimeChargeUnitPrice").value) + parseFloat(document.getElementById("txtProfitUnitPrice").value) +
            parseFloat(document.getElementById("txtManagementPrcieUnitPrice").value) +
            parseFloat(document.getElementById("txtLossPrcieUnitPrice").value)
        }
        function changetTextCoefficientLossPrcie() {
            document.getElementById("txtLossPrcie").value = parseFloat(document.getElementById("txtMarerialPriceLossPrcie").value) *
            parseFloat(document.getElementById("txtCoefficientLossPrcie").value);
            document.getElementById("txtLossPrcieUnitPrice").value = document.getElementById("txtLossPrcie").value;
            //单价
            document.getElementById("txtUnitPrice").value = parseFloat(document.getElementById("txtMarerialPriceUnitPrice").value) +
            parseFloat(document.getElementById("txtTimeChargeUnitPrice").value) + parseFloat(document.getElementById("txtProfitUnitPrice").value) +
            parseFloat(document.getElementById("txtManagementPrcieUnitPrice").value) +
            parseFloat(document.getElementById("txtLossPrcieUnitPrice").value)
        }
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
        <tr id="trSN" runat="server">
            <td align="right">
                序号：
            </td>
            <td>
                <asp:TextBox ID="txtSN" runat="server" CssClass="input required" size="25" ReadOnly="true"></asp:TextBox>
                <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                阶层：
            </td>
            <td>
                <asp:Label ID="lbHierarchy" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                产成品编号：
            </td>
            <td>
                <asp:Label ID="lbProductNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户物料编码：
            </td>
            <td>
                <asp:Label ID="lbCustomerProductNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                描述：
            </td>
            <td>
                <asp:Label ID="lbDescription" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                BOM用量：
            </td>
            <td>
                <asp:Label ID="lbBOMAmount" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                原材料单价(未税)：
            </td>
            <td>
                <asp:Label ID="lbMaterialPrcie" runat="server" Text=""></asp:Label>
                <asp:TextBox ID="txtMaterialPrcie" runat="server" CssClass="input required number"
                    size="25" MaxLength="50" ReadOnly="true" onBlur="changetTextMaterialPrcie()"
                    onfocus="changetTextMaterialPrcie()"></asp:TextBox>
            </td>
        </tr>
        <tr id="trTimeCharge" runat="server">
            <td align="right">
                工时费(未税)：
            </td>
            <td>
                <asp:Label ID="lbTimeCharge" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr id="trProfit" runat="server">
            <td align="right">
                利润(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtProfit" runat="server" CssClass="input required" size="25" MaxLength="50"
                    onBlur="changetTextProfit()" Text="0.00" onfocus="changetTextProfit()"></asp:TextBox>
            </td>
        </tr>
        <tr id="trManagementPrcie" runat="server" runat="server">
            <td align="right">
                管销研费用(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtMarerialPriceManagementPrcie" runat="server" CssClass="input required number"
                    size="25" MaxLength="50" Width="57px" ReadOnly="true"></asp:TextBox>
                *<asp:TextBox ID="txtCoefficientManagementPrcie0" runat="server" CssClass="input required number"
                    size="25" MaxLength="50" Width="35px" Text="0.08" onBlur="changetTextCoefficientManagementPrcie0()">0.08</asp:TextBox>
                +<asp:TextBox ID="txtTimeChargeManagementPrcie" runat="server" CssClass="input required number"
                    size="25" MaxLength="50" Width="28px" ReadOnly="true"></asp:TextBox>
                *<asp:TextBox ID="txtCoefficientManagementPrcie1" runat="server" CssClass="input required number"
                    size="25" MaxLength="50" Width="29px" Text="0.1" onBlur="changetTextCoefficientManagementPrcie1()"></asp:TextBox>
                =<asp:TextBox ID="txtManagementPrcie" runat="server" CssClass="input required" size="25"
                    MaxLength="50" Width="58px" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr id="trLossPrcie" runat="server">
            <td align="right">
                损耗(未税)：
            </td>
            <td>
                <asp:TextBox ID="txtMarerialPriceLossPrcie" runat="server" CssClass="input required"
                    size="25" MaxLength="50" Width="59px" ReadOnly="true"></asp:TextBox>
                *<asp:TextBox ID="txtCoefficientLossPrcie" runat="server" CssClass="input required"
                    size="25" MaxLength="50" Width="39px" Text="0.02" onBlur="changetTextCoefficientLossPrcie()"></asp:TextBox>
                =<asp:TextBox ID="txtLossPrcie" runat="server" CssClass="input required" size="25"
                    MaxLength="50" Width="51px" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                单价(未税)：
            </td>
            <td>
                <span id="sUnitPriceOne" runat="server">
                    <asp:TextBox ID="txtMarerialPriceUnitPrice" runat="server" CssClass="input" size="25"
                        MaxLength="50" minlength="1" Width="55px" ReadOnly="true"></asp:TextBox>
                    +<asp:TextBox ID="txtTimeChargeUnitPrice" runat="server" CssClass="input" size="25"
                        MaxLength="50" minlength="1" Width="35px" ReadOnly="true"></asp:TextBox>
                    +<asp:TextBox ID="txtProfitUnitPrice" runat="server" CssClass="input" size="25" MaxLength="50"
                        minlength="1" Width="35px" ReadOnly="true"></asp:TextBox>
                    +<asp:TextBox ID="txtManagementPrcieUnitPrice" runat="server" CssClass="input" size="25"
                        MaxLength="50" minlength="1" Width="35px" ReadOnly="true"></asp:TextBox>
                    +<asp:TextBox ID="txtLossPrcieUnitPrice" runat="server" CssClass="input" size="25"
                        MaxLength="50" minlength="1" Width="35px" ReadOnly="true"></asp:TextBox>
                    =<asp:TextBox ID="txtUnitPrice" runat="server" CssClass="input" size="25" MaxLength="50"
                        minlength="1" Width="52px" ReadOnly="true"></asp:TextBox>
                </span>
                <br />
                <span id="sUnitPriceTwo" runat="server">
                    <asp:TextBox ID="txtMarerialOnePrice" runat="server" Width="54px" CssClass="input"
                        ReadOnly="true" Height="18px"></asp:TextBox>*
                    <asp:TextBox ID="txtBOMAmountUnitPrice" runat="server" Width="50px" CssClass="input"
                        ReadOnly="true"></asp:TextBox>=
                    <asp:TextBox ID="txtOnePrice" runat="server" Width="61px" CssClass="input" ReadOnly="true"></asp:TextBox>
                </span>
            </td>
        </tr>
        <tr>
            <td align="right">
                固定提前期：
            </td>
            <td>
                <asp:TextBox ID="txtFixedLeadTime" runat="server" CssClass="input required" size="25"
                    MaxLength="50"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" MaxLength="200" TextMode="MultiLine" CssClass="input"
                    size="25" Height="40px" Width="200px"></asp:TextBox>
                <asp:Label ID="lbRemark" runat="server" Text="(限制输入200字)"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="修改" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;<asp:Label ID="Label13" runat="server" Text="（*号为必填项）" ForeColor="Red"></asp:Label>
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
