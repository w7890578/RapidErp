<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProductPlanSubDetail.aspx.cs"
    Inherits="Rapid.ProduceManager.EditProductPlanSubDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

  
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>开工单分表明细</title>
    <link href="../Css/Main.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <base target="_self" />
    <link href="../Css/Verification/style.css" rel="stylesheet" type="text/css" />

    <script src="../Js/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../Js/jquery.validate.min.js" type="text/javascript"></script>

    <script src="../Js/messages_cn.js" type="text/javascript"></script>

    <script src="../Js/Main.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            color: #FF0000;
        }
    </style>

    <script type="text/javascript">
        $(function() {
            $("#btnBack").click(function() {
                window.close();
            });
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
                开工单号：
            </td>
            <td>
                <asp:Label ID="lblPlanNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                班组
            </td>
            <td>
                <asp:Label ID="lblTeam" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                销售订单号
            </td>
            <td>
                <asp:Label ID="lblOrdersNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                客户产成品编号
            </td>
            <td>
                <asp:Label ID="lblCustomerProductNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                产成品编号：
            </td>
            <td>
                <asp:Label ID="lblProductNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                版本：
            </td>
            <td>
                <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                行号：
            </td>
            <td>
                <asp:Label ID="lblRowNumber" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                完成数量：
            </td>
            <td>
               <span style="display:none;">
                   <asp:TextBox runat="server" ID="txtOldQty" ></asp:TextBox>
                   <asp:TextBox runat="server" ID="txtOldFQty" ></asp:TextBox></span> 
                已完成+<asp:TextBox ID="txtFinishQty" runat="server" class="input requied" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: none;">
            <td align="right">
                交线情况：
            </td>
            <td>
                <asp:TextBox ID="txtTakeLine" runat="server" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr style="display: <%=show%>;">
            <td align="right">
                交接班组：
            </td>
            <td>
                <asp:TextBox ID="txtNextTeam" runat="server" class="input requied" size="25"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                备注：
            </td>
            <td>
                <asp:TextBox ID="txtRemark" runat="server" Height="31px" Width="300px" MaxLength="200"
                    class="input requied"></asp:TextBox>
                <asp:Label ID="lblMemo" runat="server" Text="（限制输入200字）"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="right">
                是否为半成品：
            </td>
            <td>
                <asp:DropDownList ID="drpHalf" runat="server" OnSelectedIndexChanged="drpHalf_SelectedIndexChanged"
                    AutoPostBack="true">
                    <asp:ListItem Value="否" Text="否"></asp:ListItem>
                    <asp:ListItem Value="是" Text="是"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr id="trQL" runat="server" visible="false">
            <td align="right">
                缺料原材料编号：
            </td>
            <td>
                <asp:TextBox ID="txtQL" runat="server" Height="31px" Width="300px" TextMode="MultiLine"></asp:TextBox>
                <asp:Label ID="Label1" runat="server" Text="如果是半成品"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="修改" OnClick="btnSubmit_Click" CssClass="submit" />
                &nbsp;&nbsp;&nbsp;
                <input type="button" value="返回" class="submit" id="btnBack" />
                <%--  <asp:Label ID="lbMsg" runat="server" Style="color: Red;" Text=""></asp:Label>--%>
                <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="（*号为必填项）"></asp:Label>
                &nbsp; <span class="style1">&nbsp;完成数量&lt;=套数-已完成数量</span>
            </td>
        </tr>
    </table>
    
    </form>
</body>
</html>
