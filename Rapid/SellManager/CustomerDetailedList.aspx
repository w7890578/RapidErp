<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerDetailedList.aspx.cs" Inherits="Rapid.ProduceManager.CustomerDetailedList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>客户列表详情页</title>
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

<%--    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>--%>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

</head>
<body >
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                详细信息
            </th>
        </tr>
     <tr>
            <td width="25%" align="right">
                客户编号：
            </td>
            <td>
                <label id="lbCustomerId" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                客户名称：
            </td>
            <td>
                <label id="lbCustomerName" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                注册地址：
            </td>
            <td>
                <label id="lbRegisteredAddress" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                法人代表：
            </td>
            <td>
                <label id="lbLegalPerson" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                联系人：
            </td>
            <td>
                <label id="lbContacts" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                联系电话：
            </td>
            <td>
                <label id="lbContactTelephone" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                备用电话：
            </td>
            <td>
                <label id="lbSparePhone" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                注册电话：
            </td>
            <td>
                <label id="lbRegisteredPhone" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                传真：
            </td>
            <td>
                <label id="lbFax" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                手机：
            </td>
            <td>
                <label id="lbMobilePhone" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                邮编：
            </td>
            <td>
                <label id="lbZipCode" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                Email:
            </td>
            <td>
                <label id="lbEmail" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                QQ:
            </td>
            <td>
                <label id="lbQQ" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                开户银行：
            </td>
            <td>
                <label id="lbAccountBank" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                银行行号：
            </td>
            <td>
                <label id="lbSortCode" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                银行账号：
            </td>
            <td>
                <label id="lbBankAccount" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                纳税号：
            </td>
            <td>
                <label id="lbTaxNo" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                网址：
            </td>
            <td>
                <label id="lbWebsiteAddress" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                送货地点：
            </td>
            <td>
                <label id="lbDeliveryAddress" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                备注：
            </td>
            <td>
                <label id="lbRemark" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
            </td>
            <td>
                <input type="button" id="btn" value="返回" onclick="back()" class="submit" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script type="text/javascript">
    function back() {

        window.location.href = "CustomerList.aspx";
        //window.close();
    }

</script>