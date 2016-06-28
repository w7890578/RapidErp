<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarerialInfoTableDetailedList.aspx.cs"
    Inherits="Rapid.ProduceManager.MarerialInfoTableDetailedList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>原材料信息详细列表</title>
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

<%--    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>--%>

    <script src="../Js/Main.js" type="text/javascript"></script>
 

</head>
<body  >
    <form id="form1" runat="server">
     
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                详细信息
            </th>
        </tr>
        <tr>
            <td width="25%" align="right">
                原材料编号:
            </td>
            <td>
                <label id="lbMaterialNumber" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                名称：
            </td>
            <td>
                <label id="lbMaterialName" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                描述：
            </td>
            <td>
                <label id="lbDescription" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                种类:
            </td>
            <td>
                <label id="lbKind" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                类别:
            </td>
            <td>
                <label id="lbType" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                品牌：
            </td>
            <td>
                <label id="lbBrand" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                库存安全值：
            </td>
            <td>
                <label id="lbStockSafeQty" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                采购价格：
            </td>
            <td>
                <label id="lbProcurementPrice" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                原材料仓位：
            </td>
            <td>
                <label id="lbMaterialPosition" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                最小包装：
            </td>
            <td>
                <label id="lbMinPacking" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                最小起订量：
            </td>
            <td>
                <label id="lbMinOrderQty" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                废品仓位：
            </td>
            <td>
                <label id="lbScrapPosition" runat="server">
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
                <input type="button" id="btn" value=" 关 闭 " onclick="back()" class="submit" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script type="text/javascript">
    function back() {

        window.close(this);
    }

</script>

