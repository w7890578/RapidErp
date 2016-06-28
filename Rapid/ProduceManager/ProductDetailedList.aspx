<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductDetailedList.aspx.cs" Inherits="Rapid.ProduceManager.ProductDetailedList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>产成品信息详细列表</title>
     <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>
 
</head>
<body style="padding: 10px;">
    <form id="form1" runat="server">
    <div class="navigation">
        <b id="bText" runat="server">您当前的位置：产成品信息详细列表</b>
    </div>
    <div style="padding-bottom: 10px;">
    </div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                详细信息
            </th>
        </tr>
        <tr>
            <td width="25%" align="right">
               产成品编号:
            </td>
            <td>
                <label id="lbProductNumber" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
              版本:
            </td>
            <td>
                <label id="lbVersion" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
              名称:
            </td>
            <td>
                <label id="lbProductName" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                描述:
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
              额定工时:
            </td>
            <td>
                <label id="lbRatedManhour" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
               报价工时:
            </td>
            <td>
                <label id="lbQuoteManhour" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                成本价:
            </td>
            <td>
                <label id="lbCostPrice" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
              销售报价:
            </td>
            <td>
                <label id="lbSalesQuotation" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
                半成品仓位:
            </td>
            <td>
                <label id="lbHalfProductPosition" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
             产成品仓位:
            </td>
            <td>
                <label id="lbFinishedGoodsPosition" runat="server">
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
                <input type="button" id="btn" value="返回" onclick="back()" class="submit"/>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script type="text/javascript">
    function back() {

        window.close();
    }

</script>
