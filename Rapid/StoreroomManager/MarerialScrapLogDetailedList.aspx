<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarerialScrapLogDetailedList.aspx.cs" Inherits="Rapid.StoreroomManager.MarerialScrapLogDetailed" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>原材料报废上报详情页</title>
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <script src="../Js/jquery-1.10.2.min.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="msgtable">
        <tr>
            <th colspan="2" align="left">
                详细信息
            </th>
        </tr>
     <tr>
            <td width="25%" align="right">
               编号：
            </td>
            <td>
                <label id="lbId" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
               创建时间：
            </td>
            <td>
                <label id="lbCreateTime" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
               产成品编号：
            </td>
            <td>
                <label id="lbProductNumber" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
               原材料编号：
            </td>
            <td>
                <label id="lbMaterialNumber" runat="server">
                </label>
            </td>
        </tr>
         <tr>
            <td width="25%" align="right">
               报废日期：
            </td>
            <td>
                <label id="lbScrapDate" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
               班组：
            </td>
            <td>
                <label id="lbTeam" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
              数量：
            </td>
            <td>
                <label id="lbCount" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
              责任人：
            </td>
            <td>
                <label id="lbResponsiblePerson" runat="server">
                </label>
            </td>
        </tr>
        <tr>
            <td width="25%" align="right">
              报废原因：
            </td>
            <td>
                <label id="lbScrapReason" runat="server">
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

        window.location.href = "MarerialScrapLogList.aspx";
        //window.close();
    }

</script>