<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCustomerDeliveryDetail.aspx.cs"
    Inherits="Rapid.SellManager.EditCustomerDeliveryDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>编辑客户提前要货明细表</title>
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
</head>
<body>
    <form id="form1" runat="server">
    <input type="hidden" id="hdQuoteNumber" runat="server" />
    <div style="padding-bottom: 10px;">
    </div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                基本信息填写&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbSubmit" runat="server" ForeColor="Red"></asp:Label>
            </th>
        </tr>
        <tr>
            <td align="right">
                导入时间：
            </td>
            <td>
                <asp:Label ID="lbImportTime" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户编号：
            </td>
            <td>
                <asp:Label ID="lbCustomerId" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                物料：
            </td>
            <td>
                <asp:Label ID="lbNumber" runat="server" Text=""></asp:Label>
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
                需提前交货数量：
            </td>
            <td>
                <asp:Label ID="lbAdvanceQty" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                供应商回复交货日期：
            </td>
            <td>
            <asp:TextBox ID="txtReplyDate" runat="server" CssClass="input required" onfocus="WdatePicker({skin:'green'})"></asp:TextBox>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                在制品数量：
            </td>
            <td>
                <asp:Label ID="lbWipQty" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                未交订单数量：
            </td>
            <td>
                <asp:Label ID="lbNonDeliveryQty" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                库存数量：
            </td>
            <td>
                <asp:Label ID="lbStockQty" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                是否满足交货：
            </td>
            <td>
                <asp:Label ID="lbIsMeetDelivery" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                订单对比结果：
            </td>
            <td>
                <asp:Label ID="lbOrderContrast" runat="server" Text=""></asp:Label>
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
                &nbsp;&nbsp;&nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
